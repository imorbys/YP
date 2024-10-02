using System;
using System.Drawing;
using System.Windows.Forms;
using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;
using РаботаСБД;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
namespace WindowsFormsApp1
{
    public partial class QRCode : Form
    {
        public QRCode()
        {
            InitializeComponent();
        }

        private void QRCode_Load(object sender, EventArgs e)
        {
            if (Class1.role == "Заказчик")
            {
                label1.Text = "Оцените работу нашего сервиса:";
                string qrtext = "https://forms.gle/Wysdn8k2dsMEtBB47";
                QRCodeEncoder encoder = new QRCodeEncoder();
                Bitmap qrcode = encoder.Encode(qrtext);
                pictureBox1.Image = qrcode as Image;
            }
            else
            {
                label1.Text = "Таблица с отзывами:";
                string qrtext = "https://docs.google.com/spreadsheets/d/1eRz801zaOgKmyIGmEtgviZcarevz-ZSb5szJE8HDu0M/edit?usp=sharing";
                QRCodeEncoder encoder = new QRCodeEncoder();
                Bitmap qrcode = encoder.Encode(qrtext);
                pictureBox1.Image = qrcode as Image;
            }
        }
    }
}
