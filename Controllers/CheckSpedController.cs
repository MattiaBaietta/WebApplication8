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
    public class CheckSpedController : Controller
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["Delivery"].ToString();
        SqlConnection conn = new SqlConnection(connectionString);

        // GET: CheckSped
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CheckSpedButton(string id, string user)
        {
            List<StatoSpedizioni> Stato = new List<StatoSpedizioni>();
            string tipoutente;
            conn.Open();
            if (!user.All(char.IsDigit))
            {

                tipoutente = "CFiscale";

            }
            else
            {
                tipoutente = "PIva";

            }
            var command = new SqlCommand($"SELECT idspedizione FROM Spedizioni WHERE {tipoutente}='{user}' and idspedizione={id}", conn);
            var reader = command.ExecuteReader();


            if (reader.HasRows)
            {
                conn.Close();
                conn.Open();
                var cmd = new SqlCommand($"SELECT * FROM StatoSpedizioni WHERE idspedizione={id}", conn);
                var reader2 = cmd.ExecuteReader();
                while (reader2.Read())
                {
                    var s = new StatoSpedizioni()
                    {
                        idspedizione = (int)reader2["idspedizione"],
                        LuogoAttuale = (string)reader2["LuogoAttuale"],
                        Descrizione = (string)reader2["Descrizione"],
                        DataAgg = (DateTime)reader2["DataAgg"],
                        Stato = (string)reader2["Stato"]
                    };
                    Stato.Add(s);
                }
                ViewBag.StatoSped = Stato;
            }
            conn.Close();
            return View("Index", Stato);

        }

    }
}