﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesignFormLogin
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            panel2.Width += 3;

            if (panel2.Width >= 599)
            {
                timer1.Stop();
                Form2 fm2 = new Form2();
                fm2.Show();
                this.Hide();
            }
        }
    }
}
