using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Szakdolgozat3
{
    public struct MegrendelesSzam
    {
        DateTime datum;
        int sorszam;
       

        public DateTime Datum { get => datum; set => datum = value; }
        public int Sorszam { get => sorszam; set => sorszam = value; }
        

        public override string ToString()
        {
            return $"{Datum}{Sorszam}";
        }
    }

        internal class SzallitoLevel
    {

        decimal bevetel;
        int szallitoId;
        int partnerId;
      
        DateTime bizonylatKelte;
        int megrendelesSzam;
       // MegrendelesSzam megrendelesSzam;
        


        //BindingList<Termek> kiadottTermekek = new BindingList<Termek>();


        // BindingList<Termek> kiadottTermekek;
        public decimal Bevetel { get => bevetel; set => bevetel = value; }
        public DateTime BizonylatKelte
        {
            get => bizonylatKelte;
            set
            {
               
                bizonylatKelte = value;
            }
        }
        public int SzallitoId { get => szallitoId; set => szallitoId = value; }
        public int PartnerId { get => partnerId; set => partnerId = value; }
    
        public int MegrendelesSzam
        {
            get => megrendelesSzam; set
            {
               
                    megrendelesSzam = value;
                
               
            }
        }
       

        // public MegrendelesSzam MegrendelesSzam { get => megrendelesSzam; set => megrendelesSzam = value; }






        // public BindingList<Termek> KiadottTermekek { get => kiadottTermekek; set => kiadottTermekek = value; }

        public SzallitoLevel(int partnerId,  DateTime bizonylatKelte, int megrendelesSzam, int bevetel)

        {
            PartnerId = partnerId;
           
            BizonylatKelte = bizonylatKelte;
            MegrendelesSzam = megrendelesSzam;
            Bevetel = bevetel;
            




        }

        public SzallitoLevel(int szallitoId,int partnerId,  DateTime bizonylatKelte, int megrendelesSzam, int bevetel) 
            
        {
            SzallitoId = szallitoId;
            PartnerId = partnerId;
            
            BizonylatKelte = bizonylatKelte;
            MegrendelesSzam = megrendelesSzam;
            Bevetel = bevetel;


        }

      

        public override string ToString()
        {
            return $"{SzallitoId}-{BizonylatKelte}-{MegrendelesSzam}-{Bevetel}";
        }

        public string SzallitoCSVFormatum(SzallitoLevel adat)
        {
            return $"";
        }
          

    }
}
