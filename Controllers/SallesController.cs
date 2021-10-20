using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlTypes;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Projet.Akotchaye.App_Data;

namespace Projet.Akotchaye.Controllers
{
    public class SallesController : Controller
    {
      
        public SallesController()
        {

        }
        private GESRESEntities db = new GESRESEntities();

        // GET: Salles
        public ActionResult Index()
        {
            var salle = db.Salle.Include(s => s.Gestionnaire);
            return View(salle.ToList());
        }

        // GET: Salles/Details/5
        public ActionResult Details(int? id)
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
            return View(salle);
        }

        // GET: Salles/Create
        public ActionResult Create()
        {
            if (Session["IdUser"] == null)
            {
                return RedirectToAction("Authentification", "Auth");
            }

            return View();
        }

        // POST: Salles/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]     //revoir la reception des données
        public ActionResult Create(string Statut,string NomSalle,int CapaciteSalle,string PrixSalle,string DescripSalle,string AdrSalle,HttpPostedFileBase ImgSalle1, HttpPostedFileBase ImgSalle2)
        {
            if (ModelState.IsValid)
            {
                Salle salle = new Salle();
                salle.NomSalle = NomSalle;
                salle.CapaciteSalle = CapaciteSalle;
                decimal price;
                if (Decimal.TryParse(PrixSalle, out price))
                    salle.PrixSalle = Math.Round(price,2);
                else
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                if(ImgSalle1!= null)
                {
                    salle.ImgSalle1 = new byte[ImgSalle1.ContentLength];
                    ImgSalle1.InputStream.Read(salle.ImgSalle1, 0, ImgSalle1.ContentLength);
                }
                if (ImgSalle2 != null)
                {
                    salle.ImgSalle2 = new byte[ImgSalle2.ContentLength];
                    ImgSalle2.InputStream.Read(salle.ImgSalle2, 0, ImgSalle2.ContentLength);
                }
                else
                {
                    salle.ImgSalle2 =null;
                }
                int Id = (int)Session["IdUser"];

                var ges = db.Gestionnaire.Where(a => a.IdUser.Equals(Id)).FirstOrDefault();

                /*salle.EtatSalle = EtatSalle;*/
                salle.AdrSalle = AdrSalle;
                salle.DescripSalle =DescripSalle;
                
                if (Statut == "true")
                {
                    salle.EtatSalle = "Disponible";
                    salle.IsActiveSalle = true;
                }
                else
                {
                    salle.EtatSalle = "Maintenace";
                    salle.IsActiveSalle = false;
                }
                salle.IdGes = ges.IdGes;
                salle.IdUser = Id;
                salle.IdGes = ges.IdGes;
                salle.IdUser = Id; 

                db.Salle.Add(salle);
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


                return RedirectToAction("DashGes","Dashboard");
            }

           /*ViewBag.IdUser = new SelectList(db.Gestionnaire, "IdUser", "IdUser", salle.IdUser);*/
            return View( /*salle*/);
        }

        // GET: Salles/Edit/5
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
            
            return View(salle);
        }

        // POST: Salles/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int IdSalle,string NomSalle,string Statut, int CapaciteSalle, string PrixSalle, string DescripSalle,  string AdrSalle, HttpPostedFileBase ImgSalle1, HttpPostedFileBase ImgSalle2)
        {
            if (ModelState.IsValid)
            {
                Salle salle = new Salle();
                salle.NomSalle = NomSalle;
                salle.CapaciteSalle = CapaciteSalle;
                decimal price;
                if (Decimal.TryParse(PrixSalle, out price))
                    salle.PrixSalle = Math.Round(price, 2);
                else
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                if (ImgSalle1 != null)
                {
                    salle.ImgSalle1 = new byte[ImgSalle1.ContentLength];
                    ImgSalle1.InputStream.Read(salle.ImgSalle1, 0, ImgSalle1.ContentLength);
                }
                if (ImgSalle2 != null)
                {
                    salle.ImgSalle2 = new byte[ImgSalle2.ContentLength];
                    ImgSalle2.InputStream.Read(salle.ImgSalle2, 0, ImgSalle2.ContentLength);
                }
                else
                {
                    salle.ImgSalle2 = null;
                }
                int Id = (int)Session["IdUser"];

                var ges = db.Gestionnaire.Where(a => a.IdUser.Equals(Id)).FirstOrDefault();
                salle.IdSalle=IdSalle;
                /*salle.EtatSalle = EtatSalle;*/
                salle.AdrSalle = AdrSalle;
                salle.DescripSalle = DescripSalle;
               
                if (Statut =="true")
                {
                    salle.EtatSalle ="Disponible";
                    salle.IsActiveSalle =true;
                }
                else
                {
                    salle.EtatSalle ="Maintenance";
                    salle.IsActiveSalle = false;
                }
                salle.IdGes = ges.IdGes;
                salle.IdUser = Id;


                db.Entry(salle).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("DashGes", "Dashboard");
            }
            return View("erreur");
        }
          
        

        // GET: Salles/Delete/5
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
            Salle salle = db.Salle.Find(id);
            if (salle == null)
            {
                return HttpNotFound();
            }
            return View(salle);
        }

        // POST: Salles/Delete/5
        [HttpPost, ActionName("Delete")]
       /* [ValidateAntiForgeryToken]*/
        public ActionResult DeleteConfirmed(int id)
        {
            Salle salle = db.Salle.Find(id);
            db.Salle.Remove(salle);
            Reservation reservation = db.Reservation.Where(a => a.IdSalle.Equals(id)).FirstOrDefault();
            if (reservation != null)
            {
                db.Reservation.Remove(reservation);
            }
           
            db.SaveChanges();

           
           /* db.SaveChanges();*/
            return RedirectToAction("DashGes", "Dashboard");
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
