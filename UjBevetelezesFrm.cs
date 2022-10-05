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
    public partial class UjBevetelezesFrm : Form
    {
        Partner cegPartner = ABKezelo.PartnerKivalasztasKategoriaAlapjan("1");       
        Bevetelezes bev;
        bool modositas = false;
        bool mentes = false;
        decimal kedvezmeny = -1;
        Partner vevoPartner;
        string oldal = "";
        List<Termek> termekModLista = new List<Termek>();
        List<Termek> toroltTermekek = new List<Termek>();
        List<Termek> hozzaadottTermekek = new List<Termek>();

        internal UjBevetelezesFrm(Partner partner, Bevetelezes bevetelezes, BindingList<BevettTermek> lista, string adat)
        {
            InitializeComponent();
            
            oldal = adat;
            bev = bevetelezes;
            Kategoria_cbx.DataSource = Enum.GetValues(typeof(KeresesKategoria));
            
            if (cegPartner != null) {
                VevoNev_tbx.Text = cegPartner.VezetekNev + " " + cegPartner.KeresztNev;
                VevoCim_tbx.Text = cegPartner.Irsz + " " + cegPartner.Kozterulet + " " + cegPartner.KozteruletJelleg + " " + cegPartner.HazSzam + " " + cegPartner.EmeletAjto;
                VevoAdoszam_tbx.Text = cegPartner.AdoSzam.ToString();
            }
            if (string.IsNullOrEmpty(EladoNev_tbx.Text) && string.IsNullOrEmpty(EladoCim_tbx.Text) && string.IsNullOrEmpty(EladoAdoSzam_tbx.Text))
            {
                EladoNev_tbx.Text = "Kötelező kitölteni!";
                EladoCim_tbx.Text = "Kötelező kitölteni!";
                EladoAdoSzam_tbx.Text = "Kötelező kitölteni!";
            }
            if (partner != null) {
                EladoNev_tbx.Text = partner.VezetekNev + " " + partner.KeresztNev;
                EladoCim_tbx.Text = partner.Irsz + " " + partner.Kozterulet + " " + partner.KozteruletJelleg + " " + partner.HazSzam + " " + partner.EmeletAjto;
                EladoAdoSzam_tbx.Text = partner.AdoSzam.ToString();
            }
            ABKezelo.TermekLVFeltoltes(Termekek_lv);
            Termekek_lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);//TERMÉKLV MÉRETEZÉSgggggg
            EladoNev_tbx.ReadOnly = true;
            EladoCim_tbx.ReadOnly = true;
            EladoAdoSzam_tbx.ReadOnly = true;
            VevoNev_tbx.ReadOnly = true;
            VevoCim_tbx.ReadOnly = true;
            VevoAdoszam_tbx.ReadOnly = true;
            Osszeg_tbx.ReadOnly = true;
            Osszeg_tbx.Enter += (s, zs) => { kereses_tbx.Focus(); };           
            button6.Enabled = false;
            button9.Enabled = false;
            //OK.Enabled = false;
            if (adat == "modosit") {
                //BEVÉTELEZÉS MÓDOSÍTÁSA
                modositas = true;
                vevoPartner = partner;
                this.Text = "Bevételezési bizonylat módosítása";
                button1.Visible = false;
                if (bevetelezes != null) MegrendelesSzam_tbx.Text = bevetelezes.MegrendelesSzam.ToString();
                VevoNev_tbx.ReadOnly = true;
                VevoCim_tbx.ReadOnly = true;
                VevoAdoszam_tbx.ReadOnly = true;
                MegrendelesSzam_tbx.ReadOnly = true;
                VevoNev_tbx.Enter += (s, zs) => { Bizonylatkelte_dtp.Focus(); };
                VevoCim_tbx.Enter += (s, zs) => { Bizonylatkelte_dtp.Focus(); };
                VevoAdoszam_tbx.Enter += (s, zs) => { Bizonylatkelte_dtp.Focus(); };
                EladoNev_tbx.Enter += (s, zs) => { Bizonylatkelte_dtp.Focus(); };
                EladoCim_tbx.Enter += (s, zs) => { Bizonylatkelte_dtp.Focus(); };
                EladoAdoSzam_tbx.Enter += (s, zs) => { Bizonylatkelte_dtp.Focus(); };
                MegrendelesSzam_tbx.Enter += (s, zs) => { Bizonylatkelte_dtp.Focus(); };
                bevettermekekLVFeltolt(lista);
                Osszeg_tbx.Text = BevettTermek.ArSzamito(termekModLista, vevoPartner.Kedvezmeny).ToString();

            } else {
                //ÚJ BEVÉTELEZÉS
                this.Text = "Bevételezési bizonylat létrehozása";
                VevoNev_tbx.Enter += (s, zs) => { button1.Focus(); };
                VevoCim_tbx.Enter += (s, zs) => { button1.Focus(); };
                VevoAdoszam_tbx.Enter += (s, zs) => { button1.Focus(); };
                EladoNev_tbx.Enter += (s, zs) => { button1.Focus(); };
                EladoCim_tbx.Enter += (s, zs) => { button1.Focus(); };
                EladoAdoSzam_tbx.Enter += (s, zs) => { button1.Focus(); };
            }
            

        }

        private void bevettermekekLVFeltolt(BindingList<BevettTermek> bevettTermekek=null) {
            Kiadotttermek_lv.Columns.Clear();
            Kiadotttermek_lv.Items.Clear();
            Kiadotttermek_lv.View = View.Details;
            if (Kiadotttermek_lv.Columns.Count == 0) {
                Kiadotttermek_lv.Columns.Add("Termékazonosító");
                Kiadotttermek_lv.Columns.Add("Termékkategória");
                Kiadotttermek_lv.Columns.Add("Megnevezés");
                Kiadotttermek_lv.Columns.Add("Mennyíség");
                Kiadotttermek_lv.Columns.Add("Mennyíség egysége");
                Kiadotttermek_lv.Columns.Add("Netto ár");
                Kiadotttermek_lv.Columns.Add("Brutto ár");
                Kiadotttermek_lv.Columns.Add("Áfa kulcs");
                Kiadotttermek_lv.Columns.Add("NAV sorszám");
                Kiadotttermek_lv.Columns.Add("Cikkszám");
                Kiadotttermek_lv.Columns.Add("Minimális készlet");
            }
            if (bevettTermekek != null && bevettTermekek.Count > 0) {
                termekModLista.Clear();
                for (int i = 0; i < bevettTermekek.Count; i++) {
                    Termek termek = bevettTermekek[i].toTermek();
                    termekModLista.Add(termek);
                    string[] adatok = {
                   termek.TermekAzonosito.ToString(),termek.TermekKategoria.ToString(),termek.Megnevezes,termek.Mennyiseg.ToString(),termek.MennyisegEgysege,termek.BruttoAr.ToString(),
                   termek.NettoAr.ToString(),termek.AfaKulcs.ToString(),termek.NavSorszam,termek.Cikkszam,termek.MinimalisKeszlet.ToString()};
                    Kiadotttermek_lv.Items.Add(new ListViewItem(adatok));
                    Kiadotttermek_lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                }
            } else {
                for (int i = 0; i < termekModLista.Count; i++) {
                    string[] adatok = {
                   termekModLista[i].TermekAzonosito.ToString(),termekModLista[i].TermekKategoria.ToString(),termekModLista[i].Megnevezes,termekModLista[i].Mennyiseg.ToString(),termekModLista[i].MennyisegEgysege,termekModLista[i].BruttoAr.ToString(),
                   termekModLista[i].NettoAr.ToString(),termekModLista[i].AfaKulcs.ToString(),termekModLista[i].NavSorszam,termekModLista[i].Cikkszam,termekModLista[i].MinimalisKeszlet.ToString()};
                    Kiadotttermek_lv.Items.Add(new ListViewItem(adatok));
                    Kiadotttermek_lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                }
            }
        }/// <summary>
         ///    private void termekekLVFeltoltes(List<Termek> bemenet)
         /// </summary>
         /// <param name="bemenet"></param>

        private void UjBevetelezesFrm_Load(object sender, EventArgs e)
        {
            this.BackColor = SystemColors.GradientInactiveCaption;
            
            if (MegrendelesSzam_tbx.Text == "") {
                MegrendelesSzam_tbx.Text = "Kötelező kitölteni!";
                MegrendelesSzam_tbx.ForeColor = Color.Gray;
            }
            //toolTip1.ShowAlways = true;
            toolTip1.SetToolTip(OK, "A gomb a szállítólevél\r\nmaradéktalan kitöltése\r\nesetén aktiválódik.");
            toolTip1.SetToolTip(button9, "Kérjük, jelöljön\r\nki egy terméket a\r\nterméklistából!");
            toolTip1.SetToolTip(button6, "Kérjük, jelöljön\r\nki egy terméket a\r\nKiadott termékekből!");
            toolTip1.SetToolTip(button1, "Partner adatok\r\nhozzáadása,\r\nmódosítása.");
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            PartnerBongeszesFrm.szallitoPartner = null;
            PartnerBongeszesFrm frm = new PartnerBongeszesFrm();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                if (PartnerBongeszesFrm.szallitoPartner != null)
                {
                    EladoNev_tbx.Text = PartnerBongeszesFrm.szallitoPartner.VezetekNev;
                    EladoCim_tbx.Text = PartnerBongeszesFrm.szallitoPartner.Orszag;
                    EladoAdoSzam_tbx.Text = PartnerBongeszesFrm.szallitoPartner.AdoSzam.ToString();//MŰKÖDIK
                    vevoPartner = PartnerBongeszesFrm.szallitoPartner;
                   
                }
            }
        }

        private void Termekek_lv_SelectedIndexChanged(object sender, EventArgs e)
        {
            button6.Enabled = false;
            button9.Enabled = true;
        }

        private void Kiadotttermek_lv_SelectedIndexChanged(object sender, EventArgs e)
        {
            button6.Enabled = true;
            button9.Enabled = false;
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            //HOZZÁADÁS GOMB
            if (Termekek_lv.SelectedItems.Count > 0 && vevoPartner != null && MegrendelesSzam_tbx.TextLength > 0 && MegrendelesSzam_tbx.Text != "Kötelező kitölteni!") {
                UjTermekFrm frm=null;
               
               
                    frm = new UjTermekFrm(ABKezelo.TermekKivalasztas(int.Parse(Termekek_lv.Items[Termekek_lv.SelectedIndices[0]].SubItems[0].Text)), "bevetelezes");
                
                UjTermekFrm.ujTermek = null;
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    button6.Enabled = false;
                    button9.Enabled = false;
                    int indexOfUjtermek = termekModListaVanAzonosito(UjTermekFrm.ujTermek.TermekAzonosito);
                    hozzaadottTermekek.Add(UjTermekFrm.ujTermek);
                    if (indexOfUjtermek > -1) {
                        termekModLista[indexOfUjtermek].Mennyiseg += UjTermekFrm.ujTermek.Mennyiseg;
                        termekModLista[indexOfUjtermek].NettoAr = UjTermekFrm.ujTermek.NettoAr;
                        termekModLista[indexOfUjtermek].BruttoAr = UjTermekFrm.ujTermek.BruttoAr;
                        Termek tmp = ABKezelo.TermekKivalasztas(termekModLista[indexOfUjtermek].TermekAzonosito);
                        tmp.Mennyiseg += UjTermekFrm.ujTermek.Mennyiseg;
                        ABKezelo.TermekModositas(tmp);
                        bevettermekekLVFeltolt();
                        ABKezelo.TermekLVFeltoltes(Termekek_lv);
                        Termekek_lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

                        if (vevoPartner != null && KiadottTermek.ArSzamito(termekModLista, vevoPartner.Kedvezmeny) != -1)
                        {
                            Osszeg_tbx.Text = KiadottTermek.ArSzamito(termekModLista, vevoPartner.Kedvezmeny).ToString();

                        }
                        else if (KiadottTermek.ArSzamito(termekModLista, 0) != -1)
                        {
                            Osszeg_tbx.Text = KiadottTermek.ArSzamito(termekModLista, 0).ToString();
                        }
                        else
                        {
                            tmp.Mennyiseg += UjTermekFrm.ujTermek.Mennyiseg;
                            ABKezelo.TermekModositas(tmp);
                            bevettermekekLVFeltolt();
                            ABKezelo.TermekLVFeltoltes(Termekek_lv);
                        }
                    } else {
                        termekModLista.Add(UjTermekFrm.ujTermek);
                        Termek tmp = ABKezelo.TermekKivalasztas(UjTermekFrm.ujTermek.TermekAzonosito);
                        tmp.Mennyiseg += UjTermekFrm.ujTermek.Mennyiseg;
                        ABKezelo.TermekModositas(tmp);
                        bevettermekekLVFeltolt();
                        ABKezelo.TermekLVFeltoltes(Termekek_lv);
                        Termekek_lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                        if (vevoPartner != null && KiadottTermek.ArSzamito(termekModLista, vevoPartner.Kedvezmeny) != -1)
                        {
                            Osszeg_tbx.Text = KiadottTermek.ArSzamito(termekModLista, vevoPartner.Kedvezmeny).ToString();

                        }
                        else if (KiadottTermek.ArSzamito(termekModLista, 0) != -1)
                        {
                            Osszeg_tbx.Text = KiadottTermek.ArSzamito(termekModLista, 0).ToString();
                        }
                        else
                        {
                            /*tmp.Mennyiseg += UjTermekFrm.ujTermek.Mennyiseg;
                            ABKezelo.TermekModositas(tmp);
                            bevettermekekLVFeltolt();
                            ABKezelo.TermekLVFeltoltes(Termekek_lv);*/
                        }
                    }
                }
            }
            else if (Termekek_lv.SelectedItems.Count == 0)
            {
                MessageBox.Show("Termék hozzáadás előtt kérjük jelöljön ki egy elemet a listából!", "figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("Termék hozzáadása előtt minden mező kitöltése kötelező!", "figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
           
          
        }

        private void termekekVisszaallito() {
            Termek tmp;
            if (toroltTermekek.Count > 0) {
                for (int i = 0; i < toroltTermekek.Count; i++) {
                    tmp = ABKezelo.TermekKivalasztas(toroltTermekek[i].TermekAzonosito);
                    tmp.Mennyiseg += toroltTermekek[i].Mennyiseg;
                    ABKezelo.TermekModositas(tmp);
                }
            }
            if (hozzaadottTermekek.Count > 0) {
                for (int i = 0; i < hozzaadottTermekek.Count; i++) {
                    tmp = ABKezelo.TermekKivalasztas(hozzaadottTermekek[i].TermekAzonosito);
                    tmp.Mennyiseg -= hozzaadottTermekek[i].Mennyiseg;
                    ABKezelo.TermekModositas(tmp);
                }
            }
        }

        private int termekModListaVanAzonosito(int azonositoBemenet) {
            for (int i = 0; i < termekModLista.Count; i++) if (termekModLista[i].TermekAzonosito == azonositoBemenet) return i;
            return -1;
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            //ELTÁVOLÍTÁS GOMB
            if (Kiadotttermek_lv.SelectedItems.Count > 0 )
            {
                int indexToRemove = termekModListaVanAzonosito(int.Parse(Kiadotttermek_lv.Items[Kiadotttermek_lv.SelectedIndices[0]].SubItems[0].Text));
                if (indexToRemove > -1) 
                {
                    Termek tmp = ABKezelo.TermekKivalasztas(termekModLista[indexToRemove].TermekAzonosito);
                   // MessageBox.Show(termekModLista[indexToRemove].Mennyiseg.ToString());
                    if (!ABKezelo.TermekMennyisegBevetelezes(termekModLista[indexToRemove]))
                       
                    {
                      
                        toroltTermekek.Add(termekModLista[indexToRemove]);
                        tmp.Mennyiseg -= termekModLista[indexToRemove].Mennyiseg;
                        ABKezelo.TermekModositas(tmp);
                        termekModLista.RemoveAt(indexToRemove);
                        bevettermekekLVFeltolt();
                        ABKezelo.TermekLVFeltoltes(Termekek_lv);
                        Termekek_lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                        Osszeg_tbx.Text = BevettTermek.ArSzamito(termekModLista, kedvezmeny).ToString();
                    }
                    else
                    {
                        MessageBox.Show("A termék nem törölhető, mert nincs belőle elég raktárkészleten!","Figyelem!",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("A kiadott termékek lista nem lehet üres!", "Figyelem", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
           

        }

        private void OK_Click(object sender, EventArgs e)
        {
            if (vevoPartner != null && MegrendelesSzam_tbx.TextLength > 0 && Kiadotttermek_lv.Items.Count > 0)
            {
                try
                {
                    if (toroltTermekek.Count > 0 || hozzaadottTermekek.Count > 0)
                    {
                        
                        List<BevettTermek> bevettTermekek = new List<BevettTermek>();
                        if (modositas)
                        {
                            //MODOSÍTÁS MENTÉSE
                            ABKezelo.BevetTermekTorles(bev.BevetId);
                            for (int i = 0; i < termekModLista.Count; i++) bevettTermekek.Add(new BevettTermek(bev.BevetId, termekModLista[i].TermekAzonosito, termekModLista[i].Mennyiseg, termekModLista[i].BruttoAr));
                            for (int i = 0; i < bevettTermekek.Count; i++) ABKezelo.BevetTermekFelvitel(bevettTermekek[i]);
                            mentes = true;
                        }
                        else
                        {
                            //ÚJ MENTÉSE
                            if (ABKezelo.BevetDuplikacioBizonylatAlapjan(Convert.ToInt32(MegrendelesSzam_tbx.Text)) == -1)
                            {
                                bev = new Bevetelezes(Convert.ToInt32(vevoPartner.PartnerAzonosito), Convert.ToDateTime(Bizonylatkelte_dtp.Value), Convert.ToInt32(MegrendelesSzam_tbx.Text), Convert.ToInt32(Osszeg_tbx.Text));
                                ABKezelo.BevetBizonylatFelvitel(bev);
                                for (int i = 0; i < termekModLista.Count; i++) bevettTermekek.Add(new BevettTermek(ABKezelo.BevetBizonylatIDLekerdezes(), termekModLista[i].TermekAzonosito, termekModLista[i].Mennyiseg, termekModLista[i].BruttoAr));
                                for (int i = 0; i < bevettTermekek.Count; i++) ABKezelo.BevetTermekFelvitel(bevettTermekek[i]);
                                mentes = true;
                            }
                            else
                            {
                                MessageBox.Show("A bevételezés megrendelésszáma már szerepel az adatbázisban!", "Figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                this.DialogResult = DialogResult.None;
                            }

                        }
                    }
                }
                catch (Exception)
                {

                    MessageBox.Show("Az adaok hiányosan, vagy nem megfelelően lettek megadva!", "Figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.DialogResult = DialogResult.None;
                }
            }
            else
            {
                MessageBox.Show("Az adaok hiányosan, vagy nem megfelelően lettek megadva!", "Figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.None;
            }

        }

        private void MegrendelesSzam_tbx_Enter(object sender, EventArgs e)
        {
            if (MegrendelesSzam_tbx.Text == "Kötelező kitölteni!")
            {
                MegrendelesSzam_tbx.Text = "";
                MegrendelesSzam_tbx.ForeColor = Color.Black;
            }
        }

        private void MegrendelesSzam_tbx_Leave(object sender, EventArgs e)
        {
            if (MegrendelesSzam_tbx.Text == "")
            {
                MegrendelesSzam_tbx.Text = "Kötelező kitölteni!";
                MegrendelesSzam_tbx.ForeColor = Color.Gray;
            }
            KivetelKezeles.TextBoxSzamregex(errorProvider1, MegrendelesSzam_tbx);
        }

        private void EladoNev_tbx_MouseClick(object sender, MouseEventArgs e)
        {
            if (EladoNev_tbx.Text == "Kötelező kitölteni!")
            {
                PartnerBongeszesFrm frm = new PartnerBongeszesFrm();
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    if (PartnerBongeszesFrm.szallitoPartner != null)
                    {
                        EladoNev_tbx.Text = PartnerBongeszesFrm.szallitoPartner.VezetekNev;
                        EladoCim_tbx.Text = PartnerBongeszesFrm.szallitoPartner.Orszag;
                        EladoAdoSzam_tbx.Text = PartnerBongeszesFrm.szallitoPartner.AdoSzam.ToString();//MŰKÖDIK

                    }
                }
            }
        }

        private void EladoCim_tbx_MouseClick(object sender, MouseEventArgs e)
        {
            if (EladoCim_tbx.Text == "Kötelező kitölteni!")
            {
                PartnerBongeszesFrm frm = new PartnerBongeszesFrm();
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    if (PartnerBongeszesFrm.szallitoPartner != null)
                    {
                        EladoNev_tbx.Text = PartnerBongeszesFrm.szallitoPartner.VezetekNev;
                        EladoCim_tbx.Text = PartnerBongeszesFrm.szallitoPartner.Orszag;
                        EladoAdoSzam_tbx.Text = PartnerBongeszesFrm.szallitoPartner.AdoSzam.ToString();//MŰKÖDIK

                    }
                }
            }
        }

        private void EladoAdoSzam_tbx_MouseClick(object sender, MouseEventArgs e)
        {
            if (EladoAdoSzam_tbx.Text == "Kötelező kitölteni!")
            {
                PartnerBongeszesFrm frm = new PartnerBongeszesFrm();
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    if (PartnerBongeszesFrm.szallitoPartner != null)
                    {
                        EladoNev_tbx.Text = PartnerBongeszesFrm.szallitoPartner.VezetekNev;
                        EladoCim_tbx.Text = PartnerBongeszesFrm.szallitoPartner.Orszag;
                        EladoAdoSzam_tbx.Text = PartnerBongeszesFrm.szallitoPartner.AdoSzam.ToString();//MŰKÖDIK
                    }
                }
            }
        }

        private void Kedvezmeny_numud_ValueChanged(object sender, EventArgs e)
        {
            kedvezmeny = Kedvezmeny_numud.Value;
            Osszeg_tbx.Text = BevettTermek.ArSzamito(termekModLista, kedvezmeny).ToString();
        }

        private void kereses_tbx_TextChanged(object sender, EventArgs e)
        {
            /*
            if (kereses_tbx.Text.Length > 0) {
                Termekek_lv.Items.Clear();
                ABKezelo.termekekLVFeltoltes(ABKezelo.termekKereso(kereses_tbx.Text, (KeresesKategoria)Kategoria_cbx.SelectedIndex),Termekek_lv);
            } else {
                ABKezelo.TermekLVFeltoltes(Termekek_lv);
                Termekek_lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);//TERMÉKLV MÉRETEZÉS
            }
            */
            List<Termek> termekekList = ABKezelo.termekKereso(kereses_tbx.Text, (KeresesKategoria)Kategoria_cbx.SelectedIndex);
            if (termekekList.Count > 0) Termekek_lv.Items.Clear();
            for (int i = 0; i < termekekList.Count; i++) {
                ABKezelo.termekekLVFeltoltes(ABKezelo.termekKereso(kereses_tbx.Text, (KeresesKategoria)Kategoria_cbx.SelectedIndex), Termekek_lv);
                Termekek_lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }

            if (termekekList.Count == 0 && Termekek_lv.Items[0].Text != "Nincs találat!") {
                //Termekek_lv.Columns.Clear();
                ABKezelo.UresLVFeltoltesManualisan(Termekek_lv);
                //Termekek_lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
        }

        private void Kategoria_cbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (kereses_tbx.Text.Length > 0) {
                Termekek_lv.Items.Clear();
                ABKezelo.termekekLVFeltoltes(ABKezelo.termekKereso(kereses_tbx.Text, (KeresesKategoria)Kategoria_cbx.SelectedIndex),Termekek_lv);
            } else {
                ABKezelo.TermekLVFeltoltes(Termekek_lv);
                Termekek_lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);//TERMÉKLV MÉRETEZÉS
            }
        }

        private void UjBevetelezesFrm_FormClosing(object sender, FormClosingEventArgs e) {
            if(!mentes) termekekVisszaallito();
        }

        private void UjBevetelezesFrm_MouseMove(object sender, MouseEventArgs e) {
            Control ctrl = GetChildAtPoint(e.Location);
            if (ctrl != null)
            {
                string tipstring = "";
                if (ctrl == OK && !ctrl.Enabled)
                {
                    //OK GOMB
                    tipstring = toolTip1.GetToolTip(OK);
                    toolTip1.Show(tipstring, OK, OK.Width / 2, OK.Height / 2);
                }
                else if (ctrl == button9 && !ctrl.Enabled)
                {
                    //HOZZÁADÁS GOMB
                    tipstring = toolTip1.GetToolTip(button9);
                    toolTip1.Show(tipstring, button9, button9.Width / 2, button9.Height / 2);

                }
                else if (ctrl == button6 && !ctrl.Enabled)
                {
                    //ELTÁVOLÍTÁS GOMB
                    tipstring = toolTip1.GetToolTip(button6);
                    toolTip1.Show(tipstring, button6, button6.Width / 2, button6.Height / 2);

                }
                else if (ctrl == button1)
                {
                    tipstring = toolTip1.GetToolTip(button1);
                    toolTip1.Show(tipstring, button1, button1.Width / 2, button1.Height / 2);
                }
            }
            else
            {
                toolTip1.Hide(this);
            }
        }

        private void MegrendelesSzam_tbx_TextChanged(object sender, EventArgs e)
        {
            if (MegrendelesSzam_tbx.Text != "Kötelező kitölteni!")
            {
                KivetelKezeles.TextBoxSzamregex(errorProvider1, MegrendelesSzam_tbx);
            }
        }

        private void segitseg_btn_Click(object sender, EventArgs e)
        {
            Helpfrm help = new Helpfrm(KivetelKezeles.getHelp("Bevételezés menüpont"));
            help.ShowDialog();
        }

        private void MegrendelesSzam_tbx_KeyPress(object sender, KeyPressEventArgs e)
        {
            KivetelKezeles.StringLetiltoDecimal2(MegrendelesSzam_tbx, e);
        }
    }
}
