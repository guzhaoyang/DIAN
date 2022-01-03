// Decompiled with JetBrains decompiler
// Type: DoNet.Utility.Database.EntitySql.Attribute.Table
// Assembly: DoNet.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD5DA16A-6F4C-4458-AC23-48055BBFFF84
// Assembly location: D:\Dian.Web\bin\DoNet.Utility.dll

using System;

namespace DoNet.Utility.Database.EntitySql.Attribute
{
  [AttributeUsage(AttributeTargets.Class)]
  public sealed class Table : System.Attribute
  {
    public Table(string tableName) => this.TableName = tableName;

    public string TableName { get; set; }
  }
}
