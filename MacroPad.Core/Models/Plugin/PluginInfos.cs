using MacroPad.Shared.Plugin;
using McMaster.NETCore.Plugins;
using Microsoft.DotNet.PlatformAbstractions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MacroPad.Core.Models.Plugin
{
    public class PluginInfos
    {
        [JsonPropertyName("filename")]
        public string? Filename { get; set; }
        [JsonIgnore]
        public string? PluginDirectory { get; set; }
        [JsonIgnore]
        public string? PluginId { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; } = "Another plugin";
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        [JsonPropertyName("version")]
        public string Version { get; set; } = "0.0.1";
        [JsonPropertyName("author")]
        public string? Author { get; set; }
        [JsonPropertyName("authorUrl")]
        public string? AuthorUrl { get; set; }
        [JsonPropertyName("sourceUrl")]
        public string? SourceUrl { get; set; }
        [JsonPropertyName("iconType")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PluginIconType IconType { get; set; } = PluginIconType.Symbol;
        [JsonPropertyName("icon")]
        public string Icon { get; set; } = "PlugDisconnected";
        [JsonPropertyName("supportedPlatforms")]
        public Platform[] SupportedPlatforms { get; set; } = [Platform.Windows];
        [JsonIgnore]
        public bool Enabled => DeviceManager.Config.EnabledPlugins.TryGetValue(PluginId!, out bool value) && value;

        private PluginLoader? _pluginLoader;
        [JsonIgnore]
        private IPluginInfos? _pluginInfos;
        public void Load()
        {
            if (PluginDirectory == null || Filename == null) throw new NoNullAllowedException("Plugin directory or filename are null");
            string pluginFile = Path.Combine(PluginDirectory, Filename);

            if (!File.Exists(pluginFile)) throw new FileNotFoundException("Plugin file not found");

            _pluginLoader = PluginLoader.CreateFromAssemblyFile(Path.GetFullPath(pluginFile));

            Assembly assembly;

            assembly = _pluginLoader.LoadDefaultAssembly();

            Type? pluginInfosType = assembly.GetTypes().ToList().Find((type) => type.Name == "PluginInfos") ?? 
                throw new TypeAccessException("Can't find PluginInfos type");

            object? pluginsInfosInstance = assembly.CreateInstance(pluginInfosType.FullName ?? "") ??
                throw new TypeAccessException($"Cant create instance of \"{pluginInfosType.FullName}\"");

            if (pluginsInfosInstance is not IPluginInfos pluginInfos) throw new InvalidCastException($"Can't cast \"{pluginInfosType.FullName}\" to IPluginInfos");
            _pluginInfos = pluginInfos;

            PluginManager.OnPluginEnabled(_pluginInfos);

            PluginManager.Protocols.UnionWith(_pluginInfos.Protocols);
        }
        [JsonIgnore]
        public bool IsLoaded => _pluginLoader == null ? false : true;
        [JsonIgnore]
        public bool IsUnloadable => _pluginLoader?.IsUnloadable ?? false;
        public void Unload()
        {
            if (_pluginInfos == null) return;

            PluginManager.OnPluginDisabled(_pluginInfos);

            PluginManager.Protocols.ExceptWith(_pluginInfos.Protocols);
        }
    }
}
