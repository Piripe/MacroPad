using MacroPad.Shared.Device;
using MacroPad.Shared.Plugin.Nodes;
using MacroPad.Shared.Plugin.Components;

namespace MacroPad.Core.BasePlugin.Math
{
    public class Operation : INodeGetter
    {
        public string Name => "Operation";

        public string Description => "Performs an operation.";

        public string Id => "Operation";

        public TypeNamePair[] Inputs => [new(typeof(decimal), ""), new(typeof(decimal), "")];

        public TypeNamePair[] Outputs => [new(typeof(decimal), "")];

        public INodeComponent[] Components => [
            new ComboBox() {Items = [
                "Add",
                "Substract",
                "Multiply",
                "Divide",
                "Modulo",
                "Power",
                "Root"
            ],
            GetSelection = (IResourceManager resource) => resource.GetData<int>("o"),
            SelectionChanged = (IResourceManager resource, int index) => resource.SetData("o",index)}
        ];

        private readonly Func<decimal, decimal, decimal>[] _operations = [
            (x,y)=>x+y,
            (x,y)=>x-y,
            (x,y)=>x*y,
            (x,y)=>x/y,
            (x,y)=>x%y,
            (x,y)=>(decimal)System.Math.Pow(decimal.ToDouble(x),decimal.ToDouble(y)),
            (x,y)=>(decimal)System.Math.Pow(decimal.ToDouble(x),1/decimal.ToDouble(y)),
        ];

        public bool IsVisible(IDeviceLayoutButton button, IDeviceOutput output) => true;
        public object[] GetOutputs(IResourceManager resource)
        {
            return [_operations[resource.GetData<int>("o")]((decimal)resource.GetValue(0), (decimal)resource.GetValue(1))];
        }
    }
}
