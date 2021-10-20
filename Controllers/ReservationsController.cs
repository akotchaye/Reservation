using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Projet.Akotchaye.App_Data;
using Projet.Akotchaye.Models;

namespace Projet.Akotchaye.Controllers
{
    public class ReservationsController : Controller
    {
        private GESRESEntities db = new GESRESEntities();

        // GET: Reservations
        public ActionResult Index()
        {
            var reservation = db.Reservation.Include(r => r.Client).Include(r => r.Salle).Include(r => r.Utilisateur);
            return View(reservation.ToList());
        }

        // GET: Reservations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservation.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // GET: Reservations/Create
        public ActionResult Create(int? id)
        {
            /*contrôle du client*/
           
            if (Session["IdUser"] == null)
            {
                return RedirectToAction("Authentification", "Auth");
            }
            /*contrôle de l'existence de la salle a réserver*/
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Salle salle = db.Salle.Find(id);
            if (salle == null)
            {
                return HttpNotFound();
            }
                   
            salle = db.Salle.FirstOrDefault(m => m.IdSalle == (int)id);
           /* réccupération de l'identifiant de la salle dans le ViewBag qui sera renvoyée sur la vue*/
            ViewBag.Idsalle = salle.IdSalle;
            var Allres = new ViewModelReservation();
            Allres.Reservations = (from res in db.Reservation where res.IdSalle==salle.IdSalle select res).ToList();
          

           

            return View(Allres);
        }

        // POST: Reservations/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       /* [ValidateAntiForgeryToken]*/
        public ActionResult Create(int IdSalle, DateTime DatedebutRes,DateTime DatefinRes)
        {
            
            if (ModelState.IsValid)
            {
                var rvs = from res in db.Reservation where res.IdSalle == IdSalle select res;
                var checkQ1 = rvs.Where(a => a.DatedebutRes <= DatedebutRes && a.DatefinRes>= DatedebutRes).FirstOrDefault();
              
                var checkQ2 = rvs.Where(a => a.DatedebutRes <= DatefinRes && a.DatefinRes >= DatefinRes).FirstOrDefault();
                
                var checkQ3 =rvs.Where(a => a.DatedebutRes >= DatedebutRes && a.DatefinRes <= DatefinRes).FirstOrDefault();

                if (checkQ1!=null || checkQ2!=null || checkQ3!=null ||DatedebutRes==DatefinRes)
                {
                    return View("erreur checq");
                }
                //convertir les dates en heures et chercher la difference puis multiplier par le prix de la salle
               
                int date1h = DatedebutRes.Hour;
                int date2h = DatefinRes.Hour;
                int difdate;
                if (date2h > date1h)
                {
                    difdate = date2h - date1h;
                }
                else
                    difdate = date1h - date2h;
               
                int Id = (int)Session["IdUser"];

               
                Utilisateur utilisateur = new Utilisateur();
                utilisateur = db.Utilisateur.FirstOrDefault(m => m.IdUser == Id );

                Client client = new Client();
                client = db.Client.FirstOrDefault(m => m.IdUser == Id );

                /*réccupération de  l'identifiant de la salle*/
                Salle salle = new Salle();
                salle = db.Salle.FirstOrDefault(m => m.IdSalle == IdSalle);

                Reservation reservation = new Reservation();
                reservation.DatedebutRes = DatedebutRes;
                reservation.DatefinRes = DatefinRes;
                reservation.IdSalle =salle.IdSalle;
                reservation.IdClient = client.IdClient;
                reservation.IdUser = utilisateur.IdUser;
                reservation.IsvalidRes = false;
                reservation.EtatRes = "Enregistré";
                reservation.Cli_IdUser = client.IdUser;
                reservation.MontantRes = difdate * salle.PrixSalle;

                db.Reservation.Add(reservation);
                db.SaveChanges();
               /* try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                        }
                    }
                }*/
                return RedirectToAction("DashClient","Dashboard");
            }
            return View("erreur modelstate");

           


        }


        // GET: Reservations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["IdUser"] == null)
            {
                return RedirectToAction("Authentification", "Auth");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Salle salle = db.Salle.Find(id);
                  
            if (salle == null)
            {
                return HttpNotFound();
            }
            salle = db.Salle.FirstOrDefault(m => m.IdSalle == (int)id);
            /* réccupération de l'identifiant de la salle dans le ViewBag qui sera renvoyée sur la vue*/
            ViewBag.Idsalle = salle.IdSalle;

            Reservation resv = new Reservation();
            resv = db.Reservation.FirstOrDefault(m => m.IdSalle == salle.IdSalle);
            ViewBag.DatedebutRes = resv.DatedebutRes;
            ViewBag.DatefinRes = resv.DatefinRes;
            ViewBag.IdRes = resv.IdRes;
            var Allres = new ViewModelReservation();
            Allres.Reservations = (from res in db.Reservation where res.IdSalle == salle.IdSalle select res).ToList();
           
            return View(Allres);
        }

        // POST: Reservations/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int IdRes,int IdSalle, DateTime DatedebutRes, DateTime DatefinRes)
        {
           
                if (ModelState.IsValid)
                {
                    var rvs = from res in db.Reservation where res.IdSalle == IdSalle select res;
                    var checkQ1 = rvs.Where(a => a.DatedebutRes <= DatedebutRes && a.DatefinRes >= DatedebutRes).FirstOrDefault();

                    var checkQ2 = rvs.Where(a => a.DatedebutRes <= DatefinRes && a.DatefinRes >= DatefinRes).FirstOrDefault();

                    var checkQ3 = rvs.Where(a => a.DatedebutRes >= DatedebutRes && a.DatefinRes <= DatefinRes).FirstOrDefault();

                    if (checkQ1 != null || checkQ2 != null || checkQ3 != null || DatedebutRes == DatefinRes)
                    {
                        return View("erreur checq");
                    }
                    //convertir les dates en heures et chercher la difference puis multiplier par le prix de la salle

                    int date1h = DatedebutRes.Hour;
                    int date2h = DatefinRes.Hour;
                    int difdate;
                    if (date2h > date1h)
                    {
                        difdate = date2h - date1h;
                    }
                    else
                        difdate = date1h - date2h;

                    int Id = (int)Session["IdUser"];


                    Utilisateur utilisateur = new Utilisateur();
                    utilisateur = db.Utilisateur.FirstOrDefault(m => m.IdUser == Id);

                    Client client = new Client();
                    client = db.Client.FirstOrDefault(m => m.IdUser == Id);

                    /*réccupération de  l'identifiant de la salle*/
                    Salle salle = new Salle();
                    salle = db.Salle.FirstOrDefault(m => m.IdSalle == IdSalle);

                    Reservation reservation = new Reservation();
                    reservation.IdRes = IdRes;
                    reservation.DatedebutRes = DatedebutRes;
                    reservation.DatefinRes = DatefinRes;
                    reservation.IdSalle = salle.IdSalle;
                    reservation.IdClient = client.IdClient;
                    reservation.IdUser = utilisateur.IdUser;
                    reservation.IsvalidRes = false;
                    reservation.EtatRes = "En attente";
                    reservation.Cli_IdUser = client.IdUser;
                    reservation.MontantRes = difdate * salle.PrixSalle;

                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DashClient","Dashboard");
            }
          
            return View("erreur");
        }

        // GET: Reservations/Delete/5
        public ActionResult Delete(int? id)
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
            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reservation reservation = db.Reservation.Find(id);
            db.Reservation.Remove(reservation);
            db.SaveChanges();
            return RedirectToAction("DashGes","Dashboard");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
