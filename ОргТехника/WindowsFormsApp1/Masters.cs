using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using РаботаСБД;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class Masters : Form
    {
        private int selectedId;
        private SqlConnection connection;
        void LoadQuery()
        {
            Class1.LoadData("SELECT masterID AS ID, FIO AS ФИО, phone AS Телефон FROM [dbo].[Мастера]", dataGridView1);
            toolStripStatusLabel1.Text = $"Записей: {dataGridView1.RowCount}";
        }
        public Masters()
        {
            InitializeComponent();
            connection = Class1.connection;
        }
        private void Masters_Load(object sender, EventArgs e)
        {
            LoadQuery();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string query = "UPDATE [dbo].[Мастера] SET [FIO] = @FIO, [phone] = @phone WHERE [masterID] = @SelectedId";
            SqlCommand command = connection.CreateCommand();
            command.CommandText = query;
            connection.Open();
            command.Parameters.AddWithValue("@FIO", textBox1.Text);
            command.Parameters.AddWithValue("@phone", maskedTextBox1.Text);
            command.Parameters.AddWithValue("@SelectedId", selectedId);
            command.ExecuteNonQuery();
            MessageBox.Show("Запись успешно изменена!", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
            connection.Close();
            LoadQuery();
            selectedId = 0;
            textBox1.Clear();
            maskedTextBox1.Clear();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Class1.DeleteRow("masterID", selectedId, "Мастера");
            connection.Open();
            string query = $"DELETE FROM Пользователи WHERE FIO = @FIO";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@FIO", textBox1.Text);
            cmd.ExecuteNonQuery();
            connection.Close();
            LoadQuery();
            selectedId = 0;
            textBox1.Clear();
            maskedTextBox1.Clear();
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                selectedId = Convert.ToInt32(selectedRow.Cells["ID"].Value);
                textBox1.Text = selectedRow.Cells["ФИО"].Value.ToString();
                maskedTextBox1.Text = selectedRow.Cells["Телефон"].Value.ToString();
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            bool allFieldsEmpty = string.IsNullOrEmpty(textBox1.Text) ||
                        !checkNumber(maskedTextBox1);
            button1.Enabled = !allFieldsEmpty;
            if (selectedId != 0)
            {
                button2.Enabled = !allFieldsEmpty;
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
    }
}
