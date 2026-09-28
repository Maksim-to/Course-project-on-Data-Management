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
    public partial class ГенеральныйДиректор : Form
    {
        public ГенеральныйДиректор()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form график = new График();
            график.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form услуги = new Услуги();
            услуги.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form складскаяоперация = new СкладскиеОперации();
            складскаяоперация.Show();
        }
    }
}
