using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using RpaConsultaAtivosB3.src.Executors;
using RpaConsultaAtivosB3.src.Models;
using RpaConsultaAtivosB3.src.Utils;




namespace RpaConsultaAtivosB3.src.Services
{
    public class ExcelService : IDisposable // Classe Orquestradora
    {
        private readonly string _filePath; // Caminho do arquivo Excel
        private readonly ExcelPackage _package; // Pacote Excel
        private readonly ExcelWorksheet _sheet; // Planilha Excel
        private readonly ILogger<ExcelService> _logger; //Logger para registrar mensagens de log
        private readonly ILoggerFactory _loggerFactory; // Fábrica de loggers

        public ExcelReader Reader { get; } // Leitor de Excel
        public ExcelWriter Writer { get; } // Escritor de Excel

        // Construtor que recebe o caminho do arquivo Excel
        // Inicializa o pacote Excel e a planilha
        public ExcelService(string filePath, ILogger<ExcelService> logger, ILoggerFactory loggerFactory)
        {
            _logger = logger;
            _loggerFactory = loggerFactory;

            // Define o contexto de licença para uso pessoal não comercial
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            _filePath = filePath;
            _package = new ExcelPackage(new FileInfo(_filePath));
            _sheet = _package.Workbook.Worksheets[0];

            Reader = new ExcelReader(_sheet);
            Writer = new ExcelWriter(_sheet, _loggerFactory.CreateLogger<ExcelWriter>());
        }


        public void Salvar() => _package.Save(); // Salva o arquivo Excel


        public static class ExcelServiceFactory
        {
            public static ExcelService Criar(string caminhoOriginal)
            {
                if (!File.Exists(caminhoOriginal))
                    throw new FileNotFoundException("Arquivo Excel não encontrado.", caminhoOriginal);

                var destinationPath = Path.Combine("src", "Excel", "Consulta", Path.GetFileName(caminhoOriginal));
                var dir = Path.GetDirectoryName(destinationPath);
                if (string.IsNullOrEmpty(dir))
                    throw new InvalidOperationException("Diretório de destino inválido.");

                Directory.CreateDirectory(dir);
                File.Copy(caminhoOriginal, destinationPath, overwrite: true);

                var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
                var logger = loggerFactory.CreateLogger<ExcelService>();
                return new ExcelService(destinationPath, logger, loggerFactory); // Agora passando o path copiado
            }
        }

        public void Abrir()
        {
            if (File.Exists(_filePath))
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = _filePath,
                        UseShellExecute = true
                    }
                };

                process.Start();
                process.WaitForExit();
                _logger.LogInformation("✅ O Excel foi fechado.");

            }
            else
            {
                _logger.LogError("❌ Arquivo não encontrado.");
            }
        }

        public void Dispose()
        {
            _package?.Dispose();
        }
    }
}