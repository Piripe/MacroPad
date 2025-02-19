using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using MacroPad.Core.BasePlugin.Device;
using MacroPad.Core.Device;
using MacroPad.Pages;
using System.Linq;
using System;
using MacroPad.Core.Config;
using FluentAvalonia.UI.Controls;
using ReactiveUI;
using Avalonia.Threading;

namespace MacroPad.Controls.Home;

public partial class DeviceProfileSelector : UserControl
{
    public DeviceCore Device { get; set; } = new DeviceCore(new BaseDevice());
    public DeviceViewer? DeviceViewer { get; set; }
    public MainPage? MainPage { get; set; }


    private bool _changingProfile = false;
    public DeviceProfileSelector()
    {
        InitializeComponent();

        KeyBindings.Add(new KeyBinding() { Gesture = new KeyGesture(Key.F2), Command = ReactiveCommand.Create(() => Rename_Click(this, new RoutedEventArgs())) });
        KeyBindings.Add(new KeyBinding() { Gesture = new KeyGesture(Key.Delete), Command = ReactiveCommand.Create(() => Remove_Click(this, new RoutedEventArgs())) });
        KeyBindings.Add(new KeyBinding() { Gesture = new KeyGesture(Key.Up, KeyModifiers.Alt), Command = ReactiveCommand.Create(() => MoveUp_Click(this, new RoutedEventArgs())) });
        KeyBindings.Add(new KeyBinding() { Gesture = new KeyGesture(Key.Down, KeyModifiers.Alt), Command = ReactiveCommand.Create(() => MoveDown_Click(this, new RoutedEventArgs())) });
    }

    private void Device_ProfileSelected(object? sender, EventArgs e)
    {
        Dispatcher.UIThread.Invoke(() =>
        {
            if (DeviceProfileSelectorList.SelectedIndex != Device.CurrentProfileIndex)
            {
                _changingProfile = true;
                DeviceProfileSelectorList.SelectedIndex = Device.CurrentProfileIndex;
            }
        });
    }

    private void DeviceProfileSelectorList_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (DeviceProfileSelectorList.SelectedItem != null && DeviceProfileSelectorList.ItemsSource != null)
        {
            if (_changingProfile) _changingProfile = false;
            else Device.SelectProfile(((ProfileListBoxItemViewModel)DeviceProfileSelectorList.SelectedItem).Index);
            DeviceViewer?.SelectButton(null);
        }
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        Device.ProfileSelected += Device_ProfileSelected;
        RefreshList(Device.CurrentProfileIndex);
    }

    private void AddProfile_Click(object? sender, RoutedEventArgs e)
    {
        int profileNumber = 1;

        while (Device.DeviceProfiles.Any(x=>x.Name=="New Profile" + (profileNumber == 1 ? "" : $" {profileNumber}"))) profileNumber++;

        Device.DeviceProfiles.Add(new DeviceProfile() { Name = "New Profile" + (profileNumber == 1 ? "" : $" {profileNumber}") });

        RefreshList();
    }
    private void MakeDefault_Click(object? sender, RoutedEventArgs e)
    {
        if (DeviceProfileSelectorList.SelectedItem != null)
        {
            Device.DefaultProfile = ((ProfileListBoxItemViewModel)DeviceProfileSelectorList.SelectedItem).Index;

            RefreshList();
        }
    }
    public void RefreshList(int? overrideIndex = null)
    {
        DeviceProfileSelectorList.SelectionChanged -= DeviceProfileSelectorList_SelectionChanged;
        int selectedIndex = DeviceProfileSelectorList.SelectedIndex;
        DeviceProfileSelectorList.ItemsSource = null;
        int defaultProfile = Device.DefaultProfile;
        DeviceProfileSelectorList.ItemsSource = Device.DeviceProfiles.Select(x => new ProfileListBoxItemViewModel() { Name = x.Name, Index = Device.DeviceProfiles.IndexOf(x), IsDefault = Device.DeviceProfiles.IndexOf(x) == defaultProfile });

        DeviceProfileSelectorList.SelectionChanged += DeviceProfileSelectorList_SelectionChanged;

        if (DeviceProfileSelectorList.ItemCount > 0)
        {
            DeviceProfileSelectorList.SelectedIndex = Math.Min(overrideIndex ?? selectedIndex, DeviceProfileSelectorList.ItemCount - 1);
            DeviceProfileSelectorList.SelectedItem = DeviceProfileSelectorList.Items[DeviceProfileSelectorList.SelectedIndex];
        }
    }
    private async void Rename_Click(object? sender, RoutedEventArgs e)
    {
        if (DeviceProfileSelectorList.SelectedItem != null)
        {
            var textBox = new TextBox() { Text = ((ProfileListBoxItemViewModel)DeviceProfileSelectorList.SelectedItem).Name };
            var dialog = new ContentDialog() { Title = "Rename Profile", IsPrimaryButtonEnabled = true, PrimaryButtonText = "Cancel", IsSecondaryButtonEnabled = true, SecondaryButtonText = "Rename", Content = textBox };
            dialog.KeyBindings.Add(new KeyBinding() { Gesture = new KeyGesture(Key.Enter), Command = ReactiveCommand.Create(() => dialog.Hide(ContentDialogResult.Secondary)) });
            textBox.SelectAll();
            ContentDialogResult result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Secondary)
            {
                string? newName = textBox.Text;
                if (newName != null)
                {
                    Device.DeviceProfiles[DeviceProfileSelectorList.SelectedIndex].Name = newName;

                    RefreshList();
                }
            }
        }
    }
    object? deletionWarning;
    private async void Remove_Click(object? sender, RoutedEventArgs e)
    {
        if (DeviceProfileSelectorList.SelectedItem != null)
        {
            if (deletionWarning == null) TryGetResource("deletionWarning", null, out deletionWarning);
            var dialog = new ContentDialog()
            {
                Title = "Remove Profile",
                IsPrimaryButtonEnabled = true,
                PrimaryButtonText = "Cancel",
                IsSecondaryButtonEnabled = true,
                SecondaryButtonText = "Remove",
                Content = deletionWarning
            };
            ContentDialogResult result = await dialog.ShowAsync();
            dialog.Content = null;
            if (result == ContentDialogResult.Secondary)
            {
                if (DeviceProfileSelectorList.SelectedIndex == Device.DefaultProfile) Device.DefaultProfile = 0;
                Device.DeviceProfiles.RemoveAt(DeviceProfileSelectorList.SelectedIndex);
                RefreshList();
            }

        }
    }
    private void MoveUp_Click(object? sender, RoutedEventArgs e)
    {
        if (DeviceProfileSelectorList.SelectedItem != null)
        {
            MoveSelected(DeviceProfileSelectorList.SelectedIndex - 1);
        }
    }
    private void MoveDown_Click(object? sender, RoutedEventArgs e)
    {
        if (DeviceProfileSelectorList.SelectedItem != null)
        {
            MoveSelected(DeviceProfileSelectorList.SelectedIndex + 1);
        }
    }
    public void MoveSelected(int newIndex)
    {
        DeviceProfileSelectorList.SelectionChanged -= DeviceProfileSelectorList_SelectionChanged;
        int defaultProfile = Device.DefaultProfile;
        newIndex = Math.Min(Math.Max(newIndex, 0), Device.DeviceProfiles.Count - 1);
        DeviceProfile profile = Device.DeviceProfiles[DeviceProfileSelectorList.SelectedIndex];
        if (DeviceProfileSelectorList.SelectedIndex > defaultProfile && newIndex <= defaultProfile) Device.DefaultProfile++;
        else if (DeviceProfileSelectorList.SelectedIndex < defaultProfile && newIndex >= defaultProfile) Device.DefaultProfile--;
        else if (DeviceProfileSelectorList.SelectedIndex == defaultProfile) Device.DefaultProfile = newIndex;
        Device.DeviceProfiles.RemoveAt(DeviceProfileSelectorList.SelectedIndex);
        Device.DeviceProfiles.Insert(newIndex, profile);
        RefreshList(newIndex);
    }
}

public struct ProfileListBoxItemViewModel
{
    public string Name { get; set; }
    public int Index { get; set; }
    public bool IsDefault { get; set; }
}