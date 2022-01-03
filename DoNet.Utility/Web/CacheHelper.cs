// Decompiled with JetBrains decompiler
// Type: DoNet.Utility.Web.CacheHelper
// Assembly: DoNet.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD5DA16A-6F4C-4458-AC23-48055BBFFF84
// Assembly location: D:\Dian.Web\bin\DoNet.Utility.dll

using System;
using System.Web;
using System.Web.Caching;

namespace DoNet.Utility.Web
{
  public class CacheHelper
  {
    public static object GetCache(string cacheKey) => HttpRuntime.Cache[cacheKey];

    public static void SetCache(string cacheKey, object objObject) => HttpRuntime.Cache.Insert(cacheKey, objObject);

    public static void SetCache(
      string cacheKey,
      object objObject,
      DateTime absoluteExpiration,
      TimeSpan slidingExpiration)
    {
      HttpRuntime.Cache.Insert(cacheKey, objObject, (CacheDependency) null, absoluteExpiration, slidingExpiration);
    }
  }
}
