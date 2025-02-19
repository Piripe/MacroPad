using MacroPad.Shared.Device;
using MacroPad.Shared.Plugin.Nodes;
using MacroPad.Shared.Plugin.Components;

namespace MacroPad.Core.BasePlugin.Conditions
{
    public class Condition : INodeGetter
    {
        public string Name => "Condition";

        public string Description => "Performs a condition.";

        public string Id => "Condition";

        public TypeNamePair[] Inputs => [new(typeof(object), ""), new(typeof(object), "")];

        public TypeNamePair[] Outputs => [new(typeof(bool), "")];

        public INodeComponent[] Components => [
            new ComboBox() {Items = [
                "Equals",
                "Not Equals",
                "Greater",
                "Greater or Equals",
                "Lower",
                "Lower or Equals"
            ],
            GetSelection = (IResourceManager resource) => resource.GetData<int>("o"),
            SelectionChanged = (IResourceManager resource, int index) => resource.SetData("o",index)}
        ];

        private Func<object, object, bool>[] _operations = [
            (x,y)=>x==y,
            (x,y)=>x!=y,
            (x,y)=>ObjectToDecimal(x)>ObjectToDecimal(y),
            (x,y)=>ObjectToDecimal(x)>=ObjectToDecimal(y),
            (x,y)=>ObjectToDecimal(x)<ObjectToDecimal(y),
            (x,y)=>ObjectToDecimal(x)<=ObjectToDecimal(y),
        ];

        private static decimal ObjectToDecimal(object x) => (decimal.TryParse(x.ToString(), out decimal val) ? val : 0m);

        public bool IsVisible(IDeviceLayoutButton button, IDeviceOutput output) => true;
        public object[] GetOutputs(IResourceManager resource)
        {
            return [ _operations[resource.GetData<int>("o")](resource.GetValue(0), resource.GetValue(1)) ];
        }
    }
}
