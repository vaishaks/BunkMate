using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using BunkMate.Resources;

namespace BunkMate
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;

            // Sample code to localize the ApplicationBar
            BuildApplicationBar();

            CreateFlipTile();
        }

        private void CreateFlipTile()
        {
            ShellTile appTile = ShellTile.ActiveTiles.First();
            if (appTile != null)
            {
                FlipTileData tileData = new FlipTileData()
                {
                    BackTitle = "Bunks",
                    BackContent = "LA - 3\nSTLD - 11\nDS - 5",
                    WideBackContent = "LA - 3 STLD - 11\nDS - 5 DAA - 7\nMAT - 12 PHY - 6"
                };
                appTile.Update(tileData);
            }
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        private void BuildApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/appbar.add.png", UriKind.Relative));
            appBarButton.Text = "Add Subject";
            ApplicationBar.Buttons.Add(appBarButton);

            ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem("About");
            ApplicationBar.MenuItems.Add(appBarMenuItem);
        }
    }
}