using System.Text.Json;
using WebAppSalesManagement.Models;

namespace WebAppSalesManagement.Services
{
    public class AuthService
    {
        private readonly ApiService _apiService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(ApiService apiService, IHttpContextAccessor httpContextAccessor)
        {
            _apiService = apiService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> Login(string username, string password)
        {
            try
            {
                var loginData = new { NombreUsuario = username, ContraseñaHash = password };
                var response = await _apiService.PostAsync<LoginResponse>("Login", loginData);

                if (!string.IsNullOrEmpty(response.Token))
                {
                    // Guardar el token en una cookie o en la sesión
                    _httpContextAccessor.HttpContext.Session.SetString("AuthToken", response.Token);
                    _httpContextAccessor.HttpContext.Session.SetString("UserInfo", JsonSerializer.Serialize(response.Usuario));

                    // Configurar el token para futuras llamadas a la API
                    _apiService.SetAuthToken(response.Token);

                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public void Logout()
        {
            _httpContextAccessor.HttpContext.Session.Remove("AuthToken");
            _httpContextAccessor.HttpContext.Session.Remove("UserInfo");
        }

        public bool IsAuthenticated()
        {
            return !string.IsNullOrEmpty(_httpContextAccessor.HttpContext.Session.GetString("AuthToken"));
        }

        public Usuario GetCurrentUser()
        {
            var userJson = _httpContextAccessor.HttpContext.Session.GetString("UserInfo");
            if (string.IsNullOrEmpty(userJson))
                return null;

            return JsonSerializer.Deserialize<Usuario>(userJson);
        }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
        public Usuario Usuario { get; set; }
    }
}