using Projet.Akotchaye.App_Data;
using Projet.Akotchaye.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace Projet.Akotchaye.Controllers
{
    public class DashboardController : Controller
    {
        

        private GESRESEntities db = new GESRESEntities();

        public DashboardController()
        {

        }

        /* public ActionResult Client_Annuler(int? id)/*identifiant de la salle
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
               Utilisateur utilisateur = new Utilisateur();
               utilisateur = db.Utilisateur.FirstOrDefault(m => m.IdUser == salle.IdUser);

               try
               {

                   var senderEmail = new MailAddress("akotchaybatcho@outlook.fr", "Akotchaye");
                   var receiverEmail = new MailAddress(utilisateur.Email);
                   var password = "J'@$$1#e";
                   var sub = "Demande d'annulation de reservation";
                   var body = "Identifiant de la salle: " + salle.IdSalle + " Nom Salle: " + salle.NomSalle + " Adressse: " + salle.AdrSalle;

                   var smtp = new SmtpClient
                   {
                       Host = "smtp-mail.outlook.com" /*"smtp.gmail.com",
                       Port = 587,
                       EnableSsl = true,
                       DeliveryMethod = SmtpDeliveryMethod.Network,
                       UseDefaultCredentials = false,
                       Credentials = new NetworkCredential(senderEmail.Address, password)
                   };
                   using (var mess = new MailMessage(senderEmail, receiverEmail)
                   {
                       Subject = sub,
                       Body = body
                   })
                   {
                       smtp.Send(mess);
                   }

                   return View();
               }
               catch (Exception)
               {
                   /*ViewBag.Error = "Erreur mail";
               }

               return RedirectToAction("DashClient","Dashboard");

               /*return View();
           }*/


        // GET: Dashboard
        public ActionResult DashClient()
        {
            if (Session["IdUser"] != null)
            {                                           
                int Id = (int)Session["IdUser"];
                var vmclient = new ViewModelReservation();
                vmclient.Reservations = (from reservation in db.Reservation
                                         join client in db.Utilisateur
                                         on reservation.IdUser equals client.IdUser
                                         where client.IdUser == Id
                                         select reservation).ToList();
                              
                return View(vmclient);

            }
            else
            {
                return RedirectToAction("Authentification","Auth");
            }
        }


            

            /*Reservation reservation = new Reservation();*/
            /*reservation = db.Reservation.Include().ToList().FirstOrDefault(m => m.IdClient == utilisateur.IdUser);*/

            /*lister les données et envoyer sur la page. il faut plutot envoyer les données de la réservation
            inclut les autres informations.*/

            /* var reservations = db.Reservation.Include(r => r.Client).Include(r => r.Salle).Include(r => r.Utilisateur).Where(m => m.IdClient == utilisateur.IdUser);
            return View(vmclient);
                                   
        }*/
      

        //GET: DahGes
        public ActionResult DashGes()
        {
            if (Session["IdUser"] != null)
            {
                      
           

                int Id = (int)Session["IdUser"];
                var ges = db.Gestionnaire.Where(a => a.IdUser.Equals(Id)).FirstOrDefault();
                var vmges = new ViewModelSR();
            vmges.Salles = (from salle in db.Salle where salle.IdGes == ges.IdGes /*authController.svgIdGes*/ select salle).ToList();
            /*var idQuerry = from ids in db.Salle where ids.IdGes == authController.svgIdGes select ids;*/
            vmges.Reservations = (from reservation in db.Reservation
                                                     join salle in db.Salle 
                                                     on reservation.IdSalle equals salle.IdSalle
                                                     where salle.IdGes == ges.IdGes/*authController.svgIdGes*/
                                  select reservation).ToList();
            
           /* var reservations = db.Reservation.Include(r => r.Client).Include(r => r.Salle).Include(r => r.Utilisateur).Where(m => m.IdSalle == authController.svgIdGes);*/
           /* utilisateur = from usere in db.Utilisateur where  utilisateur. == authController.svgIdGes select usere;  ceci peut marcher*/
          
            return View(vmges);
               
            }
            else
            {
                return RedirectToAction("Authentification", "Auth");
            }


           
        }

        //GET: DahCom
        public ActionResult DashCom()
        {
            if (Session["IdUser"] != null)
            {
          

            var vmcom = new ViewModel();
            vmcom.Utilisateurs = (from user in db.Utilisateur select user).ToList(); // Récupère les utilisateurs depuis la base de données
            vmcom.Salles = (from salle in db.Salle select salle).ToList();// Récupère les salles depuis la base de données
         // Récupère les societes depuis la base de données
            vmcom.Reservations = (from reservation in db.Reservation select reservation).ToList();// Récupère les reservations depuis la base de données
            vmcom.Clients= (from client in db.Client select client).ToList();
            vmcom.Gestionnaires = (from gestionnaire in db.Gestionnaire select gestionnaire).ToList();
            vmcom.Commercials = (from commercial in db.Commercial select commercial).ToList();

                return View(vmcom);

            }
            else
            {
                return RedirectToAction("Authentification", "Auth");
            }
            
        }

    }
}