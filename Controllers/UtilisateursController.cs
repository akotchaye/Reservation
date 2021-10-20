using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Projet.Akotchaye.App_Data;

namespace Projet.Akotchaye.Controllers
{
    public class UtilisateursController : Controller
    {
        public string code { get; set; }
       
        public UtilisateursController()
        {

        }
        private GESRESEntities db = new GESRESEntities();

        // GET: Utilisateurs
        public ActionResult Index()
        {
            return View(db.Utilisateur.ToList());
        }

        // GET: Utilisateurs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utilisateur utilisateur = db.Utilisateur.Find(id);
            if (utilisateur == null)
            {
                return HttpNotFound();
            }
            return View(utilisateur);
        }

        // GET: Utilisateurs/Create
        public ActionResult Create()
        {

            return View();
        }
        Boolean PasswordChecked(string string1,string string2)
        {
            return string1 == string2;
        }

        // POST: Utilisateurs/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        public ActionResult Create(string Username, string Pwd1, string Pwd2, string Nom, string Prenoms, string Tel, string Email, string Adresse,string Libelle)
        {
            if (ModelState.IsValid && PasswordChecked(Pwd1, Pwd2))
            {
                var utilisateur = new Utilisateur();
                utilisateur.Username = Username;
                utilisateur.Pwd = Pwd1;
                utilisateur.Nom = Nom;
                utilisateur.Prenoms = Prenoms;
                utilisateur.Tel = Tel;
                utilisateur.Email = Email;
                utilisateur.Adresse = Adresse;

                if (Libelle == "client")
                {
                    utilisateur.Libelle = "client";

                    db.Utilisateur.Add(utilisateur);
                    db.SaveChanges();

                    DateTime utcDate = DateTime.UtcNow;
                    var client = new Client();
                    client.DateClient = utcDate;
                    client.IdUser = utilisateur.IdUser;
                    db.Client.Add(client);
                    db.SaveChanges();
                }
                else
                {
                    utilisateur.Libelle = "gestionnaire";
                    db.Utilisateur.Add(utilisateur);
                    db.SaveChanges();

                    DateTime utcDate = DateTime.UtcNow;
                    var gestionnaire = new Gestionnaire();
                    gestionnaire.DateGes = utcDate;
                    gestionnaire.IdUser = utilisateur.IdUser;
                    db.Gestionnaire.Add(gestionnaire);
                    db.SaveChanges();

                }

                return RedirectToAction("Authentification","Auth");
            }
                           
            return View();
            
        }
        public ActionResult EnrCom()
        {
            Random random = new Random();
            code = random.Next(9999999, 999999999).ToString();
            ViewBag.code = code.ToString();
           
           try
            {

                var senderEmail = new MailAddress("akotchaybatcho@outlook.fr", "Gestion&réservations de salles");
                var receiverEmail = new MailAddress("akotchaybatcho@gmail.com");
                var password = "J'@$$1#e";
                var sub = "Tentative de création de compte Commercial";
                var body = " code d'authentification : " + code;

                var smtp = new SmtpClient
                {
                    Host = "smtp-mail.outlook.com"/*"smtp.gmail.com"*/,
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
                ViewBag.Error = "Erreur mail";
            }

            return View("erreur mail")/*new HttpStatusCodeResult(/*HttpStatusCode.BadRequest)*/;
        }


        [HttpPost]
        public ActionResult EnrCom(string Username, string Pwd1, string Pwd2, string Nom, string Prenoms, string Tel, string Email , string Adresse, string Code ,string VBcode )
        {
            
            if (ModelState.IsValid && PasswordChecked(Pwd1, Pwd2) && (Code == VBcode)) {

                Utilisateur utilisateur = new Utilisateur();
                utilisateur.Username = Username;
                utilisateur.Pwd = Pwd1;
                utilisateur.Nom = Nom;
                utilisateur.Prenoms = Prenoms;
                utilisateur.Tel = Tel;
                utilisateur.Email = Email;
                utilisateur.Libelle ="commercial";
                utilisateur.Adresse = Adresse;


                db.Utilisateur.Add(utilisateur);
                try
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
                }

                DateTime utcDate = DateTime.UtcNow;
                var commercial = new Commercial();
                commercial.DateCom = utcDate;
                commercial.IdUser = utilisateur.IdUser;
                commercial.Code = Code;
                db.Commercial.Add(commercial);
                /*db.SaveChanges();*/
                try
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
                }
                

                return RedirectToAction("Authentification","Auth");
            }
            return View();
        }

      /*  try
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


        // GET: Utilisateurs/Edit/5
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
            Utilisateur utilisateur = db.Utilisateur.Find(id);
            if (utilisateur == null)
            {
                return HttpNotFound();
            }
            return View(utilisateur);
        }

        // POST: Utilisateurs/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int IdUser,string Username, string Pwd, string Nom, string Prenoms, string Tel, string Email, string Adresse, string Libelle)
        {
            if (ModelState.IsValid)
            {
                var utilisateur = new Utilisateur();
                utilisateur.IdUser = IdUser;
                utilisateur.Username = Username;
                utilisateur.Pwd = Pwd;
                utilisateur.Nom = Nom;
                utilisateur.Prenoms = Prenoms;
                utilisateur.Tel = Tel;
                utilisateur.Email = Email;
                utilisateur.Libelle =Libelle;
                utilisateur.Adresse = Adresse;

                db.Entry(utilisateur).State = EntityState.Modified;
               db.SaveChanges();

                /*  switch (Libelle)
                  {
                      case "client":
                          Client cli = new Client();
                          cli = db.Client.FirstOrDefault(m => m.IdUser == IdUser);
                          var client = new Client();
                          client.IdClient = cli.IdClient;
                          client.DateClient = DateTime.UtcNow;
                          client.IdUser = cli.IdUser;
                          db.Entry(client).State = EntityState.Modified;
                          db.SaveChanges();
                          return RedirectToAction("DashCom", "Dashboard");

                      case "gestionnaire":
                          Gestionnaire ges = new Gestionnaire();
                          ges = db.Gestionnaire.FirstOrDefault(m => m.IdUser == IdUser);
                          var gestionnaire = new Gestionnaire();
                          gestionnaire.IdGes = ges.IdGes;
                          gestionnaire.DateGes = DateTime.UtcNow;
                          gestionnaire.IdUser = ges.IdUser;
                          db.Entry(gestionnaire).State = EntityState.Modified;
                          db.SaveChanges();
                          return RedirectToAction("DashCom", "Dashboard");

                      case "commercial":
                          // int Id = (int)Session["IdUser"];
                          Commercial com = new Commercial();
                          com = db.Commercial.FirstOrDefault(m => m.IdUser == IdUser);
                          var commercial = new Commercial();
                          commercial.IdCom = com.IdCom;
                          commercial.DateCom = DateTime.UtcNow;
                          commercial.IdUser = com.IdUser;
                          commercial.Code = com.Code;
                          db.Entry(commercial).State = EntityState.Modified;
                          db.SaveChanges();
                          return RedirectToAction("DashCom", "Dashboard");


                      default:
                          return View(utilisateur);
                  }*/
                return RedirectToAction("DashCom", "Dashboard");

            }
            return View();
        }

        // GET: Utilisateurs/Delete/5
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
            Utilisateur utilisateur = db.Utilisateur.Find(id);
            if (utilisateur == null)
            {
                return HttpNotFound();
            }
            return View(utilisateur);
        }

        // POST: Utilisateurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Utilisateur utilisateur = db.Utilisateur.Find(id);
            db.Utilisateur.Remove(utilisateur);

            Client client = db.Client.Where(a => a.IdUser.Equals(id)).FirstOrDefault();
            if (client != null)
            {
                db.Client.Remove(client);
            }
            Commercial commercial = db.Commercial.Where(a => a.IdUser.Equals(id)).FirstOrDefault();
            if (commercial != null)
            {
                db.Commercial.Remove(commercial);
                db.SaveChanges();
                return RedirectToAction("Authentification", "Auth");
            }
            Gestionnaire gestionnaire = db.Gestionnaire.Where(a => a.IdUser.Equals(id)).FirstOrDefault();
            if (gestionnaire != null)
            {
                db.Gestionnaire.Remove(gestionnaire);
            }
            Salle salle = db.Salle.Where(a => a.IdUser.Equals(id)).FirstOrDefault();
            if (salle != null)
            {
                db.Salle.Remove(salle);
            }

            Reservation reservation = db.Reservation.Where(a => a.IdUser.Equals(id)).FirstOrDefault();
            if (reservation != null)
            {
                db.Reservation.Remove(reservation);
            }
           

            db.SaveChanges();
            return RedirectToAction("DashCom","Dashboard");
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
