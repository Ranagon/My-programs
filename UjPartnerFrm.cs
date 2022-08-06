using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Szakdolgozat3// Ebbe a formban a "FindStringExact" függvénnyel volt problémám, nem találta a kifejezéseket a comboboxban. Most működik, de lehetséges a kódhalmozás. 
{
    public partial class UjPartnerFrm : Form
    {
        bool modositas = false;
        internal static Partner partnerek;
        internal Partner kivalasztottPartner;
        private BindingList<sqlString> FizetesiMod = new BindingList<sqlString>(ABKezelo.FizetesModBetoltes());
        BindingList<sqlString> partnerCsoportok = new BindingList<sqlString>(ABKezelo.PartnerBesorolasBetoltes());
        string idezettOldal = "";
        internal Partner Partnerek { get => partnerek; set => partnerek = value; }

       
        public UjPartnerFrm()
        {

            InitializeComponent();

            SzamlaKozteruletJellege_cbox.DataSource = Enum.GetValues(typeof(KozteruletJelleg));
            PostaKozteruletJellege_cbox.DataSource = Enum.GetValues(typeof(KozteruletJelleg));
            FizetesiMod_cbox.DataSource = FizetesiMod;
            PartnerBesosolasa_cbox.DataSource = partnerCsoportok;
            SzamlaOrszag_txb.Text = "Magyarország";
            PostaOrszag_txb.Text = "Magyarország";
            SzamlaOrszag_txb.ReadOnly = true;
            PostaOrszag_txb.ReadOnly = true;
            SzamlaOrszag_txb.Enter += (s, e) => { SzamlaIrsz_txb.Focus(); };
            PostaOrszag_txb.Enter += (s, e) => { PostaIranyitoSzam_txb.Focus(); };

            this.ActiveControl = null;
        }
        internal UjPartnerFrm(Partner partner, string oldal)
        {
            InitializeComponent();

            SzamlaKozteruletJellege_cbox.DataSource = Enum.GetValues(typeof(KozteruletJelleg));
            PostaKozteruletJellege_cbox.DataSource = Enum.GetValues(typeof(KozteruletJelleg));
            FizetesiMod_cbox.DataSource = FizetesiMod;
            PartnerBesosolasa_cbox.DataSource = partnerCsoportok;

            idezettOldal = oldal;

            if (partner != null)
            {
                kivalasztottPartner = partner;
            }
          
            if (partner != null && oldal == "modosit")//MÓDOSÍTÁS
            {
                modositas = true;

                SzamlaOrszag_txb.ReadOnly = true;
                PostaOrszag_txb.ReadOnly = true;
                SzamlaOrszag_txb.Enter += (s, e) => { SzamlaIrsz_txb.Focus(); };
                PostaOrszag_txb.Enter += (s, e) => { PostaIranyitoSzam_txb.Focus(); };
                this.Text = "Partner adatainak módosítása";
                SzamlaAdoSzam_txb.Text = partner.AdoSzam.ToString();
                SzamlaEUAdoSzam_txb.Text = partner.EUAdoszam1.ToString();
                TelefonSzam_txb.Text = partner.TelefonSzam.ToString();
                Fax_txb.Text = partner.Fax.ToString();
                EmailCim_tbx.Text = partner.Email;
                WebOldalCime_txb.Text = partner.WebCim;
                BankSzamlaSzam_tbx.Text = partner.BankSzamlaSzam;

                  BindingList<sqlString> FizetesiMod = new BindingList<sqlString>(ABKezelo.FizetesModBetoltes());
                  BindingList<sqlString> partnerCsoportok = new BindingList<sqlString>(ABKezelo.PartnerBesorolasBetoltes());

                foreach (sqlString item in ABKezelo.FizetesModBetoltes())
                {
                    if (item.Value == partner.FizetesiMod.ToString())
                    {
                        FizetesiMod_cbox.SelectedItem = item.Value;
                        if (FizetesiMod_cbox.Items.Count > 0) FizetesiMod_cbox.SelectedIndex = 0;
                    }
                }

                for (int i = 0; i < FizetesiMod.Count; i++)
                {
                    if (FizetesiMod[i].Value == partner.FizetesiMod)
                    {
                        FizetesiMod_cbox.SelectedIndex = FizetesiMod_cbox.FindStringExact(FizetesiMod[i].Value.ToString());
                        break;
                    }

                }
                    PartnerBesosolasa_cbox.SelectedIndex = PartnerBesosolasa_cbox.FindStringExact(partner.PartnerCsoport);
                FizetesiMod_cbox.SelectedIndex = FizetesiMod_cbox.FindStringExact(partner.FizetesiMod);
                Kedvezmeny_nud.Value = Convert.ToDecimal(partner.Kedvezmeny);
                SzamlaMegjegyzes_txb.Text = partner.SzamlaMegjegyzes;
                Partnermegjegyzes_tbx.Text = partner.SzamlaMegjegyzes;
                Titulus_txb.Text = partner.Titulus;
                vezeteknev_tbx.Text = partner.VezetekNev;
                Keresztnev_tbx.Text = partner.KeresztNev;
                UtoNev_tbx.Text = partner.UtoNev;
                if (partner.CegNev=="Magánszemély")
                {
                    maganSzemely_chbox.Checked = true;
                   
                }
                else
                {
                  Cegnev_txb.Text = partner.CegNev;
                }               
                SzamlaOrszag_txb.Text = partner.Orszag;
                SzamlaIrsz_txb.Text = partner.Irsz;
                SzamlaTelepules_txb.Text = partner.Telepules;
                SzamlaKozeruletNeve_txb.Text = partner.Kozterulet;
                SzamlaKozteruletJellege_cbox.SelectedItem = partner.KozteruletJelleg;
                SzamlaKozteruletJellege_cbox.SelectedIndex = SzamlaKozteruletJellege_cbox.FindStringExact(partner.KozteruletJelleg.ToString());
                SzamlaHazSzam_txb.Text = partner.HazSzam;
                SzamlaEmeletAjto_txb.Text = partner.EmeletAjto;
                PostaOrszag_txb.Text = partner.PostaOrszag;
                PostaIranyitoSzam_txb.Text = partner.PostaIrsz;
                Postatelepules_txb.Text = partner.PostaTelepules;
                PostaKozteruletNeve_txb.Text = partner.PostaKozterulet;
                PostaKozteruletJellege_cbox.SelectedItem = partner.PostaKozteruletJelleg.ToString();
             
                PostaKozteruletJellege_cbox.SelectedIndex = PostaKozteruletJellege_cbox.FindStringExact(partner.PostaKozteruletJelleg.ToString());
                PostaHazSzam_txb.Text = partner.PostaHazSzam;
                PostaEmeletAjto_txb.Text = partner.PostaEmeletAjto;
                SzamlaAdoSzam_txb.ReadOnly = true;
                SzamlaAdoSzam_txb.Enter += (s, e) => { TelefonSzam_txb.Focus(); };
                if (SzamlaEUAdoSzam_txb.TextLength > 0) {
                    SzamlaEUAdoSzam_txb.ReadOnly = true;
                    SzamlaEUAdoSzam_txb.Enter += (s, e) => { TelefonSzam_txb.Focus(); };
                }
                if(KivetelKezeles.PostaEllenorzo(PostaIranyitoSzam_txb, Postatelepules_txb, PostaKozteruletNeve_txb, PostaHazSzam_txb)) {
                    PostaKozteruletJellege_cbox.SelectedIndex = 0;
                }
            }
            if (partner != null && oldal == "megtekint")//MEGTEKINTÉS
            {
             
                this.Text = "partner adatainak megtekintése";
                modositas = true;
                button2.Visible = false;
                button4.Visible = false;
                SzamlaAdoSzam_txb.Text = partner.AdoSzam.ToString();
                SzamlaEUAdoSzam_txb.Text = partner.EUAdoszam1.ToString();
                TelefonSzam_txb.Text = partner.TelefonSzam.ToString();
                Fax_txb.Text = partner.Fax.ToString();
                EmailCim_tbx.Text = partner.Email;
                WebOldalCime_txb.Text = partner.WebCim;
                BankSzamlaSzam_tbx.Text = partner.BankSzamlaSzam;
                PartnerBesosolasa_cbox.SelectedItem = partner.PartnerCsoport;
                PartnerBesosolasa_cbox.SelectedIndex = PartnerBesosolasa_cbox.FindStringExact(partner.PartnerCsoport);
                FizetesiMod_cbox.SelectedItem = partner.FizetesiMod.ToString();
                FizetesiMod_cbox.SelectedIndex = FizetesiMod_cbox.FindStringExact(partner.FizetesiMod);
                Kedvezmeny_nud.Value = Convert.ToDecimal(partner.Kedvezmeny);
                SzamlaMegjegyzes_txb.Text = partner.SzamlaMegjegyzes;
                Partnermegjegyzes_tbx.Text = partner.SzamlaMegjegyzes;
                Titulus_txb.Text = partner.Titulus;
                vezeteknev_tbx.Text = partner.VezetekNev;
                Keresztnev_tbx.Text = partner.KeresztNev;
                UtoNev_tbx.Text = partner.UtoNev;
                SzamlaOrszag_txb.Text = partner.Orszag;
                SzamlaIrsz_txb.Text = partner.Irsz;
                SzamlaTelepules_txb.Text = partner.Telepules;
                SzamlaKozeruletNeve_txb.Text = partner.Kozterulet;
                SzamlaKozteruletJellege_cbox.SelectedItem = partner.KozteruletJelleg;
                SzamlaKozteruletJellege_cbox.SelectedIndex = SzamlaKozteruletJellege_cbox.FindStringExact(partner.KozteruletJelleg.ToString());
                SzamlaHazSzam_txb.Text = partner.HazSzam;
                SzamlaEmeletAjto_txb.Text = partner.EmeletAjto;
                PostaOrszag_txb.Text = partner.PostaOrszag;
                PostaIranyitoSzam_txb.Text = partner.PostaIrsz;
                Postatelepules_txb.Text = partner.PostaTelepules;
                PostaKozteruletNeve_txb.Text = partner.PostaKozterulet;
                PostaKozteruletJellege_cbox.SelectedItem = partner.PostaKozteruletJelleg.ToString();
                PostaKozteruletJellege_cbox.SelectedIndex = PostaKozteruletJellege_cbox.FindStringExact(partner.PostaKozteruletJelleg.ToString());
                PostaHazSzam_txb.Text = partner.PostaHazSzam;
                PostaEmeletAjto_txb.Text = partner.PostaEmeletAjto;
                SzamlaAdoSzam_txb.ReadOnly = true;
                SzamlaEUAdoSzam_txb.ReadOnly = true;
                TelefonSzam_txb.ReadOnly = true;
                Fax_txb.ReadOnly = true;
                EmailCim_tbx.ReadOnly = true;
                WebOldalCime_txb.ReadOnly = true;
                BankSzamlaSzam_tbx.ReadOnly = true;
                PartnerBesosolasa_cbox.Enabled = false;
                FizetesiMod_cbox.Enabled = false;
                Kedvezmeny_nud.Enabled = false;
                SzamlaMegjegyzes_txb.ReadOnly = true;
                Partnermegjegyzes_tbx.ReadOnly = true;
                Titulus_txb.ReadOnly = true;
                vezeteknev_tbx.ReadOnly = true;
                Keresztnev_tbx.ReadOnly = true;
                UtoNev_tbx.ReadOnly = true;
                Cegnev_txb.ReadOnly = true;
                SzamlaOrszag_txb.ReadOnly = true;
                SzamlaIrsz_txb.ReadOnly = true;
                SzamlaTelepules_txb.ReadOnly = true;
                SzamlaKozeruletNeve_txb.ReadOnly = true;
                SzamlaKozteruletJellege_cbox.Enabled = false;
                SzamlaHazSzam_txb.ReadOnly = true;
                SzamlaEmeletAjto_txb.ReadOnly = true;
                PostaOrszag_txb.ReadOnly = true;
                PostaIranyitoSzam_txb.ReadOnly = true;
                Postatelepules_txb.ReadOnly = true;
                PostaKozteruletNeve_txb.ReadOnly = true;
                PostaKozteruletJellege_cbox.Enabled = false;
                PostaHazSzam_txb.ReadOnly = true;
                PostaEmeletAjto_txb.ReadOnly = true;
                maganSzemely_chbox.Enabled = false;
                Postacim_chbox.Enabled = false;
                Postacim_chbox.Visible = false;
                Bovebben_chbox.Enabled = true;
                mentes_btn.Visible = false;
                segitseg_btn.Visible = false;
                megse_btn.Text = "Bezárás";

                if (partner.CegNev == "Kötelező kitölteni!")
                {
                    CegNev_lbl.Visible = false;
                    Cegnev_txb.Visible = false;
                    label10.Text = "Adóazonosító jel:";
                }
                else
                {
                    CegNev_lbl.Visible = true;
                    Cegnev_txb.Visible = true;
                    label10.Text = "Adószám:";
                    Cegnev_txb.Text = partner.CegNev;
                }

                SzamlaAdoSzam_txb.Enter += (s, e) => { megse_btn.Focus(); };
                SzamlaEUAdoSzam_txb.Enter += (s, e) => { megse_btn.Focus(); };
                TelefonSzam_txb.Enter += (s, e) => { megse_btn.Focus(); };
                Fax_txb.Enter += (s, e) => { megse_btn.Focus(); };
                EmailCim_tbx.Enter += (s, e) => { megse_btn.Focus(); };
                WebOldalCime_txb.Enter += (s, e) => { megse_btn.Focus(); };
                BankSzamlaSzam_tbx.Enter += (s, e) => { megse_btn.Focus(); };
                PartnerBesosolasa_cbox.Enter += (s, e) => { megse_btn.Focus(); };
                FizetesiMod_cbox.Enter += (s, e) => { megse_btn.Focus(); };
                Kedvezmeny_nud.Enter += (s, e) => { megse_btn.Focus(); };
                SzamlaMegjegyzes_txb.Enter += (s, e) => { megse_btn.Focus(); };
                Partnermegjegyzes_tbx.Enter += (s, e) => { megse_btn.Focus(); };
                Titulus_txb.Enter += (s, e) => { megse_btn.Focus(); };
                vezeteknev_tbx.Enter += (s, e) => { megse_btn.Focus(); };
                Keresztnev_tbx.Enter += (s, e) => { megse_btn.Focus(); };
                UtoNev_tbx.Enter += (s, e) => { megse_btn.Focus(); };
                Cegnev_txb.Enter += (s, e) => { megse_btn.Focus(); };
                SzamlaOrszag_txb.Enter += (s, e) => { megse_btn.Focus(); };
                SzamlaIrsz_txb.Enter += (s, e) => { megse_btn.Focus(); };
                SzamlaTelepules_txb.Enter += (s, e) => { megse_btn.Focus(); };
                SzamlaKozeruletNeve_txb.Enter += (s, e) => { megse_btn.Focus(); };
                SzamlaKozteruletJellege_cbox.Enter += (s, e) => { megse_btn.Focus(); };
                SzamlaHazSzam_txb.Enter += (s, e) => { megse_btn.Focus(); };
                SzamlaEmeletAjto_txb.Enter += (s, e) => { megse_btn.Focus(); };
                PostaOrszag_txb.Enter += (s, e) => { megse_btn.Focus(); };
                PostaIranyitoSzam_txb.Enter += (s, e) => { megse_btn.Focus(); };
                Postatelepules_txb.Enter += (s, e) => { megse_btn.Focus(); };
                PostaKozteruletNeve_txb.Enter += (s, e) => { megse_btn.Focus(); };
                PostaKozteruletJellege_cbox.Enter += (s, e) => { megse_btn.Focus(); };
                PostaHazSzam_txb.Enter += (s, e) => { megse_btn.Focus(); };
                PostaEmeletAjto_txb.Enter += (s, e) => { megse_btn.Focus(); };
                SzamlaAdoSzam_txb.Enter += (s, e) => { megse_btn.Focus(); };
                SzamlaEUAdoSzam_txb.Enter += (s, e) => { megse_btn.Focus(); };
                TelefonSzam_txb.Enter += (s, e) => { megse_btn.Focus(); };
                Fax_txb.Enter += (s, e) => { megse_btn.Focus(); };
                EmailCim_tbx.Enter += (s, e) => { megse_btn.Focus(); };
                WebOldalCime_txb.Enter += (s, e) => { megse_btn.Focus(); };
                BankSzamlaSzam_tbx.Enter += (s, e) => { megse_btn.Focus(); };
                PartnerBesosolasa_cbox.Enter += (s, e) => { megse_btn.Focus(); };
                FizetesiMod_cbox.Enter += (s, e) => { megse_btn.Focus(); };
                Kedvezmeny_nud.Enter += (s, e) => { megse_btn.Focus(); };
                SzamlaMegjegyzes_txb.Enter += (s, e) => { megse_btn.Focus(); };
                Partnermegjegyzes_tbx.Enter += (s, e) => { megse_btn.Focus(); };
                Titulus_txb.Enter += (s, e) => { megse_btn.Focus(); };
                vezeteknev_tbx.Enter += (s, e) => { megse_btn.Focus(); };
                Keresztnev_tbx.Enter += (s, e) => { megse_btn.Focus(); };
                UtoNev_tbx.Enter += (s, e) => { megse_btn.Focus(); };
                Cegnev_txb.Enter += (s, e) => { megse_btn.Focus(); };
                SzamlaOrszag_txb.Enter += (s, e) => { megse_btn.Focus(); };
                SzamlaIrsz_txb.Enter += (s, e) => { megse_btn.Focus(); };
                SzamlaTelepules_txb.Enter += (s, e) => { megse_btn.Focus(); };
                SzamlaKozeruletNeve_txb.Enter += (s, e) => { megse_btn.Focus(); };
                SzamlaKozteruletJellege_cbox.Enter += (s, e) => { megse_btn.Focus(); };
                SzamlaHazSzam_txb.Enter += (s, e) => { megse_btn.Focus(); };
                SzamlaEmeletAjto_txb.Enter += (s, e) => { megse_btn.Focus(); };
                PostaOrszag_txb.Enter += (s, e) => { megse_btn.Focus(); };
                PostaIranyitoSzam_txb.Enter += (s, e) => { megse_btn.Focus(); };
                Postatelepules_txb.Enter += (s, e) => { megse_btn.Focus(); };
                PostaKozteruletNeve_txb.Enter += (s, e) => { megse_btn.Focus(); };
                PostaKozteruletJellege_cbox.Enter += (s, e) => { megse_btn.Focus(); };
                PostaHazSzam_txb.Enter += (s, e) => { megse_btn.Focus(); };
                PostaEmeletAjto_txb.Enter += (s, e) => { megse_btn.Focus(); };
              //  if (KivetelKezeles.PostaEllenorzo(PostaIranyitoSzam_txb, Postatelepules_txb, PostaKozteruletNeve_txb, PostaHazSzam_txb, PostaEmeletAjto_txb)) {
                //    PostaKozteruletJellege_cbox.SelectedIndex = -1;
                //}
            }
            this.ActiveControl = null;
        }

        #region Form_Load
        private void UjPartnerFrm_Load(object sender, EventArgs e)
        {
           
            this.BackColor = SystemColors.GradientInactiveCaption;
            this.ActiveControl = mentes_btn;

            if (vezeteknev_tbx.Text == "")//BEÁLLÍTJA A KÖTELEZŐEN KITÖLTENDŐ TEXTBOXOK TARTALMÁT
            {
                vezeteknev_tbx.Text = "Kötelező kitölteni!";
                vezeteknev_tbx.ForeColor = Color.Gray;
                
            }
            if (Keresztnev_tbx.Text == "")
            {
                Keresztnev_tbx.Text = "Kötelező kitölteni!";
                Keresztnev_tbx.ForeColor = Color.Gray;
               
            }          
            if (Cegnev_txb.Text == "")
            {

                Cegnev_txb.Text = "Kötelező kitölteni!";
                Cegnev_txb.ForeColor = Color.Gray;
                
            }
            if (SzamlaOrszag_txb.Text == "")
            {
                SzamlaOrszag_txb.Text = "Kötelező kitölteni!";
                SzamlaOrszag_txb.ForeColor = Color.Gray;
                
            }
            if (SzamlaIrsz_txb.Text == "")
            {
                SzamlaIrsz_txb.Text = "Kötelező kitölteni!";
                SzamlaIrsz_txb.ForeColor = Color.Gray;
                
            }
            if (SzamlaTelepules_txb.Text == "")
            {
                SzamlaTelepules_txb.Text = "Kötelező kitölteni!";
                SzamlaTelepules_txb.ForeColor = Color.Gray;
                
            }
            if (SzamlaKozeruletNeve_txb.Text == "")
            {
                SzamlaKozeruletNeve_txb.Text = "Kötelező kitölteni!";
                SzamlaKozeruletNeve_txb.ForeColor = Color.Gray;
                
            }
            if (SzamlaHazSzam_txb.Text == "")
            {
                SzamlaHazSzam_txb.Text = "Kötelező kitölteni!";
                SzamlaHazSzam_txb.ForeColor = Color.Gray;
                
            }

            if (SzamlaAdoSzam_txb.Text == "")
            {
                SzamlaAdoSzam_txb.Text = "Kötelező kitölteni!";
                SzamlaAdoSzam_txb.ForeColor = Color.Gray;
                
            }

            if (TelefonSzam_txb.Text == "")
            {
                TelefonSzam_txb.Text = "Kötelező kitölteni!";
                TelefonSzam_txb.ForeColor = Color.Gray;
                
            }
           
            Titulus_lbl.Visible = false;
            Titulus_txb.Visible = false;
            UtoNev_lbl.Visible = false;
            UtoNev_tbx.Visible = false;
            
            
        }
        #endregion

        #region Partner besorolás_btn
        private void button4_Click_1(object sender, EventArgs e)//kategoriaFrm/Partnerkategoria_tbp-re ugrik
        {
            KategoriaFrm frm = new KategoriaFrm("partner", PartnerBesosolasa_cbox.SelectedIndex);

            if (frm.ShowDialog() == DialogResult.OK)
            {
                partnerCsoportok = new BindingList<sqlString>(ABKezelo.PartnerBesorolasBetoltes());
                PartnerBesosolasa_cbox.DataSource = partnerCsoportok;
            }
            else
            {
                partnerCsoportok = new BindingList<sqlString>(ABKezelo.PartnerBesorolasBetoltes());
                PartnerBesosolasa_cbox.DataSource = partnerCsoportok;
            }
        }
        #endregion

        #region Bővebben_chbx
        private void Bovebben_chbox_CheckedChanged(object sender, EventArgs e)
        {
            if (Bovebben_chbox.Checked)
            {
                Titulus_lbl.Visible = true;
                Titulus_txb.Visible = true;
                UtoNev_lbl.Visible = true;
                UtoNev_tbx.Visible = true;
            }
            else
            {
                Titulus_lbl.Visible = false;
                Titulus_txb.Visible = false;
                UtoNev_lbl.Visible = false;
                UtoNev_tbx.Visible = false;

            }
        }
        #endregion

        #region Mentes_btn
        private void Mentes_btn_Click(object sender, EventArgs e)
        {

            if (modositas && MessageBox.Show("Biztos hogy módosítja a kiválasztott partner adatait?", "Figyelem", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                KivetelKezeles.PostaEllenorzo(PostaIranyitoSzam_txb,Postatelepules_txb,PostaKozteruletNeve_txb,PostaHazSzam_txb);//HA A POSTÁZÁSI ADATOK KÖZÜL BÁRMELYIK IS ÜRES, KIVÉVE A COMBOBOXOT, ÜRES STRING LESZ AZ ÉRTÉKÜK
                    try {
                       
                            partnerek = new Partner(kivalasztottPartner.PartnerAzonosito,SzamlaAdoSzam_txb.Text, SzamlaEUAdoSzam_txb.Text, TelefonSzam_txb.Text, Fax_txb.Text, EmailCim_tbx.Text,
                                    WebOldalCime_txb.Text, BankSzamlaSzam_tbx.Text, Convert.ToString(PartnerBesosolasa_cbox.SelectedItem), Convert.ToString(FizetesiMod_cbox.SelectedItem), Kedvezmeny_nud.Value, SzamlaMegjegyzes_txb.Text,
                                    Partnermegjegyzes_tbx.Text, Titulus_txb.Text, vezeteknev_tbx.Text, Keresztnev_tbx.Text, UtoNev_tbx.Text, Cegnev_txb.Text, SzamlaOrszag_txb.Text,
                                    SzamlaIrsz_txb.Text, SzamlaTelepules_txb.Text, SzamlaKozeruletNeve_txb.Text, (KozteruletJelleg)SzamlaKozteruletJellege_cbox.SelectedItem, SzamlaHazSzam_txb.Text,
                                    SzamlaEmeletAjto_txb.Text, PostaOrszag_txb.Text, PostaIranyitoSzam_txb.Text, Postatelepules_txb.Text, PostaKozteruletNeve_txb.Text, (KozteruletJelleg)PostaKozteruletJellege_cbox.SelectedItem, PostaHazSzam_txb.Text, PostaEmeletAjto_txb.Text);
                            ABKezelo.PartnerModositas(partnerek);
                       
                    } catch (Exception ex) {
                        MessageBox.Show(ex.Message, "Figyelem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.DialogResult = DialogResult.None;
                    }             
            }
            else
            {
                KivetelKezeles.PostaEllenorzo(PostaIranyitoSzam_txb, Postatelepules_txb, PostaKozteruletNeve_txb, PostaHazSzam_txb);
                try {
                        if (ABKezelo.AdoszamDuplikacio(SzamlaAdoSzam_txb.Text)==""  && KivetelKezeles.AdoszamEllenorzo(SzamlaAdoSzam_txb, label10) == true) {
                        if (SzamlaEUAdoSzam_txb.TextLength>0 && ABKezelo.EUAdoszamDuplikacio(SzamlaEUAdoSzam_txb.Text) != "")
                        {

                            MessageBox.Show("Az EU adószám már szerepel az adatbázisban!", "Figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.DialogResult = DialogResult.None;
                        }
                        else
                        {
                            partnerek = new Partner(0, SzamlaAdoSzam_txb.Text, SzamlaEUAdoSzam_txb.Text, TelefonSzam_txb.Text, Fax_txb.Text, EmailCim_tbx.Text,
                                                                  WebOldalCime_txb.Text, BankSzamlaSzam_tbx.Text, Convert.ToString(PartnerBesosolasa_cbox.SelectedItem), Convert.ToString(FizetesiMod_cbox.SelectedItem), Kedvezmeny_nud.Value, SzamlaMegjegyzes_txb.Text,
                                                                  Partnermegjegyzes_tbx.Text, Titulus_txb.Text, vezeteknev_tbx.Text, Keresztnev_tbx.Text, UtoNev_tbx.Text, Cegnev_txb.Text, SzamlaOrszag_txb.Text,
                                                                  SzamlaIrsz_txb.Text, SzamlaTelepules_txb.Text, SzamlaKozeruletNeve_txb.Text, (KozteruletJelleg)SzamlaKozteruletJellege_cbox.SelectedItem, SzamlaHazSzam_txb.Text,
                                                                  SzamlaEmeletAjto_txb.Text, PostaOrszag_txb.Text, PostaIranyitoSzam_txb.Text, Postatelepules_txb.Text, PostaKozteruletNeve_txb.Text, (KozteruletJelleg)PostaKozteruletJellege_cbox.SelectedItem, PostaHazSzam_txb.Text, PostaEmeletAjto_txb.Text);

                            ABKezelo.PartnerFelvitelMySQL(partnerek);
                            MessageBox.Show("A partner  rögzítése megtörtént!", "Figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }

                    } else if(KivetelKezeles.AdoszamEllenorzo(SzamlaAdoSzam_txb, label10) == false)
                    {

                            MessageBox.Show("Nem megfelelő adószám, vagy adóazonosító jel formátum","Figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.DialogResult = DialogResult.None;
                    }
                    else
                    {
                        MessageBox.Show("A partner adószáma már szerepel az adatbázisban!", "Figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.DialogResult = DialogResult.None;
                    }
                    } catch (Exception ex) {
                        MessageBox.Show(ex.Message, "Figyelem", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
        
        #region Magánszemély_chbox event

        private void maganSzemely_chbox_CheckedChanged(object sender, EventArgs e)//MAGÁNSZEMÉLY CHECKBOX VÁLTOZÁSA ESETÉN AZ ADÓAZONOSÍTÓ JEL LABEL FELIRATA ÉS A TBX-BA BEVÁRT ÉRTÉK IS VÁLTOZIK.
        {
            
            if (maganSzemely_chbox.Checked)
            {
                CegNev_lbl.Visible = false;
                Cegnev_txb.Visible = false;
                label10.Text = "Adóazonosító jel:";
                SzamlaAdoSzam_txb.MaxLength = 8;
                if (Cegnev_txb.Text == "Kötelező kitölteni!" || Cegnev_txb.TextLength>0)
                {
                    Cegnev_txb.Text = "Magánszemély";
                }
                if (SzamlaAdoSzam_txb.TextLength>0)
                {
                    SzamlaAdoSzam_txb.Text = "Kötelező kitölteni!";
                    SzamlaAdoSzam_txb.ForeColor = Color.Gray;
                }
            }
            else
            {
                SzamlaAdoSzam_txb.MaxLength = 11;
                CegNev_lbl.Visible = true;
                Cegnev_txb.Visible = true;
                Cegnev_txb.Text = "Kötelező kitölteni!";
                Cegnev_txb.ForeColor = Color.Gray;
                label10.Text = "Adószám:";
            }
        }
        #endregion

        #region Postacím_chbox event
        private void Postacim_chbox_CheckedChanged(object sender, EventArgs e)//HA A SZÁMLÁZÁSI CÍM ÉS A POSTÁZÁSI CÍM EGYEZIK
        {
            if (Postacim_chbox.Checked)
            {
                PostaOrszag_txb.Text = SzamlaOrszag_txb.Text;
                PostaIranyitoSzam_txb.Text = SzamlaIrsz_txb.Text;
                Postatelepules_txb.Text = SzamlaTelepules_txb.Text;
                PostaKozteruletNeve_txb.Text = SzamlaKozeruletNeve_txb.Text;
                PostaKozteruletJellege_cbox.SelectedItem = SzamlaKozteruletJellege_cbox.SelectedItem;
                PostaHazSzam_txb.Text = SzamlaHazSzam_txb.Text;
                PostaEmeletAjto_txb.Text = SzamlaEmeletAjto_txb.Text;
                PostaOrszag_txb.ReadOnly = true;
                Postatelepules_txb.ReadOnly = true;
                PostaIranyitoSzam_txb.ReadOnly = true;
                Postatelepules_txb.ReadOnly = true;
                PostaKozteruletNeve_txb.ReadOnly = true;
                PostaKozteruletJellege_cbox.Enabled = false;
                PostaHazSzam_txb.ReadOnly = true;
                PostaEmeletAjto_txb.ReadOnly = true;
            }
            else
            {
                PostaOrszag_txb.Text = string.Empty;
                PostaIranyitoSzam_txb.Text = string.Empty;
                Postatelepules_txb.Text = string.Empty;
                PostaKozteruletNeve_txb.Text = string.Empty;
                PostaKozteruletJellege_cbox.SelectedItem = string.Empty;
                PostaHazSzam_txb.Text = string.Empty;
                PostaEmeletAjto_txb.Text = string.Empty;
                PostaOrszag_txb.ReadOnly = false;
                Postatelepules_txb.ReadOnly = false;
                PostaIranyitoSzam_txb.ReadOnly = false;
                Postatelepules_txb.ReadOnly = false;
                PostaKozteruletNeve_txb.ReadOnly = false;
                PostaKozteruletJellege_cbox.Enabled = true;
                PostaHazSzam_txb.ReadOnly = false;
                PostaEmeletAjto_txb.ReadOnly = false;
            }
        }
        #endregion

        #region Fizetési mód_btn
        private void button2_Click(object sender, EventArgs e)//KategoriaFrm/FizetesiMod_tbp-re ugrik
        {
            KategoriaFrm frm = new KategoriaFrm("fizetesiMod");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                FizetesiMod = new BindingList<sqlString>(ABKezelo.FizetesModBetoltes());
                FizetesiMod_cbox.DataSource = FizetesiMod;
            }
            else
            {
                FizetesiMod = new BindingList<sqlString>(ABKezelo.FizetesModBetoltes());
                FizetesiMod_cbox.DataSource = FizetesiMod;
            }
        }
        #endregion

        #region TextBox_Enter/Leave
        private void vezeteknev_tbx_Enter(object sender, EventArgs e)
        {
            if (vezeteknev_tbx.Text == "Kötelező kitölteni!")
            {
                vezeteknev_tbx.Text = "";
                vezeteknev_tbx.ForeColor = Color.Black;
            }
        }

        private void vezeteknev_tbx_Leave(object sender, EventArgs e)
        {
            if (vezeteknev_tbx.Text == "")
            {
                vezeteknev_tbx.Text = "Kötelező kitölteni!";
                vezeteknev_tbx.ForeColor = Color.Gray;
            }
            else
            {
                vezeteknev_tbx.Text = vezeteknev_tbx.Text[0].ToString().ToUpper() + vezeteknev_tbx.Text.Substring(1);             
            }
           KivetelKezeles.TextBoxNemKotelezoStringe(errorProvider1, vezeteknev_tbx);
        }

        private void Keresztnev_tbx_Enter(object sender, EventArgs e)
        {
            if (Keresztnev_tbx.Text == "Kötelező kitölteni!")
            {
                Keresztnev_tbx.Text = "";
                Keresztnev_tbx.ForeColor = Color.Black;
            }          
        }

        private void Keresztnev_tbx_Leave(object sender, EventArgs e)
        {
            if (Keresztnev_tbx.Text == "")
            {
                Keresztnev_tbx.Text = "Kötelező kitölteni!";
                Keresztnev_tbx.ForeColor = Color.Gray;
            }
            else
            {
                Keresztnev_tbx.Text = Keresztnev_tbx.Text[0].ToString().ToUpper() + Keresztnev_tbx.Text.Substring(1);
            }
            KivetelKezeles.TextBoxNemKotelezoStringe(errorProvider1, Keresztnev_tbx);
        }

        private void SzamlaOrszag_txb_Enter(object sender, EventArgs e)
        {
            if (SzamlaOrszag_txb.Text == "Kötelező kitölteni!")
            {
                SzamlaOrszag_txb.Text = "";
                SzamlaOrszag_txb.ForeColor = Color.Black;
            }           
        }

        private void SzamlaOrszag_txb_Leave(object sender, EventArgs e)
        {
            if (SzamlaOrszag_txb.Text == "")
            {
                SzamlaOrszag_txb.Text = "Kötelező kitölteni!";
                SzamlaOrszag_txb.ForeColor = Color.Gray;
            }
            KivetelKezeles.TextBoxNemKotelezoStringe(errorProvider1, SzamlaOrszag_txb);
        }

        private void SzamlaIrsz_txb_Enter(object sender, EventArgs e)
        {
            if (SzamlaIrsz_txb.Text == "Kötelező kitölteni!")
            {
                SzamlaIrsz_txb.Text = "";
                SzamlaIrsz_txb.ForeColor = Color.Black;
            }
        }

        private void SzamlaIrsz_txb_Leave(object sender, EventArgs e)
        {
            if (SzamlaIrsz_txb.Text == "")
            {
                SzamlaIrsz_txb.Text = "Kötelező kitölteni!";
                SzamlaIrsz_txb.ForeColor = Color.Gray;
            }          
                KivetelKezeles.TextBoxIranyitoSzamRegex(errorProvider1, SzamlaIrsz_txb);      
        }

        private void SzamlaTelepules_txb_Enter(object sender, EventArgs e)
        {
            if (SzamlaTelepules_txb.Text == "Kötelező kitölteni!")
            {
                SzamlaTelepules_txb.Text = "";
                SzamlaTelepules_txb.ForeColor = Color.Black;
            }
        }

        private void SzamlaTelepules_txb_Leave(object sender, EventArgs e)
        {
            if (SzamlaTelepules_txb.Text == "")
            {
                SzamlaTelepules_txb.Text = "Kötelező kitölteni!";
                SzamlaTelepules_txb.ForeColor = Color.Gray;
            }
            else
            {
                SzamlaTelepules_txb.Text = SzamlaTelepules_txb.Text[0].ToString().ToUpper() + SzamlaTelepules_txb.Text.Substring(1);
            }
            KivetelKezeles.TextBoxNemKotelezoStringe(errorProvider1, SzamlaTelepules_txb);
        }

        private void SzamlaKozeruletNeve_txb_Enter(object sender, EventArgs e)
        {
            if (SzamlaKozeruletNeve_txb.Text == "Kötelező kitölteni!")
            {
                SzamlaKozeruletNeve_txb.Text = "";
                SzamlaKozeruletNeve_txb.ForeColor = Color.Black;
            }
        }

        private void SzamlaKozeruletNeve_txb_Leave(object sender, EventArgs e)
        {
            if (SzamlaKozeruletNeve_txb.Text == "")
            {
                SzamlaKozeruletNeve_txb.Text = "Kötelező kitölteni!";
                SzamlaKozeruletNeve_txb.ForeColor = Color.Gray;

            }
            else
            {
                SzamlaKozeruletNeve_txb.Text=SzamlaKozeruletNeve_txb.Text[0].ToString().ToUpper() + SzamlaKozeruletNeve_txb.Text.Substring(1);
            }
            KivetelKezeles.TextBoxNemKotelezoStringe(errorProvider1, SzamlaKozeruletNeve_txb);
        }

        private void SzamlaHazSzam_txb_Enter(object sender, EventArgs e)
        {
            if (SzamlaHazSzam_txb.Text == "Kötelező kitölteni!")
            {
                SzamlaHazSzam_txb.Text = "";
                SzamlaHazSzam_txb.ForeColor = Color.Black;
            }
        }

        private void SzamlaHazSzam_txb_Leave(object sender, EventArgs e)
        {
            if (SzamlaHazSzam_txb.Text == "")
            {
                SzamlaHazSzam_txb.Text = "Kötelező kitölteni!";
                SzamlaHazSzam_txb.ForeColor = Color.Gray;

            }
            KivetelKezeles.TextBoxPostaHazszamRegex(errorProvider1, SzamlaHazSzam_txb);

        }
        private void SzamlaAdoSzam_txb_Enter(object sender, EventArgs e)
        {
            if (SzamlaAdoSzam_txb.Text == "Kötelező kitölteni!")
            {
                SzamlaAdoSzam_txb.Text = "";
                SzamlaAdoSzam_txb.ForeColor = Color.Black;
            }
        }

        private void SzamlaAdoSzam_txb_Leave(object sender, EventArgs e)
        {
            if (SzamlaAdoSzam_txb.Text == "")
            {
                SzamlaAdoSzam_txb.Text = "Kötelező kitölteni!";
                SzamlaAdoSzam_txb.ForeColor = Color.Gray;

            }
            else if (SzamlaAdoSzam_txb.Text != "Kötelező kitölteni!")
            {
                if (label10.Text == "Adószám:")
                {
                    KivetelKezeles.TextBoxAdoSzamRegex(errorProvider1, SzamlaAdoSzam_txb);
                }
                else
                {
                    KivetelKezeles.TextBoxAdoJelRegex(errorProvider1, SzamlaAdoSzam_txb);
                }

            }
           
        }

        private void Cegnev_txb_Enter(object sender, EventArgs e)
        {
            if (Cegnev_txb.Text == "Kötelező kitölteni!")
            {
                Cegnev_txb.Text = "";
                Cegnev_txb.ForeColor = Color.Black;
            }
        }

        private void Cegnev_txb_Leave(object sender, EventArgs e)
        {
            if (Cegnev_txb.Text == "")
            {
                Cegnev_txb.Text = "Kötelező kitölteni!";

                Cegnev_txb.ForeColor = Color.Gray;
            }
            else
            {
                Cegnev_txb.Text = Cegnev_txb.Text[0].ToString().ToUpper()+Cegnev_txb.Text.Substring(1);
            }         
        }

        private void TelefonSzam_txb_Enter(object sender, EventArgs e)
        {
            if (TelefonSzam_txb.Text == "Kötelező kitölteni!")
            {
                TelefonSzam_txb.Text = "";
                TelefonSzam_txb.ForeColor = Color.Black;
            }
        }

        private void TelefonSzam_txb_Leave(object sender, EventArgs e)
        {
            if (TelefonSzam_txb.Text == "")
            {
                TelefonSzam_txb.Text = "Kötelező kitölteni!";
                TelefonSzam_txb.ForeColor = Color.Gray;

            }
            KivetelKezeles.NemKotelezoTelefonSzamRegex(errorProvider1, TelefonSzam_txb);
        }
        #endregion

        #region Mouse_enter
        private void button4_MouseEnter(object sender, EventArgs e)
        {
           if (idezettOldal != "megtekint")
            {
                toolTip1.Active = true;
                Point x = new Point(button4.Right + 20, button4.Top-17 + this.Height / 2);
                toolTip1.SetToolTip(button4, "uff");
                toolTip1.Show("Adatok\nmódosítása.", this, x);
            }
        }

        private void button4_MouseLeave(object sender, EventArgs e)
        {
            if (idezettOldal != "megtekint")
            {
                toolTip1.Active = false;
            }
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            if (idezettOldal != "megtekint")
            {
                toolTip1.Active = true;
                Point x = new Point(button4.Right + 20, button4.Top+6 + this.Height / 2);
                toolTip1.SetToolTip(button4, "uff");
                toolTip1.Show("Adatok\nmódosítása.", this, x);
            }
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            if (idezettOldal != "megtekint")
            {
                toolTip1.Active = false;
            }
        }
        #endregion

        #region TextChanged
        private void vezeteknev_tbx_TextChanged(object sender, EventArgs e)//KIVÉTEL KEZELÉS
        {
            if (vezeteknev_tbx.Text != "Kötelező kitölteni!")
            {
                KivetelKezeles.TextBoxNemKotelezoStringe(errorProvider1, vezeteknev_tbx);
            }
        }

        private void Keresztnev_tbx_TextChanged(object sender, EventArgs e)
        {
            if (Keresztnev_tbx.Text != "Kötelező kitölteni!")
            {
                KivetelKezeles.TextBoxNemKotelezoStringe(errorProvider1, Keresztnev_tbx);
            }
        }

        private void SzamlaOrszag_txb_TextChanged(object sender, EventArgs e)
        {
            if (SzamlaOrszag_txb.Text != PostaOrszag_txb.Text && Postacim_chbox.Checked)
            {
                Postacim_chbox.Checked = false;
            }
            if (SzamlaOrszag_txb.Text != "Kötelező kitölteni!")
            {
                KivetelKezeles.TextBoxNemKotelezoStringe(errorProvider1, SzamlaOrszag_txb);
            }
        }

        private void SzamlaIrsz_txb_TextChanged(object sender, EventArgs e)
        {
            if (SzamlaIrsz_txb.Text != PostaOrszag_txb.Text && Postacim_chbox.Checked)
            {
                Postacim_chbox.Checked = false;
            }
            if (SzamlaIrsz_txb.Text != "Kötelező kitölteni!")
            {
                KivetelKezeles.TextBoxIranyitoSzamRegex(errorProvider1, SzamlaIrsz_txb);
            }
        }

        private void SzamlaTelepules_txb_TextChanged(object sender, EventArgs e)
        {
            if (SzamlaTelepules_txb.Text != Postatelepules_txb.Text && Postacim_chbox.Checked)
            {
                Postacim_chbox.Checked = false;
            }
            if (SzamlaTelepules_txb.Text != "Kötelező kitölteni!")
            {
                KivetelKezeles.TextBoxNemKotelezoStringe(errorProvider1, SzamlaTelepules_txb);
            }
        }

        private void SzamlaKozeruletNeve_txb_TextChanged(object sender, EventArgs e)
        {
            if (SzamlaKozeruletNeve_txb.Text != PostaKozteruletNeve_txb.Text && Postacim_chbox.Checked)
            {
                Postacim_chbox.Checked = false;
            }
            if (SzamlaKozeruletNeve_txb.Text != "Kötelező kitölteni!")
            {
                KivetelKezeles.TextBoxNemKotelezoStringe(errorProvider1, SzamlaKozeruletNeve_txb);
            }
        }
        private void SzamlaKozteruletJellege_cbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SzamlaKozteruletJellege_cbox.SelectedItem != PostaKozteruletJellege_cbox.SelectedItem && Postacim_chbox.Checked)
            {
                Postacim_chbox.Checked = false;
            }
        }
        private void SzamlaHazSzam_txb_TextChanged(object sender, EventArgs e)
        {
            if (SzamlaHazSzam_txb.Text != PostaHazSzam_txb.Text && Postacim_chbox.Checked)
            {
                Postacim_chbox.Checked = false;
            }
            if (SzamlaHazSzam_txb.Text != "Kötelező kitölteni!")
            {
                KivetelKezeles.TextBoxPostaHazszamRegex(errorProvider1, SzamlaHazSzam_txb);
            }
        }

        private void SzamlaEmeletAjto_txb_TextChanged(object sender, EventArgs e)
        {
            if (SzamlaEmeletAjto_txb.Text != PostaEmeletAjto_txb.Text && Postacim_chbox.Checked)
            {
                Postacim_chbox.Checked = false;
            }
            KivetelKezeles.TextBoxPostaEmeletAjtoRegex(errorProvider1, SzamlaEmeletAjto_txb);
        }

        private void SzamlaAdoSzam_txb_TextChanged(object sender, EventArgs e)
        {
            if (SzamlaAdoSzam_txb.Text!= "Kötelező kitölteni!")
            {
                if (label10.Text=="Adószám:")
                {
                    KivetelKezeles.TextBoxAdoSzamRegex(errorProvider1, SzamlaAdoSzam_txb);
                }
                else
                {
                    KivetelKezeles.TextBoxAdoJelRegex(errorProvider1, SzamlaAdoSzam_txb);
                }
               
            }          
        }

        private void TelefonSzam_txb_TextChanged(object sender, EventArgs e)
        {             
                KivetelKezeles.NemKotelezoTelefonSzamRegex(errorProvider1, TelefonSzam_txb);          
        }

        private void Titulus_txb_TextChanged(object sender, EventArgs e)
        {
            KivetelKezeles.TextBoxNemKotelezoStringe(errorProvider1, Titulus_txb);           
        }

        private void UtoNev_tbx_TextChanged(object sender, EventArgs e)
        {
            KivetelKezeles.TextBoxNemKotelezoStringe(errorProvider1, UtoNev_tbx);
        }

        private void Fax_txb_TextChanged(object sender, EventArgs e)
        {
            KivetelKezeles.FaxRegex(errorProvider1,Fax_txb);
        }

        private void EmailCim_tbx_TextChanged(object sender, EventArgs e)
        {
            KivetelKezeles.TextBoxEmail(errorProvider1, EmailCim_tbx);
        }

        private void WebOldalCime_txb_TextChanged(object sender, EventArgs e)
        {
           KivetelKezeles.TextBoxWebCim(errorProvider1, WebOldalCime_txb);
        }

        private void BankSzamlaSzam_tbx_TextChanged(object sender, EventArgs e)
        {
            KivetelKezeles.NemKotelezoBankSzamlaSzame(errorProvider1, BankSzamlaSzam_tbx);
        }

        private void PostaOrszag_txb_TextChanged(object sender, EventArgs e)
        {
           KivetelKezeles.TextBoxNemKotelezoStringe(errorProvider1, PostaOrszag_txb);
        }

        private void PostaIranyitoSzam_txb_TextChanged(object sender, EventArgs e)
        { 
                KivetelKezeles.PostaTextBoxIranyitoSzamRegex(errorProvider1, PostaIranyitoSzam_txb);         
        }

        private void Postatelepules_txb_TextChanged(object sender, EventArgs e)
        {
           KivetelKezeles.TextBoxNemKotelezoStringe(errorProvider1, Postatelepules_txb);
        
        }
        private void PostaKozteruletNeve_txb_TextChanged(object sender, EventArgs e)
        {
           KivetelKezeles.TextBoxNemKotelezoStringe(errorProvider1, PostaKozteruletNeve_txb);           
        }

        private void PostaHazSzam_txb_TextChanged(object sender, EventArgs e)
        {
                KivetelKezeles.TextBoxPostaHazszamRegex(errorProvider1, PostaHazSzam_txb);       
        }

        private void PostaEmeletAjto_txb_TextChanged(object sender, EventArgs e)
        {
            KivetelKezeles.TextBoxPostaEmeletAjtoRegex(errorProvider1, PostaEmeletAjto_txb);
        }

        private void SzamlaEUAdoSzam_txb_TextChanged(object sender, EventArgs e)
        {
            KivetelKezeles.TextBoxNemKotelezoStringe(errorProvider1, SzamlaEUAdoSzam_txb);
        }
        #endregion

        #region TextBox_Leave
        private void PostaIranyitoSzam_txb_Leave(object sender, EventArgs e)//KIVÉTELKEZELÉS ÉS NAGY KEZDŐBETÜ
        {
                KivetelKezeles.PostaTextBoxIranyitoSzamRegex(errorProvider1, PostaIranyitoSzam_txb);          
        }

        private void Postatelepules_txb_Leave(object sender, EventArgs e)
        {
            if (Postatelepules_txb.TextLength > 0)
            {
                Postatelepules_txb.Text = Postatelepules_txb.Text[0].ToString().ToUpper() + Postatelepules_txb.Text.Substring(1);
            }
            KivetelKezeles.TextBoxNemKotelezoStringe(errorProvider1, SzamlaTelepules_txb);
        }

        private void PostaKozteruletNeve_txb_Leave(object sender, EventArgs e)
        {
            if (PostaKozteruletNeve_txb.TextLength > 0)
            {
                PostaKozteruletNeve_txb.Text = PostaKozteruletNeve_txb.Text[0].ToString().ToUpper() + PostaKozteruletNeve_txb.Text.Substring(1);
            }
            KivetelKezeles.TextBoxNemKotelezoStringe(errorProvider1, SzamlaTelepules_txb);
        }

        private void UtoNev_tbx_Leave(object sender, EventArgs e)
        {
            if (UtoNev_tbx.TextLength>0)
            {
                UtoNev_tbx.Text = UtoNev_tbx.Text[0].ToString().ToUpper() + UtoNev_tbx.Text.Substring(1);
            }
        }
        private void PostaHazSzam_txb_Leave(object sender, EventArgs e)
        {
            KivetelKezeles.TextBoxPostaHazszamRegex(errorProvider1, PostaHazSzam_txb);
        }

        private void Fax_txb_Leave(object sender, EventArgs e)
        {
            KivetelKezeles.FaxRegex(errorProvider1, Fax_txb);
        }

        private void WebOldalCime_txb_Leave(object sender, EventArgs e)
        {
            KivetelKezeles.TextBoxWebCim(errorProvider1, WebOldalCime_txb);
        }

        private void BankSzamlaSzam_tbx_Leave(object sender, EventArgs e)
        {
            KivetelKezeles.NemKotelezoBankSzamlaSzame(errorProvider1, BankSzamlaSzam_tbx);
        }

        private void SzamlaEUAdoSzam_txb_Leave(object sender, EventArgs e)
        {
            if (SzamlaEUAdoSzam_txb.TextLength>0)
            {
                SzamlaEUAdoSzam_txb.Text = SzamlaEUAdoSzam_txb.Text.ToString().ToUpper();
            }

        }
        #endregion

        private void PostaEmeletAjto_txb_Leave(object sender, EventArgs e)//KIVÉTEL KEZELÉSEK
        {
            KivetelKezeles.TextBoxPostaEmeletAjtoRegex(errorProvider1, PostaEmeletAjto_txb);
        }

        private void SzamlaEmeletAjto_txb_Leave(object sender, EventArgs e)
        {
            KivetelKezeles.TextBoxPostaEmeletAjtoRegex(errorProvider1, SzamlaEmeletAjto_txb);
        }
        private void EmailCim_tbx_Leave(object sender, EventArgs e)
        {
            KivetelKezeles.TextBoxEmail(errorProvider1, EmailCim_tbx);
        }
        private void PostaOrszag_txb_Leave(object sender, EventArgs e)
        {
            KivetelKezeles.TextBoxNemKotelezoStringe(errorProvider1, PostaOrszag_txb);
        }

        private void segitseg_btn_Click(object sender, EventArgs e)
        {
            Helpfrm help = new Helpfrm(KivetelKezeles.getHelp("Partnerek menüpont"));
            help.ShowDialog();
        }

        private void SzamlaIrsz_txb_KeyPress(object sender, KeyPressEventArgs e)
        {
            KivetelKezeles.StringLetiltoDecimal(SzamlaIrsz_txb, e);
        }

        private void PostaIranyitoSzam_txb_KeyPress(object sender, KeyPressEventArgs e)
        {
            KivetelKezeles.StringLetiltoDecimal(PostaIranyitoSzam_txb, e);
        }

        private void SzamlaAdoSzam_txb_KeyPress(object sender, KeyPressEventArgs e)
        {
            KivetelKezeles.StringLetiltoDecimal2(SzamlaAdoSzam_txb,e);
        }

        private void BankSzamlaSzam_tbx_KeyPress(object sender, KeyPressEventArgs e)
        {
            KivetelKezeles.StringLetiltoDecimal2(BankSzamlaSzam_tbx, e);
        }

    }
}

