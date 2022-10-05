using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szakdolgozat3
{
   public class Nev
    {
        int? nevAzonosito;
        string titulus;
        string vezetekNev;
        string keresztNev;
        string utoNev;
        string cegNev;

        public string Titulus
        {
            get => titulus;
            set
            {
               
                
                if (KivetelKezeles.NullStringeKivetel(value)==true)
                {
                    titulus = value;
                }
                else
                {
                    throw new ArgumentException("Az adatok hiányosan, vagy nem megfelelően lettek kitöltve!", "Titulus");
                   
                }

            }
        }
        public string VezetekNev
        {
            get => vezetekNev;
            set
            {
              
                if (KivetelKezeles.StringeKivetel(value)==true && value.ToString()!= "Kötelező kitölteni!")
                {
                    vezetekNev = value; 
                }
                else
                {
                    throw new ArgumentException("Az adatok hiányosan, vagy nem megfelelően lettek kitöltve!", "Vezetéknév");
                }
                
            }
        }
        public string KeresztNev
        {
            get => keresztNev;
            set
            {
                if (KivetelKezeles.StringeKivetel(value) == true && value.ToString() != "Kötelező kitölteni!")
                {
                    keresztNev = value;
                }
                else
                {
                    throw new ArgumentException("Az adatok hiányosan, vagy nem megfelelően lettek kitöltve!", "Keresztnév");
                }

            }
        }
        public string UtoNev
        {
            get => utoNev;
            set
            {
               
                if (KivetelKezeles.NullStringeKivetel(value)==true)
                {
                    utoNev = value;
                }
                else
                {
                    throw new ArgumentException("Az adatok hiányosan, vagy nem megfelelően lettek kitöltve!", "Utónév");
                }

            }
        }

        public int? NevAzonosito { get => nevAzonosito; set => nevAzonosito = value; }
        public string CegNev
        {
            get => cegNev;
            set
            {

                if (KivetelKezeles.CegKivetel(value) == true && value.ToString() != "Kötelező kitölteni!")
                {
                    cegNev = value;
                }
                else
                {
                    throw new ArgumentException("Az adatok hiányosan, vagy nem megfelelően lettek kitöltve!", "Cégnév");
                }

            }
        }
        

        public Nev(string titulus, string vezetekNev, string keresztNev, string utoNev,string cegNev)
        {
            Titulus = titulus;
            VezetekNev = vezetekNev;
            KeresztNev = keresztNev;
            UtoNev = utoNev;
            CegNev = cegNev;
        }
        public Nev(int? nevAzonosito,string titulus, string vezetekNev, string keresztNev, string utoNev, string cegNev)
        {
            NevAzonosito = nevAzonosito;
            Titulus = titulus;
            VezetekNev = vezetekNev;
            KeresztNev = keresztNev;
            UtoNev = utoNev;
            CegNev = cegNev;


        }


        /*   public override string ToString()
           {
               return $"{Titulus}-{VezetekNev}-{KeresztNev}-{UtoNev}";
           }
           public string CSVMentes()
           {
               return $"{Titulus};{VezetekNev};{KeresztNev};{UtoNev}";
           }
           public static Nev CSVBetoltes(string[] csvAdatok)
           {
               return new Nev(csvAdatok[0],csvAdatok[1],csvAdatok[2],csvAdatok[3]); 
           }*/
    }
}
