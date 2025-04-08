using System.Text;
using System.Text.Json;
using WebAppSalesManagement.Models;

namespace WebAppSalesManagement.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Método para autenticar a un usuario
        public async Task<bool> AuthenticateAsync(LoginViewModel loginModel)
        {
            var json = JsonSerializer.Serialize(loginModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Login", content);

            return response.IsSuccessStatusCode;
        }
    }
}