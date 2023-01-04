namespace Database
{
    using Stimulsoft.Report;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;

    /// <summary>
    /// Defines the <see cref="Form1" />.
    /// </summary>
    public partial class Form1 : System.Windows.Forms.Form
    {
        public static DataTable dt2;

        public static List<ConnStrList> list = new List<ConnStrList>();

        private string path = AppDomain.CurrentDomain.BaseDirectory + "\\ConnStr";

        /// <summary>
        /// 当前所选连接串名称
        /// </summary>
        private string CurrentFileName = "";

        /// <summary>
        /// 当前所选连接串原名称
        /// </summary>
        private string CurrentFileOldName = "";

        private bool isEdit = false;

        /// <summary>
        /// 所选的连接串
        /// </summary>
        public string CurrentConnStr = "";

        public Form1()
        {
            InitializeComponent();
            GetConnStrList();
            TextEnable(false);
            button3.Enabled = false;
        }

        private void TextEnable(bool b)
        {
            host.Enabled = b;
            database.Enabled = b;
            user.Enabled = b;
            password.Enabled = b;
            name.Enabled = b;
        }

        private void TextClear()
        {
            host.Text = "";
            database.Text = "";
            user.Text = "";
            password.Text = "";
            name.Text = "";
        }

        private void LeftEnanle(bool b)
        {
            button4.Enabled = b;
            button5.Enabled = b;
            cbList.Enabled = b;
        }

        /// <summary>
        /// 获取连接串列表
        /// </summary>
        private void GetConnStrList()
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            FileSystemInfo[] files = dir.GetFiles();
            list.Clear();
            foreach (var i in files)
            {
                foreach (string str in System.IO.File.ReadAllLines(path + "\\" + i.Name, Encoding.GetEncoding("UTF-8")))
                {
                    string[] sArray = str.Split(',');
                    ConnStrList conn = new ConnStrList();
                    conn.Name = sArray[0];
                    conn.Host = sArray[1];
                    conn.DataBase = sArray[2];
                    conn.User = sArray[3];
                    conn.Password = sArray[4];
                    conn.ConnStr = "Data Source=" + conn.Host + ";initial catalog=" + conn.DataBase + ";User ID=" + conn.User + ";password=" + conn.Password + ";MultipleActiveResultSets=true";
                    list.Add(conn);
                }
            }
            cbList.DataSource = null;
            cbList.DataSource = list;
            cbList.ValueMember = "ConnStr";
            cbList.DisplayMember = "Name";
            cbList.SelectedIndex = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //报表去水印
            Stimulsoft.Base.StiLicense.LoadFromString("6vJhGtLLLz2GNviWmUTrhSqnOItdDwjBylQzQcAOiHkuEo2j1jqd65Skf0cmCinQ4JzbIg9T5ERKiDqTEsSDZtP5F7tOZV7r3o7QFppRVxbP1QzeTbB6XXDDd7uDkfLfClYuPKpcM84MNYrTUgoKodimzBOJxUroH1kfSteDspjtAXcBcX61hKGdw5lo/2XdURnBwPDCWhRNFq1DU2FgvEqP97mpjnmbswIITOk03476qb5IwdRj/jkenRqpzDTEfOmxODfKn93vi6a/aJ6qjgEmOnEtD4NU0Z5GueLBUk5ni8NRyCnRIm6TgErvI07Y/ZRl5Top4PviqfxRRsNe3F4trHrQ7uVUxWsAB5xAmj/XTfWK5GcjlllYe2azjrFVj6wKs5rXs5O1FwjQ6echI675yL7gQFg99DP4P/AJHlxqW68Y9F/FsB+Rqh8IlDz/PUkYb+8Aq+2/+Di+9RKxMC42G4uwTu40Daa3sFtaf4z+nm3m36UaOqvnNrijQ2pisJoSAQDzk7LSVUcVDQhkztpQttyxqPahHd5R+8pS3JJ/0LvNIX3+E4VBe10IOPi2vFsyWqZN23NIZBgVze19VzBuiVqIFFKqdl5fGjxf2Lq+cdSCXZDaUm5qncH4vzvueHlbuJYPKmkcz5d+FcnABExBZnF6MQzl8EOpxqlK3xs=");
        }

        //SQL
        private static DataTable ConvertDataReaderToDataTable(SqlDataReader dataReader)
        {
            ///定义DataTable
            DataTable datatable = new DataTable();
            try
            {    ///动态添加表的数据列
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    DataColumn myDataColumn = new DataColumn();
                    myDataColumn.DataType = dataReader.GetFieldType(i);
                    myDataColumn.ColumnName = dataReader.GetName(i);
                    datatable.Columns.Add(myDataColumn);
                }
                ///添加表的数据
                while (dataReader.Read())
                {
                    DataRow myDataRow = datatable.NewRow();
                    for (int i = 0; i < dataReader.FieldCount; i++)
                    {
                        myDataRow[i] = dataReader[i].ToString();
                    }
                    datatable.Rows.Add(myDataRow);
                    myDataRow = null;
                }
                ///关闭数据读取器
                dataReader.Close();
                return datatable;
            }
            catch (Exception ex)
            {
                ///抛出类型转换错误
                //SystemError.CreateErrorLog(ex.Message);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 打印数据库结构
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [Obsolete]
        private void button1_Click(object sender, EventArgs e)
        {
            //测试连接
            if (!TestConnection(CurrentConnStr))
            {
                MessageBox.Show("连接失败！请检查连接串！");
                return;
            }
            SqlConnection conn = new SqlConnection(CurrentConnStr);//实例连接对象
            conn.Open();//打开数据库连接
            conn.Close();
            //循环获取表里的字段
            conn.Open();
            SqlCommand command = conn.CreateCommand();//通过连接对象创建数据库命令对象
                                                      //获取指定表的结构(表名 数据类型 长度 说明)
            string tablesql = "select obj.name as tableName,c.name as FieldName,t.name as DateType,isnull(c.prec,'') as FieldLength,isnull(PFD.[value],'') as FieldRemark,(Select Name From Master..SysDataBases Where DbId=(Select Dbid From Master..SysProcesses Where Spid = @@spid)) as DatabaseName from sysobjects obj inner join syscolumns c on c.id=obj.id and obj.type='U' inner join systypes t on c.xusertype=t.xusertype left outer join syscomments m on c.cdefault=m.id left outer join sys.extended_properties tableRemark on tableRemark.class=1 and obj.id=tableRemark.major_id and tableRemark.minor_id=0 and tableRemark.name='MS_Description' LEFT outer JOIN sys.extended_properties PFD ON PFD.class=1 AND c.[id]=PFD.major_id AND c.colid=PFD.minor_id and PFD.name = 'MS_Description' left outer join (select IDXC.[object_id],IDXC.column_id,Sort=CASE INDEXKEY_PROPERTY(IDXC.[object_id],IDXC.index_id,IDXC.index_column_id,'IsDescending')WHEN 1 THEN 'DESC' WHEN 0 THEN 'ASC' ELSE '' END,PrimaryKey=CASE WHEN IDX.is_primary_key=1 THEN 1 ELSE 0 END,IndexName=IDX.Name from sys.indexes IDX INNER JOIN sys.index_columns IDXC ON IDX.[object_id]=IDXC.[object_id] AND IDX.index_id=IDXC.index_id LEFT outer JOIN sys.key_constraints KC ON IDX.[object_id]=KC.[parent_object_id] AND IDX.index_id=KC.unique_index_id INNER JOIN (SELECT [object_id], Column_id, index_id=MIN(index_id) FROM sys.index_columns GROUP BY [object_id], Column_id ) IDXCUQ ON IDXC.[object_id]=IDXCUQ.[object_id] AND IDXC.Column_id=IDXCUQ.Column_id AND IDXC.index_id=IDXCUQ.index_id) IDX ON C.[id]=IDX.[object_id] AND C.colid=IDX.column_id order by obj.name";
            command.CommandText = tablesql;          //确定文本对象执行的SQL语句
            SqlDataReader dataReader = command.ExecuteReader();
            dt2 = ConvertDataReaderToDataTable(dataReader);
            conn.Close();
            //导出
            var stream2 = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("Database.database.mrt");
            StiReport report2 = new StiReport();
            report2.Load(stream2);
            stream2.Close();
            stream2.Dispose();
            report2.CalculationMode = StiCalculationMode.Interpretation;
            report2.RegBusinessObject("Date", "Date", new Class1());
            report2.Render();
            report2.Show(true);
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 修改按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click_1(object sender, EventArgs e)
        {
            TextEnable(true);
            button2.Enabled = false;
            button3.Enabled = true;
            LeftEnanle(false);
            isEdit = true;
        }

        /// <summary>
        /// 保存修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            if (
                string.IsNullOrEmpty(name.Text) ||
                string.IsNullOrEmpty(host.Text) ||
                string.IsNullOrEmpty(database.Text) ||
                string.IsNullOrEmpty(user.Text) ||
                string.IsNullOrEmpty(password.Text)
                )
            {
                MessageBox.Show("所有项都为必填项！请检查！");
                return;
            }
            if (!TestConnection(CurrentConnStr))
            {
                MessageBox.Show("连接失败！请检查连接串！");
                return;
            }

            //文件路径
            string filePath = "";


            TextEnable(false);
            button2.Enabled = true;

            //保存
            if (isEdit)
            {
                //修改
                filePath = path + "\\" + CurrentFileName + ".txt";
                StreamWriter sw = new StreamWriter(filePath, false);
                sw.WriteLine(name.Text + "," + host.Text + "," + database.Text + "," + user.Text + "," + password.Text);
                sw.Close();
                if (CurrentFileName != name.Text)
                {
                    //修改了名称  需要重命名
                    string newPath = path + "\\" + name.Text + ".txt";
                    File.Move(filePath, newPath);
                    File.Delete(filePath);
                }
                GetConnStrList();
                button3.Enabled = false;
            }
            else
            {
                //新建
                filePath = path + "\\" + name.Text + ".txt";
                //判断文件是否存在
                if (File.Exists(filePath))
                {
                    MessageBox.Show("连接串【" + CurrentFileName + "】已存在！");
                    return;
                }
                //新建文件
                StreamWriter sw = new StreamWriter(filePath, false);
                sw.WriteLine(name.Text + "," + host.Text + "," + database.Text + "," + user.Text + "," + password.Text);
                sw.Close();
                GetConnStrList();
                button3.Enabled = false;
            }
            LeftEnanle(true);
            isEdit = false;
        }

        /// <summary>
        /// 测试连接串
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        private bool TestConnection(string connStr)
        {
            SqlConnection sqlConnection = new SqlConnection(CurrentConnStr);
            sqlConnection.Open();
            if (sqlConnection.State != ConnectionState.Open)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 新建连接串
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            TextEnable(true);
            TextClear();
            button2.Enabled = false;

            button3.Enabled = true;
            LeftEnanle(false);
        }

        /// <summary>
        /// 切换下拉框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbList.Text == "Database.ConnStrList")
            {
                CurrentFileName = list.Select(c => c.Name).First();
            }
            else
            {
                CurrentFileName = cbList.Text;
            }
            foreach (var i in list)
            {
                if (i.Name == CurrentFileName)
                {
                    name.Text = i.Name;
                    host.Text = i.Host;
                    database.Text = i.DataBase;
                    user.Text = i.User;
                    password.Text = i.Password;
                    CurrentConnStr = i.ConnStr;
                    CurrentFileOldName = i.Name;
                    break;
                }
            }
        }

        /// <summary>
        /// 删除连接串文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要删除所选的连接串吗？", "操作提示", MessageBoxButtons.OKCancel);
            if (dr == DialogResult.OK)
            {
                //如果点击“确定”按钮
                File.Delete(path + "\\" + CurrentFileName + ".txt");
                GetConnStrList();
            }
        }
    }
}
