 using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szakdolgozat3
{
   
    enum PartnerkeresesiKategoriak
    {
        Név,
        
    }
  
    internal class Partner : Cim //cim lesz az őse
    {
        
        //static BindingList<sqlString> partnerCsoportok = new BindingList<sqlString>(ABKezelo.PartnerBesorolasBetoltes());
        //internal static BindingList<sqlString> fizetesiModok = new BindingList<sqlString>(ABKezelo.FizetesModBetoltes());
        //static BindingList<Partner> partnerek = new BindingList<Partner>();
        int partnerAzonosito;
        string adoSzam;          //Kell partner ID
        string EUAdoszam;
        //Egyéb adatok

        string telefonSzam;
        string fax;
        string email;
        string webCim;
        string bankSzamlaSzam;
        string partnerCsoport;
        string fizetesiMod;
        decimal kedvezmeny;
        string szamlaMegjegyzes;
        string partnerMegjegyzes;

             
        public string AdoSzam
        {
            get => adoSzam;
            set
            {
                
                if (KivetelKezeles.AdoSzamKivetel(value)==true && value.ToString() != "Kötelező kitölteni!")
                {
                    adoSzam = value;
                }
                else
                {
                   throw new ArgumentException("Az adatok hiányosan, vagy nem megfelelően lettek kitöltve!", "Adószám");
                }
                
            }
        }
        public string EUAdoszam1
        {
            get => EUAdoszam;
            set
            {
             
                
                    EUAdoszam = value;
            }
        }
        
        public string TelefonSzam
        {
            get => telefonSzam; set
            {
             
               if (KivetelKezeles.TelefonSzamKivetel(value.ToString())==true)
                {
                    telefonSzam = value;
                }
                else
                {
                    throw new ArgumentException("Az adatok hiányosan, vagy nem megfelelően lettek kitöltve!","Telefonszám");
                }
               
                
            }
        }
        public string Fax
        {
            
            get => fax; set
            {

                if (KivetelKezeles.FaxKivetel(value.ToString()) == true)
                {
                    fax = value;
                }
                else
                {
                    throw new ArgumentException("Az adatok hiányosan, vagy nem megfelelően lettek kitöltve!", "fax");
                }

            }
        }
        public string Email
        {
            get => email; set
            {
                if (KivetelKezeles.EmailKivetel(value.ToString())==true)
                {
                    email = value;
                }
                else
                {
                    throw new ArgumentException("Az adatok hiányosan, vagy nem megfelelően lettek kitöltve!", "Email");
                }
            }
        }
        public string WebCim
        {
            get => webCim; set
            {
                string bemenet = KivetelKezeles.WebCimKivetel(value);
                if (bemenet=="jó")
                {
                    webCim = value;
                }
                else if (bemenet=="")
                {
                    webCim = value;
                }
                else
                {
                    throw new ArgumentException("Az adatok hiányosan, vagy nem megfelelően lettek kitöltve!", "Webcim");
                }
            }
        }
        public string BankSzamlaSzam
        {
            get => bankSzamlaSzam; set
            {
                bankSzamlaSzam = value;
            }
        }
        public string PartnerCsoport
        {
            get => partnerCsoport; set
            {
                if (value!=null)
                {
                    partnerCsoport = value;
                }
                else
                {
                    throw new ArgumentNullException("Hiba");
                }
       
            }   
    }
        public string FizetesiMod
        {
            get => fizetesiMod; set
            {
                if (value != null)
                {
                    fizetesiMod = value;
                }
                else
                {
                    throw new ArgumentNullException("Hiba");
                }
            }
        }
        public decimal Kedvezmeny
        {
            get => kedvezmeny; set
            {
                kedvezmeny = value;
            }
        }
        public string SzamlaMegjegyzes
        {
            get => szamlaMegjegyzes; set
            {
                szamlaMegjegyzes = value;
            }
        }
        public string PartnerMegjegyzes
        {
            get => partnerMegjegyzes; set
            {
                partnerMegjegyzes = value;
            }
        }
             
        public int PartnerAzonosito { get => partnerAzonosito; set => partnerAzonosito = value; }

        internal Partner(
          int partnerAzonosito, string adoSzam, string eUAdoszam1, string telefonSzam, string fax, 
          string email, string webCim, string bankSzamlaSzam, string partnerCsoport, string fizetesiMod,
          decimal kedvezmeny, string szamlaMegjegyzes, string partnerMegjegyzes, string titulus, 
          string vezetekNev, string keresztNev, string utoNev, string cegNev, string orszag, string irsz,
          string telepules, string kozterulet, KozteruletJelleg kozteruletJelleg, string hazszam,
          string emeletAjto, string postaOrszag, string postaIrsz, string postaTelepules, 
          string postaKozterulet, KozteruletJelleg postaKozteruletJelleg, string postaHazSzam, 
          string postaEmeletAjto) : base(titulus, vezetekNev, keresztNev, utoNev, cegNev, orszag,
          irsz, telepules, kozterulet, kozteruletJelleg, hazszam, emeletAjto, postaOrszag, postaIrsz, 
          postaTelepules, postaKozterulet, postaKozteruletJelleg, postaHazSzam, postaEmeletAjto)
          {
            
                PartnerAzonosito = partnerAzonosito;
                AdoSzam = adoSzam;
                EUAdoszam1 = eUAdoszam1;             
                TelefonSzam = telefonSzam;
                Fax = fax;
                Email = email;
                WebCim = webCim;
                BankSzamlaSzam = bankSzamlaSzam;
                PartnerCsoport = partnerCsoport;
                FizetesiMod = fizetesiMod;
                Kedvezmeny = kedvezmeny;// ezt a partner listájából kell kiszedni
                SzamlaMegjegyzes = szamlaMegjegyzes;
                PartnerMegjegyzes = partnerMegjegyzes;


            

        }

        internal Partner(
            string adoSzam, string eUAdoszam1, string telefonSzam, string fax, string email, string webCim, 
            string bankSzamlaSzam, string partnerCsoport, string fizetesiMod, decimal kedvezmeny, 
            string szamlaMegjegyzes, string partnerMegjegyzes, string titulus, string vezetekNev, 
            string keresztNev, string utoNev, string cegNev, string orszag, string irsz, string telepules,
            string kozterulet, KozteruletJelleg kozteruletJelleg, string hazszam, string emeletAjto,
            string postaOrszag, string postaIrsz, string postaTelepules, string postaKozterulet, 
            KozteruletJelleg postaKozteruletJelleg, string postaHazSzam, string postaEmeletAjto) :
            base(titulus, vezetekNev, keresztNev, utoNev, cegNev, orszag, irsz, telepules, kozterulet,
            kozteruletJelleg, hazszam, emeletAjto, postaOrszag, postaIrsz, postaTelepules,
            postaKozterulet, postaKozteruletJelleg, postaHazSzam, postaEmeletAjto)
        {
            
                AdoSzam = adoSzam;
                EUAdoszam1 = eUAdoszam1;
                TelefonSzam = telefonSzam;
                Fax = fax;
                Email = email;
                WebCim = webCim;
                BankSzamlaSzam = bankSzamlaSzam;
                PartnerCsoport = partnerCsoport;
                FizetesiMod = fizetesiMod;
                Kedvezmeny = kedvezmeny;// ezt a partner listájából kell kiszedni
                SzamlaMegjegyzes = szamlaMegjegyzes;
                PartnerMegjegyzes = partnerMegjegyzes;
               

            
           
        }
     
        public override string ToString()
        {
            return $"{AdoSzam};{EUAdoszam};{TelefonSzam};{Fax};{Email};{WebCim};{BankSzamlaSzam};" +
                $"{PartnerCsoport};{FizetesiMod};{Kedvezmeny};{SzamlaMegjegyzes};{PartnerMegjegyzes}";
        }
    }
}
