// Decompiled with JetBrains decompiler
// Type: DoNet.Utility.Database.EntitySql.ExpressionVisitor.ConditionBuilderGeneric`1
// Assembly: DoNet.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD5DA16A-6F4C-4458-AC23-48055BBFFF84
// Assembly location: D:\Dian.Web\bin\DoNet.Utility.dll

using DoNet.Utility.Database.EntitySql.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace DoNet.Utility.Database.EntitySql.ExpressionVisitor
{
  internal class ConditionBuilderGeneric<T> : DoNet.Utility.Database.EntitySql.ExpressionVisitor.ExpressionVisitor
  {
    private readonly string _TableAlias;
    private readonly GenericWhereEntity<T> _TheWhereEntity;
    private List<object> m_arguments;
    private Stack<string> m_conditionParts;
    private List<DbType> m_DbTypes;
    private List<string> m_ParameterNames;
    private string m_TmpDBColumnName;
    private DbType m_TmpDBColumnType = DbType.AnsiString;
    private bool m_TmpUsedParameter;

    public ConditionBuilderGeneric(string tableAlias, GenericWhereEntity<T> theEntity)
    {
      this._TableAlias = tableAlias;
      this._TheWhereEntity = theEntity;
    }

    public string Condition { get; private set; }

    public object[] Arguments { get; private set; }

    public DbType[] DbTypes { get; private set; }

    public string[] ParameterNames { get; private set; }

    public void Build(Expression expression)
    {
      Expression exp = new PartialEvaluator().Eval(expression);
      this.m_arguments = new List<object>();
      this.m_conditionParts = new Stack<string>();
      this.m_DbTypes = new List<DbType>();
      this.m_ParameterNames = new List<string>();
      this.Visit(exp);
      this.Arguments = this.m_arguments.ToArray();
      this.DbTypes = this.m_DbTypes.ToArray();
      this.ParameterNames = this.m_ParameterNames.ToArray();
      this.Condition = this.m_conditionParts.Count > 0 ? this.m_conditionParts.Pop() : (string) null;
    }

    protected override Expression VisitBinary(BinaryExpression b)
    {
      if (b == null)
        return (Expression) b;
      bool flag1 = b.Left is ConstantExpression;
      bool flag2 = b.Right is ConstantExpression;
      if (b.Left is ConstantExpression && b.Right is ConstantExpression)
        throw new NotSupportedException("不支持两端都是静态变量的表达式！" + (object) b);
      bool flag3 = false;
      if (flag2 && ((ConstantExpression) b.Right).Value == null)
        flag3 = true;
      string str1;
      switch (b.NodeType)
      {
        case ExpressionType.Add:
          str1 = "+";
          break;
        case ExpressionType.AndAlso:
          if (flag3)
            throw new NotSupportedException("对null值不支持与的操作");
          str1 = "AND";
          break;
        case ExpressionType.Divide:
          str1 = "/";
          break;
        case ExpressionType.Equal:
          str1 = !flag3 ? "=" : "is";
          break;
        case ExpressionType.GreaterThan:
          if (flag3)
            throw new NotSupportedException("对null值不支持大于的操作");
          str1 = ">";
          break;
        case ExpressionType.GreaterThanOrEqual:
          if (flag3)
            throw new NotSupportedException("对null值不支持大于等于的操作");
          str1 = ">=";
          break;
        case ExpressionType.LessThan:
          if (flag3)
            throw new NotSupportedException("对null值不支持小于的操作");
          str1 = "<";
          break;
        case ExpressionType.LessThanOrEqual:
          if (flag3)
            throw new NotSupportedException("对null值不支持小于等于的操作");
          str1 = "<=";
          break;
        case ExpressionType.Multiply:
          str1 = "*";
          break;
        case ExpressionType.NotEqual:
          str1 = !flag3 ? "<>" : "is not";
          break;
        case ExpressionType.OrElse:
          if (flag3)
            throw new NotSupportedException("对null值不支持或的操作");
          str1 = "OR";
          break;
        case ExpressionType.Subtract:
          str1 = "-";
          break;
        default:
          throw new NotSupportedException("不支持操作类型：" + (object) b.NodeType);
      }
      string tmpDbColumnName = this.m_TmpDBColumnName;
      DbType tmpDbColumnType = this.m_TmpDBColumnType;
      bool tmpUsedParameter = this.m_TmpUsedParameter;
      MemberExpression memberExpression1 = (MemberExpression) null;
      MemberExpression memberExpression2 = (MemberExpression) null;
      if (b.Left is MemberExpression)
        memberExpression1 = b.Left as MemberExpression;
      else if (b.Left.NodeType == ExpressionType.Convert)
        memberExpression1 = ((UnaryExpression) b.Left).Operand as MemberExpression;
      bool flag4 = memberExpression1 != null;
      if (b.Right is MemberExpression)
        memberExpression2 = b.Right as MemberExpression;
      else if (b.Right.NodeType == ExpressionType.Convert)
        memberExpression2 = ((UnaryExpression) b.Right).Operand as MemberExpression;
      bool flag5 = memberExpression2 != null;
      this.m_TmpUsedParameter = flag4 & flag2 || flag5 & flag1;
      if (flag4)
      {
        this.m_TmpDBColumnName = EntityMappingTool.GetDbColumnName(this._TheWhereEntity.EntityType, memberExpression1.Member.Name);
        this.m_TmpDBColumnType = EntityMappingTool.GetDbColumnType(this._TheWhereEntity.EntityType, memberExpression1.Member.Name);
        if (flag3)
        {
          this.Visit(b.Left);
          this.m_conditionParts.Push(string.Format("({0} {1} {2})", (object) this.m_conditionParts.Pop(), (object) str1, (object) "null"));
          this.m_TmpUsedParameter = tmpUsedParameter;
          this.m_TmpDBColumnType = tmpDbColumnType;
          this.m_TmpDBColumnName = tmpDbColumnName;
          return (Expression) b;
        }
      }
      else if (flag1 & flag5)
      {
        this.m_TmpDBColumnName = EntityMappingTool.GetDbColumnName(this._TheWhereEntity.EntityType, memberExpression2.Member.Name);
        this.m_TmpDBColumnType = EntityMappingTool.GetDbColumnType(this._TheWhereEntity.EntityType, memberExpression2.Member.Name);
      }
      this.Visit(b.Left);
      if (flag4 & flag5)
      {
        this.m_TmpDBColumnName = EntityMappingTool.GetDbColumnName(this._TheWhereEntity.EntityType, memberExpression2.Member.Name);
        this.m_TmpDBColumnType = EntityMappingTool.GetDbColumnType(this._TheWhereEntity.EntityType, memberExpression2.Member.Name);
      }
      this.Visit(b.Right);
      this.m_TmpUsedParameter = tmpUsedParameter;
      this.m_TmpDBColumnType = tmpDbColumnType;
      this.m_TmpDBColumnName = tmpDbColumnName;
      string str2 = this.m_conditionParts.Pop();
      this.m_conditionParts.Push(string.Format("({0} {1} {2})", (object) this.m_conditionParts.Pop(), (object) str1, (object) str2));
      return (Expression) b;
    }

    protected override Expression VisitMethodCall(MethodCallExpression m)
    {
      if (m.Object == null || !(m.Object is MemberExpression) && !(m.Object is ConstantExpression))
        return (Expression) Expression.Call(m.Object, m.Method, (IEnumerable<Expression>) m.Arguments);
      MethodCallVisitor.Visit(this._TheWhereEntity.EntityType, m, this._TableAlias, this.m_conditionParts, this.m_ParameterNames, this.m_DbTypes, this.m_arguments);
      return (Expression) m;
    }

    protected override Expression VisitConstant(ConstantExpression c)
    {
      if (c == null)
        return (Expression) c;
      this.m_arguments.Add(c.Value);
      string str1 = string.Format("@{0}_{1}", (object) this._TableAlias, (object) this.m_TmpDBColumnName);
      int num = 0;
      string str2;
      for (str2 = ""; this.m_ParameterNames.Contains(str1 + str2); str2 = num.ToString())
        ++num;
      string str3 = str1 + str2;
      this.m_ParameterNames.Add(str3);
      this.m_DbTypes.Add(this.m_TmpDBColumnType);
      this.m_conditionParts.Push(str3);
      return (Expression) c;
    }

    protected override Expression VisitMemberAccess(MemberExpression m)
    {
      if (m == null)
        return (Expression) m;
      this.m_conditionParts.Push(string.Format("{0}.[{1}]", (object) this._TableAlias, (object) this.m_TmpDBColumnName));
      return (Expression) m;
    }

    protected override Expression VisitConvert(UnaryExpression v)
    {
      if (v == null)
        return (Expression) v;
      MemberExpression operand = (MemberExpression) v.Operand;
      this.m_conditionParts.Push(string.Format("{0}.[{1}]", (object) this._TableAlias, (object) this.m_TmpDBColumnName));
      return (Expression) v;
    }
  }
}
