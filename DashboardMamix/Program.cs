using DashboardPekus.Components;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// ============================================================
// 1. REGISTRO DE SERVIÇOS (Tudo que começa com builder.Services)
// ============================================================

builder.Services.AddMudServices();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configuração de Localização (i18n)
builder.Services.AddLocalization();
builder.Services.AddControllers();
builder.Services.AddScoped(sp =>
    new HttpClient { BaseAddress = new Uri("http://localhost/") });

// Configuração da Licença do QuestPDF (Pode ficar aqui ou na RN)
QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;


var app = builder.Build(); // <--- A partir daqui, builder.Services é READ-ONLY


// ============================================================
// 2. CONFIGURAÇÃO DO PIPELINE (Middlewares e App.Use)
// ============================================================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.MapControllers();

// --- Configuração das Culturas (Idiomas) ---
var supportedCultures = new[] { "pt-BR", "en-US" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

// O Middleware de localização deve vir ANTES do MapRazorComponents
app.UseRequestLocalization(localizationOptions);

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();