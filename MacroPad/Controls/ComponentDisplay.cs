using Avalonia;
using Avalonia.Controls;
using MacroPad.Controls.Home;
using MacroPad.Core.Device;
using MacroPad.Shared.Plugin;
using System.Collections.ObjectModel;
using IComponent = MacroPad.Shared.Plugin.Components.IComponent;

namespace MacroPad.Controls
{
    public class ComponentDisplay : UserControl
    {

        /// <summary>
        /// Component StyledProperty definition
        /// </summary>
        public static readonly StyledProperty<IComponent?> ComponentProperty =
            AvaloniaProperty.Register<ComponentDisplay, IComponent?>(nameof(Component));

        /// <summary>
        /// Gets or sets the Component property. This StyledProperty 
        /// indicates the current component displayed.
        /// </summary>
        public IComponent? Component
        {
            get => GetValue(ComponentProperty);
            set => SetValue(ComponentProperty, value);
        }


        /// <summary>
        /// SmallMode StyledProperty definition
        /// </summary>
        public static readonly StyledProperty<bool> SmallModeProperty =
            AvaloniaProperty.Register<ComponentDisplay, bool>(nameof(SmallMode), false);

        /// <summary>
        /// Gets or sets the SmallMode property. This StyledProperty 
        /// indicates if the component have a small height or not.
        /// </summary>
        public bool SmallMode
        {
            get => this.GetValue(SmallModeProperty);
            set => SetValue(SmallModeProperty, value);
        }


        /// <summary>
        /// ResourceManager StyledProperty definition
        /// </summary>
        public static readonly StyledProperty<IResourceManager> ResourceManagerProperty =
            AvaloniaProperty.Register<ComponentDisplay, IResourceManager>(nameof(ResourceManager));

        /// <summary>
        /// Gets or sets the ResourceManager property. This StyledProperty
        /// indicates the resource manager used by the component.
        /// </summary>
        public IResourceManager ResourceManager
        {
            get => GetValue(ResourceManagerProperty);
            set => SetValue(ResourceManagerProperty, value);
        }






        public ComponentDisplay()
        {

        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == ComponentProperty)
            {
                if (change.NewValue is IComponent component)
                {
                    UpdateComponent(component, SmallMode);
                }
            }
            else if (change.Property == SmallModeProperty) {
                IComponent? component = Component;
                if (component != null)
                {
                    UpdateComponent(component, (change.NewValue as bool?) ?? false);
                }
            }
        }

        private void UpdateComponent(IComponent component, bool smallMode)
        {
            switch (component)
            {
                case Shared.Plugin.Components.TextBox textBox:
                    TextBox textBoxControl = new() { 
                        Text = textBox.GetText != null ? textBox.GetText(ResourceManager) : "", 
                        Width = 196,  
                    };
                    textBoxControl.TextChanged += (object? sender, TextChangedEventArgs e) => {
                        textBox.TextChanged?.Invoke(ResourceManager, textBoxControl.Text);
                    };
                    if (smallMode)
                    {
                        textBoxControl.Height = 20d;
                        textBoxControl.Classes.Add("small");
                    }
                    Content = textBoxControl;
                    break;
                case Shared.Plugin.Components.NumericUpDown numericUpDown:
                    NumericUpDown numericUpDownControl = new()
                    {
                        Minimum = numericUpDown.Min,
                        Maximum = numericUpDown.Max,
                        Value = numericUpDown.GetValue != null ? numericUpDown.GetValue(ResourceManager) : 0,
                        Width = 196
                    };
                    if (smallMode)
                    {
                        numericUpDownControl.Height = 20d;
                        numericUpDownControl.Classes.Add("small");
                    }
                    numericUpDownControl.ValueChanged += (object? sender, NumericUpDownValueChangedEventArgs e) => {
                        numericUpDown.ValueChanged?.Invoke(ResourceManager, e.NewValue ?? 0);
                    };
                    Content = numericUpDownControl;
                    break;
                case Shared.Plugin.Components.ComboBox comboBox:
                    ObservableCollection<string> GetItems()
                    {
                        if (comboBox.GetItems != null)
                        {
                            return new ObservableCollection<string>(comboBox.GetItems(ResourceManager));
                        }
                        else
                        {
                            return comboBox.Items;
                        }
                    }
                    ObservableCollection<string> items = GetItems();
                    ComboBox comboBoxControl = new() { ItemsSource = items };

                    void UpdateSelection()
                    {
                        int index = 0;
                        if (comboBox.GetSelectedItem != null)
                        {
                            string value = comboBox.GetSelectedItem(ResourceManager);
                            index = items.IndexOf(value);
                        }
                        if (comboBox.GetSelection != null)
                        {
                            index = comboBox.GetSelection(ResourceManager);
                        }
                        if (index < items.Count && index >= 0) comboBoxControl.SelectedIndex = index;
                    }
                    UpdateSelection();


                    comboBoxControl.Items.CollectionChanged += (s, e) =>
                    {
                        UpdateSelection();
                    };

                    comboBoxControl.SelectionChanged += (object? sender, SelectionChangedEventArgs e) => {
                        comboBox.SelectionChanged?.Invoke(ResourceManager, comboBoxControl.SelectedIndex);
                    };
                    comboBoxControl.Width = 196;
                    if (smallMode)
                    {
                        comboBoxControl.Height = 20;
                        comboBoxControl.Classes.Add("small");
                    }
                    Content = comboBoxControl;
                    break;
                default:
                    Content = new TextBlock() { Text = "[Unsupported component]" };
                    break;
            }
        }
    }
}
