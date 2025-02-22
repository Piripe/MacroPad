namespace MacroPad.Shared.Plugin
{
    public interface IResourceManager
    {
        public T? GetData<T>(string key);
        public void SetData(string key, object value);
        public T? GetVirtual<T>(VirtualDataKey key);
    }
}
