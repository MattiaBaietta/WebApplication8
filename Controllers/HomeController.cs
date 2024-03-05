using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.Mvc;
using System.Data.SqlClient;
using WebApplication8.Models;
using Azure.Core.Diagnostics;

namespace WebApplication8.Controllers
{
    public class HomeController : Controller
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["Delivery"].ToString();
        SqlConnection conn = new SqlConnection(connectionString);
        List<Privati> Cfiscale = new List<Privati>();
        List<Aziende> Piva = new List<Aziende>();


        public ActionResult Index()
        {

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult AddUser(string UserType)
        {
            if (UserType == null)
            {
                ViewBag.user = "Privato";
            }
            else
            {
                ViewBag.user = UserType;
            }

            return View();
        }
        public ActionResult AddPrivato(Privati p)
        {
            conn.Open();
            var command = new SqlCommand("" +
                "INSERT INTO Privati (CFiscale,Nome,Cognome,Citta,Cap,Indirizzo)" +
                $"VALUES ('{p.CFiscale}','{p.Nome}','{p.Cognome}','{p.Citta}',{p.Cap},'{p.Indirizzo}')", conn);
            command.ExecuteNonQuery();
            conn.Close();

            return View();
        }
        public ActionResult AddPIva(Aziende a)
        {
            conn.Open();
            var command = new SqlCommand("" +
                "INSERT INTO Aziende (PIva,RagioneSociale,Citta,Cap,Indirizzo)" +
                $"VALUES ('{a.PIva}','{a.RagioneSociale}','{a.Citta}',{a.Cap},'{a.Indirizzo}')", conn);
            command.ExecuteNonQuery();
            conn.Close();

            return View();
        }

        public ActionResult AddDelivery()
        {
            conn.Open();
            var commandPrivati = new SqlCommand("SELECT Nome,Cognome,CFiscale FROM PRIVATI", conn);
            var readerPrivati = commandPrivati.ExecuteReader();
            while (readerPrivati.Read())
            {
                var privato = new Privati()
                {
                    Nome = (string)readerPrivati["Nome"],
                    Cognome = (string)readerPrivati["Cognome"],
                    CFiscale = (string)readerPrivati["CFiscale"]
                };
                Cfiscale.Add(privato);
            }
            conn.Close();
            conn.Open();
            ViewBag.Cfiscale = Cfiscale;
            var commandPiva = new SqlCommand("SELECT PIva,RagioneSociale FROM AZIENDE", conn);
            var readerPiva = commandPiva.ExecuteReader();
            while (readerPiva.Read())
            {
                var azienda = new Aziende()
                {
                    PIva = (string)readerPiva["PIva"],
                    RagioneSociale = (string)readerPiva["RagioneSociale"]

                };
                Piva.Add(azienda);
            }
            ViewBag.PIva = Piva;
            conn.Close();
            return View();
        }
        public ActionResult AddDeliveryButton(Spedizioni s, string Idcliente)
        {
            conn.Open();



            SqlCommand command = new SqlCommand("INSERT INTO SPEDIZIONI (PIva,CFiscale, DataInvio, Peso, CittaDest, Indirizzo, NomeDest, Costo, DataDest, Stato) " +
                             "VALUES (@PIva,@CFiscale, @DataInvio, @Peso, @CittaDest, @Indirizzo, @NomeDest, @Costo, @DataDest, 'Spedito')", conn);

            if (!Idcliente.All(char.IsDigit))
            {
                s.CFiscale = Idcliente;
            }
            else
            {
                s.PIva = Idcliente;
            }
            command.Parameters.AddWithValue("@PIva", (object)s.PIva ?? DBNull.Value);
            command.Parameters.AddWithValue("@CFiscale", (object)s.CFiscale ?? DBNull.Value);
            command.Parameters.AddWithValue("@DataInvio", s.DataInvio);
            command.Parameters.AddWithValue("@Peso", s.Peso);
            command.Parameters.AddWithValue("@CittaDest", s.CittaDest);
            command.Parameters.AddWithValue("@Indirizzo", s.Indirizzo);
            command.Parameters.AddWithValue("@NomeDest", s.NomeDest);
            command.Parameters.AddWithValue("@Costo", s.Costo);
            command.Parameters.AddWithValue("@DataDest", s.DataDest);
            command.ExecuteNonQuery();
            conn.Close();
            return View();
        }

    }
}