using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using РаботаСБД;

namespace WindowsFormsApp1
{
    public partial class Detail : Form
    {
        private int selectedId;
        private SqlConnection connection;
        void LoadQuery()
        {
            Class1.LoadData("SELECT detailID AS ID, detailName AS [Название детали], detailCost AS Стоимость FROM [dbo].[Детали]", dataGridView1);
            dataGridView1.Columns["ID"].Width = 50;
            dataGridView1.Columns["Название детали"].Width = 220;
            dataGridView1.Columns["Стоимость"].Width = 105;
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            toolStripStatusLabel1.Text = $"Записей: {dataGridView1.RowCount}";
        }
        public Detail()
        {
            InitializeComponent();
            connection = Class1.connection;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            connection.Open();
            string checkQuery = "SELECT COUNT(*) FROM [dbo].[Детали] WHERE detailName = @Name";
            SqlCommand checkCommand = connection.CreateCommand();
            checkCommand.CommandText = checkQuery;
            checkCommand.Parameters.AddWithValue("@Name", textBox1.Text);
            int count = (int)checkCommand.ExecuteScalar();
            if (count > 0)
            {
                MessageBox.Show("Данная деталь уже существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                connection.Close();
                return;
            }
            string query = "INSERT INTO [dbo].[Детали] (detailName, detailCost) VALUES (@Name, @Cost)";
            SqlCommand command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@Name", textBox1.Text);
            command.Parameters.AddWithValue("@Cost", numericUpDown1.Value);
            MessageBox.Show("Запись успешно добавлена!", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
            command.ExecuteScalar();
            connection.Close();
            textBox1.Clear();
            numericUpDown1.Value = 0;
            LoadQuery();
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
                textBox1.Text = selectedRow.Cells["Название детали"].Value.ToString();
                numericUpDown1.Value = Convert.ToDecimal(selectedRow.Cells["Стоимость"].Value);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            bool allEmpty = string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(numericUpDown1.Text);
            button1.Enabled = !allEmpty;
            if (selectedId != 0)
            {
                button2.Enabled = !allEmpty;
                button3.Enabled = !allEmpty;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string query = "UPDATE [dbo].[Детали] SET [detailName] = @Name, [detailCost] = @Cost WHERE [detailID] = @SelectedId";
            SqlCommand command = connection.CreateCommand();
            command.CommandText = query;
            connection.Open();
            command.Parameters.AddWithValue("@Name", textBox1.Text);
            command.Parameters.AddWithValue("@Cost", numericUpDown1.Value);
            command.Parameters.AddWithValue("@SelectedId", selectedId);
            command.ExecuteNonQuery();
            MessageBox.Show("Запись успешно изменена!", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
            connection.Close();
            LoadQuery();
            selectedId = 0;
            textBox1.Clear();
            numericUpDown1.Value = 0;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Class1.DeleteRow("detailID", selectedId, "Детали");
            LoadQuery();
            selectedId = 0;
            textBox1.Clear();
            numericUpDown1.Value = 0;
        }
    }
}
