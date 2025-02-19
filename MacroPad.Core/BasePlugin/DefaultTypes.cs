using MacroPad.Shared.Plugin;
using MacroPad.Shared.Plugin.Nodes;
using MacroPad.Shared.Plugin.Components;

namespace MacroPad.Core.BasePlugin
{
    internal class DefaultTypes
    {
        public static List<NodeType> types = [
            new NodeType(
                "Number",
                new Shared.Media.Color(250, 20, 20),
                typeof(decimal),
                0m,
                x => decimal.TryParse(x.ToString(),out decimal y) ? y : null,
                (r)=>r.GetData<decimal>("d"),
                [
                    new NumericUpDown() {
                        GetValue=(r)=>r.GetData<decimal>("d"),
                        ValueChanged=(r,v)=>r.SetData("d",v),
                        Min = decimal.MinValue,
                        Max = decimal.MaxValue,
                    }
                ]),
            new NodeType(
                "Text",
                new Shared.Media.Color(20, 250, 20),
                typeof(string),
                "",
                x => x.ToString(),
                (r)=>r.GetData<string>("d"),
                [
                    new TextBox() {
                        GetText=(r)=>r.GetData<string>("d")??"",
                        TextChanged=(r,v)=>r.SetData("d",v)
                    }
                ]),
            new NodeType(
                "Boolean", 
                new Shared.Media.Color(51, 51, 255), 
                typeof(bool), 
                false,
                x => typeof(decimal).IsAssignableFrom(x.GetType()) ? (decimal)x == 1 ? true : false : bool.TryParse(x.ToString(),out bool y) ? y : null, 
                (r)=>r.GetData<bool?>("d") ?? false,
                [
                    new ComboBox() {
                        Items = ["False","True"],
                        GetSelection = (IResourceManager resource) => resource.GetData<int>("d"),
                        SelectionChanged = (IResourceManager resource, int value) => resource.SetData("d", value),
                    }
                ]),
            new NodeType(
                "Object",
                new Shared.Media.Color(170, 170, 170),
                typeof(object),
                "",
                load:(r)=>r.GetData<object>("d"),
                components: []
                ),
        ];
    }
}
