using LucasWpfFinalApp.Helpers;
using LucasWpfFinalApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LucasWpfFinalApp.MVVM.ViewModels
{
    internal class BaseViewModel : Timers, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;
        //public NavigationStore KitchenViewCommand { get; set; }
        //public KitchenViewModel KitchenViewModel { get; set; }
        //public NavigationStore BedroomViewCommand { get; set; }
        //public BedroomViewModel BedroomViewModel { get; set; }
        //public NavigationStore LivingroomViewCommand { get; set; }
        //public LivingroomViewModel LivingroomViewModel { get; set; }

        protected void OnPropertyChanged([CallerMemberName] string name = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
