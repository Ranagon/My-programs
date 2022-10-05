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
    public partial class Form1 : Form
    {

        public static Form1 Form1ref;
       
        public Form1()
        {
            InitializeComponent();

            Form1ref = this;        
        }

        internal bool szallitoBevetGomb(bool igaze)//GOMB SZÍNEZŐ ALPROGRAM A szállítólevél, és bevételezés gombjait színezi, attól függően, hogy a partner, termék, vagy a felhasználó adatok üresek-e az adatbázisban.
        {
            if (!igaze)
            {
                szállítólevélToolStripMenuItem.ForeColor = Color.Gray;
               
                bevételezésToolStripMenuItem.ForeColor = Color.Gray;               
            }
            else
            {
                szállítólevélToolStripMenuItem.ForeColor = SystemColors.WindowText;
               
                bevételezésToolStripMenuItem.ForeColor = SystemColors.WindowText;             
            }
            return igaze;
        }
      
        private void partnerekToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PartnerNyilvantartasFrm newMDIChild = new PartnerNyilvantartasFrm();
            newMDIChild.MdiParent = this;
            newMDIChild.Dock = DockStyle.Fill;
            newMDIChild.FormBorderStyle = FormBorderStyle.None;
            newMDIChild.Show();
            törzsadatokToolStripMenuItem.ForeColor = SystemColors.WindowText;//GOMB NAVIGÁLSHOZ
            kimutatásokToolStripMenuItem.ForeColor = SystemColors.WindowText;
            kilépésToolStripMenuItem.ForeColor = SystemColors.WindowText;
           // ImportToolStripMenuItem.ForeColor = SystemColors.WindowText;
            KezdoLapToolStripmenuItem.ForeColor = SystemColors.WindowText;
            partnerekToolStripMenuItem.ForeColor = Color.DarkSlateBlue;
            termékekToolStripMenuItem.ForeColor = SystemColors.WindowText;
           
            if (szállítólevélToolStripMenuItem.ForeColor != Color.Gray)
            {
                szállítólevélToolStripMenuItem.ForeColor = SystemColors.WindowText; ;
            }
            if (bevételezésToolStripMenuItem.ForeColor != Color.Gray)
            {
                bevételezésToolStripMenuItem.ForeColor = SystemColors.WindowText; 
            }
        }

        private void termékekToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TermekekFrm newMDIChild = new TermekekFrm();
            newMDIChild.MdiParent = this;
            newMDIChild.Dock = DockStyle.Fill;
            newMDIChild.FormBorderStyle = FormBorderStyle.None;
            newMDIChild.Show();
            törzsadatokToolStripMenuItem.ForeColor = SystemColors.WindowText;
            kimutatásokToolStripMenuItem.ForeColor = SystemColors.WindowText;
            kilépésToolStripMenuItem.ForeColor = SystemColors.WindowText;
           // ImportToolStripMenuItem.ForeColor = SystemColors.WindowText;
            KezdoLapToolStripmenuItem.ForeColor = SystemColors.WindowText;
            partnerekToolStripMenuItem.ForeColor = SystemColors.WindowText;
            termékekToolStripMenuItem.ForeColor = Color.DarkSlateBlue;

            if (szállítólevélToolStripMenuItem.ForeColor != Color.Gray)
            {
                szállítólevélToolStripMenuItem.ForeColor = SystemColors.WindowText; ;
            }
            if (bevételezésToolStripMenuItem.ForeColor != Color.Gray)
            {
                bevételezésToolStripMenuItem.ForeColor = SystemColors.WindowText;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            this.BackColor = SystemColors.GradientInactiveCaption;
            try//ITT LEKÉRI A PROGRAM A FELHASZNÁLÓ, A PARTNER, ÉS TERMÉKLISTA ADATAIT. HA NEM ÜRESEK, A SZÁLLÍTÓLEVÉL ÉS A BEVÉTELEZÉS GOMBOT MEG LEHET IDÉZNI,        
            {
                if (ABKezelo.PartnerKivalasztasKategoriaAlapjan("1") != null && ABKezelo.PartnerLista().Count > 0 && ABKezelo.TermekListaFeltoltes().Count > 0 || ABKezelo.vanSzallitolevel())
                {
                    szallitoBevetGomb(true);
                }
                else
                {
                    szallitoBevetGomb(false);
                }
                KezdoLapFrm newMDIChild = new KezdoLapFrm();
                newMDIChild.MdiParent = this;
                newMDIChild.Dock = DockStyle.Fill;
                newMDIChild.FormBorderStyle = FormBorderStyle.None;
                newMDIChild.Show();
                if (ActiveMdiChild.Name == "newMDIChild")
                {
                    Helpfrm help = new Helpfrm(KivetelKezeles.getHelp("Főképernyő használata Dorka"));
                    help.ShowDialog();
                }
            }
            catch
            {
                MessageBox.Show("Adatbázis kommunikációs hiba!","Figyelem!",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
           /* catch (Exception)
            {
                if (MessageBox.Show("Adatbázis nem található. Kíván importálni egyet?","Kérdés",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes) //SAJNOS AZ ADATBÁZIS IMPORT ROSSZ.:(
                {
                    using (OpenFileDialog dialog = new OpenFileDialog())
                    {
                        dialog.Filter = "MssqlDatabase|*.mdf";
                        if (dialog.ShowDialog() == DialogResult.OK)
                        {
                            ABKezelo.detachDB();
                            File.Copy(dialog.FileName, $@"{Environment.CurrentDirectory}\Database1.mdf", true);
                            File.Copy($@"{Path.GetDirectoryName(dialog.FileName)}\Database1_log.ldf", $@"{Environment.CurrentDirectory}\Database1_log.ldf", true);
                            Application.Restart();
                            Environment.Exit(0);
                        }
                        else
                        {
                            ABKezelo.detachDB();
                            File.Copy($@"{Environment.CurrentDirectory}\EmptyDB\Database1.mdf", $@"{Environment.CurrentDirectory}\Database1.mdf", true);
                            File.Copy($@"{Environment.CurrentDirectory}\EmptyDB\Database1_log.ldf", $@"{Environment.CurrentDirectory}\Database1_log.ldf", true);
                            Application.Restart();
                            Environment.Exit(0);
                        }
                    }
                }
                else
                {
                    File.Copy($@"{Environment.CurrentDirectory}\EmptyDB\Database1.mdf", $@"{Environment.CurrentDirectory}\Database1.mdf", true);
                    Application.Restart();
                    Environment.Exit(0);
                }
                
            }*/

           


        }

        private void szállítólevélToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (szállítólevélToolStripMenuItem.ForeColor == SystemColors.WindowText)//Itt kezeljük le, hogy , ha nem szürke a gomb, lehet rá kattintani.
            {
                SzallitolevelFrm newMDIChild = new SzallitolevelFrm();
                newMDIChild.MdiParent = this;
                newMDIChild.Dock = DockStyle.Fill;
                newMDIChild.FormBorderStyle = FormBorderStyle.None;
                newMDIChild.Show();
                
                szállítólevélToolStripMenuItem.ForeColor = Color.DarkSlateBlue;
                törzsadatokToolStripMenuItem.ForeColor = SystemColors.WindowText;
                kimutatásokToolStripMenuItem.ForeColor = SystemColors.WindowText;
                kilépésToolStripMenuItem.ForeColor= SystemColors.WindowText;
              //  ImportToolStripMenuItem.ForeColor = SystemColors.WindowText;
                KezdoLapToolStripmenuItem.ForeColor= SystemColors.WindowText;
                partnerekToolStripMenuItem.ForeColor= SystemColors.WindowText;
                termékekToolStripMenuItem.ForeColor = SystemColors.WindowText;
                if (bevételezésToolStripMenuItem.ForeColor!=Color.Gray)
                {
                    bevételezésToolStripMenuItem.ForeColor = SystemColors.WindowText; ;
                }

            }

        }

        private void bevételezésToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bevételezésToolStripMenuItem.ForeColor == SystemColors.WindowText)
            {
                BevetelezesFrm newMDIChild = new BevetelezesFrm();
                newMDIChild.MdiParent = this;
                newMDIChild.Dock = DockStyle.Fill;
                newMDIChild.FormBorderStyle = FormBorderStyle.None;
                newMDIChild.Show();
              
                törzsadatokToolStripMenuItem.ForeColor = SystemColors.WindowText;
                kimutatásokToolStripMenuItem.ForeColor = SystemColors.WindowText;
                kilépésToolStripMenuItem.ForeColor = SystemColors.WindowText;
              //  ImportToolStripMenuItem.ForeColor = SystemColors.WindowText;
                KezdoLapToolStripmenuItem.ForeColor = SystemColors.WindowText;
                partnerekToolStripMenuItem.ForeColor = SystemColors.WindowText;
                termékekToolStripMenuItem.ForeColor = SystemColors.WindowText;
                bevételezésToolStripMenuItem.ForeColor = Color.DarkSlateBlue;
                if (szállítólevélToolStripMenuItem.ForeColor != Color.Gray)
                {
                    szállítólevélToolStripMenuItem.ForeColor = SystemColors.WindowText;
                }
            }

        }

        private void törzsadatokToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KategoriaFrm newMDIChild = new KategoriaFrm();
            newMDIChild.MdiParent = this;
            newMDIChild.Dock = DockStyle.Fill;
            newMDIChild.FormBorderStyle = FormBorderStyle.None;
            newMDIChild.Show();
            törzsadatokToolStripMenuItem.ForeColor = Color.DarkSlateBlue;
            kimutatásokToolStripMenuItem.ForeColor = SystemColors.WindowText;
            kilépésToolStripMenuItem.ForeColor = SystemColors.WindowText;
          //  ImportToolStripMenuItem.ForeColor = SystemColors.WindowText;
            KezdoLapToolStripmenuItem.ForeColor = SystemColors.WindowText;
            partnerekToolStripMenuItem.ForeColor = SystemColors.WindowText;
            termékekToolStripMenuItem.ForeColor = SystemColors.WindowText;

            if (szállítólevélToolStripMenuItem.ForeColor != Color.Gray)
            {
                szállítólevélToolStripMenuItem.ForeColor = SystemColors.WindowText;
            }
            if (bevételezésToolStripMenuItem.ForeColor != Color.Gray)
            {
                bevételezésToolStripMenuItem.ForeColor = SystemColors.WindowText;
            }
        }

        private void kimutatásokToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KimutatasokFrm newMDIChild = new KimutatasokFrm();
            newMDIChild.MdiParent = this;
            newMDIChild.Dock = DockStyle.Fill;
            newMDIChild.FormBorderStyle = FormBorderStyle.None;
            newMDIChild.Show();
            törzsadatokToolStripMenuItem.ForeColor = SystemColors.WindowText;
            kimutatásokToolStripMenuItem.ForeColor = Color.DarkSlateBlue;
            kilépésToolStripMenuItem.ForeColor = SystemColors.WindowText;
           // ImportToolStripMenuItem.ForeColor = SystemColors.WindowText;
            KezdoLapToolStripmenuItem.ForeColor = SystemColors.WindowText;
            partnerekToolStripMenuItem.ForeColor = SystemColors.WindowText;
            termékekToolStripMenuItem.ForeColor = SystemColors.WindowText;

            if (szállítólevélToolStripMenuItem.ForeColor != Color.Gray)
            {
                szállítólevélToolStripMenuItem.ForeColor = SystemColors.WindowText;
            }
            if (bevételezésToolStripMenuItem.ForeColor != Color.Gray)
            {
                bevételezésToolStripMenuItem.ForeColor = SystemColors.WindowText;
            }
        }

        private void szállítólevélToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            if (szállítólevélToolStripMenuItem.ForeColor == Color.Gray)
            {
                toolTip1.Active = false;
            }
        }

        private void szállítólevélToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            if (szállítólevélToolStripMenuItem.ForeColor == Color.Gray)
            {


                toolTip1.Active = true;
                Point x = new Point(300, -20);
                toolTip1.SetToolTip(menuStrip1, "uff");
                toolTip1.Show("A törzsadatok menüpont kitöltése, a termék-és partnerlista\nfeltöltése szükséges a menüpont használatához!", Form1.Form1ref, x);
            }
        }

        private void bevételezésToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            if (bevételezésToolStripMenuItem.ForeColor == Color.Gray)
            {
                toolTip1.Active = true;
                Point x = new Point(380,-20);
                toolTip1.SetToolTip(menuStrip1, "uff");
                toolTip1.Show("A törzsadatok menüpont kitöltése, a termék-és partnerlista\nfeltöltése szükséges a menüpont használatához!", Form1.Form1ref, x);
            }
        }

        private void bevételezésToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            if (bevételezésToolStripMenuItem.ForeColor == Color.Gray)
            {
                toolTip1.Active = false;
            }
        }

        private void kilépésToolStripMenuItem_Click(object sender, EventArgs e)
        {
            törzsadatokToolStripMenuItem.ForeColor = SystemColors.WindowText;
            kimutatásokToolStripMenuItem.ForeColor = SystemColors.WindowText;
            kilépésToolStripMenuItem.ForeColor = Color.DarkSlateBlue;
          //  ImportToolStripMenuItem.ForeColor = SystemColors.WindowText;
            KezdoLapToolStripmenuItem.ForeColor = SystemColors.WindowText;
            partnerekToolStripMenuItem.ForeColor = SystemColors.WindowText;
            termékekToolStripMenuItem.ForeColor = SystemColors.WindowText;

            if (szállítólevélToolStripMenuItem.ForeColor != Color.Gray)
            {
                szállítólevélToolStripMenuItem.ForeColor = SystemColors.WindowText;
            }
            if (bevételezésToolStripMenuItem.ForeColor != Color.Gray)
            {
                bevételezésToolStripMenuItem.ForeColor = SystemColors.WindowText;
            }
           /* if (MessageBox.Show("Kíván biztonsági mentést végrehajtani?", "Figyelem", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {



                using (SaveFileDialog dialog = new SaveFileDialog())
                {
                    dialog.Filter = "MssqlDatabase|*.mdf";
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        ABKezelo.detachDB();
                        File.Copy($@"{Environment.CurrentDirectory}\Database1.mdf", dialog.FileName, true);//Az Environment.CurrentDirectory-ba visszamásolni az openfiledialog 
                        File.Copy($@"{Environment.CurrentDirectory}\Database1_log.ldf", $@"{Path.GetDirectoryName(dialog.FileName)}\Database1_log.ldf", true);
                    }

                }
            }*/
          
            Application.Exit();
        }

      /*  private void ImportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            törzsadatokToolStripMenuItem.ForeColor = SystemColors.WindowText;
            kimutatásokToolStripMenuItem.ForeColor = SystemColors.WindowText;
            kilépésToolStripMenuItem.ForeColor = SystemColors.WindowText;
            ImportToolStripMenuItem.ForeColor = Color.DarkSlateBlue;
            KezdoLapToolStripmenuItem.ForeColor = SystemColors.WindowText;
            partnerekToolStripMenuItem.ForeColor = SystemColors.WindowText;
            termékekToolStripMenuItem.ForeColor = SystemColors.WindowText;

            if (szállítólevélToolStripMenuItem.ForeColor != Color.Gray)
            {
                szállítólevélToolStripMenuItem.ForeColor = SystemColors.WindowText;
            }
            if (bevételezésToolStripMenuItem.ForeColor != Color.Gray)
            {
                bevételezésToolStripMenuItem.ForeColor = SystemColors.WindowText;
            }
            if (MessageBox.Show("Ön arra készül, hogy külső adatbázist importáljon. Ezzel felülírja a jelenlegi adatbázist. Biztosan folytatni kívánja?", "Figyelem", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                using (OpenFileDialog dialog = new OpenFileDialog())
                {
                    dialog.Filter = "MssqlDatabase|*.mdf";
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        ABKezelo.detachDB();
                        File.Copy(dialog.FileName, $@"{Environment.CurrentDirectory}\Database1.mdf", true);
                        File.Copy($@"{Path.GetDirectoryName(dialog.FileName)}\Database1_log.ldf", $@"{Environment.CurrentDirectory}\Database1_log.ldf", true);
                        Application.Restart();
                        Environment.Exit(0);
                    }
                }
                ImportToolStripMenuItem.ForeColor = SystemColors.WindowText;
            }
            else
            {
                ImportToolStripMenuItem.ForeColor = SystemColors.WindowText;
            }
        }*/

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Biztosan ki akar lépni a programból?","Kérdés",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.No)
            {
                e.Cancel = true;
                kilépésToolStripMenuItem.ForeColor = SystemColors.WindowText;
            }          
        }

        private void KezdoLapToolStripmenuItem_Click(object sender, EventArgs e)
        {
            KezdoLapFrm newMDIChild1 = new KezdoLapFrm();
            newMDIChild1.MdiParent = this;
            newMDIChild1.Dock = DockStyle.Fill;
            newMDIChild1.FormBorderStyle = FormBorderStyle.None;
            newMDIChild1.Show();
            törzsadatokToolStripMenuItem.ForeColor = SystemColors.WindowText;
            kimutatásokToolStripMenuItem.ForeColor = SystemColors.WindowText;
            kilépésToolStripMenuItem.ForeColor = SystemColors.WindowText;
          //  ImportToolStripMenuItem.ForeColor = SystemColors.WindowText;
            KezdoLapToolStripmenuItem.ForeColor = Color.DarkSlateBlue;
            partnerekToolStripMenuItem.ForeColor = SystemColors.WindowText;
            termékekToolStripMenuItem.ForeColor = SystemColors.WindowText;

            if (szállítólevélToolStripMenuItem.ForeColor != Color.Gray)
            {
                szállítólevélToolStripMenuItem.ForeColor = SystemColors.WindowText;
            }
            if (bevételezésToolStripMenuItem.ForeColor != Color.Gray)
            {
                bevételezésToolStripMenuItem.ForeColor = SystemColors.WindowText;
            }
           
        }

        private void KezdoLapToolStripmenuItem_MouseLeave(object sender, EventArgs e)
        {
           // KezdoLapToolStripmenuItem.ForeColor = Color.Gray;
        }

        private void névjegyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NevJegyFrm help = new NevJegyFrm(KivetelKezeles.getHelp("Adatok")); //NÉVJEGY BETÖLTÉS BIN FILEBÓL
            help.Text = "Program készítőjének adatai";
            help.Width = 350;
            help.Height = 250;
            help.ShowDialog();
        }
    }
}
