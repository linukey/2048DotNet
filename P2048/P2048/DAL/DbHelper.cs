using System;
using System.Configuration;
using System.Data.Common;
using System.Data;
using System.Windows;

namespace P2048.DAL
{
    public static class DbHelper
    {
        //获取数据源类型字符串 providerName
        public static string providerName = ConfigurationManager.ConnectionStrings["connStr"].ProviderName;

        //获取数据源链接字符串
        public static string GetConnectionStringByProvider()
        {
            string returnValue = null;

            ConnectionStringSettingsCollection settings = ConfigurationManager.ConnectionStrings;

            if (settings != null)
            {
                foreach (ConnectionStringSettings cs in settings)
                {
                    if (cs.ProviderName == providerName)
                        returnValue = cs.ConnectionString;
                    break;
                }
            }
            return returnValue;
        }

        //创建DbConnection
        public static DbConnection CreateDbConnection(string connectionString)
        {
            DbConnection connection = null;

            if (connectionString != null)
            {
                try
                {
                    DbProviderFactory factory = DbProviderFactories.GetFactory(providerName);
                    connection = factory.CreateConnection();
                    connection.ConnectionString = connectionString;
                }
                catch (Exception ex)
                {
                    if (connection != null)
                    {
                        connection = null;
                    }
                    MessageBox.Show(ex.Message);
                }
            }

            return connection;
        }

        //创建CreateDbCommand
        public static DbCommand CreateDbCommand(string cmdStr, DbConnection conn)
        {
            DbCommand cmd = null;
            if (cmdStr != null && conn != null)
            {
                try
                {
                    DbProviderFactory factory = DbProviderFactories.GetFactory(providerName);
                    cmd = factory.CreateCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = cmdStr;
                }
                catch (Exception ex)
                {
                    if (cmd != null)
                    {
                        cmd = null;
                    }
                    MessageBox.Show(ex.Message);
                }
            }
            return cmd;
        }

        //创建适配器
        public static DbDataAdapter CreateDbDataAdapter(string cmdStr, string connStr, params DbParameter[] paras)
        {
            DbDataAdapter da = null;
            if (cmdStr != null && connStr != null)
            {
                try
                {
                    DbProviderFactory factory = DbProviderFactories.GetFactory(providerName);
                    da = factory.CreateDataAdapter();
                    DbConnection conn = CreateDbConnection(connStr);
                    DbCommand cmd = CreateDbCommand(cmdStr, conn);
                    cmd.Parameters.AddRange(paras);
                    da.SelectCommand = cmd;
                }
                catch (Exception ex)
                {
                    if (da != null)
                    {
                        da = null;
                    }
                    MessageBox.Show(ex.Message);
                }
            }
            return da;
        }

        #region Excute

        public static int ExcuteNonQuery(string cmdStr, params DbParameter[] paras)
        {
            string connStr = GetConnectionStringByProvider();
            try
            {
                using (DbConnection conn = CreateDbConnection(connStr))
                {
                    DbCommand cmd = CreateDbCommand(cmdStr, conn);
                    cmd.Parameters.AddRange(paras);
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return -1;
        }

        public static object ExcuteScalar(string cmdStr, params DbParameter[] paras)
        {
            string connStr = GetConnectionStringByProvider();
            try
            {
                using (DbConnection conn = CreateDbConnection(connStr))
                {
                    DbCommand cmd = CreateDbCommand(cmdStr, conn);
                    cmd.Parameters.AddRange(paras);
                    conn.Open();
                    return cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return null;
        }

        public static DataSet ExcuteSetData(string cmdStr, params DbParameter[] paras)
        {
            DataSet ds = null;
            if (cmdStr != null)
            {
                try
                {
                    string connStr = GetConnectionStringByProvider();
                    DbDataAdapter da = CreateDbDataAdapter(cmdStr, connStr, paras);
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    if (ds != null)
                    {
                        ds = null;
                    }
                    MessageBox.Show(ex.Message);
                }
            }
            return ds;
        }

        #endregion
    }
}