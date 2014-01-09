using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BunkMate.ViewModels
{
    public class SubjectModel : INotifyPropertyChanged
    {
        public List<Subject> Subjects
        {
            get;
            set;
        }
        public bool IsDataLoaded { get; private set; }
        public const string SubjectsKey = "subjects";

        public SubjectModel()
        {
            Subjects = new List<Subject>();
            IsDataLoaded = false;
        }

        public void LoadData()
        {
            // Load data
            Subjects = LoadSavedSubjects();

            IsDataLoaded = true;
        }

        private List<Subject> LoadSavedSubjects()
        {
            // Loads saved data from a JSON file in IsolatedStorage
            List<Subject> data;
            string dataFromAppSettings;
            if (IsolatedStorageSettings.ApplicationSettings.TryGetValue(SubjectsKey, out dataFromAppSettings))
                data = JsonConvert.DeserializeObject<List<Subject>>(dataFromAppSettings);
            else
                data = new List<Subject>();
            return data;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
