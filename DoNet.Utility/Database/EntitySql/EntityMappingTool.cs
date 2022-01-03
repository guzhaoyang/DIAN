// Decompiled with JetBrains decompiler
// Type: DoNet.Utility.Database.EntitySql.EntityMappingTool
// Assembly: DoNet.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD5DA16A-6F4C-4458-AC23-48055BBFFF84
// Assembly location: D:\Dian.Web\bin\DoNet.Utility.dll

using DoNet.Utility.Database.EntitySql.Attribute;
using DoNet.Utility.Database.EntitySql.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;

namespace DoNet.Utility.Database.EntitySql
{
  internal static class EntityMappingTool
  {
    private static readonly List<EntityMappingEntity> _dbMappingEntityCache = new List<EntityMappingEntity>(1024);

    public static EntityMappingEntity GetDbTableMappingEntity(Type entityType)
    {
      lock (EntityMappingTool._dbMappingEntityCache)
      {
        string typeName = entityType.FullName;
        EntityMappingEntity tableMappingEntity1 = EntityMappingTool._dbMappingEntityCache.FirstOrDefault<EntityMappingEntity>((Func<EntityMappingEntity, bool>) (n => n.EntityTypeName == typeName));
        if (tableMappingEntity1 != null)
          return tableMappingEntity1;
        PropertyInfo[] propertys = entityType.GetProperties();
        if (propertys == null || propertys.Length == 0)
          return (EntityMappingEntity) null;
        DataTable dataTable = (DataTable) null;
        string empty = string.Empty;
        DbHelper database = DbFactory.CreateDatabase();
        List<string> stringList = new List<string>();
        Dictionary<string, PropertyInfo> dictionary1 = new Dictionary<string, PropertyInfo>();
        Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
        Dictionary<string, DbType> dictionary3 = new Dictionary<string, DbType>();
        Dictionary<string, string> dictionary4 = new Dictionary<string, string>();
        Dictionary<string, string> dictionary5 = new Dictionary<string, string>();
        for (int i = 0; i < propertys.Length; i++)
        {
          try
          {
            object[] customAttributes = propertys[i].GetCustomAttributes(typeof (Field), false);
            if ((uint) customAttributes.Length > 0U)
            {
              stringList.Add(propertys[i].Name);
              PropertyInfo propertyInfo = ((IEnumerable<PropertyInfo>) propertys).FirstOrDefault<PropertyInfo>((Func<PropertyInfo, bool>) (n => n.Name == propertys[i].Name));
              dictionary1.Add(propertys[i].Name, propertyInfo);
              dictionary2.Add(propertys[i].Name, ((Field) customAttributes[0]).FieldName);
            }
          }
          catch
          {
          }
        }
        string tableName = ((Table) entityType.GetCustomAttributes(typeof (Table), false)[0]).TableName;
        string commandText1 = "SELECT a.name as ColumnName, c.name as ColumnType, a.is_identity FROM sys.columns a\r\n                    INNER JOIN sys.tables b ON a.object_id = b.object_id\r\n                    INNER JOIN sys.types c ON a.system_type_id = c.system_type_id \r\n                    WHERE b.name = @name ";
        using (DbCommand sqlStringCommand = database.GetSqlStringCommand(commandText1))
        {
          database.AddInParameter(sqlStringCommand, "@name", DbType.AnsiString, (object) tableName);
          dataTable = database.ExecuteDataTable(sqlStringCommand);
        }
        if (dataTable != null || dataTable.Rows.Count > 0)
        {
          foreach (string key in dictionary2.Keys)
          {
            DataRow[] dataRowArray = dataTable.Select(" ColumnName = '" + dictionary2[key] + "' ");
            if (dataRowArray != null || (uint) dataRowArray.Length > 0U)
            {
              dictionary3.Add(key, SqlType.GetDbTypeFromSqlType(dataRowArray[0]["ColumnType"].ToString()));
              if (dataRowArray[0]["is_identity"].ToString() != "0")
                dictionary4.Add(key, dictionary2[key]);
            }
          }
        }
        string commandText2 = "SELECT d.name as ColumnName FROM sys.indexes a\r\n                    INNER JOIN sys.tables b ON a.object_id = b.object_id\r\n                    INNER JOIN sys.index_columns c ON c.object_id = b.object_id\r\n                    INNER JOIN sys.columns d ON d.column_id = c.column_id and d.object_id = b.object_id\r\n                    WHERE b.name = @name AND a.is_primary_key = '1' ";
        using (DbCommand sqlStringCommand = database.GetSqlStringCommand(commandText2))
        {
          database.AddInParameter(sqlStringCommand, "@name", DbType.AnsiString, (object) tableName);
          dataTable = database.ExecuteDataTable(sqlStringCommand);
        }
        if (dataTable != null && dataTable.Rows.Count > 0)
        {
          foreach (string key in dictionary2.Keys)
          {
            DataRow[] dataRowArray = dataTable.Select(" ColumnName = '" + dictionary2[key] + "' ");
            if (dataRowArray != null && (uint) dataRowArray.Length > 0U)
              dictionary5.Add(key, dictionary2[key]);
          }
        }
        EntityMappingEntity tableMappingEntity2 = new EntityMappingEntity()
        {
          EntityTypeName = typeName,
          EntityFieldNames = stringList,
          EntityPropertyInfoMapping = dictionary1,
          DbTableName = tableName,
          DbColumnNameMapping = dictionary2,
          DbColumnTypeMapping = dictionary3,
          DbIdentityMapping = dictionary4,
          DbPrimaryKeyMapping = dictionary5
        };
        EntityMappingTool._dbMappingEntityCache.Add(tableMappingEntity2);
        return tableMappingEntity2;
      }
    }

    public static string GetDbTableName(Type entityType) => EntityMappingTool.GetDbTableMappingEntity(entityType)?.DbTableName;

    public static List<string> GetEntityFieldNames(Type entityType)
    {
      EntityMappingEntity tableMappingEntity = EntityMappingTool.GetDbTableMappingEntity(entityType);
      return tableMappingEntity == null || tableMappingEntity.EntityFieldNames == null ? (List<string>) null : tableMappingEntity.EntityFieldNames;
    }

    public static List<PropertyInfo> GetEntityPropertyInfos(Type entityType)
    {
      EntityMappingEntity tableMappingEntity = EntityMappingTool.GetDbTableMappingEntity(entityType);
      return tableMappingEntity == null || tableMappingEntity.EntityPropertyInfoMapping == null ? (List<PropertyInfo>) null : tableMappingEntity.EntityPropertyInfoMapping.Values.ToList<PropertyInfo>();
    }

    public static List<PropertyInfo> GetEntityPropertyInfos(
      Type entityType,
      List<string> entityFieldNames)
    {
      EntityMappingEntity tableMappingEntity = EntityMappingTool.GetDbTableMappingEntity(entityType);
      if (tableMappingEntity == null || tableMappingEntity.EntityPropertyInfoMapping == null || entityFieldNames == null)
        return (List<PropertyInfo>) null;
      List<PropertyInfo> entityPropertyInfos = new List<PropertyInfo>(entityFieldNames.Count);
      foreach (string entityFieldName in entityFieldNames)
        entityPropertyInfos.Add(tableMappingEntity.EntityPropertyInfoMapping[entityFieldName]);
      return entityPropertyInfos;
    }

    public static string GetDbColumnName(Type entityType, string entityFieldName)
    {
      EntityMappingEntity tableMappingEntity = EntityMappingTool.GetDbTableMappingEntity(entityType);
      return tableMappingEntity == null || tableMappingEntity.DbColumnNameMapping == null || string.IsNullOrEmpty(entityFieldName) ? (string) null : tableMappingEntity.DbColumnNameMapping[entityFieldName];
    }

    public static List<string> GetDbColumnNames(Type entityType)
    {
      EntityMappingEntity tableMappingEntity = EntityMappingTool.GetDbTableMappingEntity(entityType);
      return tableMappingEntity == null || tableMappingEntity.DbColumnNameMapping == null ? (List<string>) null : tableMappingEntity.DbColumnNameMapping.Values.ToList<string>();
    }

    public static List<string> GetDbColumnNames(Type entityType, List<string> entityFieldNames)
    {
      EntityMappingEntity tableMappingEntity = EntityMappingTool.GetDbTableMappingEntity(entityType);
      if (tableMappingEntity == null || tableMappingEntity.DbColumnNameMapping == null || entityFieldNames == null)
        return (List<string>) null;
      List<string> dbColumnNames = new List<string>(entityFieldNames.Count);
      foreach (string entityFieldName in entityFieldNames)
        dbColumnNames.Add(tableMappingEntity.DbColumnNameMapping[entityFieldName]);
      return dbColumnNames;
    }

    public static DbType GetDbColumnType(Type entityType, string entityFieldName)
    {
      EntityMappingEntity tableMappingEntity = EntityMappingTool.GetDbTableMappingEntity(entityType);
      return tableMappingEntity == null || tableMappingEntity.DbColumnTypeMapping == null || string.IsNullOrEmpty(entityFieldName) ? DbType.AnsiString : tableMappingEntity.DbColumnTypeMapping[entityFieldName];
    }

    public static List<DbType> GetDbColumnTypes(
      Type entityType,
      List<string> entityFieldNames)
    {
      EntityMappingEntity tableMappingEntity = EntityMappingTool.GetDbTableMappingEntity(entityType);
      if (tableMappingEntity == null || tableMappingEntity.DbColumnTypeMapping == null || entityFieldNames == null)
        return (List<DbType>) null;
      List<DbType> dbColumnTypes = new List<DbType>(entityFieldNames.Count);
      foreach (string entityFieldName in entityFieldNames)
        dbColumnTypes.Add(tableMappingEntity.DbColumnTypeMapping[entityFieldName]);
      return dbColumnTypes;
    }

    public static List<string> GetPrimaryKeyOfEntityField(Type entityType)
    {
      EntityMappingEntity tableMappingEntity = EntityMappingTool.GetDbTableMappingEntity(entityType);
      return tableMappingEntity == null || tableMappingEntity.DbPrimaryKeyMapping == null ? (List<string>) null : tableMappingEntity.DbPrimaryKeyMapping.Keys.ToList<string>();
    }

    public static bool HasPrimaryKey(Type entityType)
    {
      List<string> keyOfEntityField = EntityMappingTool.GetPrimaryKeyOfEntityField(entityType);
      return keyOfEntityField != null && keyOfEntityField.Count > 0;
    }
  }
}
