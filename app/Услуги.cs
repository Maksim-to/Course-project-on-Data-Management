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
    public partial class Услуги : Form
    {
        public Услуги()
        {
            InitializeComponent();
        }

        private void услугаBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.услугаBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.ветеринарная_клиникаDataSet);

        }

        private void Услуги_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "ветеринарная_клиникаDataSet.Услуга". При необходимости она может быть перемещена или удалена.
            this.услугаTableAdapter.Fill(this.ветеринарная_клиникаDataSet.Услуга);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string searchText = textBox1.Text.Replace("'", "''"); // Экранируем одинарные кавычки
            услугаBindingSource.Filter = $"Название LIKE '%{searchText}%'";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            услугаBindingSource.Filter = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try 
            {
                if (string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    MessageBox.Show("Поле 'Название' не должно быть пустым.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                sqlCommand.Parameters["@Название"].Value = textBox2.Text;

                // Открыть соединение с БД
                sqlConnection.Open();

                // Выполнить sql-выражение (хранимую процедуру) и вернуть количество измененных записей
                sqlCommand.ExecuteNonQuery();
                this.услугаTableAdapter.Fill(this.ветеринарная_клиникаDataSet.Услуга);
                // Уведомить пользователя об успешном добавлении
                MessageBox.Show("Услуга успешно добавлена.");
            }
            catch (SqlException ex)
            {
                // Вывод ошибок из хранимой процедуры
                MessageBox.Show($"Ошибка при добавлении услуги: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }
}
