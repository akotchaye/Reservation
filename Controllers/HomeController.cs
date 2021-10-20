using Projet.Akotchaye.App_Data;
using Projet.Akotchaye.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Projet.Akotchaye.Controllers
{
    public class HomeController : Controller
    {
      
        private GESRESEntities db = new GESRESEntities();
        //GET
        public ActionResult Index()
        {
             var vm = new ViewModelSalle();
             vm.Salles = (from salle in db.Salle select salle).ToList();
          
            return View(vm);
        }
        [HttpPost]
        public ActionResult Index(int?id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Salle salle = db.Salle.Find(id);
            if (salle == null)
            {
                return HttpNotFound();
            }
           return RedirectToAction("Create","Reservations");
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
    }
}