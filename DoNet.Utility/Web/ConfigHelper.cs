// Decompiled with JetBrains decompiler
// Type: DoNet.Utility.Web.ConfigHelper
// Assembly: DoNet.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD5DA16A-6F4C-4458-AC23-48055BBFFF84
// Assembly location: D:\Dian.Web\bin\DoNet.Utility.dll

using System;
using System.Configuration;

namespace DoNet.Utility.Web
{
  public sealed class ConfigHelper
  {
    public static string GetAppSettings(string key)
    {
      string cacheKey = "AppSettings-" + key;
      object objObject = CacheHelper.GetCache(cacheKey);
      if (objObject == null)
      {
        try
        {
          objObject = (object) ConfigurationManager.AppSettings[key];
          if (objObject != null)
            CacheHelper.SetCache(cacheKey, objObject, DateTime.Now.AddMinutes(180.0), TimeSpan.Zero);
        }
        catch
        {
        }
      }
      return objObject?.ToString();
    }

    public static string GetConfigString(string key) => ConfigHelper.GetAppSettings(key);

    public static bool GetConfigBool(string key)
    {
      bool configBool = false;
      string appSettings = ConfigHelper.GetAppSettings(key);
      if (!string.IsNullOrEmpty(appSettings))
      {
        try
        {
          configBool = bool.Parse(appSettings);
        }
        catch (FormatException ex)
        {
        }
      }
      return configBool;
    }

    public static int GetConfigInt(string key)
    {
      int configInt = 0;
      string appSettings = ConfigHelper.GetAppSettings(key);
      if (!string.IsNullOrEmpty(appSettings))
      {
        try
        {
          configInt = int.Parse(appSettings);
        }
        catch (FormatException ex)
        {
        }
      }
      return configInt;
    }

    public static Decimal GetConfigDecimal(string key)
    {
      Decimal configDecimal = 0M;
      string appSettings = ConfigHelper.GetAppSettings(key);
      if (!string.IsNullOrEmpty(appSettings))
      {
        try
        {
          configDecimal = Decimal.Parse(appSettings);
        }
        catch (FormatException ex)
        {
        }
      }
      return configDecimal;
    }

    public static ConnectionStringSettings GetConnectionStrings(string key)
    {
      string cacheKey = "ConnectionStrings-" + key;
      object objObject = CacheHelper.GetCache(cacheKey);
      if (objObject == null)
      {
        try
        {
          objObject = (object) ConfigurationManager.ConnectionStrings[key];
          if (objObject != null)
            CacheHelper.SetCache(cacheKey, objObject, DateTime.Now.AddMinutes(180.0), TimeSpan.Zero);
        }
        catch
        {
        }
      }
      return objObject != null ? (ConnectionStringSettings) objObject : (ConnectionStringSettings) null;
    }

    public static ConnectionStringSettings GetConnectionStrings(int index)
    {
      string cacheKey = "ConnectionStrings-" + (object) index;
      object objObject = CacheHelper.GetCache(cacheKey);
      if (objObject == null)
      {
        try
        {
          objObject = (object) ConfigurationManager.ConnectionStrings[index];
          CacheHelper.SetCache(cacheKey, objObject, DateTime.Now.AddMinutes(180.0), TimeSpan.Zero);
        }
        catch
        {
        }
      }
      return objObject != null ? (ConnectionStringSettings) objObject : (ConnectionStringSettings) null;
    }

    public static string GetDbString(string key)
    {
      ConnectionStringSettings connectionStrings = ConfigHelper.GetConnectionStrings(key);
      return connectionStrings != null ? connectionStrings.ConnectionString : string.Empty;
    }

    public static string GetDbString(int key)
    {
      ConnectionStringSettings connectionStrings = ConfigHelper.GetConnectionStrings(key);
      return connectionStrings != null ? connectionStrings.ConnectionString : string.Empty;
    }

    public static string GetDbProviderName(string key)
    {
      ConnectionStringSettings connectionStrings = ConfigHelper.GetConnectionStrings(key);
      return connectionStrings != null ? connectionStrings.ProviderName : string.Empty;
    }

    public static string GetDbProviderName(int index)
    {
      ConnectionStringSettings connectionStrings = ConfigHelper.GetConnectionStrings(index);
      return connectionStrings != null ? connectionStrings.ProviderName : string.Empty;
    }

    public static string GetDbName(string key)
    {
      ConnectionStringSettings connectionStrings = ConfigHelper.GetConnectionStrings(key);
      return connectionStrings != null ? connectionStrings.Name : string.Empty;
    }

    public static string GetDbName(int index)
    {
      ConnectionStringSettings connectionStrings = ConfigHelper.GetConnectionStrings(index);
      return connectionStrings != null ? connectionStrings.Name : string.Empty;
    }
  }
}
