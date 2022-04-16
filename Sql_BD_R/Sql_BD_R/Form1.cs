using System;
using System.Data;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace Sql_BD_R
{
    public partial class Form1 : Form
    {
        //с помощью этого класса мы будем проводить всю работу с базой данных
        private SqlConnection sqlConnection = null;
        private SqlConnection SqlConnection = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //по ключу получаем нашу строку с бд
            //открываем подключение к бд
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["TestDB"].ConnectionString);
            sqlConnection.Open();
            
            SqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["NorthwinDB"].ConnectionString);
            SqlConnection.Open();

            //подключаем адаптер , чтобы заполнить данными базу данных
            //1 аргумент - это запрос к бд 2 - подключение к бд
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("SELECT * FROM Products", SqlConnection);
            //создаем связь между таблицой
            DataSet db = new DataSet();
            // добавляет или обновляет строки 
            sqlDataAdapter.Fill(db);
            //установим таблицу , которая создалась в dataset
            //получаем через tables[0] единственную таблицу
            dataGridView2.DataSource = db.Tables[0];
           
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            SqlCommand sqlCommand = new SqlCommand(
               $"INSERT INTO [Student] (Name, Surname, Birthday, Mesto_rozdenia, Phone,Email) Values (@Name, @Surname, @Birthday, @Mesto_rozdenia, @Phone, @Email)",
               sqlConnection);
            DateTime date = DateTime.Parse(textBox3.Text);

            sqlCommand.Parameters.AddWithValue("Name", textBox1.Text);
            sqlCommand.Parameters.AddWithValue("Surname", textBox2.Text);
            sqlCommand.Parameters.AddWithValue("Birthday", $"{date.Month}/{date.Day}/{date.Year}");
            sqlCommand.Parameters.AddWithValue("Mesto_rozdenia", textBox4.Text);
            sqlCommand.Parameters.AddWithValue("Phone", textBox5.Text);
            sqlCommand.Parameters.AddWithValue("Email", textBox6.Text);
            MessageBox.Show(sqlCommand.ExecuteNonQuery().ToString());
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(
               textBox7.Text,
               SqlConnection);

            DataSet dataSet = new DataSet();

            sqlDataAdapter.Fill(dataSet);
            dataGridView1.DataSource = dataSet.Tables[0];
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            //пропишем код, который будет филтровать по новому запосу данные 
            //в datasource привязана таблица 
            (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = $"ProductName LIKE '% {textBox8.Text}%'";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(comboBox1.SelectedIndex)
            {
                case 0 :
                    (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = $"UnitsINStock <=10";
                    break;
                case 1:
                    (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = $"UnitsINStock >=10 AND UnitsINStock <= 50 ";
                    break;
                case 2:
                    (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = $"UnitsINStock >=50";
                    break;
                case 3:
                    (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = " ";
                    break;

            }
        }
    }
}
