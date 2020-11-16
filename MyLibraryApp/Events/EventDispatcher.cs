using System;

namespace MyLibraryApp.Events
{
    public class EventDispatcher
    {
        public static EventDispatcher Instance
        {
            get
            {
                return new EventDispatcher();
            }
        }

        public void Dispatch(INotification notification)
        {
        }
    }
}
