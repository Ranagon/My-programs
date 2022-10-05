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
    public partial class SzallitoTermekListaFrm : Form
    {
        internal List<SzallitoLevel> szallitoLevelek = new List<SzallitoLevel>();
        internal BindingList<KiadottTermek> kiadottTermekek = new BindingList<KiadottTermek>();
        internal List<Termek> Modositotttermek = new List<Termek>(); 
        internal List<Termek> valasztottTermekek = new List<Termek>();
        internal List<Termek> TermekLista = new List<Termek>();
        internal Partner globalPartner;

        internal SzallitoTermekListaFrm(Partner partner, SzallitoLevel szallitoLevel, BindingList<KiadottTermek> lista, string adat)
        {

            InitializeComponent();

            if (partner != null && lista.Count > 0 && adat == "szallito")
            {
                PartnerBongeszesFrm.szallitoPartner = partner;
                globalPartner = partner;
                Partner cegPartner = ABKezelo.PartnerKivalasztasKategoriaAlapjan("1");

                EladoNev_tbx.Text = cegPartner.VezetekNev + " " + cegPartner.KeresztNev;
                EladoCim_tbx.Text = cegPartner.Irsz + " " + cegPartner.Kozterulet + " " + cegPartner.KozteruletJelleg + " " + cegPartner.HazSzam + " " + cegPartner.EmeletAjto;
                EladoAdoSzam_tbx.Text = cegPartner.AdoSzam.ToString();
                EladoNev_tbx.ReadOnly = true;
                EladoCim_tbx.ReadOnly = true;
                EladoAdoSzam_tbx.ReadOnly = true;
                VevoNev_tbx.ReadOnly = true;
                VevoCim_tbx.ReadOnly = true;
                VevoAdoszam_tbx.ReadOnly = true;
                Osszeg_tbx.ReadOnly = true;
                VevoNev_tbx.Text = partner.VezetekNev + " " + partner.KeresztNev;
                VevoCim_tbx.Text = partner.Irsz + " " + partner.Kozterulet + " " + partner.KozteruletJelleg + " " + partner.HazSzam + " " + partner.EmeletAjto;
                VevoAdoszam_tbx.Text = partner.AdoSzam.ToString();
                MegrendelesSzam_tbx.Text = szallitoLevel.MegrendelesSzam.ToString();
                Kedvezmeny_tbx.Text = partner.Kedvezmeny.ToString();
                Kedvezmeny_tbx.Enabled = false;
                VevoNev_tbx.ReadOnly = true;
                VevoCim_tbx.ReadOnly = true;
                VevoAdoszam_tbx.ReadOnly = true;
                MegrendelesSzam_tbx.ReadOnly = true;
                VevoNev_tbx.ReadOnly = true;
                VevoCim_tbx.ReadOnly = true;
                VevoAdoszam_tbx.ReadOnly = true;
                VevoNev_tbx.Enter += (s, zs) => { Bovebben_btn.Focus(); };
                VevoCim_tbx.Enter += (s, zs) => { Bovebben_btn.Focus(); };
                VevoAdoszam_tbx.Enter += (s, zs) => { Bovebben_btn.Focus(); };
                EladoNev_tbx.Enter += (s, zs) => { Bovebben_btn.Focus(); };
                EladoCim_tbx.Enter += (s, zs) => { Bovebben_btn.Focus(); };
                EladoAdoSzam_tbx.Enter += (s, zs) => { Bovebben_btn.Focus(); };
                MegrendelesSzam_tbx.Enter += (s, zs) => { Bezaras_btn.Focus(); };
                Kiadotttermek_lv.Enter += (s, zs) => { Bezaras_btn.Focus(); };
                Bizonylatkelte_dtp.Enabled = false;
                Bizonylatkelte_dtp.Enter += (s, zs) => { Bezaras_btn.Focus(); };
                Kedvezmeny_tbx.Enter += (s, zs) => { Bezaras_btn.Focus(); };
                Osszeg_tbx.Enter += (s, zs) => { Bezaras_btn.Focus(); };
                try
                {
                    for (int i = 0; i < lista.Count; i++)
                    {
                        Termek tmp = ABKezelo.TermekKivalasztas(lista[i].TermekId);
                        if (tmp != null) {
                            valasztottTermekek.Add(tmp);
                            valasztottTermekek[i].Mennyiseg = lista[i].DarabSzam;
                            valasztottTermekek[i].BruttoAr = lista[i].EladasiAr;
                            valasztottTermekek[i].NettoAr = Math.Round((100-tmp.AfaKulcs) * (decimal)0.01  *lista[i].EladasiAr);
                        }
                    }
                }
                catch (Exception)
                {

                   
                }
              
                
                Osszeg_tbx.Text = (KiadottTermek.ArSzamito(valasztottTermekek, partner.Kedvezmeny)).ToString();

                for (int i = 0; i < valasztottTermekek.Count; i++)
                {
                    ABKezelo.TermekLVFeltoltesManualisan(Kiadotttermek_lv, valasztottTermekek[i]);
                    Kiadotttermek_lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                }
            }
        }

        private void SzallitoTermekListaFrm_Load(object sender, EventArgs e)
        {
            this.BackColor = SystemColors.GradientInactiveCaption;
            this.Text = "Szállítólevél adatai";
        }

        private void Bezaras_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Bovebben_btn_Click_1(object sender, EventArgs e)
        {
            UjPartnerFrm frm = new UjPartnerFrm(globalPartner, "megtekint");
            frm.ShowDialog();
        }
    }
}
