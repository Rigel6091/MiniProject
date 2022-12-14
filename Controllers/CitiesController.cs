using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MiniProject.Models;
using System.Data;
using System.Data.SqlClient;

namespace MiniProject.Controllers
{
    public class CitiesController : Controller
    {
        // GET: citiesController
        public ActionResult Index()
        {

            try
            {
                List<cities> Citys = new List<cities>();
                Citys = cities.AllCitie();
                return View(Citys);
            }
            catch (Exception ex)
            {
                return View(ViewBag.Msg = ex.Message);
            }

            return View();
        }

        // GET: citiesController/Details/5
        public ActionResult Details(int CityId)
        {
            try
            {
                cities Citys = new cities();
                Citys = cities.GetByCityId(CityId);
                return View(Citys);
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: citiesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: citiesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(cities nc)
        {
            try
            {
                cities.AddNewCity(nc);
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: citiesController/Edit/5
        public ActionResult Edit(int CityId)
        {
            try
            {
                cities Citys = cities.GetByCityId(CityId);
                return View(Citys);
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // POST: citiesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(cities ct)
        {
            try
            {
                cities.UpdateCities(ct);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: citiesController/Delete/5
        public ActionResult Delete(int CityId)
        {
            cities Citys = cities.GetByCityId(CityId);
            return View(Citys);

        }

        // POST: citiesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int CityId, cities c)
        {

            try
            {
                cities.Delete(CityId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View();
            }
        }
    }
}
