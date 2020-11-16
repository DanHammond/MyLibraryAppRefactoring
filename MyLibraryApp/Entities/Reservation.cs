using System;
using System.Collections.Generic;
using System.Text;

namespace MyLibraryApp.Entities
{
    public class Reservation
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int BookId { get; set; }
        public DateTimeOffset From { get; set; }
        public DateTimeOffset To { get; set; }
    }
}
