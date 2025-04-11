using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebAppSalesManagement.Models;
using System;
using System.Collections.Generic;
using WebAppSalesManagement.Controllers;

namespace WebAppSalesManagement.Services
{
    public class ProductoApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiEndpoint = "api/producto";

        public ProductoApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ProductoViewModel>> ObtenerProductosAsync()
        {
            var response = await _httpClient.GetAsync(_apiEndpoint);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<ProductoViewModel>>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<ProductoViewModel> ObtenerProductoAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiEndpoint}/{id}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ProductoViewModel>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<(bool Success, string Message)> CrearProductoAsync(ProductoViewModel producto)
        {
            try
            {
                var productoRequest = new
                {
                    Nombre = producto.Nombre,
                    Precio = producto.Precio,
                    Stock = producto.Stock,
                    AdicionadoPor = HomeController.userNombre,
                };

                var jsonContent = JsonSerializer.Serialize(productoRequest);
                Console.WriteLine($"Enviando a API: {jsonContent}");

                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(_apiEndpoint, content);
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Respuesta API: {response.StatusCode} - {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    return (true, "Producto creado correctamente");
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

        public async Task<bool> ActualizarProductoAsync(int id, ProductoViewModel producto)
        {
            try
            {
                var productoRequest = new
                {
                    Nombre = producto.Nombre,
                    Precio = producto.Precio,
                    Stock = producto.Stock,
                    ModificadoPor = HomeController.userNombre
                };

                var jsonContent = JsonSerializer.Serialize(productoRequest);
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

        public async Task<bool> EliminarProductoAsync(int id)
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
