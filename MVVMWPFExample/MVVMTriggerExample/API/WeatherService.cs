using MVVMExample.Models;
using Newtonsoft.Json;
using System.Net.Http;

namespace MVVMExample.API
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;

        #region Constructor
        public WeatherService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new System.Uri("https://api.openweathermap.org/data/2.5/"); // OpenWeatherMap API
        }

        #endregion Constructor

        public async Task<WeatherInfo> GetWeatherAsync(string location)
        {
            var apiKey = "5c1f500ba4064ab3c70e080f6666be2d";
            var response = await _httpClient.GetAsync($"weather?q={location}&appid={apiKey}&units=imperial");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var weatherData = JsonConvert.DeserializeObject<WeatherData>(content);
                return new WeatherInfo
                {
                    Location = location,
                    Condition = weatherData.weather[0].description,
                    Temperature = weatherData.main.temp
                };
            }
            else
            {
                // Handle error
                return null;
            }
        }

        #region Nested Classes
        private class WeatherData
        {
            public Weather[] weather { get; set; }
            public Main main { get; set; }
        }

        private class Weather
        {
            public string description { get; set; }
        }

        private class Main
        {
            public double temp { get; set; }
        }

        #endregion Nested Classes
    }
}
