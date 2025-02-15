using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.Controls.Home.NodePicker
{
    public class NodeSelectedEventArgs
    {
        public string Id {  get; protected set; }

        public NodeSelectedEventArgs(string id)
        {
            Id = id;
        }
    }
}
