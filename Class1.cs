using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Database
{
    class Class1
    {
        private List<CustomersItem> customersItems = new List<CustomersItem>();
        public List<CustomersItem> Customers
        {
            get
            {
                return customersItems;
            }
            set
            {
                customersItems = value;
            }
        }
        public class CustomersItem
        {
            public string 表名 { get; set; }
            public string 字段名称 { get; set; }
            public string 字段类型 { get; set; }
            public string 字段长度 { get; set; }
            public string 字段说明 { get; set; }
            public string 数据库名称 { get; set; }
            public CustomersItem(System.String 表名, System.String 字段名称, System.String 字段类型, System.String 字段长度, System.String 字段说明,System.String 数据库名称)
            {
                this.表名 = 表名;
                this.字段名称 = 字段名称;
                this.字段类型 = 字段类型;
                this.字段长度 = 字段长度;
                this.字段说明 = 字段说明;
                this.数据库名称 = 数据库名称;
            }
        }
        public Class1()
        {
            DataTable mytbl = Form1.dt2;
             //mytbl = cor.report_get_short2(IDs, auction_id);
            if (mytbl != null && mytbl.Rows.Count > 0)
            {
                for (int a = 0; a < mytbl.Rows.Count; a++)
                {
                    string bm = mytbl.Rows[a]["tableName"].ToString();
                    string zdm = mytbl.Rows[a]["FieldName"].ToString();
                    string lx = mytbl.Rows[a]["DateType"].ToString();
                    string cd = mytbl.Rows[a]["FieldLength"].ToString();
                    string sm = mytbl.Rows[a]["FieldRemark"].ToString();
                    string sjkmc = mytbl.Rows[a]["DatabaseName"].ToString();
                    this.Customers.Add(new CustomersItem(bm, zdm,lx,cd,sm,sjkmc));
                }
            }
        }
    }
}
