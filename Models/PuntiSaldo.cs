namespace PoliziaMunicipale.Models
{
    public class PuntiSaldo
    {
        public string Cognome { get; set; }
        public string Nome { get; set; }
        public DateTime DataViolazione { get; set;}
        public decimal Importo { get; set; }
        public int DecurtamentoPunti { get; set; }

    }
}
