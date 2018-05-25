using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BLL;
using System.Data;
using Help;
using DAL;
using Helpe;

namespace TabControlAPI.Controllers
{

    public class HomeController : ApiController
    {
        JsonHelper jh = new JsonHelper();
        MyPhoneSNBLL pbll = new MyPhoneSNBLL();
        [HttpGet]
        public int GetUerIdByPhoneSN(string PhoneSN)
        {
            return pbll.GetNameByPhoenSN(PhoneSN);
        }
        public string GetTopReadyLink(int Count, int UserID)
        {
            return jh.DataTableToJsonWithJavaScriptSerializer(pbll.GetTopReadyLink(Count, UserID));

        }
        public string GetUserNameByUserID(string UserID) {
            return pbll.GetUserNameByUserID(UserID);
        }
   
     
      
        public int GetLinkControlNoReplayCount() {
         return pbll.GetLinkControlNoReplayCount();
        }
    }
}
