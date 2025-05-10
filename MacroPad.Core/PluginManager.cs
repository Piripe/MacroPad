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

        public static event EventHandler<IPluginInfos>? PluginLoaded;
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

            PluginLoaded?.Invoke(null, BasePlugin);
            PluginEnabled?.Invoke(null, BasePlugin);

            foreach (var plugin in Plugins)
            {
                if (DeviceManager.Config.LoadedPlugins.Contains(plugin.PluginId!))
                {
                    if (DeviceManager.Config.EnabledPlugins.Contains(plugin.PluginId!)) plugin.Enable();
                    else plugin.Load();
                }
            }
        }

        public static void EnablePlugin(PluginInfos plugin)
        {
            DeviceManager.Config.EnabledPlugins.Add(plugin.PluginId!);
            DeviceManager.Config.LoadedPlugins.Add(plugin.PluginId!);
            plugin.Enable();
        }
        public static void DisablePlugin(PluginInfos plugin)
        {
            DeviceManager.Config.EnabledPlugins.Remove(plugin.PluginId!);
            if (plugin.IsUnloadable) DeviceManager.Config.LoadedPlugins.Remove(plugin.PluginId!);
            plugin.Disable();
        }
        public static void OnPluginLoaded(IPluginInfos plugin)
        {
            PluginLoaded?.Invoke(null, plugin);
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
