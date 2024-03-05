using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication8.Models
{
    public class Privati
    {
        [Required(ErrorMessage = "Il Codice Fiscale è obbligatorio")]
        public string CFiscale { get; set; }
        [Required(ErrorMessage = "Il Nome è obbligatorio")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Il Cognome è obbligatorio")]
        public string Cognome { get; set; }
        [Required(ErrorMessage = "La Città è obbligatoria")]
        public string Citta { get; set; }
        [Required(ErrorMessage = "Il Cap è obbligatorio")]
        [Range(0, 99999, ErrorMessage = "Cap non valido")]
        public int Cap { get; set; }
        [Required(ErrorMessage = "L'Indirizzo è obbligatorio'")]
        public string Indirizzo { get; set; }
    }
}