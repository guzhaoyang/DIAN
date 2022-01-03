// Decompiled with JetBrains decompiler
// Type: DoNet.Utility.Web.UrlHelper
// Assembly: DoNet.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD5DA16A-6F4C-4458-AC23-48055BBFFF84
// Assembly location: D:\Dian.Web\bin\DoNet.Utility.dll

using System;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Web;

namespace DoNet.Utility.Web
{
  public class UrlHelper
  {
    public static string AddParam(string url, string paramName, string value)
    {
      if (string.IsNullOrEmpty(new Uri(url).Query))
      {
        string str = HttpContext.Current.Server.UrlEncode(value);
        return url + ("?" + paramName + "=" + str);
      }
      string str1 = HttpContext.Current.Server.UrlEncode(value);
      return url + ("&" + paramName + "=" + str1);
    }

    public static string UpdateParam(string url, string paramName, string value)
    {
      string str = paramName + "=";
      int startIndex = url.IndexOf(str) + str.Length;
      int num = url.IndexOf("&", startIndex);
      if (num == -1)
      {
        url = url.Remove(startIndex, url.Length - startIndex);
        url += value;
        return url;
      }
      url = url.Remove(startIndex, num - startIndex);
      url = url.Insert(startIndex, value);
      return url;
    }

    public static void ParseUrl(string url, out string baseUrl, out NameValueCollection nvc)
    {
      if (url == null)
        throw new ArgumentNullException(nameof (url));
      nvc = new NameValueCollection();
      baseUrl = "";
      if (url == "")
        return;
      int length = url.IndexOf('?');
      if (length == -1)
      {
        baseUrl = url;
      }
      else
      {
        baseUrl = url.Substring(0, length);
        if (length == url.Length - 1)
          return;
        foreach (Match match in new Regex("(^|&)?(\\w+)=([^&]+)(&|$)?", RegexOptions.Compiled).Matches(url.Substring(length + 1)))
          nvc.Add(match.Result("$2").ToLower(), match.Result("$3"));
      }
    }
  }
}
