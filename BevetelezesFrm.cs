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
    public partial class BevetelezesFrm : Form
    {
        private static List<Partner> beolvasottBevetPartner = new List<Partner>();
        private static List<Termek> beolvasottBevetTermek = new List<Termek>();
        private static int bevetAzonosito;
        List<Bevetelezes> bevetBizonylatLista = ABKezelo.BevetBizonylatListaFeltoltes();
        private Partner cegPartner = ABKezelo.PartnerKivalasztasKategoriaAlapjan("1");
        internal static List<Partner> BeolvasottBevetPartner { get => beolvasottBevetPartner; set => beolvasottBevetPartner = value; }
        internal static List<Termek> BeolvasottBevetTermek { get => beolvasottBevetTermek; set => beolvasottBevetTermek = value; }
        public static int BevetAzonosito { get => bevetAzonosito; set => bevetAzonosito = value; }

        private List<Bevetelezes> BevetDatetimeKereso(List<Bevetelezes> bevetLista, string keresett)
        {
            List<Bevetelezes> talalatLista = new List<Bevetelezes>();

            for (int i = 0; i < bevetLista.Count; i++)
            {
                if (bevetLista[i].BizonylatKelte.ToString().Contains(keresett))
                {
                    talalatLista.Add(bevetLista[i]);



                }

            }
            return talalatLista;

        }
     

        private List<Partner> BevetPartnerKereso(List<Partner> partnerLista, string keresett)//Szállító partnerlistáját fogja visszaadni és a parner Id alapján fogon a szállítót kiválasztani.
        {
            List<Partner> talalatLista = new List<Partner>();

            for (int i = 0; i < partnerLista.Count; i++)
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
        private void KeresoEventFuggveny()
        {
            kereses_tbx.Enabled = true;
            try
            {
                if (Kategoria_cbx.SelectedIndex == 0)
                {
                    kereses_tbx.Enabled = false;
                    ABKezelo.BevetLVFeltoltes(listView1);
                    listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

                }
                else if (Kategoria_cbx.SelectedIndex == 1 && kereses_tbx.Text.Length > 0)
                {
                    kereses_tbx.Enabled = true;
                    int ideiglenes = -1;


                    if (int.TryParse(kereses_tbx.Text, out ideiglenes))
                    {
                        ABKezelo.BevetKeresesAzonositovAlapjan(listView1, ideiglenes);
                        listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                    }


                }
                else if (Kategoria_cbx.SelectedIndex == 2 && kereses_tbx.Text.Length > 0)
                {

                    kereses_tbx.Enabled = true;
                    List<Partner> partnerek = new List<Partner>();
                    List<Bevetelezes> keresettSzallito = new List<Bevetelezes>();
                    ;
                    for (int i = 0; i < bevetBizonylatLista.Count; i++)
                    {
                        partnerek.Add(ABKezelo.PartnerKivalasztasIdAlapjan(bevetBizonylatLista[i].PartnerId));

                    }
                }
                else if (Kategoria_cbx.SelectedIndex == 3 && kereses_tbx.Text.Length > 0)
                {
                    kereses_tbx.Enabled = true;
                    int ideiglenes = -1;
                    if (int.TryParse(kereses_tbx.Text, out ideiglenes))
                    {
                        ABKezelo.BevetKeresesMegrendeloSzamAlapjan(listView1, ideiglenes);
                        listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                    }

                }
                else if (Kategoria_cbx.SelectedIndex == 4 && kereses_tbx.Text.Length > 0)
                {
                    kereses_tbx.Enabled = true;
                    List<Bevetelezes> ideiglenesbevet = BevetDatetimeKereso(bevetBizonylatLista, kereses_tbx.Text);



                    listView1.Columns.Clear();
                    listView1.Items.Clear();
                    for (int i = 0; i < ideiglenesbevet.Count; i++)
                    {
                        
                        ABKezelo.BevetLVFeltoltesManualisan(listView1, ideiglenesbevet[i], ABKezelo.PartnerKivalasztasIdAlapjan(ideiglenesbevet[i].PartnerId));

                        listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "A keresett termékazonosító csak szám lehet!", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }
      

        public BevetelezesFrm()
        {
            InitializeComponent();
        }

        private void BevetelezesFrm_Load(object sender, EventArgs e)
        {
            KivetelKezeles.ModositTorolGomb(false, módosításToolStripMenuItem, törlésToolStripMenuItem);
            toolTip1.Active = true;
            this.BackColor = SystemColors.GradientInactiveCaption;
            listView1.Left = this.Left + 10;
            listView1.Width = this.Width - 20;
            Kategoria_cbx.DataSource = Enum.GetValues(typeof(Szallitolevelkereses));
            ABKezelo.BevetLVFeltoltes(listView1);
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void ÚjbevételezésToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UjBevetelezesFrm frm = new UjBevetelezesFrm(null, null, null, null);
            frm.Location = new Point(Form1.Form1ref.Location.X + Form1.Form1ref.ClientSize.Width, Form1.Form1ref.Location.Y);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                ABKezelo.BevetLVFeltoltes(listView1);
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
        }

        private void módosításToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {


                ListViewItem item = listView1.Items[listView1.SelectedIndices[0]];

                Partner partner = ABKezelo.PartnerKivalasztasAdoszamAlapjan(item.SubItems[4].Text);

                Bevetelezes bevetelezes = ABKezelo.BevetBizonylatKivalasztasIdAlapjan(Convert.ToInt32(item.SubItems[0].Text));//terméklista beolvasás

                BindingList<BevettTermek> bevettttTermekek = ABKezelo.BevettListaFeltoltes(bevetelezes.BevetId);



                UjBevetelezesFrm frm = new UjBevetelezesFrm(partner, bevetelezes, bevettttTermekek, "modosit");
                frm.Location = new Point(Form1.Form1ref.Location.X + Form1.Form1ref.ClientSize.Width, Form1.Form1ref.Location.Y);

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    ABKezelo.BevetLVFeltoltes(listView1);
                    listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
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
                bevetAzonosito = int.Parse(item.SubItems[0].Text);



                try
                {
                    BindingList<BevettTermek> bevettttTermekek = ABKezelo.BevettListaFeltoltes(bevetAzonosito);
                   /* for (int i = 0; i < bevettttTermekek.Count; i++) {
                        Termek tmp = ABKezelo.TermekKivalasztas(bevettttTermekek[i].TermekId);
                        tmp.Mennyiseg -= bevettttTermekek[i].DarabSzam;
                        ABKezelo.TermekModositas(tmp);
                    }*/
                    ABKezelo.BevetTermekTorles(bevetAzonosito);
                    ABKezelo.BevetBizonylatTorles(bevetAzonosito);
                    ABKezelo.BevetBizonylatGarbageCollector();
                    ABKezelo.BevetLVFeltoltes(listView1);
                    listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message, "A törlés sikertelen", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void kereses_tbx_TextChanged(object sender, EventArgs e)
        {
            KeresoEventFuggveny();
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewItem item = listView1.Items[listView1.SelectedIndices[0]];

            Partner partner = ABKezelo.PartnerKivalasztasAdoszamAlapjan(item.SubItems[4].Text);

            Bevetelezes bevetelezes = ABKezelo.BevetBizonylatKivalasztasIdAlapjan(Convert.ToInt32(item.SubItems[0].Text));//terméklista beolvasás

            BindingList<BevettTermek> bevettttTermekek = ABKezelo.BevettListaFeltoltes(bevetelezes.BevetId);


            BevetTermekListaFrm frm = new BevetTermekListaFrm(partner, bevetelezes, bevettttTermekek, "bevetelezes");
            frm.Location = new Point(Form1.Form1ref.Location.X + Form1.Form1ref.ClientSize.Width, Form1.Form1ref.Location.Y);
            frm.ShowDialog();
        }

        private void kereses_tbx_TextChanged_1(object sender, EventArgs e)
        {
            /*
            if (kereses_tbx.Text.Length > 0) {
                listView1.Items.Clear();
                List<Bevetelezes> talaltBevLevelek = ABKezelo.BevetelezesKereso(kereses_tbx.Text, (Szallitolevelkereses)Kategoria_cbx.SelectedItem);
                for (int i = 0; i < talaltBevLevelek.Count; i++) ABKezelo.BevetLVFeltoltesManualisan(listView1, talaltBevLevelek[i], ABKezelo.PartnerKivalasztasIdAlapjan(talaltBevLevelek[i].PartnerId));
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            } else {
                ABKezelo.BevetLVFeltoltes(listView1);
            }
            */

            List<Bevetelezes> talaltBevLevelek = ABKezelo.BevetelezesKereso(kereses_tbx.Text, (Szallitolevelkereses)Kategoria_cbx.SelectedItem);
            if (talaltBevLevelek.Count > 0) listView1.Items.Clear();
            for (int i = 0; i < talaltBevLevelek.Count; i++) {
                ABKezelo.BevetLVFeltoltesManualisan(listView1, talaltBevLevelek[i], ABKezelo.PartnerKivalasztasIdAlapjan(talaltBevLevelek[i].PartnerId));
                //listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }

            if (talaltBevLevelek.Count == 0 && listView1.Items[0].Text != "Nincs találat!") {
                ABKezelo.UresLVFeltoltesManualisan(listView1);
            }
        }

        private void Kategoria_cbx_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (kereses_tbx.Text.Length > 0) {
                List<Bevetelezes> talaltBevLevelek = ABKezelo.BevetelezesKereso(kereses_tbx.Text, (Szallitolevelkereses)Kategoria_cbx.SelectedItem);
                if(talaltBevLevelek.Count>0) listView1.Items.Clear();
                for (int i = 0; i < talaltBevLevelek.Count; i++) ABKezelo.BevetLVFeltoltesManualisan(listView1, talaltBevLevelek[i], ABKezelo.PartnerKivalasztasIdAlapjan(talaltBevLevelek[i].PartnerId));
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            } else {
                ABKezelo.BevetLVFeltoltes(listView1);
            }
        }

        private void excelbeMentésToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0 && cegPartner != null)
            {


                ListViewItem item = listView1.Items[listView1.SelectedIndices[0]];

                Partner partner = ABKezelo.PartnerKivalasztasAdoszamAlapjan(item.SubItems[4].Text);
               




                Bevetelezes bevetelezes = ABKezelo.BevetBizonylatKivalasztasIdAlapjan(Convert.ToInt32(item.SubItems[0].Text));

                BindingList<BevettTermek> bevetttermek = ABKezelo.BevettListaFeltoltes(Convert.ToInt32(item.SubItems[0].Text));

                List<Termek> termekek = new List<Termek>();
                for (int i = 0; i < bevetttermek.Count; i++)
                {
                    Termek tmp = ABKezelo.TermekKivalasztas(bevetttermek[i].TermekId);
                    if (tmp != null) {
                        termekek.Add(tmp);//ITT KELL Lekérdezni a termék ID darabszámokat és kicseréniMajd
                        termekek[i].Mennyiseg = bevetttermek[i].DarabSzam;
                        termekek[i].NettoAr = bevetttermek[i].BeszerzesiAr;
                    }
                }
              

                ABKezelo.ExcelbevetelezesMentes(partner, cegPartner, termekek, bevetelezes);

            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            KivetelKezeles.ModositTorolGomb(true, módosításToolStripMenuItem, törlésToolStripMenuItem);
                   
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
            Helpfrm help = new Helpfrm(KivetelKezeles.getHelp("Bevételezés menüpont"));
            help.ShowDialog();
        }

        private void listView1_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.Active = true;
            Point x = new Point(listView1.Right - 200, listView1.Top - 150);
            toolTip1.SetToolTip(listView1, "");
            toolTip1.Show("Bevételezési adatok részletes\nmegtekintéséhez\nkattintson duplán\negy bevételezési adatra\na listában!", listView1, x);
        }

        private void listView1_MouseLeave(object sender, EventArgs e)
        {
            toolTip1.Active = false;
        }
    }
}