using MyLibraryApp.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyLibraryApp.Services
{
    public class BookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public bool IsCurrentlyAvailable(int bookId)
        {
            var b = _bookRepository.Get(bookId);

            var currenteDate = DateTimeOffset.Now;
            var isAvailable = true;

            foreach (var r in b.Reservations)
            {
                if ((currenteDate > r.From && currenteDate < r.To) || (currenteDate > r.From && currenteDate < r.To))
                {
                    isAvailable = false;
                }
            }
            return isAvailable;
        }
    }
}
