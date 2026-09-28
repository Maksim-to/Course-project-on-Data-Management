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
    public partial class Медикамент : Form
    {
        public Медикамент()
        {
            InitializeComponent();
        }

        private void медикаментBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.медикаментBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.ветеринарная_клиникаDataSet);

        }

        private void Медикамент_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "ветеринарная_клиникаDataSet.Медикамент". При необходимости она может быть перемещена или удалена.
            this.медикаментTableAdapter.Fill(this.ветеринарная_клиникаDataSet.Медикамент);
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;

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
                case 4:
                    Col = dataGridViewTextBoxColumn6;
                    break;
            }
            if (radioButton1.Checked)
                медикаментDataGridView.Sort(Col, System.ComponentModel.ListSortDirection.Ascending);
            else
                медикаментDataGridView.Sort(Col, System.ComponentModel.ListSortDirection.Descending);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string searchText = textBox6.Text.Replace("'", "''"); // Экранируем одинарные кавычки
            медикаментBindingSource.Filter = $"Наименование LIKE '%{searchText}%'";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            медикаментBindingSource.Filter = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Проверка на пустоту полей
                if (string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    MessageBox.Show("Поле 'Наименование' не может быть пустым.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Прерываем выполнение, если поле пустое
                }
                if (string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    MessageBox.Show("Поле 'Стоимость' не может быть пустым.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Прерываем выполнение, если поле пустое
                }
                if (string.IsNullOrWhiteSpace(textBox3.Text))
                {
                    MessageBox.Show("Поле 'СрокГодности' не может быть пустым.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Прерываем выполнение, если поле пустое
                }
                if (string.IsNullOrWhiteSpace(textBox4.Text))
                {
                    MessageBox.Show("Поле 'Количество' не может быть пустым.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Прерываем выполнение, если поле пустое
                }
                if (string.IsNullOrWhiteSpace(comboBox1.Text))
                {
                    MessageBox.Show("Поле 'ЕдиницаИзмерения' не может быть пустым.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Прерываем выполнение, если поле пустое
                }

                // Проверка на правильность ввода стоимости, срока годности и количества
                if (!decimal.TryParse(textBox2.Text, out decimal стоимость))
                {
                    MessageBox.Show("Введите корректную числовую стоимость.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Прерываем выполнение, если значение не числовое
                }
                if (!int.TryParse(textBox3.Text, out int срокГодности))
                {
                    MessageBox.Show("Введите корректный срок годности (целое число).", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Прерываем выполнение, если значение не числовое
                }
                if (!int.TryParse(textBox4.Text, out int количество))
                {
                    MessageBox.Show("Введите корректное количество (целое число).", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Прерываем выполнение, если значение не числовое
                }

                // Проверка на корректность данных
                if (стоимость <= 0)
                {
                    MessageBox.Show("Стоимость медикамента должна быть больше 0.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (срокГодности <= 0)
                {
                    MessageBox.Show("Срок годности медикамента должен быть больше 0.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (количество < 0)
                {
                    MessageBox.Show("Количество медикамента не может быть отрицательным.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Проверка на корректность единицы измерения
                if (comboBox1.SelectedItem == null || !new List<string> { "шт", "гр", "уп", "амп", "флак" }.Contains(comboBox1.SelectedItem.ToString()))
                {
                    MessageBox.Show("Некорректная единица измерения. Допустимые значения: шт, гр, уп, амп, флак.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Присвоить значения входным параметрам процедуры с преобразованием типов
                sqlCommand.Parameters["@Наименование"].Value = textBox1.Text;
                sqlCommand.Parameters["@Стоимость"].Value = стоимость;  // Используем decimal для стоимости
                sqlCommand.Parameters["@СрокГодности"].Value = срокГодности;  // Используем int для срока годности
                sqlCommand.Parameters["@Количество"].Value = количество;  // Используем int для количества
                sqlCommand.Parameters["@ЕдиницаИзмерения"].Value = comboBox1.SelectedItem.ToString(); // Используем SelectedItem для ComboBox

                // Открыть соединение с БД
                sqlConnection.Open();

                // Выполнить sql-выражение (хранимую процедуру)
                sqlCommand.ExecuteNonQuery();
                this.медикаментTableAdapter.Fill(this.ветеринарная_клиникаDataSet.Медикамент);
                // Уведомить пользователя об успешном добавлении медикамента
                MessageBox.Show("Медикамент успешно добавлен.");
            }
            catch (SqlException ex)
            {
                // Обработка ошибок: показать сообщение пользователю
                MessageBox.Show($"Ошибка при добавлении медикамента. {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Закрыть соединение с БД
                if (sqlConnection.State == System.Data.ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }
        }




        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // Проверка на пустоту полей
                if (string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    MessageBox.Show("Поле 'Наименование' не может быть пустым.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Прерываем выполнение, если поле пустое
                }
                if (string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    MessageBox.Show("Поле 'Стоимость' не может быть пустым.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Прерываем выполнение, если поле пустое
                }
                if (string.IsNullOrWhiteSpace(textBox3.Text))
                {
                    MessageBox.Show("Поле 'СрокГодности' не может быть пустым.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Прерываем выполнение, если поле пустое
                }
                if (string.IsNullOrWhiteSpace(textBox4.Text))
                {
                    MessageBox.Show("Поле 'Количество' не может быть пустым.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Прерываем выполнение, если поле пустое
                }
                if (string.IsNullOrWhiteSpace(comboBox1.Text))
                {
                    MessageBox.Show("Поле 'ЕдиницаИзмерения' не может быть пустым.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Прерываем выполнение, если поле пустое
                }

                // Проверка на правильность ввода стоимости, срока годности и количества
                if (!decimal.TryParse(textBox2.Text, out decimal стоимость))
                {
                    MessageBox.Show("Введите корректную числовую стоимость.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Прерываем выполнение, если значение не числовое
                }
                if (!int.TryParse(textBox3.Text, out int срокГодности))
                {
                    MessageBox.Show("Введите корректный срок годности (целое число).", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Прерываем выполнение, если значение не числовое
                }
                if (!int.TryParse(textBox4.Text, out int количество))
                {
                    MessageBox.Show("Введите корректное количество (целое число).", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Прерываем выполнение, если значение не числовое
                }

                // Проверка на корректность данных
                if (стоимость <= 0)
                {
                    MessageBox.Show("Стоимость медикамента должна быть больше 0.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (срокГодности <= 0)
                {
                    MessageBox.Show("Срок годности медикамента должен быть больше 0.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (количество < 0)
                {
                    MessageBox.Show("Количество медикамента не может быть отрицательным.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Проверка на корректность единицы измерения
                if (comboBox1.SelectedItem == null || !new List<string> { "шт", "гр", "уп", "амп", "флак" }.Contains(comboBox1.SelectedItem.ToString()))
                {
                    MessageBox.Show("Некорректная единица измерения. Допустимые значения: шт, гр, уп, амп, флак.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Присвоить значения входным параметрам процедуры с преобразованием типов
                sqlCommandUpdate.Parameters["@Наименование"].Value = textBox1.Text;
                sqlCommandUpdate.Parameters["@Стоимость"].Value = стоимость;  // Используем decimal для стоимости
                sqlCommandUpdate.Parameters["@СрокГодности"].Value = срокГодности;  // Используем int для срока годности
                sqlCommandUpdate.Parameters["@Количество"].Value = количество;  // Используем int для количества
                sqlCommandUpdate.Parameters["@ЕдиницаИзмерения"].Value = comboBox1.SelectedItem.ToString(); // Используем SelectedItem для ComboBox

                // Открыть соединение с БД
                sqlConnection.Open();

                // Выполнить sql-выражение (хранимую процедуру) для обновления данных
                sqlCommandUpdate.ExecuteNonQuery();
                this.медикаментTableAdapter.Fill(this.ветеринарная_клиникаDataSet.Медикамент);
                // Уведомить пользователя об успешном обновлении
                MessageBox.Show("Данные медикамента успешно обновлены.");
            }
            catch (SqlException ex)
            {
                // Обработка ошибок: показать сообщение пользователю
                MessageBox.Show($"Ошибка при обновлении медикамента. {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Закрыть соединение с БД
                if (sqlConnection.State == System.Data.ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }
        }




        private void медикаментDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Проверяем, чтобы не был кликнут заголовок
            if (e.RowIndex >= 0)
            {
                // Получаем данные выбранной строки
                var row = медикаментDataGridView.Rows[e.RowIndex];

                // Предположим, что у нас есть столбцы с именами "Id_Услуги", "Название"
                textBox1.Text = row.Cells[1].Value.ToString();
                textBox2.Text = row.Cells[2].Value.ToString();
                textBox3.Text = row.Cells[3].Value.ToString();
                textBox4.Text = row.Cells[4].Value.ToString();
                comboBox1.Text = row.Cells[5].Value.ToString();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}
