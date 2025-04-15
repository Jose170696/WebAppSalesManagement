using System.Text.Json;
using System.Text;
using WebAppSalesManagement.Controllers;
using WebAppSalesManagement.Models;

namespace WebAppSalesManagement.Services
{
    public class ClienteApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiEndpoint = "api/cliente";

        public ClienteApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ClienteViewModel>> ObtenerClientesAsync()
        {
            var response = await _httpClient.GetAsync(_apiEndpoint);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<ClienteViewModel>>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<ClienteViewModel> ObtenerClienteAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiEndpoint}/{id}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ClienteViewModel>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<(bool Success, string Message)> CrearClienteAsync(ClienteViewModel cliente)
        {
            try
            {
                var clienteRequest = new
                {
                    Nombre = cliente.Nombre,
                    Correo = cliente.Correo,
                    Telefono = cliente.Telefono,
                    Pais = cliente.Pais,
                    Provincia = cliente.Provincia,
                    Canton = cliente.Canton,
                    Distrito = cliente.Distrito,
                    AdicionadoPor = HomeController.userNombre
                };

                var jsonContent = JsonSerializer.Serialize(clienteRequest);
                Console.WriteLine($"Enviando a API: {jsonContent}");

                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(_apiEndpoint, content);
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Respuesta API: {response.StatusCode} - {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    return (true, "Cliente creado correctamente");
                }
                else
                {
                    return (false, $"Error: {response.StatusCode} - {responseContent}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción: {ex.Message}");
                return (false, $"Excepción: {ex.Message}");
            }
        }

        public async Task<bool> ActualizarClienteAsync(int id, ClienteViewModel cliente)
        {
            try
            {
                var clienteRequest = new
                {
                    Nombre = cliente.Nombre,
                    Correo = cliente.Correo,
                    Telefono = cliente.Telefono,
                    Pais = cliente.Pais,
                    Provincia = cliente.Provincia,
                    Canton = cliente.Canton,
                    Distrito = cliente.Distrito,
                    ModificadoPor = HomeController.userNombre
                };

                var jsonContent = JsonSerializer.Serialize(clienteRequest);
                Console.WriteLine($"Actualizando: {jsonContent}");

                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{_apiEndpoint}/{id}", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Actualización - Respuesta API: {response.StatusCode} - {responseContent}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción en actualización: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> EliminarClienteAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_apiEndpoint}/{id}");
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Eliminación - Respuesta API: {response.StatusCode} - {responseContent}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción en eliminación: {ex.Message}");
                return false;
            }
        }
    }
}