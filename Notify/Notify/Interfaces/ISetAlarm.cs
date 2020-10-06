using System;
using System.Collections.Generic;
using System.Text;

namespace Notify.Interfaces
{
    /// <summary>
    /// https://stackoverflow.com/questions/61079610/how-to-create-a-xamarin-foreground-service
    /// https://github.com/NeilMalcolm/Xamarin.Forms-Alarm-App
    /// </summary>
    public interface  ISetAlarm
    {
        int SetAlarm(int hour, int minute, string difficulty);

        void CancelAlarm(int id);
    }
}
