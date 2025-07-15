using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ControleDeBar.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeBar.WebApp.Controllers
{
    [Route("api")]
    public class APIController : Controller
    {
        private static readonly string apiKey = "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";

        private const string apiUrl = "https://api.openai.com/v1/chat/completions";

        private static readonly HttpClient client = new HttpClient();

       
        [HttpGet("texto-com-gpt/{pergunta}")]
        public async Task<IActionResult> ItensChatGpt([FromRoute] string pergunta)
        {
            try
            {
                string[] lista = await ObterRespostaChatGpt(pergunta);
                ItensChatGptViewModel itensVM = new ItensChatGptViewModel
                {
                    Lista = lista
                };
                return View(itensVM);           
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro: {ex.Message}");
            }
        }

      
        private static async Task<string[]> ObterRespostaChatGpt(string pergunta)
        {
           
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var requestBody = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
                    new { role = "system",
                          content = "Responda em linhas separadas, sem numeração." },
                    new { role = "user", content = pergunta }
                },
                max_tokens = 200
            };

            string json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(apiUrl, content);
            string body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"{response.StatusCode}: {body}");

            using JsonDocument doc = JsonDocument.Parse(body);
            string resposta = doc.RootElement.GetProperty("choices")[0]
                                        .GetProperty("message")
                                        .GetProperty("content")
                                        .GetString();

            var itens = resposta.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                           .Select(l => l.TrimStart(' ', '-', '*', '•')
                                         .Trim())
                           .ToArray();

            return itens;
        }
    }
}
