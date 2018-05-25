using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Help
{
    public class SQLHelperAccountManagement
    {
        //验证方式
        public static string ValidateWay = "";
        //数据库地址
        public static string Address = "139.224.187.151";
        //数据库名称
        public static string DBName = "Master";
        //登录名
        public static string UserID = "sa";
        //登录密码
        public static string Password = "maijia123456";

        //SQL Server 身份验证方式字符串
        private string SqlConCONSTR = "Data Source = {0};Initial Catalog = {1};User Id = {2};Password = {3};Connect Timeout = 3";

        private string SqlConCONSTR_Windows = "Data Source= {0};Initial Catalog= {1};Integrated Security=True;Connect Timeout = 3";

        /// <summary>
        /// 获取SQL连接
        /// </summary>
        /// <param name="msg">获取SQL连接时报错的错误信息</param>
        /// <returns>SqlConnection</returns>
        public SqlConnection GetSqlConnection(ref string msg)
        {
            SqlConnection con = null;
            try
            {
                if (SQLHelperAccountManagement.ValidateWay == "Windows")
                {
                    con = new SqlConnection(string.Format(SqlConCONSTR_Windows, SQLHelperAccountManagement.Address, SQLHelperAccountManagement.DBName));
                }
                else
                {
                    con = new SqlConnection(string.Format(SqlConCONSTR, SQLHelperAccountManagement.Address, SQLHelperAccountManagement.DBName, SQLHelperAccountManagement.UserID, SQLHelperAccountManagement.Password));
                }
            }
            catch (SqlException se)
            {
                msg += "SQL连接错误：" + se.Message + "\n";
            }
            catch (Exception ex)
            {
                msg += "异常：" + ex.Message + "\n";
            }
            return con;
        }

        /// <summary>
        /// 获取SQL连接(Windows验证方式)
        /// </summary>
        /// <param name="address">目标库地址</param>
        /// <param name="dbname">目标库名称</param>
        /// <param name="msg">错误信息</param>
        /// <returns>SqlConnection</returns>
        public SqlConnection GetSqlConnection(string address, string dbname, ref string msg)
        {
            SqlConnection con = null;
            try
            {
                con = new SqlConnection(string.Format(SqlConCONSTR_Windows, address, dbname));
            }
            catch (SqlException se)
            {
                msg += "SQL连接错误：" + se.Message + "\n";
            }
            catch (Exception ex)
            {
                msg += "异常：" + ex.Message + "\n";
            }
            return con;
        }

        /// <summary>
        /// 获取SQL连接
        /// </summary>
        /// <param name="address">服务器地址</param>
        /// <param name="dbname">数据库名称</param>
        /// <param name="uid">登录名</param>
        /// <param name="pwd">密码</param>
        /// <param name="msg">获取SQL连接时报错的错误信息</param>
        /// <returns>SqlConnection</returns>
        public SqlConnection GetSqlConnection(string address, string dbname, string uid, string pwd, ref string msg)
        {
            SqlConnection con = null;
            try
            {
                con = new SqlConnection(string.Format(SqlConCONSTR, address, dbname, uid, pwd));
            }
            catch (SqlException se)
            {
                msg += "SQL连接错误：" + se.Message + "\n";
            }
            catch (Exception ex)
            {
                msg += "异常：" + ex.Message + "\n";
            }
            return con;
        }

        /// <summary>
        /// 关闭SQL连接
        /// </summary>
        /// <param name="con">传入SQLConnection连接对象</param>
        /// <param name="msg">关闭SQL连接时的异常信息</param>
        public void CloseSqlConnection(SqlConnection con, ref string msg)
        {
            try
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
            catch (SqlException se)
            {
                msg += "SQL错误：" + se.Message + "\n";
            }
            catch (Exception ex)
            {
                msg += "异常：" + ex.Message + "\n";
            }
        }

        /// <summary>
        /// 执行增删改操作
        /// </summary>
        /// <param name="sqlStr">SQL语句</param>
        /// <param name="sqlparams">参数数组</param>
        /// <param name="msg">执行语句时的异常信息</param>
        /// <param name="CommandTimeout">执行语句超时时限(默认0)</param>
        /// <returns>result</returns>
        public int ExecuteNonQuery(string sqlStr, SqlParameter[] sqlparams, ref string msg, int CommandTimeout = 0)
        {
            int result = 0;
            using (SqlConnection con = GetSqlConnection(ref msg))
            {
                con.Open();
                SqlTransaction tx = con.BeginTransaction();
                try
                {
                    SqlCommand com = new SqlCommand(sqlStr, con);
                    com.Transaction = tx;
                    com.CommandTimeout = CommandTimeout;
                    com.Prepare();      //开启预处理
                    if (sqlparams != null)
                    {
                        com.Parameters.AddRange(sqlparams);
                    }
                    result = com.ExecuteNonQuery();
                    tx.Commit();
                }
                catch (SqlException se)
                {
                    tx.Rollback();
                    result = -1;
                    msg += "SQL执行错误：" + se.Message + "\n";
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                    result = -1;
                    msg += "异常：" + ex.Message + "\n";
                }
                finally
                {
                    con.Close();
                }
            }   //using
            return result;
        }

        /// <summary>
        /// 查询单行单列数据
        /// </summary>
        /// <param name="sqlStr">SQL语句</param>
        /// <param name="sqlparams">参数数组</param>
        /// <param name="msg">执行语句时的异常信息</param>
        /// <param name="CommandTimeout">执行语句超时时限(默认0)</param>
        /// <returns>object</returns>
        public object ExecuteScalar(string sqlStr, SqlParameter[] sqlparams, ref string msg, int CommandTimeout = 0)
        {
            object obj = null;
            using (SqlConnection con = GetSqlConnection(ref msg))
            {
                con.Open();
                try
                {
                    SqlCommand com = new SqlCommand(sqlStr, con);
                    com.CommandTimeout = CommandTimeout;
                    if (sqlparams != null)
                    {
                        com.Parameters.AddRange(sqlparams);
                    }
                    obj = com.ExecuteScalar();
                }
                catch (SqlException se)
                {
                    msg += "SQL执行错误：" + se.Message + "\n";
                }
                catch (Exception ex)
                {
                    msg += "异常：" + ex.Message + "\n";
                }
                finally
                {
                    con.Close();
                }
            }
            return obj;
        }

        /// <summary>
        /// 获取DataSet对象
        /// </summary>
        /// <param name="sqlStr">SQL语句</param>
        /// <param name="sqlparams">参数数组</param>
        /// <param name="msg">执行语句时的异常信息</param>
        /// <param name="CommandTimeout">执行语句超时时限(默认0)</param>
        /// <returns>DataSet</returns>
        public DataSet ExecuteDataSet(string sqlStr, SqlParameter[] sqlparams, ref string msg, int CommandTimeout = 0)
        {
            DataSet ds = null;
            using (SqlConnection con = GetSqlConnection(ref msg))
            {
                con.Open();
                try
                {
                    ds = new DataSet();
                    SqlCommand com = new SqlCommand(sqlStr, con);
                    com.CommandTimeout = CommandTimeout;
                    if (sqlparams != null)
                    {
                        com.Parameters.AddRange(sqlparams);
                    }
                    SqlDataAdapter adapter = new SqlDataAdapter(com);
                    adapter.Fill(ds);
                }
                catch (SqlException se)
                {
                    msg += "SQL执行错误：" + se.Message + "\n";
                }
                catch (Exception ex)
                {
                    msg += "异常：" + ex.Message + "\n";
                }
                finally
                {
                    con.Close();
                }
            }
            return ds;
        }

        /// <summary>
        /// 获取DataTable数据
        /// </summary>
        /// <param name="sqlStr">SQL语句</param>
        /// <param name="sqlparams">参数数组</param>
        /// <param name="msg">执行语句时的异常信息</param>
        /// <param name="CommandTimeout">执行语句超时时限(默认0)</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteDataTable(string sqlStr, SqlParameter[] sqlparams, ref string msg, int CommandTimeout = 0)
        {
            DataTable dt = null;
            using (SqlConnection con = GetSqlConnection(ref msg))
            {
                con.Open();
                try
                {
                    SqlCommand com = new SqlCommand(sqlStr, con);
                    com.CommandTimeout = CommandTimeout;
                    if (sqlparams != null)
                    {
                        com.Parameters.AddRange(sqlparams);
                    }
                    SqlDataAdapter adapter = new SqlDataAdapter(com);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    if (ds != null)
                    {
                        dt = ds.Tables[0];
                    }
                }
                catch (SqlException se)
                {
                    msg += "SQL执行错误：" + se.Message + "\n";
                }
                catch (Exception ex)
                {
                    msg += "异常：" + ex.Message + "\n";
                }
                finally
                {
                    con.Close();
                }
            }
            return dt;
        }

        /// <summary>
        /// 获取DataReader对象
        /// </summary>
        /// <param name="sqlStr">SQL语句</param>
        /// <param name="sqlparams">参数数组</param>
        /// <param name="msg">执行语句时的异常信息</param>
        /// <param name="CommandTimeout">执行语句超时时限(默认0)</param>
        /// <returns>SqlDataReader</returns>
        public SqlDataReader ExecuteReader(string sqlStr, SqlParameter[] sqlparams, ref string msg, int CommandTimeout = 0)
        {
            SqlDataReader reader = null;
            using (SqlConnection con = GetSqlConnection(ref msg))
            {
                con.Open();
                try
                {
                    SqlCommand com = new SqlCommand(sqlStr, con);
                    com.CommandTimeout = CommandTimeout;
                    if (sqlparams != null)
                    {
                        com.Parameters.AddRange(sqlparams);
                    }
                    reader = com.ExecuteReader(CommandBehavior.CloseConnection);
                }
                catch (SqlException se)
                {
                    msg += "SQL执行错误：" + se.Message + "\n";
                }
                catch (Exception ex)
                {
                    msg += "异常：" + ex.Message + "\n";
                }
            }
            return reader;
        }
    }

}
