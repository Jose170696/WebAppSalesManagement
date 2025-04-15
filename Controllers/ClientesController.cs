using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using System.Text.Json;
using WebAppSalesManagement.Models;
using WebAppSalesManagement.Services;

namespace WebAppSalesManagement.Controllers
{
    public class ClientesController : Controller
    {
        private readonly ClienteApiService _clienteApiService;

        public ClientesController(ClienteApiService clienteApiService)
        {
            _clienteApiService = clienteApiService;
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            try
            {
                var clientes = await _clienteApiService.ObtenerClientesAsync();
                return View(clientes);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al obtener clientes: {ex.Message}";
                return View(new List<ClienteViewModel>());
            }
        }

        // GET: Clientes/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var cliente = await _clienteApiService.ObtenerClienteAsync(id);
                if (cliente == null)
                {
                    return NotFound();
                }
                return View(cliente);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al obtener detalles del cliente: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClienteViewModel cliente)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    cliente.AdicionadoPor = HomeController.userNombre;
                    Console.WriteLine($"Enviando cliente: {JsonSerializer.Serialize(cliente)}");

                    var (success, message) = await _clienteApiService.CrearClienteAsync(cliente);
                    if (success)
                    {
                        TempData["Success"] = "Cliente creado correctamente";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["Error"] = $"Error al crear cliente: {message}";
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Error al crear cliente: {ex.Message}";
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage);
                TempData["Error"] = $"Error de validación: {string.Join(", ", errors)}";
                foreach (var entry in ModelState)
                {
                    var key = entry.Key;
                    foreach (var error in entry.Value.Errors)
                    {
                        Console.WriteLine($"Campo: {key}, Error: {error.ErrorMessage}");
                    }
                }
            }
            return View(cliente);
        }

        // GET: Clientes/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var cliente = await _clienteApiService.ObtenerClienteAsync(id);
                if (cliente == null)
                {
                    return NotFound();
                }
                return View(cliente);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al obtener cliente para editar: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Clientes/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClienteViewModel cliente)
        {
            if (id != cliente.ClienteID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    cliente.ModificadoPor = HomeController.userNombre;
                    var result = await _clienteApiService.ActualizarClienteAsync(id, cliente);
                    if (result)
                    {
                        TempData["Success"] = "Cliente actualizado correctamente";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Error al actualizar cliente: {ex.Message}";
                }
            }
            return View(cliente);
        }

        // GET: Clientes/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var cliente = await _clienteApiService.ObtenerClienteAsync(id);
                if (cliente == null)
                {
                    return NotFound();
                }
                return View(cliente);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al obtener cliente para eliminar: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Clientes/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var result = await _clienteApiService.EliminarClienteAsync(id);
                if (result)
                {
                    TempData["Success"] = "Cliente eliminado correctamente";
                }
                else
                {
                    TempData["Error"] = "No se pudo eliminar el cliente";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al eliminar cliente: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }

        // GENERAR REPORTES
        public async Task<IActionResult> GenerarPDF()
        {
            try
            {
                var clientes = await _clienteApiService.ObtenerClientesAsync();
                return new ViewAsPdf("ClienteReport", clientes)
                {
                    FileName = "ReporteClientes.pdf",
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                    PageSize = Rotativa.AspNetCore.Options.Size.A4
                };
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al generar reporte PDF: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

    }
}