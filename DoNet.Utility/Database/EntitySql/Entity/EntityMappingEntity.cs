// Decompiled with JetBrains decompiler
// Type: DoNet.Utility.Database.EntitySql.Entity.EntityMappingEntity
// Assembly: DoNet.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD5DA16A-6F4C-4458-AC23-48055BBFFF84
// Assembly location: D:\Dian.Web\bin\DoNet.Utility.dll

using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace DoNet.Utility.Database.EntitySql.Entity
{
  internal class EntityMappingEntity
  {
    public string EntityTypeName { get; set; }

    public string DbTableName { get; set; }

    public List<string> EntityFieldNames { get; set; }

    public Dictionary<string, PropertyInfo> EntityPropertyInfoMapping { get; set; }

    public Dictionary<string, string> DbColumnNameMapping { get; set; }

    public Dictionary<string, DbType> DbColumnTypeMapping { get; set; }

    public Dictionary<string, string> DbIdentityMapping { get; set; }

    public Dictionary<string, string> DbPrimaryKeyMapping { get; set; }
  }
}
