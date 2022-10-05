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
    public partial class UjTermekFrm : Form
    {
        #region Fieldek
       
        bool modositas=false;
        public static Termek ujTermek;
        internal static string oldalak;
        internal static int mennyiseg;
        private TermekekFrm termekFrm;
            
        BindingList<Termek.AFA> afak = new BindingList<Termek.AFA>(ABKezelo.AfaKategoriakBetoltes());
        BindingList<sqlString> mertekEgyseg = new BindingList<sqlString>(ABKezelo.TermekMennyisegEgysegBetoltes());
        BindingList<sqlString> termekKategoriak = new BindingList<sqlString>(ABKezelo.TermekKategoriakBetoltes());
        #endregion

        #region Konstruktor
        public UjTermekFrm(Termek termek, string oldal,TermekekFrm termekekForm=null)
        {
            termekFrm = termekekForm;
            oldalak = oldal;
            InitializeComponent();
            ActiveControl = null;
            termekKategoria_cbox.DataSource = termekKategoriak;
            mennyisegEgysege_cbx.DataSource = mertekEgyseg;
            AFAKulcs_cbox.Items.Clear();
            for (int i = 0; i < afak.Count; i++) AFAKulcs_cbox.Items.Add(afak[i].Value);
            if (AFAKulcs_cbox.Items.Count > 0) AFAKulcs_cbox.SelectedIndex = 0;



            if (termek!=null&& oldal=="modosit")
            {
                this.Text = "Termék módósítása";  
                modositas = true;             
                termekKod_tbx.Text = termek.TermekAzonosito.ToString();
                termekKod_tbx.ReadOnly = true;
                megnevezes_txb.Text = termek.Megnevezes.ToString();
                mennyiseg_tbx.Text = termek.Mennyiseg.ToString();
                mennyisegEgysege_cbx.Enabled = false;
                termekKategoria_cbox.Enabled = false;
                mennyisegEgysegModositas_btn.Visible = false;
                kategoriaModositas_btn.Visible = false;
                NavSzam_tbx.Enabled = false;
                CikkSzam_tbx.Enabled = false;
                mennyiseg_tbx.Enabled = false;
                megnevezes_txb.Enabled = false;
                termekKod_tbx.Enabled = false;
                termekKod_tbx.Enter += (s, zs) => { NettoAr_tbx.Focus(); };
                megnevezes_txb.Enter += (s, zs) => { NettoAr_tbx.Focus(); };
                CikkSzam_tbx.Enter += (s, zs) => { NettoAr_tbx.Focus(); };
                mennyiseg_tbx.Enter += (s, zs) => { NettoAr_tbx.Focus(); };
                NavSzam_tbx.Enter += (s, zs) => { NettoAr_tbx.Focus(); };
                CikkSzam_tbx.Enter += (s, zs) => { NettoAr_tbx.Focus(); };
                termekKategoria_cbox.Enter += (s, zs) => { NettoAr_tbx.Focus(); };
                mennyisegEgysege_cbx.Enter += (s, zs) => { NettoAr_tbx.Focus(); };
                CikkSzam_tbx.Text = termek.Cikkszam;
                NavSzam_tbx.Text = termek.NavSorszam;

                termekKategoria_cbox.DataSource = ABKezelo.TermekKategoriakBetoltes();
                mennyisegEgysege_cbx.DataSource = ABKezelo.TermekMennyisegEgysegBetoltes(); 
                AFAKulcs_cbox.Items.Clear();
                for (int i = 0; i < afak.Count; i++) AFAKulcs_cbox.Items.Add(afak[i].Value);
                if (AFAKulcs_cbox.Items.Count > 0) AFAKulcs_cbox.SelectedIndex = 0;

                termekKategoria_cbox.SelectedIndex = termekKategoria_cbox.FindStringExact(termek.TermekKategoria.ToString());
                mennyisegEgysege_cbx.SelectedIndex = mennyisegEgysege_cbx.FindStringExact(termek.MennyisegEgysege.ToString());
                AFAKulcs_cbox.SelectedIndex = AFAKulcs_cbox.FindStringExact(termek.AfaKulcs.ToString());
              
                foreach (sqlString item in ABKezelo.TermekMennyisegEgysegBetoltes())
                {
                    if (item.Value == termek.MennyisegEgysege.ToString())
                    {
                        mennyisegEgysege_cbx.SelectedItem= item.Value;//A HIBA AZ,hogyha törlöm a nemmyioség egységét, nem találja meg és nem ír ki xart se, Kell egy garbadge collector
                    }
                }

                for (int i = 0; i < afak.Count; i++) {
                    if(afak[i].Value == termek.AfaKulcs) {
                        AFAKulcs_cbox.SelectedIndex = AFAKulcs_cbox.FindStringExact(afak[i].Value.ToString());
                        break;
                    }
                }
                AFAKulcs_cbox.SelectedIndexChanged += new EventHandler(this.AFAKulcs_cbox_SelectedIndexChanged);            
             //   mennyiseg_tbx.Text = termek.Mennyiseg.ToString();
                mennyiseg_tbx.ReadOnly = true;
                NettoAr_tbx.Text = termek.NettoAr.ToString();
                if (NettoAr_tbx.Text!=null && AFAKulcs_cbox.SelectedItem!=null)
                {
                    BruttoAr_tbx.Text = Termek.BruttoArSzamito(Convert.ToDecimal(NettoAr_tbx.Text), afak[AFAKulcs_cbox.SelectedIndex].Value).ToString();

                }             
                BruttoAr_tbx.ReadOnly = true;             
                minimalisKeszlet_tbx.Text = termek.MinimalisKeszlet.ToString();
                BruttoAr_tbx.Enabled = false;
                BruttoAr_tbx.Enter += (s, zs) => { NavSzam_tbx.Focus(); };
                aktualisKeszlet_tbx.Text = termek.Mennyiseg.ToString();
                aktualisKeszlet_tbx.ReadOnly = true;
                aktualisKeszlet_tbx.Enter += (s, f) => { minimalisKeszlet_tbx.Focus(); };
                             
            }
          
            else if(termek != null && oldal == "bevetelezes")
            {
                this.Text = "Termék hozzáadása";
                termekKod_tbx.Text = termek.TermekAzonosito.ToString();
                termekKod_tbx.ReadOnly = true;
                megnevezes_txb.Text = termek.Megnevezes.ToString();

                mennyisegEgysege_cbx.Enabled = false;
                termekKategoria_cbox.Enabled = false;
                mennyisegEgysegModositas_btn.Visible = false;
                kategoriaModositas_btn.Visible = false;
                NavSzam_tbx.Enabled = false;
                CikkSzam_tbx.Enabled = false;              
                megnevezes_txb.Enabled = false;
                termekKod_tbx.Enabled = false;
                termekKod_tbx.Enter += (s, zs) => { NettoAr_tbx.Focus(); };
                megnevezes_txb.Enter += (s, zs) => { NettoAr_tbx.Focus(); };
                CikkSzam_tbx.Enter += (s, zs) => { NettoAr_tbx.Focus(); };               
                NavSzam_tbx.Enter += (s, zs) => { NettoAr_tbx.Focus(); };
                CikkSzam_tbx.Enter += (s, zs) => { NettoAr_tbx.Focus(); };
                termekKategoria_cbox.Enter += (s, zs) => { NettoAr_tbx.Focus(); };
                mennyisegEgysege_cbx.Enter += (s, zs) => { NettoAr_tbx.Focus(); };
                CikkSzam_tbx.Text = termek.Cikkszam;
                NavSzam_tbx.Text = termek.NavSorszam;
                mennyiseg_tbx.Text = termek.Mennyiseg.ToString();
                NettoAr_tbx.Text = termek.NettoAr.ToString();
                BruttoAr_tbx.ReadOnly = true;
                minimalisKeszlet_tbx.Text = termek.MinimalisKeszlet.ToString();
                BruttoAr_tbx.Enabled = false;
                BruttoAr_tbx.Enter += (s, zs) => { NavSzam_tbx.Focus(); };
                minimalisKeszlet_tbx.ReadOnly = true;
                minimalisKeszlet_tbx.Enter += (s, f) => { mentes_Btn.Focus(); };
                aktualisKeszlet_tbx.ReadOnly = true;
                aktualisKeszlet_tbx.Enter += (s, f) => { minimalisKeszlet_tbx.Focus(); };
                aktualisKeszlet_tbx.Text = termek.Mennyiseg.ToString();

                termekKategoria_cbox.DataSource = ABKezelo.TermekKategoriakBetoltes();
                mennyisegEgysege_cbx.DataSource = ABKezelo.TermekMennyisegEgysegBetoltes();
                AFAKulcs_cbox.Items.Clear();
                for (int i = 0; i < afak.Count; i++) AFAKulcs_cbox.Items.Add(afak[i].Value);
                if (AFAKulcs_cbox.Items.Count > 0) AFAKulcs_cbox.SelectedIndex = 0;

                termekKategoria_cbox.SelectedIndex = termekKategoria_cbox.FindStringExact(termek.TermekKategoria.ToString());
                mennyisegEgysege_cbx.SelectedIndex = mennyisegEgysege_cbx.FindStringExact(termek.MennyisegEgysege.ToString());
                AFAKulcs_cbox.SelectedIndex = AFAKulcs_cbox.FindStringExact(termek.AfaKulcs.ToString());

                foreach (sqlString item in ABKezelo.TermekMennyisegEgysegBetoltes())
                {
                    if (item.Value == termek.MennyisegEgysege.ToString())
                    {
                        mennyisegEgysege_cbx.SelectedItem = item.Value;//A HIBA AZ,hogyha törlöm a nemmyioség egységét, nem találja meg és nem ír ki xart se, Kell egy garbadge collector
                    }
                }

                for (int i = 0; i < afak.Count; i++)
                {
                    if (afak[i].Value == termek.AfaKulcs)
                    {
                        AFAKulcs_cbox.SelectedIndex = AFAKulcs_cbox.FindStringExact(afak[i].Value.ToString());
                        break;
                    }
                }
                AFAKulcs_cbox.SelectedIndexChanged += new EventHandler(this.AFAKulcs_cbox_SelectedIndexChanged);
               
                if (NettoAr_tbx.Text != null && AFAKulcs_cbox.SelectedItem != null)
                {
                    BruttoAr_tbx.Text = Termek.BruttoArSzamito(Convert.ToDecimal(NettoAr_tbx.Text), afak[AFAKulcs_cbox.SelectedIndex].Value).ToString();

                }
            
            }
            else
            {
                BruttoAr_tbx.ReadOnly = true;
                BruttoAr_tbx.Enter += (s, zs) => {NavSzam_tbx.Focus(); };
                termekKod_tbx.ReadOnly = true;
                termekKod_tbx.Enter += (s, f) => { CikkSzam_tbx.Focus(); };
                mennyiseg_tbx.Enter += (s, f) => { NettoAr_tbx.Focus(); };//Ha olyan mezőre kattol, ami readonly, akkor a sorrendben következő, módosítható mezőre ugrik.
                aktualisKeszlet_tbx.ReadOnly = true;
                aktualisKeszlet_tbx.Enter += (s, f) => { minimalisKeszlet_tbx.Focus(); };
                mennyiseg_tbx.ReadOnly = true;
                AFAKulcs_cbox.SelectedIndexChanged += new EventHandler(this.AFAKulcs_cbox_SelectedIndexChanged);
            }
            
        }
        #endregion

        #region Form Load
        private void UjTermekFrm_Load(object sender, EventArgs e)
        {
            ActiveControl = mentes_Btn;
            if (megnevezes_txb.Text == "")
            {
                megnevezes_txb.Text = "Kötelező kitölteni!";
                megnevezes_txb.ForeColor = Color.Gray;
            }
           if (mennyiseg_tbx.Text == "0" && oldalak!="modosit")
            {
                mennyiseg_tbx.Text = "0";
                mennyiseg_tbx.ForeColor = Color.Gray;
            }
            if (NettoAr_tbx.Text == "")
            {
                NettoAr_tbx.Text = "0";
                NettoAr_tbx.ForeColor = Color.Gray;
            }
            if (minimalisKeszlet_tbx.Text == "" && oldalak != "modosit")
            {
                minimalisKeszlet_tbx.Text = "0";
                minimalisKeszlet_tbx.ForeColor = Color.Gray;
            }
          
            this.BackColor = SystemColors.GradientInactiveCaption;

            //ABKezelo.TermekMennyisegEgysegBetoltes();
            
        }
        #endregion

        #region Kategória módosítás      
        private void kategoriaModositas_btn_Click(object sender, EventArgs e)
        {
            KategoriaFrm frm = new KategoriaFrm("termek", termekKategoria_cbox.SelectedIndex);
          
            if(frm.ShowDialog()==DialogResult.OK )
            {
                termekKategoriak = new BindingList<sqlString>(ABKezelo.TermekKategoriakBetoltes());
                termekKategoria_cbox.DataSource = termekKategoriak;

            }
            else
            {
                termekKategoriak = new BindingList<sqlString>(ABKezelo.TermekKategoriakBetoltes());
                termekKategoria_cbox.DataSource = termekKategoriak;
            }
        }
        #endregion
        
        #region ÁFA kulcs módosítás
       private void afaKulcsModositas_btn_Click(object sender, EventArgs e)
        {
            KategoriaFrm frm = new KategoriaFrm("afa", AFAKulcs_cbox.SelectedIndex);

            if (frm.ShowDialog() == DialogResult.OK)
            {
                afak = new BindingList<Termek.AFA>(ABKezelo.AfaKategoriakBetoltes());
                AFAKulcs_cbox.Items.Clear();
                for (int i = 0; i < afak.Count; i++)
                {
                    AFAKulcs_cbox.Items.Add(afak[i].Value);


                }

                if (AFAKulcs_cbox.Items.Count > 0)
                {
                    AFAKulcs_cbox.SelectedIndex = 0;


                    //   string tmp = AFAKulcs_cbox.SelectedItem.ToString();


                    //AFAKulcs_cbox.SelectedIndex = 0;
                    /* for (int i = 0; i < afak.Count; i++) {
                         if (afak[i].Key == tmp) {
                             AFAKulcs_cbox.SelectedIndex = AFAKulcs_cbox.FindStringExact(afak[i].Key);
                             break;
                         }
                     }*/
                }

            }
            else
            {
                afak = new BindingList<Termek.AFA>(ABKezelo.AfaKategoriakBetoltes());
                int tmp = Convert.ToInt32(AFAKulcs_cbox.SelectedItem);
                AFAKulcs_cbox.Items.Clear();
                for (int i = 0; i < afak.Count; i++)
                {
                    AFAKulcs_cbox.Items.Add(afak[i].Value);

                }
                if (AFAKulcs_cbox.Items.Count > 0)
                {
                    //AFAKulcs_cbox.SelectedIndex = 0;
                    for (int i = 0; i < afak.Count; i++)
                    {
                        if (afak[i].Value == tmp)
                        {
                            AFAKulcs_cbox.SelectedIndex = AFAKulcs_cbox.FindStringExact(afak[i].Value.ToString());
                            break;
                        }
                    }
                }
            }
        }
        #endregion

        #region Mennyíség egysége módosítás
        private void mennyisegEgysegModositas_btn_Click(object sender, EventArgs e)
        {
            KategoriaFrm frm = new KategoriaFrm("mennyisegEgyseg", mennyisegEgysege_cbx.SelectedIndex);

            if (frm.ShowDialog() == DialogResult.OK)
            {
                mertekEgyseg = new BindingList<sqlString>(ABKezelo.TermekMennyisegEgysegBetoltes());
                mennyisegEgysege_cbx.DataSource = mertekEgyseg;

            }
            else
            {
                mertekEgyseg = new BindingList<sqlString>(ABKezelo.TermekMennyisegEgysegBetoltes());
                mennyisegEgysege_cbx.DataSource = mertekEgyseg;
            }
        }
        #endregion

        #region Mentés
        private void Mentes_Btn_Click(object sender, EventArgs e)
        {


            if (modositas)
            {
                try
                {
                    if (MessageBox.Show("Biztos hogy módosítja a kiválasztott terméket??", "Figyelem", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        ujTermek = new Termek(int.Parse(termekKod_tbx.Text), termekKategoria_cbox.SelectedItem.ToString(), megnevezes_txb.Text, CikkSzam_tbx.Text, int.Parse(mennyiseg_tbx.Text), Convert.ToString(mennyisegEgysege_cbx.SelectedItem), Convert.ToDecimal(NettoAr_tbx.Text), Termek.BruttoArSzamito(Convert.ToDecimal(NettoAr_tbx.Text), Convert.ToDecimal(afak[AFAKulcs_cbox.SelectedIndex].Value)), Convert.ToDecimal(afak[AFAKulcs_cbox.SelectedIndex].Value), NavSzam_tbx.Text, int.Parse(minimalisKeszlet_tbx.Text));
                        ABKezelo.TermekModositas(ujTermek);
                    }
                   

                }
                catch (Exception ex)
                {

                    MessageBox.Show("Termék módosítási hiba!", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);//értéke van., de xar
                    this.DialogResult = DialogResult.None;
                   
                }

            }
            else if (!modositas && oldalak == "szallito" && megnevezes_txb.Text != "Kötelező kitölteni!")
            {
                try
                {

                    ujTermek = new Termek(int.Parse(termekKod_tbx.Text), Convert.ToString(termekKategoria_cbox.SelectedItem), megnevezes_txb.Text, CikkSzam_tbx.Text, int.Parse(mennyiseg_tbx.Text), Convert.ToString(mennyisegEgysege_cbx.SelectedItem), Convert.ToDecimal(NettoAr_tbx.Text), Termek.BruttoArSzamito(Convert.ToDecimal(NettoAr_tbx.Text), Convert.ToDecimal(afak[AFAKulcs_cbox.SelectedIndex].Value)), Convert.ToDecimal(afak[AFAKulcs_cbox.SelectedIndex].Value), NavSzam_tbx.Text, int.Parse(minimalisKeszlet_tbx.Text));

                    Termek lekerdezett = ABKezelo.TermekKivalasztas(int.Parse(termekKod_tbx.Text));

                    Termek feltoltendo = new Termek(int.Parse(termekKod_tbx.Text), Convert.ToString(termekKategoria_cbox.SelectedItem), megnevezes_txb.Text, CikkSzam_tbx.Text, (lekerdezett.Mennyiseg - int.Parse(mennyiseg_tbx.Text)), Convert.ToString(mennyisegEgysege_cbx.SelectedItem), Convert.ToDecimal(NettoAr_tbx.Text), Termek.BruttoArSzamito(Convert.ToDecimal(NettoAr_tbx.Text), Convert.ToDecimal(afak[AFAKulcs_cbox.SelectedIndex].Value)), Convert.ToDecimal(afak[AFAKulcs_cbox.SelectedIndex].Value), NavSzam_tbx.Text, int.Parse(minimalisKeszlet_tbx.Text));


                    if (ujTermek.Mennyiseg > lekerdezett.Mennyiseg)
                    {
                        MessageBox.Show("A megadott mennyíség nagyobb a rendelkezésre álló darabszámnál!", "Figyelem");
                        this.DialogResult = DialogResult.None;
                    }
                    else if (ujTermek.Mennyiseg < 1)
                    {
                        MessageBox.Show("A termék mennyísége csak pozitív egész szám lehet!", "Figyelem");
                        this.DialogResult = DialogResult.None;
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Termék rögzítési hiba!", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.DialogResult = DialogResult.None;
                }
            }
            else if (!modositas && oldalak == "bevetelezes" && megnevezes_txb.Text != "Kötelező kitölteni!")
            {
                try
                {
                    ujTermek = new Termek(int.Parse(termekKod_tbx.Text), Convert.ToString(termekKategoria_cbox.SelectedItem), megnevezes_txb.Text, CikkSzam_tbx.Text, int.Parse(mennyiseg_tbx.Text), Convert.ToString(mennyisegEgysege_cbx.SelectedItem), Convert.ToDecimal(NettoAr_tbx.Text), Termek.BruttoArSzamito(Convert.ToDecimal(NettoAr_tbx.Text), Convert.ToDecimal(afak[AFAKulcs_cbox.SelectedIndex].Value)), Convert.ToDecimal(afak[AFAKulcs_cbox.SelectedIndex].Value), NavSzam_tbx.Text, int.Parse(minimalisKeszlet_tbx.Text));

                    //Termek lekerdezett = ABKezelo.TermekKivalasztas(int.Parse(termekKod_tbx.Text));

                    //Termek feltoltendo = new Termek(int.Parse(termekKod_tbx.Text), Convert.ToString(termekKategoria_cbox.SelectedItem), megnevezes_txb.Text, CikkSzam_tbx.Text, (lekerdezett.Mennyiseg - int.Parse(mennyiseg_tbx.Text)), Convert.ToString(mennyisegEgysege_cbx.SelectedItem), Convert.ToDecimal(NettoAr_tbx.Text), Termek.BruttoArSzamito(Convert.ToDecimal(NettoAr_tbx.Text), Convert.ToDecimal(afak[AFAKulcs_cbox.SelectedIndex].Value)), Convert.ToDecimal(afak[AFAKulcs_cbox.SelectedIndex].Value), NavSzam_tbx.Text, int.Parse(minimalisKeszlet_tbx.Text));


                    if (ujTermek.Mennyiseg < 1)
                    {
                        MessageBox.Show("A termék mennyísége csak pozitív egész szám lehet!", "Figyelem");
                        this.DialogResult = DialogResult.None;
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Termék rögzítési hiba!", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.DialogResult = DialogResult.None;
                }
            }
            else
            {
                try
                {

                    if (ABKezelo.TermekKivalasztasCikkszamAlapjan(CikkSzam_tbx.Text) == null && AFAKulcs_cbox.Items.Count > 0 && termekKategoria_cbox.Items.Count > 0 && mennyisegEgysege_cbx.Items.Count > 0 && megnevezes_txb.Text != "Kötelező kitölteni!")
                    {
                        ujTermek = new Termek(Convert.ToString(termekKategoria_cbox.SelectedItem), megnevezes_txb.Text, CikkSzam_tbx.Text, 0, Convert.ToString(mennyisegEgysege_cbx.SelectedItem), Convert.ToDecimal(NettoAr_tbx.Text), Termek.BruttoArSzamito(Convert.ToDecimal(NettoAr_tbx.Text), Convert.ToDecimal(afak[AFAKulcs_cbox.SelectedIndex].Value)), Convert.ToDecimal(afak[AFAKulcs_cbox.SelectedIndex].Value), NavSzam_tbx.Text, int.Parse(minimalisKeszlet_tbx.Text));

                        //Termek.Termekek.Add(ujTermek);//shift+end visz a sor végére
                        ABKezelo.TermekFelvitel(ujTermek);
                        ABKezelo.TermekFelvitelMySQL(ujTermek);
                        MessageBox.Show("A termék  rögzítése megtörtént!", "Figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (CikkSzam_tbx.TextLength == 0 && AFAKulcs_cbox.Items.Count > 0 && termekKategoria_cbox.Items.Count > 0 && mennyisegEgysege_cbx.Items.Count > 0 && megnevezes_txb.Text != "Kötelező kitölteni!")
                    {
                        CikkSzam_tbx.Text =null;
                        ujTermek = new Termek(Convert.ToString(termekKategoria_cbox.SelectedItem), megnevezes_txb.Text, CikkSzam_tbx.Text, 0, Convert.ToString(mennyisegEgysege_cbx.SelectedItem), Convert.ToDecimal(NettoAr_tbx.Text), Termek.BruttoArSzamito(Convert.ToDecimal(NettoAr_tbx.Text), Convert.ToDecimal(afak[AFAKulcs_cbox.SelectedIndex].Value)), Convert.ToDecimal(afak[AFAKulcs_cbox.SelectedIndex].Value), NavSzam_tbx.Text, int.Parse(minimalisKeszlet_tbx.Text));
                        ABKezelo.TermekFelvitel(ujTermek);
                        MessageBox.Show("A termék  rögzítése megtörtént!", "Figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else if (ABKezelo.TermekKivalasztasCikkszamAlapjan(CikkSzam_tbx.Text) !="" || ABKezelo.TermekKivalasztasCikkszamAlapjan(CikkSzam_tbx.Text) ==null)
                    {
                        termekFrm.TermekDuplikacioLV(CikkSzam_tbx.Text);//lv pirosító

                        MessageBox.Show("A termék cikkszáma már szerepel az adatbázisban!", "Figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                       
                        this.DialogResult = DialogResult.None;
                    }
                    else if (AFAKulcs_cbox.Items.Count == 0)
                    {
                        MessageBox.Show("A terméklhez nincs áfa éték társítva!", "Figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.DialogResult = DialogResult.None;

                    }
                    else if (termekKategoria_cbox.Items.Count == 0)
                    {
                        MessageBox.Show("A terméklhez nincs termékkategória társítva!", "Figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.DialogResult = DialogResult.None;
                    }
                    else if (mennyisegEgysege_cbx.Items.Count == 0)
                    {
                        MessageBox.Show("A terméklhez nincs mennyíség egység társítva!", "Figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.DialogResult = DialogResult.None;
                    }
                    else
                    {
                   
                        MessageBox.Show("Az adatok nem megfelelően, vagy hiányosan lettek kitöltve!", "Figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.DialogResult = DialogResult.None;
                    }

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.DialogResult = DialogResult.None;
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
        #endregion

        #region Eventek
        private void NettoAr_tbx_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (NettoAr_tbx.Text.Length >0 && NettoAr_tbx.Text != "0"&& AFAKulcs_cbox.SelectedItem != null)
                {
                    BruttoAr_tbx.Text = Termek.BruttoArSzamito(Convert.ToDecimal(NettoAr_tbx.Text), afak[AFAKulcs_cbox.SelectedIndex].Value).ToString();
                    
                }
            }
            catch (Exception)
            {


            }
            KivetelKezeles.TextBoxSzamregex(errorProvider1, NettoAr_tbx);
        }

        private void AFAKulcs_cbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //try {
                if (NettoAr_tbx.Text != "0" && AFAKulcs_cbox.SelectedItem != null && afak.Count > 0) {
                    BruttoAr_tbx.Text = Termek.BruttoArSzamito(Convert.ToDecimal(NettoAr_tbx.Text), afak[AFAKulcs_cbox.SelectedIndex].Value).ToString();
                }
           // } catch (Exception) {}
        }
        private void megnevezes_txb_Enter(object sender, EventArgs e)
        {
            if (megnevezes_txb.Text == "Kötelező kitölteni!")
            {
                megnevezes_txb.Text = "";
                megnevezes_txb.ForeColor = Color.Black;
            }
        }

        private void megnevezes_txb_Leave(object sender, EventArgs e)
        {
            if (megnevezes_txb.Text == "")
            {
                megnevezes_txb.Text = "Kötelező kitölteni!";
                megnevezes_txb.ForeColor = Color.Gray;
            }
            else
            {
                megnevezes_txb.Text = megnevezes_txb.Text[0].ToString().ToUpper() + megnevezes_txb.Text.Substring(1);
            }
        }

        private void mennyiseg_tbx_Enter(object sender, EventArgs e)
        {
            if (mennyiseg_tbx.Text == "0")
            {
                mennyiseg_tbx.Text = "";
                mennyiseg_tbx.ForeColor = Color.Black;
            }
        }

        private void mennyiseg_tbx_Leave(object sender, EventArgs e)
        {
            if (mennyiseg_tbx.Text == "")
            {
                mennyiseg_tbx.Text = "0";
                mennyiseg_tbx.ForeColor = Color.Gray;

            }KivetelKezeles.TextBoxSzamregex(errorProvider1,mennyiseg_tbx);
        }

        private void NettoAr_tbx_Enter(object sender, EventArgs e)
        {
            if (NettoAr_tbx.Text == "0")
            {
                NettoAr_tbx.Text = "";
                NettoAr_tbx.ForeColor = Color.Black;
            }
        }

        private void NettoAr_tbx_Leave(object sender, EventArgs e)
        {
            if (NettoAr_tbx.Text == "")
            {
                NettoAr_tbx.Text = "0";
                NettoAr_tbx.ForeColor = Color.Gray;

            }
            KivetelKezeles.TextBoxSzamregex(errorProvider1, NettoAr_tbx);
        }

        private void minimalisKeszlet_tbx_Enter(object sender, EventArgs e)
        {
            if (minimalisKeszlet_tbx.Text == "0")
            {
                minimalisKeszlet_tbx.Text = "";
                minimalisKeszlet_tbx.ForeColor = Color.Black;
            }
        }

        private void minimalisKeszlet_tbx_Leave(object sender, EventArgs e)
        {
            if (minimalisKeszlet_tbx.Text == "")
            {
                minimalisKeszlet_tbx.Text = "0";
                minimalisKeszlet_tbx.ForeColor = Color.Gray;

            }
            KivetelKezeles.TextBoxSzamregex(errorProvider1, minimalisKeszlet_tbx);
        }
      
        
        private void kategoriaModositas_btn_MouseEnter(object sender, EventArgs e)
        {
            if (oldalak != "megtekint")
            {
                toolTip1.Active = true;
                Point x = new Point(kategoriaModositas_btn.Right + 20, kategoriaModositas_btn.Top);
                toolTip1.SetToolTip(kategoriaModositas_btn, "uff");
                toolTip1.Show("Adatok\nmódosítása.", this, x);
            }
        }

        private void kategoriaModositas_btn_MouseLeave(object sender, EventArgs e)
        {
            if (oldalak != "megtekint")
            {
                toolTip1.Active = false;
            }
        }

        private void mennyisegEgysegModositas_btn_MouseEnter(object sender, EventArgs e)
        {
            if (oldalak != "megtekint")
            {
                toolTip1.Active = true;
                Point x = new Point(mennyisegEgysegModositas_btn.Right + 20, mennyisegEgysegModositas_btn.Top);
                toolTip1.SetToolTip(mennyisegEgysegModositas_btn, "uff");
                toolTip1.Show("Adatok\nmódosítása.", this, x);
            }
        }

        private void mennyisegEgysegModositas_btn_MouseLeave(object sender, EventArgs e)
        {
            if (oldalak != "megtekint")
            {
                toolTip1.Active = false;
            }
        }

        private void afaKulcsModositas_btn_MouseEnter(object sender, EventArgs e)
        {
            if (oldalak != "megtekint")
            {
                toolTip1.Active = true;
                Point x = new Point(afaKulcsModositas_btn.Right + 20, afaKulcsModositas_btn.Top);
                toolTip1.SetToolTip(afaKulcsModositas_btn, "uff");
                toolTip1.Show("Adatok\nmódosítása.", this, x);
            }
        }

        private void afaKulcsModositas_btn_MouseLeave(object sender, EventArgs e)
        {
            if (oldalak != "megtekint")
            {
                toolTip1.Active = false;
            }
        }

        private void mennyiseg_tbx_TextChanged(object sender, EventArgs e)
        {
           /* if (mennyiseg_tbx.Text != "0")
            {
                KivetelKezeles.TextBoxSzamregex(errorProvider1, mennyiseg_tbx);
            }*/
        }

        private void minimalisKeszlet_tbx_TextChanged(object sender, EventArgs e)
        {
            KivetelKezeles.TextBoxSzamregex(errorProvider1, minimalisKeszlet_tbx);
        }
        #endregion

        private void mennyiseg_tbx_KeyPress(object sender, KeyPressEventArgs e)
        {
            KivetelKezeles.StringLetiltoDecimal(mennyiseg_tbx,e);
        }

        private void NettoAr_tbx_KeyPress(object sender, KeyPressEventArgs e)
        {
           
           
            
                KivetelKezeles.StringLetiltoDecimal(NettoAr_tbx, e);
            
           
        }

        private void minimalisKeszlet_tbx_KeyPress(object sender, KeyPressEventArgs e)
        {
            KivetelKezeles.StringLetiltoDecimal(minimalisKeszlet_tbx, e);
        }

        private void segitseg_btn_Click(object sender, EventArgs e)
        {
            Helpfrm help = new Helpfrm(KivetelKezeles.getHelp("Termékek menüpont"));
            help.ShowDialog();
        }

        private void AFAKulcs_cbox_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }
    }
}
