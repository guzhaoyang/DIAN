// Decompiled with JetBrains decompiler
// Type: DoNet.Utility.Database.EntitySql.ExpressionVisitor.PartialEvaluator
// Assembly: DoNet.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD5DA16A-6F4C-4458-AC23-48055BBFFF84
// Assembly location: D:\Dian.Web\bin\DoNet.Utility.dll

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DoNet.Utility.Database.EntitySql.ExpressionVisitor
{
  internal class PartialEvaluator : DoNet.Utility.Database.EntitySql.ExpressionVisitor.ExpressionVisitor
  {
    private readonly Func<Expression, bool> m_fnCanBeEvaluated;
    private HashSet<Expression> m_candidates;

    public PartialEvaluator()
      : this(new Func<Expression, bool>(PartialEvaluator.CanBeEvaluatedLocally))
    {
    }

    public PartialEvaluator(Func<Expression, bool> fnCanBeEvaluated) => this.m_fnCanBeEvaluated = fnCanBeEvaluated;

    public Expression Eval(Expression exp)
    {
      this.m_candidates = new PartialEvaluator.Nominator(this.m_fnCanBeEvaluated).Nominate(exp);
      return this.Visit(exp);
    }

    protected override Expression Visit(Expression exp)
    {
      if (exp == null)
        return (Expression) null;
      return this.m_candidates.Contains(exp) ? this.Evaluate(exp) : base.Visit(exp);
    }

    private Expression Evaluate(Expression e) => e.NodeType == ExpressionType.Constant ? e : (Expression) Expression.Constant(Expression.Lambda(e).Compile().DynamicInvoke((object[]) null), e.Type);

    private static bool CanBeEvaluatedLocally(Expression exp) => exp.NodeType != ExpressionType.Parameter;

    private class Nominator : DoNet.Utility.Database.EntitySql.ExpressionVisitor.ExpressionVisitor
    {
      private readonly Func<Expression, bool> m_fnCanBeEvaluated;
      private HashSet<Expression> m_candidates;
      private bool m_cannotBeEvaluated;

      internal Nominator(Func<Expression, bool> fnCanBeEvaluated) => this.m_fnCanBeEvaluated = fnCanBeEvaluated;

      internal HashSet<Expression> Nominate(Expression expression)
      {
        this.m_candidates = new HashSet<Expression>();
        this.Visit(expression);
        return this.m_candidates;
      }

      protected override Expression Visit(Expression expression)
      {
        if (expression != null)
        {
          bool cannotBeEvaluated = this.m_cannotBeEvaluated;
          this.m_cannotBeEvaluated = false;
          base.Visit(expression);
          if (!this.m_cannotBeEvaluated)
          {
            if (this.m_fnCanBeEvaluated(expression))
              this.m_candidates.Add(expression);
            else
              this.m_cannotBeEvaluated = true;
          }
          this.m_cannotBeEvaluated |= cannotBeEvaluated;
        }
        return expression;
      }
    }
  }
}
