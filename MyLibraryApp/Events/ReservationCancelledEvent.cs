namespace MyLibraryApp.Events
{
    public class ReservationCancelledEvent : INotification
    {
        public int BookId { get; set; }
    }
}
