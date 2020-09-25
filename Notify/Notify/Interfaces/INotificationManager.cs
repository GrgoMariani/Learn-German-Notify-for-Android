using System;
using System.Collections.Generic;
using System.Text;

namespace Notify.Interfaces
{
    public interface INotificationManager
    {
        event EventHandler NotificationReceived;

        void Initialize();

        int ScheduleNotification(int id);

        void ReceiveNotification(string title, string message);
    }

}
