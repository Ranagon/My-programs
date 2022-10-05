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
    enum partnerKereses
    {
        Összes,
        Partnercsoport,
        Adószám,
        Cégnév
    }
    public partial class PartnerNyilvantartasFrm : Form
    {
        internal static string partnerAzonosito;
        
        internal static BindingList<Partner> partner = new BindingList<Partner>();

        internal static List<Partner> PartnerCsoportKereso(List<Partner> partnerLista, string keresett)//KeresoEventFuggveny segédfüggvénye
        {
            List<Partner> talalatLista = new List<Partner>();

            for (int i = 0; i < partnerLista.Count; i++)
            {
                if (partnerLista[i].PartnerCsoport.ToString().ToUpper().Contains(keresett) || partnerLista[i].PartnerCsoport.ToString().ToLower().Contains(keresett))
                {
                    talalatLista.Add(partnerLista[i]);

                }

            }
            return talalatLista;

        }
        internal static List<Partner> AdoSzamKereso(List<Partner> partnerLista, string keresett)//KeresoEventFuggveny segédfüggvénye
        {
            List<Partner> talalatLista = new List<Partner>();

            for (int i = 0; i < partnerLista.Count; i++)
            {
                if (partnerLista[i].AdoSzam.ToString().ToUpper().Contains(keresett))
                {
                    talalatLista.Add(partnerLista[i]);//
                }

            }
            return talalatLista;
        }
        internal static List<Partner> CegNevKereso(List<Partner> partnerLista, string keresett)//KeresoEventFuggveny segédfüggvénye
        {
            List<Partner> talalatLista = new List<Partner>();

            for (int i = 0; i < partnerLista.Count; i++)
            {
                if (partnerLista[i].CegNev.ToString().ToUpper().Contains(keresett) || partnerLista[i].CegNev.ToString().ToLower().Contains(keresett))
                {
                    talalatLista.Add(partnerLista[i]);//
                }
            }
            return talalatLista;
        }

        internal static void KeresoEventFuggveny(ComboBox Kategoria_cbx, ListView listView1, TextBox kereses_tbx)//PARTNEREK KÖZT KEZES
        {
            kereses_tbx.Enabled = true;
            try
            {
                if (Kategoria_cbx.SelectedIndex == 0)
                {
                    kereses_tbx.Enabled = false;
                    ABKezelo.PartnerLVFeltoltes(listView1);
                    listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                }
                else if (Kategoria_cbx.SelectedIndex == 1 && kereses_tbx.Text.Length > 0)
                {
                    kereses_tbx.Enabled = true;
                    List<Partner> partnerek = ABKezelo.PartnerLista();

                    List<Partner> ideiglenes = PartnerCsoportKereso(partnerek, kereses_tbx.Text);
                    listView1.Columns.Clear();
                    listView1.Items.Clear();
                    for (int i = 0; i < ideiglenes.Count; i++)
                    {
                        ABKezelo.PartnerLVFeltoltesManualisan(listView1, ideiglenes[i]);

                        listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                    }

                }
                else if (Kategoria_cbx.SelectedIndex == 2 && kereses_tbx.Text.Length > 0)
                {
                    kereses_tbx.Enabled = true;
                    int ideiglenes = -1;

                    if (int.TryParse(kereses_tbx.Text, out ideiglenes))
                    {
                        ABKezelo.PartnerLVFeltoltesAdoSzamAlapjan(listView1, ideiglenes);

                        listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                    }
                }
                else if (Kategoria_cbx.SelectedIndex == 3 && kereses_tbx.Text.Length > 0)
                {
                    kereses_tbx.Enabled = true;
                    List<Partner> partnerek = ABKezelo.PartnerLista();

                    List<Partner> ideiglenes = CegNevKereso(partnerek, kereses_tbx.Text);
                    listView1.Columns.Clear();
                    listView1.Items.Clear();
                    for (int i = 0; i < ideiglenes.Count; i++)
                    {
                        ABKezelo.PartnerLVFeltoltesManualisan(listView1, ideiglenes[i]);

                        listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "A keresett termékazonosító csak szám lehet!", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }
       
        public PartnerNyilvantartasFrm()
        {
            InitializeComponent();

        }

        private void PartnerNyilvantartasFrm_Load(object sender, EventArgs e)
        {
            toolTip1.Active = true;
          
            this.BackColor = SystemColors.GradientInactiveCaption;
          
            Partner_lv.Clear();

            ABKezelo.PartnerLVFeltoltes(Partner_lv);
            Partner_lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);//TERMÉKLV MÉRETEZÉS
            Kategoria_cbx.DataSource = Enum.GetValues(typeof(partnerKereses));
            KivetelKezeles.ModositTorolGomb(false, módosításToolStripMenuItem, törlésToolStripMenuItem);//EZ A MÓDOSÍT/TÖRÖL GOMBOT ÁLLÍTJA
          
        }
        private void újPartnerToolStripMenuItem_Click(object sender, EventArgs e)//ÚJ PARTNER
        {
            UjPartnerFrm frm = new UjPartnerFrm();


            if (frm.ShowDialog() == DialogResult.OK)
            {

                ABKezelo.PartnerLVFeltoltes(Partner_lv);               
                Partner_lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);//TERMÉKLV MÉRETEZÉS               
            }
        }

        private void módosításToolStripMenuItem_Click(object sender, EventArgs e)//PARTNER MÓDOSÍTÁS
        {
            if (módosításToolStripMenuItem.ForeColor == SystemColors.WindowText && Partner_lv.SelectedItems.Count > 0 && Partner_lv.SelectedItems.Count > 0)
            {
                ListViewItem item = Partner_lv.Items[Partner_lv.SelectedIndices[0]];
                UjPartnerFrm frm = new UjPartnerFrm(ABKezelo.PartnerKivalasztasAdoszamAlapjan(item.SubItems[5].Text), "modosit");

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    ABKezelo.PartnerLVFeltoltes(Partner_lv);
                    Partner_lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);//TERMÉKLV MÉRETEZÉS
                }
            }
        }
        private void törlésToolStripMenuItem_Click(object sender, EventArgs e)//PARTNER TÖRLÉS
        {          
                if (törlésToolStripMenuItem.ForeColor == SystemColors.WindowText && Partner_lv.SelectedItems.Count > 0 && MessageBox.Show("Partner adatainak törlése esetén az összes,\n hozzárendelt szállítólevél, és bevételezés adat\n is törlődni fog.Valóban törli a kijelölt elemet?", "Figyelem!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                ListViewItem item = Partner_lv.Items[Partner_lv.SelectedIndices[0]];
                partnerAzonosito = ABKezelo.PartnerKivalasztasAdoszamAlapjan(item.SubItems[5].Text).PartnerAzonosito.ToString();
                
                try
                {

                    ABKezelo.PartnerTorles(Convert.ToInt32(partnerAzonosito), item.SubItems[5].Text);
                    ABKezelo.PartnerLVFeltoltes(Partner_lv);
                    Partner_lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);//TERMÉKLV MÉRETEZÉS
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
            List<Partner> partnerek = ABKezelo.PartnerLista();
            if (partnerek.Count>0)
            {
                ABKezelo.ExcelPartnerekmentes(partnerek);
            }          
        }

        private void textBox1_TextChanged(object sender, EventArgs e)//KERESÉS TEXTBOX 
        {
            List<Partner> partnerek = ABKezelo.PartnerKereso(kereses_tbx.Text, (partnerKereses)Kategoria_cbx.SelectedIndex);
            if(partnerek.Count > 0) Partner_lv.Items.Clear();
            for (int i = 0; i < partnerek.Count; i++)
            {
                ABKezelo.PartnerLVFeltoltesManualisan(Partner_lv, partnerek[i]);
            }

            if (partnerek.Count == 0 && Partner_lv.Items[0].Text != "Nincs találat!")
            {
                ABKezelo.UresLVFeltoltesManualisan(Partner_lv);
            }
        }

        private void Kategoria_cbx_SelectedIndexChanged(object sender, EventArgs e)//KERESÉSI KATEGÓRIA TEXTBOX
        {

            List<Partner> partnerek = ABKezelo.PartnerKereso(kereses_tbx.Text, (partnerKereses)Kategoria_cbx.SelectedIndex);
            Partner_lv.Items.Clear();
            for (int i = 0; i < partnerek.Count; i++)
            {

                ABKezelo.PartnerLVFeltoltesManualisan(Partner_lv, partnerek[i]);
                Partner_lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }

            if (partnerek.Count == 0)
            {
                Partner_lv.Columns.Clear();
                ABKezelo.UresLVFeltoltesManualisan(Partner_lv);
                Partner_lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
        }

        private void Partner_lv_SelectedIndexChanged(object sender, EventArgs e)//A MÓDOSÍTÁS/TÖRLÉS GOMB ÁLLÍTÓDIK,HA VAN KIJELÖLVE PARTNER
        {

            if (Partner_lv.SelectedItems.Count > 0)
            {
                KivetelKezeles.ModositTorolGomb(true, módosításToolStripMenuItem, törlésToolStripMenuItem);

            }
            else
            {
                KivetelKezeles.ModositTorolGomb(false, módosításToolStripMenuItem, törlésToolStripMenuItem);

            }
        }

        private void Partner_lv_MouseDoubleClick(object sender, MouseEventArgs e)//PARTNER MEGTEKINTÉSE 
        {

            if (Partner_lv.SelectedItems.Count > 0)
            {
                ListViewItem item = Partner_lv.Items[Partner_lv.SelectedIndices[0]];
                partnerAzonosito = item.SubItems[5].Text;


                UjPartnerFrm frm = new UjPartnerFrm(ABKezelo.PartnerKivalasztasAdoszamAlapjan(item.SubItems[5].Text), "megtekint");
                frm.ShowDialog();
            }
        }

        private void Partner_lv_Leave(object sender, EventArgs e)
        {
            if (Partner_lv.SelectedItems.Count > 0)
            {

                Partner_lv.SelectedItems.Clear();

            }
        }

        private void módosításToolStripMenuItem_MouseEnter(object sender, EventArgs e)//VIZSGÁLJA A GOMBOT 
        {
            if (módosításToolStripMenuItem.ForeColor == Color.Gray)//HA SZÜRKE,TOOLTIP AKTÍV
            {


                toolTip1.Active = true;
                Point x = new Point(170, 20);
                toolTip1.SetToolTip(menuStrip1, "uff");
                toolTip1.Show("Jelöljön ki egy elemet a listából!", Form1.Form1ref, x);
            }
        }

        private void módosításToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            if (módosításToolStripMenuItem.ForeColor == Color.Gray)
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

        private void segitseg_btn_Click(object sender, EventArgs e)//SEGÍTSÉG MENÜPONT
        {
            Helpfrm help = new Helpfrm(KivetelKezeles.getHelp("Partnerek menüpont"));
            help.ShowDialog();
        }

      

        private void Partner_lv_MouseLeave(object sender, EventArgs e)
        {
            toolTip1.Active = false;
        }

        private void Partner_lv_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.Active = true;
            Point x = new Point(Partner_lv.Right - 200, Partner_lv.Top - 150);
            toolTip1.SetToolTip(Partner_lv, "");
            toolTip1.Show("Partner adatok részletes\nmegtekintéséhez\nkattintson duplán\negy partnerre\na listában!", Partner_lv, x);
        }
    }
}
