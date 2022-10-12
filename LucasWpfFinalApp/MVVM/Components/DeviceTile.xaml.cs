using LucasWpfFinalApp.MVVM.Models;
using Microsoft.Azure.Devices;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LucasWpfFinalApp.MVVM.Components
{
    /// <summary>
    /// Interaction logic for DeviceTile.xaml
    /// </summary>
    public partial class DeviceTile : UserControl, INotifyPropertyChanged
    {
        public DeviceTile()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }








        public static readonly DependencyProperty DeviceNameProperty = DependencyProperty.Register("DeviceName", typeof(string), typeof(DeviceTile));
        public string DeviceName
        {
            get { return (string)GetValue(DeviceNameProperty); }
            set { SetValue(DeviceNameProperty, value); }
        }

        public static readonly DependencyProperty DeviceTypeProperty = DependencyProperty.Register("DeviceType", typeof(string), typeof(DeviceTile));
        public string DeviceType
        {
            get { return (string)GetValue(DeviceTypeProperty); }
            set { SetValue(DeviceTypeProperty, value); }
        }

        public static readonly DependencyProperty DeviceIdProperty = DependencyProperty.Register("DeviceId", typeof(string), typeof(DeviceTile));
        public string DeviceId
        {
            get { return (string)GetValue(DeviceIdProperty); }
            set { SetValue(DeviceIdProperty, value); }
        }

        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register("IsChecked", typeof(bool), typeof(DeviceTile));
        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set
            {
                SetValue(IsCheckedProperty, value);
            }
        }

        public static readonly DependencyProperty IsPressedDeleteProperty = DependencyProperty.Register("IsPressedDelete", typeof(bool), typeof(DeviceTile));
        public bool IsPressedDelete
        {
            get { return (bool)GetValue(IsPressedDeleteProperty); }
            set
            {
                SetValue(IsPressedDeleteProperty, value);
            }
        }

        public static readonly DependencyProperty IconInActiveProperty = DependencyProperty.Register("IconInActive", typeof(string), typeof(DeviceTile));


        public string IconInActive
        {
            get { return (string)GetValue(IconInActiveProperty); }
            set { SetValue(IconInActiveProperty, value); }
        }


        public static readonly DependencyProperty IconActiveProperty = DependencyProperty.Register("IconActive", typeof(string), typeof(DeviceTile));


        public string IconActive
        {
            get { return (string)GetValue(IconActiveProperty); }
            set { SetValue(IconActiveProperty, value); }
        }

        private bool _deviceState;

        public bool DeviceState
        {
            get { return _deviceState; }
            set
            {
                _deviceState = value;
                OnPropertyChanged();
            }
        }

        
        private async void DeviceTile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;
                var deviceItem = (DeviceItem)button!.DataContext;
                deviceItem.DeviceState = !deviceItem.DeviceState;
                IsPressedDelete = deviceItem.DeviceState;

                using ServiceClient serviceClient = ServiceClient.CreateFromConnectionString("HostName=1234goodIoThubname.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=kjaa5RplwIgdxdWsmFKjTzk7GP/9nLmf/9FSt59ruzw=");

                var directMethod = new CloudToDeviceMethod("OnOff");
                directMethod.SetPayloadJson(JsonConvert.SerializeObject(new { deviceState = IsChecked }));
                var result = await serviceClient.InvokeDeviceMethodAsync(deviceItem.DeviceId, directMethod);
            }
            catch { }
        }

        //
        private async void  deviceDeleteSwitch_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                var button = sender as Button;
                var deviceItem = (DeviceItem)button!.DataContext;

                using ServiceClient serviceClient = ServiceClient.CreateFromConnectionString("HostName=1234goodIoThubname.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=kjaa5RplwIgdxdWsmFKjTzk7GP/9nLmf/9FSt59ruzw=");

                var directMethod = new CloudToDeviceMethod("Delete");
                var result = await serviceClient.InvokeDeviceMethodAsync(deviceItem.DeviceId, directMethod);

                if (result.Status == 200) //Vad ska vara här?
                {
                    using var registryManager = RegistryManager.CreateFromConnectionString("HostName=1234goodIoThubname.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=kjaa5RplwIgdxdWsmFKjTzk7GP/9nLmf/9FSt59ruzw=");
                    await registryManager.RemoveDeviceAsync(deviceItem.DeviceId);
                }
            }
            catch { }

            
        }

        private void deviceDeleteSwitch_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
