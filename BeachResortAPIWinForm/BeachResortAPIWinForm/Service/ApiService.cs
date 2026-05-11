using BeachResortAPIWinForm.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Collections.Specialized.BitVector32;

namespace BeachResortAPIWinForm.Service
{
    public class ApiService
    {
        private readonly HttpClient client;

        public ApiService()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5088/api/");
        }

        public async Task<LoginResponse> Login(string username, string password)
        {
            var data = new
            {
                username = username,
                password = password
            };

            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("auth/login", content);

            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<LoginResponse>(result);
        }
        public async Task<bool> ResetPassword(string username, string newPassword)
        {
            var data = new
            {
                username = username,
                password = newPassword
            };

            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("auth/reset", content);

            var result = await response.Content.ReadAsStringAsync();
            MessageBox.Show(result); // debug

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> Register(string username, string password, string role)
        {
            var data = new
            {
                username = username,
                password = password,
                role = role
            };

            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("auth/register", content);

            return response.IsSuccessStatusCode;
        }
        public async Task<List<Booking>> GetAllBookings()
        {
            var response = await client.GetAsync("booking");
            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<Booking>>(json);
        }
        public async Task<bool> AddCottage(string name, decimal price)
        {
            var data = new
            {
                name = name,
                price = price
            };

            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("cottage", content);

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> AddBoat(string name, decimal price)
        {
            var data = new
            {
                name = name,
                price = price
            };

            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("boat", content);

            return response.IsSuccessStatusCode;
        }
        public async Task<List<Cottage>> GetCottages()
        {
            var res = await client.GetAsync("cottage");

            var json = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Cottage>>(json);
        }
        public async Task<List<Boat>> GetBoats()
        {
            var res = await client.GetAsync("boat");

            var json = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Boat>>(json);
        }
        public async Task<bool> UpdateCottage(int id, string name, decimal price)
        {
            var data = new { name, price };

            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var res = await client.PutAsync($"cottage/{id}", content);

            return res.IsSuccessStatusCode;
        }
        public async Task<bool> UpdateBoat(int id, string name, decimal price)
        {
            var data = new { name, price };

            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var res = await client.PutAsync($"boat/{id}", content);

            return res.IsSuccessStatusCode;
        }
        public async Task<List<ChartData>> GetRevenueChart()
        {
            var res = await client.GetAsync("booking/revenue-daily");

            var json = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ChartData>>(json);
        }

        public async Task<List<ChartData>> GetBookingsChart()
        {
            var res = await client.GetAsync("booking/bookings-daily");

            var json = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ChartData>>(json);
        }
        public async Task<List<Booking>> GetBookings()
        {
            var res = await client.GetAsync("booking");
            var json = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Booking>>(json);
        }
        public async Task<List<Cottage>> GetCottage()
        {
            var res = await client.GetAsync("cottage");
            var json = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Cottage>>(json);
        }

        public async Task<List<Boat>> GetBoat()
        {
            var res = await client.GetAsync("boat");
            var json = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Boat>>(json);
        }
        public async Task<bool> CreateBooking(object data)
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("booking", content);

            var result = await response.Content.ReadAsStringAsync();
            MessageBox.Show(result); // 🔥 DEBUG

            return response.IsSuccessStatusCode;
        }
        public async Task<List<Booking>> GetBookingById(int id)
        {
            var res = await client.GetAsync($"booking/{id}");
            var json = await res.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<Booking>>(json);
        }
        public async Task<bool> UpdateStatus(int id)
        {
            var res = await client.PutAsync($"booking/status/{id}",
                new StringContent(
                    JsonConvert.SerializeObject(new { status = "Checked-in" }),
                    Encoding.UTF8,
                    "application/json"
                )
            );

            return res.IsSuccessStatusCode;
        }
        public async Task<bool> Checkout(int id)
        {
            var response = await client.PutAsync($"booking/status/{id}",
                new StringContent(
                    Newtonsoft.Json.JsonConvert.SerializeObject(new { status = "Checked-out" }),
                    Encoding.UTF8,
                    "application/json"
                )
            );

            return response.IsSuccessStatusCode;
        }
        public async Task<List<BookingItem>> GetBookingItems()
        {
            var res = await client.GetAsync("booking/booking-items");
            var json = await res.Content.ReadAsStringAsync();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<List<BookingItem>>(json);
        }
        public async Task<bool> DeleteBooking(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"booking/{id}");

            request.Headers.Add("role", Session.Role); // 🔥 send role

            var response = await client.SendAsync(request);

            return response.IsSuccessStatusCode;
        }
    }
}