// Decompiled with JetBrains decompiler
// Type: DoNet.Utility.Database.EntitySql.ExpressionVisitor.ExpressionVisitor
// Assembly: DoNet.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD5DA16A-6F4C-4458-AC23-48055BBFFF84
// Assembly location: D:\Dian.Web\bin\DoNet.Utility.dll

using DoNet.Utility.Database.EntitySql.Entity;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

namespace DoNet.Utility.Database.EntitySql.ExpressionVisitor
{
  internal abstract class ExpressionVisitor
  {
    protected virtual Expression Visit(Expression exp)
    {
      if (exp == null)
        return exp;
      switch (exp.NodeType)
      {
        case ExpressionType.Add:
        case ExpressionType.AddChecked:
        case ExpressionType.And:
        case ExpressionType.AndAlso:
        case ExpressionType.ArrayIndex:
        case ExpressionType.Coalesce:
        case ExpressionType.Divide:
        case ExpressionType.Equal:
        case ExpressionType.ExclusiveOr:
        case ExpressionType.GreaterThan:
        case ExpressionType.GreaterThanOrEqual:
        case ExpressionType.LeftShift:
        case ExpressionType.LessThan:
        case ExpressionType.LessThanOrEqual:
        case ExpressionType.Modulo:
        case ExpressionType.Multiply:
        case ExpressionType.MultiplyChecked:
        case ExpressionType.NotEqual:
        case ExpressionType.Or:
        case ExpressionType.OrElse:
        case ExpressionType.RightShift:
        case ExpressionType.Subtract:
        case ExpressionType.SubtractChecked:
          return this.VisitBinary((BinaryExpression) exp);
        case ExpressionType.ArrayLength:
        case ExpressionType.ConvertChecked:
        case ExpressionType.Quote:
        case ExpressionType.TypeAs:
          return this.VisitUnary((UnaryExpression) exp);
        case ExpressionType.Call:
          return this.VisitMethodCall((MethodCallExpression) exp);
        case ExpressionType.Conditional:
          return this.VisitConditional((ConditionalExpression) exp);
        case ExpressionType.Constant:
          return this.VisitConstant((ConstantExpression) exp);
        case ExpressionType.Convert:
        case ExpressionType.Negate:
        case ExpressionType.NegateChecked:
        case ExpressionType.Not:
          return this.VisitConvert((UnaryExpression) exp);
        case ExpressionType.Invoke:
          return this.VisitInvocation((InvocationExpression) exp);
        case ExpressionType.Lambda:
          return this.VisitLambda((LambdaExpression) exp);
        case ExpressionType.ListInit:
          return this.VisitListInit((ListInitExpression) exp);
        case ExpressionType.MemberAccess:
          return this.VisitMemberAccess((MemberExpression) exp);
        case ExpressionType.MemberInit:
          return this.VisitMemberInit((MemberInitExpression) exp);
        case ExpressionType.New:
          return (Expression) this.VisitNew((NewExpression) exp);
        case ExpressionType.NewArrayInit:
        case ExpressionType.NewArrayBounds:
          return this.VisitNewArray((NewArrayExpression) exp);
        case ExpressionType.Parameter:
          return this.VisitParameter((ParameterExpression) exp);
        case ExpressionType.TypeIs:
          return this.VisitTypeIs((TypeBinaryExpression) exp);
        default:
          throw new EntitySqlException(string.Format("Unhandled expression type: '{0}'", (object) exp.NodeType));
      }
    }

    protected virtual MemberBinding VisitBinding(MemberBinding binding)
    {
      switch (binding.BindingType)
      {
        case MemberBindingType.Assignment:
          return (MemberBinding) this.VisitMemberAssignment((MemberAssignment) binding);
        case MemberBindingType.MemberBinding:
          return (MemberBinding) this.VisitMemberMemberBinding((MemberMemberBinding) binding);
        case MemberBindingType.ListBinding:
          return (MemberBinding) this.VisitMemberListBinding((MemberListBinding) binding);
        default:
          throw new EntitySqlException(string.Format("Unhandled binding type '{0}'", (object) binding.BindingType));
      }
    }

    protected virtual ElementInit VisitElementInitializer(ElementInit initializer)
    {
      ReadOnlyCollection<Expression> arguments = this.VisitExpressionList(initializer.Arguments);
      return arguments != initializer.Arguments ? Expression.ElementInit(initializer.AddMethod, (IEnumerable<Expression>) arguments) : initializer;
    }

    protected virtual Expression VisitUnary(UnaryExpression u)
    {
      Expression operand = this.Visit(u.Operand);
      return operand != u.Operand ? (Expression) Expression.MakeUnary(u.NodeType, operand, u.Type, u.Method) : (Expression) u;
    }

    protected virtual Expression VisitBinary(BinaryExpression b)
    {
      Expression left = this.Visit(b.Left);
      Expression right = this.Visit(b.Right);
      Expression conversion = this.Visit((Expression) b.Conversion);
      if (left == b.Left && right == b.Right && conversion == b.Conversion)
        return (Expression) b;
      return b.NodeType == ExpressionType.Coalesce && b.Conversion != null ? (Expression) Expression.Coalesce(left, right, conversion as LambdaExpression) : (Expression) Expression.MakeBinary(b.NodeType, left, right, b.IsLiftedToNull, b.Method);
    }

    protected virtual Expression VisitTypeIs(TypeBinaryExpression b)
    {
      Expression expression = this.Visit(b.Expression);
      return expression != b.Expression ? (Expression) Expression.TypeIs(expression, b.TypeOperand) : (Expression) b;
    }

    protected virtual Expression VisitConstant(ConstantExpression c) => (Expression) c;

    protected virtual Expression VisitConditional(ConditionalExpression c)
    {
      Expression test = this.Visit(c.Test);
      Expression ifTrue = this.Visit(c.IfTrue);
      Expression ifFalse = this.Visit(c.IfFalse);
      return test != c.Test || ifTrue != c.IfTrue || ifFalse != c.IfFalse ? (Expression) Expression.Condition(test, ifTrue, ifFalse) : (Expression) c;
    }

    protected virtual Expression VisitParameter(ParameterExpression p) => (Expression) p;

    protected virtual Expression VisitMemberAccess(MemberExpression m)
    {
      Expression expression = this.Visit(m.Expression);
      return expression != m.Expression ? (Expression) Expression.MakeMemberAccess(expression, m.Member) : (Expression) m;
    }

    protected virtual Expression VisitConvert(UnaryExpression v) => (Expression) v;

    protected virtual Expression VisitMethodCall(MethodCallExpression m)
    {
      Expression instance = this.Visit(m.Object);
      IEnumerable<Expression> arguments = (IEnumerable<Expression>) this.VisitExpressionList(m.Arguments);
      return instance != m.Object || arguments != m.Arguments ? (Expression) Expression.Call(instance, m.Method, arguments) : (Expression) m;
    }

    protected virtual ReadOnlyCollection<Expression> VisitExpressionList(
      ReadOnlyCollection<Expression> original)
    {
      List<Expression> expressionList = (List<Expression>) null;
      int index1 = 0;
      for (int count = original.Count; index1 < count; ++index1)
      {
        Expression expression = this.Visit(original[index1]);
        if (expressionList != null)
          expressionList.Add(expression);
        else if (expression != original[index1])
        {
          expressionList = new List<Expression>(count);
          for (int index2 = 0; index2 < index1; ++index2)
            expressionList.Add(original[index2]);
          expressionList.Add(expression);
        }
      }
      return expressionList != null ? expressionList.AsReadOnly() : original;
    }

    protected virtual MemberAssignment VisitMemberAssignment(
      MemberAssignment assignment)
    {
      Expression expression = this.Visit(assignment.Expression);
      return expression != assignment.Expression ? Expression.Bind(assignment.Member, expression) : assignment;
    }

    protected virtual MemberMemberBinding VisitMemberMemberBinding(
      MemberMemberBinding binding)
    {
      IEnumerable<MemberBinding> bindings = this.VisitBindingList(binding.Bindings);
      return bindings != binding.Bindings ? Expression.MemberBind(binding.Member, bindings) : binding;
    }

    protected virtual MemberListBinding VisitMemberListBinding(
      MemberListBinding binding)
    {
      IEnumerable<ElementInit> initializers = this.VisitElementInitializerList(binding.Initializers);
      return initializers != binding.Initializers ? Expression.ListBind(binding.Member, initializers) : binding;
    }

    protected virtual IEnumerable<MemberBinding> VisitBindingList(
      ReadOnlyCollection<MemberBinding> original)
    {
      List<MemberBinding> memberBindingList = (List<MemberBinding>) null;
      int index1 = 0;
      for (int count = original.Count; index1 < count; ++index1)
      {
        MemberBinding memberBinding = this.VisitBinding(original[index1]);
        if (memberBindingList != null)
          memberBindingList.Add(memberBinding);
        else if (memberBinding != original[index1])
        {
          memberBindingList = new List<MemberBinding>(count);
          for (int index2 = 0; index2 < index1; ++index2)
            memberBindingList.Add(original[index2]);
          memberBindingList.Add(memberBinding);
        }
      }
      return (IEnumerable<MemberBinding>) memberBindingList ?? (IEnumerable<MemberBinding>) original;
    }

    protected virtual IEnumerable<ElementInit> VisitElementInitializerList(
      ReadOnlyCollection<ElementInit> original)
    {
      List<ElementInit> elementInitList = (List<ElementInit>) null;
      int index1 = 0;
      for (int count = original.Count; index1 < count; ++index1)
      {
        ElementInit elementInit = this.VisitElementInitializer(original[index1]);
        if (elementInitList != null)
          elementInitList.Add(elementInit);
        else if (elementInit != original[index1])
        {
          elementInitList = new List<ElementInit>(count);
          for (int index2 = 0; index2 < index1; ++index2)
            elementInitList.Add(original[index2]);
          elementInitList.Add(elementInit);
        }
      }
      return (IEnumerable<ElementInit>) elementInitList ?? (IEnumerable<ElementInit>) original;
    }

    protected virtual Expression VisitLambda(LambdaExpression lambda)
    {
      Expression body = this.Visit(lambda.Body);
      return body != lambda.Body ? (Expression) Expression.Lambda(lambda.Type, body, (IEnumerable<ParameterExpression>) lambda.Parameters) : (Expression) lambda;
    }

    protected virtual NewExpression VisitNew(NewExpression nex)
    {
      IEnumerable<Expression> arguments = (IEnumerable<Expression>) this.VisitExpressionList(nex.Arguments);
      if (arguments == nex.Arguments)
        return nex;
      return nex.Members != null ? Expression.New(nex.Constructor, arguments, (IEnumerable<MemberInfo>) nex.Members) : Expression.New(nex.Constructor, arguments);
    }

    protected virtual Expression VisitMemberInit(MemberInitExpression init)
    {
      NewExpression newExpression = this.VisitNew(init.NewExpression);
      IEnumerable<MemberBinding> bindings = this.VisitBindingList(init.Bindings);
      return newExpression != init.NewExpression || bindings != init.Bindings ? (Expression) Expression.MemberInit(newExpression, bindings) : (Expression) init;
    }

    protected virtual Expression VisitListInit(ListInitExpression init)
    {
      NewExpression newExpression = this.VisitNew(init.NewExpression);
      IEnumerable<ElementInit> initializers = this.VisitElementInitializerList(init.Initializers);
      return newExpression != init.NewExpression || initializers != init.Initializers ? (Expression) Expression.ListInit(newExpression, initializers) : (Expression) init;
    }

    protected virtual Expression VisitNewArray(NewArrayExpression na)
    {
      IEnumerable<Expression> expressions = (IEnumerable<Expression>) this.VisitExpressionList(na.Expressions);
      if (expressions == na.Expressions)
        return (Expression) na;
      return na.NodeType == ExpressionType.NewArrayInit ? (Expression) Expression.NewArrayInit(na.Type.GetElementType(), expressions) : (Expression) Expression.NewArrayBounds(na.Type.GetElementType(), expressions);
    }

    protected virtual Expression VisitInvocation(InvocationExpression iv)
    {
      IEnumerable<Expression> arguments = (IEnumerable<Expression>) this.VisitExpressionList(iv.Arguments);
      Expression expression = this.Visit(iv.Expression);
      return arguments != iv.Arguments || expression != iv.Expression ? (Expression) Expression.Invoke(expression, arguments) : (Expression) iv;
    }
  }
}
