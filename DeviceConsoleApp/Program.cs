using Microsoft.Data.SqlClient;
using Dapper;
using System.Net.Http.Json;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Shared;
using Microsoft.Azure.Devices.Client;
using DeviceConsoleApp.Models;

DeviceClient deviceClient;
DeviceSettings deviceSettings = new DeviceSettings();
//kod från videon med 10 sek interval
//var interval = "";
//var deviceName = "";
//var deviceType = "";
//var location = "";


var connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\lucas\\source\\repos\\LucasKitchenFinalApp\\DeviceConsoleApp\\device_consoleapp_db.mdf;Integrated Security=True;Connect Timeout=30";

using var conn = new SqlConnection(connectionString);


//Hans kod ändra och gör bättre
try
{
    var settings = await conn.QueryFirstOrDefaultAsync<DeviceSettings>("SELECT * FROM Settings");
    if (settings != null)
        deviceSettings = settings;
}
catch { }


if (string.IsNullOrEmpty(deviceSettings.DeviceId))
{
    deviceSettings.DeviceId = Guid.NewGuid().ToString();

    Console.Write("Enter A Device Name You Want: ");
    deviceSettings.DeviceName = Console.ReadLine();
    Console.Write("Enter Device Type (Fan, Tv, Lamp): ");
    deviceSettings.DeviceType = Console.ReadLine();
    Console.Write("Enter Location: (Kitchen, Bedroom)");
    deviceSettings.Location = Console.ReadLine();

    await conn.ExecuteAsync("INSERT INTO Settings VALUES (@DeviceId, @ConnectionString, @DeviceName, @DeviceType, @Location)", deviceSettings);
}

if (string.IsNullOrEmpty(deviceSettings.ConnectionString))
{
    using var client = new HttpClient();
    var result = await client.PostAsJsonAsync("http://localhost:7003/api/devices/connect", deviceSettings);
    deviceSettings.ConnectionString = await result.Content.ReadAsStringAsync();
    await conn.ExecuteAsync("UPDATE Settings SET ConnectionString = @ConnectionString WHERE DeviceId = @DeviceId", deviceSettings);
}
//Mqtt svar från internet: MQTT uses your existing Internet home network to send messages to your IoT devices and respond to those messages. MQTT (Message Queuing Telemetry Transport) is a publish/subscribe messaging protocol that works on top of the TCP/IP protocol.
deviceClient = DeviceClient.CreateFromConnectionString(deviceSettings.ConnectionString, Microsoft.Azure.Devices.Client.TransportType.Mqtt);
var twin = await deviceClient.GetTwinAsync();
try { deviceSettings.Interval = (int)twin.Properties.Desired["interval"]; } catch { }

var twinCollection = new TwinCollection();
twinCollection["deviceName"] = deviceSettings.DeviceName;
twinCollection["deviceType"] = deviceSettings.DeviceType;
twinCollection["location"] = deviceSettings.Location;
await deviceClient.UpdateReportedPropertiesAsync(twinCollection);

await deviceClient.SetMethodHandlerAsync("StartStop", StartStopAsync, null);

Task<MethodResponse> StartStopAsync(MethodRequest methodRequest, object userContext)
{
    Console.WriteLine($"Method {methodRequest.Name} has been triggred.");
    return Task.FromResult(new MethodResponse(new byte[0], 200));
}

Console.ReadKey();
