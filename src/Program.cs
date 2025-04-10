using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using RpaConsultaAtivosB3.src.Utils;

//Configuração do logger
// Essa configuração é feita antes de criar o Host, para que o logger seja utilizado em toda a aplicação.
Logger.CreateLogger();

// Criação do Host
// O Host é responsável por gerenciar a aplicação, incluindo a configuração de serviços e o 
//ciclo de vida da aplicação.
var builder = Host.CreateDefaultBuilder(args)
    .UseSerilog() // usa Serilog como logger global
    .ConfigureServices((context, services) =>
    {
        //configuração dos serviços
    });

var app = builder.Build();

//Execução aqui

app.Run();
