using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SpectabisLib.Repositories;
using SpectabisUI.Events;
using SpectabisUI.Interfaces;

namespace SpectabisNext.Pages
{
    public class CreateProfile : UserControl, IPage
    {
        private readonly IPageNavigationProvider _navigation;

        public string PageTitle { get; } = "Add Game";
        public bool ShowInTitlebar { get; } = true;
        public bool HideTitlebar { get; } = false;
        public bool ReloadOnNavigation { get; } = true;

        private Image BoxArtImage;
        private Button SelectGameButton;

        [Obsolete("XAMLIL placeholder", true)]
        public CreateProfile() { }

        public CreateProfile(IPageNavigationProvider navigation)
        {
            InitializeComponent();
            RegisterChildren();

            _navigation = navigation;
            _navigation.PageNavigationEvent += OnNavigation;
        }

        private void OnNavigation(object sender, NavigationArgs e)
        {
            if(e.Page != this)
            {
                _navigation.PageNavigationEvent -= OnNavigation;
                return;
            }
        }

        private void RegisterChildren()
        {
            BoxArtImage = this.FindControl<Image>(nameof(BoxArtImage));
            SelectGameButton = this.FindControl<Button>(nameof(SelectGameButton));
        }

        public void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void SelectGame()
        {
            var dialogWindow = new Window();
            var fileDialog = new OpenFileDialog();
            fileDialog.ShowAsync(dialogWindow);
        }

        ~CreateProfile()
        {
            Console.WriteLine("Destroying CreateProfile page");
        }
    }
}