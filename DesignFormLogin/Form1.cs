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

namespace DesignFormLogin
{
    public partial class Form1 : Form
    {
        MySqlConnection conn = conncectionService.getConnection();

        public Form1()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, EventArgs e)
        {
            if (login(textBox1.Text, textBox2.Text))
            {
                Form3 fm = new Form3();
                fm.ShowDialog();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Tidak berhasil");
            }
        }

       


        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }



        private Boolean login(String sUsername, String sPassword)
        {
            string SQL = "SELECT username, password FROM login";
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(SQL, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if ((sUsername == reader.GetString(0) && (sPassword == reader.GetString(1))))
                {
                    conn.Close();
                    return true;
                }
            }
            conn.Close();
            return false;

        }
    }
}
