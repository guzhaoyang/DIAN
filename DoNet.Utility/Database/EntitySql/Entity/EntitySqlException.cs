// Decompiled with JetBrains decompiler
// Type: DoNet.Utility.Database.EntitySql.Entity.EntitySqlException
// Assembly: DoNet.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD5DA16A-6F4C-4458-AC23-48055BBFFF84
// Assembly location: D:\Dian.Web\bin\DoNet.Utility.dll

using System;

namespace DoNet.Utility.Database.EntitySql.Entity
{
  [Serializable]
  public class EntitySqlException : Exception
  {
    public EntitySqlException()
    {
    }

    public EntitySqlException(string msg)
      : base(msg)
    {
    }
  }
}
