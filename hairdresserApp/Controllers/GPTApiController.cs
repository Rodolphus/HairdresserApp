using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;

namespace HairdresserApp.Controllers
{
    [Route("api/gpt")]
    public class GPTApiController : Controller
    {
        private const string ApiUrl = "https://api.openai.com/v1/chat/completions";
        private const string ApiKey = "YOUR_KEY";

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetResult(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ViewBag.Response = "Lütfen bir görsel yükleyin.";
                return View("Index");
            }

            
            string base64Image;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                base64Image = Convert.ToBase64String(memoryStream.ToArray());
            }

            
            string gptResponse;
            try
            {
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ApiKey}");

                var prompt = "İlettiğim görseldeki kişinin yüz şekline ve saç yapısına uygun 3 farklı saç modeli önerisi yap. Cevap olarak 1. ve 2. saç önerisi diye direk cevap ver.";

                var requestBody = new
                {
                    model = "gpt-4o-mini",
                    messages = new[]
                    {
                        new { role = "system", content = "You are a helpful assistant." },
                        new { role = "user", content = $"{base64Image} {prompt}" }
                    },
                    max_tokens = 500
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await httpClient.PostAsync(ApiUrl, content);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                var jsonDoc = JsonDocument.Parse(responseBody);

                gptResponse = jsonDoc.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();
            }
            catch (HttpRequestException ex)
            {
                ViewBag.Response = $"GPT API'ye erişilirken bir hata oluştu: {ex.Message}";
                return View("Index");
            }

            
            ViewBag.ImagePreview = $"data:{file.ContentType};base64,{base64Image}";
            ViewBag.Response = gptResponse;

            return View("Index");
        }

    }

}