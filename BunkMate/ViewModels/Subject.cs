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
    public class Subject : INotifyPropertyChanged
    {
        private int _intBunkCounter;
        private string _bunkCounter;
        private string _bunkCounterColor;
        private string _percentageBunked;

        public string Name { get; set; }
        public string ShortCode { get; set; }
        public int MaxBunks { get; set; }
        public int IntBunkCounter
        {
            get
            {
                return _intBunkCounter;
            }
            set
            {
                if (value != _intBunkCounter)
                {
                    _intBunkCounter = value;
                    NotifyPropertyChanged("IntBunkCounter");
                }
            }
        }
        public string BunkCounter 
        { 
            get
            {
                return _bunkCounter;
            }
            set
            {
                if (value != _bunkCounter)
                {
                    _bunkCounter = value;
                    NotifyPropertyChanged("BunkCounter");   
                }                
            }
        }
        public string BunkCounterColor 
        {
            get
            {
                return _bunkCounterColor;
            }
            set
            {
                if (value != _bunkCounterColor)
                {
                    _bunkCounterColor = value;
                    NotifyPropertyChanged("BunkCounterColor");
                }
            }
        }
        public string PercentageBunked 
        {
            get
            {
                return _percentageBunked;
            }
            set
            {
                if (value != _percentageBunked)
                {
                    _percentageBunked = value;
                    NotifyPropertyChanged("PercentageBunked");
                }
            }
        }

        public Subject()
        {
            _intBunkCounter = 0;
            BunkCounter = "00";
            BunkCounterColor = "Green";
            PercentageBunked = "0"+"% Bunked";
        }

        public void IncrementBunkCounter()
        {
            _intBunkCounter++;
            if (_intBunkCounter < 10)
                BunkCounter = "0" + Convert.ToString(_intBunkCounter);
            else
                BunkCounter = Convert.ToString(_intBunkCounter);
            PercentageBunked = Convert.ToString(
                Convert.ToInt32((float)_intBunkCounter / (float)MaxBunks * 100)) + "% Bunked";
            if (Convert.ToInt32((float)_intBunkCounter / (float)MaxBunks * 100) <= 40)
            {
                BunkCounterColor = "Green";
            }
            else if (Convert.ToInt32((float)_intBunkCounter / (float)MaxBunks * 100) <= 75)
            {
                BunkCounterColor = "Orange";
            }
            else
            {
                BunkCounterColor = "Red";
            }

            // Save the incremented data back into IsolatedStorage
            App.ViewModel.SaveModel();
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
