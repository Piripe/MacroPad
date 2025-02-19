namespace MacroPad.Shared.Plugin.Nodes
{
    public interface IResourceManager
    {
        public object GetValue(int index);
        public T? GetData<T>(string key);
        public void SetData(string key, object value);
    }
}
