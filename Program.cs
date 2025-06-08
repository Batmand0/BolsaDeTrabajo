using BolsaDeTrabajo;
using BolsaDeTrabajo.Servicios;
using DinkToPdf.Contracts;
using DinkToPdf;

var builder = WebApplication.CreateBuilder(args);

// Crear el contexto de carga personalizado
var context = new CustomAssemblyLoadContext();

// Cargar la DLL que necesitas (asegúrate de que 'libwkhtmltox.dll' esté en 'wwwroot/lib')
context.LoadNativeLibrary("libwkhtmltox.dll");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddTransient<IRepositorioDepartamento, RepositorioDepartamento>();
builder.Services.AddTransient<IRepositorioDocumento, RepositorioDocumento>();
builder.Services.AddTransient<IRepositorioEscolaridad, RepositorioEscolaridad>();
builder.Services.AddTransient<IRepositorioEstadoCivil, RepositorioEstadoCivil>();
builder.Services.AddTransient<IRepositorioExperiencia, RepositorioExperiencia>();
builder.Services.AddTransient<IRepositorioNacionalidad, RepositorioNacionalidad>();
builder.Services.AddTransient<IRepositorioPerfil, RepositorioPerfil>();
builder.Services.AddTransient<IRepositorioPuestos, RepositorioPuestos>();
builder.Services.AddTransient<IRepositorioSexo, RepositorioSexo>();
builder.Services.AddTransient<IRepositorioCurso, RepositorioCurso>();
builder.Services.AddTransient<IRepositorioHorario, RepositorioHorario>();
builder.Services.AddTransient<IRepositorioMantenimientoBolsa, RepositorioMantenimientoBolsa>();
builder.Services.AddTransient<IRepositorioMantenimientoEmpleado, RepositorioMantenimientoEmpleado>();
builder.Services.AddTransient<IRepositorioMantenimientoRestringido, RepositorioMantenimientoRestringido>();
// Registrar el servicio de DinkToPdf
builder.Services.AddSingleton<IConverter, SynchronizedConverter>(provider =>
    new SynchronizedConverter(new PdfTools()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
