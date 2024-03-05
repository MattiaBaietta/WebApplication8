using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication8.Models
{
    public class Spedizioni
    {
        public string Idcliente { get; set; }
        public DateTime DataInvio { get; set; }
        public int Peso { get; set; }
        public string CittaDest { get; set; }
        public string Indirizzo { get; set; }
        public string NomeDest { get; set; }
        public int Costo { get; set; }
        public DateTime DataDest { get; set; }
        public string Stato { get; set; }
        public string PIva { get; set; }
        public string CFiscale { get; set; }
    }
}