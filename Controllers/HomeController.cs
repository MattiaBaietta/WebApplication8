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
        List<int> TrackingNum = new List<int>();

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
            DateTime now = DateTime.Now;



            SqlCommand command = new SqlCommand("INSERT INTO SPEDIZIONI (PIva, CFiscale, DataInvio, Peso, CittaDest, Indirizzo, NomeDest, Costo, DataDest) " +
                 "VALUES (@PIva, @CFiscale, @DataInvio, @Peso, @CittaDest, @Indirizzo, @NomeDest, @Costo, @DataDest); SELECT SCOPE_IDENTITY();", conn);

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

            int nuovoId = Convert.ToInt32(command.ExecuteScalar());
            conn.Close();
            conn.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO StatoSpedizioni (idspedizione,Descrizione, LuogoAttuale, DataAgg, Stato) VALUES (@idspedizione,'', 'Rimini', @DataAgg, 'Spedito')", conn);
            cmd.Parameters.AddWithValue("@idspedizione", nuovoId);

            cmd.ExecuteNonQuery();
            conn.Close();
            return View();
        }
        public ActionResult EditDelivery()
        {
            conn.Open();
            var command = new SqlCommand("SELECT idspedizione FROM Spedizioni", conn);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {

                TrackingNum.Add((int)reader["idspedizione"]);
            }
            ViewBag.Tracking = TrackingNum;
            conn.Close();
            return View();
        }
        public ActionResult EditDeliveryButton(StatoSpedizioni Aggiornamento)
        {
            conn.Open();
            DateTime now = DateTime.Now;
            SqlCommand cmd = new SqlCommand($"INSERT INTO StatoSpedizioni (idspedizione,Descrizione, LuogoAttuale, DataAgg, Stato) VALUES ({Aggiornamento.idspedizione},'{Aggiornamento.Descrizione}', '{Aggiornamento.LuogoAttuale}', @DataAgg, '{Aggiornamento.Stato}')", conn);
            cmd.Parameters.AddWithValue("@DataAgg", now.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.ExecuteNonQuery();
            conn.Close();

            return View();
        }
        public ActionResult Stats()
        {
            return View();
        }
        public ActionResult ShowStats(string valbtn)
        {
            DateTime now = DateTime.Now;
            List<int> ints = new List<int>();
            List<Citta> cities = new List<Citta>();
            conn.Open();
            SqlCommand cmd;
            switch (valbtn)
            {
                case "Consegna":
                    string formattedDate = now.ToString("yyyy-MM-dd");
                    cmd = new SqlCommand("SELECT * FROM Spedizioni WHERE Stato='In Consegna' AND CONVERT(date, DataAgg) = @DataAgg", conn);
                    cmd.Parameters.AddWithValue("@DataAgg", formattedDate);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ints.Add((int)reader["idspedizione"]);
                    }
                    conn.Close();
                    ViewBag.ShowStats = "Consegna";
                    return View("Stats", ints);

                case "Attesa":
                    cmd = new SqlCommand("SELECT COUNT(*) FROM SPEDIZIONI WHERE Stato='In consegna'", conn);
                    var readerAtt = cmd.ExecuteReader();
                    if (readerAtt.Read())
                    {
                        ViewBag.ShowStats = "Attesa";
                        return View("Stats", readerAtt.GetInt32(0));
                    }
                    conn.Close();
                    ViewBag.ShowStats = "Attesa";
                    break;


                case "Citta":
                    cmd = new SqlCommand("SELECT Count(*) FROM Spedizioni GROUP BY CittaDest", conn);
                    var readerCit = cmd.ExecuteReader();
                    while (readerCit.Read())
                    {
                        var c = new Citta()
                        {
                            id = readerCit.GetInt32(0),
                            City = readerCit.GetString(1),
                        };
                        cities.Add(c);

                    }
                    ViewBag.ShowStats = "Citta";
                    return View("Stats", cities);

            }

            return View();
        }
    }
}