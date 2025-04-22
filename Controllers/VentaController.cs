using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using WebAppSalesManagement.Models;
using WebAppSalesManagement.Services;
using Microsoft.Extensions.Logging;

namespace WebAppSalesManagement.Controllers
{
    public class VentaController : Controller
    {
        private readonly VentaApiService _ventaApiService;
        private readonly ClienteApiService _clienteApiService;
        private readonly ILogger<VentaController> _logger;

        public VentaController(
            VentaApiService ventaApiService,
            ClienteApiService clienteApiService,
            ILogger<VentaController> logger)
        {
            _ventaApiService = ventaApiService;
            _clienteApiService = clienteApiService;
            _logger = logger;
        }

        // GET: Venta
        public async Task<IActionResult> Index()
        {
            try
            {
                var ventas = await _ventaApiService.ObtenerVentasAsync();
                return View(ventas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener ventas");
                TempData["Error"] = $"Error al obtener ventas: {ex.Message}";
                return View(new List<VentaCompleta>());
            }
        }

        // GET: Venta/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                // Obtener la venta completa
                var venta = await _ventaApiService.ObtenerVentaAsync(id);
                if (venta == null) return NotFound();

                // Traer el cliente para obtener el nombre
                var cliente = await _clienteApiService.ObtenerClienteAsync(venta.ClienteID);
                venta.Nombre = cliente?.Nombre;

                
                return View(venta);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener detalles de venta {id}");
                TempData["Error"] = $"Error al obtener detalles de venta: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }


        // GET: Venta/Create
        public async Task<IActionResult> Create()
        {
            await CargarClientesEnViewBag();
            return View();
        }

        // POST: Venta/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VentaConDetalles venta)
        {
            venta.AdicionadoPor = HomeController.userNombre;
            ModelState.Remove(nameof(venta.AdicionadoPor));

            if (!ModelState.IsValid)
            {
                await CargarClientesEnViewBag();
                return View(venta);
            }

            if (venta.Detalles == null || !venta.Detalles.Any())
            {
                ModelState.AddModelError("", "La venta debe tener al menos un detalle.");
                await CargarClientesEnViewBag();
                return View(venta);
            }

            venta.Total = venta.Detalles.Sum(d => d.Subtotal);

            try
            {
                await _ventaApiService.CrearVentaAsync(venta);
                TempData["Success"] = "Venta creada correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear venta");
                ModelState.AddModelError("", $"Error al crear venta: {ex.Message}");
                await CargarClientesEnViewBag();
                return View(venta);
            }
        }

        // GET: Venta/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                _logger.LogInformation($"Obteniendo venta {id} para editar");

                var ventaComp = await _ventaApiService.ObtenerVentaAsync(id);
                if (ventaComp == null) return NotFound();

                var model = new VentaConDetalles
                {
                    ClienteID = ventaComp.ClienteID,
                    Total = ventaComp.Total,
                    AdicionadoPor = ventaComp.AdicionadoPor,
                    Detalles = ventaComp.Detalles
                                         .Select(d => new DetalleVentaItem
                                         {
                                             ProductoID = d.ProductoID,
                                             Cantidad = d.Cantidad,
                                             Subtotal = d.Subtotal
                                         }).ToList()
                };

                await CargarClientesEnViewBag(model.ClienteID);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener venta {id} para editar");
                TempData["Error"] = $"Error al obtener venta para editar: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Venta/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VentaConDetalles venta)
        {
            if (id <= 0)
            {
                TempData["Error"] = "ID de venta inválido.";
                return RedirectToAction(nameof(Index));
            }

            // Se asigna el AdicionadoPor antes de validar
            venta.AdicionadoPor = HomeController.userNombre;
            //Se remueve del ModelState para que no de error la validación
            ModelState.Remove(nameof(venta.AdicionadoPor));

            if (!ModelState.IsValid)
            {
                await CargarClientesEnViewBag(venta.ClienteID);
                return View(venta);
            }

            if (venta.Detalles == null || !venta.Detalles.Any())
            {
                ModelState.AddModelError("", "La venta debe tener al menos un detalle.");
                await CargarClientesEnViewBag(venta.ClienteID);
                return View(venta);
            }

            var totalCalculado = venta.Detalles.Sum(d => d.Subtotal);
            venta.Total = totalCalculado;

            try
            {
                await _ventaApiService.ActualizarVentaAsync(id, venta);
                TempData["Success"] = "Venta actualizada correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar venta {id}");
                ModelState.AddModelError("", $"Error al actualizar venta: {ex.Message}");
                await CargarClientesEnViewBag(venta.ClienteID);
                return View(venta);
            }
        }

        // GET: Venta/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation($"Obteniendo venta {id} para eliminar");

                var venta = await _ventaApiService.ObtenerVentaAsync(id);
                if (venta == null) return NotFound();
                return View(venta);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener venta {id} para eliminar");
                TempData["Error"] = $"Error al obtener venta para eliminar: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Venta/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                _logger.LogInformation($"Eliminando venta {id}");
                await _ventaApiService.EliminarVentaAsync(id);
                TempData["Success"] = "Venta eliminada correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar venta {id}");
                TempData["Error"] = $"Error al eliminar venta: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Venta/GenerarPDF
        public async Task<IActionResult> GenerarPDF()
        {
            try
            {
                var ventas = await _ventaApiService.ObtenerVentasAsync();
                return new ViewAsPdf("VentaReport", ventas)
                {
                    FileName = "ReporteVentas.pdf",
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                    PageSize = Rotativa.AspNetCore.Options.Size.A4
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar reporte PDF de ventas");
                TempData["Error"] = $"Error al generar reporte PDF: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Venta/GenerarPDFVenta/{id}
        public async Task<IActionResult> GenerarPDFVenta(int id)
        {
            try
            {
               // se trae la venta
                var venta = await _ventaApiService.ObtenerVentaAsync(id);
                if (venta == null) return NotFound();

                //Se trae el cliente y se asigna el nombre
                var cliente = await _clienteApiService.ObtenerClienteAsync(venta.ClienteID);
                venta.Nombre = cliente.Nombre;

                // se genera el PDF
                return new ViewAsPdf("VentaDetalleReport", venta)
                {
                    FileName = $"Venta_{id}.pdf",
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                    PageSize = Rotativa.AspNetCore.Options.Size.A4
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al generar reporte PDF de venta {id}");
                TempData["Error"] = $"Error al generar reporte PDF: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }


        /// <summary>
        /// esto carga carga la lista de clientes en ViewBag para el dropdown.
        /// </summary>
        private async Task CargarClientesEnViewBag(int? seleccionado = null)
        {
            var clientes = await _clienteApiService.ObtenerClientesAsync();
            ViewBag.Clientes = new SelectList(
                clientes,
                nameof(ClienteViewModel.ClienteID),
                nameof(ClienteViewModel.Nombre),
                seleccionado);
        }
    }
}
