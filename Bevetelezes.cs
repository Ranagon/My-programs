using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szakdolgozat3
{
    class Bevetelezes
    {
        decimal kiadas;
        int bevetId;
        int partnerId;

        DateTime bizonylatKelte;
        int megrendelesSzam;
        // MegrendelesSzam megrendelesSzam;



        BindingList<Termek> kiadottTermekek = new BindingList<Termek>();


        // BindingList<Termek> kiadottTermekek;
        public decimal Kiadas { get => kiadas; set => kiadas = value; }
        public DateTime BizonylatKelte { get => bizonylatKelte; set => bizonylatKelte = value; }
        public int BevetId { get => bevetId; set => bevetId = value; }
        public int PartnerId { get => partnerId; set => partnerId = value; }

        public int MegrendelesSzam { get => megrendelesSzam; set => megrendelesSzam = value; }


        // public MegrendelesSzam MegrendelesSzam { get => megrendelesSzam; set => megrendelesSzam = value; }






        // public BindingList<Termek> KiadottTermekek { get => kiadottTermekek; set => kiadottTermekek = value; }

        public Bevetelezes(int partnerId, DateTime bizonylatKelte, int megrendelesSzam, int kiadas)

        {
            PartnerId = partnerId;

            BizonylatKelte = bizonylatKelte;
            MegrendelesSzam = megrendelesSzam;
            Kiadas = kiadas;





        }

        public Bevetelezes(int bevetId, int partnerId, DateTime bizonylatKelte, int megrendelesSzam, int kiadas)

        {
            BevetId = bevetId;
            PartnerId = partnerId;

            BizonylatKelte = bizonylatKelte;
            MegrendelesSzam = megrendelesSzam;
            Kiadas = kiadas;


        }



        public override string ToString()
        {
            return $"{BevetId}-{BizonylatKelte}-{MegrendelesSzam}-{kiadas}";
        }

        public string SzallitoCSVFormatum(SzallitoLevel adat)
        {
            return $"";
        }

    }
}
