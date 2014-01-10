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
using BunkMate.ViewModels;
using Coding4Fun.Toolkit.Controls;
using Newtonsoft.Json;
using System.IO.IsolatedStorage;

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
                // TODO: Make the strings of equal length
                
                string backContent = "";
                for (int i = 0; i < App.ViewModel.Subjects.Count && i < 3; i++)
                {
                    string spaces = "";
                    for (int j = 0; j < 5 - App.ViewModel.Subjects[i].ShortCode.Length; j++)
                        spaces += " ";
                    backContent += 
                        App.ViewModel.Subjects[i].ShortCode + 
                        spaces +
                        spaces +
                        Convert.ToString(App.ViewModel.Subjects[i].IntBunkCounter) + "\n";
                }
                string wideBackContent = "";
                for (int i = 0; i < App.ViewModel.Subjects.Count - 1 && i < 6; i+=2)
                {
                    string spaces1 = "";
                    for (int j = 0; j < 5 - App.ViewModel.Subjects[i].ShortCode.Length; j++)
                        spaces1 += " ";
                    string spaces2 = "";
                    for (int j = 0; j < 5 - App.ViewModel.Subjects[i+1].ShortCode.Length; j++)
                        spaces2 += " ";
                    string spaces3 = "  ";
                    if (App.ViewModel.Subjects[i].IntBunkCounter < 10)
                        spaces3 += " ";
                    wideBackContent +=
                        App.ViewModel.Subjects[i].ShortCode +
                        spaces1 +
                        spaces1 +
                        Convert.ToString(App.ViewModel.Subjects[i].IntBunkCounter) +
                        spaces3 +
                        App.ViewModel.Subjects[i+1].ShortCode +
                        spaces2 +
                        spaces2 +
                        Convert.ToString(App.ViewModel.Subjects[i+1].IntBunkCounter) + "\n";
                }
                FlipTileData tileData = new FlipTileData()
                {
                    BackTitle = "Bunks",
                    BackContent = backContent,
                    WideBackContent = wideBackContent
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

            ApplicationBarIconButton appBarAddButton = 
                new ApplicationBarIconButton(new Uri("/Assets/appbar.add.png", UriKind.Relative));
            appBarAddButton.Text = "Add Subject";
            ApplicationBar.Buttons.Add(appBarAddButton);
            appBarAddButton.Click += appBarAddButton_Click;

            ApplicationBarMenuItem appBarAboutMenuItem = new ApplicationBarMenuItem("About");
            ApplicationBar.MenuItems.Add(appBarAboutMenuItem);
            appBarAboutMenuItem.Click += appBarAboutMenuItem_Click;
        }

        void appBarAboutMenuItem_Click(object sender, EventArgs e)
        {
            AboutPrompt aboutMe = new AboutPrompt();
            aboutMe.Show("vaishaks", "@vaishaks", "vaishaks@outlook.com", "vaishaks.tumblr.com");
        }

        void appBarAddButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AddNewSubject.xaml", UriKind.RelativeOrAbsolute));
        }

        private void IncrementButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Grid grid = sender as Grid;
            Subject subject = grid.DataContext as Subject;
            if (subject == null)
                return;
            if (subject.MaxBunks > subject.IntBunkCounter)
                subject.IncrementBunkCounter();
            else
            {
                MessageBox.Show("You're out of bunks!", "Oops!", MessageBoxButton.OK);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            Subject subject = menuItem.DataContext as Subject;
            if (subject == null)
                return;
            // Remove subject from the ViewModel
            App.ViewModel.Subjects.Remove(subject);
            // Update the IsolatedStorage
            App.ViewModel.SaveModel();
            NavigationService.Navigate(new Uri("/MainPage.xaml?Refresh=true", UriKind.Relative));
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            Subject subject = menuItem.DataContext as Subject;
            NavigationService.Navigate(new Uri("/EditSubject.xaml?SubjectName=" + subject.Name, UriKind.RelativeOrAbsolute));        
        }
    }
}