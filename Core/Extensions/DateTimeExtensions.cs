using System;

namespace Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTimeOffset UtcToday = new DateTimeOffset(DateTime.Today).UtcDateTime;
        public static DateTime LocalToday = DateTime.SpecifyKind(DateTime.Today, DateTimeKind.Unspecified);

        public static string AsLongDateTime(this DateTimeOffset date)
        {
            return date.ToLocalTime().ToString("dd/MM/yyyy h:mmtt");
        }

        public static string AsLocalDate(this DateTimeOffset date)
        {
            return ((DateTimeOffset?)date).AsLocalDate();
        }

        public static string AsLocalDate(this DateTimeOffset? date)
        {
            return date?.ToLocalTime().ToString("dd/MM/yyyy");
        }

        public static string AsLocalDate(this DateTime date)
        {
            return ((DateTime?)date).AsLocalDate();
        }

        public static string AsLocalDate(this DateTime? date)
        {
            return date?.ToString("dd/MM/yyyy");
        }

        public static int DifferenceInMonths(this DateTime fromDate, DateTime tillDate)
        {
            return ((tillDate.Year - fromDate.Year) * 12) + tillDate.Month - fromDate.Month;
        }

        public static string AsFilterValue(this DateTimeOffset date)
        {
            return date.ToISO8601String();
        }

        public static string AsFilterValue(this DateTime date)
        {
            return date.ToDisplayString();
        }

        public static string ToISO8601String(this DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.UtcDateTime.ToString("s") + "Z";
        }

        public static string ToDisplayString(this DateTime dateTime)
        {
            return dateTime.AsLocalDate();
        }
    }
}