// Decompiled with JetBrains decompiler
// Type: DoNet.Utility.Web.SessionHelper
// Assembly: DoNet.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD5DA16A-6F4C-4458-AC23-48055BBFFF84
// Assembly location: D:\Dian.Web\bin\DoNet.Utility.dll

using System.Web;

namespace DoNet.Utility.Web
{
  public class SessionHelper
  {
    public static object GetSession(string name) => HttpContext.Current.Session[name];

    public static void SetSession(string name, object val)
    {
      HttpContext.Current.Session.Remove(name);
      HttpContext.Current.Session.Add(name, val);
    }

    public static void SetSession(string name, object val, int timeout)
    {
      HttpContext.Current.Session.Remove(name);
      HttpContext.Current.Session.Add(name, val);
      HttpContext.Current.Session.Timeout = timeout;
    }

    public static void ClearSession() => HttpContext.Current.Session.Clear();

    public static void RemoveSession(string name) => HttpContext.Current.Session.Remove(name);

    public static void RemoveAllSession(string name) => HttpContext.Current.Session.RemoveAll();
  }
}
