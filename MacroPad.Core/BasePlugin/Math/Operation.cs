using MacroPad.Shared.Device;
using MacroPad.Shared.Plugin.Nodes;
using MacroPad.Shared.Plugin.Nodes.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.Core.BasePlugin.Math
{
    public class Operation : INodeGetter
    {
        public string Name => "Operation";

        public string Description => "Performs an operation.";

        public string Id => "Operation";

        public TypeNamePair[] Inputs => new TypeNamePair[] { new TypeNamePair(typeof(decimal), ""), new TypeNamePair(typeof(decimal), "") };

        public TypeNamePair[] Outputs => new TypeNamePair[] { new TypeNamePair(typeof(decimal), "") };

        public INodeComponent[] Components => new INodeComponent[] {
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
        };

        private Func<decimal, decimal, decimal>[] _operations = new Func<decimal, decimal, decimal>[] {
            (x,y)=>x+y,
            (x,y)=>x-y,
            (x,y)=>x*y,
            (x,y)=>x/y,
            (x,y)=>x%y,
            (x,y)=>(decimal)System.Math.Pow(decimal.ToDouble(x),decimal.ToDouble(y)),
            (x,y)=>(decimal)System.Math.Pow(decimal.ToDouble(x),1/decimal.ToDouble(y)),
        };

        public bool IsVisible(IDeviceLayoutButton button, IDeviceOutput output) => true;
        public object[] GetOutputs(IResourceManager resource)
        {
            return new object[] { _operations[resource.GetData<int>("o")]((decimal)resource.GetValue(0), (decimal)resource.GetValue(1)) };
        }
    }
}
