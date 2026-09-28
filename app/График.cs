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
    public partial class График : Form
    {
        public График()
        {
            InitializeComponent();
        }

        private void графикBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.графикBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.ветеринарная_клиникаDataSet);

        }

        private void График_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "ветеринарная_клиникаDataSet.График". При необходимости она может быть перемещена или удалена.
            this.графикTableAdapter.Fill(this.ветеринарная_клиникаDataSet.График);
        }
        
       
        private void button1_Click(object sender, EventArgs e)
        {
            int.TryParse(textBox1.Text, out int searchValue); // Проверяем, что введённое значение числовое
            графикBindingSource.Filter = $"Id_Графика = {searchValue}";

        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            графикBindingSource.Filter = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (GlobalData.Password != "1234")
            {
                try
                {
                    // Задание параметров с использованием метода GetDbValue
                    sqlCommandAdd.Parameters["@Понедельник"].Value = GetDbValue(textBox2.Text);
                    sqlCommandAdd.Parameters["@Вторник"].Value = GetDbValue(textBox3.Text);
                    sqlCommandAdd.Parameters["@Среда"].Value = GetDbValue(textBox4.Text);
                    sqlCommandAdd.Parameters["@Четверг"].Value = GetDbValue(textBox5.Text);
                    sqlCommandAdd.Parameters["@Пятница"].Value = GetDbValue(textBox6.Text);
                    sqlCommandAdd.Parameters["@Суббота"].Value = GetDbValue(textBox7.Text);
                    sqlCommandAdd.Parameters["@Воскресенье"].Value = GetDbValue(textBox8.Text);

                    // Выполнение команды
                    sqlConnection.Open();
                    sqlCommandAdd.ExecuteNonQuery();
                    this.графикTableAdapter.Fill(this.ветеринарная_клиникаDataSet.График);
                    MessageBox.Show("График успешно добавлен.");
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
            else MessageBox.Show("В доступе отказано.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private object GetDbValue(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return DBNull.Value; // Пустая строка или null заменяется на DBNull
            }
            else
            {
                return input.Trim(); // Удаляем лишние пробелы
            }
        }
        private void графикDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Проверяем, чтобы не был кликнут заголовок
            if (e.RowIndex >= 0)
            {
                // Получаем данные выбранной строки
                var row = графикDataGridView.Rows[e.RowIndex];

                // Предположим, что у нас есть столбцы с именами "Id_Услуги", "Название"
                textBox9.Text = row.Cells[0].Value.ToString();
                textBox2.Text = row.Cells[1].Value.ToString();
                textBox3.Text = row.Cells[2].Value.ToString();
                textBox4.Text = row.Cells[3].Value.ToString();
                textBox5.Text = row.Cells[4].Value.ToString();
                textBox6.Text = row.Cells[5].Value.ToString();
                textBox7.Text = row.Cells[6].Value.ToString();
                textBox8.Text = row.Cells[7].Value.ToString();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (GlobalData.Password != "1234")
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(textBox9.Text) || textBox9.Text == "0")
                    {
                        MessageBox.Show("Поле Id_Графика не должно быть пустым или равным нулю.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    int idГрафика;
                    if (!int.TryParse(textBox9.Text, out idГрафика))
                    {
                        MessageBox.Show("Id_Графика должно быть числом.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (idГрафика <= 0)
                    {
                        MessageBox.Show("Id_Графика должно быть больше нуля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    // Задание параметров с использованием метода GetDbValue
                    sqlCommandDelete.Parameters["@Id_Графика"].Value = textBox9.Text;
                    sqlCommandDelete.Parameters["@Понедельник"].Value = GetDbValue(textBox2.Text);
                    sqlCommandDelete.Parameters["@Вторник"].Value = GetDbValue(textBox3.Text);
                    sqlCommandDelete.Parameters["@Среда"].Value = GetDbValue(textBox4.Text);
                    sqlCommandDelete.Parameters["@Четверг"].Value = GetDbValue(textBox5.Text);
                    sqlCommandDelete.Parameters["@Пятница"].Value = GetDbValue(textBox6.Text);
                    sqlCommandDelete.Parameters["@Суббота"].Value = GetDbValue(textBox7.Text);
                    sqlCommandDelete.Parameters["@Воскресенье"].Value = GetDbValue(textBox8.Text);

                    sqlConnection.Open();
                    sqlCommandDelete.ExecuteNonQuery();
                    this.графикTableAdapter.Fill(this.ветеринарная_клиникаDataSet.График);
                    MessageBox.Show("График успешно удален.");
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
            else MessageBox.Show("В доступе отказано.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox9.Text) || textBox9.Text == "0")
                {
                    MessageBox.Show("Поле Id_Графика не должно быть пустым или равным нулю.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int idГрафика;
                if (!int.TryParse(textBox9.Text, out idГрафика))
                {
                    MessageBox.Show("Id_Графика должно быть числом.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (idГрафика <= 0)
                {
                    MessageBox.Show("Id_Графика должно быть больше нуля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // Задание параметров с использованием метода GetDbValue
                sqlCommandUpdate.Parameters["@Id_Графика"].Value = textBox9.Text;
                sqlCommandUpdate.Parameters["@Понедельник"].Value = GetDbValue(textBox2.Text);
                sqlCommandUpdate.Parameters["@Вторник"].Value = GetDbValue(textBox3.Text);
                sqlCommandUpdate.Parameters["@Среда"].Value = GetDbValue(textBox4.Text);
                sqlCommandUpdate.Parameters["@Четверг"].Value = GetDbValue(textBox5.Text);
                sqlCommandUpdate.Parameters["@Пятница"].Value = GetDbValue(textBox6.Text);
                sqlCommandUpdate.Parameters["@Суббота"].Value = GetDbValue(textBox7.Text);
                sqlCommandUpdate.Parameters["@Воскресенье"].Value = GetDbValue(textBox8.Text);

                sqlConnection.Open();
                sqlCommandUpdate.ExecuteNonQuery();
                this.графикTableAdapter.Fill(this.ветеринарная_клиникаDataSet.График);
                MessageBox.Show("График успешно обновлен.");
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
