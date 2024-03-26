using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace TopLearn.Core.Convertors
{
    public static class DateConvertor
    {
        public static string ToShamsi(this DateTime value)
        {
            PersianCalendar persianCalendar = new PersianCalendar();
            return persianCalendar.GetYear(value) + "/" + persianCalendar.GetMonth(value).ToString("00") + "/" + persianCalendar.GetDayOfMonth(value).ToString("00");
        }
    }
}
