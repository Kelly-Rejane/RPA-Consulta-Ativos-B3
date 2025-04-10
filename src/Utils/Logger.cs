using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace RpaConsultaAtivosB3.src.Utils
{
    // Essa classe é responsável por criar o logger que será utilizado em toda a aplicação.
    // Ela utiliza a biblioteca Serilog para criar logs em console e em arquivo.
    public class Logger 
    {
        public static void CreateLogger()
        {
            var logPath = Path.Combine(AppContext.BaseDirectory, "Logs", "logs", "log.txt");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Console(
                    theme: AnsiConsoleTheme.Code,
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(
                    path: logPath,
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7, // mantém os últimos 7 dias
                    outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();
        }
    }
}