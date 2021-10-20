using Projet.Akotchaye.App_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Projet.Akotchaye.Controllers
{
    public class AnnulerController : Controller
    {
        // GET: Annuler
        private GESRESEntities db = new GESRESEntities();
        public ActionResult Annuler(int? id)/*identifiant de la salle*/
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
            Utilisateur utilisateur = new Utilisateur();
            utilisateur = db.Utilisateur.FirstOrDefault(m => m.IdUser == reservation.IdUser);

            try
            {

                var senderEmail = new MailAddress("akotchaybatcho@outlook.fr", "Gestion&réservations de salles");
                var receiverEmail = new MailAddress("akotchaybatcho@gmail.com");/*utilisateur.Email*/
                var password = "J'@$$1#e";
                var sub = "Demande d'annulation de réservation";
                var body = "Identifiant de la reservation: " + reservation.IdRes + ".  Nom du client: " + utilisateur.Nom + ".  prenoms du client: " + utilisateur.Prenoms + ".  Email du client: " + utilisateur.Email + ".  Datedebut: " + reservation.DatedebutRes + ".  Datefin: " + reservation.DatefinRes;

                var smtp = new SmtpClient
                {
                    Host = "smtp-mail.outlook.com" /*"smtp.gmail.com"*/,
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

                return RedirectToAction("DashClient", "Dashboard");
            }
            catch (Exception)
            {
                /*ViewBag.Error = "Erreur mail";*/
            }

            return RedirectToAction("erreur mail");

           
        }
    }
}