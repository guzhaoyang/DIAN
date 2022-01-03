// Decompiled with JetBrains decompiler
// Type: DoNet.Utility.Database.EntitySql.Entity.GenericJoinEntity`2
// Assembly: DoNet.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD5DA16A-6F4C-4458-AC23-48055BBFFF84
// Assembly location: D:\Dian.Web\bin\DoNet.Utility.dll

using System;
using System.Linq.Expressions;

namespace DoNet.Utility.Database.EntitySql.Entity
{
  public class GenericJoinEntity<TA, TB>
  {
    public string LeftTableGuid { get; private set; }

    public string RightTableGuid { get; private set; }

    public GenericWhereEntity<TA> MainEntity { get; set; }

    public GenericWhereEntity<TB> EntityToJoin { get; set; }

    public JoinModeEnum JoinMode { get; set; }

    public string JoinCondition { get; set; }

    public string JoinConditionFirstParameter { get; private set; }

    public Expression JoinConditionExpression { get; private set; }

    public void InnerJoin(
      GenericWhereEntity<TA> TA,
      GenericWhereEntity<TB> TB,
      Expression<Func<TA, TB, bool>> conditionExpression)
    {
      this.Join(TA, TB, conditionExpression, JoinModeEnum.InnerJoin);
    }

    public void LeftJoin(
      GenericWhereEntity<TA> TA,
      GenericWhereEntity<TB> TB,
      Expression<Func<TA, TB, bool>> conditionExpression)
    {
      this.Join(TA, TB, conditionExpression, JoinModeEnum.LeftJoin);
    }

    private void Join(
      GenericWhereEntity<TA> TA,
      GenericWhereEntity<TB> TB,
      Expression<Func<TA, TB, bool>> conditionExpression,
      JoinModeEnum joinMode)
    {
      if (conditionExpression.Body == null)
        throw new EntitySqlException("未指定连接条件！");
      if (!(conditionExpression.Body is BinaryExpression) || !GenericJoinEntity<TA, TB>.CheckJoinCondition(conditionExpression.Body))
        throw new EntitySqlException("指定的连接条件无效！");
      this.JoinMode = joinMode;
      this.MainEntity = TA;
      this.EntityToJoin = TB;
      this.LeftTableGuid = TA.Guid;
      this.RightTableGuid = TB.Guid;
      this.JoinConditionExpression = conditionExpression.Body;
      this.JoinConditionFirstParameter = conditionExpression.Parameters[0].Name;
    }

    private static bool CheckJoinCondition(Expression joinExpression)
    {
      switch (joinExpression)
      {
        case BinaryExpression _:
          BinaryExpression binaryExpression = (BinaryExpression) joinExpression;
          return GenericJoinEntity<TA, TB>.CheckJoinCondition(binaryExpression.Left) && GenericJoinEntity<TA, TB>.CheckJoinCondition(binaryExpression.Right);
        case MemberExpression _:
          return true;
        default:
          return false;
      }
    }
  }
}
