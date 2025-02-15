
using MacroPad.Core;
using MacroPad.Core.Device;
using MacroPad.Shared.Plugin;
using MacroPad.Shared.Plugin.Protocol;
using System.Reflection;


DeviceManager.DeviceDetected += (sender, e) =>
{
    e.Device.DeviceInput += (sender, e) =>
    {
        if (sender is IProtocolDevice device)
            Console.WriteLine($"[{device.Name}] Input value {MathF.Round(e.Value,2)} in button {e.ButtonID}");
    };
};

DeviceManager.Init();

bool running = true;
while (running) {
    string[]? command = Console.ReadLine()?.Split('/');

    int color, button,x,y;

    if (command != null )
    {
        switch (command[0]) {
            case "stop":
                running = false;
                break;
            case "connect":
                if (command.Length <= 1) break;
                DeviceManager.EnableDevice(command[1]);
                break;
            case "disconnect":
                if (command.Length <= 1) break;
                DeviceManager.DisableDevice(command[1]);
                break;
            case "set":
                switch (command.Length)
                {
                    case 4:
                        if (int.TryParse(command[2], out button) && int.TryParse(command[3], out color))
                            DeviceManager.ConnectedDevices.Find(device=>device.ProtocolDevice.Id == command[1])?.ProtocolDevice.SetButtonPalette(button, color);
                        break;
                    case 5:
                        DeviceCore? deviceCore = DeviceManager.ConnectedDevices.Find(device => device.ProtocolDevice.Id == command[1]);
                        if (deviceCore != null && int.TryParse(command[2], out x) && int.TryParse(command[3], out y) && int.TryParse(command[4], out color))
                        {
                            DeviceLayoutButton? layoutButton = deviceCore.CordsToButton(x, y);
                            if (layoutButton != null)
                            {
                                deviceCore.SetButtonContent(layoutButton, color);
                            }
                        }

                        break;
                }
                break;
            case "protocols":
                Console.WriteLine($"Protocols:\n - {string.Join("\n - ",PluginLoader.protocols.Select((x)=>$"{x.GetType().FullName} [{(DeviceManager.Config.PluginsConfig[x.GetType().FullName ?? ""] ? "Enabled" : "Disabled")}]"))}");
                break;
            case "enable":
                if (command.Length <= 2) break;
                switch(command[1])
                {
                    case "protocol":
                        DeviceManager.EnableProtocol(command[2]);
                        break;
                }
                break;
            case "save":
                DeviceManager.Config.SaveConfig();
                break;
        }
    }
}