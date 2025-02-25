using MacroPad.Core.BasePlugin;
using MacroPad.Core.Models.Plugin;
using MacroPad.Shared.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MacroPad.Core
{
    public static class PluginManager
    {
        public readonly static HashSet<IProtocol> Protocols = [];

        public readonly static HashSet<PluginInfos> Plugins = [];
        public readonly static IPluginInfos BasePlugin = new BasePluginInfos();

        public static event EventHandler<IPluginInfos>? PluginEnabled;
        public static event EventHandler<IPluginInfos>? PluginDisabled;
        public static event EventHandler<PluginInfos>? PluginAdded;
        public static event EventHandler<PluginInfos>? PluginRemoved;

        public static void ScanPlugins()
        {
            if (!Directory.Exists("plugins")) Directory.CreateDirectory("plugins");

            JsonSerializerOptions options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());

            Plugins.UnionWith(Directory.EnumerateDirectories("plugins").Where(directory=>!Plugins.Any(x=>x.PluginDirectory==directory)).Select((directory) =>
            {
                string pluginJsonPath = Path.Combine(directory, "plugin.json");
                if (!File.Exists(pluginJsonPath)) return null;

                var file = File.OpenRead(pluginJsonPath);
                PluginInfos? pluginInfos = JsonSerializer.Deserialize<PluginInfos>(file, options);
                file.Close();

                if (pluginInfos == null) return null;
                pluginInfos.PluginDirectory = directory;
                pluginInfos.PluginId = Path.GetFileName(directory);
                PluginAdded?.Invoke(null, pluginInfos);
                return pluginInfos;
            }).Where(x=>x!=null).Select(x=>x!).ToHashSet());

            PluginEnabled?.Invoke(null, BasePlugin);

            foreach (var plugin in Plugins)
            {
                // Ignore if plugin is disabled or not present in the config
                if (!DeviceManager.Config.EnabledPlugins.TryGetValue(plugin.PluginId!, out bool value) || !value) continue;

                plugin.Load();
            }
        }

        public static void EnablePlugin(PluginInfos plugin)
        {
            if (!DeviceManager.Config.EnabledPlugins.TryAdd(plugin.PluginId!, true)) DeviceManager.Config.EnabledPlugins[plugin.PluginId!] = true;
            plugin.Load();
        }
        public static void DisablePlugin(PluginInfos plugin)
        {
            if (!DeviceManager.Config.EnabledPlugins.TryAdd(plugin.PluginId!, false)) DeviceManager.Config.EnabledPlugins[plugin.PluginId!] = false;
            plugin.Unload();
        }
        public static void OnPluginEnabled(IPluginInfos plugin)
        {
            PluginEnabled?.Invoke(null, plugin);
        }
        public static void OnPluginDisabled(IPluginInfos plugin)
        {
            PluginDisabled?.Invoke(null, plugin);
        }
    }
}
