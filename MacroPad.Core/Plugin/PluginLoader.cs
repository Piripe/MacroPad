using MacroPad.Core.BasePlugin;
using MacroPad.Shared.Plugin;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace MacroPad.Core.Plugin
{
    public class PluginLoader
    {
        public readonly static HashSet<IPluginInfos> plugins = [];

        public readonly static HashSet<IProtocol> protocols = [];
        public readonly static HashSet<NodeType> nodeTypes = [.. DefaultTypes.types];
        public readonly static HashSet<INodeCategory> nodeCategories = [new BranchingCategory(), new ButtonCategory(), new ConditionsCategory(), new ConstantsCategory(), new DebugCategory(), new MathCategory(), new ProfileCategory(), new TextCategory(), new VariableCategory()];

        public static event EventHandler<IPluginInfos>? PluginAdded;
        public static event EventHandler<IPluginInfos>? PluginRemoved;

        public static void LoadPlugins()
        {
            if (!Directory.Exists("plugins")) return;
            IEnumerable<string> pluginDirs = Directory.EnumerateDirectories("plugins");

            foreach (var pluginDir in pluginDirs)
            {
                string pluginJsonFile = Path.Combine(pluginDir, "plugin.json");

                if (!File.Exists(pluginJsonFile)) continue;

                JsonObject? pluginJson = (JsonObject?)JsonNode.Parse(File.ReadAllText(pluginJsonFile));

                if (pluginJson == null || !pluginJson.ContainsKey("Main")) continue;

                string pluginFile = Path.Combine(pluginDir, pluginJson["Main"]?.ToString() ?? "");

                if (!File.Exists(pluginFile)) continue;

                McMaster.NETCore.Plugins.PluginLoader pluginLoader = McMaster.NETCore.Plugins.PluginLoader.CreateFromAssemblyFile(Path.GetFullPath(pluginFile));

                Assembly assembly;
                try
                {
                    assembly = pluginLoader.LoadDefaultAssembly();
                }
                catch
                {
                    throw new Exception("Failed to load plugin assembly");
                }


                Type? pluginInfosType = assembly.GetTypes().ToList().Find((type) => type.Name == "PluginInfos");

                if (pluginInfosType == null) continue;

                object? pluginsInfosInstance = assembly.CreateInstance(pluginInfosType.FullName ?? "");

                if (pluginsInfosInstance == null) continue;

                IPluginInfos? pluginInfos = pluginsInfosInstance as IPluginInfos;

                if (pluginInfos == null) continue;

                plugins.Add(pluginInfos);
                if (pluginInfos.Protocols != null) protocols.UnionWith(pluginInfos.Protocols);

                if (pluginInfos.NodeCategories != null) nodeCategories.UnionWith(pluginInfos.NodeCategories);
                if (pluginInfos.NodeTypes != null) nodeTypes.UnionWith(pluginInfos.NodeTypes);
                PluginAdded?.Invoke(null, pluginInfos);
            }

            //plugins.Sort((a,b)=>a.Name.CompareTo(b.Name));
            //nodeCategories.Sort((a,b)=>a.Name.CompareTo(b.Name));
        }
    }
}