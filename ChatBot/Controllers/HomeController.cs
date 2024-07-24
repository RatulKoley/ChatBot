using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace ChatBot.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var answer = TempData["Answer"] as string;
            ViewBag.Answer = answer;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(string name, string query)
        {
            using var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.pawan.krd/cosmosrp/v1/chat/completions");
            var requestBody = new
            {
                messages = new[] {
                new
                {
                    role = "user",
                    content = query
                }}
            };
            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            request.Content = content;

            HttpResponseMessage response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var jsonResponse = JObject.Parse(responseBody);
            var messageContent = jsonResponse["choices"]?[0]["message"]?["content"]?.ToString();
            TempData["Answer"] = messageContent;
            return RedirectToAction("Index");
        }
        public IActionResult GroupChat()
        {
            return View();
        }

    }
}
