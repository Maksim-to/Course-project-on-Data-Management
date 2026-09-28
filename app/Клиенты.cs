using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace Итоговая_проектная_работа
{
    public partial class Client : Form
    {
        public Client()
        {
            InitializeComponent();
        }

        private void клиентBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.клиентBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.ветеринарная_клиникаDataSet);

        }

        private void Клиенты_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "ветеринарная_клиникаDataSet.Клиент". При необходимости она может быть перемещена или удалена.
            this.клиентTableAdapter.Fill(this.ветеринарная_клиникаDataSet.Клиент);

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.DataGridViewColumn Col = default(System.Windows.Forms.DataGridViewColumn);
            switch (listBox1.SelectedIndex)
            {
                case 0:
                    Col = dataGridViewTextBoxColumn2;
                    break;
                case 1:
                    Col = dataGridViewTextBoxColumn3;
                    break;
                case 2:
                    Col = dataGridViewTextBoxColumn4;
                    break;
                case 3:
                    Col = dataGridViewTextBoxColumn5;
                    break;
                //case 4:
                  //  Col = dataGridViewTextBoxColumn6;
            }
            if (radioButton1.Checked)
                клиентDataGridView.Sort(Col, System.ComponentModel.ListSortDirection.Ascending);
            else
                клиентDataGridView.Sort(Col, System.ComponentModel.ListSortDirection.Descending);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string searchText = textBox1.Text.Replace("'", "''"); // Экранируем одинарные кавычки
            клиентBindingSource.Filter = $"ФИО LIKE '%{searchText}%'"; // Используем интерполяцию строк и правильное расположение %
            //клиентBindingSource.Filter = "ФИО Like N'" + textBox1.Text + "%'";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            клиентBindingSource.Filter = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                // Проверяем, что все обязательные поля заполнены
                if (string.IsNullOrWhiteSpace(textBox2.Text))
                    throw new ArgumentException("ФИО клиента не может быть пустым.");
                if (string.IsNullOrWhiteSpace(textBox3.Text))
                    throw new ArgumentException("Контактный телефон не может быть пустым.");
                if (string.IsNullOrWhiteSpace(textBox4.Text))
                    throw new ArgumentException("Email не может быть пустым.");
                if (string.IsNullOrWhiteSpace(textBox5.Text))
                    throw new ArgumentException("Адрес не может быть пустым.");

                // Присваиваем значения параметрам
                sqlCommand.Parameters["@ФИО"].Value = textBox2.Text.Trim();
                sqlCommand.Parameters["@Контактный_телефон"].Value = textBox3.Text.Trim();
                sqlCommand.Parameters["@Email"].Value = textBox4.Text.Trim();
                sqlCommand.Parameters["@Адрес"].Value = textBox5.Text.Trim();

                // Открываем соединение с БД
                sqlConnection.Open();

                // Выполняем хранимую процедуру
                sqlCommand.ExecuteNonQuery();
                this.клиентTableAdapter.Fill(this.ветеринарная_клиникаDataSet.Клиент);
                // Уведомляем пользователя об успешном добавлении
                MessageBox.Show("Клиент успешно добавлен.");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlConnection.State == ConnectionState.Open)
                    sqlConnection.Close();
            }
        }



        private void клиентDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Проверяем, чтобы не был кликнут заголовок
            if (e.RowIndex >= 0)
            {
                // Получаем данные выбранной строки
                var row = клиентDataGridView.Rows[e.RowIndex];

                // Предположим, что у нас есть столбцы с именами "Id_Услуги", "Название"
                textBox2.Text = row.Cells[1].Value.ToString();
                textBox3.Text = row.Cells[2].Value.ToString();
                textBox4.Text = row.Cells[3].Value.ToString();
                textBox5.Text = row.Cells[4].Value.ToString();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox2.Text))
                    throw new ArgumentException("ФИО клиента не может быть пустым.");
                if (string.IsNullOrWhiteSpace(textBox3.Text))
                    throw new ArgumentException("Контактный телефон не может быть пустым.");
                if (string.IsNullOrWhiteSpace(textBox4.Text))
                    throw new ArgumentException("Email не может быть пустым.");
                if (string.IsNullOrWhiteSpace(textBox5.Text))
                    throw new ArgumentException("Адрес не может быть пустым.");

                sqlCommandUpdate.Parameters["@ФИО"].Value = textBox2.Text.Trim();
                sqlCommandUpdate.Parameters["@Контактный_телефон"].Value = textBox3.Text.Trim();
                sqlCommandUpdate.Parameters["@Email"].Value = textBox4.Text.Trim();
                sqlCommandUpdate.Parameters["@Адрес"].Value = textBox5.Text.Trim();

                sqlConnection.Open();
                sqlCommandUpdate.ExecuteNonQuery();
                this.клиентTableAdapter.Fill(this.ветеринарная_клиникаDataSet.Клиент);
                MessageBox.Show("Данные клиента успешно обновлены.");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlConnection.State == ConnectionState.Open)
                    sqlConnection.Close();
            }
        }

    }
}
