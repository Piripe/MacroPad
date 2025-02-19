namespace MacroPad.Shared.Plugin.Nodes
{
    public struct TypeNamePair(Type type, string name)
    {
        public Type Type { get; set; } = type;
        public string Name { get; set; } = name;
    }
}
