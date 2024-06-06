using System;
using System.Linq;
using System.Runtime.InteropServices;


public enum Location
{
    NewYork,
    London,
    Paris
}

public enum AlertLevel
{
    Early,
    Standard,
    Late
}

public static class Appointment
{
    public static DateTime ShowLocalTime(DateTime dtUtc) => dtUtc.ToLocalTime();

    public static DateTime Schedule(string appointmentDateDescription, Location location)
    {
        string timeZoneId;
        
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            timeZoneId = location switch
            {
                Location.NewYork => "Eastern Standard Time",
                Location.London => "GMT Standard Time",
                Location.Paris => "W. Europe Standard Time",
            };
        }
        else
        {
            timeZoneId = location switch
            {
                Location.NewYork => "America/New_York",
                Location.London => "Europe/London",
                Location.Paris => "Europe/Paris",
            };
        }

        return TimeZoneInfo.ConvertTimeToUtc(
            DateTime.Parse(appointmentDateDescription), TimeZoneInfo.FindSystemTimeZoneById(timeZoneId));
    }

    public static DateTime GetAlertTime(DateTime appointment, AlertLevel alertLevel) =>
        alertLevel switch
        {
            AlertLevel.Early => appointment - new TimeSpan(1, 0, 0, 0),
            AlertLevel.Standard => appointment - new TimeSpan(1, 45, 0),
            AlertLevel.Late => appointment - new TimeSpan(0, 30, 0)
        };
    public static bool HasDaylightSavingChanged(DateTime dt, Location location)
    {
        throw new NotImplementedException("Please implement the (static) Appointment.HasDaylightSavingChanged() method");
    }

    public static DateTime NormalizeDateTime(string dtStr, Location location)
    {
        throw new NotImplementedException("Please implement the (static) Appointment.NormalizeDateTime() method");
    }
}
