using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MiniProject.Models;
using System.Data.SqlClient;
using System.Data;
using System;
using System.Linq;

namespace MiniProject.Controllers
{
    public class ResisterUserController : Controller
    {
        //GET
        public ActionResult Create(bool isTrue)
        {
            ViewData["isTrue"] = isTrue;
            ResisterUser model = new ResisterUser();
            model.drdncity = DropDownCity();
            return View(model);

        }
        // POST: ResisterUserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ResisterUser ruc)
        {
            try
            {
                ResisterUser.registerUser(ruc);
                return RedirectToAction("Create", new { isTrue = true});
            }
            catch(Exception e)
            {
                return View(e.Message);
            }
        }
        public static List<SelectListItem> DropDownCity()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = @"Data Source=(localdb)\ProjectModels;Initial Catalog=MP;Integrated Security=True";
            cn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Select * from City";
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                items.Add(new SelectListItem
                {
                    Text = dr["CityName"].ToString(),
                    Value = dr["Cityid"].ToString()
                });
            }
            cn.Close();
            return items;
        }

        public ActionResult Login()
        {
            ResisterUser r = new ResisterUser();
            r.LoginName = Request.Cookies["LoginName"];
            r.Password = Request.Cookies["Password"];

            if (r.LoginName != null)
            {
                string Fullame = ResisterUser.isvalidlnlp(r.LoginName, r.Password);
                if (Fullame != null)
                {
                    HttpContext.Session.SetString("LoginName", r.LoginName);
                    HttpContext.Session.SetString("FullName", Fullame);
                    return RedirectToAction("Welcome");
                }
            }
           
            return View(); 
            
        }
        [HttpPost]
        public ActionResult Login(string LoginName, string Password, string RememberMe)
        {
            try
            {
                string fullName = ResisterUser.isvalidlnlp(LoginName, Password);
                HttpContext.Session.SetString("FullName", fullName);
                HttpContext.Session.SetString("LoginName", LoginName);
                if (RememberMe == "on")
                {
                    CookieOptions cos = new CookieOptions();
                    cos.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Append("LoginName", LoginName, cos);
                    Response.Cookies.Append("Password", Password, cos);      
                }
                return RedirectToAction("Welcome");
            }
            catch (Exception exception)
            {
                ViewData["Error"] = exception.Message;
                return View("Login", new { ViewBag.Error });
            }
        }

        public ActionResult Welcome()
        {
            string fullName = HttpContext.Session.GetString("FullName");
            if (fullName == null)
            {
                return RedirectToAction("Welcome");
            }
            ViewData["FullName"] = fullName;
            return View();
        }
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            Response.Cookies.Delete("LoginName");
            Response.Cookies.Delete("Password");
            return RedirectToAction("Login");
        }

        public ActionResult Update(bool isTrue)
        {
            try
            {
                ViewData["isTrue"] = isTrue;
                string fullName = HttpContext.Session.GetString("FullName");
                string loginName = HttpContext.Session.GetString("LoginName");
                ResisterUser user = new ResisterUser();
                user = ResisterUser.UserByLoginname(loginName);
                user.drdncity = DropDownCity();
                return View(user);
            }
            catch (Exception ex)
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult Update(string LoginName ,ResisterUser ru1)
        {
            try
            {
                ResisterUser.UpdateUser(LoginName, ru1);
                return RedirectToAction("Update", new { isTrue = true });
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public ActionResult Showtable()
        {
            SortedDictionary<string,List<ResisterUser>> ru=ResisterUser.marsViewdata();
            return View(ru);
        }




        // GET: ResisterUserController/Details/5
        public ActionResult Details()
        {
            ResisterUser model = new ResisterUser();
            model.drdncity = DropDownCity();
            ResisterUser.ShowCityEntry(model.CityId);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(ResisterUser ru)
        {
            try
            {
                //ResisterUser.ShowCityEntry(ru);
                return View();
            }
            catch
            {
                return View();
            }
        }
    }
}
