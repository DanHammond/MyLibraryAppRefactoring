using System;
using System.Collections.Generic;
using System.Text;

namespace MyLibraryApp.Entities
{
    public class Member
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsCancelled { get; set; }
        public List<Reservation> Reservations { get; set; } = new List<Reservation>();
        public List<Reminder> Reminders { get; set; } = new List<Reminder>();
    }
}
