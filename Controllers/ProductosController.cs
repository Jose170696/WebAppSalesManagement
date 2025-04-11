using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebAppSalesManagement.Models;
using WebAppSalesManagement.Services;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace WebAppSalesManagement.Controllers
{
    public class ProductosController : Controller
    {
        private readonly ProductoApiService _productoApiService;

        public ProductosController(ProductoApiService productoApiService)
        {
            _productoApiService = productoApiService;
        }

        // GET: Productos
        public async Task<IActionResult> Index()
        {
            try
            {
                var productos = await _productoApiService.ObtenerProductosAsync();
                return View(productos);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al obtener productos: {ex.Message}";
                return View(new List<ProductoViewModel>());
            }
        }

        // GET: Productos/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var producto = await _productoApiService.ObtenerProductoAsync(id);
                if (producto == null)
                {
                    return NotFound();
                }
                return View(producto);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al obtener detalles del producto: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Productos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Productos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductoViewModel producto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Asigna el usuario por defecto
                    producto.AdicionadoPor = HomeController.userNombre;
                    Console.WriteLine($"Enviando producto: {JsonSerializer.Serialize(producto)}");

                    var (success, message) = await _productoApiService.CrearProductoAsync(producto);
                    if (success)
                    {
                        TempData["Success"] = "Producto creado correctamente";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["Error"] = $"Error al crear producto: {message}";
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Error al crear producto: {ex.Message}";
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
            return View(producto);
        }

        // GET: Productos/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var producto = await _productoApiService.ObtenerProductoAsync(id);
                if (producto == null)
                {
                    return NotFound();
                }
                return View(producto);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al obtener producto para editar: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Productos/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductoViewModel producto)
        {
            if (id != producto.ProductoID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Asigna el usuario por defecto para modificaciones
                    producto.ModificadoPor = HomeController.userNombre;
                    var result = await _productoApiService.ActualizarProductoAsync(id, producto);
                    if (result)
                    {
                        TempData["Success"] = "Producto actualizado correctamente";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Error al actualizar producto: {ex.Message}";
                }
            }
            return View(producto);
        }

        // GET: Productos/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var producto = await _productoApiService.ObtenerProductoAsync(id);
                if (producto == null)
                {
                    return NotFound();
                }
                return View(producto);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al obtener producto para eliminar: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Productos/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var result = await _productoApiService.EliminarProductoAsync(id);
                if (result)
                {
                    TempData["Success"] = "Producto eliminado correctamente";
                }
                else
                {
                    TempData["Error"] = "No se pudo eliminar el producto";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al eliminar producto: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
