// Decompiled with JetBrains decompiler
// Type: DoNet.Utility.Database.EntitySql.ExpressionVisitor.MethodCallVisitor
// Assembly: DoNet.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD5DA16A-6F4C-4458-AC23-48055BBFFF84
// Assembly location: D:\Dian.Web\bin\DoNet.Utility.dll

using DoNet.Utility.Database.EntitySql.Entity;
using DoNet.Utility.Database.EntitySql.ExpressionVisitor.MethodCall;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace DoNet.Utility.Database.EntitySql.ExpressionVisitor
{
  internal static class MethodCallVisitor
  {
    public static void Visit(
      Type theEntityType,
      MethodCallExpression m,
      string tableAlias,
      Stack<string> colConditionParts,
      List<string> colParameterNames,
      List<DbType> colDbTypes,
      List<object> colArguments)
    {
      if (m.Object is MemberExpression)
      {
        if (m.Object.Type != typeof (string))
          throw new EntitySqlException("暂不支持{" + (object) m + "}的调用!");
        StringMethodCallVisitor.Visit(theEntityType, m, tableAlias, colConditionParts, colParameterNames, colDbTypes, colArguments);
      }
      else
      {
        if (!(m.Object is ConstantExpression))
          throw new EntitySqlException("暂不支持{" + (object) m + "}的调用!");
        if ((m.Object as ConstantExpression).Type != typeof (string))
          throw new EntitySqlException("暂不支持{" + (object) m + "}的调用!");
        StringMethodCallVisitor.Visit(theEntityType, m, tableAlias, colConditionParts, colParameterNames, colDbTypes, colArguments);
      }
    }
  }
}
