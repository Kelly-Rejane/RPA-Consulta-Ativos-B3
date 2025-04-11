using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using RpaConsultaAtivosB3.src.Models;
using RpaConsultaAtivosB3.src.Utils;

namespace RpaConsultaAtivosB3.src.Executors
{
    public class ExcelWriter //– Classe de Escrita
    {
        private readonly ExcelWorksheet _sheet; //– Atributo privado que armazena a planilha do Excel
        private readonly ILogger<ExcelWriter> _logger; //– Atributo privado que armazena o logger para registrar mensagens de log

        //– Construtor que recebe a planilha do Excel como parâmetro
        public ExcelWriter(ExcelWorksheet sheet, ILogger<ExcelWriter> logger)
        {
            _sheet = sheet; //– Inicializa a planilha
            _logger = logger; //– Inicializa o logger
        }

        //– Método que escreve os dados dos ativos na planilha do Excel
        public void EscreverAtivo(int linha, AtivoInfo info, int colunaInicial = 2)
        {
            try
            {
                bool valorValido = !string.IsNullOrWhiteSpace(info.UltimoValor) && !info.UltimoValor.Contains("_____");
                bool oscilacaoValida = !string.IsNullOrWhiteSpace(info.Oscilacao) && !info.Oscilacao.Contains("_____");

                _sheet.Cells[linha, colunaInicial].Value = valorValido ? info.UltimoValor.Trim() : "";
                _sheet.Cells[linha, colunaInicial + 1].Value = oscilacaoValida ? info.Oscilacao.Trim() : "";

                _sheet.Cells[linha, colunaInicial + 2].Value = info.Data.ToShortDateString();
                _sheet.Cells[linha, colunaInicial + 3].Value = info.Hora.ToString(@"hh\:mm\:ss");
            }
            catch (Exception ex)
            {
                _logger.LogError($"❌ Erro ao escrever na linha {linha}: {ex.Message}"); //– Registra uma mensagem de erro no log
                Console.WriteLine($"❌ Erro ao escrever na linha {linha}: {ex.Message}"); //– Exibe uma mensagem de erro no console
            }
        }
    }
}