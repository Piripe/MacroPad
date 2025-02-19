using MacroPad.Core.Config;
using MacroPad.Core.Device;
using MacroPad.Core.Node;
using MacroPad.Shared.Plugin;
using MacroPad.Shared.Plugin.Nodes;
using Newtonsoft.Json.Linq;

namespace MacroPad.Core
{
    public static class NodeManager
    {
        const int NODE_EXECUTION_LIMIT = 2048;

        public static Dictionary<Type, NodeType> Types = [];
        public readonly static Dictionary<string, INodeRunner> Runners = [];
        public readonly static Dictionary<string, INodeGetter> Getters = [];
        public static DeviceCore? CurrentDevice { get; private set; }
        public static DeviceLayoutButton? CurrentButton { get; private set; }

        public static void Init()
        {
            foreach (NodeType type in PluginLoader.nodeTypes) {
                Types.Add(type.Type, type);
            }

            foreach (INodeCategory category in PluginLoader.nodeCategories)
            {
                foreach (INodeRunner node in category.Runners)
                {
                    Runners.Add(category.Id + "." + node.Id, node);
                }
                foreach (INodeGetter node in category.Getters)
                {
                    Getters.Add(category.Id + "." + node.Id, node);
                }
            }
        }

        public static void Run(NodeScript script, DeviceCore device, DeviceLayoutButton button)
        {
            CurrentDevice = device;
            CurrentButton = button;
            Dictionary<int, object[]> cache = [];
            int nodeExecutionLimit = NODE_EXECUTION_LIMIT;

            void RunLine(int lineId)
            {
                if (!script.NodeLines.TryGetValue(lineId, out NodeLine? value)) return;

                int nodeLinkId = value.Node;

                RunLinks(nodeLinkId);
            }

            void RunLinks(int linksId)
            {
                if (nodeExecutionLimit > 0)
                {
                    nodeExecutionLimit--;
                    if (!script.NodesLinks.TryGetValue(linksId, out NodeLinks? links)) return;
                    if (!Runners.TryGetValue(links.Id, out INodeRunner? nodeRunner)) return;
                    object GetValue(int index)
                    {
                        NodeType type = Types[nodeRunner.Inputs[index].Type];
                        object value = type.DefaultValue;
                        if (links.Getters.TryGetValue(index, out int value2)) value = GetLine(value2) ?? type.DefaultValue;
                        else return GetConst(links.Consts, type, index) ?? type.DefaultValue;
                        if (value.GetType().IsAssignableFrom(typeof(JValue))) value = ((JValue)value).Value ?? type.DefaultValue;
                        if (type.Type.IsAssignableFrom(value.GetType())) return value;
                        if (type.TypeConverter != null) return type.TypeConverter(value) ?? type.DefaultValue;
                        return type.DefaultValue;
                    }

                    NodeRunnerResult result = nodeRunner.Run(new NodeResourceManager(GetValue, links.Data));

                    if (cache.ContainsKey(linksId)) cache[linksId] = result.Results;
                    else cache.Add(linksId, result.Results);

                    if (links.Runners.TryGetValue(result.RunnerOutputIndex, out int value)) RunLine(value);
                }
            }

            object? GetLine(int lineId)
            {
                if (!script.NodeLines.TryGetValue(lineId, out NodeLine? nodeLine)) return null;
                return GetLinks(nodeLine.Node,nodeLine.PointIndex);
            }

            object? GetLinks(int linksId, int index)
            {
                if (nodeExecutionLimit > 0)
                {
                    nodeExecutionLimit--;
                    if (cache.ContainsKey(linksId))
                    {
                        if (index < cache[linksId].Length) return cache[linksId][index];
                    }
                    if (!script.NodesLinks.TryGetValue(linksId, out NodeLinks? links)) return null;
                    if (!Getters.TryGetValue(links.Id, out INodeGetter? nodeGetter)) return null;
                    object GetValue(int index)
                    {
                        NodeType type = Types[nodeGetter.Inputs[index].Type];
                        object value = type.DefaultValue;
                        if (links.Getters.TryGetValue(index, out int value2)) value = GetLine(value2) ?? type.DefaultValue;
                        else return GetConst(links.Consts, type, index) ?? type.DefaultValue;
                        if (value.GetType().IsAssignableFrom(typeof(JValue))) value = ((JValue)value).Value ?? type.DefaultValue;
                        if (type.Type.IsAssignableFrom(value.GetType())) return value;
                        if (type.TypeConverter != null) return type.TypeConverter(value) ?? type.DefaultValue;
                        return type.DefaultValue;
                    }

                    object[] result = nodeGetter.GetOutputs(new NodeResourceManager(GetValue, links.Data));

                    if (!cache.TryAdd(linksId, result)) cache[linksId] = result;
                    if (index < result.Length) return result[index];
                }
                return null;
            }

            object? GetConst(Dictionary<int,Dictionary<string, JToken>> consts, NodeType type, int index)
            {
                if (!consts.TryGetValue(index, out Dictionary<string, JToken>? data)) return null;
                return type.Load != null ? type.Load(new NodeResourceManager((i) => throw new Exception("Can't get value of a type"), data)) : null;
            }


            RunLine(-1);


        }
    }
}
