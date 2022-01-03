// Decompiled with JetBrains decompiler
// Type: DoNet.Utility.Database.EntitySql.SqlType
// Assembly: DoNet.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD5DA16A-6F4C-4458-AC23-48055BBFFF84
// Assembly location: D:\Dian.Web\bin\DoNet.Utility.dll

using System.Data;

namespace DoNet.Utility.Database.EntitySql
{
  public class SqlType
  {
    public static string GetObjectTypeFromSqlType(string sqlType)
    {
      string objectTypeFromSqlType = "string";
      if (string.IsNullOrEmpty(sqlType))
        return objectTypeFromSqlType;
      sqlType = sqlType.ToLower();
      if (sqlType.Contains("char") || sqlType.Contains("text"))
        return objectTypeFromSqlType;
      switch (sqlType)
      {
        case "bigint":
          objectTypeFromSqlType = "long";
          break;
        case "bit":
          objectTypeFromSqlType = "bool";
          break;
        case "date":
          objectTypeFromSqlType = "DateTime";
          break;
        case "datetime":
          objectTypeFromSqlType = "DateTime";
          break;
        case "datetime2":
          objectTypeFromSqlType = "DateTime";
          break;
        case "decimal":
          objectTypeFromSqlType = "decimal";
          break;
        case "float":
          objectTypeFromSqlType = "double";
          break;
        case "int":
          objectTypeFromSqlType = "int";
          break;
        case "money":
          objectTypeFromSqlType = "decimal";
          break;
        case "numeric":
          objectTypeFromSqlType = "decimal";
          break;
        case "smalldatetime":
          objectTypeFromSqlType = "DateTime";
          break;
        case "smallint":
          objectTypeFromSqlType = "short";
          break;
        case "smallmoney":
          objectTypeFromSqlType = "decimal";
          break;
        case "tinyint":
          objectTypeFromSqlType = "byte";
          break;
      }
      return objectTypeFromSqlType;
    }

    public static DbType GetDbTypeFromSqlType(string sqlType)
    {
      DbType dbTypeFromSqlType = DbType.AnsiString;
      if (string.IsNullOrEmpty(sqlType))
        return dbTypeFromSqlType;
      sqlType = sqlType.ToLower();
      if (sqlType.Contains("char") || sqlType.Contains("text"))
        return dbTypeFromSqlType;
      switch (sqlType)
      {
        case "bigint":
          dbTypeFromSqlType = DbType.Int64;
          break;
        case "bit":
          dbTypeFromSqlType = DbType.Boolean;
          break;
        case "date":
          dbTypeFromSqlType = DbType.DateTime;
          break;
        case "datetime":
          dbTypeFromSqlType = DbType.DateTime;
          break;
        case "datetime2":
          dbTypeFromSqlType = DbType.DateTime;
          break;
        case "decimal":
          dbTypeFromSqlType = DbType.Decimal;
          break;
        case "float":
          dbTypeFromSqlType = DbType.Double;
          break;
        case "int":
          dbTypeFromSqlType = DbType.Int32;
          break;
        case "money":
          dbTypeFromSqlType = DbType.Decimal;
          break;
        case "numeric":
          dbTypeFromSqlType = DbType.Decimal;
          break;
        case "smalldatetime":
          dbTypeFromSqlType = DbType.DateTime;
          break;
        case "smallint":
          dbTypeFromSqlType = DbType.Int16;
          break;
        case "smallmoney":
          dbTypeFromSqlType = DbType.Decimal;
          break;
        case "tinyint":
          dbTypeFromSqlType = DbType.Byte;
          break;
      }
      return dbTypeFromSqlType;
    }

    public static string GetDbTypeFromSqlType2(string sqlType)
    {
      string typeFromSqlType2 = "DbType.AnsiString";
      if (string.IsNullOrEmpty(sqlType))
        return typeFromSqlType2;
      sqlType = sqlType.ToLower();
      if (sqlType.Contains("char") || sqlType.Contains("text"))
        return typeFromSqlType2;
      switch (sqlType)
      {
        case "bigint":
          typeFromSqlType2 = "DbType.Int64";
          break;
        case "bit":
          typeFromSqlType2 = "DbType.Boolean";
          break;
        case "date":
          typeFromSqlType2 = "DbType.DateTime";
          break;
        case "datetime":
          typeFromSqlType2 = "DbType.DateTime";
          break;
        case "datetime2":
          typeFromSqlType2 = "DbType.DateTime";
          break;
        case "decimal":
          typeFromSqlType2 = "DbType.Decimal";
          break;
        case "float":
          typeFromSqlType2 = "DbType.Double";
          break;
        case "int":
          typeFromSqlType2 = "DbType.Int32";
          break;
        case "money":
          typeFromSqlType2 = "DbType.Decimal";
          break;
        case "numeric":
          typeFromSqlType2 = "DbType.Decimal";
          break;
        case "smalldatetime":
          typeFromSqlType2 = "DbType.DateTime";
          break;
        case "smallint":
          typeFromSqlType2 = "DbType.Int16";
          break;
        case "smallmoney":
          typeFromSqlType2 = "DbType.Decimal";
          break;
        case "tinyint":
          typeFromSqlType2 = "DbType.Byte";
          break;
      }
      return typeFromSqlType2;
    }
  }
}
