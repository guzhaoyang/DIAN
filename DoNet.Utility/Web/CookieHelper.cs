// Decompiled with JetBrains decompiler
// Type: DoNet.Utility.Web.CookieHelper
// Assembly: DoNet.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD5DA16A-6F4C-4458-AC23-48055BBFFF84
// Assembly location: D:\Dian.Web\bin\DoNet.Utility.dll

using System;
using System.Web;

namespace DoNet.Utility.Web
{
  public class CookieHelper
  {
    public static void ClearCookie(string cookiename)
    {
      HttpCookie cookie = HttpContext.Current.Request.Cookies[cookiename];
      if (cookie == null)
        return;
      cookie.Expires = DateTime.Now.AddYears(-3);
      HttpContext.Current.Response.Cookies.Add(cookie);
    }

    public static string GetCookieValue(string cookiename)
    {
      HttpCookie cookie = HttpContext.Current.Request.Cookies[cookiename];
      string cookieValue = string.Empty;
      if (cookie != null)
        cookieValue = HttpUtility.UrlDecode(cookie.Value);
      return cookieValue;
    }

    public static void SetCookie(string cookiename, string cookievalue) => CookieHelper.SetCookie(cookiename, cookievalue, DateTime.Now.AddDays(1.0));

    public static void SetCookie(string cookiename, string cookievalue, DateTime expires) => HttpContext.Current.Response.Cookies.Add(new HttpCookie(cookiename)
    {
      Value = cookievalue,
      Expires = expires
    });
  }
}
