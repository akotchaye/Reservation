using Projet.Akotchaye.App_Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Projet.Akotchaye.Controllers
{
    public class DesactivSalleController : Controller
    {
        private GESRESEntities db = new GESRESEntities();
        // GET: DesactivSalle
        public ActionResult DesactivSalle(int? id)
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
            ViewBag.ImgSalle1 = Convert.ToBase64String(salle.ImgSalle1);
            if (salle.ImgSalle2 != null)
                ViewBag.ImgSalle2 = Convert.ToBase64String(salle.ImgSalle2);
            else
                ViewBag.ImgSalle2 = null;

            return View(salle);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DesactivSalle(int IdSalle, int IdGes, int IdUser, string NomSalle, int CapaciteSalle, decimal PrixSalle, string DescripSalle, string AdrSalle, string ImgSalle1, string ImgSalle2)
        {

            Salle salle = new Salle();

           
            salle.IdGes = IdGes;
            salle.IdUser = IdUser;
            salle.IdSalle = IdSalle;
            salle.NomSalle = NomSalle;
            salle.CapaciteSalle = CapaciteSalle;
            salle.PrixSalle = PrixSalle;
            salle.DescripSalle = DescripSalle;
            salle.IsActiveSalle = false;
            salle.EtatSalle = "Maintenance";
            salle.AdrSalle = AdrSalle;
            salle.ImgSalle1 = Convert.FromBase64String(ImgSalle1);
            if (ImgSalle2 != null)
                salle.ImgSalle2 = Convert.FromBase64String(ImgSalle2);
            else
                salle.ImgSalle2 = null;


            db.Entry(salle).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("DashGes", "Dashboard");

        }

    }

}
