using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Szakdolgozat3
{
    internal class KivetelKezeles
    {
        public static string startIndex = "";
        //[a-zA-Z]{2}[0-9]{2}[a-zA-Z0-9]{4}[0-9]{7}([a-zA-Z0-9]?){0,16} bankszamla


        internal static bool PostaEllenorzo(TextBox text1, TextBox text2, TextBox text3, TextBox text4)
        {
            if (text1.TextLength == 0 || text2.TextLength == 0 || text3.TextLength == 0 || text4.TextLength == 0)
            {
                text1.Text = "";
                text2.Text = "";
                text3.Text = "";
                text4.Text = "";
             
                return true;

            }
            return false;
        }

        private static bool vaneBenneSzam(string bemenet) {
            int numCounter = 0;
            for (int i = 0; i < bemenet.Length; i++) if (char.IsDigit(bemenet[i])) numCounter++;
            return numCounter==bemenet.Length;
        }
        public static string NullStringe(object bemenet)
        {
            int valtozo;

            if (bemenet != null && int.TryParse(bemenet.ToString(), out valtozo))
            {
                return "A beviteli mező nem tartalmazhat számot!";
            }          
            else
            {
                return "";
            }
        }
        public static bool CegKivetel(string bemenet)
        {
            if (string.IsNullOrEmpty(bemenet))
            {
                return false;
            }
            else if (string.IsNullOrWhiteSpace(bemenet))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static bool NullStringeKivetel(string bemenet)
        {
            int valtozo;

            if (bemenet != null && int.TryParse(bemenet.ToString(), out valtozo))
            {
                return false;
            }
            else if (bemenet.Length == 0)
            {
                return true;
            }
            else
            {
                return true;
            }
        }
        public static string NullSzame(object bemenet)
        {
            int valtozo;

            if (bemenet != null && !int.TryParse(bemenet.ToString(), out valtozo))
            {
                return "A beviteli mező értéke csak szám lehet!";
            }          
            else
            {
                return "";
            }
        }
        public static string Stringe(object bemenet)
        {
            int valtozo;

            if (bemenet != null && int.TryParse(bemenet.ToString(), out valtozo))
            {
                return "A beviteli mező nem tartalmazhat számot!";
            }
            else if (string.IsNullOrEmpty(bemenet.ToString()))
            {
                return "A beviteli mező nem lehet üres";
            }
            else
            {
                return "";
            }
        }
        public static bool StringeKivetel(string bemenet)
        {
            if (bemenet != null && vaneBenneSzam(bemenet))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(bemenet.ToString()))
            {
                return false;
            }
            else if (bemenet== "Kötelező kitölteni!")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static string Szame(object bemenet)
        {
            if (!vaneBenneSzam(bemenet.ToString()))
            {
                return "A beviteli mező értéke csak szám lehet!";
            }
            else if (string.IsNullOrEmpty(bemenet.ToString()))
            {
                return "A beviteli mező nem lehet üres";
            }
            else
            {
                return "";
            }
        }
        /*
        public static bool AdoSzame(ErrorProvider provider, TextBox adat)
        {
            if (!vaneBenneSzam(adat.Text))
            {
                provider.SetError(adat, "A beviteli mező értéke csak szám lehet!");
                adat.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(adat.Text))
            {
                provider.SetError(adat, "A beviteli mező nem lehet üres!");
                adat.Focus();
                return false;
            }
            else if (adat.Text.Length != 8)
            {
                provider.SetError(adat, "Az adószámnak 8 számjegyűnek kell lennie!");
                adat.Focus();
                return false;
            }
            else
            {
                provider.SetError(adat, "");
                provider.Clear();
                return true;
            }

        }
        */
        public static bool NemKotelezoAdoSzame(ErrorProvider provider, TextBox adat)
        {
            if (!vaneBenneSzam(adat.Text) && !string.IsNullOrEmpty(adat.Text) && adat.Text != "Kötelező kitölteni!")
            {
                provider.SetError(adat, "A beviteli mező értéke csak szám lehet!");
                adat.Focus();
                return false;
            }
            else if(!string.IsNullOrEmpty(adat.Text) && adat.Text.Length != 8 && adat.Text != "Kötelező kitölteni!")
            {
                provider.SetError(adat, "Az adószámnak 8 számjegyűnek kell lennie!");
                adat.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(adat.Text))
            {
              
                return true;
            }
            else if (string.IsNullOrEmpty(adat.Text))
            {
             
                return true;
               
            }
            else
            {
                provider.SetError(adat, "");
                provider.Clear();
                return true;
            }

        }
        public static bool NemKotelezoBankSzamlaSzame(ErrorProvider provider, TextBox adat)
        {
            int valtozo;
            if (!vaneBenneSzam(adat.Text) && !string.IsNullOrEmpty(adat.Text) && !string.IsNullOrWhiteSpace(adat.Text))
            {
                provider.SetError(adat, "A beviteli mező értéke csak szám lehet!");
                adat.Focus();
                return false;
            }
            else if (adat.Text.Length != 16 && !string.IsNullOrEmpty(adat.Text) && !string.IsNullOrWhiteSpace(adat.Text))
            {
                provider.SetError(adat, "A bankszámlaszámnak 16 számjegyűnek kell lennie!");
                adat.Focus();
                return false;
            }
                
            else
            {
                provider.SetError(adat, "");
                provider.Clear();
                return true;
            }

        }
        public static bool AdoSzamKivetel(string bemenet)
        {
            Regex pattern1 = new Regex(@"^[0-9]{11}$");


            if (pattern1.IsMatch(bemenet) && !string.IsNullOrEmpty(bemenet) && !string.IsNullOrWhiteSpace(bemenet) && bemenet != "Kötelező kitölteni!")
            {
               
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool TextBoxAdoSzamRegex(ErrorProvider provider, TextBox adat)
        {
            Regex pattern1 = new Regex(@"^[0-9]{11}$");
            if (!pattern1.IsMatch(adat.Text) && adat.Text != "Kötelező kitölteni!" && !string.IsNullOrEmpty(adat.Text) && !string.IsNullOrWhiteSpace(adat.Text))
            {
                provider.SetError(adat, "Az adószámnak 11 számjegyűnek kell lennie!");
                adat.Focus();
                return false;
            }
            else
            {
                provider.SetError(adat, "");
                provider.Clear();
                return true;
            }

        }
        public static bool AdoszamEllenorzo(TextBox textBox,Label label)
        {
            if (label.Text== "Adószám:" && textBox.TextLength==11)
            {
                return true;
            }
            else if (label.Text== "Adóazonosító jel:" && textBox.TextLength==8)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool TextBoxAdoJelRegex(ErrorProvider provider, TextBox adat)
        {
            Regex pattern1 = new Regex(@"^[0-9]{8}$");
            if (!pattern1.IsMatch(adat.Text) && adat.Text != "Kötelező kitölteni!" && !string.IsNullOrEmpty(adat.Text) && !string.IsNullOrWhiteSpace(adat.Text))
            {
                provider.SetError(adat, "Az adóazonosító jelnek 8 számjegyűnek kell lennie!");
                adat.Focus();
                return false;
            }
            else
            {
                provider.SetError(adat, "");
                provider.Clear();
                return true;
            }

        }

        public static string EmailFormatum()
        {
            return null;
        }
        public static bool TextBoxStringe(ErrorProvider provider, TextBox adat)
        {
            if (string.IsNullOrWhiteSpace(adat.Text))
            {
                provider.SetError(adat, "A beviteli mező nem lehet üres és nem tartalmazhat szóközöket!");
                adat.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(adat.Text))
            {              
                provider.SetError(adat, "A beviteli mező nem lehet üres!");
                adat.Focus();
                return false;
            }
            else if (vaneBenneSzam(adat.Text))
            {               
                provider.SetError(adat, "A beviteli mező nem tartalmazhat számot!");
                adat.Focus();
                return false;
            }
            else if (adat.Text== "Kötelező kitölteni!") 
            {
                provider.SetError(adat, "A beviteli mező nem lehet üres!");
                adat.Focus();
                return false;
            }
            else
            {
                provider.SetError(adat, "");
               // provider.Clear();
                return true;
              
                
            }
           
          
        }
    
        public static bool TextBoxNemKotelezoStringe(ErrorProvider provider, TextBox adat)
        {
            Regex pattern1 = new Regex("^[A-Za-záéiíoóöőuúüűÁÉIÍOÓÖŐUÚÜŰä]*$");
            Regex pattern2 = new Regex("^[0-9]*$");

            

            if (adat.Text.Length!=0)
            {
                startIndex = adat.Text.Substring(0, 1);
            }
            
               
            
          

            if (pattern1.IsMatch(adat.Text) && !string.IsNullOrEmpty(adat.Text) && !string.IsNullOrWhiteSpace(adat.Text) && adat.Text != "Kötelező kitölteni!")
            {
                provider.SetError(adat, "");
                return true;
            }
        
            else if (string.IsNullOrEmpty(adat.Text) && string.IsNullOrWhiteSpace(adat.Text))
            {
                provider.SetError(adat, "");
                return true;
            }
            else if (adat.Text == "Kötelező kitölteni!")
            {
                provider.SetError(adat, " ");
                return true;
            }
            else if (startIndex==" ")
            {
                provider.SetError(adat, "A beviteli mező nem kezdődhet szóközzel!");
                return false;
            }
            else if (pattern2.IsMatch(adat.Text) && !string.IsNullOrEmpty(adat.Text) && !string.IsNullOrWhiteSpace(adat.Text) && adat.Text != "Kötelező kitölteni!")
            {
                provider.SetError(adat, "A beviteli mező nem tartalmazhat számot!");
                adat.Focus();
                return false;
            }
            else
            {
                
                provider.SetError(adat, "");
                return true;
            }

        }
        public static bool TextBoxSzamregex(ErrorProvider provider, TextBox adat)
        {
            Regex pattern1 = new Regex("^[0-9]*[1-9]*$");
           

            if (pattern1.IsMatch(adat.Text) && !string.IsNullOrEmpty(adat.Text) && !string.IsNullOrWhiteSpace(adat.Text) && adat.Text != "Kötelező kitölteni!")
            {
                provider.SetError(adat, "");
                provider.Clear();
                return true;
            }
            
            else if (string.IsNullOrEmpty(adat.Text) && string.IsNullOrWhiteSpace(adat.Text))
            {
                provider.SetError(adat, "");
                provider.Clear();
                return true;
            }
            else if (adat.Text == "Kötelező kitölteni!")
            {
                provider.SetError(adat, "");
                provider.Clear();
                return true;
            }
            else
            {
                provider.SetError(adat, "A beviteli mező csak számot tartalmazhat!");
                
                adat.Focus();
              
                return false;
            }
        }
        public static bool TextBoxSzame(ErrorProvider provider, TextBox adat, bool irsz=true)
        {
            if (adat.Text.Length != 0)
            {
                startIndex = adat.Text.Substring(0, 1);
            }

            if (string.IsNullOrWhiteSpace(adat.Text))
            {
                provider.SetError(adat, "A beviteli mező nem lehet üres és nem tartalmazhat szóközöket!");
                //adat.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(adat.Text))
            {
                provider.SetError(adat, "A beviteli mező nem lehet üres!");
                //adat.Focus();
                return false;
            }
            else if (startIndex == " ")
            {
                provider.SetError(adat, "A beviteli mező nem kezdődhet szóközzel!");
                return false;
            }
            else if (adat.Text != "Kötelező kitölteni!" && !vaneBenneSzam(adat.Text))
            {
                provider.SetError(adat, "A beviteli mező értéke csak szám lehet!");
                //adat.Focus();
                return false;
            }
            else if(adat.Text == "Kötelező kitölteni!") 
            {
                provider.SetError(adat, "A beviteli mező nem lehet üres!");
                //adat.Focus();
                return false;
            }else if(irsz && adat.Text.Length != 4) {
                provider.SetError(adat, "Az irányítószám csak 4 számjegyű lehet!");
                //adat.Focus();
                return false;
            }
            else
            {
                provider.SetError(adat, "");
                provider.Clear();
                return true;
            }
           
        }
        public static void TextBoxAdoSzame(ErrorProvider provider, TextBox adat)
        {
            if (string.IsNullOrWhiteSpace(adat.Text))
            {
               
                provider.SetError(adat, "A beviteli mező nem lehet üres és nem tartalmazhat szóközöket!");
                
            }
            if (adat.TextLength != 8)
            {
                
                provider.SetError(adat, "Az adószámnak 8 számjegyűnek kell lennie!");
               
            }
            else if (string.IsNullOrEmpty(adat.Text))
            {
                
                provider.SetError(adat, "A beviteli mező nem lehet üres!");
                
            }
            else if (adat.Text != "Kötelező kitölteni!" && !vaneBenneSzam(adat.Text))
            {
               
                provider.SetError(adat, "A beviteli mező értéke csak szám lehet!");
               
            }
            else
            {
               
                provider.SetError(adat, "");

                
            }
        }
      
        public static bool TextBoxNemKotelezoSzame(ErrorProvider provider, TextBox adat)
        {
            if (!string.IsNullOrEmpty(adat.Text) && !vaneBenneSzam(adat.Text) && adat.Text != "Kötelező kitölteni!")
            {
                provider.SetError(adat, "A beviteli mező értéke csak szám lehet!");
                //adat.Focus();
                return false;
            }
            else if (!string.IsNullOrEmpty(adat.Text) && adat.TextLength!=4 && adat.Text != "Kötelező kitölteni!")
            {
                provider.SetError(adat, "Az irányítószám csak 4 számjegyű lehet!");
                //adat.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(adat.Text))
            {
                provider.SetError(adat, "");
                provider.Clear();
                return true;
            }
            else if (string.IsNullOrWhiteSpace(adat.Text))
            {
                provider.SetError(adat, "");
                provider.Clear();
                return true;
            }else
            {
                provider.SetError(adat, "");
                provider.Clear();
                return true;
            }
        }


        public static bool TextBoxSemleges(ErrorProvider provider, TextBox adat)//nem kell
        {
            if (string.IsNullOrWhiteSpace(adat.Text))
            {            
                provider.SetError(adat, "A beviteli mező nem lehet üres és nem tartalmazhat szóközöket!");
                adat.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(adat.Text))
            {
                provider.SetError(adat, "A beviteli mező nem lehet üres!");
                adat.Focus();
                return false;

            }else if (adat.Text== "Kötelező kitölteni!") {
                provider.SetError(adat, "A beviteli mező nem lehet üres!");
                adat.Focus();
                return false;
            }
            else
            {
                provider.SetError(adat, "");
                provider.Clear();
                return true;
            }
          
        }
        public static bool TextBoxSemlegesUres(ErrorProvider provider, TextBox adat)
        {
            if (string.IsNullOrWhiteSpace(adat.Text))
            {
                provider.SetError(adat, "A beviteli mező nem lehet üres és nem tartalmazhat szóközöket!");
                adat.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(adat.Text))
            {
                provider.SetError(adat, "A beviteli mező nem lehet üres!");
                adat.Focus();
                return false;

            }
            else if (adat.Text == "Kötelező kitölteni!")
            {
                provider.SetError(adat, "A beviteli mező nem lehet üres!");
                adat.Focus();
                return false;
            }
            else
            {
                provider.SetError(adat, "");
                provider.Clear();
                return true;
            }

        }
        public static bool TextBoxEmail(ErrorProvider provider, TextBox adat)
        {
            Regex pattern = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

            if (adat.TextLength > 0 && !pattern.IsMatch(adat.Text))
            {              
                provider.SetError(adat, "Nem megfelelő e-mail formátum!");
                adat.Focus();
                return false;
            }
            else
            {
                provider.SetError(adat, "");
                provider.Clear();
                return true;
            }
            
        }
        public static bool EmailKivetel(string adat)
        {
            Regex pattern = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

            if (adat.Length > 0 && pattern.IsMatch(adat))
            {
                return true;
            }
            else if (adat.Length==0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool TextBoxIranyitoSzamRegex(ErrorProvider provider, TextBox adat)
        {
            Regex pattern1 = new Regex(@"^[1-9]{1}[0-9]{3}$");
            if (!pattern1.IsMatch(adat.Text) && adat.Text != "Kötelező kitölteni!" && !string.IsNullOrEmpty(adat.Text) && !string.IsNullOrWhiteSpace(adat.Text))
            {
                provider.SetError(adat, "Az irányítószámnak 4 számjegyűnek kell lennie!");
                adat.Focus();
                return false;
            }
            else
            {
                provider.SetError(adat, "");
                provider.Clear();
                return true;
            }

        }
        public static bool PostaTextBoxIranyitoSzamRegex(ErrorProvider provider, TextBox adat)
        {
            Regex pattern1 = new Regex(@"^[1-9]{1}[0-9]{3}$");
            if (!pattern1.IsMatch(adat.Text) && !string.IsNullOrEmpty(adat.Text) && !string.IsNullOrWhiteSpace(adat.Text) && adat.Text!="")
            {
                provider.SetError(adat, "Az irányítószámnak 4 számjegyűnek kell lennie!");
                //adat.Focus();
                return false;
            }
            else
            {
                provider.SetError(adat, "");
                provider.Clear();
                return true;
            }

        }

        public static bool IranyitoSzamRegex(string adat)
        {
            Regex pattern1 = new Regex(@"^[1-9]{1}[0-9]{3}$");


            if (pattern1.IsMatch(adat) && !string.IsNullOrEmpty(adat))
            {

                return true;
            }
            else 
            {

                return false;
            }
            

        }
        public static string PostaIranyitoSzamRegexKivetel(string adat)
        {
            Regex pattern1 = new Regex(@"^[1-9]{1}[0-9]{3}$");


            if (pattern1.IsMatch(adat) && !string.IsNullOrEmpty(adat))
            {

                return adat;
            }
            else if (string.IsNullOrEmpty(adat))
            {

                return "";
            }
            else if (string.IsNullOrWhiteSpace(adat))
            {

                return "";
            }
            else
            {

                return "hiba";
            }

        }

        public static string PostaTelepulesKivetel(string adat)
        {
            if (string.IsNullOrEmpty(adat))
            {
                 return "";
            }
            else if (string.IsNullOrWhiteSpace(adat))
            {
                return "";
            }
            else if (adat.Length>1 && adat.Length<20)
            {
                return adat;
            }
            else 
            {
                return "hiba";
            }

        }
        public static string PostaKozteruletKivetel(string adat)
        {
            if (string.IsNullOrEmpty(adat))
            {
                return "";
            }
            else if (string.IsNullOrWhiteSpace(adat))
            {
                return "";
            }
            else if (adat.ToString().Length > 1 && adat.ToString().Length < 27)
            {
                return adat;
            }
            else
            {
                return "hiba";
            }

        }
        /* public static bool NemKotelezoHazSzamRegex(string adat)
         {
             Regex pattern1 = new Regex(@"^[1-9]{1}[0-9]{0,2}(/){0,1}[a-z]{0,1}$");
             Regex pattern2 = new Regex(@"^[1-9]{1}[0-9]{0,2}(.){0,1}[a-z]{0,1}$");

             if (adat.Length > 0 && pattern1.IsMatch(adat))
             {
                 return true;
             }
            else if (adat.Length > 0 && pattern2.IsMatch(adat))
             {
                 return true;
             }
             else if (adat.Length==0)
             {
                 return true;
             }
             else
             {
                 return false;
             }
         }*/
        public static bool HazSzamRegex(string adat)
        {
            Regex pattern1 = new Regex(@"^[1-9]{1}[0-9]{0,2}(/){0,1}[a-z]{0,1}$");
            Regex pattern2 = new Regex(@"^[1-9]{1}[0-9]{0,2}(.){0,1}[a-z]{0,1}$");

            if (adat.Length > 0 && pattern1.IsMatch(adat))
            {
                return true;
            }
            if (adat.Length > 0 && pattern2.IsMatch(adat))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static string PostaHazSzamRegexkivetel(string adat)
        {
            Regex pattern1 = new Regex(@"^[1-9]{1}[0-9]{0,2}(/){0,1}[a-z]{0,1}$");
            Regex pattern2 = new Regex(@"^[1-9]{1}[0-9]{0,2}(.){0,1}[a-z]{0,1}$");

            if (adat.Length > 0 && pattern1.IsMatch(adat))
            {
                return adat;
            }
            if (adat.Length > 0 && pattern2.IsMatch(adat))
            {
                return adat;
            }
            else if(string.IsNullOrEmpty(adat))
            {
                return "" ;
            }
            else if (string.IsNullOrWhiteSpace(adat))
            {
                return "";
            }
            else
            {
                return "hiba";
            }
        }
        public static bool TextBoxPostaHazszamRegex(ErrorProvider provider, TextBox adat)
        {
            Regex pattern1 = new Regex(@"^[1-9]{1}[0-9]{0,2}(/){0,1}[a-z]{0,1}$");
            Regex pattern2 = new Regex(@"^[1-9]{1}[0-9]{0,2}(.){0,1}[a-z]{0,1}$");

            if (!pattern1.IsMatch(adat.Text) && adat.Text != "Kötelező kitölteni!" && !string.IsNullOrEmpty(adat.Text) && !string.IsNullOrWhiteSpace(adat.Text))
            {
                provider.SetError(adat, "Nem megfelelő házszám formátum!\nA helyes formátumok például 1/a, vagy 1/b\n,és a házszám 1-1000-ig terjed.");
                adat.Focus();
                return false;
            }
            else if (!pattern2.IsMatch(adat.Text) && adat.Text != "Kötelező kitölteni!" && !string.IsNullOrEmpty(adat.Text) && !string.IsNullOrWhiteSpace(adat.Text))
            {
                provider.SetError(adat, "Nem megfelelő házszám formátum!\nA helyes formátumok például 1/a, vagy 1/b\n,és a házszám 1-1000-ig terjed.");
                adat.Focus();
                return false;
            }
           
            else
            {

                provider.SetError(adat, "");
                provider.Clear();
                return true;
            }

        }
        public static bool TextBoxPostaEmeletAjtoRegex(ErrorProvider provider, TextBox adat)
        {
            Regex pattern1 = new Regex(@"^[1-9]{1}[0-9]{0,1}(\.){1}(em){1}(\.){1}[1-9]{1}[0-9]{0,1}$");
            Regex pattern2 = new Regex(@"^[1-9]{1}[0-9]{0,1}(\.){1}(\s){1}(em){1}(\.){1}(\s){1}[1-9]{0,1}[0-9]{0,1}$");            
            Regex pattern3 = new Regex(@"^[1-9]{1}[0-9]{0,1}(\.){1}[1-9]{1}[0-9]{0,1}$");
            Regex pattern4 = new Regex(@"^[1-9]{1}[0-9]{0,1}(/){1}[1-9]{1}[0-9]{0,1}$");
          

            if (pattern1.IsMatch(adat.Text) && !string.IsNullOrEmpty(adat.Text))
            {
                provider.SetError(adat, "");
                provider.Clear();
                return true;
            }
           else if (pattern2.IsMatch(adat.Text) && !string.IsNullOrEmpty(adat.Text))
            {
                provider.SetError(adat, "");
                provider.Clear();
                return true;
            }
            else if (pattern3.IsMatch(adat.Text) && !string.IsNullOrEmpty(adat.Text))
            {
                provider.SetError(adat, "");
                provider.Clear();
                return true;
            }
            else if (pattern4.IsMatch(adat.Text) && !string.IsNullOrEmpty(adat.Text))
            {
                provider.SetError(adat, "");
                provider.Clear();
                return true;
           }
            else if (string.IsNullOrEmpty(adat.Text))
            {
                provider.SetError(adat, "");
                provider.Clear();
                return true;
            }
            else if (string.IsNullOrWhiteSpace(adat.Text))
            {
                provider.SetError(adat, "");
                provider.Clear();
                return true;
            }
            else
            {

                provider.SetError(adat, "Nem megfelelő emelet/ajtó formátum!\nA helyes formátumok például 1.em.15,vagy 1.15 vagy 1/15\nvagy 1. em. 15,és az emelet szám 1-100-ig terjed.");
                adat.Focus();
                return false;
            }

        }
        public static string EmeletAjtoRegexKivetel(string adat)
        {
            Regex pattern1 = new Regex(@"^[1-9]{1}[0-9]{0,1}(.)(em)(.)[1-9]{0,1}[0-9]{0,1}$");
            Regex pattern2 = new Regex(@"^[1-9]{1}[0-9]{0,1}(.)(\s)(em)(.)(\s)[1-9]{0,1}[0-9]{0,1}$");
            Regex pattern3 = new Regex(@"^[1-9]{1}[0-9]{0,1}(.)[1-9]{0,1}[0-9]{0,1}$");
            Regex pattern4 = new Regex(@"^[1-9]{1}[0-9]{0,1}(/)[1-9]{0,1}[0-9]{0,1}$");

            string tmp="";
            if (pattern1.IsMatch(adat) && !string.IsNullOrEmpty(adat))
            {
                tmp = "";
            }
            else if (pattern2.IsMatch(adat) && !string.IsNullOrEmpty(adat))
            {
                tmp = "";
            }
            else if (pattern3.IsMatch(adat) && !string.IsNullOrEmpty(adat))
            {
                tmp = "";
            }
            else if (pattern4.IsMatch(adat) && !string.IsNullOrEmpty(adat))
            {

                tmp = "";
            }
            else if (string.IsNullOrEmpty(adat))
            {

                tmp = "";
            }
            else if (string.IsNullOrWhiteSpace(adat))
            {

                tmp = "";
            }
            else
            {

               
                tmp= "hiba";
            }return tmp;

        }
        

        public static bool TextBoxWebCim(ErrorProvider provider, TextBox adat)
        {
            Regex pattern = new Regex(@"^(?:https?://|www\.)[a-z]*(\.)[a-z]{2,3}$");
            if (adat.TextLength > 0 && !pattern.IsMatch(adat.Text))
            {
                provider.SetError(adat, "Nem megfelelő domain cím formátum!");
                adat.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(adat.Text) && string.IsNullOrWhiteSpace(adat.Text))
            {
                provider.SetError(adat, "");
                provider.Clear();

            }
            provider.SetError(adat, "");
            return true;
        }
        public static string WebCimKivetel(string adat)
        {
            Regex pattern = new Regex(@"^(?:https?://|www\.)[a-z]*(\.)[a-z]{2,3}$");


            if (pattern.IsMatch(adat))
            {

                return "jó";
            }
            else if(string.IsNullOrEmpty(adat))
            {
                return "";
            }
            else
            {
                return "hiba";
            }
           
        }
        public static bool TelefonSzamRegex(ErrorProvider provider, TextBox adat)
        {

            Regex pattern1 = new Regex(@"^(?:0620)[0-9]{7}$");
            Regex pattern2 = new Regex(@"^(?:0630)[0-9]{7}$");
            Regex pattern3 = new Regex(@"^(?:0650)[0-9]{7}$");
            Regex pattern4 = new Regex(@"^(?:0670)[0-9]{7}$");
            Regex pattern5 = new Regex(@"^(?:0680)[0-9]{7}$");
            Regex pattern6 = new Regex(@"^(?:061)[0-9]{7}$");//pesti hívószám
            Regex pattern7 = new Regex(@"^(?:06)[1-9]{2}[0-9]{6}$");//vidéki hívószám:

            if (pattern1.IsMatch(adat.Text))
            {
                provider.SetError(adat, "");
                provider.Clear();
                return true;
            }
            else if (pattern2.IsMatch(adat.Text))
            {
                provider.SetError(adat, "");
                provider.Clear();
                return true;
            }
            else if (pattern3.IsMatch(adat.Text))
            {
                provider.SetError(adat, "");
                provider.Clear();
                return true;
            }
            else if (pattern4.IsMatch(adat.Text))
            {
                provider.SetError(adat, "");
                provider.Clear();
                return true;
            }
            else if (pattern5.IsMatch(adat.Text))
            {
                provider.SetError(adat, "");
                provider.Clear();
                return true;
            }
            else if (pattern6.IsMatch(adat.Text))
            {
                provider.SetError(adat, "");
                provider.Clear();
                return true;
            }
            else if (pattern7.IsMatch(adat.Text))
            {
                provider.SetError(adat, "");
                provider.Clear();
                return true;
               
            }
            else
            {         
                provider.SetError(adat, "Nem megfelelő telefonszám formátum!");
                adat.Focus();
                return false;
            }
        }
        public static bool NemKotelezoTelefonSzamRegex(ErrorProvider provider, TextBox adat)
        {

            Regex pattern1 = new Regex(@"^(?:0620)[0-9]{7}$");
            Regex pattern2 = new Regex(@"^(?:0630)[0-9]{7}$");
            Regex pattern3 = new Regex(@"^(?:0650)[0-9]{7}$");
            Regex pattern4 = new Regex(@"^(?:0670)[0-9]{7}$");
            Regex pattern5 = new Regex(@"^(?:0680)[0-9]{7}$");
            Regex pattern6 = new Regex(@"^(?:061)[0-9]{7}$");//pesti hívószám
            Regex pattern7 = new Regex(@"^(?:06)[1-9]{2}[0-9]{6}$");//vidéki hívószám:

            if (pattern1.IsMatch(adat.Text) && adat.Text != "Kötelező kitölteni!" && !string.IsNullOrEmpty(adat.Text) && !string.IsNullOrWhiteSpace(adat.Text))
            {
                provider.SetError(adat, "");
                provider.Clear();
                return true;
            }
            else if (pattern2.IsMatch(adat.Text) && adat.Text != "Kötelező kitölteni!" && !string.IsNullOrEmpty(adat.Text) && !string.IsNullOrWhiteSpace(adat.Text))
            {
                provider.SetError(adat, "");
                provider.Clear();
                return true;
            }
            else if (pattern3.IsMatch(adat.Text) && adat.Text != "Kötelező kitölteni!" && !string.IsNullOrEmpty(adat.Text) && !string.IsNullOrWhiteSpace(adat.Text))
            {
                provider.SetError(adat, "");
                provider.Clear();
                return true;
            }
            else if (pattern4.IsMatch(adat.Text) && adat.Text != "Kötelező kitölteni!" && !string.IsNullOrEmpty(adat.Text) && !string.IsNullOrWhiteSpace(adat.Text))
            {
                provider.SetError(adat, "");
                provider.Clear();
                return true;
            }
            else if (pattern5.IsMatch(adat.Text) && adat.Text != "Kötelező kitölteni!" && !string.IsNullOrEmpty(adat.Text) && !string.IsNullOrWhiteSpace(adat.Text))
            {
                provider.SetError(adat, "");
                provider.Clear();
                return true;
            }
            else if (pattern6.IsMatch(adat.Text) && adat.Text != "Kötelező kitölteni!" && !string.IsNullOrEmpty(adat.Text) && !string.IsNullOrWhiteSpace(adat.Text))
            {
                provider.SetError(adat, "");
                provider.Clear();
                return true;
            }
            else if (pattern7.IsMatch(adat.Text) && adat.Text != "Kötelező kitölteni!" && !string.IsNullOrEmpty(adat.Text) && !string.IsNullOrWhiteSpace(adat.Text))
            {
                provider.SetError(adat, "");
                provider.Clear();
                return true;

            }
            else if (string.IsNullOrEmpty(adat.Text) && string.IsNullOrWhiteSpace(adat.Text))
            {
                provider.SetError(adat, "");
                provider.Clear();
                return true;
            }
            else if (adat.Text=="Kötelező kitölteni!")
            {
                provider.SetError(adat, "");
                provider.Clear();
                return true;
            }          
            else
            {
                provider.SetError(adat, "Nem megfelelő telefonszám formátum!\nA helyes formátum például\n061..., vagy 0620...");
                adat.Focus();

                return false;
            }
        }
        public static bool TelefonSzamKivetel(string adat)
        {

            Regex pattern1 = new Regex(@"^(?:0620)[0-9]{7}$");
            Regex pattern2 = new Regex(@"^(?:0630)[0-9]{7}$");
            Regex pattern3 = new Regex(@"^(?:0650)[0-9]{7}$");
            Regex pattern4 = new Regex(@"^(?:0670)[0-9]{7}$");
            Regex pattern5 = new Regex(@"^(?:0680)[0-9]{7}$");
            Regex pattern6 = new Regex(@"^(?:061)[0-9]{7}$");//pesti hívószám
            Regex pattern7 = new Regex(@"^(?:06)[1-9]{2}[0-9]{6}$");//vidéki hívószám:

            if (pattern1.IsMatch(adat))
            {
                return true;
            }
            else if (pattern2.IsMatch(adat))
            {
                return true;

            }
            else if (pattern3.IsMatch(adat))
            {
                return true;

            }
            else if (pattern4.IsMatch(adat))
            {
                return true;

            }
            else if (pattern5.IsMatch(adat))
            {

                return true;
            }
            else if (pattern6.IsMatch(adat))
            {
                return true;

            }
            else if (pattern7.IsMatch(adat))
            {

                return true;
            }
            else
            {

                return false;
            }
        }
        public static void FaxRegex(ErrorProvider provider, TextBox adat)
        {
            Regex pattern1 = new Regex(@"^(?:061)[0-9]{7}$");//pesti hívószám
            Regex pattern2 = new Regex(@"^(?:06)[1-9]{2}[0-9]{6}$");

            if (pattern1.IsMatch(adat.Text) && !string.IsNullOrEmpty(adat.Text) && !string.IsNullOrWhiteSpace(adat.Text))
            {
                
                provider.SetError(adat, "");
                provider.Clear();

            }
            else if (pattern2.IsMatch(adat.Text) && !string.IsNullOrEmpty(adat.Text) && !string.IsNullOrWhiteSpace(adat.Text))
            {
                provider.SetError(adat, "");
                provider.Clear();
            }
            else if (string.IsNullOrEmpty(adat.Text) && string.IsNullOrWhiteSpace(adat.Text))
            {
                provider.SetError(adat, "");
                provider.Clear();
                
            }
            else
            {
                
                provider.SetError(adat, "Nem megfelelő Fax formátum!\nA helyes formátumok például:\n0612222222, vagy 06281111111.");
                adat.Focus();
            }
        }
        public static bool FaxKivetel(string adat)
        {
            Regex pattern1 = new Regex(@"^(?:061)[0-9]{7}$");//pesti hívószám
            Regex pattern2 = new Regex(@"^(?:06)[1-9]{2}[0-9]{6}$");
            if (pattern1.IsMatch(adat))
            {

                return true;

            }
            else if (pattern2.IsMatch(adat))
            {
                return true;
            }
            else if (adat.Length == 0)
            {
                return true;
            }
            else
            {

                return false;
            }
        }

        public static string PropertyTelefonSzamRegex(int value)
        {
            Regex pattern1 = new Regex(@"^(?:0620)[0-9]{7}$");
            Regex pattern2 = new Regex(@"^(?:0630)[0-9]{7}$");
            Regex pattern3 = new Regex(@"^(?:0650)[0-9]{7}$");
            Regex pattern4 = new Regex(@"^(?:0670)[0-9]{7}$");
            Regex pattern5 = new Regex(@"^(?:0680)[0-9]{7}$");
            Regex pattern6 = new Regex(@"^(?:061)[0-9]{7}$");//pesti hívószám
            Regex pattern7 = new Regex(@"^(?:06)[1-9]{2}[0-9]{6}$");//vidéki hívószám:

            if (!(pattern1.IsMatch(Convert.ToString(value)) && pattern2.IsMatch(value.ToString()) && pattern3.IsMatch(value.ToString()) && pattern4.IsMatch(value.ToString()) && pattern5.IsMatch(value.ToString()) && pattern6.IsMatch(value.ToString()) && pattern7.IsMatch(value.ToString())))

            {
                return "Nem megfelelő telefonszám formátum!";
            }

            else
            {

                return "";
            }
        }

        public static void StringLetilto(ErrorProvider provider, TextBox adat, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8 || e.KeyChar == 127)
            {
                e.Handled = false;
                provider.SetError(adat, "");
                provider.Clear();
            }
            else if (adat.Text == "Kötelező kitölteni!")
            {
                e.Handled = false;
                provider.SetError(adat, "");
                provider.Clear();
            }
            else
            {
                e.Handled = true;

                provider.SetError(adat, "A bevitt érték csak pozitív egész szám lehet!");
                adat.Focus();

            }
        }
     
        public static void StringLetiltoDecimal(TextBox adat, KeyPressEventArgs e)
        {
            if (adat.Text=="" && e.KeyChar==48)
            {
                e.Handled = true;
            }
            else if (char.IsDigit(e.KeyChar) || e.KeyChar == 8 || e.KeyChar == 127 || e.KeyChar == ',')
            {
                e.Handled = false;
            
            }
            else
            {
                e.Handled = true;

             

            }
        }
        public static void StringLetiltoDecimal2(TextBox adat, KeyPressEventArgs e)
        {
           
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8 || e.KeyChar == 127 || e.KeyChar == ',')
            {
                e.Handled = false;

            }
            else
            {
                e.Handled = true;



            }
        }
        internal static bool ModositTorolGomb(bool igaze, ToolStripMenuItem menu1, ToolStripMenuItem menu2)
        {
           
            if (!igaze)
            {

                menu1.ForeColor = Color.Gray;              
                menu2.ForeColor = Color.Gray;
                
            }
            else
            {
                menu1.ForeColor = SystemColors.WindowText;             
                menu2.ForeColor = SystemColors.WindowText;
               
            }
            return igaze;

        }
        internal static bool Kijelol(bool igaze, Button menu1)
        {
            menu1.Enabled = igaze;
            return igaze;

        }
        public static string getHelp(string name)
        {
            string result = "";
            string tmp = Encoding.Unicode.GetString(File.ReadAllBytes($@"{Environment.CurrentDirectory}\help.bin"));
            string[] tmpArr = tmp.Split('\0');
            for (int i = 0; i < tmpArr.Length; i += 2)
            {
                if (tmpArr[i] == name) result = tmpArr[i + 1];
            }
            return result;
        }

    }


}
