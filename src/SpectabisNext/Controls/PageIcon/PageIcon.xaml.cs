using System;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using SpectabisUI.Interfaces;

namespace SpectabisNext.Controls.PageIcon
{
    public class PageIcon : UserControl, IPageIcon
    {
        public IPage Destination { get; private set; }
        public event EventHandler InvokedCallback;

        private Image DisplayImage { get; set; }
        private TextBlock DisplayText { get; set; }
        private Rectangle HoverOverlayRectangle { get; set; }

        [Obsolete("XAMLIL placeholder", true)]
        public PageIcon() { }

        public PageIcon(IPage destination)
        {
            Initialize(destination);
            SetIconString(destination.PageTitle);
        }

        public PageIcon(IPage destination, string stringDisplay)
        {
        }

        private void Initialize(IPage destination)
        {
            InitializeComponent();
            RegisterChildren();
            RegisterEvents();

            Destination = destination;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void SetIconString(string str)
        {
            DisplayText.Text = str;
            DisplayText.IsVisible = true;
        }

        private void RegisterChildren()
        {
            DisplayImage = this.FindControl<Image>("DisplayImage");
            DisplayText = this.FindControl<TextBlock>("DisplayText");
            HoverOverlayRectangle = this.FindControl<Rectangle>("HoverOverlay");
        }

        private void RegisterEvents()
        {
            this.PointerReleased += OnPointerReleased;
            this.PointerEnter += OnMousePointerEnter;
            this.PointerLeave += OnMousePointerLeave;
        }

        private void OnPointerReleased(object sender, PointerReleasedEventArgs args)
        {
            InvokedCallback?.Invoke(this, EventArgs.Empty);
        }

        private void OnMousePointerLeave(object sender, PointerEventArgs e)
        {
            SetShowHoverOverlay(false);
        }

        private void SetShowHoverOverlay(bool value)
        {
            HoverOverlayRectangle.IsVisible = value;
        }

        private void OnMousePointerEnter(object sender, PointerEventArgs e)
        {
            SetShowHoverOverlay(true);
        }
    }
}