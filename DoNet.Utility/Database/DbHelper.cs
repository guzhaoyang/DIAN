// Decompiled with JetBrains decompiler
// Type: DoNet.Utility.Database.DbHelper
// Assembly: DoNet.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD5DA16A-6F4C-4458-AC23-48055BBFFF84
// Assembly location: D:\Dian.Web\bin\DoNet.Utility.dll

using DoNet.Utility.Web;
using System;
using System.Data;
using System.Data.Common;

namespace DoNet.Utility.Database
{
  public class DbHelper
  {
    private const string StrSetIsoLevelReadCommited = " SET TRANSACTION ISOLATION LEVEL READ COMMITTED; ";
    private const string StrSetIsoLevelReadUnCommited = " SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; ";

    public DbHelper(string dbName = "", bool checkStoredProcedurePara = true)
    {
      if (string.IsNullOrEmpty(dbName))
      {
        this.ConnectionString = ConfigHelper.GetDbString(2);
        if (string.IsNullOrEmpty(this.ConnectionString))
          throw new Exception("尚未配置数据库连接字符串！");
        this.DbName = ConfigHelper.GetDbName(2);
        this.ProviderName = ConfigHelper.GetDbProviderName(2);
        this.ProviderFactory = DbProviderFactories.GetFactory(this.ProviderName);
      }
      else
      {
        this.ConnectionString = ConfigHelper.GetDbString(dbName);
        if (string.IsNullOrEmpty(this.ConnectionString))
          throw new Exception("尚未配置数据库连接字符串！");
        this.DbName = ConfigHelper.GetDbName(dbName);
        this.ProviderName = ConfigHelper.GetDbProviderName(dbName);
        this.ProviderFactory = DbProviderFactories.GetFactory(this.ProviderName);
      }
      if (Encryption.IsEncrypted(this.ConnectionString))
        this.ConnectionString = Encryption.Decrypt(this.ConnectionString);
      this.CheckStoredProcedurePara = checkStoredProcedurePara;
    }

    public bool CheckStoredProcedurePara { get; set; }

    public string DbName { get; private set; }

    public string ConnectionString { get; }

    public string ProviderName { get; }

    public DbProviderFactory ProviderFactory { get; }

    public DbTransaction DbTransactions { get; set; }

    public DbParameter AddInParameter(
      DbCommand dm,
      string name,
      DbType dbType,
      object value)
    {
      return this.AddParameter(dm, name, dbType, 0, ParameterDirection.Input, false, (byte) 0, (byte) 0, string.Empty, DataRowVersion.Default, value);
    }

    public virtual DbParameter AddParameter(
      DbCommand dm,
      string name,
      DbType dbType,
      int size,
      ParameterDirection direction,
      bool nullable,
      byte precision,
      byte scale,
      string sourceColumn,
      DataRowVersion sourceVersion,
      object value)
    {
      if (this.CheckInjectAttackForSp(dm, value))
        throw new Exception("输入的部分内容可能对系统稳定性造成影响，操作已停止！[" + value + "]");
      DbParameter parameter = this.ProviderFactory.CreateParameter();
      if (parameter != null)
      {
        parameter.ParameterName = name;
        parameter.DbType = dbType;
        parameter.Size = size;
        parameter.Value = value ?? (object) DBNull.Value;
        parameter.Direction = direction;
        parameter.IsNullable = nullable;
        parameter.SourceColumn = sourceColumn;
        parameter.SourceVersion = sourceVersion;
        dm.Parameters.Add((object) parameter);
      }
      return parameter;
    }

    public virtual DbCommand GetSqlStringCommand(string commandText) => !string.IsNullOrEmpty(commandText) ? this.CreateCommand(CommandType.Text, commandText) : throw new ArgumentException("命令为空", nameof (commandText));

    public virtual DbCommand GetStoredProcCommand(string storedProcedureName) => !string.IsNullOrEmpty(storedProcedureName) ? this.CreateCommand(CommandType.StoredProcedure, storedProcedureName) : throw new ArgumentException("存储过程名字为空", nameof (storedProcedureName));

    public virtual DbCommand GetStoredProcCommand(
      string storedProcedureName,
      params object[] parameterValues)
    {
      DbCommand dm = !string.IsNullOrEmpty(storedProcedureName) ? this.CreateCommand(CommandType.StoredProcedure, storedProcedureName) : throw new ArgumentException("存储过程名字为空", nameof (storedProcedureName));
      for (int index = 0; index < parameterValues.Length; ++index)
      {
        IDataParameter parameter = (IDataParameter) dm.Parameters[index];
        if (this.CheckInjectAttackForSp(dm, parameterValues[index]))
          throw new Exception("输入的部分内容可能对系统稳定性造成影响，操作已停止！[" + parameterValues[index] + "]");
        dm.Parameters[parameter.ParameterName].Value = parameterValues[index] ?? (object) DBNull.Value;
      }
      return dm;
    }

    private DbCommand CreateCommand(CommandType commandType, string commandText = "")
    {
      DbCommand command = this.ProviderFactory.CreateCommand();
      if (command != null)
      {
        command.CommandType = commandType;
        command.CommandText = commandText;
      }
      return command;
    }

    public virtual DbTransaction CreateTransaction() => this.CreateTransaction(IsolationLevel.ReadCommitted);

    public virtual DbTransaction CreateTransaction(IsolationLevel iso) => this.CreateConnection().BeginTransaction(iso);

    public virtual DbConnection CreateConnection()
    {
      DbConnection connection = this.ProviderFactory.CreateConnection();
      if (connection != null && connection.State != ConnectionState.Open)
      {
        connection.ConnectionString = this.ConnectionString;
        connection.Open();
      }
      return connection;
    }

    public virtual DataTable ExecuteDataTable(string commandText)
    {
      using (DbCommand sqlStringCommand = this.GetSqlStringCommand(commandText))
        return this.ExecuteDataTable(sqlStringCommand);
    }

    public virtual DataTable ExecuteDataTable(DbCommand dm)
    {
      using (DbConnection connection = this.CreateConnection())
      {
        dm.Connection = connection;
        DataTable dataTable = new DataTable();
        using (DbDataAdapter dataAdapter = this.ProviderFactory.CreateDataAdapter())
        {
          if (dataAdapter != null)
          {
            dataAdapter.SelectCommand = dm;
            dataAdapter.Fill(dataTable);
          }
          dataTable.RemotingFormat = SerializationFormat.Binary;
          return dataTable;
        }
      }
    }

    public virtual DataSet ExecuteDataSet(string commandText)
    {
      using (DbCommand sqlStringCommand = this.GetSqlStringCommand(commandText))
        return this.ExecuteDataSet(sqlStringCommand);
    }

    public virtual DataSet ExecuteDataSet(DbCommand dm)
    {
      using (DbConnection connection = this.CreateConnection())
      {
        dm.Connection = connection;
        using (DbDataAdapter dataAdapter = this.ProviderFactory.CreateDataAdapter())
        {
          DataSet dataSet = new DataSet();
          if (dataAdapter != null)
          {
            dataAdapter.SelectCommand = dm;
            dataAdapter.Fill(dataSet);
          }
          dataSet.RemotingFormat = SerializationFormat.Binary;
          return dataSet;
        }
      }
    }

    public virtual int ExecuteNonQuery(string commandText)
    {
      using (DbCommand sqlStringCommand = this.GetSqlStringCommand(commandText))
        return this.ExecuteNonQuery(sqlStringCommand);
    }

    public virtual int ExecuteNonQuery(DbCommand dm)
    {
      using (DbConnection connection = this.CreateConnection())
      {
        dm.Connection = connection;
        return dm.ExecuteNonQuery();
      }
    }

    public virtual int ExecuteNonQuery(string commandText, DbTransaction dt)
    {
      using (DbCommand sqlStringCommand = this.GetSqlStringCommand(commandText))
        return sqlStringCommand == null ? 0 : this.ExecuteNonQuery(sqlStringCommand, dt);
    }

    public virtual int ExecuteNonQuery(DbCommand dm, DbTransaction dt)
    {
      dm.Transaction = dt;
      dm.Connection = dt.Connection;
      return dm.ExecuteNonQuery();
    }

    public virtual IDataReader ExecuteReader(string commandText)
    {
      using (DbCommand sqlStringCommand = this.GetSqlStringCommand(commandText))
        return this.ExecuteReader(sqlStringCommand);
    }

    public virtual IDataReader ExecuteReader(DbCommand dm)
    {
      dm.Connection = this.CreateConnection();
      if (dm.CommandType == CommandType.Text)
        dm.CommandText = " SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; " + dm.CommandText;
      return (IDataReader) dm.ExecuteReader(CommandBehavior.CloseConnection);
    }

    public virtual IDataReader ExecuteReader(
      string storedProcedureName,
      params object[] parameterValues)
    {
      using (DbCommand storedProcCommand = this.GetStoredProcCommand(storedProcedureName, parameterValues))
        return this.ExecuteReader(storedProcCommand);
    }

    public virtual object ExecuteScalar(string commandText)
    {
      using (DbCommand sqlStringCommand = this.GetSqlStringCommand(commandText))
      {
        sqlStringCommand.CommandText = " SET TRANSACTION ISOLATION LEVEL READ COMMITTED; " + commandText;
        return this.ExecuteScalar(sqlStringCommand);
      }
    }

    public virtual object ExecuteScalar(DbCommand dm)
    {
      using (DbConnection connection = this.CreateConnection())
      {
        dm.Connection = connection;
        if (dm.CommandType == CommandType.Text)
          dm.CommandText = " SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; " + dm.CommandText;
        return dm.ExecuteScalar();
      }
    }

    public virtual object ExecuteScalar(string storedProcedureName, params object[] parameterValues)
    {
      using (DbCommand storedProcCommand = this.GetStoredProcCommand(storedProcedureName, parameterValues))
        return this.ExecuteScalar(storedProcCommand);
    }

    private bool CheckInjectAttackForSp(DbCommand dm)
    {
      if (!this.CheckStoredProcedurePara || dm == null || dm.CommandType != CommandType.StoredProcedure || dm.Parameters.Count == 0)
        return false;
      for (int index = 0; index < dm.Parameters.Count; ++index)
      {
        if (dm.Parameters[index].Value != null && !(dm.Parameters[index].Value is DBNull) && dm.Parameters[index].Value is string && !SqlInjectionReject.CheckMssqlParameter(dm.Parameters[index].Value.ToString()))
          return true;
      }
      return false;
    }

    private bool CheckInjectAttackForSp(DbCommand dm, object val) => this.CheckStoredProcedurePara && dm != null && dm.CommandType == CommandType.StoredProcedure && val != null && !(val is DBNull) && val is string && !SqlInjectionReject.CheckMssqlParameter(val.ToString());
  }
}
