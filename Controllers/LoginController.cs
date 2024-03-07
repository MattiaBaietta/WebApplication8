using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.Ajax.Utilities;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace WebApplication8.Controllers
{
    public class LoginController : Controller
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["Delivery"].ToString();
        SqlConnection conn = new SqlConnection(connectionString);
        // GET: Login
        public ActionResult Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("AlreadyLogged");
            }

            return View();
        }
        public ActionResult Login(string User, string Password)
        {

            conn.Open();
            var command = new SqlCommand($"SELECT * FROM Utenti WHERE [Password]='{Password}'and [User]='{User}'", conn);
            var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                FormsAuthentication.SetAuthCookie(User, true);
            }
            else
            {
                TempData["login"] = false;

            }
            conn.Close();
            return RedirectToAction("Index");
        }
        public ActionResult AlreadyLogged()
        {
            return View();
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}