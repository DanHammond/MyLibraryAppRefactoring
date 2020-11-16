using MyLibraryApp.Entities;
using System.Collections.Generic;

namespace MyLibraryApp.Repository
{
    public interface IMemberRepository
    {
        Member Get(int memberId);
        Reservation AddReservation(Reservation reservation);
        List<Reservation> GetAllReservations();
    }
}
