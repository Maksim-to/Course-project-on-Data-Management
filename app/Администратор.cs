using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Итоговая_проектная_работа
{
    public partial class Администратор : Form
    {
        public Администратор()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form client = new Client();
            client.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form sotrudnik = new Sotrudnik();
            sotrudnik.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form график = new График();
            график.Show();
        }
    }
}
