// Decompiled with JetBrains decompiler
// Type: DoNet.Utility.Database.EntitySql.ExpressionVisitor.MethodCall.StringMethodCallVisitor
// Assembly: DoNet.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD5DA16A-6F4C-4458-AC23-48055BBFFF84
// Assembly location: D:\Dian.Web\bin\DoNet.Utility.dll

using DoNet.Utility.Database.EntitySql.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace DoNet.Utility.Database.EntitySql.ExpressionVisitor.MethodCall
{
  internal static class StringMethodCallVisitor
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
      string name = m.Method.Name;
      if (m.Object is MemberExpression)
      {
        string dbColumnName = EntityMappingTool.GetDbColumnName(theEntityType, ((MemberExpression) m.Object).Member.Name);
        DbType dbColumnType = EntityMappingTool.GetDbColumnType(theEntityType, ((MemberExpression) m.Object).Member.Name);
        string parameterName = StringMethodCallVisitor.GetParameterName(colParameterNames, dbColumnName, tableAlias);
        string str1 = name;
        if (!(str1 == "Contains"))
        {
          if (!(str1 == "StartsWith"))
          {
            if (!(str1 == "EndsWith"))
              throw new EntitySqlException("暂不支持{" + (object) m + "}的调用！");
            string str2 = string.Format("({0}.[{1}] like {2})", (object) tableAlias, (object) dbColumnName, (object) parameterName);
            colConditionParts.Push(str2);
            colParameterNames.Add(parameterName);
            colDbTypes.Add(dbColumnType);
            colArguments.Add((object) ("%" + StringMethodCallVisitor.GetArgumentValue(m.Arguments[0] as ConstantExpression)));
          }
          else
          {
            string str3 = string.Format("({0}.[{1}] like {2})", (object) tableAlias, (object) dbColumnName, (object) parameterName);
            colConditionParts.Push(str3);
            colParameterNames.Add(parameterName);
            colDbTypes.Add(dbColumnType);
            colArguments.Add((object) (StringMethodCallVisitor.GetArgumentValue(m.Arguments[0] as ConstantExpression) + "%"));
          }
        }
        else
        {
          string str4 = string.Format("({0}.[{1}] like {2})", (object) tableAlias, (object) dbColumnName, (object) parameterName);
          colConditionParts.Push(str4);
          colParameterNames.Add(parameterName);
          colDbTypes.Add(dbColumnType);
          colArguments.Add((object) ("%" + StringMethodCallVisitor.GetArgumentValue(m.Arguments[0] as ConstantExpression) + "%"));
        }
      }
      else
      {
        if (!(m.Object is ConstantExpression))
          throw new EntitySqlException("暂不支持{" + (object) m + "}的调用！");
        string dbColumnName = EntityMappingTool.GetDbColumnName(theEntityType, ((MemberExpression) m.Arguments[0]).Member.Name);
        DbType dbColumnType = EntityMappingTool.GetDbColumnType(theEntityType, ((MemberExpression) m.Arguments[0]).Member.Name);
        string parameterName = StringMethodCallVisitor.GetParameterName(colParameterNames, dbColumnName, tableAlias);
        string str5 = name;
        if (!(str5 == "Contains"))
        {
          if (!(str5 == "StartsWith"))
            throw new EntitySqlException("暂不支持{" + (object) m + "}的调用！");
          string str6 = string.Format("(CHARINDEX({0},{1}.[{2}])=1)", (object) parameterName, (object) tableAlias, (object) dbColumnName);
          colConditionParts.Push(str6);
          colParameterNames.Add(parameterName);
          colDbTypes.Add(dbColumnType);
          colArguments.Add((object) StringMethodCallVisitor.GetArgumentValue(m.Object as ConstantExpression));
        }
        else
        {
          string str7 = string.Format("(CHARINDEX({0},{1}.[{2}])>0)", (object) parameterName, (object) tableAlias, (object) dbColumnName);
          colConditionParts.Push(str7);
          colParameterNames.Add(parameterName);
          colDbTypes.Add(dbColumnType);
          colArguments.Add((object) StringMethodCallVisitor.GetArgumentValue(m.Object as ConstantExpression));
        }
      }
    }

    private static string GetParameterName(
      List<string> colParameterNames,
      string memberName,
      string tableAlias)
    {
      string parameterName = "@" + tableAlias + "_" + memberName;
      if (!colParameterNames.Contains(parameterName))
        return parameterName;
      int num = 1;
      while (colParameterNames.Contains(parameterName + (object) num))
        ++num;
      return parameterName + (object) num;
    }

    private static string GetArgumentValue(ConstantExpression c) => c.Value.ToString();
  }
}
