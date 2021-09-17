using System;

namespace WinForm_AIO.Common
{
    public static class WeekHelper
    {
        public static string GetWeekOfDate()
        {
            string[] weekdays = { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
            string week = weekdays[Convert.ToInt32(DateTime.Now.DayOfWeek)];
            return week;
        }
    }
}
