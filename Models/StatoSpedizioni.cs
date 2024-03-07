using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication8.Models
{
    public class StatoSpedizioni
    {
        public int idspedizione { get; set; }
        public string LuogoAttuale { get; set; }
        public string Descrizione { get; set; }
        public DateTime DataAgg { get; set; }
        public string Stato { get; set; }
    }
}