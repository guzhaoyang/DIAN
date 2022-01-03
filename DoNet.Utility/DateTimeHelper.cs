// Decompiled with JetBrains decompiler
// Type: DoNet.Utility.DateTimeHelper
// Assembly: DoNet.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD5DA16A-6F4C-4458-AC23-48055BBFFF84
// Assembly location: D:\Dian.Web\bin\DoNet.Utility.dll

using System;
using System.Globalization;

namespace DoNet.Utility
{
  public class DateTimeHelper
  {
    public static int SecondToMinute(int second) => Convert.ToInt32(Math.Ceiling((Decimal) second / 60M));

    public static int GetMonthLastDate(int year, int month) => new DateTime(year, month, new GregorianCalendar().GetDaysInMonth(year, month)).Day;

    public static string DateDiff(DateTime dateTime1, DateTime dateTime2)
    {
      string str = (string) null;
      try
      {
        TimeSpan timeSpan = dateTime2 - dateTime1;
        if (timeSpan.Days >= 1)
          str = dateTime1.Month.ToString() + "月" + (object) dateTime1.Day + "日";
        else
          str = timeSpan.Hours <= 1 ? timeSpan.Minutes.ToString() + "分钟前" : timeSpan.Hours.ToString() + "小时前";
      }
      catch
      {
      }
      return str;
    }
  }
}
