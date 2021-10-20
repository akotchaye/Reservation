using Projet.Akotchaye.App_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projet.Akotchaye.Controllers
{
    public class AuthController : Controller
    {
      

        private GESRESEntities db = new GESRESEntities();

        public AuthController()
        {

        }

       

        
        // GET: Auth
        public ActionResult Authentification()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Authentification(string username, string pwd)
        {
           
                                  
            var utilisateur = db.Utilisateur.Where(a => a.Username.Equals(username) && a.Pwd.Equals(pwd)).FirstOrDefault();
            if (utilisateur!=null)
            {
               

                Session["IdUser"] = utilisateur.IdUser;
                Session["Username"] = utilisateur.Username;
                Session["Pwd"] = utilisateur.Pwd;
                Session["Libelle"] = utilisateur.Libelle;
                Session["Nom"] = utilisateur.Nom;
                Session["Prenoms"] = utilisateur.Prenoms;
                Session["Adresse"] = utilisateur.Adresse;
                Session["Email"] = utilisateur.Email;

                switch (utilisateur.Libelle)
                {
                    case "client":
                       
                        /*svgIdClient = utilisateur.IdUser;*/

                        return RedirectToAction("DashClient","Dashboard");
                        
                    case "gestionnaire":
                       
                       /* svgIdGes = utilisateur.IdUser;*/
                        return RedirectToAction("DashGes","Dashboard");
                        
                    case "commercial":
                        
                       /* svgIdCom = utilisateur.IdUser;*/
                        return  RedirectToAction("DashCom","Dashboard");
                        
                    default:
                        return View(utilisateur);
                }
               
             

            }
            return View(utilisateur);
        }
    }
}