// Decompiled with JetBrains decompiler
// Type: DoNet.Utility.Database.EntitySql.EntityInstanceTool
// Assembly: DoNet.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD5DA16A-6F4C-4458-AC23-48055BBFFF84
// Assembly location: D:\Dian.Web\bin\DoNet.Utility.dll

using DoNet.Utility.Database.EntitySql.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace DoNet.Utility.Database.EntitySql
{
  public static class EntityInstanceTool
  {
    public static T FillOneEntity<T>(IDataReader reader) where T : class, new()
    {
      List<PropertyInfo> entityPropertyInfos = EntityMappingTool.GetEntityPropertyInfos(typeof (T));
      T obj = new T();
      for (int index = 0; index < entityPropertyInfos.Count; ++index)
      {
        if (!reader.IsDBNull(index))
          entityPropertyInfos[index].SetValue((object) obj, reader.GetValue(index), (object[]) null);
      }
      return obj;
    }

    public static GenericPairEntity<TA, TB> FillOnePairEntity<TA, TB>(
      IDataReader reader)
      where TA : class, new()
      where TB : class, new()
    {
      List<PropertyInfo> entityPropertyInfos1 = EntityMappingTool.GetEntityPropertyInfos(typeof (TA));
      List<PropertyInfo> entityPropertyInfos2 = EntityMappingTool.GetEntityPropertyInfos(typeof (TB));
      GenericPairEntity<TA, TB> genericPairEntity = new GenericPairEntity<TA, TB>();
      genericPairEntity.EntityA = new TA();
      genericPairEntity.EntityB = new TB();
      int num = 0;
      for (int index = 0; index < entityPropertyInfos1.Count; ++index)
      {
        if (!reader.IsDBNull(num + index))
          entityPropertyInfos1[index].SetValue((object) genericPairEntity.EntityA, reader.GetValue(num + index), (object[]) null);
      }
      int count = entityPropertyInfos1.Count;
      for (int index = 0; index < entityPropertyInfos2.Count; ++index)
      {
        if (!reader.IsDBNull(count + index))
          entityPropertyInfos2[index].SetValue((object) genericPairEntity.EntityB, reader.GetValue(count + index), (object[]) null);
      }
      return genericPairEntity;
    }

    public static List<string> GetNotNullFields<T>(T entity)
    {
      List<string> entityFieldNames = EntityMappingTool.GetEntityFieldNames(typeof (T));
      List<PropertyInfo> entityPropertyInfos = EntityMappingTool.GetEntityPropertyInfos(typeof (T));
      List<string> notNullFields = new List<string>(entityFieldNames.Count);
      for (int index = 0; index < entityFieldNames.Count; ++index)
      {
        if (entityPropertyInfos[index].GetValue((object) entity, (object[]) null) != null)
          notNullFields.Add(entityFieldNames[index]);
      }
      return notNullFields;
    }

    public static List<PropertyInfo> GetNotNullEntityPropertys<T>(T entity)
    {
      List<PropertyInfo> entityPropertyInfos = EntityMappingTool.GetEntityPropertyInfos(typeof (T));
      List<PropertyInfo> nullEntityPropertys = new List<PropertyInfo>(entityPropertyInfos.Count);
      for (int index = 0; index < entityPropertyInfos.Count; ++index)
      {
        if (entityPropertyInfos[index].GetValue((object) entity, (object[]) null) != null)
          nullEntityPropertys.Add(entityPropertyInfos[index]);
      }
      return nullEntityPropertys;
    }

    public static bool HasPrimaryKeyValue<T>(T entity)
    {
      if (!EntityMappingTool.HasPrimaryKey(typeof (T)))
        return false;
      List<string> notNullFields = EntityInstanceTool.GetNotNullFields<T>(entity);
      if (notNullFields == null)
        return false;
      List<string> primaryKeyEntityFieldNames = EntityMappingTool.GetPrimaryKeyOfEntityField(typeof (T));
      for (int i = 0; i < primaryKeyEntityFieldNames.Count; i++)
      {
        if (!notNullFields.Any<string>((Func<string, bool>) (n => n == primaryKeyEntityFieldNames[i])))
          return false;
      }
      return true;
    }
  }
}
