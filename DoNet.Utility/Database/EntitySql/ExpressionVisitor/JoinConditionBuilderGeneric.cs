// Decompiled with JetBrains decompiler
// Type: DoNet.Utility.Database.EntitySql.ExpressionVisitor.JoinConditionBuilderGeneric
// Assembly: DoNet.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD5DA16A-6F4C-4458-AC23-48055BBFFF84
// Assembly location: D:\Dian.Web\bin\DoNet.Utility.dll

using DoNet.Utility.Database.EntitySql.Entity;
using System;
using System.Linq.Expressions;

namespace DoNet.Utility.Database.EntitySql.ExpressionVisitor
{
  internal static class JoinConditionBuilderGeneric
  {
    public static string GetJoinCondition<TA, TB>(GenericJoinEntity<TA, TB> theJoinEntity) => JoinConditionBuilderGeneric.GetSubConditions<TA, TB>(theJoinEntity.MainEntity, theJoinEntity.EntityToJoin, theJoinEntity.JoinConditionExpression, theJoinEntity.JoinConditionFirstParameter);

    private static string GetSubConditions<TA, TB>(
      GenericWhereEntity<TA> mainEntity,
      GenericWhereEntity<TB> joinEntity,
      Expression joinExpression,
      string firstParameter)
    {
      if (joinExpression is MemberExpression)
      {
        MemberExpression memberExpression = (MemberExpression) joinExpression;
        if (memberExpression.Expression.ToString() == firstParameter)
        {
          string dbColumnName = EntityMappingTool.GetDbColumnName(mainEntity.EntityType, memberExpression.Member.Name);
          return string.Format("{0}.[{1}]", (object) mainEntity.TableName, (object) dbColumnName);
        }
        string dbColumnName1 = EntityMappingTool.GetDbColumnName(joinEntity.EntityType, memberExpression.Member.Name);
        return string.Format("{0}.[{1}]", (object) joinEntity.TableName, (object) dbColumnName1);
      }
      BinaryExpression binaryExpression = (BinaryExpression) joinExpression;
      string str;
      switch (binaryExpression.NodeType)
      {
        case ExpressionType.Equal:
          str = "=";
          break;
        case ExpressionType.GreaterThan:
          str = ">";
          break;
        case ExpressionType.GreaterThanOrEqual:
          str = ">=";
          break;
        case ExpressionType.LessThan:
          str = "<";
          break;
        case ExpressionType.LessThanOrEqual:
          str = "<=";
          break;
        case ExpressionType.NotEqual:
          str = "<>";
          break;
        default:
          throw new NotSupportedException("不支持连接条件类型：" + (object) joinExpression.NodeType);
      }
      string subConditions1 = JoinConditionBuilderGeneric.GetSubConditions<TA, TB>(mainEntity, joinEntity, binaryExpression.Left, firstParameter);
      string subConditions2 = JoinConditionBuilderGeneric.GetSubConditions<TA, TB>(mainEntity, joinEntity, binaryExpression.Right, firstParameter);
      return string.Format("({0} {1} {2})", (object) subConditions1, (object) str, (object) subConditions2);
    }
  }
}
