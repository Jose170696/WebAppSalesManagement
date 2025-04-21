using Microsoft.AspNetCore.Mvc;
using WebAppSalesManagement.Models;
using WebAppSalesManagement.Services;

namespace WebAppSalesManagement.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UsuarioApiService _usuarioApiService;

        public UsuariosController(UsuarioApiService usuarioApiService)
        {
            _usuarioApiService = usuarioApiService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var usuarios = await _usuarioApiService.ObtenerUsuariosAsync();
                return View(usuarios);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al obtener usuarios: {ex.Message}";
                return View(new List<Usuario>());
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var usuario = await _usuarioApiService.ObtenerUsuarioAsync(id);
                return usuario == null ? NotFound() : View(usuario);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al obtener detalles del usuario: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                var (success, message) = await _usuarioApiService.CrearUsuarioAsync(usuario);
                if (success)
                {
                    TempData["Success"] = message;
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, message);
            }
            return View(usuario);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var usuario = await _usuarioApiService.ObtenerUsuarioAsync(id);
            return usuario == null ? NotFound() : View(usuario);
        }
    }
}