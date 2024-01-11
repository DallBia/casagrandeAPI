using System;
using System.ComponentModel.DataAnnotations;

namespace ClinicaAPI.Models
{
    public class FinanceiroModel
    {
        public  int id { get; set; }
        public int idCliente { get; set; }
        public int idFuncAlt { get; set; }
        public DateTime data { get; set; }
        public string nome { get; set; }
        public string descricao { get; set; }
        public double valor { get; set; }
        public int refAgenda { get; set; }
        public Boolean? selecionada { get; set; }
        public string recibo { get; set; }

    }
}
