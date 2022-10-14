using LucasWpfFinalApp.MVVM.Models;
using Microsoft.Azure.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace LucasWpfFinalApp.MVVM.Views
{
    /// <summary>
    /// Interaction logic for LivingroomView.xaml
    /// </summary>
    public partial class LivingroomView : UserControl
    {
        public LivingroomView()
        {
            InitializeComponent();
        }


        private async void btn_DirectMethod_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;
                var deviceItem = (DeviceItem)button!.DataContext;
                using ServiceClient serviceClient = ServiceClient.CreateFromConnectionString("HostName=1234goodIoThubname.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=kjaa5RplwIgdxdWsmFKjTzk7GP/9nLmf/9FSt59ruzw=");

                var directMethod = new CloudToDeviceMethod("OnOff");

                //vad gör den?
                //deviceMethod.SetPayloadJson(JsonConvert.SerializeObject(new { interval = 50000 }));

                var result = await serviceClient.InvokeDeviceMethodAsync(deviceItem.DeviceId, directMethod);
            }
            catch { }

        }
    }
}
