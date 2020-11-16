using MyLibraryApp.Entities;
using MyLibraryApp.Events;
using MyLibraryApp.Repository;
using System;
using System.Linq;

namespace MyLibraryApp.Services
{
    public class BookReservationService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMemberRepository _memberRepository;

        public BookReservationService(
            IBookRepository bookRepository,
            IMemberRepository memberRepository)
        {
            _bookRepository = bookRepository;
            _memberRepository = memberRepository;
        }


        public Reservation ReserveBook(int member, int itemId, DateTimeOffset from, DateTimeOffset to)
        {
            bool isFree = true, bookingAvailable = true;

            var res = new Reservation
            {
                BookId = itemId,
                From = from,
                To = to,
                MemberId = member
                // IsDeleted = false
            };

            
            var b = _bookRepository.Get(itemId);
            // if book is already reserved during this time
            foreach (var r in b.Reservations)
            {
                if (to > r.From)
                {
                    if (to < r.To)
                    {
                        isFree = false;
                    }


                }


                else if (from > r.From)
                {
                    if (from < r.To)
                    {
                        isFree = false;
                    }
                }
            }

                if (isFree && _memberRepository.Get(member).Reservations.Count(r => r.From > DateTime.Now) < 3)
                {
                    _memberRepository.AddReservation(res);
                    EventDispatcher.Instance.Dispatch(new ReservationCreatedEvent { BookId = res.BookId });
                }
            else
            {
                res = null;
            }

            if (res != null)
            {
                  _memberRepository.Get(member).Reminders.Add(new Reminder { Date = to.AddDays(-1), BookId = itemId });


            }

            return res;

        }

        public Reservation CancelReservation(int memberId, int bookId)
        {
            var member = _memberRepository.Get(memberId);
            var reservation = member.Reservations.SingleOrDefault(r => r.BookId == bookId && r.To < DateTimeOffset.Now);
            if (reservation != null)
            {
                member.Reservations.Remove(reservation); member.Reminders.Remove(member.Reminders.Single(r => r.BookId == bookId));
                EventDispatcher.Instance.Dispatch(new ReservationCancelledEvent { BookId = reservation.BookId });

                return reservation;
            }
            throw new Exception("User does not have reservation for this book!");
        }
    }
}
