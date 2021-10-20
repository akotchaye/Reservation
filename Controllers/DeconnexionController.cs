using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projet.Akotchaye.Controllers
{
    public class DeconnexionController : Controller
    {
        // GET: Deconnexion
        public ActionResult Deconnexion()
        {
                 
                Session["IdUser"] = null;
                return RedirectToAction("Authentification", "Auth");
           
        }
    }
}