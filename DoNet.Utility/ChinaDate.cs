// Decompiled with JetBrains decompiler
// Type: DoNet.Utility.ChinaDate
// Assembly: DoNet.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD5DA16A-6F4C-4458-AC23-48055BBFFF84
// Assembly location: D:\Dian.Web\bin\DoNet.Utility.dll

using System;
using System.Collections;
using System.Globalization;

namespace DoNet.Utility
{
  public static class ChinaDate
  {
    private static readonly ChineseLunisolarCalendar China = new ChineseLunisolarCalendar();
    private static readonly Hashtable GHoliday = new Hashtable();
    private static readonly Hashtable NHoliday = new Hashtable();
    private static readonly string[] Jq = new string[24]
    {
      "小寒",
      "大寒",
      "立春",
      "雨水",
      "惊蛰",
      "春分",
      "清明",
      "谷雨",
      "立夏",
      "小满",
      "芒种",
      "夏至",
      "小暑",
      "大暑",
      "立秋",
      "处暑",
      "白露",
      "秋分",
      "寒露",
      "霜降",
      "立冬",
      "小雪",
      "大雪",
      "冬至"
    };
    private static readonly int[] JqData = new int[24]
    {
      0,
      21208,
      43467,
      63836,
      85337,
      107014,
      128867,
      150921,
      173149,
      195551,
      218072,
      240693,
      263343,
      285989,
      308563,
      331033,
      353350,
      375494,
      397447,
      419210,
      440795,
      462224,
      483532,
      504758
    };

    static ChinaDate()
    {
      ChinaDate.GHoliday.Add((object) "0101", (object) "元旦");
      ChinaDate.GHoliday.Add((object) "0214", (object) "情人节");
      ChinaDate.GHoliday.Add((object) "0305", (object) "雷锋日");
      ChinaDate.GHoliday.Add((object) "0308", (object) "妇女节");
      ChinaDate.GHoliday.Add((object) "0312", (object) "植树节");
      ChinaDate.GHoliday.Add((object) "0315", (object) "消费者权益日");
      ChinaDate.GHoliday.Add((object) "0401", (object) "愚人节");
      ChinaDate.GHoliday.Add((object) "0501", (object) "劳动节");
      ChinaDate.GHoliday.Add((object) "0504", (object) "青年节");
      ChinaDate.GHoliday.Add((object) "0601", (object) "儿童节");
      ChinaDate.GHoliday.Add((object) "0701", (object) "建党节");
      ChinaDate.GHoliday.Add((object) "0801", (object) "建军节");
      ChinaDate.GHoliday.Add((object) "0910", (object) "教师节");
      ChinaDate.GHoliday.Add((object) "1001", (object) "国庆节");
      ChinaDate.GHoliday.Add((object) "1224", (object) "平安夜");
      ChinaDate.GHoliday.Add((object) "1225", (object) "圣诞节");
      ChinaDate.NHoliday.Add((object) "0101", (object) "春节");
      ChinaDate.NHoliday.Add((object) "0115", (object) "元宵节");
      ChinaDate.NHoliday.Add((object) "0505", (object) "端午节");
      ChinaDate.NHoliday.Add((object) "0815", (object) "中秋节");
      ChinaDate.NHoliday.Add((object) "0909", (object) "重阳节");
      ChinaDate.NHoliday.Add((object) "1208", (object) "腊八节");
    }

    public static string GetChinaDate(DateTime dt)
    {
      if (dt > ChinaDate.China.MaxSupportedDateTime || dt < ChinaDate.China.MinSupportedDateTime)
      {
        DateTime supportedDateTime = ChinaDate.China.MinSupportedDateTime;
        string str1 = supportedDateTime.ToString("yyyy-MM-dd");
        supportedDateTime = ChinaDate.China.MaxSupportedDateTime;
        string str2 = supportedDateTime.ToString("yyyy-MM-dd");
        throw new Exception(string.Format("日期超出范围！必须在{0}到{1}之间！", (object) str1, (object) str2));
      }
      string chinaDate = string.Format("{0}{1}", (object) ChinaDate.GetMonth(dt), (object) ChinaDate.GetDay(dt));
      string solarTerm = ChinaDate.GetSolarTerm(dt);
      if (solarTerm != "")
        chinaDate = chinaDate + " (" + solarTerm + ")";
      string holiday = ChinaDate.GetHoliday(dt);
      if (holiday != "")
        chinaDate = chinaDate + " " + holiday;
      string chinaHoliday = ChinaDate.GetChinaHoliday(dt);
      if (chinaHoliday != "")
        chinaDate = chinaDate + " " + chinaHoliday;
      return chinaDate;
    }

    public static string GetYear(DateTime dt)
    {
      int sexagenaryYear = ChinaDate.China.GetSexagenaryYear(dt);
      int year = ChinaDate.China.GetYear(dt);
      int celestialStem = ChinaDate.China.GetCelestialStem(sexagenaryYear);
      int terrestrialBranch = ChinaDate.China.GetTerrestrialBranch(sexagenaryYear);
      return string.Format("[{1}]{2}{3}{0}", (object) year, (object) " 鼠牛虎兔龙蛇马羊猴鸡狗猪"[terrestrialBranch], (object) " 甲乙丙丁戊己庚辛壬癸"[celestialStem], (object) " 子丑寅卯辰巳午未申酉戌亥"[terrestrialBranch]);
    }

    public static string GetMonth(DateTime dt)
    {
      int year = ChinaDate.China.GetYear(dt);
      int month = ChinaDate.China.GetMonth(dt);
      int leapMonth = ChinaDate.China.GetLeapMonth(year);
      bool flag = month == leapMonth;
      if (leapMonth != 0 && month >= leapMonth)
        --month;
      string str1 = "正二三四五六七八九十";
      string str2 = flag ? "闰" : "";
      return (month > 10 ? (month != 11 ? str2 + "腊" : str2 + "十一") : str2 + str1.Substring(month - 1, 1)) + "月";
    }

    public static string GetDay(DateTime dt)
    {
      int dayOfMonth = ChinaDate.China.GetDayOfMonth(dt);
      string str1 = "初十廿三";
      string str2 = "一二三四五六七八九十";
      string day;
      switch (dayOfMonth)
      {
        case 20:
          day = "二十";
          break;
        case 30:
          day = "三十";
          break;
        default:
          day = str1.Substring((dayOfMonth - 1) / 10, 1) + str2.Substring((dayOfMonth - 1) % 10, 1);
          break;
      }
      return day;
    }

    public static string GetSolarTerm(DateTime dt)
    {
      DateTime dateTime = new DateTime(1900, 1, 6, 2, 5, 0);
      string solarTerm = "";
      int year = dt.Year;
      for (int index = 1; index <= 24; ++index)
      {
        double num = 525948.76 * (double) (year - 1900) + (double) ChinaDate.JqData[index - 1];
        if (dateTime.AddMinutes(num).DayOfYear == dt.DayOfYear)
          solarTerm = ChinaDate.Jq[index - 1];
      }
      return solarTerm;
    }

    public static string GetHoliday(DateTime dt)
    {
      string holiday = "";
      Hashtable gholiday = ChinaDate.GHoliday;
      int num = dt.Month;
      string str1 = num.ToString("00");
      num = dt.Day;
      string str2 = num.ToString("00");
      string key = str1 + str2;
      object obj = gholiday[(object) key];
      if (obj != null)
        holiday = obj.ToString();
      return holiday;
    }

    public static string GetChinaHoliday(DateTime dt)
    {
      string chinaHoliday = "";
      int year = ChinaDate.China.GetYear(dt);
      int month = ChinaDate.China.GetMonth(dt);
      int leapMonth = ChinaDate.China.GetLeapMonth(year);
      int dayOfMonth = ChinaDate.China.GetDayOfMonth(dt);
      if (ChinaDate.China.GetDayOfYear(dt) == ChinaDate.China.GetDaysInYear(year))
        chinaHoliday = "除夕";
      else if (leapMonth != month)
      {
        if (leapMonth != 0 && month >= leapMonth)
          --month;
        object obj = ChinaDate.NHoliday[(object) (month.ToString("00") + dayOfMonth.ToString("00"))];
        if (obj != null)
          chinaHoliday = !(chinaHoliday == "") ? chinaHoliday + " " + obj : obj.ToString();
      }
      return chinaHoliday;
    }

    public static string GetWeek(DateTime dt)
    {
      switch (dt.DayOfWeek)
      {
        case DayOfWeek.Sunday:
          return "星期日";
        case DayOfWeek.Monday:
          return "星期一";
        case DayOfWeek.Tuesday:
          return "星期二";
        case DayOfWeek.Wednesday:
          return "星期三";
        case DayOfWeek.Thursday:
          return "星期四";
        case DayOfWeek.Friday:
          return "星期五";
        case DayOfWeek.Saturday:
          return "星期六";
        default:
          return "";
      }
    }
  }
}
