using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using RpaConsultaAtivosB3.src.Utils;
using RpaConsultaAtivosB3.src.Services;
using RpaConsultaAtivosB3.src.Orchestrators;
using RpaConsultaAtivosB3.src.Interface;
using RpaConsultaAtivosB3.src.Pages;
using OpenQA.Selenium;
using RpaConsultaAtivosB3.src.Builders;

// Configuração do logger
Logger.CreateLogger();

// Criação do Host para injeção de dependências e configuração do aplicativo
// O Host é responsável por gerenciar o ciclo de vida do aplicativo e suas dependências
var builder = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .UseSerilog()
    .ConfigureServices((context, services) =>
    {
        services.AddScoped<IWebDriver>(provider =>
        {
            return WebDriverBuilder.Create(); // ou headless: true
        });

        services.AddScoped<ExcelService>();
        services.AddScoped<EmailService>();
        services.AddScoped<IConsultaAtivoService, ConsultaAtivoPage>();
        services.AddScoped<AutomationRunnerOrchestrator>();
    });


var app = builder.Build();

try
{
    var configuration = app.Services.GetRequiredService<IConfiguration>();
    string caminhoRelativo = configuration["Caminhos:Excel"];

    Console.WriteLine($"📄 Caminho relativo do Excel: {caminhoRelativo}");

    if (string.IsNullOrWhiteSpace(caminhoRelativo))
        throw new Exception("❌ Caminho do Excel não definido no appsettings.json!");

    // Torna o caminho absoluto baseado no diretório de execução
    string caminhoAbsoluto = Path.GetFullPath(caminhoRelativo, AppDomain.CurrentDomain.BaseDirectory);

    var runner = app.Services.GetRequiredService<AutomationRunnerOrchestrator>();
    Console.WriteLine($"📂 Caminho absoluto: {caminhoAbsoluto}");
    runner.Executar(caminhoAbsoluto);
}
catch (Exception ex)
{
    Console.WriteLine($"Erro: {ex.Message}");
}
