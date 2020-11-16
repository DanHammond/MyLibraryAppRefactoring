namespace MyLibraryApp.Events
{
    public class ReservationCreatedEvent : INotification
    {
        public int BookId { get; set; }
    }
}
