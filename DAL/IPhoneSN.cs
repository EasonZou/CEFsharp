using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    interface IPhoneSN
    {
        /// <summary>
        /// 通过SN获取花名
        /// </summary>
        /// <returns></returns>
        object GetNameByPhoenSN(string UserID);
       
    }
}
