using System.Globalization;
using WebAppSalesManagement.Services;

var builder = WebApplication.CreateBuilder(args);

// Configurar la cultura predeterminada para 'es-ES'
// Esto permitir� que los n�meros decimales se interpreten con coma en las vistas
CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("es-ES");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("es-ES");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession(); // Habilita el servicio de sesi�n

// Configuraci�n del servicio de autenticaci�n
builder.Services.AddHttpClient<AuthService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7109/");
});

// Registro del servicio de productos
builder.Services.AddHttpClient<ProductoApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7109/");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}");

app.Run();
