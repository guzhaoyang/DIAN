// Decompiled with JetBrains decompiler
// Type: DoNet.Utility.Database.EntitySql.Attribute.Field
// Assembly: DoNet.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD5DA16A-6F4C-4458-AC23-48055BBFFF84
// Assembly location: D:\Dian.Web\bin\DoNet.Utility.dll

using System;

namespace DoNet.Utility.Database.EntitySql.Attribute
{
  [AttributeUsage(AttributeTargets.Property)]
  public sealed class Field : System.Attribute
  {
    public Field(string fieldName) => this.FieldName = fieldName;

    public string FieldName { get; set; }
  }
}
