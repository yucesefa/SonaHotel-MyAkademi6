﻿using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GetSearchList(HotelSearchViewModel searchViewModel)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://booking-com15.p.rapidapi.com/api/v1/hotels/searchDestination?query={searchViewModel.cityName}"),
                Headers =
    {
        { "X-RapidAPI-Key", "be45a2560emsh29ab76b46954109p1c5419jsna95fb80cd475" },
        { "X-RapidAPI-Host", "booking-com15.p.rapidapi.com" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<SearchHotelDestination>(body);
                var cityId = values.data[0].dest_id;
                var getSearch = new HotelSearchViewModel
                {
                    destId = cityId,
                    cityName = searchViewModel.cityName,
                    arrivalDate = searchViewModel.arrivalDate,
                    departureDate = searchViewModel.departureDate,
                    adultCount = searchViewModel.adultCount,
                    roomCount = searchViewModel.roomCount
                };
                return RedirectToAction("GetHotelList", getSearch);
            }
        }
        public async Task<IActionResult> GetHotelList(HotelSearchViewModel searchViewModel)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://booking-com15.p.rapidapi.com/api/v1/hotels/searchHotels?dest_id={searchViewModel.destId}&search_type=CITY&arrival_date={searchViewModel.arrivalDate.ToString("yyyy-MM-dd")}&departure_date={searchViewModel.departureDate.ToString("yyyy-MM-dd")}&adults={searchViewModel.adultCount}&room_qty={searchViewModel.roomCount}&page_number=1&languagecode=en-us&currency_code=EUR"),
                Headers =
    {
        { "X-RapidAPI-Key", "be45a2560emsh29ab76b46954109p1c5419jsna95fb80cd475" },
        { "X-RapidAPI-Host", "booking-com15.p.rapidapi.com" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<HotelListViewModel>(body);
                TempData["Photo"] = values.data.hotels[0].property.photoUrls[0].Replace("square60", "square480");
                return View(values.data.hotels.ToList());
            }
        }
        [HttpPost]
        public async Task<IActionResult> GetHotelDetails(string hotelId, string departureDate, string arrivalDate)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://booking-com15.p.rapidapi.com/api/v1/hotels/getHotelDetails?hotel_id=" + hotelId + "&arrival_date=" + arrivalDate + "&departure_date=" + departureDate + "&languagecode=en-us&currency_code=EUR"),
                Headers =
                {
                        {
                        "X-RapidAPI-Key",
                       "be45a2560emsh29ab76b46954109p1c5419jsna95fb80cd475"
                        },
                        {
                        "X-RapidAPI-Host",
                        "booking-com15.p.rapidapi.com"
                        },
                },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<HotelDetailsViewModel>(body);
                if(values.data !=null)
                {
                    return View(values.data);
                }
            }
            return View();
        }
    }
}
