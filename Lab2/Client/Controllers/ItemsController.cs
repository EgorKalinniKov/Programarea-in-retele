using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Client.Models;

namespace Client.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public ItemsController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("MyApiClient");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> Get()
        {
            var response = await _httpClient.GetAsync("api/items"); // Полный URL
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var items = JsonSerializer.Deserialize<List<Item>>(content, 
                new JsonSerializerOptions 
                { 
                    PropertyNameCaseInsensitive = true // Если имена свойств в JSON отличаются по регистру
                });
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> Get(int id)
        {
            var response = await _httpClient.GetAsync($"api/items/{id}"); // Полный URL
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound();
            }
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var items = JsonSerializer.Deserialize<List<Item>>(content, 
                new JsonSerializerOptions 
                { 
                    PropertyNameCaseInsensitive = true // Если имена свойств в JSON отличаются по регистру
                });
            return Ok(items);
        }

        [HttpPost]
        public async Task<ActionResult<Item>> Post([FromBody] Item item)
        {
            var json = JsonSerializer.Serialize(item);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/items", content); // Полный URL
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var items = JsonSerializer.Deserialize<List<Item>>(responseContent, 
                new JsonSerializerOptions 
                { 
                    PropertyNameCaseInsensitive = true // Если имена свойств в JSON отличаются по регистру
                });
            return Ok(items);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Item item)
        {
            var json = JsonSerializer.Serialize(item);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/items/{id}", content); // Полный URL
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound();
            }
            response.EnsureSuccessStatusCode();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/items/{id}"); // Полный URL
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound();
            }
            response.EnsureSuccessStatusCode();
            return NoContent();
        }
    }
}