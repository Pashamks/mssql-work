using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MSSQLForCSharp
{
    public partial class Form1 : Form
    {
        private SqlConnection sqlConnection = null;
        private SqlConnection northConnection = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["TestDB"].ConnectionString);

            sqlConnection.Open();

            northConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["NorthDB"].ConnectionString);

            northConnection.Open();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand(
                "INSERT INTO [Students] (Name, Surname, Birthday) VALUES (@Name, @Surname, @Birthday)",
                sqlConnection);
            DateTime date = DateTime.Parse(textBox3.Text);

            command.Parameters.AddWithValue("Name", textBox1.Text);

            command.Parameters.AddWithValue("Surname", textBox1.Text);

            command.Parameters.AddWithValue("Birthday", $"{date.Month}/{date.Day}/{date.Year}");

            MessageBox.Show(command.ExecuteNonQuery().ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter(
                textBox7.Text, northConnection);

            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);

            dataGridView1.DataSource = dataSet.Tables[0];
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();

            SqlDataReader dataReader = null;

            try
            {
                SqlCommand sqlCommand = new SqlCommand(
                    "SELECT ProductName, QuantityPrice, UnitPrice FROM Products",
                    northConnection);

                dataReader = sqlCommand.ExecuteReader();

                ListViewItem item = null;

                while(dataReader.Read())
                {
                    item = new ListViewItem(new string[] { Convert.ToString(dataReader["ProductName"]),
                    Convert.ToString(dataReader["QuantityPrice"]), Convert.ToString(dataReader["UnitPrice"])
                    });

                    listView1.Items.Add(item);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);    
            }
            finally
            {
                if(dataReader!=null && !dataReader.IsClosed)
                {
                    dataReader.Close();
                }
            }
        }
    }
}
