using LucasWpfFinalApp.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace LucasWpfFinalApp.Services
{
    internal interface IWeatherService
    {
        public Task<WeatherResponse> GetWeatherDataAsync(string uri = "https://api.openweathermap.org/data/2.5/weather?lat=44.34&lon=10.99&appid=bcf8bd2bf6561493badb4ded2a316f53"); //var samma väder i skolan blev kallare nu
    }

    internal class WeatherService : IWeatherService
    {
        public async Task<WeatherResponse> GetWeatherDataAsync(string uri)
        {
            try
            {
                using var client = new HttpClient();
                var response = await client.GetFromJsonAsync<WeatherApiResponse>(uri);
                return new WeatherResponse
                {
                    Temperature = (int)response!.main.temp,
                    Humidity = response.main.humidity,
                    WeatherCondition = response.weather[0].main
                };
            }
            catch { }
            return null!;
        }
    }
}
