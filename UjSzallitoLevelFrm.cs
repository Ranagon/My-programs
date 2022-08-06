using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Szakdolgozat3 {
    internal partial class UjSzallitoLevelFrm : Form {
        string oldal = "";
        bool modositas = false;
        bool mentes = false;
        Partner cegPartner = ABKezelo.PartnerKivalasztasKategoriaAlapjan("1");
        Partner vevoPartner;
        SzallitoLevel szal;
        List<Termek> termekModLista = new List<Termek>();
        List<Termek> toroltTermekek = new List<Termek>();
        List<Termek> hozzaadottTermekek = new List<Termek>();

        public UjSzallitoLevelFrm(Partner partner, SzallitoLevel szallitoLevel, BindingList<KiadottTermek> lista, string adat) {
            InitializeComponent();
            szal = szallitoLevel;
            oldal = adat;
            vevoPartner = partner;
            EladoNev_tbx.ReadOnly = true;
            EladoCim_tbx.ReadOnly = true;
            EladoAdoSzam_tbx.ReadOnly = true;
            VevoNev_tbx.ReadOnly = true;
            VevoCim_tbx.ReadOnly = true;
            VevoAdoszam_tbx.ReadOnly = true;
            Osszeg_tbx.ReadOnly = true;
            Osszeg_tbx.Enter += (s, zs) => { kereses_tbx.Focus(); };
            VevoNev_tbx.Text = "Kötelező kitölteni!";
            VevoCim_tbx.Text = "Kötelező kitölteni!";
            VevoAdoszam_tbx.Text = "Kötelező kitölteni!";
            Hozzaadas.Enabled = false;
            button6.Enabled = false;
          //  OK.Enabled = false;
            ABKezelo.TermekLVFeltoltes(Termekek_lv);
            Termekek_lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);//TERMÉKLV MÉRETEZÉS

            if (cegPartner != null) {
                EladoNev_tbx.Text = cegPartner.VezetekNev + " " + cegPartner.KeresztNev;
                EladoCim_tbx.Text = cegPartner.Irsz + " " + cegPartner.Kozterulet + " " + cegPartner.KozteruletJelleg + " " + cegPartner.HazSzam + " " + cegPartner.EmeletAjto;
                EladoAdoSzam_tbx.Text = cegPartner.AdoSzam.ToString();
            }
            if (adat == "modosit") {
                
                this.Text = "Szállítólevél módosítása";
                modositas = true;
                vevoPartner = partner;
                VevoNev_tbx.Text = partner.VezetekNev + " " + partner.KeresztNev;
                VevoCim_tbx.Text = partner.Irsz + " " + partner.Kozterulet + " " + partner.KozteruletJelleg + " " + partner.HazSzam + " " + partner.EmeletAjto;
                VevoAdoszam_tbx.Text = partner.AdoSzam.ToString();
                MegrendelesSzam_tbx.Text = szallitoLevel.MegrendelesSzam.ToString();
                MegrendelesSzam_tbx.ReadOnly = true;
                VevoNev_tbx.Enter += (s, zs) => { Bizonylatkelte_dtp.Focus(); };
                VevoCim_tbx.Enter += (s, zs) => { Bizonylatkelte_dtp.Focus(); };
                VevoAdoszam_tbx.Enter += (s, zs) => { Bizonylatkelte_dtp.Focus(); };
                EladoNev_tbx.Enter += (s, zs) => { Bizonylatkelte_dtp.Focus(); };
                EladoCim_tbx.Enter += (s, zs) => { Bizonylatkelte_dtp.Focus(); };
                EladoAdoSzam_tbx.Enter += (s, zs) => { Bizonylatkelte_dtp.Focus(); };
                MegrendelesSzam_tbx.Enter += (s, zs) => { Bizonylatkelte_dtp.Focus(); };
                button1.Visible = false;
                kiadottTermekekLVFeltolt(lista);
                Osszeg_tbx.Text = KiadottTermek.ArSzamito(termekModLista, vevoPartner.Kedvezmeny).ToString();
            } else {
                this.Text = "Szállítólevél létrehozása";
                VevoNev_tbx.Enter += (s, zs) => { button1.Focus(); };
                VevoCim_tbx.Enter += (s, zs) => { button1.Focus(); };
                VevoAdoszam_tbx.Enter += (s, zs) => { button1.Focus(); };
                EladoNev_tbx.Enter += (s, zs) => { button1.Focus(); };
                EladoCim_tbx.Enter += (s, zs) => { button1.Focus(); };
                EladoAdoSzam_tbx.Enter += (s, zs) => { button1.Focus(); };
            }
            Termekek_lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);//TERMÉKLV MÉRETEZÉS

        }

        private void kiadottTermekekLVFeltolt(BindingList<KiadottTermek> kiadottTermekek = null) {
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
            if (kiadottTermekek != null && kiadottTermekek.Count > 0) {
                termekModLista.Clear();
                for (int i = 0; i < kiadottTermekek.Count; i++) {
                    Termek termek = kiadottTermekek[i].toTermek();
                    termekModLista.Add(termek);
                    string[] adatok = {
                   termek.TermekAzonosito.ToString(),termek.TermekKategoria.ToString(),termek.Megnevezes,termek.Mennyiseg.ToString(),termek.MennyisegEgysege,termek.BruttoAr.ToString(),
                   termek.NettoAr.ToString(),termek.AfaKulcs.ToString(),termek.NavSorszam,termek.Cikkszam,termek.MinimalisKeszlet.ToString()};
                    Kiadotttermek_lv.Items.Add(new ListViewItem(adatok));
                    Kiadotttermek_lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);//TERMÉKLV MÉRETEZÉS
                }
            } else {
                for (int i = 0; i < termekModLista.Count; i++) {
                    string[] adatok = {
                   termekModLista[i].TermekAzonosito.ToString(),termekModLista[i].TermekKategoria.ToString(),termekModLista[i].Megnevezes,termekModLista[i].Mennyiseg.ToString(),termekModLista[i].MennyisegEgysege,termekModLista[i].BruttoAr.ToString(),
                   termekModLista[i].NettoAr.ToString(),termekModLista[i].AfaKulcs.ToString(),termekModLista[i].NavSorszam,termekModLista[i].Cikkszam,termekModLista[i].MinimalisKeszlet.ToString()};
                    Kiadotttermek_lv.Items.Add(new ListViewItem(adatok));
                    Kiadotttermek_lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);//TERMÉKLV MÉRETEZÉS
                }
            }
        }

        private void termekekLVFeltoltes(List<Termek> bemenet) {
            Termekek_lv.Columns.Clear();
            Termekek_lv.Items.Clear();
            Termekek_lv.View = View.Details;
            if (Termekek_lv.Columns.Count == 0) {
                Termekek_lv.Columns.Add("Termékazonosító");
                Termekek_lv.Columns.Add("Termékkategória");
                Termekek_lv.Columns.Add("Megnevezés");
                Termekek_lv.Columns.Add("Mennyíség");
                Termekek_lv.Columns.Add("Mennyíség egysége");
                Termekek_lv.Columns.Add("Netto ár");
                Termekek_lv.Columns.Add("Brutto ár");
                Termekek_lv.Columns.Add("Áfa kulcs");
                Termekek_lv.Columns.Add("NAV sorszám");
                Termekek_lv.Columns.Add("Cikkszám");
                Termekek_lv.Columns.Add("Minimális készlet");
            }
            for (int i = 0; i < bemenet.Count; i++) {
                string[] adatok = {
                bemenet[i].TermekAzonosito.ToString(),bemenet[i].TermekKategoria.ToString(),bemenet[i].Megnevezes,bemenet[i].Mennyiseg.ToString(),bemenet[i].MennyisegEgysege,bemenet[i].BruttoAr.ToString(),
                bemenet[i].NettoAr.ToString(),bemenet[i].AfaKulcs.ToString(),bemenet[i].NavSorszam,bemenet[i].Cikkszam,bemenet[i].MinimalisKeszlet.ToString()};
                Termekek_lv.Items.Add(new ListViewItem(adatok));
                Termekek_lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);//TERMÉKLV MÉRETEZÉS

            }
            TermekekFrm.HianyListaJeloles(Termekek_lv, null, bemenet);
          

        }

        private void UjSzallitoLevelFrm_Load(object sender, EventArgs e) {
            this.BackColor = SystemColors.GradientInactiveCaption;
            Kategoria_cbx.DataSource = Enum.GetValues(typeof(KeresesKategoria));
            if (MegrendelesSzam_tbx.Text == "") {
                MegrendelesSzam_tbx.Text = "Kötelező kitölteni!";
                MegrendelesSzam_tbx.ForeColor = Color.Gray;
            }
       
            toolTip1.SetToolTip(OK, "A gomb a szállítólevél\r\nmaradéktalan kitöltése\r\nesetén aktiválódik.");
            toolTip1.SetToolTip(Hozzaadas, "Kérjük, jelöljön\r\nki egy terméket a\r\nterméklistából!");
            toolTip1.SetToolTip(button6, "Kérjük, jelöljön\r\nki egy terméket a\r\nKiadott termékekből!");
            toolTip1.SetToolTip(button1, "Partner adatok\r\nhozzáadása,\r\nmódosítása.");
        }


        private void HozzaAdas(object sender, EventArgs e) {

                if (Termekek_lv.SelectedItems.Count > 0 && vevoPartner != null && MegrendelesSzam_tbx.TextLength > 0 && MegrendelesSzam_tbx.Text != "Kötelező kitölteni!")
                {
                    ListViewItem item = Termekek_lv.Items[Termekek_lv.SelectedIndices[0]];
                    UjTermekFrm frm = new UjTermekFrm(ABKezelo.TermekKivalasztas(int.Parse(Termekek_lv.Items[Termekek_lv.SelectedIndices[0]].SubItems[0].Text)), "bevetelezes"); ;

                    //frm = new UjTermekFrm(ABKezelo.TermekKivalasztas(int.Parse(Termekek_lv.Items[Termekek_lv.SelectedIndices[0]].SubItems[0].Text)), "bevetelezes");//A Termék LV-ban kiválasztott string tömb 0 eleme alapján kiválasztja az adatbázisból a kijelölt "Terméket
                                                                                                                                                                    //és azt adja be paraméterül a megidézendő UjTermekFrm konstruktorának , melyben elvégződik az adatok beillesztése a paraméterül kapott "bevetelezés" string vonathozásában.
                    UjTermekFrm.ujTermek = null;
                    if (int.Parse(item.SubItems[3].Text) != 0)
                    {

                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            button6.Enabled = false;
                            Hozzaadas.Enabled = false;
                            //  try
                            //{
                            if (int.Parse(item.SubItems[3].Text) > UjTermekFrm.ujTermek.Mennyiseg || int.Parse(item.SubItems[3].Text) == UjTermekFrm.ujTermek.Mennyiseg)
                            {
                                int indexOfUjtermek = termekModListaVanAzonosito(UjTermekFrm.ujTermek.TermekAzonosito);//megnézi, hogy a termekModLista-ban benne van e a kiválasztott termék termékazonosítója
                                hozzaadottTermekek.Add(UjTermekFrm.ujTermek);
                                if (indexOfUjtermek > -1)//ha igen....
                                {
                                    termekModLista[indexOfUjtermek].Mennyiseg += UjTermekFrm.ujTermek.Mennyiseg;//hozzáadja a listában talált termék darabszámához a kiválasztott termék darabszámát.
                                    termekModLista[indexOfUjtermek].NettoAr = UjTermekFrm.ujTermek.NettoAr;
                                    termekModLista[indexOfUjtermek].BruttoAr = UjTermekFrm.ujTermek.BruttoAr;
                                    Termek tmp = ABKezelo.TermekKivalasztas(termekModLista[indexOfUjtermek].TermekAzonosito);//lekérdezi az adatbázisból a terméket az azonosítója alapján(új termék példány)
                                    tmp.Mennyiseg -= UjTermekFrm.ujTermek.Mennyiseg;//A lekérdezett termék mennyíségéból kivonja a kiválasztott termék mennyíségét
                                    
                                    ABKezelo.TermekModositas(tmp);//A kapott eredménnyel updateli a terméket az adatbázisban.
                                    kiadottTermekekLVFeltolt();//frissíti a kiadott termékek LV-t a termekModLista adataival
                                    ABKezelo.TermekLVFeltoltes(Termekek_lv);//Frissíti a TermékekLV-t az adatbázisból
                                    Termekek_lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);//TERMÉKLV MÉRETEZÉS

                                    Osszeg_tbx.Text = KiadottTermek.ArSzamito(termekModLista, vevoPartner.Kedvezmeny).ToString();
                                }
                                else
                                {
                                    termekModLista.Add(UjTermekFrm.ujTermek);//Ebben az esetben, mivel nincs a listában az elem, a kiválasztott terméket hozzáadja a listához. A metodika többi része megegyezik a fentebb lévő műveletekkel.
                                    Termek tmp = ABKezelo.TermekKivalasztas(UjTermekFrm.ujTermek.TermekAzonosito);
                                    tmp.Mennyiseg -= UjTermekFrm.ujTermek.Mennyiseg;
                                    ABKezelo.TermekModositas(tmp);
                                    kiadottTermekekLVFeltolt();
                                    ABKezelo.TermekLVFeltoltes(Termekek_lv);
                                    Termekek_lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);//TERMÉKLV MÉRETEZÉS
                                    if (vevoPartner != null && KiadottTermek.ArSzamito(termekModLista, vevoPartner.Kedvezmeny) != -1)
                                    {
                                        Osszeg_tbx.Text = KiadottTermek.ArSzamito(termekModLista, vevoPartner.Kedvezmeny).ToString();//kiszámolja partnerkedvezmény alapján a teljes összeget.

                                    }
                                    else if (KiadottTermek.ArSzamito(termekModLista, 0) != -1)
                                    {
                                        Osszeg_tbx.Text = KiadottTermek.ArSzamito(termekModLista, 0).ToString();//ha nincs partner, a kedvezmény nélküli összeget számolja ki.
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("A kiválasztott termékből nincs elég raktáron!", "Készlet hiány!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("A kiválasztott termékből nincs elég raktáron!", "Készlet hiány!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private int termekModListaVanAzonosito(int azonositoBemenet) {
            for (int i = 0; i < termekModLista.Count; i++) if (termekModLista[i].TermekAzonosito == azonositoBemenet) return i;
            return -1;
        }

        private void Eltavolitas(object sender, EventArgs e) {
            //ELTÁVOLÍTÁS GOMB
            if (Kiadotttermek_lv.SelectedItems.Count > 0) {
                int indexToRemove = termekModListaVanAzonosito(int.Parse(Kiadotttermek_lv.Items[Kiadotttermek_lv.SelectedIndices[0]].SubItems[0].Text));
                if (indexToRemove > -1) {
                    toroltTermekek.Add(termekModLista[indexToRemove]);
                    Termek tmp = ABKezelo.TermekKivalasztas(termekModLista[indexToRemove].TermekAzonosito);
                    tmp.Mennyiseg += termekModLista[indexToRemove].Mennyiseg;
                    ABKezelo.TermekModositas(tmp);
                    termekModLista.RemoveAt(indexToRemove);
                    kiadottTermekekLVFeltolt();

                    ABKezelo.TermekLVFeltoltes(Termekek_lv);
                    Termekek_lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                    if (PartnerBongeszesFrm.szallitoPartner != null)
                    {
                        Osszeg_tbx.Text = KiadottTermek.ArSzamito(termekModLista, vevoPartner.Kedvezmeny).ToString();

                    }
                    else
                    {
                        Osszeg_tbx.Text = KiadottTermek.ArSzamito(termekModLista, 1).ToString();
                    }
                }
            }
            else
            {
                MessageBox.Show("A kiadott termékek lista nem lehet üres!", "Figyelem", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }


        private void termekekVisszaallito() {//VISSZAÁLLÍTJA A TERMÉKEK TÁBLÁBAN AZ ADATOKAT AZ ujSzallitoLevelFRM megidézése előtti állapotra FORMCLOSING EVENTNÉL
            Termek tmp;
            if (toroltTermekek.Count > 0) {
                for (int i = 0; i < toroltTermekek.Count; i++) {
                    tmp = ABKezelo.TermekKivalasztas(toroltTermekek[i].TermekAzonosito);
                    tmp.Mennyiseg -= toroltTermekek[i].Mennyiseg;
                    ABKezelo.TermekModositas(tmp);
                }
            }
            if (hozzaadottTermekek.Count > 0) {
                for (int i = 0; i < hozzaadottTermekek.Count; i++) {
                    tmp = ABKezelo.TermekKivalasztas(hozzaadottTermekek[i].TermekAzonosito);
                    tmp.Mennyiseg += hozzaadottTermekek[i].Mennyiseg;
                    ABKezelo.TermekModositas(tmp);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            PartnerBongeszesFrm.szallitoPartner = null;
            PartnerBongeszesFrm frm = new PartnerBongeszesFrm();
            if (frm.ShowDialog() == DialogResult.OK) {
               
                if (PartnerBongeszesFrm.szallitoPartner != null) {
                    VevoNev_tbx.Text = PartnerBongeszesFrm.szallitoPartner.VezetekNev;
                    VevoCim_tbx.Text = PartnerBongeszesFrm.szallitoPartner.Orszag;
                    VevoAdoszam_tbx.Text = PartnerBongeszesFrm.szallitoPartner.AdoSzam.ToString();//MŰKÖDIK
                    vevoPartner = PartnerBongeszesFrm.szallitoPartner;
                    if (Kiadotttermek_lv.Items.Count>0)
                    {
                        Osszeg_tbx.Text = KiadottTermek.ArSzamito(termekModLista, vevoPartner.Kedvezmeny).ToString();
                    }
                    

                }

            }
        }

        private void UjSzallitoLevelFrm_FormClosing(object sender, FormClosingEventArgs e) {
            PartnerBongeszesFrm.szallitoPartner = null;//kiüríti a statikus változót.
            if (!mentes ) termekekVisszaallito();//Azért van szükség a toroltTermekek és a hozzaadottTermekek listákra, mert ezek alapján állítja vissza az adatbázist a program. Ha egyik-másik nem üres, a bennük lévő értéket adja hozzá, vagy veszi el updatelés esetén a program.
            //Törölt esetén hozzáad, hozzáad esetén elvesz. A listában tárolt Termékazonosító alapján updateli a megfelelő termékek mennyíségét az adatbázisban.
        }

        private void Termekek_lv_SelectedIndexChanged(object sender, EventArgs e) {
            Hozzaadas.Enabled = true;
            button6.Enabled = false;
        }

        private void Kiadotttermek_lv_SelectedIndexChanged(object sender, EventArgs e)//itt kezem le a hozzáadás/eltávolítás gombot, rossz, nem megy a gombos rész, ki kéne venni.
        {
            button6.Enabled = true;
            Hozzaadas.Enabled = false;
        }

        private void VevoNev_tbx_MouseClick(object sender, MouseEventArgs e) {//ITT NYITJA MEG,MAJD KÉRI BE A A PartnerBongeszesFrm-ben kiválasztott partner adatait.
                                                                                //Bármelyik mezőre s kattint a felhasználó, megnyílik a partnerBongeszesFrm. Ez felhasználó segítő opció akar lenni.
            if (VevoNev_tbx.Text == "Kötelező kitölteni!") {
                PartnerBongeszesFrm frm = new PartnerBongeszesFrm();
                if (frm.ShowDialog() == DialogResult.OK) {
                    if (PartnerBongeszesFrm.szallitoPartner != null) {
                        VevoNev_tbx.Text = PartnerBongeszesFrm.szallitoPartner.VezetekNev;
                        VevoCim_tbx.Text = PartnerBongeszesFrm.szallitoPartner.Orszag;
                        VevoAdoszam_tbx.Text = PartnerBongeszesFrm.szallitoPartner.AdoSzam.ToString();//MŰKÖDIK
                    }
                }
            }
        }

        private void VevoCim_tbx_MouseClick(object sender, MouseEventArgs e) {
            if (VevoCim_tbx.Text == "Kötelező kitölteni!") {
                PartnerBongeszesFrm frm = new PartnerBongeszesFrm();
                if (frm.ShowDialog() == DialogResult.OK) {
                    if (PartnerBongeszesFrm.szallitoPartner != null) {
                        VevoNev_tbx.Text = PartnerBongeszesFrm.szallitoPartner.VezetekNev;
                        VevoCim_tbx.Text = PartnerBongeszesFrm.szallitoPartner.Orszag;
                        VevoAdoszam_tbx.Text = PartnerBongeszesFrm.szallitoPartner.AdoSzam.ToString();

                    }
                }
            }
        }

        private void VevoAdoszam_tbx_MouseClick(object sender, MouseEventArgs e) {
            if (VevoAdoszam_tbx.Text == "Kötelező kitölteni!") {
                PartnerBongeszesFrm frm = new PartnerBongeszesFrm();
                if (frm.ShowDialog() == DialogResult.OK) {
                    if (PartnerBongeszesFrm.szallitoPartner != null) {
                        VevoNev_tbx.Text = PartnerBongeszesFrm.szallitoPartner.VezetekNev;
                        VevoCim_tbx.Text = PartnerBongeszesFrm.szallitoPartner.Orszag;
                        VevoAdoszam_tbx.Text = PartnerBongeszesFrm.szallitoPartner.AdoSzam.ToString();

                    }
                }
            }
        }

        private void MegrendelesSzam_tbx_Enter(object sender, EventArgs e) {
            if (MegrendelesSzam_tbx.Text == "Kötelező kitölteni!") {
                MegrendelesSzam_tbx.Text = "";
                MegrendelesSzam_tbx.ForeColor = Color.Black;
            }
        }

        private void MegrendelesSzam_tbx_Leave(object sender, EventArgs e) {
            if (MegrendelesSzam_tbx.Text == "") {
                MegrendelesSzam_tbx.Text = "Kötelező kitölteni!";
                MegrendelesSzam_tbx.ForeColor = Color.Gray;

            }KivetelKezeles.TextBoxSzamregex(errorProvider1, MegrendelesSzam_tbx);
        }

        private void kereses_tbx_TextChanged(object sender, EventArgs e) {
            List<Termek> termekekList = ABKezelo.termekKereso(kereses_tbx.Text, (KeresesKategoria)Kategoria_cbx.SelectedIndex);
            if(termekekList.Count>0)Termekek_lv.Items.Clear();
            for (int i = 0; i < termekekList.Count; i++)
            {
                ABKezelo.termekekLVFeltoltes(ABKezelo.termekKereso(kereses_tbx.Text, (KeresesKategoria)Kategoria_cbx.SelectedIndex), Termekek_lv);
                Termekek_lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
          
            if (termekekList.Count == 0 && Termekek_lv.Items[0].Text != "Nincs találat!")
            {
                
                ABKezelo.UresLVFeltoltesManualisan(Termekek_lv);
                
            }
        }

        private void Kategoria_cbx_SelectedIndexChanged(object sender, EventArgs e) {
            List<Termek> termekekList = ABKezelo.termekKereso(kereses_tbx.Text, (KeresesKategoria)Kategoria_cbx.SelectedIndex);
            Termekek_lv.Items.Clear();
            for (int i = 0; i < termekekList.Count; i++)
            {

                ABKezelo.termekekLVFeltoltes(ABKezelo.termekKereso(kereses_tbx.Text, (KeresesKategoria)Kategoria_cbx.SelectedIndex), Termekek_lv);
                Termekek_lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
         
            if (termekekList.Count == 0)
            {
                Termekek_lv.Columns.Clear();
                ABKezelo.UresLVFeltoltesManualisan(Termekek_lv);
                Termekek_lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
        }

        private void MegrendelesSzam_tbx_Validating(object sender, CancelEventArgs e) {
            
        }

       

        private void OK_Click(object sender, EventArgs e) {
            if (vevoPartner!=null && MegrendelesSzam_tbx.TextLength>0 && Kiadotttermek_lv.Items.Count>0)
            {

                try
                {
                    if (toroltTermekek.Count > 0 || hozzaadottTermekek.Count > 0)
                    {
                        
                        List<KiadottTermek> kiadottTermekek = new List<KiadottTermek>();
                        if (modositas)//MÓDOSÍTÁS
                        {

                            ABKezelo.KiadottTermekTorles(szal.SzallitoId);
                            for (int i = 0; i < termekModLista.Count; i++) kiadottTermekek.Add(new KiadottTermek(szal.SzallitoId, termekModLista[i].TermekAzonosito, termekModLista[i].Mennyiseg, termekModLista[i].BruttoAr));
                            for (int i = 0; i < kiadottTermekek.Count; i++) ABKezelo.KiadottTermekFelvitel(kiadottTermekek[i]);
                            mentes = true;
                        }
                        else
                        {
                            if (ABKezelo.SzallitoLevelDuplikacioBizonylatAlapjan(Convert.ToInt32(MegrendelesSzam_tbx.Text)) == -1)
                            {
                                szal = new SzallitoLevel(Convert.ToInt32(PartnerBongeszesFrm.szallitoPartner.PartnerAzonosito), Convert.ToDateTime(Bizonylatkelte_dtp.Value), Convert.ToInt32(MegrendelesSzam_tbx.Text), Convert.ToInt32(Osszeg_tbx.Text));
                                ABKezelo.SzallitoLevelFelvitel(szal);
                                for (int i = 0; i < termekModLista.Count; i++) kiadottTermekek.Add(new KiadottTermek(ABKezelo.SzallitoLevelIDLekerdezes(), termekModLista[i].TermekAzonosito, termekModLista[i].Mennyiseg, termekModLista[i].BruttoAr));
                                for (int i = 0; i < kiadottTermekek.Count; i++) ABKezelo.KiadottTermekFelvitel(kiadottTermekek[i]);
                                mentes = true;
                            }
                            else
                            {
                                MessageBox.Show("A szállítólevél megrendelésszáma már szerepel az adatbázisban!","Figyelem!",MessageBoxButtons.OK,MessageBoxIcon.Warning);
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
                MessageBox.Show("Az adaok hiányosan, vagy nem megfelelően lettek megadva!", "Figyelem!", MessageBoxButtons.OK,MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.None;
                
            }
           
        }

        private void MegrendelesSzam_tbx_TextChanged(object sender, EventArgs e)
        {
            if (MegrendelesSzam_tbx.Text != "Kötelező kitölteni!")
            {
                KivetelKezeles.TextBoxSzamregex(errorProvider1, MegrendelesSzam_tbx);
            }
        }

        private void UjSzallitoLevelFrm_MouseMove(object sender, MouseEventArgs e)
        {
            
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
                else if (ctrl == Hozzaadas && !ctrl.Enabled)
                {
                    //HOZZÁADÁS GOMB
                    tipstring = toolTip1.GetToolTip(Hozzaadas);
                    toolTip1.Show(tipstring, Hozzaadas, Hozzaadas.Width / 2, Hozzaadas.Height / 2);

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

   

  

        

      

        private void Termekek_lv_Click(object sender, EventArgs e)
        {
           // button9.Enabled = true;
            //button6.Enabled = false;
        }

        private void Termekek_lv_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (Termekek_lv.SelectedItems.Count > 0)
            {
                Hozzaadas.Enabled = true;
            }
            else
            {
                Hozzaadas.Enabled = false;
            }
        }

        private void segitseg_btn_Click(object sender, EventArgs e)
        {
            Helpfrm help = new Helpfrm(KivetelKezeles.getHelp("Szállítólevél menüpont"));
            help.ShowDialog();
        }

        private void Megse_Click(object sender, EventArgs e)
        {

        }

        private void MegrendelesSzam_tbx_KeyPress(object sender, KeyPressEventArgs e)
        {
            KivetelKezeles.StringLetiltoDecimal2(MegrendelesSzam_tbx,e);
        }
    }
}

