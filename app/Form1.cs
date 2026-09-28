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
    public partial class СтартоваяФорма : Form
    {
        public СтартоваяФорма()
        {
            InitializeComponent();
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            // Проверка, что выбрана роль в ComboBox и введен пароль
            if (string.IsNullOrWhiteSpace(comboBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Пожалуйста, выберите роль и введите пароль.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Проверка для каждой роли
            if ("Генеральный директор" == comboBox1.Text && textBox2.Text == "1234")
            {
                GlobalData.Password = textBox2.Text;
                Form ген_директор = new ГенеральныйДиректор();
                ген_директор.Show();
            }
            else if ("Администратор" == comboBox1.Text && textBox2.Text == "5678")
            {
                GlobalData.Password = textBox2.Text;
                Form администратор = new Администратор();
                администратор.Show();
            }
            else if ("Менеджер по запасам" == comboBox1.Text && textBox2.Text == "9012")
            {
                GlobalData.Password = textBox2.Text;
                Form менеджер_по_запасам = new МенеджерПоЗапасам();
                менеджер_по_запасам.Show();
            }
            else if ("Ветеринар" == comboBox1.Text && textBox2.Text == "3456")
            {
                GlobalData.Password = textBox2.Text;
                Form ветеринар = new Ветеринар();
                ветеринар.Show();
            }
            else
            {
                // Если ни одно из условий не выполнено, выводим сообщение об ошибке
                MessageBox.Show("Неверный пароль!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
