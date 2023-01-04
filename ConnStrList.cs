using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class ConnStrList
    {
        /// <summary>
        /// 连接名称
        /// </summary>
        public string Name { get; set; }

        public string Host { get; set; }
        public string DataBase { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        /// <summary>
        /// 连接串
        /// </summary>
        public string ConnStr { get; set; }

        public string FullPath {get;set;}
    }
}
