using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Serilog;

namespace RpaConsultaAtivosB3.src.Builders
{
    public class WebDriverBuilder // classe responsável por construir o WebDriver
    {
        public static IWebDriver Create()
        {
            try
            {
                var service = ChromeDriverService.CreateDefaultService(@"C:\WebDrivers\chromedriver-win32");
                service.HideCommandPromptWindow = true; // Evita que a janela do console apareça

                var options = new ChromeOptions();
                options.AddArgument("--start-maximized");

                var driver = new ChromeDriver(service, options);

                Log.Information("✅ WebDriver iniciado com sucesso.");
                return driver;
            }
            catch (Exception ex)
            {
                Log.Error("❌ Erro ao iniciar o WebDriver: {Erro}", ex.Message);
                throw;
            }
        }
    }
}