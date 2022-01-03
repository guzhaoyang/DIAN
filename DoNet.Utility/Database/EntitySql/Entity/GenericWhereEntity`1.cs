// Decompiled with JetBrains decompiler
// Type: DoNet.Utility.Database.EntitySql.Entity.GenericWhereEntity`1
// Assembly: DoNet.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD5DA16A-6F4C-4458-AC23-48055BBFFF84
// Assembly location: D:\Dian.Web\bin\DoNet.Utility.dll

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace DoNet.Utility.Database.EntitySql.Entity
{
  public class GenericWhereEntity<T>
  {
    public GenericWhereEntity()
    {
      this.Guid = new System.Guid().ToString();
      this.DbProviderName = "System.Data.SqlClient";
      this.EntityType = typeof (T);
      this.WhereExpressions = new List<Expression>();
      this.WhereParameterNames = new List<string>(8);
      this.WhereParameterValues = new List<object>(8);
      this.WhereParameterTypes = new List<DbType>(8);
      this.TableName = nameof (T) + this.TableNameIndex.ToString().PadLeft(2, '0');
      this.TableNameIndex = 0;
    }

    public int TableNameIndex { get; private set; }

    public string TableName { get; private set; } = string.Empty;

    public string DbProviderName { get; set; }

    public string Guid { get; }

    public List<Expression> WhereExpressions { get; }

    public Type EntityType { get; }

    public string WhereCondition { get; set; }

    public bool DisableTableAlias { get; set; }

    public List<string> WhereParameterNames { get; }

    public List<object> WhereParameterValues { get; }

    public List<DbType> WhereParameterTypes { get; }

    public void Where(Expression<Func<T, bool>> conditionExpression)
    {
      if (conditionExpression == null || conditionExpression.Body == null)
        return;
      this.WhereExpressions.Add(conditionExpression.Body);
    }

    public void Where(Expression conditionExpression)
    {
      if (conditionExpression == null)
        return;
      this.WhereExpressions.Add(conditionExpression);
    }

    public void ResetTableName(int tableNameIndex)
    {
      this.TableNameIndex = tableNameIndex;
      this.TableName = nameof (T) + this.TableNameIndex.ToString().PadLeft(2, '0');
    }
  }
}
