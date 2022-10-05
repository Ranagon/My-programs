using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Szakdolgozat3
{
    class BevettTermek
    {
        int bevetId;
        int termekId;
        int darabSzam;
        decimal beszerzesiAr;

        public int BevetId
        {
            get => bevetId;
            set
            {
                if (value > -1)
                {
                    bevetId = value;
                }
                else
                {
                    throw new ArgumentException(bevetId.ToString() + "Az érték csak pozitív egész szám lehet!");
                }
            }
        }
        public int TermekId
        {
            get => termekId;
            set
            {
                if (value > -1)
                {
                    termekId = value;
                }
                else
                {
                    throw new ArgumentException(termekId + "Az érték csak pozitív egész szám lehet!");
                }
            }
        }
        public int DarabSzam
        {
            get => darabSzam;
            set
            {
                if (value > -1)
                {
                    darabSzam = value;
                }
                else
                {
                    throw new ArgumentException(darabSzam + "Az érték csak pozitív egész szám lehet!");
                }
            }
        }
        public decimal BeszerzesiAr
        {
            get => beszerzesiAr;
            set
            {
                if (value > -1)
                {
                    beszerzesiAr = value;
                }
                else
                {
                    throw new ArgumentException(darabSzam + "Az érték csak pozitív egész szám lehet!");
                }
            }
        }


        public BevettTermek(int bevetId, int termekId, int darabSzam, decimal beszerzesiAr)
        {
            TermekId = termekId;
            DarabSzam = darabSzam;
            BevetId = bevetId;
            BeszerzesiAr = beszerzesiAr;
        }
        public static decimal ArSzamito(List<Termek> bruttoar, decimal kedvezmeny)//numericupdownból fog bevárni %-os adatot.
        {
            try
            {

                decimal osszeg = 0;

                if (bruttoar.Count > -1)
                {


                    for (int i = 0; i < bruttoar.Count; i++)
                    {
                        if (Math.Ceiling(bruttoar[i].Mennyiseg * bruttoar[i].BruttoAr * ((100 - kedvezmeny) / 100))+osszeg<decimal.MaxValue)
                        {
                            osszeg += Math.Ceiling(bruttoar[i].Mennyiseg * bruttoar[i].BruttoAr * ((100 - kedvezmeny) / 100));

                        }
                        else
                        {
                            MessageBox.Show("Irreálisan magas összeg", "Figyelem",MessageBoxButtons.OK,MessageBoxIcon.Warning );
                            return -1;
                        }
                       



                    }
                }
                return osszeg;
            }
            catch (Exception)
            {

                throw new Exception("Irreálisan magas összeg!");
            }
        }

        public Termek toTermek() {
            Termek tmp = ABKezelo.TermekKivalasztas(TermekId);
            //if (tmp != null) {
                tmp.Mennyiseg = DarabSzam;
                tmp.NettoAr = BeszerzesiAr;
            //}
            return tmp;
        }
    }
}
