using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MacroPad.Core.Device;
using MacroPad.Core.BasePlugin.Device;
using MacroPad.Controls.Settings;
using MacroPad.Pages.Settings;
using MacroPad.Shared.Plugin;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using Avalonia.Media;
using Avalonia.Threading;
using Avalonia.Media.Imaging;
using System;
using Avalonia.Interactivity;
using System.Threading.Tasks;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Newtonsoft.Json.Linq;
using System.IO;
using MacroPad.Shared.Device;

namespace MacroPad.Controls;

public partial class DeviceViewer : UserControl
{
    public static readonly StyledProperty<DeviceCore> DeviceProperty =
        AvaloniaProperty.Register<DeviceViewer, DeviceCore>(nameof(Device), new DeviceCore(new BaseDevice()));
    public DeviceCore Device
    {
        get => this.GetValue(DeviceProperty);
        set => SetValue(DeviceProperty, value);
    }

    private Dictionary<int, Grid> _buttons = new Dictionary<int, Grid>();
    private Dictionary<int, Control> _buttonsDisplay = new Dictionary<int, Control>();
    public event EventHandler<DeviceViewerButtonPressedEventArgs>? ButtonPressed;

    private Grid? _selectedButton;

    bool originalZoom = true;
    bool movingToOriginalPan = false;
    double lastZoom;

    public DeviceViewer()
    {
        InitializeComponent();

        ZoomAndPan.SizeChanged += ZoomAndPan_SizeChanged;
        ZoomAndPan.ZoomChanged += ZoomAndPan_ZoomChanged;
    }

    private async void ZoomAndPan_SizeChanged(object? sender, SizeChangedEventArgs e)
    {
        if (originalZoom && Device.Layout != null)
        {
            ZoomAndPan.Zoom(Math.Min(ZoomAndPan.Bounds.Width / Device.Layout.DWidth, ZoomAndPan.Bounds.Height / Device.Layout.DHeight), ZoomAndPan.Bounds.Width / 2, ZoomAndPan.Bounds.Height / 2, false);
            originalZoom = true;
            if (!movingToOriginalPan)
            {
                movingToOriginalPan = true;
                await Task.Delay(500);
                originalZoom = true;
                movingToOriginalPan = false;
            }
        }
    }

    private void ZoomAndPan_ZoomChanged(object sender, Avalonia.Controls.PanAndZoom.ZoomChangedEventArgs e)
    {
        if (lastZoom != e.ZoomX + e.ZoomY)
        {
            originalZoom = false;
            lastZoom = e.ZoomX + e.ZoomY;
        }

        if (ZoomAndPan.Background is VisualBrush visualBrush) visualBrush.DestinationRect = new RelativeRect(e.OffsetX * -1, e.OffsetY * -1, 20, 20, RelativeUnit.Absolute);
    }


    // TODO: Implement slider using this way (but in C# btw)
    //<Slider x:Name="TestTrack" Minimum="0" Maximum="100" Value="25" Width="196" Height="32" VerticalAlignment="Center" HorizontalAlignment="Center">
    //  <Slider.Template>
    //      <ControlTemplate>
    //          <Track Name = "PART_Track" Grid.Row="1" Margin="2 0"
    //                 Grid.ColumnSpan="3" Orientation="Horizontal"
    //                 Minimum="{TemplateBinding Minimum}"
    //                 Maximum="{TemplateBinding Maximum}"
    //                 Value="{TemplateBinding Value, Mode=TwoWay}"
    //                 ClipToBounds="False">
    //              <Thumb Name = "thumb"
    //                     Margin="0" Padding="0"
    //					   DataContext="{TemplateBinding Value}">
    //                  <Thumb.Template>
    //                      <ControlTemplate>
    //                          <Rectangle Width = "32" Height="16" Fill="Aqua" />
    //						</ControlTemplate>
    //					</Thumb.Template>
    //				</Thumb>
    //			</Track>
    //		</ControlTemplate>
    //  </Slider.Template>
    //</Slider>

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        if (Device.Layout != null)
        {
            DisplayCanvas.Children.Clear();
            _buttons.Clear();
            _buttonsDisplay.Clear();

            DisplayCanvas.Width = Device.Layout.DWidth;
            DisplayCanvas.Height = Device.Layout.DHeight;

            if (Device.Layout.DImage != null)
            {
                Image image = new Image()
                {
                    Source = new Bitmap(Device.Layout.GetAssetPath(Device.Layout.DImage)),
                    Width = Device.Layout.DWidth,
                    Height = Device.Layout.DHeight
                };
                DisplayCanvas.Children.Add(image);
            }

            foreach (DeviceLayoutButton button in Device.Layout.Buttons)
            {

                if (Device.Layout.OutputTypes.ContainsKey(button.Output))
                {
                    DeviceOutput outputType = Device.Layout.OutputTypes[button.Output];


                    Grid buttonContainer = new Grid();
                    Control control;
                    Grid container = new Grid();
                    Control? displayControl = null;
                    switch (button.Type)
                    {
                        case ButtonType.Slider:
                            var slider = new Slider()
                            {
                                Minimum = 0,
                                Maximum = 1,
                                Value = Device.ButtonsCurrentValue.ContainsKey(button.Id) ? Device.ButtonsCurrentValue[button.Id].Value : 0,
                                Width = button.DWidth,
                                Height = button.DHeight,
                                Background = Brushes.Transparent,
                                Template = new FuncControlTemplate((value, namescore) =>
                                    new Track()
                                    {
                                        Name = "PART_Track",
                                        Margin = new Thickness(2, 0),
                                        ClipToBounds = false,
                                        [!Track.MinimumProperty] = new TemplateBinding(Slider.MinimumProperty),
                                        [!Track.MaximumProperty] = new TemplateBinding(Slider.MaximumProperty),
                                        [!Track.ValueProperty] = new TemplateBinding(Slider.ValueProperty),
                                        Thumb = new Thumb()
                                        {
                                            Margin = new Thickness(0),
                                            Padding = new Thickness(0),
                                            Template = new FuncControlTemplate((value, namescope) => container),
                                            [!Thumb.DataContextProperty] = new TemplateBinding(Track.ValueProperty),
                                        }
                                    }
                                )
                            };
                            //var thumb = new Thumb()
                            //{
                            //    Name="thumb",
                            //    Margin = new Thickness(0),
                            //    Padding = new Thickness(0),
                            //    Template = new ControlTemplate()
                            //    {
                            //        Content = container
                            //    },
                            //    DataContext = 0,
                            //};
                            //var track = new Track()
                            //{
                            //    Name = "PART_Track",
                            //    Margin = new Thickness(2, 0),
                            //    ClipToBounds = false,
                            //    Thumb = thumb
                            //};
                            //track.GetObservable(Track.ValueProperty).Subscribe(value => thumb.DataContext = value);
                            //slider.GetObservable(Slider.MinimumProperty).Subscribe(value => track.Minimum = value);
                            //slider.GetObservable(Slider.MaximumProperty).Subscribe(value => track.Maximum = value);
                            //slider.GetObservable(Slider.ValueProperty).Subscribe(value => track.Value = value);

                            //slider.Template = new FuncControlTemplate()

                            control = slider;
                            break;
                        default:
                            control = container;
                            container.Width = button.DWidth;
                            container.Height = button.DHeight;
                            break;
                    }

                    switch (outputType.OutputType)
                    {
                        case OutputType.None:
                        case OutputType.Palette:
                            displayControl = new Grid()
                            {
                                Children = { 
                                    new Image(),
                                    //new GifImage()
                                }
                            };

                            break;
                    }

                    if (displayControl != null) container.Children.Add(displayControl);
                    buttonContainer.Children.Add(control);

                    CornerRadius cornerRadius = new CornerRadius(4);

                    if (Device.Layout.OutputTypes.ContainsKey(button.Output))
                    {
                        if (outputType.CornerRadius != null) cornerRadius = CornerRadius.Parse(outputType.CornerRadius);
                    }

                    Border border = new Border()
                    {
                        Width = button.DWidth,
                        Height = button.DHeight,
                        CornerRadius = cornerRadius,
                        BorderThickness = new Thickness(0)
                    };
                    buttonContainer.Children.Add(border);
                    Border selectionBorder = new Border()
                    {
                        Width = button.DWidth,
                        Height = button.DHeight,
                        CornerRadius = cornerRadius,
                        BorderThickness = new Thickness(6),
                        Opacity = 0
                    };
                    border.Child = selectionBorder;

                    buttonContainer.Tag = button;
                    buttonContainer.SetCurrentValue(Canvas.TopProperty, button.DY);
                    buttonContainer.SetCurrentValue(Canvas.LeftProperty, button.DX);
                    if (button.Rotation != 0) buttonContainer.RenderTransform = new RotateTransform(button.Rotation);

                    buttonContainer.Tapped += Control_Tapped;
                    _buttons.Add(button.Id, buttonContainer);
                    if (displayControl != null) _buttonsDisplay.Add(button.Id, displayControl);
                    DisplayCanvas.Children.Add(buttonContainer);

                    object currentStatus = 0;

                    if (Device.LayoutButtonsCurrentStatus.ContainsKey(button)) currentStatus = Device.LayoutButtonsCurrentStatus[button];

                    SetButtonDisplay(buttonContainer, displayControl, button, currentStatus);
                }
            }

            if (_selectedButton != null)
            {
                Border? lastSelectionBorder = (Border?)((Border)_selectedButton.Children[1]).Child;
                if (lastSelectionBorder != null) lastSelectionBorder.Opacity = 1;
            }

            Device.OutputSet += Device_OutputSet;
            Device.Input += Device_Input;


            DisplayCanvas.Children.Last().Loaded += (object? sender, RoutedEventArgs e) =>
            {
                ZoomAndPan.Uniform(true);
                ZoomAndPan.Zoom(Math.Min(ZoomAndPan.Bounds.Width / Device.Layout.DWidth, ZoomAndPan.Bounds.Height / Device.Layout.DHeight), ZoomAndPan.Bounds.Width / 2, ZoomAndPan.Bounds.Height / 2, true);
                originalZoom = true;
            };
        }
        else
        {
            //TODO: Maybe add a fallback viewer
        }
    }

    private void Control_Tapped(object? sender, Avalonia.Input.TappedEventArgs e)
    {
        SelectButton((Grid?)sender);
    }
    public void SelectButton(Grid? button)
    {
        if (_selectedButton != null)
        {
            Border? lastSelectionBorder = (Border?)((Border)_selectedButton.Children[1]).Child;
            if (lastSelectionBorder != null) lastSelectionBorder.Opacity = 0;
        }
        if (button != null)
        {
            _selectedButton = button;
            Border? selectionBorder = (Border?)((Border)button.Children[1]).Child;
            if (selectionBorder != null) selectionBorder.Opacity = 1;
        }


        DeviceLayoutButton? layoutButton = (DeviceLayoutButton?)button?.Tag;
        ButtonPressed?.Invoke(this, new DeviceViewerButtonPressedEventArgs(layoutButton));
    }

    private void Device_Input(object? sender, DeviceCoreInputEventArgs e)
    {
        if (_buttons.ContainsKey(e.Button.Id))
        {
            Grid container = _buttons[e.Button.Id];
            Control control = container.Children[0];
            Border border = (Border)container.Children[1];
            Dispatcher.UIThread.Invoke(() =>
            {
                switch (e.Button.Type)
                {
                    case ButtonType.Button:
                        border.BorderThickness = new Thickness(e.IsPressed ? 3 : 0);
                        break;
                    case ButtonType.Slider:
                        if (control is Slider slider)
                        {
                            slider.Value = e.Value;
                        }
                        break;
                }
            });
        }
    }

    private void Device_OutputSet(object? sender, DeviceCoreOutputSetEventArgs e)
    {
        if (_buttons.ContainsKey(e.Button.Id))
        {
            SetButtonDisplay(_buttons[e.Button.Id], _buttonsDisplay[e.Button.Id], e.Button, e.Value);
        }
    }

    private void SetButtonDisplay(Control control, Control? displayControl, DeviceLayoutButton button, object content)
    {
        if (Device.Layout != null)
        {
            if (Device.Layout.OutputTypes.ContainsKey(button.Output))
            {
                DeviceOutput outputType = Device.Layout.OutputTypes[button.Output];
                
                Dispatcher.UIThread.Invoke(() =>
                {

                    Grid grid = (Grid)control;
                    Border border = (Border)grid.Children[1];
                    Border? selectionBorder = (Border?)border.Child;

                    Grid? displayGrid;
                    Image? image = null;
                    //GifImage? gifImage = null;

                    Color? borderColor = null;

                    switch (outputType.OutputType)
                    {
                        case OutputType.None:
                            borderColor = Color.FromUInt32(outputType.Color);

                            displayGrid = displayControl as Grid;
                            if (displayGrid != null)
                            {
                                image = (Image)displayGrid.Children[0];
                                //gifImage = (GifImage)displayGrid.Children[1];
                            }

                            if (outputType.Image == null)
                            {
                                if (image != null) image.Source = null;
                                //if (gifImage != null) gifImage.SourceUriRaw = null;
                                border.Background = new SolidColorBrush(Color.FromUInt32(outputType.Color), 0.5);
                            }
                            else
                            {
                                border.Background = null;
                                if (Path.GetExtension(outputType.Image) == ".gif")
                                {
                                    //if (gifImage != null) gifImage.SourceUriRaw = Device.Layout.GetAssetPath(outputType.Image);
                                    if (image != null) image.Source = null;
                                }
                                else
                                {
                                    if (image != null) image.Source = new Bitmap(Device.Layout.GetAssetPath(outputType.Image));
                                    //if (gifImage != null) gifImage.SourceUriRaw = null;
                                }
                            }
                            break;
                        case OutputType.Palette:

                            int contentInt = (content.GetType() == typeof(int) ? (int?)content : null) ?? 0;
                            if (contentInt < outputType.Palette.Length)
                            {
                                IPaletteValue value = outputType.Palette[contentInt];

                                borderColor = Color.FromUInt32(value.Color);

                                displayGrid = displayControl as Grid;
                                if (displayGrid != null)
                                {
                                    image = (Image)displayGrid.Children[0];
                                    //gifImage = (GifImage)displayGrid.Children[1];
                                }

                                if (value.Image == null)
                                {
                                    if (image != null) image.Source = null;
                                    //if (gifImage != null) gifImage.SourceUriRaw = null;
                                    border.Background = new SolidColorBrush(Color.FromUInt32(value.Color), 0.5);
                                }
                                else
                                {
                                    border.Background = null;
                                    if (Path.GetExtension(value.Image) == ".gif")
                                    {
                                        //if (gifImage != null) gifImage.SourceUriRaw = Device.Layout.GetAssetPath(value.Image);
                                        if (image != null) image.Source = null;
                                    }
                                    else
                                    {
                                        if (image != null) image.Source = new Bitmap(Device.Layout.GetAssetPath(value.Image));
                                        //if (gifImage != null) gifImage.SourceUriRaw = null;
                                    }
                                }

                            }
                            break;
                    }

                    if (borderColor.HasValue)
                    {
                        border.BorderBrush = new SolidColorBrush(borderColor.Value, 0.85);
                        if (selectionBorder != null)
                        {
                            selectionBorder.BorderBrush = new SolidColorBrush(borderColor.Value, 1);
                            selectionBorder.Background = new SolidColorBrush(borderColor.Value, 0.5);
                        }
                    }
                });

            }
        }
    }
}