using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml;

namespace RpaConsultaAtivosB3.src.Executors
{
    public class ExcelReader //– Classe de Leitura
    {
        private readonly ExcelWorksheet _sheet; //– Atributo privado que armazena a planilha do Excel

        //– Construtor que recebe a planilha do Excel como parâmetro
        public ExcelReader(ExcelWorksheet sheet)
        {
            _sheet = sheet;
        }

        //– Método que lê os códigos ativos da planilha do Excel
        //– O método recebe como parâmetros a coluna e a linha inicial para começar a leitura
        public List<string> LerCodigosAtivos(int coluna = 1, int linhaInicial = 2)
        {
            var codigos = new List<string>(); //– Cria uma lista vazia para armazenar os códigos ativos lidos da planilha
            int row = linhaInicial; //– Inicializa a variável row com o valor da linha inicial

            //– Laço que percorre as linhas da planilha a partir da linha inicial
            while (true)
            {
                var cellValue = _sheet.Cells[row, coluna].Text; //– Lê o valor da célula na linha e coluna especificadas
                //– Verifica se o valor da célula é nulo ou vazio

                if (string.IsNullOrWhiteSpace(cellValue))
                    break;

                // adiciona o valor da célula à lista de códigos ativos, removendo espaços em branco
                codigos.Add(cellValue.Trim());
                row++;
            }

            return codigos; //– Retorna a lista de códigos ativos lidos da planilha
        }
    }
}