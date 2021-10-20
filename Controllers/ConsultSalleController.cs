using Projet.Akotchaye.App_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Projet.Akotchaye.Controllers
{
    public class ConsultSalleController : Controller
    {
        private GESRESEntities db = new GESRESEntities();
        // GET: ConsultSalle
        public ActionResult ConsultSalle(int? id)
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
    }
}