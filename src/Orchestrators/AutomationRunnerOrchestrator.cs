using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RpaConsultaAtivosB3.src.Interface;
using RpaConsultaAtivosB3.src.Services;
using static RpaConsultaAtivosB3.src.Services.ExcelService;
using RpaConsultaAtivosB3.src.Executors;
using Serilog;
using RpaConsultaAtivosB3.src.Models;
using OpenQA.Selenium;


namespace RpaConsultaAtivosB3.src.Orchestrators
{
    //Classe responsavel por fazer a orquestração de todo o processo.
    public class AutomationRunnerOrchestrator
    {
        private readonly EmailService _emailService;
        private readonly IConsultaAtivoService _consultaService;
        private IWebDriver _driver; // WebDriver para automação de navegador

        public AutomationRunnerOrchestrator(EmailService emailService, IConsultaAtivoService consultaService,
            IWebDriver driver)
        {
            _driver = driver;
            _emailService = emailService;
            _consultaService = consultaService;
        }
       

        public void Executar(string caminhoExcel)
        {
            try
            {
                using (var excel = ExcelServiceFactory.Criar(caminhoExcel))
                {
                    var codigos = excel.Reader.LerCodigosAtivos();
                    _consultaService.AcessarPagina();

                    int linha = 2;
                    foreach (var codigo in codigos)
                    {
                        try
                        {
                            _consultaService.PesquisarAtivo(codigo);
                            var info = _consultaService.ExtrairInformacoes(codigo);

                            info.Ativo = codigo;
                            excel.Writer.EscreverAtivo(linha++, info);
                            Log.Information("✅ Informações gravadas para o ativo: {Codigo}", codigo);
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex, "❌ Erro ao processar ativo '{Codigo}'", codigo);
                            excel.Writer.EscreverAtivo(linha++, new AtivoInfo
                            {
                                Ativo = codigo,
                                UltimoValor = "",
                                Oscilacao = "",
                                Data = DateTime.Now.Date,
                                Hora = DateTime.Now.TimeOfDay
                            });
                        }
                    }

                    excel.Salvar();
                    Log.Information("✅ Planilha salva com sucesso.");
                    _driver.Quit();
                    Log.Information("✅ WebDriver fechado com sucesso.");
                }

                _emailService.EnviarPlanilhaPorEmail(caminhoExcel);
                Log.Information("✅ Automação finalizada com sucesso.");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "❌ Erro fatal ao executar automação.");
            }
        }

    }
}