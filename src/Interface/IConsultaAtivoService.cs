using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RpaConsultaAtivosB3.src.Models;

namespace RpaConsultaAtivosB3.src.Interface
{
    public interface IConsultaAtivoService
    {
        void AcessarPagina();
        void PesquisarAtivo(string codigo);
        AtivoInfo ExtrairInformacoes(string codigo);
    }
}