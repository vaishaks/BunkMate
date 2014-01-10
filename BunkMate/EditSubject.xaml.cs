using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using BunkMate.ViewModels;
using Newtonsoft.Json;
using System.IO.IsolatedStorage;

namespace BunkMate
{
    public partial class EditSubject : PhoneApplicationPage
    {
        public EditSubject()
        {
            InitializeComponent();

            BuildApplicationBar();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (DataContext == null)
            {
                string subjectName = "";
                Subject subject = null;
                if (NavigationContext.QueryString.TryGetValue("SubjectName", out subjectName))
                {                    
                    foreach (Subject item in App.ViewModel.Subjects)
                    {
                        if (item.Name == subjectName)
                        {
                            subject = item;
                            break;
                        }
                    }
                    DataContext = subject;
                }
            }
        }

        private void BuildApplicationBar()
        {
            ApplicationBar = new ApplicationBar();

            ApplicationBarIconButton appBarCheckButton =
                new ApplicationBarIconButton(new Uri("/Assets/appbar.check.png", UriKind.RelativeOrAbsolute));
            appBarCheckButton.Text = "Check";
            ApplicationBar.Buttons.Add(appBarCheckButton);
            appBarCheckButton.Click += appBarCheckButton_Click;

            ApplicationBarIconButton appBarCancelButton =
                new ApplicationBarIconButton(new Uri("/Assets/appbar.cancel.png", UriKind.RelativeOrAbsolute));
            appBarCancelButton.Text = "Cancel";
            ApplicationBar.Buttons.Add(appBarCancelButton);
            appBarCancelButton.Click += appBarCancelButton_Click;
        }

        private void appBarCheckButton_Click(object sender, EventArgs e)
        {
            // Check for erroneous input
            int num;
            if (!int.TryParse(IntBunkCounterTextBox.Text, out num))
            {
                MessageBox.Show("Bunk Counter value must be an integer between 0-99",
                    "Looks like you made a booboo.", MessageBoxButton.OK);
                return;
            }
            if (!int.TryParse(MaxBunksTextBox.Text, out num))
            {
                MessageBox.Show("Bunk Counter value must be an integer between 0-99",
                    "Looks like you made a booboo.", MessageBoxButton.OK);
                return;
            }

            // Remove the old subject
            App.ViewModel.Subjects.Remove(DataContext as Subject);

            // Create a new subject
            Subject newSubject = new Subject();
            newSubject.Name = SubjectNameTextBox.Text;
            newSubject.IntBunkCounter = Convert.ToInt32(IntBunkCounterTextBox.Text);
            newSubject.ShortCode = ShortCodeTextBox.Text;
            newSubject.MaxBunks = Convert.ToInt32(MaxBunksTextBox.Text);
            if (newSubject.IntBunkCounter < 10)
                newSubject.BunkCounter = "0" + Convert.ToString(newSubject.IntBunkCounter);
            else
                newSubject.BunkCounter = Convert.ToString(newSubject.IntBunkCounter);
            newSubject.PercentageBunked = 
                Convert.ToString(Convert.ToInt32((float)newSubject.IntBunkCounter / (float)newSubject.MaxBunks * 100)) + "% Bunked";
            if (Convert.ToInt32((float)newSubject.IntBunkCounter / (float)newSubject.MaxBunks * 100) <= 40)
            {
                newSubject.BunkCounterColor = "Green";
            }
            else if (Convert.ToInt32((float)newSubject.IntBunkCounter / (float)newSubject.MaxBunks * 100) <= 75)
            {
                newSubject.BunkCounterColor = "Orange";
            }
            else
            {
                newSubject.BunkCounterColor = "Red";
            }

            // Add it to the ViewModel
            App.ViewModel.Subjects.Add(newSubject);
            
            // Update the IsolatedStorage
            var data = JsonConvert.SerializeObject(App.ViewModel.Subjects);
            IsolatedStorageSettings.ApplicationSettings[SubjectModel.SubjectsKey] = data;
            IsolatedStorageSettings.ApplicationSettings.Save();

            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }

        private void appBarCancelButton_Click(object sender, EventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            foreach (var item in FormPanel.Children)
            {
                if (item is TextBox)
                {
                    TextBox textBox = item as TextBox;
                    if (textBox.Text == string.Empty)
                    {
                        ApplicationBarIconButton appBarCheckButton = 
                            ApplicationBar.Buttons[0] as ApplicationBarIconButton;
                        appBarCheckButton.IsEnabled = false;
                        return;
                    }
                }
            }
            ApplicationBarIconButton appBarCheckButton1 = 
                ApplicationBar.Buttons[0] as ApplicationBarIconButton;
            appBarCheckButton1.IsEnabled = true;
        }
    }
}