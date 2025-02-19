using Avalonia;
using Avalonia.Controls;
using FluentAvalonia.Core;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Navigation;
using MacroPad.Pages;
using MacroPad.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace MacroPad.Views
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
        }


        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
        }
    }
}
