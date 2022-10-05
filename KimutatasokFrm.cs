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
    public partial class KimutatasokFrm : Form
    {
        public KimutatasokFrm()
        {
            InitializeComponent();
           
        }

        private void KimutatasokFrm_Load(object sender, EventArgs e)
        {
            BackColor = SystemColors.GradientInactiveCaption;
            tabControl1.SelectedTab.BackColor= this.BackColor = SystemColors.GradientInactiveCaption;
            if (tabControl1.SelectedTab.Name== "Hianylista")
            {
                ABKezelo.TermekHianyLVFeltoltes(HianyLista_lv);
                HianyLista_lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);//TERMÉKLV MÉRETEZÉS
            }
        }

        private void excelbeMentésToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Termek> termek = ABKezelo.HianylistaFeltoltes();
            if (termek.Count > 0)
            {
                ABKezelo.ExcelHianylistamentes(termek);
            }
        }
    }
}
