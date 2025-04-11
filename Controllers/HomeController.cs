using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebAppSalesManagement.Models;
using WebAppSalesManagement.Services;

namespace WebAppSalesManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AuthService _authService;
        public static string userNombre;

        // Constructor que inyecta el servicio de autenticaci�n
        public HomeController(ILogger<HomeController> logger, AuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        // Acci�n GET para la p�gina de inicio
        public IActionResult Index()
        {
            return View();
        }

        // Acci�n GET para la p�gina de Login
        public IActionResult Login()
        {
            return View();
        }

        // Acci�n POST para manejar el login de los usuarios
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Llamamos al servicio de autenticaci�n para validar las credenciales
                    var isAuthenticated = await _authService.AuthenticateAsync(model);

                    if (isAuthenticated)
                    {
                        _logger.LogInformation($"Usuario autenticado: {model.NombreUsuario}");
                        TempData["MensajeBienvenida"] = "�Bienvenido/a al sistema!";
                        userNombre = model.NombreUsuario;
                        return RedirectToAction("Index");
                       
                    }
                    else
                    {
                        _logger.LogWarning($"Intento de inicio de sesi�n fallido para el nombre de usuario: {model.NombreUsuario}");
                        ViewBag.ErrorMessage = "Nombre de usuario o contrase�a incorrectos.";
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error al autenticar usuario: {ex.Message}");
                    ViewBag.ErrorMessage = "Ocurri� un problema al iniciar sesi�n. Int�ntalo de nuevo.";
                }
            }

            return View(model);
        }

        // Acci�n de error para manejar problemas de la aplicaci�n
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}