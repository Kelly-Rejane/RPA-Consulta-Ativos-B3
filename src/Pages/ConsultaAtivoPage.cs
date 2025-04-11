using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using RpaConsultaAtivosB3.src.Interface;
using RpaConsultaAtivosB3.src.Models;
using SeleniumExtras.WaitHelpers;

namespace RpaConsultaAtivosB3.src.Pages
{
    public class ConsultaAtivoPage : IConsultaAtivoService
    {
        private readonly IWebDriver _driver; // Driver do Selenium para automação de navegador
        private readonly WebDriverWait _wait; // Espera explícita para aguardar elementos
        private readonly string _url;   // URL da página de consulta de ativos
        private readonly ILogger<ConsultaAtivoPage> _logger;

        // Método para abrir a página de consulta de ativos 
        public ConsultaAtivoPage(IWebDriver driver, IConfiguration configuration, ILogger<ConsultaAtivoPage> logger)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20)); // Espera explícita padrão
            _url = configuration["Dados:UrlSiteB3"];
            _logger = logger;
        }

        // Método para navegar até a página de consulta de ativos
        public void AcessarPagina()
        {
            try
            {
                _logger.LogInformation("Acessando a página: {Url}", _url);
                _driver.Navigate().GoToUrl(_url);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao acessar a página: {Url}", _url);
                throw;
            }
        }

        public void PesquisarAtivo(string codigo)
        {
            try
            {
                _logger.LogInformation("Pesquisando o ativo: {Codigo}", codigo);

                var input = _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("txtCampoPesquisa")));
                input.Clear();
                input.SendKeys(codigo);
                input.SendKeys(Keys.Enter);
            }
            catch (WebDriverTimeoutException ex)
            {
                _logger.LogError(ex, "Campo de pesquisa não encontrado na página.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao pesquisar o ativo: {Codigo}", codigo);
                throw;
            }
            
        }

        public AtivoInfo ExtrairInformacoes(string codigo)
        {
            try
            {
                _logger.LogInformation("Extraindo informações para o ativo: {Codigo}", codigo);

                _wait.Until(driver =>
                {
                    var txt = driver.FindElement(By.Id("cotacaoAtivo")).Text;
                    return Regex.IsMatch(txt, @"\d");
                });

                var ultimoValorRaw = _driver.FindElement(By.Id("cotacaoAtivo")).Text;
                var oscilacaoRaw = _driver.FindElement(By.Id("oscilacaoAtivo")).Text;

                var info = new AtivoInfo
                {
                    Ativo = codigo,
                    UltimoValor = ultimoValorRaw,
                    Oscilacao = oscilacaoRaw,
                    Data = DateTime.Now.Date,
                    Hora = DateTime.Now.TimeOfDay
                };

                _logger.LogInformation("Informações extraídas com sucesso: {@AtivoInfo}", info);
                return info;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao extrair informações do ativo: {Codigo}", codigo);
                throw;
            }
        }

    }
}