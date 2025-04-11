using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RpaConsultaAtivosB3.src.Models
{
    public class AtivoInfo
    {
        public string? Ativo { get; set; }                 // Código do ativo
        public string? UltimoValor { get; set; }           // Último preço
        public string? Oscilacao { get; set; }             // Variação percentual
        public DateTime Data { get; set; }               // Data da cotação
        public TimeSpan Hora { get; set; }               // Hora da cotação
    }
}