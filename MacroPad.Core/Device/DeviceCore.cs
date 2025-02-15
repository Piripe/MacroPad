using MacroPad.Core.Config;
using MacroPad.Shared.Plugin.Protocol;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using MacroPad.Shared.Device;

namespace MacroPad.Core.Device
{
    public class DeviceCore
    {
        public IProtocolDevice ProtocolDevice { get; }
        public DeviceLayout? Layout { get; set; }
        public List<DeviceProfile> DeviceProfiles { get; }
        public int CurrentProfileIndex { get; set; }
        public int DefaultProfile { get => DeviceManager.Config.DefaultProfile.ContainsKey(ProtocolDevice.Id) ? DeviceManager.Config.DefaultProfile[ProtocolDevice.Id] : 0; set => DeviceManager.Config.DefaultProfile[ProtocolDevice.Id] = value; }
        public DeviceProfile? CurrentProfile { get; set; }
        public event EventHandler<DeviceCoreOutputSetEventArgs>? OutputSet;
        public event EventHandler<DeviceCoreInputEventArgs>? Input;
        public event EventHandler? ProfileSelected;

        public Dictionary<DeviceLayoutButton, object> LayoutButtonsCurrentStatus { get; set; } = new Dictionary<DeviceLayoutButton, object>();
        public Dictionary<int, DeviceInputEventArgs> ButtonsCurrentValue => _lastValue;


        public string Name => Layout==null?ProtocolDevice.Name:Layout.Name;

        public DeviceCore(IProtocolDevice protocolDevice)
        {
            ProtocolDevice = protocolDevice;
            Layout = DeviceLayout.SearchLayout(protocolDevice);
            DeviceProfiles = DeviceManager.Config.DevicesProfiles.ContainsKey(protocolDevice.Id) ? DeviceManager.Config.DevicesProfiles[protocolDevice.Id] : new List<DeviceProfile>();
            CurrentProfileIndex = DeviceManager.Config.DefaultProfile.ContainsKey(protocolDevice.Id) ? DeviceManager.Config.DefaultProfile[protocolDevice.Id] : 0;

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
                    if (Layout.OutputTypes.ContainsKey(button.Output))
                    {
                        DeviceOutput output = Layout.OutputTypes[button.Output];
                        if (CurrentProfile.ButtonsConfig.ContainsKey(button.X))
                        {
                            Dictionary<int, ButtonConfig> buttonsColumn = CurrentProfile.ButtonsConfig[button.X];
                            if (buttonsColumn.ContainsKey(button.Y))
                            {
                                ButtonConfig buttonConfig = buttonsColumn[button.Y];

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
            if (Layout!=null && Layout.OutputTypes.ContainsKey(button.Output) && (LayoutButtonsCurrentStatus) != content)
            {
                DeviceOutput output = Layout.OutputTypes[button.Output];
                switch (output.OutputType) {
                    case OutputType.Palette:
                        if (content.GetType() == typeof(int)) ProtocolDevice.SetButtonPalette(button.Id, output.Palette[(int)content].Value);
                        break;
                }
                if (LayoutButtonsCurrentStatus.ContainsKey(button)) LayoutButtonsCurrentStatus[button] = content;
                else LayoutButtonsCurrentStatus.Add(button, content); 

                OutputSet?.Invoke(this, new DeviceCoreOutputSetEventArgs(button, content));
            }
        }
        public void ResetAllButtons()
        {
            if (Layout != null)
            {
                Dictionary<string, object> clearValues = new();
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

        private Dictionary<int, DeviceInputEventArgs> _lastValue = new Dictionary<int, DeviceInputEventArgs>();
        private void ProtocolDevice_DeviceInput(object? sender, DeviceInputEventArgs e)
        {
            DeviceLayoutButton? button = IdToButton(e.ButtonID);
            if (button != null)
            {
                Input?.Invoke(this,new DeviceCoreInputEventArgs(button, e.Value,e.IsPressed));

                DeviceInputEventArgs lastE;
                if (_lastValue.ContainsKey(button.Id)) lastE=_lastValue[button.Id];
                else
                {
                    lastE = new DeviceInputEventArgs(e.ButtonID,0);
                    _lastValue.Add(button.Id, lastE);
                }
                _lastValue[button.Id] = e;

                if (!lastE.IsPressed && e.IsPressed) RunEvent(button, ButtonEvent.Pressed);
                else if(lastE.IsPressed && !e.IsPressed) RunEvent(button, ButtonEvent.Released);

                if (lastE.Value != e.Value) RunEvent(button, ButtonEvent.ValueChanged);




                //if (e.IsPressed && Layout != null && Layout.OutputTypes.ContainsKey(button.Output))
                //{
                //    DeviceOutput output = Layout.OutputTypes[button.Output];
                //    if (output.OutputType == OutputType.Palette) SetButtonContent(button,Random.Shared.Next(0, output.Palette.Length - 1));
                //}
            }
        }
        public void RunEvent(DeviceLayoutButton button, ButtonEvent e) {
            if (CurrentProfile != null && CurrentProfile.ButtonsConfig.ContainsKey(button.X))
            {
                Dictionary<int, ButtonConfig> columnButtons = CurrentProfile.ButtonsConfig[button.X];
                if (columnButtons.ContainsKey(button.Y))
                {
                    ButtonConfig buttonConfig = columnButtons[button.Y];
                    if (buttonConfig.EventScripts.ContainsKey(e))
                    {
                        NodeManager.Run(buttonConfig.EventScripts[e], this, button);
                    }
                }
            }
        }

    }
}
