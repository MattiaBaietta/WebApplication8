﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication8.Models
{
    public class Aziende
    {
        [Required(ErrorMessage = "La Partita Iva è obbligatoria")]
        public string PIva { get; set; }

        [Required(ErrorMessage = "La Ragione Sociale è obbligatoria")]
        public string RagioneSociale { get; set; }

        [Required(ErrorMessage = "La Città è obbligatoria")]
        public string Citta { get; set; }
        [Required(ErrorMessage = "Il Cap è obbligatorio")]
        [Range(0, 99999, ErrorMessage = "Cap non valido")]
        public int Cap { get; set; }
        [Required(ErrorMessage = "L'Indirizzo è obbligatorio'")]
        public string Indirizzo { get; set; }
    }
}