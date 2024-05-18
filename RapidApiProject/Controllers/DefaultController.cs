using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RapidApiProject.Models;

namespace RapidApiProject.Controllers
{
    public class DefaultController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GetSearchList(SearchViewModel searchViewModel)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://booking-com15.p.rapidapi.com/api/v1/hotels/searchDestination?query={searchViewModel.cityName}"),
                Headers =
    {
        { "X-RapidAPI-Key", "08d4297573mshf4f79e4627fbc46p18f620jsnf86ecf342d7e" },
        { "X-RapidAPI-Host", "booking-com15.p.rapidapi.com" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                Console.WriteLine(body);
            }

            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<GetCityIdViewModel>(body);
                var cityId = values.data[0].dest_id;
                var getSearch = new SearchViewModel
                {
                    destID = cityId,
                    cityName = searchViewModel.cityName,
                    arrivalDate = searchViewModel.arrivalDate,
                    departureDate = searchViewModel.departureDate,
                    adultCount = searchViewModel.adultCount,
                    roomCount = searchViewModel.roomCount
                };
                return RedirectToAction("HotelList", getSearch);
            }
        }
    }
}
