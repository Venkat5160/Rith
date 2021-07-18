using System;

namespace SDAE.Common
{
    public static class Formatter
    {
        public static DateTime GetUtcDateTime()
        {
            return DateTime.UtcNow;
        }

        public static string GetFormattedDate(this DateTime date)
        {
            return ConvertToLocalTime(date).ToString(GetDateFormatString());
        }
        public static string GetNullableFormattedDate(DateTime? dateVal)
        {
            if (dateVal != null)
                return ConvertToLocalTime(dateVal.Value).ToString(GetDateFormatString());
            else
                return GetDefaultDate();
        }
        public static string GetFormattedDateTime(this DateTime dateTime)
        {
            return ConvertToLocalTime(dateTime).ToString(GetFullDateFormatString());
        }
        public static string GetNullableFormattedDateTime(this DateTime? dateTime)
        {
            if (dateTime != null)
                return ConvertToLocalTime(dateTime.Value).ToString(GetFullDateFormatString());
            else
                return GetDefaultDate();
        }

        public static DateTime ConvertToLocalTime(this DateTime utcDate)
        {
            DateTime runtimeUtcDate = DateTime.SpecifyKind(utcDate, DateTimeKind.Utc);

            DateTime localDate = runtimeUtcDate.ToLocalTime();

            return localDate;
        }

        public static string LocalFormattedDate(this DateTime? utcDate)
        {
            if (utcDate != null)
            {
                DateTime runtimeUtcDate = DateTime.SpecifyKind(utcDate.GetValueOrDefault(), DateTimeKind.Utc);
                DateTime localDate = runtimeUtcDate.ToLocalTime();
                return localDate.ToString(GetFullDateFormatString());
            }
            else
            {
                return GetDefaultDate();
            }
        }

        public static string GetDateFormatString()
        {
            return "dd-MMM-yyyy";
        }
        public static string GetFullDateFormatString()
        {
            return "dd-MMM-yyyy hh':'mm tt";
        }

        public static string GetDefaultDate()
        {
            return "";
        }
    }
}
