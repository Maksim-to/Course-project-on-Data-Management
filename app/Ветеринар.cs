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
    public partial class Ветеринар : Form
    {
        public Ветеринар()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form питомец = new Питомцы();
            питомец.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form прием = new Прием();
            прием.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form услугиприема = new УслугаПриема();
            услугиприема.Show();
        }
    }
}
