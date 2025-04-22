using System;
using System.Text;
using System.Text.Json;
using WebAppSalesManagement.Models;

namespace WebAppSalesManagement.Services
{
    public class VentaApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiEndpoint = "api/venta";
        private readonly ILogger<VentaApiService> _logger;

        private JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        public VentaApiService(HttpClient httpClient, IConfiguration configuration, ILogger<VentaApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        // Método para obtener todas las ventas
        public async Task<List<VentaCompleta>> ObtenerVentasAsync()
        {
            var response = await _httpClient.GetAsync(_apiEndpoint);
            response.EnsureSuccessStatusCode();
            return await response.Content
                .ReadFromJsonAsync<List<VentaCompleta>>(_jsonOptions)
                ?? new List<VentaCompleta>();
        }

        // Método para obtener una venta específica
        public async Task<VentaCompleta> ObtenerVentaAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiEndpoint}/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content
                .ReadFromJsonAsync<VentaCompleta>(_jsonOptions);
        }

        // Método para crear una venta
        public async Task<int> CrearVentaAsync(VentaConDetalles venta)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(venta, options),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync(_apiEndpoint, content);

                // Leer el contenido de la respuesta en caso de error
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Error al crear venta. Status: {response.StatusCode}, Detalle: {errorContent}");
                    throw new Exception($"Error al crear venta: {response.StatusCode}. Detalles: {errorContent}");
                }

                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();
                return int.Parse(responseString);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear venta");
                throw new Exception($"Error al crear venta: {ex.Message}", ex);
            }
        }

        // Método para actualizar una venta
        public async Task ActualizarVentaAsync(int id, VentaConDetalles venta)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(venta, options),
                    Encoding.UTF8,
                    "application/json");

                _logger.LogInformation($"Enviando actualización para venta ID {id}: {JsonSerializer.Serialize(venta)}");

                var response = await _httpClient.PutAsync($"{_apiEndpoint}/{id}", content);

                // Leer el contenido de la respuesta en caso de error
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Error al actualizar venta {id}. Status: {response.StatusCode}, Detalle: {errorContent}");
                    throw new Exception($"Error al actualizar venta: {response.StatusCode}. Detalles: {errorContent}");
                }

                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar venta con ID {id}");
                throw new Exception($"Error al actualizar venta: {ex.Message}", ex);
            }
        }

        // Método para eliminar una venta
        public async Task EliminarVentaAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Eliminando venta ID {id}");

                var response = await _httpClient.DeleteAsync($"{_apiEndpoint}/{id}");

                // Leer el contenido de la respuesta en caso de error
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Error al eliminar venta {id}. Status: {response.StatusCode}, Detalle: {errorContent}");
                    throw new Exception($"Error al eliminar venta: {response.StatusCode}. Detalles: {errorContent}");
                }

                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar venta con ID {id}");
                throw new Exception($"Error al eliminar venta: {ex.Message}", ex);
            }
        }
    }
}


