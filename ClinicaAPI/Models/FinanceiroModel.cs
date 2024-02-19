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
        public string descricao { get; set; }
        public double valor { get; set; }
        public double saldo { get; set; }
        public string refAgenda { get; set; } // é uma referência ao número da sessão ou o número da multi, pra quando for pago especificamente estas. Se estiver vazia, é saldo a mais de um pagamento
        public Boolean? selecionada { get; set; } // verifica o saldo: se tiver sido totalmente consumido, fica true
        public string recibo { get; set; }

    }
}
