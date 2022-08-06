using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Szakdolgozat3
{
    enum KeresesKategoria
    {
        Összes,
        TermekAzonosito,
        TermekKategoria,
        Megnevezes,
        CikkSzam
    }

    public partial class TermekekFrm : Form
    {
        Termek modosit;
        internal Termek hiany;
        List<Termek> hianyLista = new List<Termek>();
        public static int termekAzonosito;
        internal string keresettSzoveg = "";


        public Termek Modosit { get => modosit; set => modosit = value; }

        internal static void HianyListaJeloles(ListView listView1, Termek hiany, List<Termek> hianyLista)//A LISTVIEW ADATOK ALAPJÁN KIPIROSÍTJA AZON TERMÉKEKET MELYEKBŐL A MINIMÁLIS KÉSZLET ALAPJÁN HIÁNY VAN
        {
            if (listView1.Items.Count > 0)
            {

                for (int i = 0; i < listView1.Items.Count; i++) //hiánylista kiemelés
                {
                    ListViewItem item = listView1.Items[i];

                    if (item.SubItems[0].Text != "Nincs találat!")
                    {

                        if (int.Parse(item.SubItems[3].Text) < int.Parse(item.SubItems[10].Text))
                        {

                            hiany = new Termek(int.Parse(item.SubItems[0].Text), item.SubItems[1].Text, item.SubItems[2].Text, item.SubItems[8].Text,
                                int.Parse(item.SubItems[3].Text), item.SubItems[4].Text, int.Parse(item.SubItems[5].Text), decimal.Parse(item.SubItems[6].Text), decimal.Parse(item.SubItems[7].Text),
                                 item.SubItems[9].Text, int.Parse(item.SubItems[10].Text));

                            hianyLista.Add(hiany);
                            item.ForeColor = Color.Red;//Ez a módszer kell majd a kimutatásokba.*/
                        }
                        else
                        {
                            item.ForeColor = SystemColors.WindowText;
                        }
                    }


                }

            }
        }

        internal static List<Termek> TermekKategoriaKereso(List<Termek> termekLista, string keresett)//KERESŐFÜGGVÉNY
        {
            List<Termek> talalatLista = new List<Termek>();

            for (int i = 0; i < termekLista.Count; i++)
            {
                if (termekLista[i].TermekKategoria.ToString().ToUpper().Contains(keresett) || termekLista[i].TermekKategoria.ToString().ToLower().Contains(keresett))
                {
                    talalatLista.Add(termekLista[i]);
                }
            }
            return talalatLista;
        }
        internal static List<Termek> TermekMegnevezesKereso(List<Termek> termekLista, string keresett)
        {
            List<Termek> talalatLista = new List<Termek>();

            for (int i = 0; i < termekLista.Count; i++)
            {
                if (termekLista[i].Megnevezes.ToString().ToUpper().Contains(keresett) || termekLista[i].Megnevezes.ToString().ToLower().Contains(keresett))
                {
                    talalatLista.Add(termekLista[i]);
                }

            }
            return talalatLista;
        }

        public TermekekFrm()
        {
            InitializeComponent();
        }
        private void TermekekFrm_Load(object sender, EventArgs e)
        {
            KivetelKezeles.ModositTorolGomb(false, termékMódosításaToolStripMenuItem, törlésToolStripMenuItem);//MÓDOSÍT/TÖRÖL GOMB SZÍNEZŐ
            this.BackColor = SystemColors.GradientInactiveCaption;
            Kategoria_cbx.DataSource = Enum.GetValues(typeof(KeresesKategoria));

            try
            {
                ABKezelo.TermekLVFeltoltes(listView1);
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "Hiba", "Sikertelen felolvasás", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            HianyListaJeloles(listView1, hiany, hianyLista);
            this.ActiveControl = listView1;
        }

       internal void TermekDuplikacioLV(string cikkszam)
        {
            for (int i = 0; i < listView1.Items.Count; i++) //hiánylista kiemelés
            {
                ListViewItem item2 = listView1.Items[i];
                if (item2.SubItems[8].Text == cikkszam)
                {
                    item2.ForeColor = Color.DarkRed;//Ez a módszer kell majd a kimutatásokba.
                }
                if (int.Parse(item2.SubItems[3].Text) < int.Parse(item2.SubItems[10].Text))
                {
                    item2.ForeColor = SystemColors.WindowText;
                }
            }
        }

        private void újTermékToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UjTermekFrm frm = new UjTermekFrm(null, null, this);
            frm.Location = new Point(Form1.Form1ref.Location.X + Form1.Form1ref.ClientSize.Width, Form1.Form1ref.Location.Y);

            if (frm.ShowDialog() == DialogResult.OK)
            {
                ABKezelo.TermekLVFeltoltes(listView1);
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);//TERMÉKLV MÉRETEZÉS
                for (int i = 0; i < listView1.Items.Count; i++) //hiánylista kiemelés
                {
                    ListViewItem item2 = listView1.Items[i];
                    if (item2.ForeColor == Color.DarkRed)
                    {
                        item2.ForeColor = SystemColors.WindowText;//Ez a módszer kell majd a kimutatásokba.
                    }
                    if (int.Parse(item2.SubItems[3].Text) < int.Parse(item2.SubItems[10].Text))
                    {
                        item2.ForeColor = Color.Red;
                    }

                }HianyListaJeloles(listView1,hiany,hianyLista);
                listView1.ForeColor = SystemColors.WindowText;
            }
        
        }

        private void termékMódosításaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Termek termek;
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem item = listView1.Items[listView1.SelectedIndices[0]];
                termek = new Termek(int.Parse(item.SubItems[0].Text), item.SubItems[1].Text, item.SubItems[2].Text, item.SubItems[8].Text,
                   int.Parse(item.SubItems[3].Text), item.SubItems[4].Text, int.Parse(item.SubItems[5].Text), decimal.Parse(item.SubItems[6].Text), decimal.Parse(item.SubItems[7].Text),
                    item.SubItems[9].Text, int.Parse(item.SubItems[10].Text));

                UjTermekFrm frm = new UjTermekFrm(termek, "modosit", null);
                frm.Location = new Point(Form1.Form1ref.Location.X + Form1.Form1ref.ClientSize.Width, Form1.Form1ref.Location.Y);

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    ABKezelo.TermekLVFeltoltes(listView1);
                    HianyListaJeloles(listView1, hiany, hianyLista);
                    listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);//TERMÉKLV MÉRETEZÉS
                }
            }
            else
            {

            }
        }
        private void törlésToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0 && MessageBox.Show("Biztos, hogy törli a kijelölt tételt?", "Figyelem!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ListViewItem item = listView1.Items[listView1.SelectedIndices[0]];
                termekAzonosito = Convert.ToInt32(item.SubItems[0].Text);
                try
                {
                    ABKezelo.TermekLomtarba(termekAzonosito);//kell még egy select, ami megnézi, hohy a szállítólevelek közt/bevételezésekben benne van-e. ha igen, akkor lomtár, h nem, delete!!!
                    ABKezelo.TermekLVFeltoltes(listView1);
                    listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);//TERMÉKLV MÉRETEZÉS
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "A törlés sikertelen", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            if (ABKezelo.PartnerKivalasztasKategoriaAlapjan("1") != null && ABKezelo.PartnerLista().Count > 0 && ABKezelo.TermekListaFeltoltes().Count > 0 || ABKezelo.vanSzallitolevel())
            {
                Form1.Form1ref.szallitoBevetGomb(true);
            }
            else
            {
                Form1.Form1ref.szallitoBevetGomb(false);
            }
        }

        private void excelbeMentésToolStripMenuItem_Click(object sender, EventArgs e)
        {

      
        List<Termek> termek = ABKezelo.TermekListaFeltoltes();
            MessageBox.Show(termek[0].AfaKulcs.ToString(), termek[1].AfaKulcs.ToString());
            if (termek.Count>0)
            {
                ABKezelo.ExcelTermekmentes(termek);
            }
                
        }

        private void kereses_tbx_TextChanged(object sender, EventArgs e)
        {
            List<Termek> termekekList = ABKezelo.termekKereso(kereses_tbx.Text, (KeresesKategoria)Kategoria_cbx.SelectedIndex);
            if(termekekList.Count > 0) listView1.Items.Clear();
            for (int i = 0; i < termekekList.Count; i++)
            {
                ABKezelo.TermekLVFeltoltesManualisan(listView1, termekekList[i]);
            }
            HianyListaJeloles(listView1, hiany, hianyLista);
            if (termekekList.Count==0 && listView1.Items[0].Text != "Nincs találat!")
            {
                ABKezelo.UresLVFeltoltesManualisan(listView1);
            }
        }

        private void Kategoria_cbx_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            List<Termek> termekekList = ABKezelo.termekKereso(kereses_tbx.Text, (KeresesKategoria)Kategoria_cbx.SelectedIndex);
            listView1.Items.Clear();
            for (int i = 0; i < termekekList.Count; i++)
            {
                ABKezelo.TermekLVFeltoltesManualisan(listView1, termekekList[i]);
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
            HianyListaJeloles(listView1, hiany, hianyLista);
            if (termekekList.Count==0)
            {
                listView1.Columns.Clear();
                ABKezelo.UresLVFeltoltesManualisan(listView1);
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count>0)
            {
                KivetelKezeles.ModositTorolGomb(true, termékMódosításaToolStripMenuItem, törlésToolStripMenuItem);
            }
            else
            {
                KivetelKezeles.ModositTorolGomb(false, termékMódosításaToolStripMenuItem, törlésToolStripMenuItem);

            }
        }

        private void segitseg_btn_Click(object sender, EventArgs e)
        {           
            Helpfrm help = new Helpfrm(KivetelKezeles.getHelp("Termékek menüpont"));
            help.ShowDialog();
        }

        private void listView1_Leave(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count>0)
            {            
                listView1.SelectedItems.Clear();   
            }
        }

        private void termékMódosításaToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            if (termékMódosításaToolStripMenuItem.ForeColor == Color.Gray)
            {
                toolTip1.Active = true;
                Point x = new Point(170, 20);
                toolTip1.SetToolTip(menuStrip1, "uff");
                toolTip1.Show("Jelöljön ki egy elemet a listából!", Form1.Form1ref, x);
            }
        }

        private void termékMódosításaToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            if (termékMódosításaToolStripMenuItem.ForeColor == Color.Gray)
            {
                toolTip1.Active = false;
            }
        }

        private void törlésToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            if (törlésToolStripMenuItem.ForeColor == Color.Gray)
            {
                toolTip1.Active = true;
                Point x = new Point(220, 20);
                toolTip1.SetToolTip(menuStrip1, "uff");
                toolTip1.Show("Jelöljön ki egy elemet a listából!", Form1.Form1ref, x);
            }
        }

        private void törlésToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            if (törlésToolStripMenuItem.ForeColor == Color.Gray)
            {
                toolTip1.Active = false;
            }
        }

        private void TermekekFrm_MouseMove(object sender, MouseEventArgs e)
        {
           
        }
    }
}
