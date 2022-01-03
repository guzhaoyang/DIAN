// Decompiled with JetBrains decompiler
// Type: DoNet.Utility.Database.DbFactory
// Assembly: DoNet.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD5DA16A-6F4C-4458-AC23-48055BBFFF84
// Assembly location: D:\Dian.Web\bin\DoNet.Utility.dll

namespace DoNet.Utility.Database
{
  public class DbFactory
  {
    public static DbHelper CreateDatabase() => new DbHelper();

    public static DbHelper CreateDatabase(string dbName) => new DbHelper(dbName);
  }
}
