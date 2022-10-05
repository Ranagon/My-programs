using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szakdolgozat3
{
    public enum KozteruletJelleg
    {
        Út,
        Utca,
        Tér,
        Park
    }

    internal abstract class Cim:Nev
    {
        int? cimAzonosito;
        string orszag;   
        string irsz;
        string telepules;
        string kozterulet;
        string hazSzam;
        string emeletAjto;
        KozteruletJelleg kozteruletJelleg;
        string postaOrszag;
        string postaIrsz;
        string postaTelepules;
        string postaKozterulet;
        string postaHazSzam;
        string postaEmeletAjto;
        KozteruletJelleg postaKozteruletJelleg;

        public string Orszag
        {
            get => orszag;
            set
            {
                if (KivetelKezeles.StringeKivetel(value)==true || value.ToString() == "Kötelező kitölteni!")
                {
                    orszag = value;
                }
                else
                {
                    throw new ArgumentException("Az adatok hiányosan, vagy nem megfelelően lettek kitöltve!","Ország");
                }
                
            }
        }
        public string Irsz
        {
            get => irsz;
            set
            {
                if (KivetelKezeles.IranyitoSzamRegex(value) == true || value.ToString() != "Kötelező kitölteni!")
                {
                    irsz = value;
                }
                else
                {
                    throw new ArgumentException("Az adatok hiányosan, vagy nem megfelelően lettek kitöltve!", "Házszám");
                }

            }
        }
        public string Telepules
        {
            get => telepules;
            set
            {
                if (string.IsNullOrEmpty(value) || value.ToString() == "Kötelező kitölteni!")
                {
                    throw new ArgumentException("Az adatok hiányosan, vagy nem megfelelően lettek kitöltve!", "Település");
                }
                else if (string.IsNullOrWhiteSpace(value) || value.ToString() == "Kötelező kitölteni!")
                {
                    throw new ArgumentException("Az adatok hiányosan, vagy nem megfelelően lettek kitöltve!", "Település");
                }
                else if (value.ToString().Length<2 && value.ToString().Length> 16 || value.ToString() == "Kötelező kitölteni!")
                {
                    throw new ArgumentException("Az adatok hiányosan, vagy nem megfelelően lettek kitöltve!", "Település");
                }
                {
                    telepules = value;
                    
                }

            }
        }
        public string Kozterulet
        {
            get => kozterulet;
            set
            {
                if (string.IsNullOrEmpty(value) || value.ToString() == "Kötelező kitölteni!")
                {
                    throw new ArgumentException("Az adatok hiányosan, vagy nem megfelelően lettek kitöltve!", "Közterület");
                }
                else if (string.IsNullOrWhiteSpace(value) || value.ToString() == "Kötelező kitölteni!")
                {
                    throw new ArgumentException("Az adatok hiányosan, vagy nem megfelelően lettek kitöltve!", "Közterület");
                }
                else if (value.ToString().Length < 1 && value.ToString().Length > 27 || value.ToString() == "Kötelező kitölteni!")
                {
                    throw new ArgumentException("Az adatok hiányosan, vagy nem megfelelően lettek kitöltve!", "Közterület");
                }
                else
                {
                    kozterulet = value;

                }

            }
        }


        public KozteruletJelleg KozteruletJelleg
        {
            get => kozteruletJelleg;
            set
            {
              
                kozteruletJelleg = value;
            }
        }

        public string HazSzam
        {
            get => hazSzam;
            set
            {
                if (KivetelKezeles.HazSzamRegex(value) == true && value.ToString() != "Kötelező kitölteni!")
                {
                    hazSzam = value;
                }
                else
                {
                    throw new ArgumentException("Az adatok hiányosan, vagy nem megfelelően lettek kitöltve!", "Házszám");
                }
            }
        }
        public string EmeletAjto
        {
            get => emeletAjto;
            set
            {
                if (KivetelKezeles.EmeletAjtoRegexKivetel(value) == "")
                {
                    emeletAjto = value;
                }
                else
                {
                    throw new ArgumentException("Az adatok hiányosan, vagy nem megfelelően lettek kitöltve!", "Emelet/ajtó");
                }
              
            }
        }
        public int? CimAzonosito { get => cimAzonosito; set => cimAzonosito = value; }
        public string PostaOrszag
        {
            get => postaOrszag; set///ezeknél stringet kell visszaadni, ami maga a value lesz, mert null esetén "" lesz.
            {
                if (string.IsNullOrEmpty(value))
                {
                    postaOrszag = "";
                }
                else if (string.IsNullOrWhiteSpace(value))
                {
                    postaOrszag = "";
                }
                else
                {
                    postaOrszag = value;

                }
              
            }
        }
        public string PostaIrsz
        {
            get => postaIrsz;
            set
            {
                
                if (KivetelKezeles.PostaIranyitoSzamRegexKivetel(value)!= "hiba")
                {
                    postaIrsz = value;
                }
                else
                {
                    throw new ArgumentException("Az adatok hiányosan, vagy nem megfelelően lettek kitöltve!", "Posta irányítószám");
                }
            }
        }
        public string PostaTelepules
        {
            get => postaTelepules; set
            {
                if (KivetelKezeles.PostaTelepulesKivetel(value) != "hiba")
                {
                    postaTelepules = value;
                }
                else
                {
                    throw new ArgumentException("Az adatok hiányosan, vagy nem megfelelően lettek kitöltve!", "Posta telepulés");
                }


            }
        }
        public string PostaKozterulet
        {
            get => postaKozterulet;
            set
            {
                if (KivetelKezeles.PostaKozteruletKivetel(value)!= "hiba")
                {
                    postaKozterulet = value;
                }
                else
                {
                    throw new ArgumentException("Az adatok hiányosan, vagy nem megfelelően lettek kitöltve!", "Posta közterület");
                    
                }
                
            }
        }
        public string PostaHazSzam
        {
            get => postaHazSzam;
            set
            {
                if (KivetelKezeles.PostaHazSzamRegexkivetel(value)!="hiba")
                {
                    postaHazSzam = value;
                }
                else
                {
                    throw new ArgumentException("Az adatok hiányosan, vagy nem megfelelően lettek kitöltve!", "Posta házszám");
                }
              
            }
        }
        public string PostaEmeletAjto
        {
            get => postaEmeletAjto;
            set
            {
                if (KivetelKezeles.EmeletAjtoRegexKivetel(value)!= "hiba")
                {
                    postaEmeletAjto = value;
                }
                else
                {
                    throw new ArgumentException("Az adatok hiányosan, vagy nem megfelelően lettek kitöltve!", "Posta házszám");
                }
                
            }
        }
        public KozteruletJelleg PostaKozteruletJelleg { get => postaKozteruletJelleg; set => postaKozteruletJelleg = value; }

        internal Cim(string titulus, string vezetekNev, string keresztNev, string utoNev,string cegNev, string orszag, string irsz, string telepules, string kozterulet, KozteruletJelleg kozteruletJelleg, string hazSzam, string emeletAjto, string postaOrszag,string postaIrsz, string postaTelepules, string postaKozterulet, KozteruletJelleg postaKozteruletJelleg, string postaHazSzam, string postaEmeletAjto) : base(titulus,vezetekNev,keresztNev,utoNev,cegNev)
        {
            Orszag = orszag;
            Irsz = irsz;
            Telepules = telepules;
            Kozterulet = kozterulet;         
            KozteruletJelleg = kozteruletJelleg;
            HazSzam = hazSzam;
            EmeletAjto = emeletAjto;
            PostaOrszag = postaOrszag;
            PostaIrsz = postaIrsz;
            PostaTelepules = postaTelepules;
            PostaKozterulet = postaKozterulet;
            PostaHazSzam = postaHazSzam;
            PostaEmeletAjto = postaEmeletAjto;
            PostaKozteruletJelleg = postaKozteruletJelleg;

        }
        internal Cim(int? cimAzonosito,string titulus, string vezetekNev, string keresztNev, string utoNev,string cegNev, string orszag, string irsz, string telepules, string kozterulet, KozteruletJelleg kozteruletJelleg, string hazSzam, string emeletAjto, string postaOrszag, string postaIrsz, string postaTelepules, string postaKozterulet, KozteruletJelleg postaKozteruletJelleg, string postaHazSzam, string postaEmeletAjto) : base(titulus, vezetekNev, keresztNev, utoNev,cegNev)
        {
            CimAzonosito = cimAzonosito;
            Orszag = orszag;
            Irsz = irsz;
            Telepules = telepules;
            Kozterulet = kozterulet;
            KozteruletJelleg = kozteruletJelleg;
            HazSzam = hazSzam;
            EmeletAjto = emeletAjto;
            PostaOrszag = postaOrszag;
            PostaIrsz = postaIrsz;
            PostaTelepules = postaTelepules;
            PostaKozterulet = postaKozterulet;
            PostaHazSzam = postaHazSzam;
            PostaEmeletAjto = postaEmeletAjto;
            PostaKozteruletJelleg = postaKozteruletJelleg;

        }

        public override string ToString()
        {
            //return base.ToString();
            return $"{orszag} - {irsz} - {telepules} - {kozterulet} - {kozteruletJelleg} - {HazSzam} - {EmeletAjto}-{postaOrszag}-{PostaIrsz}-{PostaTelepules}-{PostaKozterulet}-{PostaKozteruletJelleg}-{PostaHazSzam}-{PostaEmeletAjto}";
        }
        public string CSVMentes()
        {
            return $"{orszag};{Irsz};{Telepules};{Kozterulet};{KozteruletJelleg};{HazSzam};{EmeletAjto};{postaOrszag};{PostaIrsz};{PostaTelepules};{PostaKozterulet};{PostaKozteruletJelleg};{PostaHazSzam};{PostaEmeletAjto}";//offline munkához jó majd a módosítás dátumát és a felhasználót hozzá kell csapni.      
        }
       /* public static Cim  CSVbetoltes(string[]csvAdatok)
        {
            return new Cim(csvAdatok[0],csvAdatok[1],csvAdatok[2],csvAdatok[3],csvAdatok[4],csvAdatok[5],csvAdatok[6],csvAdatok[7],(KozteruletJelleg)int.Parse(csvAdatok[8]),csvAdatok[9],csvAdatok[10]);
        } Ez majd az egyéb adatok végén lesz a többivel együtt, mivel a cím és az egyéb adatok abstractok*/ 
    }
}
