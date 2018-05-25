using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Help;
using System.Data.SqlClient;
using System.Data;

namespace DAL
{

    public class MyPhoneSNDAL
    {
        SQLHelperAccountManagement sqlhelp = new SQLHelperAccountManagement();
        /// <summary>
        /// 通过手机SN查询UserId
        /// </summary>
        /// <param name="PhoneSN"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public object GetNameByPhoenSN(string PhoneSN, string msg)
        {
            string sql = @"SELECT [UserId] FROM [AccountManagement].[dbo].[PhoneSN]
                          where [PhoneSN] = @PhoneSN";
            SqlParameter[] sqlparameter = new SqlParameter[] {
            new SqlParameter("@PhoneSN",PhoneSN),
            };
            return sqlhelp.ExecuteScalar(sql, sqlparameter, ref msg);
        }
        public object GetUserNameByUserID(string UserID, string msg)
        {
            string sql = @"SELECT [UserName] FROM [TopDB].[dbo].[Personnel] where ID=@ID";
            SqlParameter[] sqlparameter = new SqlParameter[] {
            new SqlParameter("@ID",UserID),
            };
            return sqlhelp.ExecuteScalar(sql, sqlparameter, ref msg);
        }
        public object GetLinkControlNoReplayCount(string msg)
        {
            string sql = @"SELECT [NoReplayCount] FROM [TaoBaoSearchDB].[dbo].[LinkControl]where SearchFrom = 3";
            return sqlhelp.ExecuteScalar(sql, null, ref msg);
        }
        public DataTable GetTopReadyLink(int Count, int UserID, string msg)
        {
            string sql = @" SELECT Top 10 DataNick FROM[TaoBaoSearchDB].[dbo].[ReadyLink]
             where UserID = @UserID and WangwangQ is null";
            SqlParameter[] sqlparameter = new SqlParameter[] {          
            new SqlParameter("@UserID",UserID),
            };
            return sqlhelp.ExecuteDataTable(sql, sqlparameter, ref msg);


        }
    }
}

