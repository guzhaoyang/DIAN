// Decompiled with JetBrains decompiler
// Type: DoNet.Utility.Database.EntitySql.SqlCreator
// Assembly: DoNet.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD5DA16A-6F4C-4458-AC23-48055BBFFF84
// Assembly location: D:\Dian.Web\bin\DoNet.Utility.dll

using DoNet.Utility.Database.EntitySql.Entity;
using DoNet.Utility.Database.EntitySql.ExpressionVisitor;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;

namespace DoNet.Utility.Database.EntitySql
{
  internal static class SqlCreator
  {
    public static string CreateSelectSql<T>(GenericWhereEntity<T> theWhereEntity, int topCount) => SqlCreator.CreateSelectSql<T>(theWhereEntity, (List<string>) null, topCount);

    public static string CreateSelectSql<T>(
      GenericWhereEntity<T> theWhereEntity,
      List<string> dbColumnNames = null,
      int topCount = 0)
    {
      string dbTableName = EntityMappingTool.GetDbTableName(theWhereEntity.EntityType);
      if (string.IsNullOrEmpty(dbTableName))
        throw new EntitySqlException(string.Format("未给类型{0}设置数据表信息!", (object) theWhereEntity.EntityType.FullName));
      if (dbColumnNames == null)
        dbColumnNames = EntityMappingTool.GetDbColumnNames(typeof (T));
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("SELECT ");
      if (topCount > 0)
        stringBuilder.AppendFormat("TOP {0} ", (object) topCount);
      for (int index = 0; index < dbColumnNames.Count; ++index)
      {
        if (index > 0)
          stringBuilder.Append(", ");
        if (theWhereEntity.DisableTableAlias)
          stringBuilder.Append(string.Format("{0}.[{1}]", (object) dbTableName, (object) dbColumnNames[index]));
        else
          stringBuilder.Append(string.Format("{0}.[{1}]", (object) theWhereEntity.TableName, (object) dbColumnNames[index]));
      }
      return stringBuilder.ToString();
    }

    public static string CreateWhereSql<T>(GenericWhereEntity<T> theWhereEntity)
    {
      string dbTableName = EntityMappingTool.GetDbTableName(theWhereEntity.EntityType);
      if (string.IsNullOrEmpty(dbTableName))
        throw new EntitySqlException(string.Format("未给类型{0}设置数据表信息!", (object) theWhereEntity.EntityType.FullName));
      StringBuilder stringBuilder = new StringBuilder(2048);
      if (theWhereEntity.DisableTableAlias)
        stringBuilder.Append(" FROM [").Append(dbTableName).Append("]");
      else
        stringBuilder.Append(" FROM [").Append(dbTableName).Append("] AS ").Append(theWhereEntity.TableName);
      if (theWhereEntity.WhereExpressions.Count > 0)
      {
        stringBuilder.Append(" WHERE ");
        for (int index = 0; index < theWhereEntity.WhereExpressions.Count; ++index)
        {
          ConditionBuilderGeneric<T> conditionBuilderGeneric = new ConditionBuilderGeneric<T>(theWhereEntity.DisableTableAlias ? dbTableName : theWhereEntity.TableName, theWhereEntity);
          conditionBuilderGeneric.Build(theWhereEntity.WhereExpressions[index]);
          if (index > 0)
            stringBuilder.Append(" AND ");
          stringBuilder.Append(conditionBuilderGeneric.Condition);
          if (conditionBuilderGeneric.Arguments != null && (uint) conditionBuilderGeneric.Arguments.Length > 0U)
            theWhereEntity.WhereParameterValues.AddRange((IEnumerable<object>) conditionBuilderGeneric.Arguments);
          if (conditionBuilderGeneric.ParameterNames != null && (uint) conditionBuilderGeneric.ParameterNames.Length > 0U)
            theWhereEntity.WhereParameterNames.AddRange((IEnumerable<string>) conditionBuilderGeneric.ParameterNames);
          if (conditionBuilderGeneric.DbTypes != null && (uint) conditionBuilderGeneric.DbTypes.Length > 0U)
            theWhereEntity.WhereParameterTypes.AddRange((IEnumerable<DbType>) conditionBuilderGeneric.DbTypes);
        }
      }
      return stringBuilder.ToString();
    }

    public static DbCommand CreateInsertCommand<T>(DbHelper db, T entity)
    {
      Type entityType = typeof (T);
      List<string> notNullFields = EntityInstanceTool.GetNotNullFields<T>(entity);
      List<string> dbColumnNames = EntityMappingTool.GetDbColumnNames(entityType, notNullFields);
      List<DbType> dbColumnTypes = EntityMappingTool.GetDbColumnTypes(entityType, notNullFields);
      List<PropertyInfo> nullEntityPropertys = EntityInstanceTool.GetNotNullEntityPropertys<T>(entity);
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("INSERT INTO [").Append(EntityMappingTool.GetDbTableName(entity.GetType())).Append("] (");
      for (int index = 0; index < dbColumnNames.Count; ++index)
      {
        if (index > 0)
          stringBuilder.Append(", ");
        stringBuilder.Append("[").Append(dbColumnNames[index]).Append("]");
      }
      stringBuilder.Append(") VALUES (");
      for (int index = 0; index < dbColumnNames.Count; ++index)
      {
        if (index > 0)
          stringBuilder.Append(", ");
        stringBuilder.Append("@").Append(dbColumnNames[index]);
      }
      stringBuilder.Append(")");
      DbCommand sqlStringCommand = db.GetSqlStringCommand(stringBuilder.ToString());
      for (int index = 0; index < dbColumnNames.Count; ++index)
        db.AddInParameter(sqlStringCommand, "@" + dbColumnNames[index], dbColumnTypes[index], nullEntityPropertys[index].GetValue((object) entity, (object[]) null));
      return sqlStringCommand;
    }

    public static DbCommand CreateInsertCommandWithIdentity<T>(DbHelper db, T entity)
    {
      Type entityType = typeof (T);
      List<string> notNullFields = EntityInstanceTool.GetNotNullFields<T>(entity);
      List<string> dbColumnNames = EntityMappingTool.GetDbColumnNames(entityType, notNullFields);
      List<DbType> dbColumnTypes = EntityMappingTool.GetDbColumnTypes(entityType, notNullFields);
      List<PropertyInfo> nullEntityPropertys = EntityInstanceTool.GetNotNullEntityPropertys<T>(entity);
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("INSERT INTO [").Append(EntityMappingTool.GetDbTableName(entityType)).Append("] (");
      for (int index = 0; index < dbColumnNames.Count; ++index)
      {
        if (index > 0)
          stringBuilder.Append(", ");
        stringBuilder.Append("[").Append(dbColumnNames[index]).Append("]");
      }
      stringBuilder.Append(") VALUES (");
      for (int index = 0; index < dbColumnNames.Count; ++index)
      {
        if (index > 0)
          stringBuilder.Append(", ");
        stringBuilder.Append("@").Append(dbColumnNames[index]);
      }
      stringBuilder.Append(") select @@identity");
      DbCommand sqlStringCommand = db.GetSqlStringCommand(stringBuilder.ToString());
      for (int index = 0; index < dbColumnNames.Count; ++index)
        db.AddInParameter(sqlStringCommand, "@" + dbColumnNames[index], dbColumnTypes[index], nullEntityPropertys[index].GetValue((object) entity, (object[]) null));
      return sqlStringCommand;
    }

    public static DbCommand CreateUpdateCommand<T>(DbHelper db, T entity)
    {
      Type entityType = typeof (T);
      List<string> keyOfEntityField = EntityMappingTool.GetPrimaryKeyOfEntityField(entityType);
      List<string> dbColumnNames1 = EntityMappingTool.GetDbColumnNames(entityType, keyOfEntityField);
      EntityMappingTool.GetDbColumnTypes(entityType, keyOfEntityField);
      List<string> notNullFields = EntityInstanceTool.GetNotNullFields<T>(entity);
      List<string> dbColumnNames2 = EntityMappingTool.GetDbColumnNames(entityType, notNullFields);
      List<DbType> dbColumnTypes = EntityMappingTool.GetDbColumnTypes(entityType, notNullFields);
      List<PropertyInfo> nullEntityPropertys = EntityInstanceTool.GetNotNullEntityPropertys<T>(entity);
      List<string> stringList = new List<string>();
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("UPDATE [").Append(EntityMappingTool.GetDbTableName(entityType)).Append("] SET ");
      bool flag = true;
      for (int index = 0; index < dbColumnNames2.Count; ++index)
      {
        string str = dbColumnNames2[index];
        if (!dbColumnNames1.Contains(str))
        {
          stringBuilder.Append(flag ? "" : ",");
          flag = false;
          stringBuilder.AppendFormat("[{0}]=@{0}", (object) str);
          stringList.Add(str);
        }
      }
      stringBuilder.Append(" WHERE ");
      for (int index = 0; index < dbColumnNames1.Count; ++index)
      {
        stringBuilder.Append(index > 0 ? " AND " : "");
        stringBuilder.AppendFormat("([{0}]=@{0})", (object) dbColumnNames1[index]);
        stringList.Add(dbColumnNames1[index]);
      }
      DbCommand sqlStringCommand = db.GetSqlStringCommand(stringBuilder.ToString());
      for (int index1 = 0; index1 < stringList.Count; ++index1)
      {
        int index2 = dbColumnNames2.IndexOf(stringList[index1]);
        db.AddInParameter(sqlStringCommand, "@" + dbColumnNames2[index2], dbColumnTypes[index2], nullEntityPropertys[index2].GetValue((object) entity, (object[]) null));
      }
      return sqlStringCommand;
    }

    public static DbCommand CreateUpdateCommand<T>(
      DbHelper db,
      T entity,
      GenericWhereEntity<T> whereEntity)
    {
      Type entityType = typeof (T);
      List<string> notNullFields = EntityInstanceTool.GetNotNullFields<T>(entity);
      List<string> dbColumnNames = EntityMappingTool.GetDbColumnNames(entityType, notNullFields);
      List<DbType> dbColumnTypes = EntityMappingTool.GetDbColumnTypes(entityType, notNullFields);
      List<PropertyInfo> nullEntityPropertys = EntityInstanceTool.GetNotNullEntityPropertys<T>(entity);
      List<string> stringList = new List<string>();
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendFormat("UPDATE {0} SET ", (object) whereEntity.TableName);
      bool flag = true;
      for (int index = 0; index < dbColumnNames.Count; ++index)
      {
        string str = dbColumnNames[index];
        stringBuilder.Append(flag ? "" : ",");
        flag = false;
        stringBuilder.AppendFormat("{0}.[{1}]=@{1}", (object) whereEntity.TableName, (object) str);
        stringList.Add(str);
      }
      string whereSql = SqlCreator.CreateWhereSql<T>(whereEntity);
      stringBuilder.Append(" ").Append(whereSql);
      DbCommand sqlStringCommand = db.GetSqlStringCommand(stringBuilder.ToString());
      for (int index = 0; index < dbColumnNames.Count; ++index)
        db.AddInParameter(sqlStringCommand, "@" + dbColumnNames[index], dbColumnTypes[index], nullEntityPropertys[index].GetValue((object) entity, (object[]) null));
      SqlCreator.FillSqlParameters<T>(db, sqlStringCommand, whereEntity);
      return sqlStringCommand;
    }

    public static DbCommand CreateUpdateMemberToNullCommand<T>(
      DbHelper db,
      GenericWhereEntity<T> whereEntity,
      string memberName)
    {
      string dbColumnName = EntityMappingTool.GetDbColumnName(typeof (T), memberName);
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendFormat("UPDATE {0} SET  {0}.[{1}]=null", (object) whereEntity.TableName, (object) dbColumnName);
      string whereSql = SqlCreator.CreateWhereSql<T>(whereEntity);
      stringBuilder.Append(" ").Append(whereSql);
      DbCommand sqlStringCommand = db.GetSqlStringCommand(stringBuilder.ToString());
      SqlCreator.FillSqlParameters<T>(db, sqlStringCommand, whereEntity);
      return sqlStringCommand;
    }

    public static DbCommand CreateUpdateMemberToNullCommand<T>(
      DbHelper db,
      GenericWhereEntity<T> whereEntity,
      List<string> memberNames)
    {
      List<string> dbColumnNames = EntityMappingTool.GetDbColumnNames(typeof (T), memberNames);
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendFormat("UPDATE {0} SET", (object) whereEntity.TableName);
      for (int index = 0; index < dbColumnNames.Count; ++index)
      {
        stringBuilder.Append(index == 0 ? "" : ",");
        stringBuilder.AppendFormat("{0}.[{1}]=null", (object) whereEntity.TableName, (object) dbColumnNames[index]);
      }
      string whereSql = SqlCreator.CreateWhereSql<T>(whereEntity);
      stringBuilder.Append(" ").Append(whereSql);
      DbCommand sqlStringCommand = db.GetSqlStringCommand(stringBuilder.ToString());
      SqlCreator.FillSqlParameters<T>(db, sqlStringCommand, whereEntity);
      return sqlStringCommand;
    }

    public static DbCommand CreatDeleteCommand<T>(DbHelper db, T entity)
    {
      Type entityType = typeof (T);
      List<string> keyOfEntityField = EntityMappingTool.GetPrimaryKeyOfEntityField(entityType);
      List<string> dbColumnNames = EntityMappingTool.GetDbColumnNames(entityType, keyOfEntityField);
      List<DbType> dbColumnTypes = EntityMappingTool.GetDbColumnTypes(entityType, keyOfEntityField);
      List<PropertyInfo> entityPropertyInfos = EntityMappingTool.GetEntityPropertyInfos(entityType, keyOfEntityField);
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendFormat("DELETE FROM [{0}] WHERE ", (object) EntityMappingTool.GetDbTableName(entityType));
      for (int index = 0; index < keyOfEntityField.Count; ++index)
      {
        stringBuilder.Append(index > 0 ? " AND " : "");
        stringBuilder.AppendFormat("([{0}]=@{0})", (object) dbColumnNames[index]);
      }
      DbCommand sqlStringCommand = db.GetSqlStringCommand(stringBuilder.ToString());
      for (int index = 0; index < keyOfEntityField.Count; ++index)
        db.AddInParameter(sqlStringCommand, "@" + dbColumnNames[index], dbColumnTypes[index], entityPropertyInfos[index].GetValue((object) entity, (object[]) null));
      return sqlStringCommand;
    }

    public static DbCommand CreatDeleteCommand<T>(
      DbHelper db,
      GenericWhereEntity<T> whereEntity)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendFormat("DELETE ", (object) EntityMappingTool.GetDbTableName(typeof (T)));
      stringBuilder.Append(SqlCreator.CreateWhereSql<T>(whereEntity));
      DbCommand sqlStringCommand = db.GetSqlStringCommand(stringBuilder.ToString());
      SqlCreator.FillSqlParameters<T>(db, sqlStringCommand, whereEntity);
      return sqlStringCommand;
    }

    public static void FillSqlParameters<T>(
      DbHelper db,
      DbCommand cmd,
      GenericWhereEntity<T> whereEntity)
    {
      for (int index = 0; index < whereEntity.WhereParameterNames.Count; ++index)
        db.AddInParameter(cmd, whereEntity.WhereParameterNames[index], whereEntity.WhereParameterTypes[index], whereEntity.WhereParameterValues[index]);
    }
  }
}
