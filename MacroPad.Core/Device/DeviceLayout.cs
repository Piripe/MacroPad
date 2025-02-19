using MacroPad.Shared.Plugin.Protocol;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace MacroPad.Core.Device
{
    public class DeviceLayout
    {
        [JsonProperty("detectionMode")]
        public DeviceDetectionMode DetectionMode { get; set; } = DeviceDetectionMode.Name | DeviceDetectionMode.Equal;
        [JsonProperty("detectionValue")]
        public string DetectionValue { get; set; } = "";
        [JsonProperty("protocol")]
        public string Protocol { get; set; } = "";
        [JsonProperty("name")]
        public string Name { get; set; } = "";
        [JsonProperty("outputs")]
        public Dictionary<string, DeviceOutput> OutputTypes { get; set; } = [];
        [JsonProperty("buttons")]
        public DeviceLayoutButton[] Buttons { get; set; } = [];
        [JsonProperty("assets")]
        public string AssetsFolder { get; set; } = "";
        [JsonProperty("width")]
        public int DWidth { get; set; }
        [JsonProperty("height")]
        public int DHeight { get; set; }
        [JsonProperty("image")]
        public string? DImage { get; set; }
        private string _layoutPath = "";

        public void SetLayoutPath(string path) => _layoutPath = path;
        public string GetAssetPath(string path)
        {
            return Path.GetFullPath(Path.Combine(_layoutPath,AssetsFolder, path));
        }


        public static List<DeviceLayout> LoadLayouts() {
            var layouts = new List<DeviceLayout>();

            if (Directory.Exists("layouts"))
            {
                IEnumerable<string> layoutFiles = Directory.EnumerateFiles("layouts", "*.layout.json", SearchOption.AllDirectories);
                foreach (string layoutFile in layoutFiles)
                {
                    var layout = JsonConvert.DeserializeObject<DeviceLayout>(File.ReadAllText(layoutFile));
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

            return DeviceManager.Layouts.Find((layout) =>
            {
                if (layout.Protocol != device.Protocol) return false;
                string input = layout.DetectionMode.HasFlag(DeviceDetectionMode.Name) ? device.Name : device.Id;

                if (layout.DetectionMode.HasFlag(DeviceDetectionMode.Equal)) return input == layout.DetectionValue;
                if (layout.DetectionMode.HasFlag(DeviceDetectionMode.Contains)) return input.Contains(layout.DetectionValue);
                if (layout.DetectionMode.HasFlag(DeviceDetectionMode.Regex)) return Regex.IsMatch(input, layout.DetectionValue);

                return false;
            });
        }
    }
}
