using Microsoft.AspNetCore.Mvc;
using WeatherApp_Project.Models;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace WeatherApp_Project.Controllers
{
    public class WeatherController : Controller
    {
        private readonly string apiKey = "1a05541f040946970a0fce0af6ff4170"; //Openweathermap API code
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                ViewBag.Error = "Please Enter The City Name.";
                return View();
            }

            Weather weather = new Weather();

            using (HttpClient client = new HttpClient())
            {
                string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric&lang=tr";

                try
                {
                    var response = await client.GetStringAsync(url);
                    JObject json = JObject.Parse(response);

                    weather.City = json["name"]?.ToString();
                    weather.Description = json["weather"]?[0]?["description"]?.ToString();
                    weather.Temperature = double.TryParse(json["main"]?["temp"]?.ToString(), out double temp) ? (int)temp : 0;

                    var iconCode = json["weather"]?[0]?["icon"]?.ToString();
                    weather.Icon = $"https://openweathermap.org/img/wn/{iconCode}@2x.png";
                }
                catch (HttpRequestException ex)
                {
                    ViewBag.Error = "Please Check City Name You Enter!";
                    return View();
                }
            }

            return View(weather);
        }
    }
}
