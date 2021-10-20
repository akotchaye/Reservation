using Projet.Akotchaye.App_Data;
using Projet.Akotchaye.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Projet.Akotchaye.Controllers
{
    public class ValidResController : Controller
    {
        private GESRESEntities db = new GESRESEntities();
        // GET: ValidRes
        public ActionResult ValidRes(int? id)
        {
            if (Session["IdUser"] == null)
            {
                return RedirectToAction("Authentification", "Auth");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservation.Find(id);

            if (reservation == null)
            {
                return HttpNotFound();
            }
           

            Reservation resv = new Reservation();
            resv = db.Reservation.FirstOrDefault(m => m.IdRes == id);

            return View(reservation);
           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ValidRes(int IdRes,int IdUser,int IdSalle,int Cli_IdUser,int IdClient,decimal MontantRes,  DateTime DatedebutRes,DateTime DatefinRes)
        {

            Reservation reservation = new Reservation();
            reservation.IsvalidRes = true;
            reservation.IdRes = IdRes;
            reservation.IdUser = IdUser;
            reservation.IdSalle = IdSalle;
            reservation.Cli_IdUser = Cli_IdUser;
            reservation.IdClient = IdClient;
            reservation.MontantRes = MontantRes;
            reservation.EtatRes = "Validé";
            reservation.DatedebutRes = DatedebutRes;
            reservation.DatefinRes = DatefinRes;

            db.Entry(reservation).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("DashGes", "Dashboard");
        }
    }
}