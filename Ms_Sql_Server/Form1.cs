using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ms_Sql_Server
{
    public partial class Form1 : Form
    {
        public SqlConnection conn = new SqlConnection();
        public Form1()
        {
            InitializeComponent();
        }

        private void ConnectDB()
        {
            conn.ConnectionString = string.Format("Data Source=({0});" +
                "initial Catalog = {1};" +
                "Integrated Security = {2};" +
                "Timeout = 3",
                "local", "MYDB1", "SSPI");
            conn = new SqlConnection(conn.ConnectionString);
            conn.Open();
        }

        private void Query_Select()
        {
            ConnectDB();

            //SQL 명령어 선언
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select * from TB_CUST order by CUST_ID desc";

            //DataAdapter와 DataSet으로 DB table 불러오기
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds, "TB_CUST");     //select * from TB_CUST의 결과가 DataSet인 ds에 들어옴.

            //datagridview에 db에서 가져온 데이터를 입력합니다.
            dataGridView1.DataSource = ds;
            dataGridView1.DataMember = "TB_CUST";
            conn.Close();
        }

        private void button_Select_Click(object sender, EventArgs e)
        {
            Query_Select();
        }

        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                var temp = dataGridView1.CurrentRow;

                textBox_Id.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                textBox_Birth.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            }
            catch (Exception)
            {

            }
        }

        private void Query_Insert()
        {
            try
            {
                ConnectDB();
                string sqlcommand = "Insert into TB_CUST(CUST_ID, BIRTH_DT) values (@parameter1, @parameter2)";
                SqlCommand cmd = new SqlCommand();

                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                //Column 명은 별도의 파라메터 형태를 선언 합니다.
                //SQL Injection을 방지하고자 함
                cmd.Parameters.AddWithValue("@parameter1", textBox_Id.Text);
                cmd.Parameters.AddWithValue("@parameter2", textBox_Birth.Text);
                cmd.CommandText = sqlcommand;
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + Environment.NewLine + e.StackTrace);
            }
        }

        private void button_Insert_Click(object sender, EventArgs e)
        {
            Query_Insert();
            Query_Select();
        }

        private void Query_Update()
        {
            ConnectDB();
            string sqlcommand = "Update TB_CUST set CUST_ID = @p1, BIRTH_DT = @p2 where BIRTH_DT = @p3";
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@p1", textBox_Id.Text);
            cmd.Parameters.AddWithValue("@p2", textBox_Birth.Text);
            cmd.Parameters.AddWithValue("@p3", textBox_Birth.Text);
            cmd.CommandText = sqlcommand;
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        private void button_Update_Click(object sender, EventArgs e)
        {
            Query_Update();
            Query_Select();
        }

        private void Query_Delete()
        {
            ConnectDB();
            string sqlcommand = "Delete TB_CUST where BIRTH_DT = @p1";
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@p1", textBox_Birth.Text);
            cmd.CommandText = sqlcommand;
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        private void button_Delete_Click(object sender, EventArgs e)
        {
            Query_Delete();
            Query_Select();
        }
    }
}
