using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Szakdolgozat3
{
   
    public class Termek
    {
        public struct AFA 
        {
          
            decimal value;
            int sqlID;
            public int SQLID {
                get => sqlID;
                set {
                    sqlID = value;
                }
            }
           
            public decimal Value
            {
                get => value;
                set
                {
                    if (this.value > 100)
                    {
                        throw new ArgumentException("Az adó mértékének 0% és 100% közé kell esnie!");
                    }
                    else
                    {
                        this.value = value;
                    }
                }
            }

            public override string ToString()
            {
                return $" {value} % ";
            }

        }
       
        int termekAzonosito;
        string termekKategoria;
        string megnevezes;
        int mennyiseg;
        string mennyisegEgysege;
        decimal bruttoAr;
        decimal nettoAr;
        decimal afaKulcs;    
        string cikkszam;
        string navSorszam;     
        int minimalisKeszlet;
       
        public string Megnevezes
        {
            get => megnevezes; set
            {
                if (value.ToString() != "Kötelező kitölteni!")
                {
                    megnevezes = value;
                }
                else
                {
                    throw new ArgumentException("A beviteli mező nem lehet üres!", "Megnevezés");
                }
            }
        }
        public int Mennyiseg
        {
            get => mennyiseg; set
            {
               
                    mennyiseg = value;
               

            }
        }
        public string MennyisegEgysege
        {
            get => mennyisegEgysege;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    mennyisegEgysege = value;
                }
                else
                {
                    throw new ArgumentException("A 'Mennyígég egysége' beviteli mező nem lehet üres!","mennyiseg");
                }

            }
        }
        public decimal BruttoAr
        {
            get => bruttoAr;
            set
            {
                if (value > -1)
                {
                    bruttoAr = value;
                }
                else
                {
                    throw new ArgumentException("Az érték csak pozitív egész szám lehet!", "brutto ár");
                }
            }
        }
        public decimal NettoAr
        {
            get => nettoAr; set
            {
                if (value > 0)
                {
                    nettoAr = value;
                }

                else
                {
                    throw new ArgumentException("Az érték csak pozitív egész szám lehet!", "Netto ár");
                }

            }
        }

        public string Cikkszam
        {
            get => cikkszam;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    cikkszam =value;
                }
                else
                {
                    cikkszam = value;
                }
            }
        }
        public string NavSorszam
        {
            get => navSorszam;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    navSorszam = value;
                }
                else
                {
                    navSorszam = value;
                }
            }
        }
     
        public int MinimalisKeszlet
        {
            get => minimalisKeszlet;
            set
            {
                if (value > -1)
                {
                   minimalisKeszlet = value;
                }
                else
                {
                    throw new ArgumentException("Az érték nem lehet nullánál kisebb!","Minimaliskeszlet");
                }
            }
        }
        public string TermekKategoria
        {
            get => termekKategoria;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    termekKategoria = value;
                }
                else
                {
                    throw new ArgumentException("A beviteli mező nem lehet üres!", "Termékkategória");
                }
            }
        }
        public decimal AfaKulcs
        {
            get => afaKulcs;

            set
            {
                if (value>-1)
                {
                    afaKulcs = value;
                }
                else
                {
                    throw new ArgumentException("A beviteli mező nem lehet üres", "ÁFA kulcs");
                }           
            }
        }      
        public int TermekAzonosito { get => termekAzonosito; set => termekAzonosito = value; }
        public Termek(string termekKategoria, string megnevezes, string cikkszam, int mennyiseg, string mennyisegEgysege, decimal nettoAr, decimal bruttoAr, decimal afaKulcs, string nAVsorszam1, int minimalisKeszlet)
        {
            TermekKategoria = termekKategoria;
            Megnevezes = megnevezes;
            Cikkszam = cikkszam;
            Mennyiseg = mennyiseg;
            MennyisegEgysege = mennyisegEgysege;
            NettoAr = nettoAr;
            BruttoAr = bruttoAr;
            AfaKulcs = afaKulcs;
            NavSorszam = nAVsorszam1;
            MinimalisKeszlet = minimalisKeszlet;
        }
        public Termek(int termekAzonosito,string termekKategoria, string megnevezes, string cikkszam, int mennyiseg, string mennyisegEgysege, decimal nettoAr,decimal bruttoAr,decimal afaKulcs, string nAVsorszam1, int minimalisKeszlet)
        {
            TermekAzonosito = termekAzonosito;
            TermekKategoria = termekKategoria;
            Megnevezes = megnevezes;
            Cikkszam = cikkszam;
            Mennyiseg = mennyiseg;
            MennyisegEgysege = mennyisegEgysege;
            NettoAr = nettoAr;
            BruttoAr = bruttoAr;
            AfaKulcs = afaKulcs;
            NavSorszam = nAVsorszam1;
            MinimalisKeszlet = minimalisKeszlet;           
        }
        public override string ToString()
        {
            return $"{TermekAzonosito} - {TermekKategoria} - {Megnevezes} - {Cikkszam} - {Mennyiseg} - {MennyisegEgysege} - {NettoAr} - {bruttoAr} - {AfaKulcs} - {NavSorszam}";


        }
        public virtual string Serialize()
        {
            return $"{TermekAzonosito}●{TermekKategoria}●{Megnevezes}●{Cikkszam}●{Mennyiseg}●{MennyisegEgysege}●{NettoAr}●{bruttoAr}●{AfaKulcs}●{NavSorszam}●{minimalisKeszlet}";

        }

        public static Termek Deserialize(string input) {
            string[] tmp = input.Split('●');
            Termek termek = new Termek(tmp[1], tmp[2], tmp[3], int.Parse(tmp[4]),tmp[5],decimal.Parse(tmp[6]), decimal.Parse(tmp[7]), decimal.Parse(tmp[8]),tmp[9],int.Parse(tmp[10]));
            termek.TermekAzonosito = int.Parse(tmp[0]);
            return termek;
        }

        public static decimal BruttoArSzamito(decimal nettoAr,decimal afa)
        {
            if (afa==0)
            {
                return nettoAr;
            }
            else
            {
                return (int)Math.Round(nettoAr * (afa / 100+1));
            }

        }
        public static decimal AktualiskeszletSzamito(decimal darabszam, decimal aktualiskeszlet)
        {
            if (aktualiskeszlet>0  && darabszam<=aktualiskeszlet)
            {
                return aktualiskeszlet - darabszam;
            }
            else
            {
                return 0;
            }
            

        }
    }
}

