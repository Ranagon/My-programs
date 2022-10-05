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
    public partial class PartnerBongeszesFrm : Form
    {
        bool IsShown = false;
        internal static Partner szallitoPartner;

        internal static List<Partner> PartnerCsoportKereso(List<Partner> partnerLista, string keresett)
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
        internal static List<Partner> AdoSzamKereso(List<Partner> partnerLista, string keresett)//nem kell
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
        internal static List<Partner> CegNevKereso(List<Partner> partnerLista, string keresett)
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

        internal static void KeresoEventFuggveny(ComboBox Kategoria_cbx, ListView listView1, TextBox kereses_tbx)//Ha azt akarom,hgy az eventek ugyanazt a dolgot csinálják, 1 függvény kell az asszinronitás miatt.
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
    
        public PartnerBongeszesFrm()
        {
            InitializeComponent();
        }

        private void PartnerBongeszesFrm_Load(object sender, EventArgs e)
        {
            this.Text = "Partner kiválasztása";
            this.BackColor = SystemColors.GradientInactiveCaption;
            Partner_lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);//TERMÉKLV MÉRETEZÉS
            Kategoria_cbx.DataSource = Enum.GetValues(typeof(partnerKereses));
            ABKezelo.PartnerLVFeltoltes(Partner_lv);
            KivetelKezeles.Kijelol(false, PartnerKivalasztas_btn);

            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 1000;
            toolTip1.ReshowDelay = 500;
            toolTip1.ShowAlways = true;
            toolTip1.SetToolTip(this.PartnerKivalasztas_btn, "Kérjük jelöljön\r\nki egy partnert a\r\npartnerlistából!");
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Partner_lv.SelectedItems.Count > 0)
            {
                KivetelKezeles.Kijelol(true, PartnerKivalasztas_btn);
            }
            else
            {
                KivetelKezeles.Kijelol(false, PartnerKivalasztas_btn);
            }
        }

        private void PartnerKivalasztas_btn_Click(object sender, EventArgs e)
        {
                     
            if (Partner_lv.SelectedItems.Count > 0)
            {               
                try
                {
                    ListViewItem item = Partner_lv.Items[Partner_lv.SelectedIndices[0]];
                    szallitoPartner = ABKezelo.PartnerKivalasztasAdoszamAlapjan(item.SubItems[5].Text);
                }
                catch (Exception ex)
                {
                      MessageBox.Show(ex.Message, "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                      this.DialogResult = DialogResult.None;
                }
            }
        }
     
        private void kereses_tbx_TextChanged(object sender, EventArgs e)
        {
            List<Partner> partnerek = ABKezelo.PartnerKereso(kereses_tbx.Text, (partnerKereses)Kategoria_cbx.SelectedIndex);
            if(partnerek.Count >0)Partner_lv.Items.Clear();
            for (int i = 0; i < partnerek.Count; i++)
            {

                ABKezelo.PartnerLVFeltoltesManualisan(Partner_lv, partnerek[i]);
                //Partner_lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
          
            if (partnerek.Count == 0 && Partner_lv.Items[0].Text != "Nincs találat!")
            {
                //Partner_lv.Columns.Clear();
                ABKezelo.UresLVFeltoltesManualisan(Partner_lv);
                //Partner_lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
        }

        private void Kategoria_cbx_SelectedIndexChanged(object sender, EventArgs e)
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

        private void Partner_lv_MouseDoubleClick(object sender, MouseEventArgs e)
        {
          
                if (Partner_lv.SelectedItems.Count > 0)
                {
                    ListViewItem item = Partner_lv.Items[Partner_lv.SelectedIndices[0]];
                    UjPartnerFrm frm = new UjPartnerFrm(ABKezelo.PartnerKivalasztasAdoszamAlapjan(item.SubItems[5].Text), "megtekint");
                    frm.ShowDialog();
                }          
        }

        private void kereses_tbx_Enter(object sender, EventArgs e)
        {
            if (Partner_lv.SelectedItems.Count > 0)
            {

                Partner_lv.SelectedItems.Clear();
            }          
        }

        private void Kategoria_cbx_Enter(object sender, EventArgs e)
        {
            if (Partner_lv.SelectedItems.Count > 0)
            {
                Partner_lv.SelectedItems.Clear();
            }
        }

        private void PartnerBongeszesFrm_MouseMove(object sender, MouseEventArgs e)
        {
            Control ctrl = this.GetChildAtPoint(e.Location);

            if (ctrl != null)
            {
                if (ctrl == this.PartnerKivalasztas_btn && !IsShown)
                {
                    string tipstring = this.toolTip1.GetToolTip(this.PartnerKivalasztas_btn);
                    this.toolTip1.Show(tipstring, this.PartnerKivalasztas_btn, this.PartnerKivalasztas_btn.Width / 2, this.PartnerKivalasztas_btn.Height / 2);
                    IsShown = true;
                }
            }
            else
            {
                this.toolTip1.Hide(this.PartnerKivalasztas_btn);
                IsShown = false;
            }
        }
    }
}

        
