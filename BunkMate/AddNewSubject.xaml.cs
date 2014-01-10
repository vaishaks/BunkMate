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
    public partial class AddNewSubject : PhoneApplicationPage
    {
        public AddNewSubject()
        {
            InitializeComponent();

            BuildApplicationBar();
        }

        private void BuildApplicationBar()
        {
            ApplicationBar = new ApplicationBar();

            ApplicationBarIconButton appBarCheckButton =
                new ApplicationBarIconButton(new Uri("/Assets/appbar.check.png", UriKind.RelativeOrAbsolute));
            appBarCheckButton.Text = "Check";
            appBarCheckButton.IsEnabled = false;
            ApplicationBar.Buttons.Add(appBarCheckButton);
            appBarCheckButton.Click += appBarCheckButton_Click;

            ApplicationBarIconButton appBarCancelButton =
                new ApplicationBarIconButton(new Uri("/Assets/appbar.cancel.png", UriKind.RelativeOrAbsolute));
            appBarCancelButton.Text = "Cancel";
            ApplicationBar.Buttons.Add(appBarCancelButton);
            appBarCancelButton.Click += appBarCancelButton_Click;
        }

        void appBarCancelButton_Click(object sender, EventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }

        void appBarCheckButton_Click(object sender, EventArgs e)
        {
            // Check for erroneous input and display appropriate error messages
            int num;
            if (!int.TryParse(MaxBunksTextbox.Text, out num))
            {
                MessageBox.Show("Maximum bunks must be an integer value between 0-99.",
                    "Looks like you made a booboo.", MessageBoxButton.OK);
                return;
            }

            Subject subject = new Subject();
            subject.Name = SubjectNameTextbox.Text;
            subject.ShortCode = SubjectCodeTextbox.Text;
            subject.MaxBunks = Convert.ToInt32(MaxBunksTextbox.Text);

            // Add the subject data to local storage
            App.ViewModel.Subjects.Add(subject);
            var data = JsonConvert.SerializeObject(App.ViewModel.Subjects);
            IsolatedStorageSettings.ApplicationSettings[SubjectModel.SubjectsKey] = data;
            IsolatedStorageSettings.ApplicationSettings.Save();

            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }

        private void Textbox_LostFocus(object sender, RoutedEventArgs e)
        {
            // Enable the check button in the AppBar only if all the TextBoxes are filled
            foreach (var item in FormPanel.Children)
            {
                if (item is PhoneTextBox)
                {
                    PhoneTextBox phoneTextBox = item as PhoneTextBox;
                    if (phoneTextBox.Text == string.Empty)
                        return;
                }
            }
            ApplicationBarIconButton appBarCheckButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
            appBarCheckButton.IsEnabled = true;
        }
    }
}