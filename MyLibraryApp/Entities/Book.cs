using System;
using System.Collections.Generic;
using System.Text;

namespace MyLibraryApp.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Reservation> Reservations { get; set; } = new List<Reservation>();

    }
}
