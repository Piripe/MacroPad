using MacroPad.Core.Config;
using MacroPad.Shared.Plugin.Protocol;
using Newtonsoft.Json.Linq;
using MacroPad.Shared.Device;

namespace MacroPad.Core.Device
{
    public class DeviceCore
    {
        public IProtocolDevice ProtocolDevice { get; }
        public DeviceLayout? Layout { get; set; }
        public List<DeviceProfile> DeviceProfiles { get; }
        public int CurrentProfileIndex { get; set; }
        public int DefaultProfile { get => DeviceManager.Config.DefaultProfile.TryGetValue(ProtocolDevice.Id, out int value) ? value : 0; set => DeviceManager.Config.DefaultProfile[ProtocolDevice.Id] = value; }
        public DeviceProfile? CurrentProfile { get; set; }
        public event EventHandler<DeviceCoreOutputSetEventArgs>? OutputSet;
        public event EventHandler<DeviceCoreInputEventArgs>? Input;
        public event EventHandler? ProfileSelected;

        public Dictionary<DeviceLayoutButton, object> LayoutButtonsCurrentStatus { get; set; } = [];
        public Dictionary<int, DeviceInputEventArgs> ButtonsCurrentValue => _lastValue;


        public string Name => Layout?.Name ?? ProtocolDevice.Name;

        public DeviceCore(IProtocolDevice protocolDevice)
        {
            ProtocolDevice = protocolDevice;
            Layout = DeviceLayout.SearchLayout(protocolDevice);
            DeviceProfiles = DeviceManager.Config.DevicesProfiles.TryGetValue(protocolDevice.Id, out List<DeviceProfile>? value) ? value : [];
            CurrentProfileIndex = DeviceManager.Config.DefaultProfile.TryGetValue(protocolDevice.Id, out int value2) ? value2 : 0;

            ProtocolDevice.DeviceInput += ProtocolDevice_DeviceInput;
        }

        public void Connect()
        {
            if (ProtocolDevice.IsConnected) return;
            ProtocolDevice.Connect();

            SelectProfile(DefaultProfile);
        }
        public void Disconnect()
        {
            if (ProtocolDevice.IsConnected)
            {
                ResetAllButtons();
                ProtocolDevice.Disconnect();
            }
        }
        public void SelectProfile(int profile)
        {
            CurrentProfileIndex = profile;
            CurrentProfile = DeviceProfiles.Count>CurrentProfileIndex ? DeviceProfiles[CurrentProfileIndex] : null;
            ProfileSelected?.Invoke(this, new EventArgs());
            if (CurrentProfile != null && Layout != null)
            {
                foreach (DeviceLayoutButton button in Layout.Buttons)
                {
                    if (Layout.OutputTypes.TryGetValue(button.Output, out DeviceOutput? output))
                    {
                        if (CurrentProfile.ButtonsConfig.TryGetValue(button.X, out Dictionary<int, ButtonConfig>? buttonsColumn))
                        {
                            if (buttonsColumn.TryGetValue(button.Y, out ButtonConfig? buttonConfig))
                            {
                                switch (output.OutputType)
                                {
                                    case OutputType.Palette:
                                        if (buttonConfig.Status.Value != null)
                                        {
                                            int value = buttonConfig.Status.Value.Value<int?>() ?? 0;
                                            SetButtonContent(button, value);
                                        }
                                        else SetButtonContent(button, 0);
                                        break;
                                    case OutputType.Number:
                                        if (buttonConfig.Status.Value != null)
                                        {
                                            decimal value = buttonConfig.Status.Value.Value<decimal?>() ?? 0;
                                            SetButtonContent(button, value);
                                        }
                                        else SetButtonContent(button, 0);
                                        break;
                                }

                                continue;
                            }
                        }

                        switch (output.OutputType)
                        {
                            case OutputType.Palette:
                            case OutputType.Number:
                                SetButtonContent(button, 0);
                                break;
                        }
                    }
                }
                
                foreach (DeviceLayoutButton button in Layout.Buttons)
                {
                    RunEvent(button, ButtonEvent.Init);
                }
            }
        }
        public void SetButtonContent(DeviceLayoutButton button, object content)
        {
            if (Layout!=null && Layout.OutputTypes.TryGetValue(button.Output, out DeviceOutput? output) && (LayoutButtonsCurrentStatus) != content)
            {
                switch (output.OutputType) {
                    case OutputType.Palette:
                        if (content.GetType() == typeof(int)) ProtocolDevice.SetButtonPalette(button.Id, output.Palette[(int)content].Value);
                        break;
                }
                if (!LayoutButtonsCurrentStatus.TryAdd(button, content)) LayoutButtonsCurrentStatus[button] = content;
                OutputSet?.Invoke(this, new DeviceCoreOutputSetEventArgs(button, content));
            }
        }
        public void ResetAllButtons()
        {
            if (Layout != null)
            {
                Dictionary<string, object> clearValues = [];
                foreach (DeviceLayoutButton btn in Layout.Buttons)
                {
                    if (!clearValues.ContainsKey(btn.Output))
                    {
                        DeviceOutput output = Layout.OutputTypes[btn.Output];
                        switch (output.OutputType)
                        {
                            case OutputType.Palette:
                                clearValues.Add(btn.Output, output.Palette.FirstOrDefault()?.Value??0);
                                break;
                        }
                    }
                    if (clearValues.TryGetValue(btn.Output, out object? val)) SetButtonContent(btn, val);
                }
            }
        }
        public DeviceLayoutButton? CordsToButton(int x, int y) {
            try
            {
                return Layout?.Buttons.First(button => button.X == x && button.Y == y);
            }
            catch { }
            return null;
        }
        public DeviceLayoutButton? IdToButton(int id)
        {
            try
            {
                return Layout?.Buttons.First(button => button.Id == id);
            }
            catch { }
            return null;
        }

        private readonly Dictionary<int, DeviceInputEventArgs> _lastValue = [];
        private void ProtocolDevice_DeviceInput(object? sender, DeviceInputEventArgs e)
        {
            DeviceLayoutButton? button = IdToButton(e.ButtonID);
            if (button != null)
            {
                Input?.Invoke(this,new DeviceCoreInputEventArgs(button, e.Value,e.IsPressed));

                DeviceInputEventArgs lastE;
                if (_lastValue.TryGetValue(button.Id, out DeviceInputEventArgs? value)) lastE = value;
                else
                {
                    lastE = new DeviceInputEventArgs(e.ButtonID,0);
                    _lastValue.Add(button.Id, lastE);
                }
                _lastValue[button.Id] = e;

                if (!lastE.IsPressed && e.IsPressed) RunEvent(button, ButtonEvent.Pressed);
                else if(lastE.IsPressed && !e.IsPressed) RunEvent(button, ButtonEvent.Released);

                if (lastE.Value != e.Value) RunEvent(button, ButtonEvent.ValueChanged);

            }
        }
        public void RunEvent(DeviceLayoutButton button, ButtonEvent e) {
            if (CurrentProfile != null && CurrentProfile.ButtonsConfig.TryGetValue(button.X, out Dictionary<int, ButtonConfig>? columnButtons))
            {
                if (columnButtons.TryGetValue(button.Y, out ButtonConfig? buttonConfig))
                {
                    if (buttonConfig.EventScripts.TryGetValue(e, out NodeScript? value))
                    {
                        NodeManager.Run(value, this, button);
                    }
                }
            }
        }

    }
}
