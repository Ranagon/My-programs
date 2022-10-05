using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Szakdolgozat3
{
    public partial class Helpfrm : Form
    {
        public Helpfrm(string adat)
        {
            InitializeComponent();
            richTextBox1.Rtf = adat;
            richTextBox1.ReadOnly = true;
        }

        private void Helpfrm_Load(object sender, EventArgs e)
        {

        }
    }
}
