using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace DesignFormLogin
{
    public partial class Form2 : Form
    {
        MySqlConnection conn = conncectionService.getConnection();
        DataTable dataTable = new DataTable();



        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            filldataTable();


        }

        public DataTable getDataEntitas()
        {
            dataTable.Reset();
            dataTable = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM data_entitas", conn))
            {
                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                dataTable.Load(reader);
            }
            return dataTable;
        }

        public void filldataTable()
        {
            dgv_dataentitas.DataSource = getDataEntitas();

            DataGridViewButtonColumn colEdit = new DataGridViewButtonColumn();
            colEdit.UseColumnTextForButtonValue = true;
            colEdit.Text = "Edit";
            colEdit.Name = "";
            dgv_dataentitas.Columns.Add(colEdit);

            DataGridViewButtonColumn colDelete = new DataGridViewButtonColumn();
            colDelete.UseColumnTextForButtonValue = true;
            colDelete.Text = "Delete";
            colDelete.Name = "";
            dgv_dataentitas.Columns.Add(colDelete);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MySqlCommand cmd;
            //conn.open();

            try
            {
                cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE data_entitas SET Status = @Status, Name = @Name, Race = @Race, Gender = @Gender, Residence = @Residence WHERE ID = @ID";
                cmd.Parameters.AddWithValue("@ID", textBox11.Text);
                cmd.Parameters.AddWithValue("@Status", textBox10.Text);
                cmd.Parameters.AddWithValue("@Name", textBox9.Text);
                cmd.Parameters.AddWithValue("@Race", textBox8.Text);
                cmd.Parameters.AddWithValue("@Gender", textBox7.Text);
                cmd.Parameters.AddWithValue("@Residence", textBox6.Text);
                cmd.ExecuteNonQuery();

                conn.Close();

                dgv_dataentitas.Columns.Clear();
                dataTable.Clear();
                filldataTable();

                textBox11.Clear();
                textBox10.Clear();
                textBox9.Clear();
                textBox8.Clear();
                textBox7.Clear();
                textBox6.Clear();
            }
            catch
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MySqlCommand cmd;
            //conn.open();

            try
            {
                cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO data_entitas(Status, Name, Race, Gender, Residence) VALUE(@Status, @Name, @Race, @Gender, @Residence)";
                cmd.Parameters.AddWithValue("@Status", textBox1.Text);
                cmd.Parameters.AddWithValue("@Name", textBox2.Text);
                cmd.Parameters.AddWithValue("@Race", textBox3.Text);
                cmd.Parameters.AddWithValue("@Gender", textBox4.Text);
                cmd.Parameters.AddWithValue("@Residence", textBox5.Text);
                cmd.ExecuteNonQuery();

                conn.Close();

                dgv_dataentitas.Columns.Clear();
                dataTable.Clear();
                filldataTable();

                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
            }
            catch
            {

            }
        }

        private void dgv_dataentitas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgv_dataentitas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 6)
            {
                int id = Convert.ToInt32(dgv_dataentitas.CurrentCell.RowIndex.ToString());
                textBox11.Text = dgv_dataentitas.Rows[id].Cells[0].Value.ToString();
                textBox10.Text = dgv_dataentitas.Rows[id].Cells[1].Value.ToString();
                textBox9.Text = dgv_dataentitas.Rows[id].Cells[2].Value.ToString();
                textBox8.Text = dgv_dataentitas.Rows[id].Cells[3].Value.ToString();
                textBox7.Text = dgv_dataentitas.Rows[id].Cells[4].Value.ToString();
                textBox6.Text = dgv_dataentitas.Rows[id].Cells[5].Value.ToString();

            }

            if(e.ColumnIndex == 7)
            {
                int id = Convert.ToInt32(dgv_dataentitas.CurrentCell.RowIndex.ToString());

                MySqlCommand cmd;
                //conn.Open();
                try
                {
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "DELETE FROM data_entitas WHERE ID = @ID";
                    cmd.Parameters.AddWithValue("@ID", dgv_dataentitas.Rows[id].Cells[0].Value.ToString());

                    cmd.ExecuteNonQuery();

                    resetId();

                    conn.Close();

                    dgv_dataentitas.Columns.Clear();
                    dataTable.Clear();
                    filldataTable();

                    
                }
                catch (Exception ex)
                {
                    //ex.Message.EndWith("");
                }
            }
        }
        public void searchData(string ValueToFind)
        {
            string searchQuery = "SELECT * FROM data_entitas WHERE CONCAT(Status, Name, Race, Gender, Residence) LIKE '%" + ValueToFind + "%'";
            MySqlDataAdapter adapter = new MySqlDataAdapter(searchQuery, conn);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dgv_dataentitas.DataSource = table;
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            searchData(textBox12.Text);
        }

        public void resetId()
        {
            MySqlScript script = new MySqlScript(conn, "SET @num := 0;" +
                "UPDATE data_entitas SET ID = @num := (@num + 1);" +
                "ALTER TABLE data_entitas AUTO_INCREMENT = 1;");

            script.Execute();

            conn.Close();
        }

        public void exportgridtopdf(DataGridView dgw,string filename)
        {
            BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, BaseFont.EMBEDDED);
            PdfPTable pdftable = new PdfPTable(dgw.Columns.Count);
            pdftable.DefaultCell.Padding = 3;
            pdftable.WidthPercentage = 100;
            pdftable.HorizontalAlignment = Element.ALIGN_LEFT;
            pdftable.DefaultCell.BorderWidth = 1;

            iTextSharp.text.Font text = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.NORMAL);
            //add header
            foreach (DataGridViewColumn column in dgw.Columns)
            {
                PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText, text));
                cell.BackgroundColor = new iTextSharp.text.BaseColor(240, 240, 240);
                pdftable.AddCell(cell);
            }

            //add datarow
            foreach (DataGridViewRow row in dgw.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    pdftable.AddCell(new Phrase(cell.Value.ToString(), text));
                }
            }

            var savefiledialoge = new SaveFileDialog();
            savefiledialoge.FileName = filename;
            savefiledialoge.DefaultExt = ".pdf";
            if (savefiledialoge.ShowDialog() == DialogResult.OK)
            {
                using (FileStream stream = new FileStream(savefiledialoge.FileName, FileMode.Create))
                {
                    Document pdfdoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                    PdfWriter.GetInstance(pdfdoc, stream);
                    pdfdoc.Open();
                    pdfdoc.Add(pdftable);
                    pdfdoc.Close();
                    stream.Close();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            exportgridtopdf(dgv_dataentitas, "test");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form1 fm1 = new Form1();
            fm1.Show();
            this.Hide();
        }
    }
}
