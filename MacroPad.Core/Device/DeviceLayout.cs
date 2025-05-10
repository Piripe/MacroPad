using MacroPad.Shared.Plugin.Protocol;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace MacroPad.Core.Device
{
    public class DeviceLayout
    {
        [JsonPropertyName("detectionMode")]
        public DeviceDetectionMode DetectionMode { get; set; } = DeviceDetectionMode.Name | DeviceDetectionMode.Equal;
        [JsonPropertyName("detectionValue")]
        public string DetectionValue { get; set; } = "";
        [JsonPropertyName("protocol")]
        public string Protocol { get; set; } = "";
        [JsonPropertyName("name")]
        public string Name { get; set; } = "";
        [JsonPropertyName("outputs")]
        public Dictionary<string, DeviceOutput> OutputTypes { get; set; } = [];
        [JsonPropertyName("buttons")]
        public DeviceLayoutButton[] Buttons { get; set; } = [];
        [JsonPropertyName("assets")]
        public string AssetsFolder { get; set; } = "";
        [JsonPropertyName("width")]
        public int DWidth { get; set; }
        [JsonPropertyName("height")]
        public int DHeight { get; set; }
        [JsonPropertyName("image")]
        public string? DImage { get; set; }
        private string _layoutPath = "";

        public void SetLayoutPath(string path) => _layoutPath = path;
        public string GetAssetPath(string path)
        {
            return Path.GetFullPath(Path.Combine(_layoutPath,AssetsFolder, path));
        }


        public static HashSet<DeviceLayout> LoadLayouts() {
            var layouts = new HashSet<DeviceLayout>();

            if (Directory.Exists("layouts"))
            {
                IEnumerable<string> layoutFiles = Directory.EnumerateFiles("layouts", "*.layout.json", SearchOption.AllDirectories);
                foreach (string layoutFile in layoutFiles)
                {
                    var layout = JsonSerializer.Deserialize<DeviceLayout>(File.ReadAllText(layoutFile));
                    if (layout != null)
                    {
                        layout.SetLayoutPath(Path.GetDirectoryName(layoutFile)??"");
                        layouts.Add(layout);
                    }
                }
            }

            return layouts;
        }

        public static DeviceLayout? SearchLayout(IProtocolDevice device) {

            return DeviceManager.Layouts.FirstOrDefault((layout) =>
            {
                if (layout.Protocol != device.Protocol) return false;
                string input = layout.DetectionMode.HasFlag(DeviceDetectionMode.ID) ? device.Id : device.Name;

                if (layout.DetectionMode.HasFlag(DeviceDetectionMode.Equal)) return input == layout.DetectionValue;
                if (layout.DetectionMode.HasFlag(DeviceDetectionMode.Contains)) return input.Contains(layout.DetectionValue);
                if (layout.DetectionMode.HasFlag(DeviceDetectionMode.Regex)) return Regex.IsMatch(input, layout.DetectionValue);

                return false;
            });
        }
    }
}
