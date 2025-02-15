using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.Shared.Plugin.Nodes
{
    public struct NodeRunnerResult
    {
        public int RunnerOutputIndex { get; set; }
        public object[] Results { get; set; }

    }
}
