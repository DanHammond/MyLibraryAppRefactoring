using MyLibraryApp.Entities;
using MyLibraryApp.Events;
using MyLibraryApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyLibraryApp.Services
{
    public class MembershipStatus
    {
        public bool CanMakeReservation { get; set; }
        public int RewardPoints { get; set; }
        public IEnumerable<Reminder> UpcomingReminders { get; internal set; }
    }
    public class MembershipService
    {
        private readonly IMemberRepository _memberRepository;        

        public MembershipService(
            IMemberRepository memberRepository
            )
        {
            _memberRepository = memberRepository;
        }

        public MembershipStatus GetStaus(int memberId)
        {
            var member = _memberRepository.Get(memberId);
            var reservations = _memberRepository.GetAllReservations()
                .Where(r => r.MemberId == memberId)
                .ToList();

            return new MembershipStatus
            {
                CanMakeReservation = reservations.Count(r => r.From > DateTimeOffset.Now) < 3,
                RewardPoints = reservations.Count(),
                UpcomingReminders = member.Reminders.Where(r => r.Date < DateTimeOffset.Now.AddDays(3))
            };

        }

        public void CancelMembership(int memberId)
        {
            var member = _memberRepository.Get(memberId);

            member.IsCancelled = true;

            var reservations = _memberRepository.GetAllReservations()
                .Where(r => r.MemberId == memberId)
                .Where(r => r.To < DateTimeOffset.Now)
                .ToList();

            foreach (var reservation in reservations)
            {
                if (reservation != null)
                {
                    member.Reservations.Remove(reservation);
                    member.Reminders.Remove(member.Reminders.Single(r => r.BookId == reservation.BookId));
                    EventDispatcher.Instance.Dispatch(new ReservationCancelledEvent { BookId = reservation.BookId });
                }
            }
        }
    }
}
