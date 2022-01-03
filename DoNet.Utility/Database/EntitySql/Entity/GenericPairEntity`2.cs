// Decompiled with JetBrains decompiler
// Type: DoNet.Utility.Database.EntitySql.Entity.GenericPairEntity`2
// Assembly: DoNet.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD5DA16A-6F4C-4458-AC23-48055BBFFF84
// Assembly location: D:\Dian.Web\bin\DoNet.Utility.dll

using System;

namespace DoNet.Utility.Database.EntitySql.Entity
{
  [Serializable]
  public class GenericPairEntity<TA, TB>
    where TA : class, new()
    where TB : class, new()
  {
    public TA EntityA { get; set; }

    public TB EntityB { get; set; }
  }
}
