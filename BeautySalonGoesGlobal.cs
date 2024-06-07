using System;
using System.Globalization;
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
    private static TimeZoneInfo GetTimeZoneId(Location location)
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

        return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
    }

    private static CultureInfo GetCultureInfo(Location location) =>
         location switch
        {
            Location.London => new CultureInfo("en-GB"),
            Location.NewYork => new CultureInfo("en-US"),
            Location.Paris => new CultureInfo("fr-FR"),
            _ => CultureInfo.CurrentCulture
        };
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
            DateTime.Parse(appointmentDateDescription), GetTimeZoneId(location));
    }

    public static DateTime GetAlertTime(DateTime appointment, AlertLevel alertLevel) =>
        alertLevel switch
        {
            AlertLevel.Early => appointment - new TimeSpan(1, 0, 0, 0),
            AlertLevel.Standard => appointment - new TimeSpan(1, 45, 0),
            AlertLevel.Late => appointment - new TimeSpan(0, 30, 0)
        };

    public static bool HasDaylightSavingChanged(DateTime dt, Location location) =>
        GetTimeZoneId(location).IsDaylightSavingTime(dt) || GetTimeZoneId(location).IsDaylightSavingTime(dt.AddDays(-7));


    public static DateTime NormalizeDateTime(string dtStr, Location location) =>
        DateTime.TryParse(dtStr, GetCultureInfo(location), out DateTime parsedDateTime) ? 
            parsedDateTime : 
            new DateTime(1, 1, 1, 0, 0, 0);

}
