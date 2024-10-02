using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using РаботаСБД;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class Comment : Form
    {
        private int selectedId;
        private int masterID;
        private SqlConnection connection;
        public Comment(int selectedId, int masterID)
        {
            this.selectedId = selectedId;
            this.masterID = masterID;
            InitializeComponent();
            connection = Class1.connection;
        }
        public Comment(int selectedId)
        {
            this.selectedId = selectedId;
            InitializeComponent();
            connection = Class1.connection;
        }
        private void Comment_Load(object sender, EventArgs e)
        {
            if (Class1.role == "Заказчик" || Class1.role == "Оператор")
            {
                button3.Visible = false;
                button2.Visible = false;
            }
            else
            {
                button3.Visible = true;
                button2.Visible = true;
            }
            label3.Text = $"Комментарий к заявке: {selectedId}";
            connection.Open();
            string checkQuery = "SELECT COUNT(*) FROM [dbo].[Комментарии] WHERE requestID = @requestID";
            SqlCommand checkCommand = connection.CreateCommand();
            checkCommand.CommandText = checkQuery;
            checkCommand.Parameters.AddWithValue("@requestID", selectedId);
            int count = (int)checkCommand.ExecuteScalar();
            if (count > 0)
            {
                string query = "SELECT message FROM [dbo].[Комментарии] WHERE requestID = @requestID";
                SqlCommand command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("@requestID", selectedId);
                var result = command.ExecuteScalar();
                textBox1.Text = result.ToString();
                connection.Close();
                return;
            }
            else
            {
                string query = "INSERT INTO [dbo].[Комментарии] (requestID, masterID) VALUES (@requestID, @masterID)";
                SqlCommand command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("@requestID", selectedId);
                command.Parameters.AddWithValue("@masterID", masterID);
                connection.Close();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            connection.Open();
            string checkQuery = "SELECT commentID FROM [dbo].[Комментарии] WHERE requestID = @requestID";
            SqlCommand checkCommand = connection.CreateCommand();
            checkCommand.CommandText = checkQuery;
            checkCommand.Parameters.AddWithValue("@requestID", selectedId);
            int ID = (int)checkCommand.ExecuteScalar();
            string query = "UPDATE [dbo].[Комментарии] SET message = @message WHERE commentID = @commentID";
            SqlCommand command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@message", textBox1.Text);
            command.Parameters.AddWithValue("@commentID", ID);
            command.ExecuteNonQuery();
            connection.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string checkQuery = "SELECT commentID FROM [dbo].[Комментарии] WHERE requestID = @requestID";
            SqlCommand checkCommand = connection.CreateCommand();
            checkCommand.CommandText = checkQuery;
            checkCommand.Parameters.AddWithValue("@requestID", selectedId);
            int count = (int)checkCommand.ExecuteScalar();
            Class1.DeleteRow("commentID", count, "Комментарии");
        }
    }
}
