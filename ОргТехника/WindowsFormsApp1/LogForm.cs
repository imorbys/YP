using System;
using System.Windows.Forms;
using РаботаСБД;

namespace WindowsFormsApp1
{
    public partial class LogForm : Form
    {
        public LogForm()
        {
            InitializeComponent();
        }
        private void LogForm_Load(object sender, EventArgs e)
        {
            Class1.LoadDataLog("SELECT dateTime AS [Время попытки], login AS Логин, isSuccessful AS [Успешно/Не успешно] FROM [dbo].[Логи]", dataGridView1);
            dataGridView1.Columns["Успешно/Не успешно"].Width = 133;
        }
    }
}
