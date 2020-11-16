using System;
using System.Collections.Generic;
using System.Text;

namespace MyLibraryApp.Entities
{
    public class Reminder
    {
        public DateTimeOffset Date { get; set; }
        public int BookId { get; set; }
    }
}
