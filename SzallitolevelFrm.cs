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
    enum Szallitolevelkereses
    {

        Összes,
        Azonosító,
        Adószám,
        Bizonylatszám,
        Dátum
    }

    public partial class SzallitolevelFrm : Form
    {
        internal static List<Partner> beolvasottPartner = new List<Partner>();
        internal static List<Termek> beolvasottTermek = new List<Termek>();
        public static int szallitoAzonosito;

        internal List<Partner> BeolvasottPartner { get => beolvasottPartner; set => beolvasottPartner = value; }
        internal List<Termek> BeolvasottTermek { get => beolvasottTermek; set => beolvasottTermek = value; }
        internal List<SzallitoLevel> szallitoLista = ABKezelo.SzallitoListaFeltoltes();
        private Partner cegPartner = ABKezelo.PartnerKivalasztasKategoriaAlapjan("1");
       



        private List<SzallitoLevel> SzallitoDatetimeKereso(List<SzallitoLevel> szallitoLista, string keresett)
        {
            List<SzallitoLevel> talalatLista = new List<SzallitoLevel>();

            for (int i = 0; i < szallitoLista.Count; i++)
            {
                if (szallitoLista[i].BizonylatKelte.ToString().Contains(keresett))
                {
                    talalatLista.Add(szallitoLista[i]);//



                }

            }
            return talalatLista;

        }


        private List<Partner> SzallitoPartnerKereso(List<Partner> partnerLista, string keresett)//Szállító partnerlistáját fogja visszaadni és a parner Id alapján fogon a szállítót kiválasztani.
        {
            List<Partner> talalatLista = new List<Partner>();

            for (int i = 0; i < partnerLista.Count; i++)//ITT MÓDOSULT A BUG
            {
                if (partnerLista[i].VezetekNev.ToString().Contains(keresett) || partnerLista[i].VezetekNev.ToString().ToLower().Contains(keresett) ||
                    partnerLista[i].KeresztNev.ToString().Contains(keresett) || partnerLista[i].KeresztNev.ToString().ToLower().Contains(keresett) ||
                    partnerLista[i].CegNev.ToString().Contains(keresett) || partnerLista[i].CegNev.ToString().ToLower().Contains(keresett) ||
                    partnerLista[i].AdoSzam.ToString().Contains(keresett) || partnerLista[i].AdoSzam.ToString().ToLower().Contains(keresett))
                {
                    talalatLista.Add(partnerLista[i]);



                }

            }
            return talalatLista;

        }


        public SzallitolevelFrm()
        {
            InitializeComponent();
        }



        private void SzallitolevelFrm_Load(object sender, EventArgs e)
        {
            toolTip1.Active = true;
            KivetelKezeles.ModositTorolGomb(false, módosításToolStripMenuItem, törlésToolStripMenuItem);
            this.BackColor = SystemColors.GradientInactiveCaption;
            listView1.Left = this.Left + 10;
            listView1.Width = this.Width - 20;

            Kategoria_cbx.DataSource = Enum.GetValues(typeof(Szallitolevelkereses));

            try
            {

              ABKezelo.SzallitoLVFeltoltes(listView1);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "Hiba", "Sikertelen felolvasás", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void újSzállítólevélToolStripMenuItem_Click(object sender, EventArgs e)
        {

            UjSzallitoLevelFrm frm = new UjSzallitoLevelFrm(null, null, null, null);
            frm.Location = new Point(Form1.Form1ref.Location.X + Form1.Form1ref.ClientSize.Width, Form1.Form1ref.Location.Y);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                ABKezelo.SzallitoLVFeltoltes(listView1);
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }


            
        }

        private void módosításToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (listView1.SelectedItems.Count > 0 && cegPartner != null)
            {


                ListViewItem item = listView1.Items[listView1.SelectedIndices[0]];

                Partner partner = ABKezelo.PartnerKivalasztasAdoszamAlapjan(item.SubItems[4].Text);




                SzallitoLevel szallitoLevel = ABKezelo.SzallitoLevelKivalasztas(Convert.ToInt32(item.SubItems[0].Text));//terméklista beolvasás

                BindingList<KiadottTermek> kiadottTermekek = ABKezelo.KiadottListaFeltoltes(szallitoLevel.SzallitoId);



                UjSzallitoLevelFrm frm = new UjSzallitoLevelFrm(partner, szallitoLevel, kiadottTermekek, "modosit");
                frm.Location = new Point(Form1.Form1ref.Location.X + Form1.Form1ref.ClientSize.Width, Form1.Form1ref.Location.Y);

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    ABKezelo.SzallitoLVFeltoltes(listView1);
                    //listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
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
                szallitoAzonosito = int.Parse(item.SubItems[0].Text);
                try
                {

                    ABKezelo.KiadottTermekTorles(szallitoAzonosito);
                    ABKezelo.SzallitoLevelTorles(szallitoAzonosito);
                    ABKezelo.SzallitoLevelGarbageCollector();
                    ABKezelo.SzallitoLVFeltoltes(listView1);
                    //listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message, "A törlés sikertelen", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e) {
            /*
            if (kereses_tbx.Text.Length > 0){
                listView1.Items.Clear();
                List<SzallitoLevel> talaltSzalLevelek = ABKezelo.SzallitoLevelKereso(kereses_tbx.Text, (Szallitolevelkereses)Kategoria_cbx.SelectedItem);
                for (int i = 0; i < talaltSzalLevelek.Count; i++) ABKezelo.SzallitoLVFeltoltesManualisan(listView1, talaltSzalLevelek[i], ABKezelo.PartnerKivalasztasIdAlapjan(talaltSzalLevelek[i].PartnerId));
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }else{
                ABKezelo.SzallitoLVFeltoltes(listView1);
            }
            */

            List<SzallitoLevel> talaltSzalLevelek = ABKezelo.SzallitoLevelKereso(kereses_tbx.Text, (Szallitolevelkereses)Kategoria_cbx.SelectedItem);
            if (talaltSzalLevelek.Count > 0) listView1.Items.Clear();
            for (int i = 0; i < talaltSzalLevelek.Count; i++) {
                ABKezelo.SzallitoLVFeltoltesManualisan(listView1, talaltSzalLevelek[i], ABKezelo.PartnerKivalasztasIdAlapjan(talaltSzalLevelek[i].PartnerId));
            }
            //HianyListaJeloles(listView1, hiany, hianyLista);
            if (talaltSzalLevelek.Count == 0 && listView1.Items[0].Text != "Nincs találat!") {
                ABKezelo.UresLVFeltoltesManualisan(listView1);
            }
        }

        private void Kategoria_cbx_SelectedIndexChanged(object sender, EventArgs e) {
            if (kereses_tbx.Text.Length > 0) {
                listView1.Items.Clear();
                List<SzallitoLevel> talaltSzalLevelek = ABKezelo.SzallitoLevelKereso(kereses_tbx.Text, (Szallitolevelkereses)Kategoria_cbx.SelectedItem);
                for (int i = 0; i < talaltSzalLevelek.Count; i++) ABKezelo.SzallitoLVFeltoltesManualisan(listView1, talaltSzalLevelek[i], ABKezelo.PartnerKivalasztasIdAlapjan(talaltSzalLevelek[i].PartnerId));
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            } else {
                ABKezelo.SzallitoLVFeltoltes(listView1);
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewItem item = listView1.Items[listView1.SelectedIndices[0]];
            Partner partner = ABKezelo.PartnerKivalasztasAdoszamAlapjan(item.SubItems[4].Text);
            SzallitoLevel szallitoLevel = ABKezelo.SzallitoLevelKivalasztas(Convert.ToInt32(item.SubItems[0].Text));//terméklista beolvasás
            BindingList<KiadottTermek> kiadottTermekek = ABKezelo.KiadottListaFeltoltes(szallitoLevel.SzallitoId);
            SzallitoTermekListaFrm frm = new SzallitoTermekListaFrm(partner, szallitoLevel, kiadottTermekek, "szallito");
            frm.Location = new Point(Form1.Form1ref.Location.X + Form1.Form1ref.ClientSize.Width, Form1.Form1ref.Location.Y);
            frm.ShowDialog();
        }

        private void nyomtatásToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void excelbeMentésToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0 && cegPartner != null)
            {


                ListViewItem item = listView1.Items[listView1.SelectedIndices[0]];

                Partner partner = ABKezelo.PartnerKivalasztasAdoszamAlapjan(item.SubItems[4].Text);




                SzallitoLevel szallitoLevel = ABKezelo.SzallitoLevelKivalasztas(Convert.ToInt32(item.SubItems[0].Text));//terméklista beolvasás

                BindingList<KiadottTermek> kiadottTermekek = ABKezelo.KiadottListaFeltoltes(Convert.ToInt32(item.SubItems[0].Text));

                List<Termek> termekek = new List<Termek>();
                for (int i = 0; i < kiadottTermekek.Count; i++)
                {
                    Termek tmp = ABKezelo.TermekKivalasztas(kiadottTermekek[i].TermekId);
                    if (tmp != null) {
                        termekek.Add(tmp);//ITT KELL Lekérdezni a termék ID darabszámokat és kicseréniMajd
                        termekek[i].Mennyiseg = kiadottTermekek[i].DarabSzam;
                        termekek[i].NettoAr = kiadottTermekek[i].EladasiAr;
                    }
                }
               

                ABKezelo.ExcelSzallitoMentes(cegPartner, partner, termekek, szallitoLevel);
   
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                KivetelKezeles.ModositTorolGomb(true, módosításToolStripMenuItem, törlésToolStripMenuItem);

            }
            else
            {
                KivetelKezeles.ModositTorolGomb(false, módosításToolStripMenuItem, törlésToolStripMenuItem);

            }
        }

        private void listView1_Leave(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {

                listView1.SelectedItems.Clear();

            }
        }

        private void módosításToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            if (módosításToolStripMenuItem.ForeColor == Color.Gray)
            {


                toolTip1.Active = true;
                Point x = new Point(150, 50);
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
            if (módosításToolStripMenuItem.ForeColor == Color.Gray)
            {


                toolTip1.Active = true;
                Point x = new Point(200, 50);
                toolTip1.SetToolTip(menuStrip1, "uff");
                toolTip1.Show("Jelöljön ki egy elemet a listából!", Form1.Form1ref, x);
            }
        }

        private void törlésToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            if (módosításToolStripMenuItem.ForeColor == Color.Gray)
            {
                toolTip1.Active = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Helpfrm help = new Helpfrm(KivetelKezeles.getHelp("Szállítólevél menüpont"));
            help.ShowDialog();
        }

        private void listView1_MouseLeave(object sender, EventArgs e)
        {
            toolTip1.Active = false;
        }

        private void listView1_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.Active = true;
            Point x = new Point(listView1.Right - 200, listView1.Top - 150);
            toolTip1.SetToolTip(listView1, "");
            toolTip1.Show("Szállítólevél adatok részletes\nmegtekintéséhez\nkattintson duplán\negy szállítólevélre\na listában!", listView1, x);
        }
    }
}
            
