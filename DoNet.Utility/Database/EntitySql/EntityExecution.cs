// Decompiled with JetBrains decompiler
// Type: DoNet.Utility.Database.EntitySql.EntityExecution
// Assembly: DoNet.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD5DA16A-6F4C-4458-AC23-48055BBFFF84
// Assembly location: D:\Dian.Web\bin\DoNet.Utility.dll

using DoNet.Utility.Database.EntitySql.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;

namespace DoNet.Utility.Database.EntitySql
{
  public static class EntityExecution
  {
    public static void Insert<T>(this T entity, DbHelper db = null) where T : BaseEntity
    {
      if (db == null)
        db = DbFactory.CreateDatabase();
      using (DbCommand insertCommand = SqlCreator.CreateInsertCommand<T>(db, entity))
      {
        if (db.ExecuteNonQuery(insertCommand) <= 0)
          throw new EntitySqlException("新增记录失败(DB)！");
      }
    }

    public static object InsertWithIdentity<T>(this T entity, DbHelper db = null) where T : BaseEntity
    {
      if (db == null)
        db = DbFactory.CreateDatabase();
      using (DbCommand commandWithIdentity = SqlCreator.CreateInsertCommandWithIdentity<T>(db, entity))
        return db.ExecuteScalar(commandWithIdentity);
    }

    public static int Delete<T>(Expression<Func<T, bool>> conditionExpression, DbHelper db = null) where T : BaseEntity
    {
      if (conditionExpression == null)
        throw new EntitySqlException("删除记录时，必须指定Where条件，否则将导致整个表的数据被删除！");
      if (db == null)
        db = DbFactory.CreateDatabase();
      GenericWhereEntity<T> whereEntity = new GenericWhereEntity<T>();
      whereEntity.Where(conditionExpression);
      return EntityExecution.Delete<T>(whereEntity, db);
    }

    public static int Delete<T>(GenericWhereEntity<T> whereEntity, DbHelper db = null) where T : BaseEntity
    {
      whereEntity.DisableTableAlias = true;
      if (db == null)
        db = DbFactory.CreateDatabase();
      using (DbCommand dm = SqlCreator.CreatDeleteCommand<T>(db, whereEntity))
        return db.ExecuteNonQuery(dm);
    }

    public static int Delete<T>(this T entity, DbHelper db = null) where T : BaseEntity
    {
      Type entityType = typeof (T);
      if (!EntityMappingTool.HasPrimaryKey(entityType))
        throw new EntitySqlException(string.Format("实体类{0}未设置主键字段！", (object) entityType.FullName));
      if (db == null)
        db = DbFactory.CreateDatabase();
      using (DbCommand dm = SqlCreator.CreatDeleteCommand<T>(db, entity))
        return db.ExecuteNonQuery(dm);
    }

    public static void Update<T>(this T entity, DbHelper db = null) where T : BaseEntity
    {
      Type entityType = typeof (T);
      if (!EntityMappingTool.HasPrimaryKey(entityType))
        throw new EntitySqlException(string.Format("实体类{0}未设置主键字段！", (object) entityType.FullName));
      if (!EntityInstanceTool.HasPrimaryKeyValue<T>(entity))
        throw new EntitySqlException(string.Format("未给实体类{0}实例的主键字段赋值！", (object) entityType.FullName));
      if (db == null)
        db = DbFactory.CreateDatabase();
      using (DbCommand updateCommand = SqlCreator.CreateUpdateCommand<T>(db, entity))
      {
        if (db.ExecuteNonQuery(updateCommand) <= 0)
          throw new EntitySqlException("修改记录失败(DB)！");
      }
    }

    public static void Update<T>(
      this T entity,
      Expression<Func<T, bool>> conditionExpression,
      DbHelper db = null)
      where T : BaseEntity
    {
      if (conditionExpression == null)
        throw new EntitySqlException("修改记录时，必须指定Where条件，否则将导致整个表的数据被修改！");
      GenericWhereEntity<T> whereEntity = new GenericWhereEntity<T>();
      if (conditionExpression != null)
        whereEntity.Where(conditionExpression);
      entity.Update<T>(whereEntity, db);
    }

    public static void Update<T>(this T entity, GenericWhereEntity<T> whereEntity, DbHelper db = null) where T : BaseEntity
    {
      if (db == null)
        db = DbFactory.CreateDatabase();
      using (DbCommand updateCommand = SqlCreator.CreateUpdateCommand<T>(db, entity, whereEntity))
        db.ExecuteNonQuery(updateCommand);
    }

    public static void SetMemberNull<T, TKey>(
      Expression<Func<T, bool>> conditionExpression,
      Expression<Func<T, TKey>> keySelector,
      DbHelper db = null)
      where T : BaseEntity
    {
      if (conditionExpression == null)
        throw new EntitySqlException("修改记录时，必须指定Where条件，否则将导致整个表的数据被修改！");
      GenericWhereEntity<T> whereEntity = new GenericWhereEntity<T>();
      if (conditionExpression != null)
        whereEntity.Where(conditionExpression);
      EntityExecution.SetMemberNull<T, TKey>(whereEntity, keySelector, db);
    }

    public static void SetMembersNull<T>(
      Expression<Func<T, bool>> conditionExpression,
      DbHelper db = null,
      params Expression<Func<T, object>>[] keySelectors)
      where T : BaseEntity
    {
      if (conditionExpression == null)
        throw new EntitySqlException("修改记录时，必须指定Where条件，否则将导致整个表的数据被修改！");
      GenericWhereEntity<T> whereEntity = new GenericWhereEntity<T>();
      whereEntity.Where(conditionExpression);
      EntityExecution.SetMembersNull<T>(whereEntity, db, keySelectors);
    }

    public static void SetMemberNull<T, TKey>(
      GenericWhereEntity<T> whereEntity,
      Expression<Func<T, TKey>> keySelector,
      DbHelper db = null)
      where T : BaseEntity
    {
      Type type = typeof (T);
      string name = (keySelector.Body as MemberExpression).Member.Name;
      if (db == null)
        db = DbFactory.CreateDatabase();
      using (DbCommand memberToNullCommand = SqlCreator.CreateUpdateMemberToNullCommand<T>(db, whereEntity, name))
        db.ExecuteNonQuery(memberToNullCommand);
    }

    public static void SetMembersNull<T>(
      GenericWhereEntity<T> whereEntity,
      DbHelper db = null,
      params Expression<Func<T, object>>[] keySelectors)
      where T : BaseEntity
    {
      Type type = typeof (T);
      List<string> memberNames = new List<string>(keySelectors.Length);
      for (int index = 0; index < keySelectors.Length; ++index)
      {
        if (keySelectors[index].Body is MemberExpression body2)
          memberNames.Add(body2.Member.Name);
        else if (keySelectors[index].Body is UnaryExpression body1 && body1.Operand is MemberExpression operand)
          memberNames.Add(operand.Member.Name);
      }
      if (db == null)
        db = DbFactory.CreateDatabase();
      using (DbCommand memberToNullCommand = SqlCreator.CreateUpdateMemberToNullCommand<T>(db, whereEntity, memberNames))
        db.ExecuteNonQuery(memberToNullCommand);
    }

    public static T SelectOne<T>(Expression<Func<T, bool>> conditionExpression, DbHelper db = null) where T : BaseEntity, new()
    {
      GenericWhereEntity<T> whereEntity = new GenericWhereEntity<T>();
      if (conditionExpression != null)
        whereEntity.Where(conditionExpression);
      return EntityExecution.SelectOne<T>(whereEntity, db);
    }

    public static T SelectOne<T>(GenericWhereEntity<T> whereEntity, DbHelper db = null) where T : BaseEntity, new()
    {
      string selectSql = SqlCreator.CreateSelectSql<T>(whereEntity, 1);
      string whereSql = SqlCreator.CreateWhereSql<T>(whereEntity);
      if (db == null)
        db = DbFactory.CreateDatabase();
      using (DbCommand sqlStringCommand = db.GetSqlStringCommand(selectSql + whereSql))
      {
        SqlCreator.FillSqlParameters<T>(db, sqlStringCommand, whereEntity);
        using (IDataReader reader = db.ExecuteReader(sqlStringCommand))
          return reader.Read() ? EntityInstanceTool.FillOneEntity<T>(reader) : default (T);
      }
    }

    public static List<T> SelectAll<T>(
      Expression<Func<T, bool>> conditionExpression = null,
      DbHelper db = null,
      params int[] maxRowCounts)
      where T : BaseEntity, new()
    {
      GenericWhereEntity<T> whereEntity = new GenericWhereEntity<T>();
      if (conditionExpression != null)
        whereEntity.Where(conditionExpression);
      return EntityExecution.SelectAll<T>(whereEntity, db, maxRowCounts);
    }

    public static List<T> SelectAll<T>(
      GenericWhereEntity<T> whereEntity,
      DbHelper db = null,
      params int[] maxRowCounts)
      where T : BaseEntity, new()
    {
      int topCount = maxRowCounts == null || maxRowCounts.Length < 1 ? 0 : maxRowCounts[0];
      string selectSql = SqlCreator.CreateSelectSql<T>(whereEntity, topCount);
      string whereSql = SqlCreator.CreateWhereSql<T>(whereEntity);
      if (db == null)
        db = DbFactory.CreateDatabase();
      using (DbCommand sqlStringCommand = db.GetSqlStringCommand(selectSql + whereSql))
      {
        SqlCreator.FillSqlParameters<T>(db, sqlStringCommand, whereEntity);
        using (IDataReader reader = db.ExecuteReader(sqlStringCommand))
        {
          List<T> objList = new List<T>();
          while (reader.Read())
            objList.Add(EntityInstanceTool.FillOneEntity<T>(reader));
          return objList;
        }
      }
    }

    public static int Count<T>(Expression<Func<T, bool>> conditionExpression, DbHelper db = null) where T : BaseEntity
    {
      GenericWhereEntity<T> whereEntity = new GenericWhereEntity<T>();
      if (conditionExpression != null)
        whereEntity.Where(conditionExpression);
      return EntityExecution.Count<T>(whereEntity, db);
    }

    public static int Count<T>(GenericWhereEntity<T> whereEntity, DbHelper db = null) where T : BaseEntity
    {
      string whereSql = SqlCreator.CreateWhereSql<T>(whereEntity);
      string str = "SELECT COUNT(*) ";
      if (db == null)
        db = DbFactory.CreateDatabase();
      using (DbCommand sqlStringCommand = db.GetSqlStringCommand(str + whereSql))
      {
        SqlCreator.FillSqlParameters<T>(db, sqlStringCommand, whereEntity);
        return (int) db.ExecuteScalar(sqlStringCommand);
      }
    }

    public delegate TResult VisitMember<T, TResult>(T arg);
  }
}
