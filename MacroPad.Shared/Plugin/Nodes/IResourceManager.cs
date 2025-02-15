using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.Shared.Plugin.Nodes
{
    public interface IResourceManager
    {
        public object GetValue(int index);
        public T? GetData<T>(string key);
        public void SetData(string key, object value);
    }
}
