using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Szakdolgozat3
{
    public struct sqlString
    {
        int sqlID;
        string value;
        public int SQLID { get => sqlID; set => sqlID = value; }
        public string Value { get => value; set => this.value = value; }
        public override string ToString()
        {
            return value;
        }
    }
    public partial class KategoriaFrm : Form
    {

        private BindingList<sqlString> ideiglenesTermekKategoria;
        private BindingList<Termek.AFA> ideiglenesAFA;
        private BindingList<sqlString> ideiglenesPartnerKategoria;
        private BindingList<sqlString> ideiglenesFizetesiMod;
        private BindingList<sqlString> ideiglenesMertekEgyseg;
        private Partner cegPartner = ABKezelo.PartnerKivalasztasKategoriaAlapjan("1");

        private int listBoxStartIndex;
        private Termek.AFA tmp = new Termek.AFA();
        private sqlString tmp2 = new sqlString();


        public static string StringCheck(BindingList<sqlString> list, sqlString ertek)
        {
            if (string.IsNullOrEmpty(ertek.Value))
            {
                //throw new ArgumentException("A beviteli mező nem lehet üres!");
                return "A beviteli mező nem lehet üres!";
            }

            else
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Value == ertek.Value)
                    {
                        return "Nem lehet duplikáció!";
                    }
                }
                //throw new ArgumentException("nem lehet duplikáció!");

            }
            return "";
        }

        public KategoriaFrm(string kategoriaFajta, int selectCheckBoxIndex = -1)
        {
            InitializeComponent();
            this.ControlBox = false;
            this.Width = 420;
            this.Height = 480;
          
            if (cegPartner != null)
            {
                CegTulajFelvitel_btn.Enabled = false;
            }
            listBoxStartIndex = selectCheckBoxIndex;
            if (kategoriaFajta == "termek")
            {
                tabcontrol1.TabPages.Remove(afaKategoria_tbp);
                tabcontrol1.TabPages.Remove(partnerKategoria_tbp);
                tabcontrol1.TabPages.Remove(fizetesiMod_tbp);
                tabcontrol1.TabPages.Remove(mennyisegEgyseg_tbp);
                tabcontrol1.TabPages.Remove(CegAdatok_tbp);
                tabcontrol1.Height = 440;
            }
            else if (kategoriaFajta == "partner")
            {
                tabcontrol1.TabPages.Remove(afaKategoria_tbp);
                tabcontrol1.TabPages.Remove(termekKategoria_tbp);
                tabcontrol1.TabPages.Remove(fizetesiMod_tbp);
                tabcontrol1.TabPages.Remove(mennyisegEgyseg_tbp);
                tabcontrol1.TabPages.Remove(CegAdatok_tbp);
                tabcontrol1.Height = 440;
            }
            else if (kategoriaFajta == "afa")
            {
                tabcontrol1.TabPages.Remove(partnerKategoria_tbp);
                tabcontrol1.TabPages.Remove(termekKategoria_tbp);
                tabcontrol1.TabPages.Remove(fizetesiMod_tbp);
                tabcontrol1.TabPages.Remove(mennyisegEgyseg_tbp);
                tabcontrol1.TabPages.Remove(CegAdatok_tbp);
                tabcontrol1.Height = 440;

            }
            else if (kategoriaFajta == "mennyisegEgyseg")
            {
                tabcontrol1.TabPages.Remove(afaKategoria_tbp);
                tabcontrol1.TabPages.Remove(termekKategoria_tbp);
                tabcontrol1.TabPages.Remove(partnerKategoria_tbp);
                tabcontrol1.TabPages.Remove(fizetesiMod_tbp);
                tabcontrol1.TabPages.Remove(CegAdatok_tbp);
                tabcontrol1.Height = 440;
            }
            else if (kategoriaFajta == "fizetesiMod")
            {
                tabcontrol1.TabPages.Remove(afaKategoria_tbp);
                tabcontrol1.TabPages.Remove(termekKategoria_tbp);
                tabcontrol1.TabPages.Remove(partnerKategoria_tbp);
                tabcontrol1.TabPages.Remove(mennyisegEgyseg_tbp);
                tabcontrol1.TabPages.Remove(CegAdatok_tbp);
                tabcontrol1.Height = 440;
            }
            else if (kategoriaFajta == "cegAdatok")
            {
                tabcontrol1.TabPages.Remove(afaKategoria_tbp);
                tabcontrol1.TabPages.Remove(termekKategoria_tbp);
                tabcontrol1.TabPages.Remove(partnerKategoria_tbp);
                tabcontrol1.TabPages.Remove(mennyisegEgyseg_tbp);
                tabcontrol1.TabPages.Remove(fizetesiMod_tbp);
                tabcontrol1.Height = 440;
            }

            else
            {

            }
        }
        public KategoriaFrm()
        {

            InitializeComponent();
            this.ControlBox = false;
            listBox2.SelectedIndex = -1;
         
            if (cegPartner != null)
            {
                CegTulajFelvitel_btn.Enabled = false;
            }
            Bezaras_btn1.Visible = false;
            Bezaras_btn2.Visible = false;
            Bezaras_btn3.Visible = false;
            button15.Visible = false;
            Bezaras_btn5.Visible = false;         
            SzamlaOrszag_txb.Text = "Magyarország";
            PostaOrszag_txb.Text = "Magyarország";           
            SzamlaOrszag_txb.ReadOnly = true;
            PostaOrszag_txb.ReadOnly = true;
            SzamlaOrszag_txb.Enter += (s, e) => { SzamlaIrsz_txb.Focus(); };
            PostaOrszag_txb.Enter += (s, e) => { PostaIranyitoSzam_txb.Focus(); };
            textBox1.Text = "0";
          

        }
        private void Kategoria_Load(object sender, EventArgs e)
        {
            this.BackColor = SystemColors.GradientInactiveCaption;
            afaKategoria_tbp.BackColor = SystemColors.GradientInactiveCaption;
            termekKategoria_tbp.BackColor = SystemColors.GradientInactiveCaption;
            partnerKategoria_tbp.BackColor = SystemColors.GradientInactiveCaption;
            fizetesiMod_tbp.BackColor = SystemColors.GradientInactiveCaption;
            mennyisegEgyseg_tbp.BackColor = SystemColors.GradientInactiveCaption;
            CegAdatok_tbp.BackColor = SystemColors.GradientInactiveCaption;

            ideiglenesTermekKategoria = new BindingList<sqlString>(ABKezelo.TermekKategoriakBetoltes());
            ideiglenesAFA = new BindingList<Termek.AFA>(ABKezelo.AfaKategoriakBetoltes());
            ideiglenesPartnerKategoria = new BindingList<sqlString>(ABKezelo.PartnerBesorolasBetoltes());
            ideiglenesMertekEgyseg = new BindingList<sqlString>(ABKezelo.TermekMennyisegEgysegBetoltes());
            ideiglenesFizetesiMod = new BindingList<sqlString>(ABKezelo.FizetesModBetoltes());

            listBox1.DataSource = ideiglenesTermekKategoria;
            listBox2.DataSource = ideiglenesAFA;
            listBox3.DataSource = ideiglenesPartnerKategoria;
            listBox4.DataSource = ideiglenesFizetesiMod;
            listBox5.DataSource = ideiglenesMertekEgyseg;

            if(ideiglenesTermekKategoria.Count!=0) listBox1.SelectedIndexChanged += new EventHandler(listBox1_SelectedIndexChanged);
            if(ideiglenesAFA.Count!=0) listBox2.SelectedIndexChanged += new EventHandler(listBox2_SelectedIndexChanged);
            if(ideiglenesPartnerKategoria.Count!=0) listBox3.SelectedIndexChanged += new EventHandler(listBox3_SelectedIndexChanged);
            if(ideiglenesFizetesiMod.Count!=0) listBox4.SelectedIndexChanged += new EventHandler(listBox4_SelectedIndexChanged);
            if(ideiglenesMertekEgyseg.Count!=0) listBox5.SelectedIndexChanged += new EventHandler(listBox5_SelectedIndexChanged);
            try {
                if (listBoxStartIndex > -1) {
                    if (listBox1.Items.Count > listBoxStartIndex) listBox1.SelectedIndex = listBoxStartIndex;
                    if (listBox2.Items.Count > listBoxStartIndex) listBox2.SelectedIndex = listBoxStartIndex;
                    if (listBox3.Items.Count > listBoxStartIndex) listBox3.SelectedIndex = listBoxStartIndex;
                    if (listBox4.Items.Count > listBoxStartIndex) listBox3.SelectedIndex = listBoxStartIndex;
                    if (listBox5.Items.Count > listBoxStartIndex) listBox5.SelectedIndex = listBoxStartIndex;
                }
            } catch (Exception) {}
            if (ideiglenesTermekKategoria.Count == 0) listBox1.SelectedIndexChanged += new EventHandler(listBox1_SelectedIndexChanged);
            if (ideiglenesAFA.Count == 0) listBox2.SelectedIndexChanged += new EventHandler(listBox2_SelectedIndexChanged);
            if (ideiglenesPartnerKategoria.Count == 0) listBox3.SelectedIndexChanged += new EventHandler(listBox3_SelectedIndexChanged);
            if (ideiglenesFizetesiMod.Count == 0) listBox4.SelectedIndexChanged += new EventHandler(listBox4_SelectedIndexChanged);
            if (ideiglenesMertekEgyseg.Count == 0) listBox5.SelectedIndexChanged += new EventHandler(listBox5_SelectedIndexChanged);

            Titulus_lbl.Visible = false;
            Titulus_txb.Visible = false;
            UtoNev_lbl.Visible = false;
            UtoNev_tbx.Visible = false;

            SzamlaKozteruletJellege_cbox.DataSource = Enum.GetValues(typeof(KozteruletJelleg));
            PostaKozteruletJellege_cbox.DataSource = Enum.GetValues(typeof(KozteruletJelleg));

            if (cegPartner != null)
            {
               
                SzamlaAdoSzam_txb.Text = cegPartner.AdoSzam.ToString();
                SzamlaEUAdoSzam_txb.Text = cegPartner.EUAdoszam1.ToString();
                TelefonSzam_txb.Text = cegPartner.TelefonSzam.ToString();
                Fax_txb.Text = cegPartner.Fax.ToString();
                EmailCim_tbx.Text = cegPartner.Email;
                WebOldalCime_txb.Text = cegPartner.WebCim;
                BankSzamlaSzam_tbx.Text = cegPartner.BankSzamlaSzam;              
                Titulus_txb.Text = cegPartner.Titulus;
                vezeteknev_tbx.Text = cegPartner.VezetekNev;
                Keresztnev_tbx.Text = cegPartner.KeresztNev;
                UtoNev_tbx.Text = cegPartner.UtoNev;
                Cegnev_txb.Text = cegPartner.CegNev;
                SzamlaOrszag_txb.Text = cegPartner.Orszag;
                SzamlaIrsz_txb.Text = cegPartner.Irsz;
                SzamlaTelepules_txb.Text = cegPartner.Telepules;
                SzamlaKozeruletNeve_txb.Text = cegPartner.Kozterulet;
                SzamlaKozteruletJellege_cbox.SelectedItem = cegPartner.KozteruletJelleg;
                SzamlaHazSzam_txb.Text = cegPartner.HazSzam;
                SzamlaEmeletAjto_txb.Text = cegPartner.EmeletAjto;
                PostaOrszag_txb.Text = cegPartner.PostaOrszag;
                PostaIranyitoSzam_txb.Text = cegPartner.PostaIrsz;
                Postatelepules_txb.Text = cegPartner.PostaTelepules;
                PostaKozteruletNeve_txb.Text = cegPartner.PostaKozterulet;
                PostaKozteruletJellege_cbox.SelectedItem = cegPartner.PostaKozteruletJelleg.ToString();
                PostaHazSzam_txb.Text = cegPartner.PostaHazSzam;
                PostaEmeletAjto_txb.Text = cegPartner.PostaEmeletAjto;               
            }
            if (vezeteknev_tbx.Text == "")
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
        }

        private void AfaHozzaadas_btn_Click(object sender, EventArgs e)
        {
            try
            {
                Termek.AFA tmp = new Termek.AFA();
               
                tmp.Value = Convert.ToDecimal(textBox1.Text);
                tmp.SQLID = -1;
                if (checkIfDuplicateValue(tmp.Value))
                {
                    MessageBox.Show("Nem lehet duplikáció!", "Figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
               
                else
                {

                    tmp.SQLID = ABKezelo.AfaFelvitel( tmp.Value);
                    ideiglenesAFA.Add(tmp);
                    MessageBox.Show("Az ÁFA besorolás felvitele megtörtént!", "Figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    listBox2.SelectedIndex = listBox2.Items.Count - 1;
                }
                textBox1.Text = "0";
              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

      
       
        private bool checkIfDuplicateValue(decimal value)
        {
            for (int i = 0; i < ideiglenesAFA.Count; i++)
            {
                if (ideiglenesAFA[i].Value == value) return true;//29.-ei
            }
            return false;
        }

        
        private void AfaTorles_btn_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex > -1 && ABKezelo.TermekiValasztasAfaAlapjan(Convert.ToInt32(ideiglenesAFA[listBox2.SelectedIndex].Value)) == null)
            {


                if (MessageBox.Show("Biztos hogy törli a kiválasztott Áfa típust?", "Figyelem", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    ABKezelo.AfaTorles(ideiglenesAFA[listBox2.SelectedIndex].SQLID);
                    ideiglenesAFA.RemoveAt(listBox2.SelectedIndex);
                    MessageBox.Show("Az ÁFA besorolás törlése megtörtént!", "Figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            else
            {
                MessageBox.Show("Az érték nem törölhető, mert hozzá van rendelve egy termékhez", "Figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void kategoriaHozzaadasa_btn_Click(object sender, EventArgs e)
        {
            try
            {
                tmp2.Value = Megnevezes_tbx.Text;
                string error = StringCheck(ideiglenesTermekKategoria, tmp2);
                if (error.Length == 0)
                {
                    tmp2.SQLID = ABKezelo.TermekKategoriaFelvitel(tmp2.Value);
                    ideiglenesTermekKategoria.Add(tmp2);
                    listBox1.SelectedIndex = listBox1.Items.Count - 1;

                    MessageBox.Show("A termék kategória felvitele megtörtént!", "Figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(error, "Figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

      

        private void KategoriaTorlese_btn_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1 && MessageBox.Show("Biztos hogy törli a kiválasztott kategóriát?", "Figyelem", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                ABKezelo.TermekKategoriaTorles(ideiglenesTermekKategoria[listBox1.SelectedIndex].SQLID);
                ideiglenesTermekKategoria.RemoveAt(listBox1.SelectedIndex);
                MessageBox.Show("A termék kategória törlése megtörtént!", "Figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try {
                Megnevezes_tbx.Text = ideiglenesTermekKategoria[listBox1.SelectedIndex].Value;
            } catch (Exception) { }
            
            
        }
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

            //if (listBox2.SelectedIndex > -1 && listBox2.SelectedIndex < ideiglenesAFA.Count)
            //{
            try {
           
                textBox1.Text = ideiglenesAFA[listBox2.SelectedIndex].Value.ToString();
            } catch (Exception) {}
                
            //}

        }
        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try {
                Besorolas_txb.Text = ideiglenesPartnerKategoria[listBox3.SelectedIndex].Value;
            } catch (Exception) { }
        }
        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {   
            try {
                FizetesiMod_tbx.Text = ideiglenesFizetesiMod[listBox4.SelectedIndex].Value;
            } catch (Exception) { }
            
        }
        private void listBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            try {
                mennyisegEgysege_tbx.Text = ideiglenesMertekEgyseg[listBox5.SelectedIndex].Value;
            } catch (Exception) { }
        }

        private void BesorolasHozzaadas_btn_Click(object sender, EventArgs e)
        {
            try
            {
                tmp2.Value = Besorolas_txb.Text;
                string error = StringCheck(ideiglenesPartnerKategoria, tmp2);
                if (error.Length == 0)
                {
                    tmp2.SQLID = ABKezelo.PartnerBesorolasFelvitel(tmp2.Value);
                    ideiglenesPartnerKategoria.Add(tmp2);
                    listBox3.SelectedIndex = listBox3.Items.Count - 1;
                    MessageBox.Show("A partner besorolás felvitele megtörtént!", "Figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(error, "Figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

       

        private void BesorolasTorles_btn_Click(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex > -1 && ABKezelo.PartnerKivalasztasKategoriaAlapjan(listBox3.SelectedItem.ToString()) == null)
            {


                if (MessageBox.Show("Biztos hogy törli a kiválasztott partner besorolást?", "Figyelem", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    ABKezelo.PartnerBesorolasTorles(ideiglenesPartnerKategoria[listBox3.SelectedIndex].SQLID);
                    ideiglenesPartnerKategoria.RemoveAt(listBox3.SelectedIndex);
                    MessageBox.Show("A partner besorolás törlése megtörtént!", "Figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            else
            {
                MessageBox.Show("Az érték nem törölhető, mert hozzá van rendelve egy partnerhez", "Figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void FizetesiModHozzaadasa_btn_Click(object sender, EventArgs e)
        {
            try
            {

                tmp2.Value = FizetesiMod_tbx.Text;
                string error = StringCheck(ideiglenesFizetesiMod, tmp2);
                if (error.Length == 0)
                {
                    tmp2.SQLID = ABKezelo.FizetesModFelvitel(tmp2.Value);
                    ideiglenesFizetesiMod.Add(tmp2);
                    try { listBox4.SelectedIndex = listBox4.Items.Count - 1; } catch (Exception) { }

                    MessageBox.Show("A fizetési mód felvitele megtörtént!", "Figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    MessageBox.Show(error, "Figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                
            }
        }

      

        private void FizetesiModTorlese_btn_Click(object sender, EventArgs e)
        {
            if (listBox4.SelectedIndex > -1 && ABKezelo.PartnerKivalasztasFizetesiModAlapjan(listBox4.SelectedItem.ToString()) == null)
            {


                if (MessageBox.Show("Biztos hogy törli a kiválasztott fizetési módot?", "Figyelem", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    ABKezelo.FizetesModTorles(ideiglenesFizetesiMod[listBox4.SelectedIndex].SQLID);
                    ideiglenesFizetesiMod.RemoveAt(listBox4.SelectedIndex);
                    MessageBox.Show("A fizetési mód törlése megtörtént!", "Figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            else
            {
                MessageBox.Show("Az érték nem törölhető, mert hozzá van rendelve egy partnerhez", "Figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CegTulajFelvitel_btn_Click(object sender, EventArgs e)
        {
            KivetelKezeles.PostaEllenorzo(PostaIranyitoSzam_txb, Postatelepules_txb, PostaKozteruletNeve_txb, PostaHazSzam_txb);
            if (vezeteknev_tbx.Text != "Kötelező kitölteni!" && Keresztnev_tbx.Text != "Kötelező kitölteni!" && Cegnev_txb.Text != "Kötelező kitölteni!" && PostaOrszag_txb.Text != "Kötelező kitölteni!" && SzamlaIrsz_txb.Text != "Kötelező kitölteni!" &&
                       SzamlaTelepules_txb.Text != "Kötelező kitölteni!" && SzamlaKozeruletNeve_txb.Text != "Kötelező kitölteni!" && SzamlaHazSzam_txb.Text != "Kötelező kitölteni!" && SzamlaAdoSzam_txb.Text != "Kötelező kitölteni!" && TelefonSzam_txb.Text != "Kötelező kitölteni!") {
                try {
                    if (ABKezelo.AdoszamDuplikacio(SzamlaAdoSzam_txb.Text) =="") {
                        Partner partnerek = new Partner(-1, SzamlaAdoSzam_txb.Text, SzamlaEUAdoSzam_txb.Text, TelefonSzam_txb.Text, Fax_txb.Text, EmailCim_tbx.Text,
                                WebOldalCime_txb.Text, BankSzamlaSzam_tbx.Text, "1", "", 0, "",
                                "", Titulus_txb.Text, vezeteknev_tbx.Text, Keresztnev_tbx.Text, UtoNev_tbx.Text, Cegnev_txb.Text, SzamlaOrszag_txb.Text,
                                SzamlaIrsz_txb.Text, SzamlaTelepules_txb.Text, SzamlaKozeruletNeve_txb.Text, (KozteruletJelleg)SzamlaKozteruletJellege_cbox.SelectedItem, SzamlaHazSzam_txb.Text,
                                SzamlaEmeletAjto_txb.Text, PostaOrszag_txb.Text, PostaIranyitoSzam_txb.Text, Postatelepules_txb.Text, PostaKozteruletNeve_txb.Text, (KozteruletJelleg)PostaKozteruletJellege_cbox.SelectedItem, PostaHazSzam_txb.Text, PostaEmeletAjto_txb.Text);
                        ABKezelo.PartnerFelvitel(partnerek);
                        cegPartner = partnerek;


                        MessageBox.Show("A cégtulajdonos adatainak rögzítése sikeres volt.", "Figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    } else {
                        MessageBox.Show("A cégtulajdonos adószáma már szerepel az adatbázisban!", "Figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.DialogResult = DialogResult.None;
                    }
                } catch (Exception ex) {

                    MessageBox.Show(ex.Message, "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.DialogResult = DialogResult.None;
                }
            }
            if (cegPartner != null)
            {
                CegTulajFelvitel_btn.Enabled = false;
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

        private void CegTulajModositas_btn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Biztos, hogy módosítja az adatokat?", "Figyelem!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    if ((ABKezelo.AdoszamDuplikacio(SzamlaAdoSzam_txb.Text) =="") || SzamlaAdoSzam_txb.Text == cegPartner.AdoSzam)
                    {
                        Partner partnerek = new Partner(cegPartner.PartnerAzonosito,SzamlaAdoSzam_txb.Text, SzamlaEUAdoSzam_txb.Text, TelefonSzam_txb.Text, Fax_txb.Text, EmailCim_tbx.Text,
                            WebOldalCime_txb.Text, BankSzamlaSzam_tbx.Text, "1", "", 0, "",
                            "", Titulus_txb.Text, vezeteknev_tbx.Text, Keresztnev_tbx.Text, UtoNev_tbx.Text, Cegnev_txb.Text, SzamlaOrszag_txb.Text,
                            SzamlaIrsz_txb.Text, SzamlaTelepules_txb.Text, SzamlaKozeruletNeve_txb.Text, (KozteruletJelleg)SzamlaKozteruletJellege_cbox.SelectedItem, SzamlaHazSzam_txb.Text,
                            SzamlaEmeletAjto_txb.Text, PostaOrszag_txb.Text, PostaIranyitoSzam_txb.Text, Postatelepules_txb.Text, PostaKozteruletNeve_txb.Text, (KozteruletJelleg)PostaKozteruletJellege_cbox.SelectedItem, PostaHazSzam_txb.Text, PostaEmeletAjto_txb.Text);
                        ABKezelo.PartnerModositas(partnerek);
                        cegPartner = partnerek;
                        MessageBox.Show("A cégtulajdonos adatainak módosítása sekeres volt.", "Figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("A cégtulajdonos adószáma már szerepel az adatbázisban!", "Figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.DialogResult = DialogResult.None;
                    }
                }
                catch (Exception ex)
                {
                    foreach (Control item in Controls)
                    {
                        if (item is TextBox && item.ForeColor == SystemColors.WindowText)//talán ez a jó
                        {
                            item.ForeColor = Color.Red;

                        }
                        MessageBox.Show(ex.Message, "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.DialogResult = DialogResult.None;
                    }


                }
            }
        }

        private void CegTulajTorles_btn_Click(object sender, EventArgs e)
        {
            try
            {

                if (MessageBox.Show("A cégtulajdonos törlésével az adatbázisban tárolt összes információ el fog veszni, és a program alaphelyzetbe áll. Biztosan folytatni kívánja  a műveletet?", "FIGYELEM!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {

                    ABKezelo.AdatbazisTorles();

                    cegPartner = null;
                    SzamlaAdoSzam_txb.Text = "";
                    SzamlaEUAdoSzam_txb.Text = "";
                    TelefonSzam_txb.Text = "";
                    Fax_txb.Text = "";
                    EmailCim_tbx.Text = "";
                    WebOldalCime_txb.Text = "";
                    BankSzamlaSzam_tbx.Text = "";
                    Titulus_txb.Text = "";
                    vezeteknev_tbx.Text = "";
                    Keresztnev_tbx.Text = "";
                    UtoNev_tbx.Text = "";
                    Cegnev_txb.Text = "";
                    SzamlaOrszag_txb.Text = "";
                    SzamlaIrsz_txb.Text = "";
                    SzamlaTelepules_txb.Text = "";
                    SzamlaKozeruletNeve_txb.Text = "";
                    SzamlaKozteruletJellege_cbox.SelectedItem = "";
                    SzamlaHazSzam_txb.Text = "";
                    SzamlaEmeletAjto_txb.Text = "";


                    PostaOrszag_txb.Text = "";
                    PostaIranyitoSzam_txb.Text = "";
                    Postatelepules_txb.Text = "";
                    PostaKozteruletNeve_txb.Text = "";
                    PostaKozteruletJellege_cbox.SelectedItem = "";
                    PostaHazSzam_txb.Text = "";
                    PostaEmeletAjto_txb.Text = "";
                    MessageBox.Show("A cégtulajdonos teljes adatállományának törlése sikeres volt, a program alaphelyzetbe állt.", "Figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CegTulajFelvitel_btn.Enabled = true;                  
                    ideiglenesTermekKategoria.Clear();
                    ideiglenesAFA.Clear();
                    ideiglenesPartnerKategoria.Clear();
                    ideiglenesFizetesiMod.Clear();
                    ideiglenesMertekEgyseg.Clear();

                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "A törlés sikertelen", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void Postacim_chbox_CheckedChanged(object sender, EventArgs e)
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
            }
        }
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
        }

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

        private void MennyisegEgysegHozzaAdas_btn_Click(object sender, EventArgs e)
        {
            try
            {

                tmp2.Value = mennyisegEgysege_tbx.Text;
                string error = StringCheck(ideiglenesMertekEgyseg, tmp2);
                if (error.Length == 0)
                {
                    tmp2.SQLID = ABKezelo.TermekMennyisegEgysegFelvitel(tmp2.Value);
                    ideiglenesMertekEgyseg.Add(tmp2);
                    listBox5.SelectedIndex = listBox5.Items.Count - 1;
                }
                else
                {
                    MessageBox.Show(error, "Figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void MennyisegEgysegModositas_btn_Click(object sender, EventArgs e)
        {
            try
            {

                tmp2.Value = mennyisegEgysege_tbx.Text;
                tmp2.SQLID = ideiglenesMertekEgyseg[listBox5.SelectedIndex].SQLID;
                string error = StringCheck(ideiglenesMertekEgyseg, tmp2);
                if (error.Length == 0)
                {
                    ABKezelo.TermekMennyisegEgysegModositas(tmp2.Value, tmp2.SQLID);
                    ideiglenesMertekEgyseg[listBox5.SelectedIndex] = tmp2;
                    listBox5.SelectedIndex = listBox5.Items.Count - 1;
                }
                else
                {
                    MessageBox.Show(error, "Figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Warning);


            }
        }

        private void mennyisegegysegTorles_btn_Click(object sender, EventArgs e)
        {
            if (listBox5.SelectedIndex > -1)
            {
                ABKezelo.TermekMennyisegEgysegTorles(ideiglenesMertekEgyseg[listBox5.SelectedIndex].SQLID);
                ideiglenesMertekEgyseg.RemoveAt(listBox5.SelectedIndex);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Helpfrm help = new Helpfrm(KivetelKezeles.getHelp("Törzsadatok menüpont"));
            help.ShowDialog();
        }

        private void segitseg_btn_Click(object sender, EventArgs e)
        {
            Helpfrm help = new Helpfrm(KivetelKezeles.getHelp("Törzsadatok menüpont"));
            help.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Helpfrm help = new Helpfrm(KivetelKezeles.getHelp("Törzsadatok menüpont"));
            help.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Helpfrm help = new Helpfrm(KivetelKezeles.getHelp("Törzsadatok menüpont"));
            help.ShowDialog();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Helpfrm help = new Helpfrm(KivetelKezeles.getHelp("Törzsadatok menüpont"));
            help.ShowDialog();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Helpfrm help = new Helpfrm(KivetelKezeles.getHelp("Törzsadatok menüpont"));
            help.ShowDialog();
        }

        private void SzamlaOrszag_txb_TabIndexChanged(object sender, EventArgs e)
        {
            if (SzamlaOrszag_txb.Text != PostaOrszag_txb.Text && Postacim_chbox.Checked)
            {
                Postacim_chbox.Checked = false;
            }
        }

        private void SzamlaIrsz_txb_TextChanged(object sender, EventArgs e)
        {
            if (SzamlaOrszag_txb.Text != PostaOrszag_txb.Text && Postacim_chbox.Checked)
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
            if (SzamlaKozeruletNeve_txb.Text != PostaKozteruletNeve_txb.Text && Postacim_chbox.Checked)
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

        private void vezeteknev_tbx_Enter_1(object sender, EventArgs e)
        {
            if (vezeteknev_tbx.Text == "Kötelező kitölteni!")
            {
                vezeteknev_tbx.Text = "";
                vezeteknev_tbx.ForeColor = Color.Black;
            }
        }

        private void vezeteknev_tbx_Leave_1(object sender, EventArgs e)
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
                Cegnev_txb.Text = Cegnev_txb.Text[0].ToString().ToUpper() + Cegnev_txb.Text.Substring(1);
            }
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
                SzamlaKozeruletNeve_txb.Text = SzamlaKozeruletNeve_txb.Text[0].ToString().ToUpper() + SzamlaKozeruletNeve_txb.Text.Substring(1);
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
            KivetelKezeles.TextBoxAdoSzamRegex(errorProvider1, SzamlaAdoSzam_txb);
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

        private void vezeteknev_tbx_TextChanged(object sender, EventArgs e)
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

        private void Titulus_txb_TextChanged(object sender, EventArgs e)
        {
            KivetelKezeles.TextBoxNemKotelezoStringe(errorProvider1, UtoNev_tbx);
        }

        private void UtoNev_tbx_Leave(object sender, EventArgs e)
        {
            if (UtoNev_tbx.TextLength > 0)
            {
                UtoNev_tbx.Text = UtoNev_tbx.Text[0].ToString().ToUpper() + UtoNev_tbx.Text.Substring(1);
            }
        }

        private void UtoNev_tbx_TextChanged(object sender, EventArgs e)
        {
            KivetelKezeles.TextBoxNemKotelezoStringe(errorProvider1, UtoNev_tbx);
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

        private void SzamlaEmeletAjto_txb_Leave(object sender, EventArgs e)
        {
            KivetelKezeles.TextBoxPostaEmeletAjtoRegex(errorProvider1, SzamlaEmeletAjto_txb);
        }

        private void SzamlaAdoSzam_txb_TextChanged(object sender, EventArgs e)
        {
            if (SzamlaAdoSzam_txb.Text != "Kötelező kitölteni!")
            {
                KivetelKezeles.TextBoxAdoSzamRegex(errorProvider1, SzamlaAdoSzam_txb);
            }
        }

        private void SzamlaEUAdoSzam_txb_Leave(object sender, EventArgs e)
        {
            if (SzamlaEUAdoSzam_txb.TextLength > 0)
            {
                SzamlaEUAdoSzam_txb.Text = SzamlaEUAdoSzam_txb.Text.ToString().ToUpper();
            }
        }

        private void SzamlaEUAdoSzam_txb_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void TelefonSzam_txb_TextChanged(object sender, EventArgs e)
        {
            KivetelKezeles.NemKotelezoTelefonSzamRegex(errorProvider1, TelefonSzam_txb);
        }

        private void Fax_txb_Leave(object sender, EventArgs e)
        {
            KivetelKezeles.FaxRegex(errorProvider1, Fax_txb);
        }

        private void Fax_txb_TextChanged(object sender, EventArgs e)
        {
            KivetelKezeles.FaxRegex(errorProvider1, Fax_txb);
        }

        private void EmailCim_tbx_TextChanged(object sender, EventArgs e)
        {
            KivetelKezeles.TextBoxEmail(errorProvider1, EmailCim_tbx);
        }

        private void EmailCim_tbx_Leave(object sender, EventArgs e)
        {
            KivetelKezeles.TextBoxEmail(errorProvider1, EmailCim_tbx);
        }

        private void WebOldalCime_txb_Leave(object sender, EventArgs e)
        {
            KivetelKezeles.TextBoxWebCim(errorProvider1, WebOldalCime_txb);
        }

        private void WebOldalCime_txb_TextChanged(object sender, EventArgs e)
        {
            KivetelKezeles.TextBoxWebCim(errorProvider1, WebOldalCime_txb);
        }

        private void BankSzamlaSzam_tbx_Leave(object sender, EventArgs e)
        {
            KivetelKezeles.NemKotelezoBankSzamlaSzame(errorProvider1, BankSzamlaSzam_tbx);
        }

        private void BankSzamlaSzam_tbx_TextChanged(object sender, EventArgs e)
        {
            KivetelKezeles.NemKotelezoBankSzamlaSzame(errorProvider1, BankSzamlaSzam_tbx);
        }

        private void PostaOrszag_txb_TextChanged(object sender, EventArgs e)
        {
            KivetelKezeles.TextBoxNemKotelezoStringe(errorProvider1, PostaOrszag_txb);
        }

        private void PostaOrszag_txb_Leave(object sender, EventArgs e)
        {
            KivetelKezeles.TextBoxNemKotelezoStringe(errorProvider1, PostaOrszag_txb);
        }

        private void PostaIranyitoSzam_txb_Leave(object sender, EventArgs e)
        {
            KivetelKezeles.PostaTextBoxIranyitoSzamRegex(errorProvider1, PostaIranyitoSzam_txb);
        }

        private void PostaIranyitoSzam_txb_TextChanged(object sender, EventArgs e)
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

        private void Postatelepules_txb_TextChanged(object sender, EventArgs e)
        {
            KivetelKezeles.TextBoxNemKotelezoStringe(errorProvider1, Postatelepules_txb);
        }

        private void PostaKozteruletNeve_txb_Leave(object sender, EventArgs e)
        {
            if (PostaKozteruletNeve_txb.TextLength > 0)
            {
                PostaKozteruletNeve_txb.Text = PostaKozteruletNeve_txb.Text[0].ToString().ToUpper() + PostaKozteruletNeve_txb.Text.Substring(1);
            }
            KivetelKezeles.TextBoxNemKotelezoStringe(errorProvider1, PostaKozteruletNeve_txb);
        }

        private void PostaKozteruletNeve_txb_TextChanged(object sender, EventArgs e)
        {
            KivetelKezeles.TextBoxNemKotelezoStringe(errorProvider1, PostaKozteruletNeve_txb);
        }

        private void PostaHazSzam_txb_Leave(object sender, EventArgs e)
        {
            KivetelKezeles.TextBoxPostaHazszamRegex(errorProvider1, PostaHazSzam_txb);
        }

        private void PostaHazSzam_txb_TextChanged(object sender, EventArgs e)
        {
            KivetelKezeles.TextBoxPostaHazszamRegex(errorProvider1, PostaHazSzam_txb);
        }

        private void PostaEmeletAjto_txb_Leave(object sender, EventArgs e)
        {
            KivetelKezeles.TextBoxPostaEmeletAjtoRegex(errorProvider1, PostaEmeletAjto_txb);
        }

        private void PostaEmeletAjto_txb_TextChanged(object sender, EventArgs e)
        {
            KivetelKezeles.TextBoxPostaEmeletAjtoRegex(errorProvider1, PostaEmeletAjto_txb);
        }

        private void Bezaras_btn2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Bezaras_btn3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            KivetelKezeles.StringLetiltoDecimal(textBox1,e);
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            textBox1.Text = "";
            listBox2.SelectedItem=null;
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.TextLength==0)
            {
                textBox1.Text = "0";
            }
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
            KivetelKezeles.StringLetiltoDecimal2(SzamlaAdoSzam_txb, e);
        }
    }
}