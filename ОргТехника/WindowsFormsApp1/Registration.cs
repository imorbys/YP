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
using РаботаСБД;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace WindowsFormsApp1
{
    public partial class Registration : Form
    {
        private SqlConnection connection;
        public Registration()
        {
            InitializeComponent();
            connection = Class1.connection;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string password = textBox3.Text;
            string password2 = textBox4.Text;
            if (password == password2)
            {
                connection.Open();
                string checkQuery = "SELECT COUNT(*) FROM [dbo].[Пользователи] WHERE login = @login OR FIO = @FIO";
                SqlCommand checkCommand = connection.CreateCommand();
                checkCommand.CommandText = checkQuery;
                checkCommand.Parameters.AddWithValue("@FIO", textBox1.Text);
                checkCommand.Parameters.AddWithValue("@login", textBox2.Text);
                int count = (int)checkCommand.ExecuteScalar();
                if (count > 0)
                {
                    MessageBox.Show("Данный пользователь уже существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    connection.Close();
                    return;
                }
                string query = "INSERT INTO [dbo].[Пользователи] (FIO, phone, login, password, role) VALUES (@FIO, @phone, @login, @pass, @role)";
                SqlCommand command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("@FIO", textBox1.Text);
                command.Parameters.AddWithValue("@phone", maskedTextBox1.Text);
                command.Parameters.AddWithValue("@login", textBox2.Text);
                command.Parameters.AddWithValue("@pass", textBox3.Text);
                command.Parameters.AddWithValue("@role", "Заказчик");
                MessageBox.Show("Пользователь добавлен!", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                command.ExecuteScalar();
                connection.Close();
                Close();
            }
            else
            {
                MessageBox.Show("Пароли не совпадают", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            bool allFieldsEmpty = string.IsNullOrEmpty(textBox1.Text) ||
                                      string.IsNullOrEmpty(textBox2.Text) ||
                                      string.IsNullOrEmpty(textBox3.Text) ||
                                      string.IsNullOrEmpty(textBox4.Text) ||
                                      !checkNumber(maskedTextBox1);
            button1.Enabled = !allFieldsEmpty;
        }
        private bool checkNumber(MaskedTextBox mask)
        {
            for (int i=1; i<mask.Text.Length; i++)
            {
                if (!char.IsDigit(mask.Text[i]))
                {
                    return false;
                }
            }
            return true;
        }
        int current = 1;
        int current1 = 1;
        private void button2_Click(object sender, EventArgs e)
        {
            if (current == 1)
            {
                textBox3.PasswordChar = '\0';
                current = 0;
                button2.ForeColor = Color.Green;
            }
            else
            {
                textBox3.PasswordChar = '*';
                current = 1;
                button2.ForeColor = Color.Black;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (current == 1)
            {
                textBox4.PasswordChar = '\0';
                current = 0;
                button3.ForeColor = Color.Green;
            }
            else
            {
                textBox4.PasswordChar = '*';
                current = 1;
                button3.ForeColor = Color.Black;
            }
        }
    }
}
