using Projet.Akotchaye.App_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projet.Akotchaye.Models
{
    public class ViewModel
    {
        public ViewModel()
        {

        }
        public List<Utilisateur> Utilisateurs { get; set; }
        public List<Reservation> Reservations { get; set; }
        public List<Salle> Salles { get; set; }
       
        public List<Client> Clients { get; set; }
        public List<Commercial> Commercials { get; set; }
        public List<Gestionnaire> Gestionnaires { get; set; }




    }
}