using Projet.Akotchaye.App_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projet.Akotchaye.Models
{
    public class ViewModelReservation
    {
        public ViewModelReservation()
        {

        }
        public List<Reservation> Reservations { get; set; }
    }
}