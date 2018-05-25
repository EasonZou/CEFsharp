using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class MyPhoneSNBLL
    {
        MyPhoneSNDAL pdal = new MyPhoneSNDAL();
        public int GetNameByPhoenSN(string PhoneSN) {
            object resu = pdal.GetNameByPhoenSN(PhoneSN, "");
            if (resu != null)
            {
                resu = pdal.GetNameByPhoenSN(PhoneSN, "");
            }
            else {
                return 0;
            }
            return Convert.ToInt32(resu);
        }
        public string GetUserNameByUserID(string UserID) {
            object resu = pdal.GetUserNameByUserID(UserID, "");
            if (resu != null)
            {
                resu = pdal.GetUserNameByUserID(UserID, "").ToString();
            }
            else
            {
                return "名字错误";
            }
            return resu.ToString();
        }
        public int GetLinkControlNoReplayCount() {
            return Convert.ToInt32(pdal.GetLinkControlNoReplayCount(""));
          
        }
        public DataTable GetTopReadyLink(int Count, int UserID) {
            DataTable dt = pdal.GetTopReadyLink(Count, UserID, "");
            return dt;


        }
    }
}
