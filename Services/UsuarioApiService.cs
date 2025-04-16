using System.Text.Json;
using System.Text;
using WebAppSalesManagement.Models;

namespace WebAppSalesManagement.Services
{
    public class UsuarioApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiEndpoint = "api/usuario";

        public UsuarioApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Usuario>> ObtenerUsuariosAsync()
        {
            var response = await _httpClient.GetAsync(_apiEndpoint);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Usuario>>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<Usuario> ObtenerUsuarioAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiEndpoint}/{id}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Usuario>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<(bool Success, string Message)> CrearUsuarioAsync(Usuario usuario)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(usuario);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(_apiEndpoint, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                return response.IsSuccessStatusCode
                    ? (true, "Usuario creado correctamente")
                    : (false, $"Error: {response.StatusCode} - {responseContent}");
            }
            catch (Exception ex)
            {
                return (false, $"Excepción: {ex.Message}");
            }
        }
    }
}