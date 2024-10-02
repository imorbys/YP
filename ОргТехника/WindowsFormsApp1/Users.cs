using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using РаботаСБД;

namespace WindowsFormsApp1
{
    public partial class Users : Form
    {
        private int selectedId;
        private SqlConnection connection;
        int count;
        void LoadQuery()
        {
            Class1.LoadData("SELECT userID AS ID, FIO AS ФИО, phone AS Телефон, login AS Логин, password AS Пароль, role AS Роль FROM [dbo].[Пользователи]", dataGridView1);
            dataGridView1.Columns["ID"].Width = 50;
            dataGridView1.Columns["ФИО"].Width = 200;
            dataGridView1.Columns["Телефон"].Width = 78;
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            count = dataGridView1.RowCount;
            toolStripStatusLabel1.Text = $"Записей: {count}";
        }
        public Users()
        {
            InitializeComponent();
            connection = Class1.connection;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            connection.Open();
            string checkQuery = "SELECT COUNT(*) FROM [dbo].[Пользователи] WHERE FIO = @FIO OR login = @login";
            SqlCommand checkCommand = connection.CreateCommand();
            checkCommand.CommandText = checkQuery;
            checkCommand.Parameters.AddWithValue("@FIO", textBox1.Text);
            checkCommand.Parameters.AddWithValue("@login", textBox2.Text);
            int count = (int)checkCommand.ExecuteScalar();
            if (count > 0)
            {
                MessageBox.Show("Данная пользователь уже существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                connection.Close();
                return;
            }
            string query = "INSERT INTO [dbo].[Пользователи] (FIO, phone, login, password, role) VALUES (@FIO, @phone, @login, @password, @role)";
            SqlCommand command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@FIO", textBox1.Text);
            command.Parameters.AddWithValue("@phone", maskedTextBox1.Text);
            command.Parameters.AddWithValue("@login", textBox2.Text);
            command.Parameters.AddWithValue("@password", textBox3.Text);
            command.Parameters.AddWithValue("@role", comboBox1.SelectedItem.ToString());
            MessageBox.Show("Запись успешно добавлена!", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
            command.ExecuteScalar();
            if (comboBox1.SelectedItem.ToString() == "Мастер")
            {
                string addMaster = "INSERT INTO [dbo].[Мастера] (FIO, phone) VALUES (@FIO, @phone)";
                SqlCommand command1 = connection.CreateCommand();
                command1.CommandText = addMaster;
                command1.Parameters.AddWithValue("@FIO", textBox1.Text);
                command1.Parameters.AddWithValue("@phone", maskedTextBox1.Text);
                command1.ExecuteScalar();
            }
            connection.Close();
            LoadQuery();
            textBox1.Clear();
            maskedTextBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            comboBox1.SelectedIndex = -1;
        }
        private void Detail_Load(object sender, EventArgs e)
        {
            LoadQuery();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                selectedId = Convert.ToInt32(selectedRow.Cells["ID"].Value);
                textBox1.Text = selectedRow.Cells["ФИО"].Value.ToString();
                maskedTextBox1.Text = selectedRow.Cells["Телефон"].Value.ToString();
                textBox2.Text = selectedRow.Cells["Логин"].Value.ToString();
                textBox3.Text = selectedRow.Cells["Пароль"].Value.ToString();
                comboBox1.SelectedIndex = comboBox1.FindStringExact(selectedRow.Cells["Роль"].Value.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string query = "UPDATE [dbo].[Пользователи] SET [FIO] = @FIO, [phone] = @phone, [login] = @login,[password] = @pass, [role] = @role WHERE [userID] = @SelectedId";
            SqlCommand command = connection.CreateCommand();
            command.CommandText = query;
            connection.Open();
            command.Parameters.AddWithValue("@FIO", textBox1.Text);
            command.Parameters.AddWithValue("@phone", maskedTextBox1.Text);
            command.Parameters.AddWithValue("@login", textBox2.Text);
            command.Parameters.AddWithValue("@pass", textBox3.Text);
            command.Parameters.AddWithValue("@role", comboBox1.SelectedItem.ToString());
            command.Parameters.AddWithValue("@SelectedId", selectedId);
            command.ExecuteNonQuery();
            MessageBox.Show("Запись успешно изменена!", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
            connection.Close();
            LoadQuery();
            selectedId = 0;
            textBox1.Clear();
            maskedTextBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            comboBox1.SelectedIndex = -1;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Class1.DeleteRow("userID", selectedId, "Пользователи");
            if (comboBox1.SelectedItem.ToString() == "Мастер")
            {
                connection.Open();
                string query = $"DELETE FROM Мастера WHERE FIO = @FIO";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@FIO", textBox1.Text);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            LoadQuery();
            selectedId = 0;
            textBox1.Clear();
            maskedTextBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            comboBox1.SelectedIndex = -1;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            bool allEmpty = string.IsNullOrEmpty(textBox1.Text) ||
                                     !checkNumber(maskedTextBox1) ||
                                     string.IsNullOrEmpty(textBox2.Text) ||
                                     string.IsNullOrEmpty(textBox3.Text) ||
                                     comboBox1.SelectedIndex == -1;
            button1.Enabled = !allEmpty;
            if (selectedId != 0)
            {
                button2.Enabled = !allEmpty;
                button3.Enabled = !allEmpty;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool allEmpty = string.IsNullOrEmpty(textBox1.Text) ||
                         !checkNumber(maskedTextBox1) ||
                         string.IsNullOrEmpty(textBox2.Text) ||
                         string.IsNullOrEmpty(textBox3.Text) ||
                         comboBox1.SelectedIndex == -1;
            button1.Enabled = !allEmpty;
            if (selectedId != 0)
            {
                button2.Enabled = !allEmpty;
                button3.Enabled = !allEmpty;
            }
        }
        private bool checkNumber(MaskedTextBox mask)
        {
            for (int i = 1; i < mask.Text.Length; i++)
            {
                if (!char.IsDigit(mask.Text[i]))
                {
                    return false;
                }
            }
            return true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Class1.LoadDataWithParameter("SELECT userID AS ID, FIO AS ФИО, phone AS Телефон, login AS Логин, password AS Пароль, role AS Роль FROM [dbo].[Пользователи] WHERE role = @role", dataGridView1, "role", comboBox2.SelectedItem.ToString());
            toolStripStatusLabel1.Text = $"Записей: {dataGridView1.RowCount} из {count}";
        }
        private void button5_Click(object sender, EventArgs e)
        {
            LoadQuery();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            button4.Enabled = true;
        }
    }
}
