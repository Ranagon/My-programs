using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace Szakdolgozat3 

{
    
    class ABKezelo {
		//internal static List<int> ideiglenesszallito=new List<int>();
        internal static int szallitoId;
       

        //static readonly string cs = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={Environment.CurrentDirectory}\Database1.mdf;Integrated Security=True";
        static readonly string cs = ConfigurationManager.ConnectionStrings["Adatbazis"].ConnectionString;
        const string cs2 = "datasource=127.0.0.1;port=3306;uid=root;database=adatbazis";
        //  internal static readonly string cs2 = $@"{Environment.CurrentDirectory}";  EZT MAJD VISSZA KELL RAKNI!!!

        #region Termékfelvitel

        public static void TermekFelvitel(Termek adat) //kell termék id a táblába, ami lehet cikkszám is.
        {
			using(SqlConnection kapcsolat = new SqlConnection(cs)) {
                kapcsolat.Open();
				using (SqlCommand Parancs = kapcsolat.CreateCommand()) {
                        Parancs.Connection = kapcsolat;
                        Parancs.Parameters.Clear();

                        Parancs.CommandText = " INSERT [TERMEKEK]  ([TermekKategoria],[Megnevezes],[CikkSzam],[Mennyiseg],[MennyisegEgysege],[NettoAr],[BruttoAr],[AfaKulcs],[NavSorszam],[MinimalisKeszlet],[Statusz]) " +
                            "OUTPUT INSERTED.TermekAzonosito VALUES(@termekKategoria,@megnevezes,@cikkszam,@mennyiseg,@mennyisegEgysege,@nettoAr,@bruttoAr,@afaKulcs," +
                            "@NavSorszam,@minimalisKeszlet,@statusz)";

                        Parancs.Parameters.AddWithValue("@termekKategoria", adat.TermekKategoria);
                        Parancs.Parameters.AddWithValue("@megnevezes", adat.Megnevezes);
                        Parancs.Parameters.AddWithValue("@cikkSzam", adat.Cikkszam);
                        Parancs.Parameters.AddWithValue("@mennyiseg", adat.Mennyiseg);
                        Parancs.Parameters.AddWithValue("@mennyisegEgysege", adat.MennyisegEgysege);
                        Parancs.Parameters.AddWithValue("@nettoAr", adat.NettoAr);
                        Parancs.Parameters.AddWithValue("@bruttoAr", adat.BruttoAr);
                        Parancs.Parameters.AddWithValue("@afaKulcs", adat.AfaKulcs);
                        Parancs.Parameters.AddWithValue("@NavSorszam", adat.NavSorszam);
                        Parancs.Parameters.AddWithValue("@minimalisKeszlet", adat.MinimalisKeszlet);
                        Parancs.Parameters.AddWithValue("@statusz",1);

                    adat.TermekAzonosito = (int)Parancs.ExecuteScalar();
				}
				kapcsolat.Close();
			}

        }
        #endregion
        #region Termékmódosítás
        public static void TermekModositas(Termek adat)
        {

            using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                kapcsolat.Open();
                using (SqlCommand Parancs = new SqlCommand())
                {
                    Parancs.Connection = kapcsolat;
                    Parancs.Parameters.Clear();
                    Parancs.CommandText = "UPDATE [Termekek] SET " +
                 "[TermekKategoria]=@termekKategoria,[Megnevezes]=@megnevezes,[Cikkszam]=@cikkSzam,[Mennyiseg]=@mennyiseg,[MennyisegEgysege]=@mennyisegEgysege,[NettoAr]=@nettoAr,[BruttoAr]=@bruttoAr,[AfaKulcs]=@afaKulcs,[NAVsorszam]=@NavSorszam,[minimalisKeszlet]=@MinimalisKeszlet " +
                         "WHERE [TermekAzonosito]=@termekAzonosito";
                    Parancs.Parameters.AddWithValue("@termekAzonosito", adat.TermekAzonosito);
                    Parancs.Parameters.AddWithValue("@termekKategoria", adat.TermekKategoria);
                    Parancs.Parameters.AddWithValue("@megnevezes", adat.Megnevezes);
                    Parancs.Parameters.AddWithValue("@cikkSzam", adat.Cikkszam);
                    Parancs.Parameters.AddWithValue("@mennyiseg", adat.Mennyiseg);
                    Parancs.Parameters.AddWithValue("@mennyisegEgysege", adat.MennyisegEgysege);
                    Parancs.Parameters.AddWithValue("@nettoAr", adat.NettoAr);
                    Parancs.Parameters.AddWithValue("@bruttoAr", adat.BruttoAr);
                    Parancs.Parameters.AddWithValue("@afaKulcs", adat.AfaKulcs);
                    Parancs.Parameters.AddWithValue("@NAVsorszam", adat.NavSorszam);
                    Parancs.Parameters.AddWithValue("@minimalisKeszlet", adat.MinimalisKeszlet);
                    Parancs.ExecuteNonQuery();
                }
                kapcsolat.Close();
            }
        }
        #endregion
        #region TermekLomtarba
        public static void TermekLomtarba(int adat) 
        {
            using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                kapcsolat.Open();
                using (SqlCommand Parancs = kapcsolat.CreateCommand())
                {
                    using (SqlTransaction Tranzakcio = kapcsolat.BeginTransaction())
                    { //tranzakció megkezdése
                        Parancs.Connection = kapcsolat;
                        Parancs.Transaction = Tranzakcio;
                        try
                        {

                            Parancs.Parameters.Clear();
                            Parancs.CommandText = "UPDATE[Termekek] SET [Statusz]=@statusz WHERE [TermekAzonosito]=@termekId";
                            Parancs.Parameters.AddWithValue("@termekId", adat);
                            Parancs.Parameters.AddWithValue("@statusz",0);

                            Parancs.ExecuteNonQuery();


                            Tranzakcio.Commit(); 
                        }
                        catch (Exception ex1)
                        {
                            MessageBox.Show(ex1.Message); 
                            try
                            {
                                Tranzakcio.Rollback(); 
                            }
                            catch (Exception ex2)
                            {
                                MessageBox.Show(ex2.Message);
                            }
                        }
                    }
                }
                kapcsolat.Close();
            }
        }
        #endregion
        #region TermekTorles
        public static void TermekTorles(int adat) 
        {
            using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                kapcsolat.Open();
                using (SqlCommand Parancs = kapcsolat.CreateCommand())
                {
                    using (SqlTransaction Tranzakcio = kapcsolat.BeginTransaction())
                    { 
                        Parancs.Connection = kapcsolat;
                        Parancs.Transaction = Tranzakcio;
                        try
                        {

                            Parancs.Parameters.Clear();
                            Parancs.CommandText = "DELETE FROM [Termekek] WHERE [TermekAzonosito]=@termekId";
                            Parancs.Parameters.AddWithValue("@termekId", adat);
                            Parancs.ExecuteNonQuery();


                            Tranzakcio.Commit(); 
                        }
                        catch (Exception ex1)
                        {
                            MessageBox.Show(ex1.Message); 
                            try
                            {
                                Tranzakcio.Rollback(); 
                            }
                            catch (Exception ex2)
                            {
                                MessageBox.Show(ex2.Message); 
                            }
                        }
                    }
                }
                kapcsolat.Close();
            }
        }
        #endregion
        #region SzallitoLevelFelvitel
        public static void SzallitoLevelFelvitel(SzallitoLevel szallito) 
        {
            using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                kapcsolat.Open();
                using (SqlCommand Parancs = kapcsolat.CreateCommand())
                {
                    Parancs.Connection = kapcsolat;
                    Parancs.Parameters.Clear();
                    Parancs.CommandText = " INSERT [SzallitoLevel]  ([PartnerId2],[BizonylatKelte],[BizonylatSzam],[Bevetel]) OUTPUT INSERTED.SzallitoId  VALUES(@partnerId2,@bizonylatkelte,@bizonylatSzam,@bevetel)";

                    Parancs.Parameters.AddWithValue("@partnerId2", szallito.PartnerId);

                    Parancs.Parameters.AddWithValue("@bizonylatkelte", szallito.BizonylatKelte);
                    Parancs.Parameters.AddWithValue("@bizonylatSzam", szallito.MegrendelesSzam);
                    Parancs.Parameters.AddWithValue("@bevetel", szallito.Bevetel);
                    szallito.SzallitoId = (int)Parancs.ExecuteScalar();

                }
                kapcsolat.Close();
            }

        }
        #endregion
        #region SzallitoLevelGarbageCollector
        public static void SzallitoLevelGarbageCollector() 
        {
            using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                kapcsolat.Open();
                using (SqlCommand Parancs = kapcsolat.CreateCommand())
                {
                    Parancs.Connection = kapcsolat;
                    Parancs.Parameters.Clear();
                    Parancs.CommandText =" DECLARE @length INT = (SELECT COUNT(*) FROM[Termekek])  " +
                        "DECLARE @cnt INT = 0;WHILE @cnt <@length BEGIN DELETE FROM[Termekek] WHERE [Statusz] = 0 " +
                        "AND (SELECT COUNT(*) FROM [KiadottTermek] WHERE[KiadottTermekID] = " +
                        "[TermekAzonosito]) = 0; SET @cnt = @cnt + 1; END" ;

                    Parancs.ExecuteNonQuery();

                }
                kapcsolat.Close();
            }

        }
        #endregion
       
        #region SzallitoLevelIDLekerdezes
        public static int SzallitoLevelIDLekerdezes() 
        {
            int azonosito = -1;
            using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                try
                {
                    kapcsolat.Open();
                    using (SqlCommand Parancs = kapcsolat.CreateCommand())
                    {
                        Parancs.Connection = kapcsolat;
                        Parancs.Parameters.Clear();
                        Parancs.CommandText = "SELECT max([SzallitoId]) FROM [SzallitoLevel]"; 
                        using (SqlDataReader reader = Parancs.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                
                                azonosito = reader.GetInt32(0);
                            }
                            reader.Close();
                        }
                    }
                    kapcsolat.Close();
                }
                catch (Exception)
                {
                    kapcsolat.Close();
                   
                    azonosito = 1;
                    return azonosito;
                }
            }
            return azonosito;
        }
        #endregion
        #region KiadottTermekFelvitel
        public static void KiadottTermekFelvitel(KiadottTermek termekek) 
        {
            using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                kapcsolat.Open();
                using (SqlCommand Parancs = kapcsolat.CreateCommand())
                {
                    Parancs.Connection = kapcsolat;
                    Parancs.Parameters.Clear();
                    Parancs.CommandText = "INSERT [KiadottTermek] ([SzallitoID],[KiadottTermekId],[DarabSzam],[EladasiAr]) VALUES(@szallitoID,@kiadottTermekId,@darabSzam,@eladasiAr)";
                    Parancs.Parameters.AddWithValue("@szallitoID", termekek.SzallitoID);
                    Parancs.Parameters.AddWithValue("@kiadottTermekId", termekek.TermekId);
                    Parancs.Parameters.AddWithValue("@darabSzam", termekek.DarabSzam);
                    Parancs.Parameters.AddWithValue("@eladasiAr", termekek.EladasiAr);
                    Parancs.ExecuteNonQuery();                                
                }
                kapcsolat.Close();
            }

        }
        #endregion
        #region KiadottTermekTorles
        public static void KiadottTermekTorles(int szallitoId) 
        {
            using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                kapcsolat.Open();
                using (SqlCommand Parancs = kapcsolat.CreateCommand())
                {
                    Parancs.Connection = kapcsolat;
                    Parancs.Parameters.Clear();
                    Parancs.CommandText = "DELETE FROM [KiadottTermek] WHERE [SzallitoID]=@szallitoID";
                    Parancs.Parameters.AddWithValue("@szallitoID", szallitoId);
                   
                    Parancs.ExecuteNonQuery();
                }
                kapcsolat.Close();
            }

        }
        #endregion
        #region SzallitoLevelModositas
        public static void SzallitoLevelModositas(SzallitoLevel adat)
        {

            using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                kapcsolat.Open();
                using (SqlCommand Parancs = new SqlCommand())
                {
                    Parancs.Connection = kapcsolat;
                    Parancs.Parameters.Clear();
                    Parancs.CommandText = "UPDATE [SzallitoLevel] SET " +
                 "[PartnerId2]=@partnerId2,[BizonylatKelte] =@bizonylatkelte +[BizonylatSzam]=@bizonylatSzam"+
                         "WHERE [SzallitoId]=@szallitoId";

                    Parancs.Parameters.AddWithValue("@szallitoId", adat.MegrendelesSzam);
                    Parancs.Parameters.AddWithValue("@partnerId2", adat.PartnerId);
                 
                    Parancs.Parameters.AddWithValue("@bizonylatkelte", adat.BizonylatKelte);
                    Parancs.Parameters.AddWithValue("@bizonylatSzam", adat.MegrendelesSzam);
                    
                    Parancs.ExecuteNonQuery();
                }
                kapcsolat.Close();
            }
        }
        #endregion
        #region SzallitoLevelTorles
        public static void SzallitoLevelTorles(int adat)
        {

            using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                kapcsolat.Open();
                using (SqlCommand Parancs = new SqlCommand())
                {
                    Parancs.Connection = kapcsolat;
                    Parancs.Parameters.Clear();
                    Parancs.CommandText = "DELETE FROM [SzallitoLevel] WHERE [SzallitoId]=@szallitoId";

                    Parancs.Parameters.AddWithValue("@szallitoId", adat);
                    

                    Parancs.ExecuteNonQuery();
                }
                kapcsolat.Close();
            }
        }
        #endregion
        #region detachDB
        public static void detachDB()
        {
            using (SqlConnection sqlDatabaseConnection = new SqlConnection(cs))
            {
                try
                {
                    sqlDatabaseConnection.Open();
                    string commandString = $"ALTER DATABASE [{sqlDatabaseConnection.Database}] SET OFFLINE WITH ROLLBACK IMMEDIATE ALTER DATABASE [{sqlDatabaseConnection.Database}] SET SINGLE_USER EXEC sp_detach_db [{sqlDatabaseConnection.Database}]";
                    using (SqlCommand sqlDatabaseCommand = new SqlCommand(commandString, sqlDatabaseConnection))
                    {
                        sqlDatabaseCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        #endregion
        # region BevetBizonylatFelvitel
        public static void BevetBizonylatFelvitel(Bevetelezes bevetBizonylat) 
        {
            using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                kapcsolat.Open();
                using (SqlCommand Parancs = kapcsolat.CreateCommand())
                {
                    Parancs.Connection = kapcsolat;
                    Parancs.Parameters.Clear();
                    Parancs.CommandText = " INSERT [Bevetelezesek] ([PartnerId3],[BizonylatKelte2],[BizonylatSzam2],[Kiadas]) OUTPUT INSERTED.BevetId  VALUES(@partnerId3,@bizonylatkelte2,@bizonylatSzam2,@kiadas)";

                    Parancs.Parameters.AddWithValue("@partnerId3", bevetBizonylat.PartnerId);

                    Parancs.Parameters.AddWithValue("@bizonylatkelte2", bevetBizonylat.BizonylatKelte);
                    Parancs.Parameters.AddWithValue("@bizonylatSzam2", bevetBizonylat.MegrendelesSzam);
                    Parancs.Parameters.AddWithValue("@kiadas", bevetBizonylat.Kiadas);
                    bevetBizonylat.BevetId = (int)Parancs.ExecuteScalar();

                }
                kapcsolat.Close();
            }

        }
        #endregion
        #region BevetBizonylatModositas
        public static void BevetBizonylatModositas(Bevetelezes bevetBizonylat)
        {

            using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                kapcsolat.Open();
                using (SqlCommand Parancs = new SqlCommand())
                {
                    Parancs.Connection = kapcsolat;
                    Parancs.Parameters.Clear();
                    Parancs.CommandText = "UPDATE [Bevetelezesek]  SET " +
                 "[PartnerId3]=@partnerId3,[BizonylatKelte2] =@bizonylatkelte2 ,[BizonylatSzam2]=@bizonylatSzam2,[Kiadas]=@kiadas" +
                         "WHERE [BevetId]=@bevetId";

                    Parancs.Parameters.AddWithValue("@bevetId", bevetBizonylat.MegrendelesSzam);
                    Parancs.Parameters.AddWithValue("@partnerId3", bevetBizonylat.PartnerId);
                                                                                   
                    Parancs.Parameters.AddWithValue("@bizonylatkelte2", bevetBizonylat.BizonylatKelte);
                    Parancs.Parameters.AddWithValue("@bizonylatSzam2", bevetBizonylat.MegrendelesSzam);
                    Parancs.Parameters.AddWithValue("@kiadas", bevetBizonylat.Kiadas);

                    Parancs.ExecuteNonQuery();
                }
                kapcsolat.Close();
            }
        }
        #endregion
        #region BevetBizonylatTorles
        public static void BevetBizonylatTorles(int adat)
        {

            using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                kapcsolat.Open();
                using (SqlCommand Parancs = new SqlCommand())
                {
                    Parancs.Connection = kapcsolat;
                    Parancs.Parameters.Clear();
                    Parancs.CommandText = "DELETE FROM [Bevetelezesek]  WHERE [BevetId]=@bevetId";

                    Parancs.Parameters.AddWithValue("@bevetId", adat);


                    Parancs.ExecuteNonQuery();
                }
                kapcsolat.Close();
            }
        }
        #endregion
        #region BevetTermekFelvitel
        public static void BevetTermekFelvitel(BevettTermek termek) 
        {
            using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                kapcsolat.Open();
                using (SqlCommand Parancs = kapcsolat.CreateCommand())
                {
                    Parancs.Connection = kapcsolat;
                    Parancs.Parameters.Clear();
                    Parancs.CommandText = "INSERT [BevettTermek]  ([BevetId],[BevettTermekId],[DarabSzam2],[BeszerzesiAr]) VALUES(@bevetId,@bevettTermekId,@darabSzam2,@beszerzesiAr)";
                    Parancs.Parameters.AddWithValue("@bevetId", termek.BevetId);
                    Parancs.Parameters.AddWithValue("@bevettTermekId", termek.TermekId);
                    Parancs.Parameters.AddWithValue("@darabSzam2", termek.DarabSzam);
                    Parancs.Parameters.AddWithValue("@beszerzesiAr", termek.BeszerzesiAr);
                    Parancs.ExecuteNonQuery();
                }
                kapcsolat.Close();
            }

        }
        #endregion
        #region BevetTermekTorles
        public static void BevetTermekTorles(int bevettId) 
        {
            using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                kapcsolat.Open();
                using (SqlCommand Parancs = kapcsolat.CreateCommand())
                {
                    Parancs.Connection = kapcsolat;
                    Parancs.Parameters.Clear();
                    Parancs.CommandText = "DELETE FROM [BevettTermek] WHERE [BevetId]=@bevettId ";
                    Parancs.Parameters.AddWithValue("@bevettId", bevettId);

                    Parancs.ExecuteNonQuery();
                }
                kapcsolat.Close();
            }

        }
        #endregion
        #region BevetBizonylatIDLekerdezes
        public static int BevetBizonylatIDLekerdezes() 
        {
            int azonosito = -1;
            using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                try
                {
                    kapcsolat.Open();
                    using (SqlCommand Parancs = kapcsolat.CreateCommand())
                    {
                        Parancs.Connection = kapcsolat;
                        Parancs.Parameters.Clear();
                        Parancs.CommandText = "SELECT max([BevetId]) FROM [Bevetelezesek]";
                        using (SqlDataReader reader = Parancs.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                
                                azonosito = reader.GetInt32(0);
                            }
                            reader.Close();
                        }
                    }
                    kapcsolat.Close();
                }
                catch (Exception)
                {
                    kapcsolat.Close();
                    azonosito = 1;
                    return azonosito;

                }
            }
            return azonosito;
        }
        #endregion
        #region BevetBizonylatKivalasztasIdAlapjan
        public static Bevetelezes BevetBizonylatKivalasztasIdAlapjan(int adat)
        {
            Bevetelezes tmp = null;
           
            using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                kapcsolat.Open();
                using (SqlCommand Parancs = new SqlCommand())
                {

                    Parancs.Connection = kapcsolat;
                    Parancs.Parameters.Clear();
                    Parancs.CommandText = "SELECT * FROM [Bevetelezesek]  WHERE [BevetId]= @bevetId";
                    Parancs.Parameters.AddWithValue("@bevetId", adat);
                    using (SqlDataReader reader = Parancs.ExecuteReader())
                    {
                        
                        while (reader.Read())
                        {



                            tmp = new Bevetelezes(reader.GetInt32(0), reader.GetInt32(1), reader.GetDateTime(2), reader.GetInt32(3), reader.GetInt32(4));

                        }
                        reader.Close();
                    }
                }
                kapcsolat.Close();
            }
         
            return tmp;
        }
        #endregion
        #region BevetKivalasztasPartnerAlapjan
        public static Bevetelezes BevetKivalasztasPartnerAlapjan(int adat)
        {
            Bevetelezes tmp = null;
         
            using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                kapcsolat.Open();
                using (SqlCommand Parancs = new SqlCommand())
                {

                    Parancs.Connection = kapcsolat;
                    Parancs.Parameters.Clear();
                    Parancs.CommandText = "SELECT * FROM [Bevetelezesek]  WHERE [PartnerId3]= @partnerId";
                    Parancs.Parameters.AddWithValue("@partnerId", adat);
                    using (SqlDataReader reader = Parancs.ExecuteReader())
                    {
                        
                        while (reader.Read())
                        {



                            tmp = new Bevetelezes(reader.GetInt32(1), reader.GetDateTime(2), reader.GetInt32(3), reader.GetInt32(4));

                        }
                        reader.Close();
                    }
                }
                kapcsolat.Close();
            }
            
            return tmp;
        }
        #endregion
        #region BevetKivalasztasBizonylatSzamAlapjan
        public static Bevetelezes BevetKivalasztasBizonylatSzamAlapjan(int adat)
        {
            Bevetelezes tmp = null;
           
            using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                kapcsolat.Open();
                using (SqlCommand Parancs = new SqlCommand())
                {

                    Parancs.Connection = kapcsolat;
                    Parancs.Parameters.Clear();
                    Parancs.CommandText = "SELECT * FROM [Bevetelezesek]  WHERE [BizonylatSzam2]= @bizonylatszam";
                    Parancs.Parameters.AddWithValue("@bizonylatszam", adat);
                    using (SqlDataReader reader = Parancs.ExecuteReader())
                    {
                       
                        while (reader.Read())
                        {



                            tmp = new Bevetelezes(reader.GetInt32(1), reader.GetDateTime(2), reader.GetInt32(3), reader.GetInt32(4));

                        }
                        reader.Close();
                    }
                }
                kapcsolat.Close();
            }
           
            return tmp;
        }
        #endregion
        #region BevetBizonylatGarbageCollector
        public static void BevetBizonylatGarbageCollector() //kell termék id a táblába, ami lehet cikkszám is.
        {
            using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                kapcsolat.Open();
                using (SqlCommand Parancs = kapcsolat.CreateCommand())
                {
                    Parancs.Connection = kapcsolat;
                    Parancs.Parameters.Clear();
                    Parancs.CommandText = " DECLARE @length INT = (SELECT COUNT(*) FROM[Termekek])  " +
                        "DECLARE @cnt INT = 0;WHILE @cnt <@length BEGIN DELETE FROM[Termekek] WHERE [Statusz] = 0 " +
                        "AND (SELECT COUNT(*) FROM [BevettTermek]  WHERE [BevettTermekId]  = " +
                        "[TermekAzonosito]) = 0; SET @cnt = @cnt + 1; END";

                    Parancs.ExecuteNonQuery();

                }
                kapcsolat.Close();
            }

        }
        #endregion
        #region PartnerFelvitel
        public static void PartnerFelvitel(Partner adat) 
        {
			using (SqlConnection kapcsolat = new SqlConnection(cs)) {
                kapcsolat.Open();
				using (SqlCommand Parancs = kapcsolat.CreateCommand()) {
                    using (SqlTransaction Tranzakcio = kapcsolat.BeginTransaction()) { 
                        Parancs.Connection = kapcsolat;
                        Parancs.Transaction = Tranzakcio;
                        try {

                            Parancs.Parameters.Clear();
                            Parancs.CommandText = " INSERT [Partner]  ([AdoSzam],[EuadoSzam],[TelefonSzam],[Fax],[Email],[WebCim],[BankSzamlaSzam],[PartnerCsoport],[FizetesiMod],[Kedvezmeny],[SzamlaMegjegyzes],[PartnerMegjegyzes]) " +
                                "OUTPUT INSERTED.PartnerId VALUES(@adoSzam,@EuadoSzam,@telefonSzam,@fax,@email,@webCim,@bankszamlaSzam,@partnerCsoport," +
                                "@fizetesiMod,@kedvezmeny,@szamlaMegjegyzes,@partnerMegjegyzes)";

                            Parancs.Parameters.AddWithValue("@adoSzam", adat.AdoSzam);
                            Parancs.Parameters.AddWithValue("@EuadoSzam", adat.EUAdoszam1);               
                            Parancs.Parameters.AddWithValue("@telefonSzam", adat.TelefonSzam);
                            Parancs.Parameters.AddWithValue("@fax", adat.Fax);
                            Parancs.Parameters.AddWithValue("@email", adat.Email);
                            Parancs.Parameters.AddWithValue("@webCim", adat.WebCim);
                            Parancs.Parameters.AddWithValue("@bankSzamlaSzam", adat.BankSzamlaSzam);
                            Parancs.Parameters.AddWithValue("@partnerCsoport", adat.PartnerCsoport);
                            Parancs.Parameters.AddWithValue("@fizetesiMod", adat.FizetesiMod);
                            Parancs.Parameters.AddWithValue("@kedvezmeny", adat.Kedvezmeny);
                            Parancs.Parameters.AddWithValue("@szamlaMegjegyzes", adat.SzamlaMegjegyzes);
                            Parancs.Parameters.AddWithValue("@partnerMegjegyzes", adat.PartnerMegjegyzes);
                            adat.PartnerAzonosito = (int)Parancs.ExecuteScalar();

                            Parancs.Parameters.Clear();

                            Parancs.CommandText = " INSERT [Cim]  (  [Orszag],[Irsz],[Telepules],[Kozterulet],[KozteruletJelleg],[HazSzam],[EmeletAjto],[PostaOrszag], [PostaIrsz],[PostaTelepules],[PostaKozterulet],[PostaKozteruletJelleg],[PostaHazSzam],[PostaEmeletAjto])" +
                                "OUTPUT INSERTED.CimId VALUES(@orszag,@irsz,@telepules,@kozterulet,@kozterjelleg,@hazszam,@emeletajto,@postaorszag,@postairsz,@postatelepules,@postakozterulet,@postakozterjelleg,@postahazszam,@postaemeletajto)";


                            Parancs.Parameters.AddWithValue("@orszag", adat.Orszag);
                            Parancs.Parameters.AddWithValue("@irsz", adat.Irsz);
                            Parancs.Parameters.AddWithValue("@telepules", adat.Telepules);
                            Parancs.Parameters.AddWithValue("@kozterulet", adat.Kozterulet);
                            Parancs.Parameters.AddWithValue("@kozterjelleg", adat.KozteruletJelleg);
                            Parancs.Parameters.AddWithValue("@hazszam", adat.HazSzam);
                            Parancs.Parameters.AddWithValue("@emeletajto", adat.EmeletAjto);
                            Parancs.Parameters.AddWithValue("@postaorszag", adat.PostaOrszag);
                            Parancs.Parameters.AddWithValue("@postairsz", adat.PostaIrsz);
                            Parancs.Parameters.AddWithValue("@postatelepules", adat.PostaTelepules);
                            Parancs.Parameters.AddWithValue("@postakozterulet", adat.PostaKozterulet);
                            Parancs.Parameters.AddWithValue("@postakozterjelleg", adat.PostaKozteruletJelleg);
                            Parancs.Parameters.AddWithValue("@postahazszam", adat.PostaHazSzam);
                            Parancs.Parameters.AddWithValue("@postaemeletajto", adat.PostaEmeletAjto);
                            adat.CimAzonosito = (int)Parancs.ExecuteScalar();
    


                            Parancs.Parameters.Clear();
                            Parancs.CommandText = " INSERT [Nev]  ([Titulus],[VezetekNev],[KeresztNev],[UtoNev],[CegNev],[PartnerId])" +
                                "OUTPUT INSERTED.NevId VALUES(@titulus,@vezetekNev,@keresztNev,@utoNev,@cegNev,@partnerID)";
                            Parancs.Parameters.AddWithValue("@titulus", adat.Titulus);
                            Parancs.Parameters.AddWithValue("@vezetekNev", adat.VezetekNev);
                            Parancs.Parameters.AddWithValue("@keresztNev", adat.KeresztNev);
                            Parancs.Parameters.AddWithValue("@utoNev", adat.UtoNev);
                            Parancs.Parameters.AddWithValue("@cegNev", adat.CegNev);
                            Parancs.Parameters.AddWithValue("@partnerID", adat.PartnerAzonosito);
                            adat.NevAzonosito = (int)Parancs.ExecuteScalar();


                            
                            Tranzakcio.Commit(); 
                        } catch (Exception ex1) {
                            MessageBox.Show(ex1.Message); 
                            try {
                                Tranzakcio.Rollback(); 
                            } catch (Exception ex2) {
                                MessageBox.Show(ex2.Message); 
                            }
                        }
                    }
				}
				kapcsolat.Close();
			}
        }
        #endregion
        //#region PartnerModositas
        public static void PartnerModositas(Partner adat) 
        {
           
            using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                kapcsolat.Open();
                using (SqlCommand Parancs = kapcsolat.CreateCommand())
                {
                    using (SqlTransaction Tranzakcio = kapcsolat.BeginTransaction())
                    { 
                        Parancs.Connection = kapcsolat;
                        Parancs.Transaction = Tranzakcio;
                        try
                        {

                            Parancs.Parameters.Clear();
                            Parancs.CommandText = " UPDATE [Partner] SET [AdoSzam]=@adoSzam,[EuadoSzam]=@EuadoSzam," +
                                "[TelefonSzam]=@telefonSzam,[Fax]=@fax,[Email]=@email,[WebCim]=@webCim,[BankSzamlaSzam]=@bankSzamlaSzam,[PartnerCsoport]=@partnerCsoport," +
                                "[FizetesiMod]=@fizetesiMod,[Kedvezmeny]=@kedvezmeny,[SzamlaMegjegyzes]=@szamlaMegjegyzes,[PartnerMegjegyzes]=@partnerMegjegyzes " +                 
                                "WHERE [PartnerID]=@partnerId";
                            Parancs.Parameters.AddWithValue("@partnerId", adat.PartnerAzonosito);
                            Parancs.Parameters.AddWithValue("@adoSzam", adat.AdoSzam);
                            Parancs.Parameters.AddWithValue("@EuadoSzam", adat.EUAdoszam1);                          
                            Parancs.Parameters.AddWithValue("@telefonSzam", adat.TelefonSzam);
                            Parancs.Parameters.AddWithValue("@fax", adat.Fax);
                            Parancs.Parameters.AddWithValue("@email", adat.Email);
                            Parancs.Parameters.AddWithValue("@webCim", adat.WebCim);
                            Parancs.Parameters.AddWithValue("@bankSzamlaSzam", adat.BankSzamlaSzam);
                            Parancs.Parameters.AddWithValue("@partnerCsoport", adat.PartnerCsoport);
                            Parancs.Parameters.AddWithValue("@fizetesiMod", adat.FizetesiMod);
                            Parancs.Parameters.AddWithValue("@kedvezmeny", adat.Kedvezmeny);
                            Parancs.Parameters.AddWithValue("@szamlaMegjegyzes", adat.SzamlaMegjegyzes);
                            Parancs.Parameters.AddWithValue("@partnerMegjegyzes", adat.PartnerMegjegyzes);
                            Parancs.ExecuteNonQuery();

                            Parancs.Parameters.Clear();

                            Parancs.CommandText = " UPDATE [Cim]  SET  [Orszag]=@orszag,[Irsz]=@irsz,[Telepules]=@telepules,[Kozterulet]=@kozterulet,[KozteruletJelleg]=@kozterjelleg,[HazSzam]=@hazszam,[EmeletAjto]=@emeletajto," +
                                "[PostaOrszag]=@postaorszag,[PostaIrsz]=@postairsz,[PostaTelepules]=@postatelepules,[PostaKozterulet]=@postakozterulet,[PostaKozteruletJelleg]=@postakozterjelleg,[PostaHazSzam]=@postahazszam,[PostaEmeletAjto]=@postaemeletajto" +
                                " WHERE [CimId]=@cimId";

                            Parancs.Parameters.AddWithValue("@cimId", adat.PartnerAzonosito);
                            Parancs.Parameters.AddWithValue("@orszag", adat.Orszag);
                            Parancs.Parameters.AddWithValue("@irsz", adat.Irsz);
                            Parancs.Parameters.AddWithValue("@telepules", adat.Telepules);
                            Parancs.Parameters.AddWithValue("@kozterulet", adat.Kozterulet);
                            Parancs.Parameters.AddWithValue("@kozterjelleg", adat.KozteruletJelleg);
                            Parancs.Parameters.AddWithValue("@hazszam", adat.HazSzam);
                            Parancs.Parameters.AddWithValue("@emeletajto", adat.EmeletAjto);
                            Parancs.Parameters.AddWithValue("@postaorszag", adat.PostaOrszag);
                            Parancs.Parameters.AddWithValue("@postairsz", adat.PostaIrsz);
                            Parancs.Parameters.AddWithValue("@postatelepules", adat.PostaTelepules);
                            Parancs.Parameters.AddWithValue("@postakozterulet", adat.PostaKozterulet);
                            Parancs.Parameters.AddWithValue("@postakozterjelleg", adat.PostaKozteruletJelleg);
                            Parancs.Parameters.AddWithValue("@postahazszam", adat.PostaHazSzam);
                            Parancs.Parameters.AddWithValue("@postaemeletajto", adat.PostaEmeletAjto);
                            Parancs.ExecuteNonQuery();



                            Parancs.Parameters.Clear();
                            Parancs.CommandText = " UPDATE [Nev] SET  [Titulus]=@titulus,[VezetekNev]=@vezetekNev,[KeresztNev]=@keresztNev,[UtoNev]=@utoNev,[CegNev]=@cegNev,[PartnerId]=@partnerID " +
                                "WHERE [PartnerId]=@partnerID";
                            Parancs.Parameters.AddWithValue("@titulus", adat.Titulus);
                            Parancs.Parameters.AddWithValue("@vezetekNev", adat.VezetekNev);
                            Parancs.Parameters.AddWithValue("@keresztNev", adat.KeresztNev);
                            Parancs.Parameters.AddWithValue("@utoNev", adat.UtoNev);
                            Parancs.Parameters.AddWithValue("@cegNev", adat.CegNev);
                            Parancs.Parameters.AddWithValue("@partnerID", adat.PartnerAzonosito);
                            Parancs.ExecuteNonQuery();



                            Tranzakcio.Commit(); 
                        }
                        catch (Exception ex1)
                        {
                            MessageBox.Show(ex1.Message); 
                            try
                            {
                                Tranzakcio.Rollback(); 
                            }
                            catch (Exception ex2)
                            {
                                MessageBox.Show(ex2.Message); 
                            }
                        }
                    }
                }
                kapcsolat.Close();
            }
        }
      
        #region PartnerTorles
        public static void PartnerTorles(int adat1,string adat2) 
        {
            using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                kapcsolat.Open();
                using (SqlCommand Parancs = kapcsolat.CreateCommand())
                {
                    using (SqlTransaction Tranzakcio = kapcsolat.BeginTransaction())
                    { 
                        Parancs.Connection = kapcsolat;
                        Parancs.Transaction = Tranzakcio;
                        try
                        {

                            Parancs.Parameters.Clear();
                            Parancs.CommandText = "DELETE FROM [Nev] WHERE [NevId]=@partnerId";
                            Parancs.Parameters.AddWithValue("@partnerId", adat1);
                            Parancs.Parameters.AddWithValue("@adoszam", adat2);
                            Parancs.ExecuteNonQuery();

                          

                            Parancs.CommandText = "DELETE FROM [Cim] WHERE [CimId]=@partnerId";


                            Parancs.ExecuteNonQuery();



                          
                            Parancs.CommandText = "DELETE FROM [Partner] WHERE [AdoSzam]=@adoszam";
                           
                            Parancs.ExecuteNonQuery();

                            Parancs.CommandText = "DELETE FROM [SzallitoLevel] WHERE [PartnerId2]=@partnerId";





                            Tranzakcio.Commit(); 
                        }
                        catch (Exception ex1)
                        {
                            MessageBox.Show(ex1.Message); 
                            try
                            {
                                Tranzakcio.Rollback(); 
                            }
                            catch (Exception ex2)
                            {
                                MessageBox.Show(ex2.Message); 
                            }
                        }
                    }
                }
                kapcsolat.Close();
            }
        }
        #endregion
        #region AdatbazisTorles
        public static void AdatbazisTorles() 
        {
            using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                kapcsolat.Open();
                using (SqlCommand Parancs = kapcsolat.CreateCommand())
                {
                    using (SqlTransaction Tranzakcio = kapcsolat.BeginTransaction())
                    { 
                        Parancs.Connection = kapcsolat;
                        Parancs.Transaction = Tranzakcio;
                        try
                        {
                            Parancs.Parameters.Clear();
                            Parancs.CommandText = "DELETE FROM [AfaKategoriak]; DBCC CHECKIDENT ([AfaKategoriak], RESEED, 0) ";
                            Parancs.ExecuteNonQuery();
                            Parancs.Parameters.Clear();
                            Parancs.CommandText = "DELETE FROM [Bevetelezesek]; DBCC CHECKIDENT ([Bevetelezesek], RESEED, 0)";
                            Parancs.ExecuteNonQuery();
                            Parancs.Parameters.Clear();
                            Parancs.CommandText = "DELETE FROM [BevettTermek]; DBCC CHECKIDENT ([BevettTermek], RESEED, 0) ";
                            Parancs.ExecuteNonQuery();
                            Parancs.Parameters.Clear();
                            Parancs.CommandText = "DELETE FROM [Cim]; DBCC CHECKIDENT ([Cim], RESEED, 0)";
                            Parancs.ExecuteNonQuery();
                            Parancs.Parameters.Clear();
                            Parancs.CommandText = "DELETE FROM [FizetesiMod] ; DBCC CHECKIDENT ([FizetesiMod], RESEED, 0)";
                            Parancs.ExecuteNonQuery();
                            Parancs.Parameters.Clear();
                            Parancs.CommandText = "DELETE FROM [KiadottTermek]; DBCC CHECKIDENT ([KiadottTermek], RESEED, 0)";
                            Parancs.ExecuteNonQuery();
                            Parancs.Parameters.Clear();
                            Parancs.CommandText = "DELETE FROM [Nev]; DBCC CHECKIDENT ([Nev], RESEED, 0)";                       
                            Parancs.ExecuteNonQuery();
                            Parancs.Parameters.Clear();
                            Parancs.CommandText = "DELETE FROM [Partner]; DBCC CHECKIDENT ([Partner], RESEED, 0)";
                            Parancs.ExecuteNonQuery();
                            Parancs.Parameters.Clear();
                            Parancs.CommandText = "DELETE FROM [PartnerBesorolas]; DBCC CHECKIDENT ([PartnerBesorolas], RESEED, 0)";
                            Parancs.ExecuteNonQuery();
                            Parancs.Parameters.Clear();
                            Parancs.CommandText = "DELETE FROM [SzallitoLevel]; DBCC CHECKIDENT ([SzallitoLevel], RESEED, 0)";
                            Parancs.ExecuteNonQuery();
                            Parancs.Parameters.Clear();
                            Parancs.CommandText = "DELETE FROM [Termekek]; DBCC CHECKIDENT ([Termekek], RESEED, 0) ";
                            Parancs.ExecuteNonQuery();
                            Parancs.Parameters.Clear();
                            Parancs.CommandText = "DELETE FROM [TermekKategoriak]; DBCC CHECKIDENT ([TermekKategoriak], RESEED, 0) ";
                            Parancs.ExecuteNonQuery();
                            Parancs.Parameters.Clear();
                            Parancs.CommandText = "DELETE FROM [TermekMennyisegEgyseg]; DBCC CHECKIDENT ([TermekMennyisegEgyseg], RESEED, 0) ";
                            Parancs.ExecuteNonQuery();
                            
                           
                            Tranzakcio.Commit(); 
                        }
                        catch (Exception ex1)
                        {
                            MessageBox.Show(ex1.Message); 
                            try
                            {
                                Tranzakcio.Rollback(); 
                            }
                            catch (Exception ex2)
                            {
                                MessageBox.Show(ex2.Message); 
                            }
                        }
                    }
                }
                kapcsolat.Close();
            }
        }
        #endregion
        #region PartnerKivalasztasIdAlapjan
        public static Partner PartnerKivalasztasIdAlapjan(int id)
        {
            Partner tmp = null;

            
                using (SqlConnection kapcsolat = new SqlConnection(cs))
                {
                try
                {
                    kapcsolat.Open();
                    using (SqlCommand Parancs = new SqlCommand())
                    {
                        Parancs.Connection = kapcsolat;
                        Parancs.Parameters.Clear();
                        Parancs.CommandText = "SELECT * FROM [Nev]  LEFT JOIN [Cim] ON [Cim].[CimId]=[Nev].NevId LEFT JOIN [Partner] ON [Partner].PartnerId=[Nev].PartnerId WHERE [Partner].PartnerId=@partnerId;"; // JAJJ, DEJÓ, MŰKÖDIK, csak rosszul!!
                        Parancs.Parameters.AddWithValue("@partnerId", id);
                        using (SqlDataReader reader = Parancs.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                tmp = new Partner(reader.GetInt32(6), reader.GetString(23), reader.GetString(24), reader.GetString(25),
                                  reader.GetString(26), reader.GetString(27), reader.GetString(28), reader.GetString(29), reader.GetString(30), reader.GetString(31),
                                  Convert.ToDecimal(reader.GetInt32(32)), reader.GetString(33), reader.GetString(34), reader.GetString(1), reader.GetString(2), reader.GetString(3),
                                  reader.GetString(4), reader.GetString(5), reader.GetString(8), reader.GetString(9).ToString(), reader.GetString(10), reader.GetString(11),
                                 (KozteruletJelleg)int.Parse(reader.GetString(12)), reader.GetString(13).ToString(), reader.GetString(14), reader.GetString(15), reader.GetString(16).ToString(), reader.GetString(17), reader.GetString(18),
                                 (KozteruletJelleg)int.Parse(reader.GetString(19)), reader.GetString(20).ToString(), reader.GetString(21));



                            }

                            reader.Close();
                        }
                    }

                    kapcsolat.Close();

                }
                catch (Exception ex)
                {
                    kapcsolat.Close();
                    throw new Exception(ex.Message + "Sikertelen felolvasás!", ex);
                }
                return tmp;
            }



        }
        #endregion
        #region PartnerKivalasztasAdoszamAlapjan
        public static Partner PartnerKivalasztasAdoszamAlapjan(string adat)
        {
            Partner tmp = null;
           
            using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                kapcsolat.Open();
                using (SqlCommand Parancs = new SqlCommand())
                {

                    Parancs.Connection = kapcsolat;
                    Parancs.Parameters.Clear();
                    Parancs.CommandText = "SELECT * FROM[Nev] LEFT JOIN[Cim] ON[Cim].[CimId] =[Nev].[NevId] LEFT JOIN[Partner] ON[Partner].[PartnerId] =[Nev].[PartnerId] WHERE[AdoSzam] = @adoSzam";
                    Parancs.Parameters.AddWithValue("@adoSzam", adat);
                    using (SqlDataReader reader = Parancs.ExecuteReader())
                    {
                        
                        while (reader.Read())
                        {
                            tmp = new Partner(reader.GetInt32(6), reader.GetString(23), reader.GetString(24), reader.GetString(25),
                                  reader.GetString(26), reader.GetString(27), reader.GetString(28), reader.GetString(29), reader.GetString(30), reader.GetString(31),
                                  Convert.ToDecimal(reader.GetInt32(32)), reader.GetString(33), reader.GetString(34), reader.GetString(1), reader.GetString(2), reader.GetString(3),
                                  reader.GetString(4), reader.GetString(5), reader.GetString(8), reader.GetString(9).ToString(), reader.GetString(10), reader.GetString(11),
                                 (KozteruletJelleg)int.Parse(reader.GetString(12)), reader.GetString(13).ToString(), reader.GetString(14), reader.GetString(15), reader.GetString(16).ToString(), reader.GetString(17), reader.GetString(18),
                                 (KozteruletJelleg)int.Parse(reader.GetString(19)), reader.GetString(20).ToString(), reader.GetString(21));
                        }
                        reader.Close();
                    }
                }
                kapcsolat.Close();
            }
           
            return tmp;
        }
        #endregion
        #region PartnerKivalasztasKategoriaAlapjan
        public static Partner PartnerKivalasztasKategoriaAlapjan(string adat)
        {
            Partner tmp = null;
            
            using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                kapcsolat.Open();
                using (SqlCommand Parancs = new SqlCommand())
                {

                    Parancs.Connection = kapcsolat;
                    Parancs.Parameters.Clear();
                    Parancs.CommandText = "SELECT * FROM[Nev] LEFT JOIN[Cim] ON[Cim].[CimId] =[Nev].[NevId] LEFT JOIN[Partner] ON[Partner].[PartnerId] =[Nev].[PartnerId] WHERE[PartnerCsoport] = @partnercsoport";
                    Parancs.Parameters.AddWithValue("@partnercsoport", adat);
                    using (SqlDataReader reader = Parancs.ExecuteReader())
                    {
                       
                        while (reader.Read())
                        {
                            tmp = new Partner(reader.GetInt32(6), reader.GetString(23), reader.GetString(24), reader.GetString(25),
                                 reader.GetString(26), reader.GetString(27), reader.GetString(28), reader.GetString(29), reader.GetString(30), reader.GetString(31),
                                 Convert.ToDecimal(reader.GetInt32(32)), reader.GetString(33), reader.GetString(34), reader.GetString(1), reader.GetString(2), reader.GetString(3),
                                 reader.GetString(4), reader.GetString(5), reader.GetString(8), reader.GetString(9).ToString(), reader.GetString(10), reader.GetString(11),
                                (KozteruletJelleg)int.Parse(reader.GetString(12)), reader.GetString(13).ToString(), reader.GetString(14), reader.GetString(15), reader.GetString(16).ToString(), reader.GetString(17), reader.GetString(18),
                                (KozteruletJelleg)int.Parse(reader.GetString(19)), reader.GetString(20).ToString(), reader.GetString(21));
                        }
                        reader.Close();
                    }
                }
                kapcsolat.Close();
            }
           
            return tmp;
        }
        #endregion
        #region PartnerKivalasztasFizetesiModAlapjan
        public static Partner PartnerKivalasztasFizetesiModAlapjan(string adat)
        {
            Partner tmp = null;

            using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                kapcsolat.Open();
                using (SqlCommand Parancs = new SqlCommand())
                {

                    Parancs.Connection = kapcsolat;
                    Parancs.Parameters.Clear();
                    Parancs.CommandText = "SELECT * FROM[Nev] LEFT JOIN[Cim] ON[Cim].[CimId] =[Nev].[NevId] LEFT JOIN[Partner] ON[Partner].[PartnerId] =[Nev].[PartnerId] WHERE[FizetesiMod] = @fizetesiMod";
                    Parancs.Parameters.AddWithValue("@fizetesiMod", adat);
                    using (SqlDataReader reader = Parancs.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            tmp = new Partner(reader.GetInt32(6), reader.GetString(23), reader.GetString(24), reader.GetString(25),
                                 reader.GetString(26), reader.GetString(27), reader.GetString(28), reader.GetString(29), reader.GetString(30), reader.GetString(31),
                                 Convert.ToDecimal(reader.GetInt32(32)), reader.GetString(33), reader.GetString(34), reader.GetString(1), reader.GetString(2), reader.GetString(3),
                                 reader.GetString(4), reader.GetString(5), reader.GetString(8), reader.GetString(9).ToString(), reader.GetString(10), reader.GetString(11),
                                (KozteruletJelleg)int.Parse(reader.GetString(12)), reader.GetString(13).ToString(), reader.GetString(14), reader.GetString(15), reader.GetString(16).ToString(), reader.GetString(17), reader.GetString(18),
                                (KozteruletJelleg)int.Parse(reader.GetString(19)), reader.GetString(20).ToString(), reader.GetString(21));
                        }
                        reader.Close();
                    }
                }
                kapcsolat.Close();
            }

            return tmp;
        }
        #endregion
        #region PartnerLista
        public static List<Partner> PartnerLista()
        {
            List<Partner> tmp=new List<Partner>();
            
            using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                kapcsolat.Open();
                using (SqlCommand Parancs = new SqlCommand())
                {

                    Parancs.Connection = kapcsolat;
                    Parancs.Parameters.Clear();
                    Parancs.CommandText = "SELECT * FROM[Nev] LEFT JOIN[Cim] ON[Cim].[CimId] =[Nev].[NevId] LEFT JOIN[Partner] ON[Partner].[PartnerId] =[Nev].[PartnerId] WHERE [Partner].PartnerCsoport<>@adat";
                    Parancs.Parameters.AddWithValue("@adat","1");
                    using (SqlDataReader reader = Parancs.ExecuteReader())
                    {
                        
                        


                            while (reader.Read())
                            {
                                tmp.Add(new Partner(reader.GetInt32(6), reader.GetString(23), reader.GetString(24), reader.GetString(25),
                                     reader.GetString(26), reader.GetString(27), reader.GetString(28), reader.GetString(29), reader.GetString(30), reader.GetString(31),
                                     Convert.ToDecimal(reader.GetInt32(32)), reader.GetString(33), reader.GetString(34), reader.GetString(1), reader.GetString(2), reader.GetString(3),
                                     reader.GetString(4), reader.GetString(5), reader.GetString(8), reader.GetString(9).ToString(), reader.GetString(10), reader.GetString(11),
                                    (KozteruletJelleg)int.Parse(reader.GetString(12)), reader.GetString(13), reader.GetString(14), reader.GetString(15), reader.GetString(16), reader.GetString(17), reader.GetString(18),
                                    (KozteruletJelleg)int.Parse(reader.GetString(19)), reader.GetString(20), reader.GetString(21)));
                            }
                        
                        reader.Close();
                    }
                }
                kapcsolat.Close();
            }
           
            return tmp;
        }
        #endregion
        #region SzallitoIdKivalasztas
        public static int SzallitoIdKivalasztas(int megrendelesSzam)
        {
            


           
                using (SqlConnection kapcsolat = new SqlConnection(cs))
                {
                try
                {
                    kapcsolat.Open();
                    using (SqlCommand Parancs = new SqlCommand())
                    {

                        

                        Parancs.Connection = kapcsolat;
                        Parancs.Parameters.Clear();
                        Parancs.CommandText = "SELECT [SzallitoId] FROM [SzallitoLevel] WHERE [BizonylatSzam]=@bizonylatSzam"; // JAJJ, DEJÓ, MŰKÖDIK, csak rosszul!!
                        Parancs.Parameters.AddWithValue("@bizonylatSzam", megrendelesSzam);
                        using (SqlDataReader reader = Parancs.ExecuteReader())
                        {
                           
                            while (reader.Read())
                            {

                               szallitoId=(int)reader["SzallitoId"];
                             
                            }
                            
                            reader.Close();
                        }
                    }
                    kapcsolat.Close();

                }
                catch (Exception ex)
                {
                    kapcsolat.Close();
                    throw new Exception(ex.Message + "Sikertelen felolvasás!", ex);
                }
            }
           

            return szallitoId;
        }
        #endregion
        #region AfaFelvitel
        public static int AfaFelvitel(decimal afa) 
        {
            int sqlID=-1;
            using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                kapcsolat.Open();
                using (SqlCommand Parancs = new SqlCommand())
                {
                    Parancs.Connection = kapcsolat;

                    Parancs.Parameters.Clear();
                    Parancs.CommandText = " INSERT [AfaKategoriak]  ([AfaKategoriaSzam]) " +
                        "OUTPUT INSERTED.AfaID VALUES(@afakulcs)";
                    Parancs.Parameters.AddWithValue("@afakulcs", afa);
                    
                    sqlID=(int)Parancs.ExecuteScalar();
                    
                }
                kapcsolat.Close();
            }
            return sqlID;
        }
        #endregion
        #region AfaModositas
        public static void AfaModositas( decimal afa, int id) 
        {
            using (SqlConnection kapcsolat = new SqlConnection(cs)) {
                kapcsolat.Open();
                using (SqlCommand Parancs = new SqlCommand()) {
                    Parancs.Connection = kapcsolat;
                    Parancs.Parameters.Clear();
                    Parancs.CommandText = " UPDATE [AfaKategoriak] SET [AfaKategoriaSzam]=@afakulcs WHERE [AfaID]=@afaid";
                    Parancs.Parameters.AddWithValue("@afakulcs", afa);
                  
                    Parancs.Parameters.AddWithValue("@afaid", id);
                    Parancs.ExecuteNonQuery();
                }
                kapcsolat.Close();
            }
        }
        #endregion
        #region AfaTorles
        public static void AfaTorles(int id) 
        {
            using (SqlConnection kapcsolat = new SqlConnection(cs)) {
                kapcsolat.Open();
                using (SqlCommand Parancs = new SqlCommand()) {
                    Parancs.Connection = kapcsolat;
                    Parancs.Parameters.Clear();
                    Parancs.CommandText = "DELETE FROM [AfaKategoriak] WHERE [AfaID]=@afaid";
                    Parancs.Parameters.AddWithValue("@afaid", id);
                    Parancs.ExecuteNonQuery();
                }
                kapcsolat.Close();
            }
        }
        #endregion
        #region TermekKategoriaFelvitel
        public static int TermekKategoriaFelvitel(string termekkategoria) 
        {
            int sqlID=-1;
            using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                kapcsolat.Open();
                using (SqlCommand Parancs = new SqlCommand())
                {
                    Parancs.Connection = kapcsolat;

                    Parancs.Parameters.Clear();
                    Parancs.CommandText = " INSERT [TermekKategoriak]  ([KategoriaMegnevezes])" +
                        "OUTPUT INSERTED.TermekKategoriaId VALUES(@kategoriaNev)";
                    Parancs.Parameters.AddWithValue("@kategoriaNev", termekkategoria);
                    sqlID = (int)Parancs.ExecuteScalar();

                }
                kapcsolat.Close();
            }
            return sqlID;
        }
        #endregion
        #region TermekKategoriaModositas
        public static void TermekKategoriaModositas(string termekkategoria, int id) 
        {
            using (SqlConnection kapcsolat = new SqlConnection(cs)) {
                kapcsolat.Open();
                using (SqlCommand Parancs = new SqlCommand()) {
                    Parancs.Connection = kapcsolat;
                    Parancs.Parameters.Clear();
                    Parancs.CommandText = " UPDATE [TermekKategoriak] SET [KategoriaMegnevezes]=@kategoriaMegnev WHERE [TermekKategoriaId]=@tkid";
                    Parancs.Parameters.AddWithValue("@kategoriaMegnev", termekkategoria);
                    Parancs.Parameters.AddWithValue("@tkid", id);
                    Parancs.ExecuteNonQuery();
                }
                kapcsolat.Close();
            }
        }
        #endregion
        #region TermekKategoriaTorles
        public static void TermekKategoriaTorles(int id) 
        {
            using (SqlConnection kapcsolat = new SqlConnection(cs)) {
                kapcsolat.Open();
                using (SqlCommand Parancs = new SqlCommand()) {
                    Parancs.Connection = kapcsolat;
                    Parancs.Parameters.Clear();
                    Parancs.CommandText = "DELETE FROM [TermekKategoriak] WHERE [TermekKategoriaId]=@tkid";
                    Parancs.Parameters.AddWithValue("@tkid", id);
                    Parancs.ExecuteNonQuery();
                }
                kapcsolat.Close();
            }
        }
        #endregion
        #region AfaKategoriakBetoltes
        public static List<Termek.AFA> AfaKategoriakBetoltes() {
            List<Termek.AFA> ideiglenesAFA = new List<Termek.AFA>();
           
                using (SqlConnection kapcsolat = new SqlConnection(cs)) {
                try
                {
                    kapcsolat.Open();
                    using (SqlCommand Parancs = new SqlCommand()) {
                        Parancs.Connection = kapcsolat;
                        Parancs.Parameters.Clear();
                        Parancs.CommandText = "SELECT * FROM [AfaKategoriak]"; 
                        using (SqlDataReader reader = Parancs.ExecuteReader()) {
                            while (reader.Read()) {
                                Termek.AFA tmp = new Termek.AFA();
                                tmp.SQLID = reader.GetInt32(0);
                                tmp.Value = (decimal)reader.GetInt32(1);
                               
                                ideiglenesAFA.Add(tmp);
                            }
                            reader.Close();
                        }
                    }
                    kapcsolat.Close();
                }
                catch (Exception ex)
                {
                    kapcsolat.Close();
                    throw new Exception(ex.Message + "Sikertelen felolvasás!", ex);
                }
            } 
            return ideiglenesAFA;
        }
        #endregion
        #region FizetesModBetoltes
        public static List<sqlString> FizetesModBetoltes() {
            List<sqlString> ideiglenesLista = new List<sqlString>();
            
                using (SqlConnection kapcsolat = new SqlConnection(cs)) {
                try
                {
                    kapcsolat.Open();
                    using (SqlCommand Parancs = new SqlCommand()) {
                        Parancs.Connection = kapcsolat;
                        Parancs.Parameters.Clear();
                        Parancs.CommandText = "SELECT * FROM [FizetesiMod]"; 
                        using (SqlDataReader reader = Parancs.ExecuteReader()) {
                            while (reader.Read()) {
                                sqlString tmp = new sqlString();
                                tmp.SQLID = reader.GetInt32(0);
                                tmp.Value = reader.GetString(1);
                                ideiglenesLista.Add(tmp);
                            }
                            reader.Close();
                        }
                    }
                    kapcsolat.Close();
                }
                catch (Exception ex)
                {
                    kapcsolat.Close();
                    throw new Exception(ex.Message + "Sikertelen felolvasás!", ex);
                }
            } 
            return ideiglenesLista;
        }
        #endregion
        #region FizetesModFelvitel
        public static int FizetesModFelvitel(string fizetesMod) 
        {
            int sqlID = -1;
            using (SqlConnection kapcsolat = new SqlConnection(cs)) {
                kapcsolat.Open();
                using (SqlCommand Parancs = new SqlCommand()) {
                    Parancs.Connection = kapcsolat;
                    Parancs.Parameters.Clear();
                    Parancs.CommandText = " INSERT [FizetesiMod]  ([FizetesiMod])" +
                        "OUTPUT INSERTED.FizetesiModID VALUES(@kategoriaNev)";
                    Parancs.Parameters.AddWithValue("@kategoriaNev", fizetesMod);
                    sqlID = (int)Parancs.ExecuteScalar();

                }
                kapcsolat.Close();
            }
            return sqlID;
        }
        #endregion
        #region FizetesModModositas
        public static void FizetesModModositas(string fizetesMod, int id) 
        {
            using (SqlConnection kapcsolat = new SqlConnection(cs)) {
                kapcsolat.Open();
                using (SqlCommand Parancs = new SqlCommand()) {
                    Parancs.Connection = kapcsolat;
                    Parancs.Parameters.Clear();
                    Parancs.CommandText = " UPDATE [FizetesiMod] SET [FizetesiMod]=@kategoriaMegnev WHERE [FizetesiModID]=@tkid";
                    Parancs.Parameters.AddWithValue("@kategoriaMegnev", fizetesMod);
                    Parancs.Parameters.AddWithValue("@tkid", id);
                    Parancs.ExecuteNonQuery();
                }
                kapcsolat.Close();
            }
        }
        #endregion
        #region FizetesModTorles
        public static void FizetesModTorles(int id) 
        {
            using (SqlConnection kapcsolat = new SqlConnection(cs)) {
                kapcsolat.Open();
                using (SqlCommand Parancs = new SqlCommand()) {
                    Parancs.Connection = kapcsolat;
                    Parancs.Parameters.Clear();
                    Parancs.CommandText = "DELETE FROM [FizetesiMod] WHERE [FizetesiModID]=@tkid";
                    Parancs.Parameters.AddWithValue("@tkid", id);
                    Parancs.ExecuteNonQuery();
                }
                kapcsolat.Close();
            }
        }
        #endregion
        #region PartnerBesorolasBetoltes
        public static List<sqlString> PartnerBesorolasBetoltes() {
            List<sqlString> ideiglenesLista = new List<sqlString>();
           
                using (SqlConnection kapcsolat = new SqlConnection(cs)) {
                try
                {
                    kapcsolat.Open();
                    using (SqlCommand Parancs = new SqlCommand()) {
                        Parancs.Connection = kapcsolat;
                        Parancs.Parameters.Clear();
                        Parancs.CommandText = "SELECT * FROM [PartnerBesorolas]"; 
                        using (SqlDataReader reader = Parancs.ExecuteReader()) {
                            while (reader.Read()) {
                                sqlString tmp = new sqlString();
                                tmp.SQLID = reader.GetInt32(0);
                                tmp.Value = reader.GetString(1);
                                ideiglenesLista.Add(tmp);
                            }
                            reader.Close();
                        }
                    }
                    kapcsolat.Close();
                }
                catch (Exception ex)
                {
                    kapcsolat.Close();
                    throw new Exception(ex.Message + "Sikertelen felolvasás!", ex);
                }
            } 
            return ideiglenesLista;
        }
        #endregion
        #region PartnerBesorolasFelvitel
        public static int PartnerBesorolasFelvitel(string partnerKategoria) 
        {
            int sqlID = -1;
            using (SqlConnection kapcsolat = new SqlConnection(cs)) {
                kapcsolat.Open();
                using (SqlCommand Parancs = new SqlCommand()) {
                    Parancs.Connection = kapcsolat;

                    Parancs.Parameters.Clear();
                    Parancs.CommandText = " INSERT [PartnerBesorolas]  ([PartnerBesorolasNeve])" +
                        "OUTPUT INSERTED.PartnerBesorolasID VALUES(@kategoriaNev)";
                    Parancs.Parameters.AddWithValue("@kategoriaNev", partnerKategoria);
                    sqlID = (int)Parancs.ExecuteScalar();

                }
                kapcsolat.Close();
            }
            return sqlID;
        }
        #endregion
        #region PartnerBesorolasModositas
        public static void PartnerBesorolasModositas(string partnerKategoria, int id) 
        {
            using (SqlConnection kapcsolat = new SqlConnection(cs)) {
                kapcsolat.Open();
                using (SqlCommand Parancs = new SqlCommand()) {
                    Parancs.Connection = kapcsolat;
                    Parancs.Parameters.Clear();
                    Parancs.CommandText = " UPDATE [PartnerBesorolas] SET [PartnerBesorolasNeve]=@kategoriaMegnev WHERE [PartnerBesorolasID]=@tkid";
                    Parancs.Parameters.AddWithValue("@kategoriaMegnev", partnerKategoria);
                    Parancs.Parameters.AddWithValue("@tkid", id);
                    Parancs.ExecuteNonQuery();
                }
                kapcsolat.Close();
            }
        }
        #endregion
        #region PartnerBesorolasTorles
        public static void PartnerBesorolasTorles(int id) 
        {
            using (SqlConnection kapcsolat = new SqlConnection(cs)) {
                kapcsolat.Open();
                using (SqlCommand Parancs = new SqlCommand()) {
                    Parancs.Connection = kapcsolat;
                    Parancs.Parameters.Clear();
                    Parancs.CommandText = "DELETE FROM [PartnerBesorolas] WHERE [PartnerBesorolasID]=@tkid";
                    Parancs.Parameters.AddWithValue("@tkid", id);
                    Parancs.ExecuteNonQuery();
                }
                kapcsolat.Close();
            }
        }
        #endregion
        #region TermekKategoriakBetoltes
        public static List<sqlString> TermekKategoriakBetoltes() {
            List<sqlString> ideiglenesLista = new List<sqlString>();
          
                using (SqlConnection kapcsolat = new SqlConnection(cs)) {
                try
                {
                    kapcsolat.Open();
                    using (SqlCommand Parancs = new SqlCommand()) {
                        Parancs.Connection = kapcsolat;
                        Parancs.Parameters.Clear();
                        Parancs.CommandText = "SELECT * FROM [TermekKategoriak]"; 
                        using (SqlDataReader reader = Parancs.ExecuteReader()) {
                            while (reader.Read()) {
                                sqlString tmp = new sqlString();
                                tmp.SQLID = reader.GetInt32(0);
                                tmp.Value = reader.GetString(1);
                                ideiglenesLista.Add(tmp);
                            }
                            reader.Close();
                        }
                    }
                    kapcsolat.Close();
                }
                catch (Exception ex)
                {
                    kapcsolat.Close();
                    throw new Exception(ex.Message + "Sikertelen felolvasás!", ex);
                }
            } 
            return ideiglenesLista;
        }
        #endregion
        #region TermekMennyisegEgysegBetoltes
        public static List<sqlString> TermekMennyisegEgysegBetoltes() {
            List<sqlString> ideiglenesLista = new List<sqlString>();
          
                using (SqlConnection kapcsolat = new SqlConnection(cs)) {
                try
                {
                    kapcsolat.Open();
                    using (SqlCommand Parancs = new SqlCommand()) {
                        Parancs.Connection = kapcsolat;
                        Parancs.Parameters.Clear();
                        Parancs.CommandText = "SELECT * FROM [TermekMennyisegEgyseg]"; 
                        using (SqlDataReader reader = Parancs.ExecuteReader()) {
                            while (reader.Read()) {
                                sqlString tmp = new sqlString();
                                tmp.SQLID = reader.GetInt32(0);
                                tmp.Value = reader.GetString(1);
                                ideiglenesLista.Add(tmp);
                            }
                            reader.Close();
                        }
                    }
                    kapcsolat.Close();
                }
                catch (Exception ex)
                {
                    kapcsolat.Close();
                    throw new Exception(ex.Message + "Sikertelen felolvasás!", ex);
                }
            } 
            return ideiglenesLista;
        }
        #endregion
        #region TermekMennyisegEgysegFelvitel
        public static int TermekMennyisegEgysegFelvitel(string termekMennyisegEgyseg) 
        {
            int sqlID = -1;
            using (SqlConnection kapcsolat = new SqlConnection(cs)) {
                kapcsolat.Open();
                using (SqlCommand Parancs = new SqlCommand()) {
                    Parancs.Connection = kapcsolat;
                    Parancs.Parameters.Clear();
                    Parancs.CommandText = " INSERT [TermekMennyisegEgyseg]  ([TermekMennyisegEgyseg])" +
                        "OUTPUT INSERTED.TermekMennyisegEgysegID VALUES(@kategoriaNev)";
                    Parancs.Parameters.AddWithValue("@kategoriaNev", termekMennyisegEgyseg);
                    sqlID = (int)Parancs.ExecuteScalar();
                }
                kapcsolat.Close();
            }
            return sqlID;
        }
        #endregion
        #region TermekMennyisegEgysegModositas
        public static void TermekMennyisegEgysegModositas(string termekMennyisegEgyseg, int id) 
        {
            using (SqlConnection kapcsolat = new SqlConnection(cs)) {
                kapcsolat.Open();
                using (SqlCommand Parancs = new SqlCommand()) {
                    Parancs.Connection = kapcsolat;
                    Parancs.Parameters.Clear();
                    Parancs.CommandText = " UPDATE [TermekMennyisegEgyseg] SET [TermekMennyisegEgyseg]=@kategoriaMegnev WHERE [TermekMennyisegEgysegID]=@tkid";
                    Parancs.Parameters.AddWithValue("@kategoriaMegnev", termekMennyisegEgyseg);
                    Parancs.Parameters.AddWithValue("@tkid", id);
                    Parancs.ExecuteNonQuery();
                }
                kapcsolat.Close();
            }
        }
        #endregion
        #region TermekMennyisegEgysegTorles
        public static void TermekMennyisegEgysegTorles(int id) 
        {
            using (SqlConnection kapcsolat = new SqlConnection(cs)) {
                kapcsolat.Open();
                using (SqlCommand Parancs = new SqlCommand()) {
                    Parancs.Connection = kapcsolat;
                    Parancs.Parameters.Clear();
                    Parancs.CommandText = "DELETE FROM [TermekMennyisegEgyseg] WHERE [TermekMennyisegEgysegID]=@tkid";
                    Parancs.Parameters.AddWithValue("@tkid", id);
                    Parancs.ExecuteNonQuery();
                }
                kapcsolat.Close();
            }
        }
        #endregion
        #region TermekKivalasztas
        public static bool TermekMennyisegBevetelezes(Termek adat)
        {
            bool igaze=false;
            using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                kapcsolat.Open();
                using (SqlCommand Parancs = new SqlCommand())
                {

                    Parancs.Connection = kapcsolat;
                    Parancs.Parameters.Clear();
                    Parancs.CommandText = "SELECT * FROM [Termekek] WHERE  [TermekAzonosito]=@azonosito AND [Mennyiseg]- @mennyiseg<0 AND [Statusz]<>0";
                    Parancs.Parameters.AddWithValue("@azonosito", adat.TermekAzonosito);
                    Parancs.Parameters.AddWithValue("@mennyiseg", adat.Mennyiseg);
                    using (SqlDataReader reader = Parancs.ExecuteReader())
                    {
                        igaze = reader.HasRows;
                      
                    }
                }
                kapcsolat.Close();
            }

            return igaze ;
        }
        public static Termek TermekKivalasztas(int adat)
        {
            Termek tmp = null;

            using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                kapcsolat.Open();
                using (SqlCommand Parancs = new SqlCommand())
                {

                    Parancs.Connection = kapcsolat;
                    Parancs.Parameters.Clear();
                    Parancs.CommandText = "SELECT * FROM [Termekek] WHERE [TermekAzonosito] = @termekAzonosito";
                    Parancs.Parameters.AddWithValue("@termekAzonosito", adat);
                    using (SqlDataReader reader = Parancs.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            tmp = new Termek(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(8), reader.GetInt32(3),
                                reader.GetString(4), Convert.ToDecimal(reader.GetInt32(5)), Convert.ToDecimal(reader.GetInt32(6)), Convert.ToDecimal(reader.GetInt32(7)), reader.GetString(9),
                                reader.GetInt32(10));
                        }
                        reader.Close();
                    }
                }
                kapcsolat.Close();
            }

            return tmp;
        }
        #endregion
        #region TermekiValasztasAfaAlapjan
        public static Termek TermekiValasztasAfaAlapjan(int adat)
        {
            Termek tmp = null;

            using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                kapcsolat.Open();
                using (SqlCommand Parancs = new SqlCommand())
                {

                    Parancs.Connection = kapcsolat;
                    Parancs.Parameters.Clear();
                    Parancs.CommandText = "SELECT * FROM [Termekek] WHERE [AfaKulcs] = @afaKulcs";
                    Parancs.Parameters.AddWithValue("@afaKulcs", adat);
                    using (SqlDataReader reader = Parancs.ExecuteReader())
                    {

                        while (reader.Read())
                        {

                            tmp = new Termek(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(8), reader.GetInt32(3),
                                reader.GetString(4), Convert.ToDecimal(reader.GetInt32(5)), Convert.ToDecimal(reader.GetInt32(6)), Convert.ToDecimal(reader.GetInt32(7)), reader.GetString(9),
                                reader.GetInt32(10));
                        }
                        reader.Close();
                    }
                }
                kapcsolat.Close();
            }

            return tmp;
        }
        #endregion
        #region SzallitoLevelKivalasztas
        public static SzallitoLevel SzallitoLevelKivalasztas(int adat)
        {
            SzallitoLevel tmp = null;
          
            using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                kapcsolat.Open();
                using (SqlCommand Parancs = new SqlCommand())
                {

                    Parancs.Connection = kapcsolat;
                    Parancs.Parameters.Clear();
                    Parancs.CommandText = "SELECT * FROM [SzallitoLevel] WHERE [SzallitoId] = @szallitoId";
                    Parancs.Parameters.AddWithValue("@szallitoId", adat);
                    using (SqlDataReader reader = Parancs.ExecuteReader())
                    {
                        
                        while (reader.Read())
                        {
                            
                          
                            
                            tmp = new SzallitoLevel(reader.GetInt32(0), reader.GetInt32(1), reader.GetDateTime(2), reader.GetInt32(3), reader.GetInt32(4));

                        }
                        reader.Close();
                    }
                }
                kapcsolat.Close();
            }
            
            return tmp;
        }
        #endregion
        #region SzallitoLevelKivalasztasPartnerAlapjan
        public static SzallitoLevel SzallitoLevelKivalasztasPartnerAlapjan(int adat)
        {
            SzallitoLevel tmp = null;
            
            using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                kapcsolat.Open();
                using (SqlCommand Parancs = new SqlCommand())
                {

                    Parancs.Connection = kapcsolat;
                    Parancs.Parameters.Clear();
                    Parancs.CommandText = "SELECT * FROM [SzallitoLevel] WHERE [PartnerId2] = @partnerid";
                    Parancs.Parameters.AddWithValue("@partnerid", adat);
                    using (SqlDataReader reader = Parancs.ExecuteReader())
                    {
                       
                        while (reader.Read())
                        {



                            tmp = new SzallitoLevel(reader.GetInt32(0), reader.GetInt32(1), reader.GetDateTime(2), reader.GetInt32(3), reader.GetInt32(4));

                        }
                        reader.Close();
                    }
                }
                kapcsolat.Close();
            }
         
            return tmp;
        }
        #endregion
        #region ToroltTermekKivalasztas
        public static List<Termek> ToroltTermekKivalasztas()
        {
            List<Termek> toroltTermek = new List<Termek>();
            Termek tmp = null;
           
            using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                kapcsolat.Open();
                using (SqlCommand Parancs = new SqlCommand())
                {

                    Parancs.Connection = kapcsolat;
                    Parancs.Parameters.Clear();
                    Parancs.CommandText = "SELECT * FROM [Termekek] WHERE [Statusz] =0";
                   
                    using (SqlDataReader reader = Parancs.ExecuteReader())
                    {
                        
                        while (reader.Read())
                        {
                           
                            tmp = new Termek(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(8), reader.GetInt32(3),
                                reader.GetString(4), Convert.ToDecimal(reader.GetInt32(5)), Convert.ToDecimal(reader.GetInt32(6)), Convert.ToDecimal(reader.GetInt32(7)), reader.GetString(9),
                                reader.GetInt32(10));
                            toroltTermek.Add(tmp);
                        }
                        reader.Close();
                    }
                }
                kapcsolat.Close();
            }
         
            return toroltTermek;
        }
        #endregion
        #region ToroltTermekKivalasztasSzallitobol
        public static List<Termek> ToroltTermekKivalasztasSzallitobol(List<Termek> lista)
        {
            List<Termek> toroltTermek = new List<Termek>();
            Termek tmp = null;
            
            using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                kapcsolat.Open();
                using (SqlCommand Parancs = new SqlCommand())
                {

                    Parancs.Connection = kapcsolat;
                    Parancs.Parameters.Clear();
                    Parancs.CommandText = "SELECT * FROM [Termekek] WHERE [Statusz]=0 AND [TermekAzonosito]=(SELECT [KiadottTermekId] FROM [KiadottTermek]) ";

                    using (SqlDataReader reader = Parancs.ExecuteReader())
                    {
                        
                        while (reader.Read())
                        {
                            
                            tmp = new Termek(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(8), reader.GetInt32(3),
                                reader.GetString(4), Convert.ToDecimal(reader.GetInt32(5)), Convert.ToDecimal(reader.GetInt32(6)), Convert.ToDecimal(reader.GetInt32(7)), reader.GetString(9),
                                reader.GetInt32(10));
                            toroltTermek.Add(tmp);
                        }
                        reader.Close();
                    }
                }
                kapcsolat.Close();
            }
            
            return toroltTermek;
        }
        #endregion
        #region TermekKivalasztasCikkszamAlapjan
        public static string TermekKivalasztasCikkszamAlapjan(string adat)
        {
            string tmp = null;


           
            using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                try
                {
                    
                    kapcsolat.Open();
                    using (SqlCommand Parancs = new SqlCommand())
                    {

                    Parancs.Connection = kapcsolat;
                    Parancs.Parameters.Clear();
                    Parancs.CommandText = "SELECT * FROM [Termekek] WHERE [CikkSzam] = @cikkSzam";
                    Parancs.Parameters.AddWithValue("@cikkSzam", adat);
                    using (SqlDataReader reader = Parancs.ExecuteReader())
                    {
                          

                        while (reader.Read())
                        {

                            tmp = reader.GetString(8);
                                
                        }
                        reader.Close();
                    }
                }
                kapcsolat.Close();
            }
                catch (Exception ex)
                {
                    kapcsolat.Close();
                    throw new Exception(ex.Message + "A termék már cikkszám alapján szerepel az adatbázisban!", ex);


                }
            }
            
            return tmp;
        }
        #endregion
        #region TermekLVFeltoltes
        public static void TermekLVFeltoltes(ListView lv)
        {

           
                
                lv.Columns.Clear();
                lv.Items.Clear();
                lv.View = View.Details;

              
				using (SqlConnection kapcsolat = new SqlConnection(cs)) {
                try
                {
                    kapcsolat.Open();
					using (SqlCommand Parancs = new SqlCommand()) {
						Parancs.Connection = kapcsolat;
                        Parancs.Parameters.Clear();
						Parancs.CommandText = "SELECT * FROM [Termekek] WHERE [Statusz]=@statusz";
                        Parancs.Parameters.AddWithValue("@statusz",1);
						using (SqlDataReader reader = Parancs.ExecuteReader())
						{
							while (reader.Read())
							{
								if (lv.Columns.Count == 0) 
								{  string[] oszlopnevek =
                                    {
                                        "Termékazonosító","Termékkategória","Megnevezés","Mennyíség","Mennyíség egysége","Netto ár","Brutto ár","ÁFA kulcs","Cikkszám","Vtsz/Szj/Teszor",
                                        "Minimális készlet"
                                    };
									for (int i = 0; i < reader.FieldCount-1; i++)
									{
										lv.Columns.Add(oszlopnevek[i]); 
									}
								}
								string[] adatok = new string[reader.FieldCount]; 
								for (int i = 0; i < reader.FieldCount-1; i++)
								{
									adatok[i] = reader[i].ToString(); 
								}
								lv.Items.Add(new ListViewItem(adatok)); 
							}
							reader.Close();
						}
					}
					kapcsolat.Close();
                    TermekekFrm.HianyListaJeloles(lv, null, termekKereso());
                }
                
                catch (Exception ex)
                {
                    kapcsolat.Close();
                    throw new Exception(ex.Message + "Sikertelen felolvasás!", ex);
                }
            }

        }
        public static void termekekLVFeltoltes(List<Termek> bemenet, ListView adat)
        {
            adat.Columns.Clear();
            adat.Items.Clear();
            adat.View = View.Details;
            if (adat.Columns.Count == 0)
            {
                adat.Columns.Add("Termékazonosító");
                adat.Columns.Add("Termékkategória");
                adat.Columns.Add("Megnevezés");
                adat.Columns.Add("Mennyíség");
                adat.Columns.Add("Mennyíség egysége");
                adat.Columns.Add("Netto ár");
                adat.Columns.Add("Brutto ár");
                adat.Columns.Add("ÁFA kulcs");
                adat.Columns.Add("Cikkszám");
                adat.Columns.Add("Vtsz/Szj/Teszor");     
                adat.Columns.Add("Minimális készlet");
            }

            for (int i = 0; i < bemenet.Count; i++)
            {
                string[] adatok = {
                bemenet[i].TermekAzonosito.ToString(),bemenet[i].TermekKategoria.ToString(),bemenet[i].Megnevezes,bemenet[i].Mennyiseg.ToString(),bemenet[i].MennyisegEgysege,bemenet[i].BruttoAr.ToString(),
                bemenet[i].NettoAr.ToString(),bemenet[i].AfaKulcs.ToString(),bemenet[i].Cikkszam,bemenet[i].NavSorszam,bemenet[i].MinimalisKeszlet.ToString()};
                adat.Items.Add(new ListViewItem(adatok));
            }
            TermekekFrm.HianyListaJeloles(adat, null, bemenet);
        }

        public static List<Termek> termekKereso(string kulcsszo="", KeresesKategoria kat = KeresesKategoria.Összes) {
            List<Termek> result = new List<Termek>();
            using (SqlConnection kapcsolat = new SqlConnection(cs)) {
                kapcsolat.Open();
                using (SqlCommand Parancs = new SqlCommand()) {
                    Parancs.Connection = kapcsolat;
                    if(kat == KeresesKategoria.Összes) {
                        Parancs.CommandText = "SELECT * FROM [Termekek] WHERE [Megnevezes] LIKE @param AND [Statusz]=@statusz OR [TermekAzonosito] LIKE @param AND [Statusz]=@statusz OR [TermekKategoria] LIKE @param AND [Statusz]=@statusz OR [CikkSzam] LIKE @param AND [Statusz]=@statusz";
                        Parancs.Parameters.AddWithValue("@param", $"%{kulcsszo}%");
                        Parancs.Parameters.AddWithValue("@statusz", 1);
                    } else if (kat == KeresesKategoria.Megnevezes) {
                        Parancs.CommandText = "SELECT * FROM [Termekek] WHERE [Megnevezes] LIKE @param AND [Statusz]=@statusz";
                        Parancs.Parameters.AddWithValue("@param", $"%{kulcsszo}%");
                        Parancs.Parameters.AddWithValue("@statusz", 1);
                    } else if (kat == KeresesKategoria.TermekAzonosito) {
                        Parancs.CommandText = "SELECT * FROM [Termekek] WHERE [TermekAzonosito] LIKE @param AND [Statusz]=@statusz";
                        Parancs.Parameters.AddWithValue("@param", $"%{kulcsszo}%");
                        Parancs.Parameters.AddWithValue("@statusz", 1);
                    } else if (kat == KeresesKategoria.TermekKategoria) {
                        Parancs.CommandText = "SELECT * FROM [Termekek] WHERE [TermekKategoria] LIKE @param AND [Statusz]=@statusz";
                        Parancs.Parameters.AddWithValue("@param", $"%{kulcsszo}%");
                        Parancs.Parameters.AddWithValue("@statusz", 1);
                    } else if (kat == KeresesKategoria.CikkSzam) {
                        Parancs.CommandText = "SELECT * FROM [Termekek] WHERE [CikkSzam] LIKE @param AND [Statusz]=@statusz";
                        Parancs.Parameters.AddWithValue("@param", $"%{kulcsszo}%");
                        Parancs.Parameters.AddWithValue("@statusz", 1);
                    }
                    using (SqlDataReader reader = Parancs.ExecuteReader()) {
                        while (reader.Read()) {
                            result.Add(new Termek(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(8), reader.GetInt32(3),
                                reader.GetString(4), Convert.ToDecimal(reader.GetInt32(5)), Convert.ToDecimal(reader.GetInt32(6)), Convert.ToDecimal(reader.GetInt32(7)), reader.GetString(9),
                                reader.GetInt32(10))); 
                        }
                        reader.Close();
                    }
                }
                kapcsolat.Close();
            }
            return result;
        }
        public static List<SzallitoLevel> SzallitoLevelKereso(string kulcsszo = "", Szallitolevelkereses kat = Szallitolevelkereses.Összes)
        {
            //"SELECT * FROM[Nev] LEFT JOIN[Cim] ON[Cim].[CimId] =[Nev].[NevId] LEFT JOIN[Partner] ON[Partner].[PartnerId] =[Nev].[PartnerId]WHERE[PartnerCsoport] <>@adat AND [Titulus] LIKE @param  OR [PartnerCsoport] <>@adat AND [VezetekNev] LIKE @param" +
            //" OR [PartnerCsoport] <>@adat AND [KeresztNev] LIKE @param  OR [PartnerCsoport] <>@adat AND [CegNev] LIKE @param  OR [PartnerCsoport] <>@adat AND [AdoSzam] LIKE @param  OR [PartnerCsoport] <>@adat AND[TelefonSzam] LIKE @param  OR [PartnerCsoport] <>@adat AND[Fax] LIKE @param  OR [PartnerCsoport] <>@adat AND [Email] LIKE @param  OR [PartnerCsoport] <>@adat AND [Irsz] LIKE @param  OR [PartnerCsoport] <>@adat AND [Telepules] LIKE @param  OR [PartnerCsoport] <>@adat AND [Kozterulet] LIKE @param  OR" +
            //" [PartnerCsoport] <>@adat AND [KozteruletJelleg] LIKE @param  OR [PartnerCsoport] <>@adat AND" +
            //" [HazSzam] LIKE @param  OR [PartnerCsoport] <>@adat AND [EmeletAjto] LIKE @param"
            List<SzallitoLevel> result = new List<SzallitoLevel>();
            //Partner tmp = null;
            using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                kapcsolat.Open();
                using (SqlCommand Parancs = new SqlCommand())
                {
                    Parancs.Connection = kapcsolat;
                    if (kat == Szallitolevelkereses.Összes)
                    {
                        Parancs.CommandText = "SELECT * FROM [SzallitoLevel] LEFT JOIN [Partner] ON [Partner].[PartnerId]=[SzallitoLevel].[PartnerId2] LEFT JOIN [Nev] ON [Partner].[PartnerId]=[Nev].[PartnerId] WHERE [AdoSzam] LIKE @param OR [SzallitoId] LIKE @param OR [PartnerId2] LIKE @param OR CONVERT(VARCHAR(25),[BizonylatKelte],102) LIKE @param OR [BizonylatSzam] LIKE @param OR [Bevetel] LIKE @param OR [VezetekNev] LIKE @param OR [KeresztNev] LIKE @param OR [CegNev] LIKE @param";
                        Parancs.Parameters.AddWithValue("@param", $"%{kulcsszo}%");
                    }        
                    else if (kat == Szallitolevelkereses.Azonosító)
                    {
                        Parancs.CommandText = "SELECT * FROM [SzallitoLevel] WHERE [SzallitoId] LIKE @param";
                        Parancs.Parameters.AddWithValue("@param", $"%{kulcsszo}%");
                    }
                    else if (kat == Szallitolevelkereses.Adószám)
                    {
                        Parancs.CommandText = "SELECT * FROM [SzallitoLevel] LEFT JOIN [Partner] ON [Partner].[PartnerId]=[SzallitoLevel].[PartnerId2] WHERE [AdoSzam] LIKE @param";
                        Parancs.Parameters.AddWithValue("@param", $"%{kulcsszo}%");
                    }
                    else if (kat == Szallitolevelkereses.Bizonylatszám)
                    {
                        Parancs.CommandText = "SELECT * FROM [SzallitoLevel] WHERE [BizonylatSzam] LIKE @param";
                        Parancs.Parameters.AddWithValue("@param", $"%{kulcsszo}%");
                    }
                    else if (kat == Szallitolevelkereses.Dátum)
                    {
                      
                       
                        Parancs.CommandText = "SELECT * FROM [SzallitoLevel] WHERE CONVERT(VARCHAR(25),[BizonylatKelte],102) LIKE @param";//A 102 a dátum formátum, ami alapján keresünk
                        Parancs.Parameters.AddWithValue("@param", $"%{kulcsszo}%");
                    }
                    using (SqlDataReader reader = Parancs.ExecuteReader())
                    {
                        while (reader.Read())
                        {



                            result.Add(new SzallitoLevel(reader.GetInt32(0), reader.GetInt32(1), reader.GetDateTime(2), reader.GetInt32(3), reader.GetInt32(4)));

                        }
                        reader.Close();
                    }

                }
                kapcsolat.Close();
            }
            return result;
        }

       
        public static List<Bevetelezes> BevetelezesKereso(string kulcsszo = "", Szallitolevelkereses kat = Szallitolevelkereses.Összes) {
            List<Bevetelezes> result = new List<Bevetelezes>();
            using (SqlConnection kapcsolat = new SqlConnection(cs)) {
                kapcsolat.Open();
                using (SqlCommand Parancs = new SqlCommand()) {
                    Parancs.Connection = kapcsolat;
                    if (kat == Szallitolevelkereses.Összes) {
                        Parancs.CommandText = "SELECT * FROM [Bevetelezesek] LEFT JOIN [Partner] ON [Partner].[PartnerId]=[Bevetelezesek].[PartnerId3] LEFT JOIN [Nev] ON [Partner].[PartnerId]=[Nev].[PartnerId] WHERE [BevetId] LIKE @param OR [PartnerId3] LIKE @param OR [BizonylatKelte2] LIKE @param OR [BizonylatSzam2] LIKE @param OR [Kiadas] LIKE @param OR [AdoSzam] LIKE @param OR [VezetekNev] LIKE @param OR [KeresztNev] LIKE @param";
                        Parancs.Parameters.AddWithValue("@param", $"%{kulcsszo}%");
                    } else if (kat == Szallitolevelkereses.Azonosító) {
                        Parancs.CommandText = "SELECT * FROM [Bevetelezesek] WHERE [BevetId] LIKE @param";
                        Parancs.Parameters.AddWithValue("@param", $"%{kulcsszo}%");
                    } else if (kat == Szallitolevelkereses.Adószám) {
                        
                        Parancs.CommandText = "SELECT * FROM [Bevetelezesek] LEFT JOIN [Partner] ON [Partner].[PartnerId]=[Bevetelezesek].[PartnerId3] WHERE [AdoSzam] LIKE @param";
                        Parancs.Parameters.AddWithValue("@param", $"%{kulcsszo}%");
                    } else if (kat == Szallitolevelkereses.Bizonylatszám) {
                        Parancs.CommandText = "SELECT * FROM [Bevetelezesek] WHERE [BizonylatSzam2] LIKE @param";
                        Parancs.Parameters.AddWithValue("@param", $"%{kulcsszo}%");
                    } else if (kat == Szallitolevelkereses.Dátum) {
                        Parancs.CommandText = "SELECT * FROM [Bevetelezesek] WHERE [BizonylatKelte2] LIKE @param";
                        Parancs.Parameters.AddWithValue("@param", $"%{kulcsszo}%");
                    }
                    using (SqlDataReader reader = Parancs.ExecuteReader()) {
                        while (reader.Read()) {
                            result.Add(new Bevetelezes(reader.GetInt32(0), reader.GetInt32(1), reader.GetDateTime(2), reader.GetInt32(3), reader.GetInt32(4)));
                        }
                        reader.Close();
                    }
                }
                kapcsolat.Close();
            }
            return result;
        }

            #region PartnerKereso
            public static List<Partner> PartnerKereso(string kulcsszo = "", partnerKereses kat = partnerKereses.Összes) //Ezt módosítom
        {
            List<Partner> result = new List<Partner>();
            using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                kapcsolat.Open();
                using (SqlCommand Parancs = new SqlCommand())
                {
                    Parancs.Connection = kapcsolat;
                    if (kat == partnerKereses.Összes)
                    {
                        Parancs.CommandText = "SELECT * FROM[Nev] LEFT JOIN[Cim] ON[Cim].[CimId] =[Nev].[NevId] LEFT JOIN[Partner] ON[Partner].[PartnerId] =[Nev].[PartnerId]WHERE[PartnerCsoport] <>@adat AND [Titulus] LIKE @param  OR [PartnerCsoport] <>@adat AND [VezetekNev] LIKE @param" +
                            " OR [PartnerCsoport] <>@adat AND [KeresztNev] LIKE @param  OR [PartnerCsoport] <>@adat AND [CegNev] LIKE @param  OR [PartnerCsoport] <>@adat AND [AdoSzam] LIKE @param  OR [PartnerCsoport] <>@adat AND[TelefonSzam] LIKE @param  OR [PartnerCsoport] <>@adat AND[Fax] LIKE @param  OR [PartnerCsoport] <>@adat AND [Email] LIKE @param  OR [PartnerCsoport] <>@adat AND [Irsz] LIKE @param  OR [PartnerCsoport] <>@adat AND [Telepules] LIKE @param  OR [PartnerCsoport] <>@adat AND [Kozterulet] LIKE @param  OR" +
                            " [PartnerCsoport] <>@adat AND [KozteruletJelleg] LIKE @param  OR [PartnerCsoport] <>@adat AND" +
                            " [HazSzam] LIKE @param  OR [PartnerCsoport] <>@adat AND [EmeletAjto] LIKE @param";
                        
                    }
                    else if (kat == partnerKereses.Partnercsoport)
                    {
                        Parancs.CommandText = "SELECT * FROM[Nev] LEFT JOIN[Cim] ON[Cim].[CimId] =[Nev].[NevId] LEFT JOIN[Partner] ON[Partner].[PartnerId] =[Nev].[PartnerId] WHERE[PartnerCsoport] <>@adat AND [PartnerCsoport] LIKE @param";
                      
                    }
                    else if (kat == partnerKereses.Cégnév)
                    {
                        Parancs.CommandText = "SELECT * FROM[Nev] LEFT JOIN[Cim] ON[Cim].[CimId] =[Nev].[NevId] LEFT JOIN[Partner] ON[Partner].[PartnerId] =[Nev].[PartnerId]WHERE[PartnerCsoport] <>@adat AND [CegNev] LIKE @param";
                      
                    }
                    else if (kat == partnerKereses.Adószám)
                    {
                        Parancs.CommandText = "SELECT * FROM[Nev] LEFT JOIN[Cim] ON[Cim].[CimId] =[Nev].[NevId] LEFT JOIN[Partner] ON[Partner].[PartnerId] =[Nev].[PartnerId] WHERE[PartnerCsoport] <>@adat AND [AdoSzam] LIKE @param";
                      
                    }
                    Parancs.Parameters.AddWithValue("@param", $"%{kulcsszo}%");
                    Parancs.Parameters.AddWithValue("@adat", "1");
                    using (SqlDataReader reader = Parancs.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                           


                            result.Add(new Partner(reader.GetInt32(6), reader.GetString(23), reader.GetString(24), reader.GetString(25),
                                  reader.GetString(26), reader.GetString(27), reader.GetString(28), reader.GetString(29), reader.GetString(30), reader.GetString(31),
                                  Convert.ToDecimal(reader.GetInt32(32)), reader.GetString(33), reader.GetString(34), reader.GetString(1), reader.GetString(2), reader.GetString(3),
                                  reader.GetString(4), reader.GetString(5), reader.GetString(8), reader.GetString(9).ToString(), reader.GetString(10), reader.GetString(11),
                                 (KozteruletJelleg)int.Parse(reader.GetString(12)), reader.GetString(13).ToString(), reader.GetString(14), reader.GetString(15), reader.GetString(16).ToString(), reader.GetString(17), reader.GetString(18),
                                 (KozteruletJelleg)int.Parse(reader.GetString(19)), reader.GetString(20).ToString(), reader.GetString(21)));
                        }
                        reader.Close();
                    }
                }
                kapcsolat.Close();
            }
            return result;
        }
        #endregion
        
        #region ExcelSzallitoMentes
        public static void ExcelSzallitoMentes(Partner adat1,Partner adat2,List<Termek>termekek,SzallitoLevel szallito)
        {
            
         
            int l=0;

            using (XLWorkbook excel=new XLWorkbook())

            {
                IXLWorksheet munkalap = excel.AddWorksheet();
                decimal ar = 0;

                int k = termekek.Count+ 26;

                for (int i = 0; i < termekek.Count; i++)
                {
                    ar += termekek[i].BruttoAr;
                }
               

                munkalap.Name = "Szállítólevél";
                 munkalap.Cell("A2").Value = "Szállítólevél";
                 munkalap.Cell("A4").Value = "Szállítólevél azonosító:";
                 munkalap.Cell("B4").Value = szallito.SzallitoId;
                munkalap.Cell("A6").Value = "   Eladó adatai:";//+2

                munkalap.Cell("A8").Value = "Cégnév";//+2
                if (adat2.CegNev == "Magánszemély")
                {
                   
                    munkalap.Cell("G15").Value = "Adóazonosító jel:";
                }
                else
                {
                    
                    munkalap.Cell("G15").Value = "Adószám:";
                }
                munkalap.Cell("B8").Value = adat1.CegNev;
                 munkalap.Cell("A9").Value = "kapcsolattartó";
                 munkalap.Cell("B9").Value = adat1.VezetekNev;
                 munkalap.Cell("C9").Value = adat1.KeresztNev;
                 munkalap.Cell("A10").Value="Cím:";
                 munkalap.Cell("A12").Value = adat1.Irsz;
                
                munkalap.Cell("B12").Value = adat1.Telepules;
                 munkalap.Cell("C12").Value = adat1.Kozterulet;
                 munkalap.Cell("A13").Value = adat1.KozteruletJelleg.ToString();
                 munkalap.Cell("B13").Value = adat1.HazSzam;
                 munkalap.Cell("C13").Value = adat1.EmeletAjto;
                 munkalap.Cell("A15").Value ="Adószám:";
                munkalap.Cell("B15").Value = adat1.AdoSzam;
                munkalap.Cell("A16").Value ="Telefonszám:";
                 munkalap.Cell("B16").Value = adat1.TelefonSzam;
                 munkalap.Cell("A17").Value = "Bankszámlaszám:";
                 munkalap.Cell("B17").Value = adat1.BankSzamlaSzam;
                 munkalap.Cell("A19").Value = "Megrendelésszám:";
                 munkalap.Cell("B19").Value = szallito.MegrendelesSzam;
                 munkalap.Cell("G4").Value = "Kiállítás dátuma:";
                 munkalap.Cell("H4").Value = szallito.BizonylatKelte;
                munkalap.Cell("G6").Value = "Vevő adatai:";
                munkalap.Cell("G8").Value = "Cégnév";
                 munkalap.Cell("H8").Value = adat2.CegNev;
                 munkalap.Cell("G9").Value = "kapcsolattartó";
                 munkalap.Cell("H9").Value = adat2.VezetekNev;
                 munkalap.Cell("I9").Value = adat2.KeresztNev;
                 munkalap.Cell("G10").Value = "Cím:";
                 munkalap.Cell("G12").Value = adat2.Irsz;
                 munkalap.Cell("H12").Value = adat2.Telepules;
                 munkalap.Cell("I12").Value = adat2.Kozterulet;
                 munkalap.Cell("G13").Value = adat2.KozteruletJelleg.ToString();
                 munkalap.Cell("H13").Value = adat2.HazSzam;
                 munkalap.Cell("I13").Value = adat2.EmeletAjto;
                               
                 munkalap.Cell("H15").Value = adat2.AdoSzam;
                 munkalap.Cell("G16").Value = "Telefonszám:";
                 munkalap.Cell("H16").Value = adat2.TelefonSzam;
                 munkalap.Cell("G17").Value = "Bankszámlaszám";//                 
                 munkalap.Cell("A20").Value = "Teljesítés dátuma:";
                 munkalap.Cell("B20").Value = DateTime.Now;

                 munkalap.Cell("A22").Value = "Termékkategória";
                 munkalap.Cell("B22").Value = "Megnevezés";
                 munkalap.Cell("C22").Value = "Cikkszám";
                 munkalap.Cell("D22").Value = "Mennyíség";
                 munkalap.Cell("E22").Value = "Mennyíség egysége";
                 munkalap.Cell("F22").Value = "Nettó ár";
                 munkalap.Cell("G22").Value = "Bruttó ár";
                 munkalap.Cell("H22").Value = "Áfa kulcs";
                 munkalap.Cell("I22").Value = "Vtsz/Szj/Teszor"; munkalap.Cell($"G19").Value = "Kedvezmény:";
                munkalap.Cell($"H19").Value = $"{adat2.Kedvezmeny} %";
                munkalap.Cell($"G20").Value = "Végösszeg:";
                munkalap.Cell($"H20").Value = $"{ar} Ft.";
                munkalap.Columns().AdjustToContents();
                munkalap.Rows().AdjustToContents();

                for (int i = 0; i < termekek.Count; i++)
                {
                    for (int j = 24; j < termekek.Count + 24; j++, i++)
                    {

                        munkalap.Cell($"A{j}").Value = termekek[i].TermekKategoria;
                        munkalap.Cell($"B{j}").Value = termekek[i].Megnevezes;
                        munkalap.Cell($"C{j}").Value = termekek[i].Cikkszam;
                        munkalap.Cell($"D{j}").Value = termekek[i].Mennyiseg;
                        munkalap.Cell($"E{j}").Value = termekek[i].MennyisegEgysege;
                        munkalap.Cell($"F{j}").Value = termekek[i].NettoAr;
                        munkalap.Cell($"G{j}").Value = termekek[i].BruttoAr;
                        munkalap.Cell($"H{j}").Value = termekek[i].AfaKulcs;
                        munkalap.Cell($"I{j}").Value = termekek[i].NavSorszam;

                    }
                }
              
                //FORMÁZÁS
                IXLRange range;
                range = munkalap.Range(munkalap.Cell("A2").Address, munkalap.Cell("I4").Address);
                range.Style.Fill.SetBackgroundColor(XLColor.FromArgb(220, 230, 241));
                range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
               for (int i = 5; i < termekek.Count+21; i++)
                {
                    range = munkalap.Range(munkalap.Cell("A5").Address, munkalap.Cell("I28").Address);//Fő háttér
                    range.Style.Fill.SetBackgroundColor(XLColor.FromArgb(197, 217, 241));
                    range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                }
               

                //paragraphs
                range = munkalap.Range(munkalap.Cell("A6").Address, munkalap.Cell("C6").Address);
                range.Style.Fill.SetBackgroundColor(XLColor.FromArgb(220, 230, 241));
                range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                range = munkalap.Range(munkalap.Cell("A8").Address, munkalap.Cell("C13").Address);
                range.Style.Fill.SetBackgroundColor(XLColor.FromArgb(220, 230, 241));
                range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                range = munkalap.Range(munkalap.Cell("A15").Address, munkalap.Cell("C20").Address);//Megrendelő
                range.Style.Fill.SetBackgroundColor(XLColor.FromArgb(220, 230, 241));
                range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                range = munkalap.Range(munkalap.Cell("G6").Address, munkalap.Cell("I6").Address);
                range.Style.Fill.SetBackgroundColor(XLColor.FromArgb(220, 230, 241));
                range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                range = munkalap.Range(munkalap.Cell("G8").Address, munkalap.Cell("I13").Address);
                range.Style.Fill.SetBackgroundColor(XLColor.FromArgb(220, 230, 241));
                range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                range = munkalap.Range(munkalap.Cell("G15").Address, munkalap.Cell("I20").Address);//Vevő
                range.Style.Fill.SetBackgroundColor(XLColor.FromArgb(220, 230, 241));
                range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                range = munkalap.Range(munkalap.Cell("A22").Address, munkalap.Cell("I23").Address);//Vevő
                range.Style.Fill.SetBackgroundColor(XLColor.FromArgb(220, 230, 241));
                range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                for (int i=26; i < termekek.Count+26; i++)
                {
                   
                        range = munkalap.Range(munkalap.Cell("A24").Address, munkalap.Cell($"I{i}").Address);
                        range.Style.Fill.SetBackgroundColor(XLColor.FromArgb(220, 230, 241));  //   range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                }
       
                       
                using (SaveFileDialog mentes = new SaveFileDialog())
                {
                    mentes.Filter = "excelFile|*.xlsx";
                    if (mentes.ShowDialog() == DialogResult.OK)
                    {

                        if (FileEllenorzo(mentes.FileName) == true)
                        {
                            excel.SaveAs(mentes.FileName);
                        }
                    }
                }             
            }
        }
        public static void ExcelTermekmentes(List<Termek> termekek)
        {
            using (XLWorkbook excel = new XLWorkbook())

            {
                IXLWorksheet munkalap = excel.AddWorksheet();



                munkalap.Name = "Terméklista";
                munkalap.Cell("A2").Value = "Terméklista:";
                munkalap.Cell("A4").Value = "Termékkategória";
                munkalap.Cell("B4").Value = "Megnevezés";//+2              
                munkalap.Cell("C4").Value = "Cikkszám";
                munkalap.Cell("D4").Value = "Mennyíség";
                munkalap.Cell("E4").Value = "Mennyíség egysége";
                munkalap.Cell("F4").Value = "Nettó ár";
                munkalap.Cell("G4").Value = "Bruttó ár";
                munkalap.Cell("H4").Value = "Áfa kulcs";
                munkalap.Cell("I4").Value = "Vtsz/Szj/Teszor";



                for (int i = 0; i < termekek.Count; i++)
                {
                    for (int j = 6; j < termekek.Count + 6; j++, i++)
                    {
                        munkalap.Cell($"A{j}").Value = termekek[i].TermekKategoria;
                        munkalap.Cell($"B{j}").Value = termekek[i].Megnevezes;
                        munkalap.Cell($"C{j}").Value = termekek[i].Cikkszam;
                        munkalap.Cell($"D{j}").Value = termekek[i].Mennyiseg;
                        munkalap.Cell($"E{j}").Value = termekek[i].MennyisegEgysege;
                        munkalap.Cell($"F{j}").Value = termekek[i].NettoAr;
                        munkalap.Cell($"G{j}").Value = termekek[i].BruttoAr;
                        munkalap.Cell($"H{j}").Value = termekek[i].AfaKulcs;
                        munkalap.Cell($"I{j}").Value = termekek[i].NavSorszam;
                      

                    }
                }
                munkalap.Columns().AdjustToContents();
                munkalap.Rows().AdjustToContents();
                using (SaveFileDialog mentes = new SaveFileDialog())
                {
                    mentes.Filter = "excelFile|*.xlsx";
                    if (mentes.ShowDialog() == DialogResult.OK)
                    {
                        excel.SaveAs(mentes.FileName);
                    }
                }
            }
        }
        public static void ExcelHianylistamentes(List<Termek> termekek)
        {
            int hiany = 0;
            using (XLWorkbook excel = new XLWorkbook())

            {
                IXLWorksheet munkalap = excel.AddWorksheet();



                munkalap.Name = "Hiánylista";
             
                munkalap.Cell("A2").Value = "Hiánylista:";
                munkalap.Cell("A4").Value = "Termékkategória";
                munkalap.Cell("B4").Value = "Megnevezés";//+2              
                munkalap.Cell("C4").Value = "Cikkszám";
                munkalap.Cell("D4").Value = "Mennyíség";
                munkalap.Cell("E4").Value = "Mennyíség egysége";
                munkalap.Cell("F4").Value = "Nettó ár";
                munkalap.Cell("G4").Value = "Bruttó ár";
                munkalap.Cell("H4").Value = "Áfa kulcs";
                munkalap.Cell("I4").Value = "Vtsz/Szj/Teszor";
                munkalap.Cell("J4").Value = "Minimális készlet";
                munkalap.Cell("K4").Value = "Rendelendő";



                for (int i = 0; i < termekek.Count; i++)
                {
                    for (int j = 6; j < termekek.Count + 6; j++,i++)
                    {
                        hiany= termekek[i].MinimalisKeszlet - termekek[i].Mennyiseg;

                        munkalap.Cell($"A{j}").Value = termekek[i].TermekKategoria;
                        munkalap.Cell($"B{j}").Value = termekek[i].Megnevezes;
                        munkalap.Cell($"C{j}").Value = termekek[i].Cikkszam;
                        munkalap.Cell($"D{j}").Value = termekek[i].Mennyiseg;
                        munkalap.Cell($"E{j}").Value = termekek[i].MennyisegEgysege;
                        munkalap.Cell($"F{j}").Value = termekek[i].NettoAr;
                        munkalap.Cell($"G{j}").Value = termekek[i].BruttoAr;
                        munkalap.Cell($"H{j}").Value = termekek[i].AfaKulcs;
                        munkalap.Cell($"I{j}").Value = termekek[i].NavSorszam;
                        munkalap.Cell($"J{j}").Value = termekek[i].MinimalisKeszlet;
                        munkalap.Cell($"K{j}").Value = hiany;
                    }
                }
                munkalap.Columns().AdjustToContents();
                munkalap.Rows().AdjustToContents();
                using (SaveFileDialog mentes = new SaveFileDialog())
                {
                    mentes.Filter = "excelFile|*.xlsx";
                    if (mentes.ShowDialog() == DialogResult.OK)
                    {
                        if (FileEllenorzo(mentes.FileName) == true)
                        {
                            excel.SaveAs(mentes.FileName);
                        }
                    }
                }
            }
        }

        public static void ExcelPartnerekmentes(List<Partner> partnerek)
        {
            using (XLWorkbook excel = new XLWorkbook())

            {
                IXLWorksheet munkalap = excel.AddWorksheet();



                munkalap.Name = "Partnerlista";
                munkalap.Cell("A2").Value = "Partnerek:";
                munkalap.Cell("A4").Value = "Titulus";
                munkalap.Cell("B4").Value = "Vezetéknév";//+2              
                munkalap.Cell("C4").Value = "Keresztnév";
                munkalap.Cell("D4").Value = "Utónév";
                munkalap.Cell("E4").Value = "Cégnév";
                munkalap.Cell("F2").Value = "Számlázási cím";
                munkalap.Cell("F4").Value = "Ország";
                munkalap.Cell("G4").Value = "Irányítószám";
                munkalap.Cell("H4").Value = "Település";
                munkalap.Cell("I4").Value = "Közterület";
                munkalap.Cell("J4").Value = "közterület jellege";
                munkalap.Cell("K4").Value = "Házszám";
                munkalap.Cell("L4").Value = "Emelet/ajtó";
                munkalap.Cell("M2").Value = "Póstázási cím";
                munkalap.Cell("M4").Value = "Ország";
                munkalap.Cell("N4").Value = "Irányítószám";
                munkalap.Cell("O4").Value = "Település";
                munkalap.Cell("P4").Value = "Közterület";
                munkalap.Cell("Q4").Value = "közterület jellege";
                munkalap.Cell("R4").Value = "Házszám";
                munkalap.Cell("S4").Value = "Emelet/ajtó";
                munkalap.Cell("T4").Value = "Telefonszám";
                munkalap.Cell("U4").Value = "Fax";
                munkalap.Cell("V4").Value = "E-mail cím";
                munkalap.Cell("W4").Value = "Weboldal cím";
                munkalap.Cell("X4").Value = "Bankszámlaszám,";
                munkalap.Cell("Y4").Value = "Weboldal címe:";
                munkalap.Cell("Z4").Value = "Partnercsport";
                munkalap.Cell("AA4").Value = "Fizetési mód";
                munkalap.Cell("AB4").Value = "Kedvezmény";
                munkalap.Cell("AC4").Value = "Számla megjegyzés";
                munkalap.Cell("AD4").Value = "Partner megjegyzés";


                for (int i = 0; i < partnerek.Count; i++)
                {
                    for (int j = 6; j < partnerek.Count + 6; j++, i++)
                    {
                        munkalap.Cell($"A{j}").Value = partnerek[i].Titulus;
                        munkalap.Cell($"B{j}").Value = partnerek[i].VezetekNev;
                        munkalap.Cell($"C{j}").Value = partnerek[i].KeresztNev;
                        munkalap.Cell($"D{j}").Value = partnerek[i].UtoNev;
                        munkalap.Cell($"E{j}").Value = partnerek[i].CegNev;
                        munkalap.Cell($"F{j}").Value = partnerek[i].Orszag;
                        munkalap.Cell($"G{j}").Value = partnerek[i].Irsz;
                        munkalap.Cell($"H{j}").Value = partnerek[i].Telepules;
                        munkalap.Cell($"I{j}").Value = partnerek[i].Kozterulet;
                        munkalap.Cell($"J{j}").Value = partnerek[i].KozteruletJelleg;
                        munkalap.Cell($"K{j}").Value = partnerek[i].HazSzam;
                        munkalap.Cell($"L{j}").Value = partnerek[i].EmeletAjto;
                        munkalap.Cell($"M{j}").Value = partnerek[i].PostaOrszag;
                        munkalap.Cell($"N{j}").Value = partnerek[i].PostaIrsz;
                        munkalap.Cell($"O{j}").Value = partnerek[i].PostaTelepules;
                        munkalap.Cell($"P{j}").Value = partnerek[i].PostaKozterulet;
                        munkalap.Cell($"Q{j}").Value = partnerek[i].PostaKozteruletJelleg;
                        munkalap.Cell($"R{j}").Value = partnerek[i].PostaHazSzam;
                        munkalap.Cell($"S{j}").Value = partnerek[i].PostaEmeletAjto;
                        munkalap.Cell($"T{j}").Value = partnerek[i].TelefonSzam;
                        munkalap.Cell($"U{j}").Value = partnerek[i].Fax;
                        munkalap.Cell($"V{j}").Value = partnerek[i].Email;
                        munkalap.Cell($"W{j}").Value = partnerek[i].WebCim;
                        munkalap.Cell($"X{j}").Value = partnerek[i].BankSzamlaSzam;
                        munkalap.Cell($"Y{j}").Value = partnerek[i].PartnerCsoport;
                        munkalap.Cell($"Z{j}").Value = partnerek[i].FizetesiMod;
                        munkalap.Cell($"AA{j}").Value = partnerek[i].Kedvezmeny;
                        munkalap.Cell($"AB{j}").Value = partnerek[i].SzamlaMegjegyzes;
                        munkalap.Cell($"AC{j}").Value = partnerek[i].PartnerMegjegyzes;
                     
                       
                    }
                }
                munkalap.Columns().AdjustToContents();
                munkalap.Rows().AdjustToContents();
                using (SaveFileDialog mentes = new SaveFileDialog())
                {
                    mentes.Filter = "excelFile|*.xlsx";
                    if (mentes.ShowDialog() == DialogResult.OK)
                    {
                        if (FileEllenorzo(mentes.FileName) == true)
                        {
                            excel.SaveAs(mentes.FileName);
                        }
                    }
                }
            }
        }
        #endregion
        #region ExcelBevetelezesMentes
        public static void ExcelbevetelezesMentes(Partner adat1, Partner adat2, List<Termek> termekek, Bevetelezes bevetelezes)
        {
            int l = 0;

            using (XLWorkbook excel = new XLWorkbook())

            {
                IXLWorksheet munkalap = excel.AddWorksheet();
                decimal ar = 0;

                int k = termekek.Count + 26;

                for (int i = 0; i < termekek.Count; i++)
                {
                    ar += termekek[i].BruttoAr;
                }


                munkalap.Name = "Bevételezés";
                munkalap.Cell("A2").Value = "Bevételezési Bizonylat";
                munkalap.Cell("A4").Value = "Bevételezés azonosító:";
                munkalap.Cell("B4").Value = bevetelezes.BevetId;
                munkalap.Cell("A6").Value = "   Eladó adatai:";//+2

                munkalap.Cell("A8").Value = "Cégnév";//+2
                if (adat2.CegNev == "Magánszemély")
                {

                    munkalap.Cell("G15").Value = "Adóazonosító jel:";
                }
                else
                {

                    munkalap.Cell("G15").Value = "Adószám:";
                }
                munkalap.Cell("B8").Value = adat1.CegNev;
                munkalap.Cell("A9").Value = "kapcsolattartó";
                munkalap.Cell("B9").Value = adat1.VezetekNev;
                munkalap.Cell("C9").Value = adat1.KeresztNev;
                munkalap.Cell("A10").Value = "Cím:";
                munkalap.Cell("A12").Value = adat1.Irsz;

                munkalap.Cell("B12").Value = adat1.Telepules;
                munkalap.Cell("C12").Value = adat1.Kozterulet;
                munkalap.Cell("A13").Value = adat1.KozteruletJelleg.ToString();
                munkalap.Cell("B13").Value = adat1.HazSzam;
                munkalap.Cell("C13").Value = adat1.EmeletAjto;
                munkalap.Cell("A15").Value = "Adószám:";
                munkalap.Cell("B15").Value = adat1.AdoSzam;
                munkalap.Cell("A16").Value = "Telefonszám:";
                munkalap.Cell("B16").Value = adat1.TelefonSzam;
                munkalap.Cell("A17").Value = "Bankszámlaszám:";
                munkalap.Cell("B17").Value = adat1.BankSzamlaSzam;
                munkalap.Cell("A19").Value = "Megrendelésszám:";
                munkalap.Cell("B19").Value = bevetelezes.MegrendelesSzam;
                munkalap.Cell("G4").Value = "Kiállítás dátuma:";
                munkalap.Cell("H4").Value = bevetelezes.BizonylatKelte;
                munkalap.Cell("G6").Value = "Vevő adatai:";
                munkalap.Cell("G8").Value = "Cégnév";
                munkalap.Cell("H8").Value = adat2.CegNev;
                munkalap.Cell("G9").Value = "kapcsolattartó";
                munkalap.Cell("H9").Value = adat2.VezetekNev;
                munkalap.Cell("I9").Value = adat2.KeresztNev;
                munkalap.Cell("G10").Value = "Cím:";
                munkalap.Cell("G12").Value = adat2.Irsz;
                munkalap.Cell("H12").Value = adat2.Telepules;
                munkalap.Cell("I12").Value = adat2.Kozterulet;
                munkalap.Cell("G13").Value = adat2.KozteruletJelleg.ToString();
                munkalap.Cell("H13").Value = adat2.HazSzam;
                munkalap.Cell("I13").Value = adat2.EmeletAjto;

                munkalap.Cell("H15").Value = adat2.AdoSzam;
                munkalap.Cell("G16").Value = "Telefonszám:";
                munkalap.Cell("H16").Value = adat2.TelefonSzam;
                munkalap.Cell("G17").Value = "Bankszámlaszám";//                 
                munkalap.Cell("A20").Value = "Teljesítés dátuma:";
                munkalap.Cell("B20").Value = DateTime.Now;

                munkalap.Cell("A22").Value = "Termékkategória";
                munkalap.Cell("B22").Value = "Megnevezés";
                munkalap.Cell("C22").Value = "Cikkszám";
                munkalap.Cell("D22").Value = "Mennyíség";
                munkalap.Cell("E22").Value = "Mennyíség egysége";
                munkalap.Cell("F22").Value = "Nettó ár";
                munkalap.Cell("G22").Value = "Bruttó ár";
                munkalap.Cell("H22").Value = "Áfa kulcs";
                munkalap.Cell("I22").Value = "Vtsz/Szj/Teszor"; 
               
                munkalap.Cell($"G20").Value = "Végösszeg:";
                munkalap.Cell($"H20").Value = $"{ar} Ft.";
                munkalap.Columns().AdjustToContents();
                munkalap.Rows().AdjustToContents();

                for (int i = 0; i < termekek.Count; i++)
                {
                    for (int j = 24; j < termekek.Count + 24; j++, i++)
                    {

                        munkalap.Cell($"A{j}").Value = termekek[i].TermekKategoria;
                        munkalap.Cell($"B{j}").Value = termekek[i].Megnevezes;
                        munkalap.Cell($"C{j}").Value = termekek[i].Cikkszam;
                        munkalap.Cell($"D{j}").Value = termekek[i].Mennyiseg;
                        munkalap.Cell($"E{j}").Value = termekek[i].MennyisegEgysege;
                        munkalap.Cell($"F{j}").Value = termekek[i].NettoAr;
                        munkalap.Cell($"G{j}").Value = termekek[i].BruttoAr;
                        munkalap.Cell($"H{j}").Value = termekek[i].AfaKulcs;
                        munkalap.Cell($"I{j}").Value = termekek[i].NavSorszam;

                    }
                }

                //FORMÁZÁS
                IXLRange range;
                range = munkalap.Range(munkalap.Cell("A2").Address, munkalap.Cell("I4").Address);
                range.Style.Fill.SetBackgroundColor(XLColor.FromArgb(220, 230, 241));
                range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                for (int i = 5; i < termekek.Count + 21; i++)
                {
                    range = munkalap.Range(munkalap.Cell("A5").Address, munkalap.Cell("I28").Address);//Fő háttér
                    range.Style.Fill.SetBackgroundColor(XLColor.FromArgb(197, 217, 241));
                    range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                }


                //paragraphs
                range = munkalap.Range(munkalap.Cell("A6").Address, munkalap.Cell("C6").Address);
                range.Style.Fill.SetBackgroundColor(XLColor.FromArgb(220, 230, 241));
                range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                range = munkalap.Range(munkalap.Cell("A8").Address, munkalap.Cell("C13").Address);
                range.Style.Fill.SetBackgroundColor(XLColor.FromArgb(220, 230, 241));
                range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                range = munkalap.Range(munkalap.Cell("A15").Address, munkalap.Cell("C20").Address);//Megrendelő
                range.Style.Fill.SetBackgroundColor(XLColor.FromArgb(220, 230, 241));
                range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                range = munkalap.Range(munkalap.Cell("G6").Address, munkalap.Cell("I6").Address);
                range.Style.Fill.SetBackgroundColor(XLColor.FromArgb(220, 230, 241));
                range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                range = munkalap.Range(munkalap.Cell("G8").Address, munkalap.Cell("I13").Address);
                range.Style.Fill.SetBackgroundColor(XLColor.FromArgb(220, 230, 241));
                range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                range = munkalap.Range(munkalap.Cell("G15").Address, munkalap.Cell("I20").Address);//Vevő
                range.Style.Fill.SetBackgroundColor(XLColor.FromArgb(220, 230, 241));
                range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                range = munkalap.Range(munkalap.Cell("A22").Address, munkalap.Cell("I23").Address);//Vevő
                range.Style.Fill.SetBackgroundColor(XLColor.FromArgb(220, 230, 241));
                range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                for (int i = 26; i < termekek.Count + 26; i++)
                {

                    range = munkalap.Range(munkalap.Cell("A24").Address, munkalap.Cell($"I{i}").Address);
                    range.Style.Fill.SetBackgroundColor(XLColor.FromArgb(220, 230, 241));  //   range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                }
              

                using (SaveFileDialog mentes = new SaveFileDialog())
                {
                    mentes.Filter = "excelFile|*.xlsx";
                    if (mentes.ShowDialog() == DialogResult.OK)
                    {


                        if (FileEllenorzo(mentes.FileName)==true)
                        {
                            excel.SaveAs(mentes.FileName);
                        }
                        

                        
                    }
                }
            }
        }
        #region FileEllenorzo
        public static bool FileEllenorzo(string check)
        {//where string check is the path of required file

            try
            {
                Stream s = File.Open(check, FileMode.Open, FileAccess.Read, FileShare.None);
                s.Close();
               
                return true;
            }
            catch (Exception)
            {
                MessageBox.Show("A kiválasztott File nyitva van, kérjük mentés előtt zárja be","Figyelem!", MessageBoxButtons.OK,MessageBoxIcon.Warning);

                return false;
            }
        }
        #endregion
        #endregion
        #region TermekHianyLVFeltoltes
        public static void TermekHianyLVFeltoltes(ListView lv)
        {

          
                //lV beállítások

                lv.Columns.Clear();
                lv.Items.Clear();
                lv.View = View.Details;


                using (SqlConnection kapcsolat = new SqlConnection(cs))
                {
                try
                {
                    kapcsolat.Open();
                    using (SqlCommand Parancs = new SqlCommand())
                    {
                        Parancs.Connection = kapcsolat;
                        Parancs.Parameters.Clear();
                        Parancs.CommandText = "SELECT * FROM [Termekek] WHERE [Statusz]=@statusz AND [Mennyiseg]<[MinimalisKeszlet] ORDER BY [Mennyiseg] DESC";
                        Parancs.Parameters.AddWithValue("@statusz", 1);
                        using (SqlDataReader reader = Parancs.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (lv.Columns.Count == 0) 
                                {
                                    string[] oszlopnevek =
                                     {
                                        "Termékazonosító","Termékkategória","Megnevezés","Mennyíség","Mennyíség egysége","nettó ár","Bruttó ár","ÁFA kulcs","Cikkszám","Vtsz/Szj/Teszor",
                                        "Minimális készlet","Rendelendő"
                                    };
                                    for (int i = 0; i < oszlopnevek.Length; i++)
                                    {
                                        lv.Columns.Add(oszlopnevek[i]); 
                                    }
                                }
                                string[] adatok = new string[reader.FieldCount]; 
                                for (int i = 0; i < reader.FieldCount - 1; i++)
                                {
                                    adatok[i] = reader[i].ToString(); 
                                }
                                adatok[adatok.Length - 1] = (Convert.ToInt32(adatok[10]) - Convert.ToInt32(adatok[3])).ToString();
                                lv.Items.Add(new ListViewItem(adatok)); 
                            }
                            reader.Close();
                        }
                    }
                    kapcsolat.Close();
                }
                catch (Exception ex)
                {
                    kapcsolat.Close();
                    throw new Exception(ex.Message + "Sikertelen felolvasás!", ex);
                }
            }

        }
        #endregion
        #region TermekKategoriaKereses
        public static void TermekKategoriaKereses(ListView lv, string adat)
        {

             

                lv.Columns.Clear();
                lv.Items.Clear();
                lv.View = View.Details;


                using (SqlConnection kapcsolat = new SqlConnection(cs))
                {
                try
                {

                    kapcsolat.Open();
                    using (SqlCommand Parancs = new SqlCommand())
                    {
                        Parancs.Connection = kapcsolat;
                        Parancs.Parameters.Clear();
                        Parancs.CommandText = "SELECT * FROM [Termekek] WHERE [TermekKategoria] LIKE @en AND [Statusz]=1";
                        Parancs.Parameters.AddWithValue("@en", adat);
                        using (SqlDataReader reader = Parancs.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (lv.Columns.Count == 0) 
                                {
                                    for (int i = 0; i < reader.FieldCount - 1; i++)
                                    {
                                        lv.Columns.Add(reader.GetName(i)); 
                                    }
                                }
                                string[] adatok = new string[reader.FieldCount]; 
                                for (int i = 0; i < reader.FieldCount - 1; i++)
                                {
                                    adatok[i] = reader[i].ToString(); 
                                }
                                lv.Items.Add(new ListViewItem(adatok)); 
                            }
                            reader.Close();
                        }
                    }
                    kapcsolat.Close();
                }
                catch (Exception ex)
                {
                    kapcsolat.Close();
                    throw new Exception(ex.Message + "Sikertelen felolvasás!", ex);
                }
            }

        }
        #endregion
        #region TermekKeresesAzonositovAlapjan
        public static void TermekKeresesAzonositovAlapjan(ListView lv, int adat)
        {

              

                lv.Columns.Clear();
                lv.Items.Clear();
                lv.View = View.Details;


                using (SqlConnection kapcsolat = new SqlConnection(cs))
                {

                try
                {
                    kapcsolat.Open();
                    using (SqlCommand Parancs = new SqlCommand())
                    {
                        Parancs.Connection = kapcsolat;
                        Parancs.Parameters.Clear();
                        Parancs.CommandText = "SELECT * FROM [Termekek] WHERE [TermekAzonosito] LIKE @en AND [Statusz]=1";
                        Parancs.Parameters.AddWithValue("@en", adat);
                        using (SqlDataReader reader = Parancs.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (lv.Columns.Count == 0) 
                                {
                                    for (int i = 0; i < reader.FieldCount - 1; i++)
                                    {
                                        lv.Columns.Add(reader.GetName(i)); 
                                    }
                                }
                                string[] adatok = new string[reader.FieldCount]; 
                                for (int i = 0; i < reader.FieldCount - 1; i++)
                                {
                                    adatok[i] = reader[i].ToString(); 
                                }
                                lv.Items.Add(new ListViewItem(adatok)); 
                            }
                            reader.Close();
                        }
                    }
                    kapcsolat.Close();
                }
                catch (Exception ex)
                {
                    kapcsolat.Close();
                    throw new Exception(ex.Message + "Sikertelen felolvasás!", ex);
                }
            }

        }
        #endregion
        #region TermekKeresesCikkszamAlapjan
        public static void TermekKeresesCikkszamAlapjan(ListView lv, string adat)
        {

           
                

                lv.Columns.Clear();
                lv.Items.Clear();
                lv.View = View.Details;


                using (SqlConnection kapcsolat = new SqlConnection(cs))
                {
                try
                {
                    kapcsolat.Open();
                    using (SqlCommand Parancs = new SqlCommand())
                    {
                        Parancs.Connection = kapcsolat;
                        Parancs.Parameters.Clear();
                        Parancs.CommandText = "SELECT * FROM [Termekek] WHERE [CikkSzam] LIKE @en AND [Statusz]=1";
                        Parancs.Parameters.AddWithValue("@en", adat);
                        using (SqlDataReader reader = Parancs.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (lv.Columns.Count == 0) 
                                {
                                    for (int i = 0; i < reader.FieldCount - 1; i++)
                                    {
                                        lv.Columns.Add(reader.GetName(i)); 
                                    }
                                }
                                string[] adatok = new string[reader.FieldCount]; 
                                for (int i = 0; i < reader.FieldCount - 1; i++)
                                {
                                    adatok[i] = reader[i].ToString(); 
                                }
                                lv.Items.Add(new ListViewItem(adatok)); 
                            }
                            reader.Close();
                        }
                    }
                    kapcsolat.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + "Sikertelen felolvasás!", ex);
                }
            }

        }
        #endregion
        #region TermekKeresesMegnevezesAlapjan
        public static void TermekKeresesMegnevezesAlapjan(ListView lv, string adat)
        {

           
               

                lv.Columns.Clear();
                lv.Items.Clear();
                lv.View = View.Details;


                using (SqlConnection kapcsolat = new SqlConnection(cs))
                {
                try
                {
                    kapcsolat.Open();
                    using (SqlCommand Parancs = new SqlCommand())
                    {
                        Parancs.Connection = kapcsolat;
                        Parancs.Parameters.Clear();
                        Parancs.CommandText = "SELECT * FROM [Termekek] WHERE [Megnevezes] LIKE @en AND [Statusz]=1";
                        Parancs.Parameters.AddWithValue("@en", adat);
                        using (SqlDataReader reader = Parancs.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (lv.Columns.Count == 0) 
                                {
                                    for (int i = 0; i < reader.FieldCount - 1; i++)
                                    {
                                        lv.Columns.Add(reader.GetName(i)); 
                                    }
                                }
                                string[] adatok = new string[reader.FieldCount]; 
                                for (int i = 0; i < reader.FieldCount - 1; i++)
                                {
                                    adatok[i] = reader[i].ToString(); 
                                }
                                lv.Items.Add(new ListViewItem(adatok)); 
                            }
                            reader.Close();
                        }
                    }
                    kapcsolat.Close();
                }
                catch (Exception ex)
                {
                    kapcsolat.Close();
                    throw new Exception(ex.Message + "Sikertelen felolvasás!", ex);
                }
            }

        }
        #endregion
        #region TermekOsszesKereses
        public static void TermekOsszesKereses(ListView lv, string adat)
        {

            
               

                lv.Columns.Clear();
                lv.Items.Clear();
                lv.View = View.Details;


                using (SqlConnection kapcsolat = new SqlConnection(cs))
                {
                try
                {
                    kapcsolat.Open();
                    using (SqlCommand Parancs = new SqlCommand())
                    {
                        Parancs.Connection = kapcsolat;
                        Parancs.Parameters.Clear();
                        Parancs.CommandText = ("SELECT [TermekKategoria],[Megnevezes],[Mennyiseg],[MennyisegEgysege],[NettoAr],[BruttoAr],[AfaKulcs],[CikkSzam],[NavSorSzam],[MinimalisKeszlet]" +
                            "   FROM [Termekek] WHERE [TermekKategoria]=@en OR [Megnevezes]=@en OR [Mennyiseg]=@en OR [MennyisegEgysege]=@en OR [NettoAr]=@en OR [BruttoAr]=en OR [AfaKulcs]=@en OR" +
                            "[CikkSzam]=@en OR [NavSorSzam]=@en OR [AfaKulcs]=@en");
                        Parancs.Parameters.AddWithValue("@en", adat);
                        using (SqlDataReader reader = Parancs.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (lv.Columns.Count == 0) 
                                {
                                    for (int i = 0; i < reader.FieldCount - 1; i++)
                                    {
                                        lv.Columns.Add(reader.GetName(i)); 
                                    }
                                }
                                string[] adatok = new string[reader.FieldCount]; 
                                for (int i = 0; i < reader.FieldCount - 1; i++)
                                {
                                    adatok[i] = reader[i].ToString(); 
                                }
                                lv.Items.Add(new ListViewItem(adatok)); 
                            }
                            reader.Close();
                        }
                    }
                    kapcsolat.Close();
                }
                catch (Exception ex)
                {
                    kapcsolat.Close();
                    throw new Exception(ex.Message + "Sikertelen felolvasás!", ex);
                }
            }

        }
        #endregion
        #region PartnerKeresAdoSzamAlapjan
        public static void PartnerKeresAdoSzamAlapjan(ListView lv, int adat)
        {

          
              

                lv.Columns.Clear();
                lv.Items.Clear();
                lv.View = View.Details;


                using (SqlConnection kapcsolat = new SqlConnection(cs))
                {
                try
                {
                    kapcsolat.Open();
                    using (SqlCommand Parancs = new SqlCommand())
                    {
                        Parancs.Connection = kapcsolat;
                        Parancs.Parameters.Clear();
                        Parancs.CommandText = "SELECT * FROM [Partner] WHERE [AdoSzam] LIKE @en";
                        Parancs.Parameters.AddWithValue("@en", adat);
                        using (SqlDataReader reader = Parancs.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (lv.Columns.Count == 0) 
                                {
                                    for (int i = 0; i < reader.FieldCount - 1; i++)
                                    {
                                        lv.Columns.Add(reader.GetName(i)); 
                                    }
                                }
                                string[] adatok = new string[reader.FieldCount]; 
                                for (int i = 0; i < reader.FieldCount - 1; i++)
                                {
                                    adatok[i] = reader[i].ToString(); 
                                }
                                lv.Items.Add(new ListViewItem(adatok)); 
                            }
                            reader.Close();
                        }
                    }
                    kapcsolat.Close();
                }
                catch (Exception ex)
                {
                    kapcsolat.Close();
                    throw new Exception(ex.Message + "Sikertelen felolvasás!", ex);
                }
            }

        }
        #endregion
        #region SzallitolevelKeresesAzonositovAlapjan
        public static void SzallitolevelKeresesAzonositovAlapjan(ListView lv, int adat)
        {

          
                

                lv.Columns.Clear();
                lv.Items.Clear();
                lv.View = View.Details;


                using (SqlConnection kapcsolat = new SqlConnection(cs))
                {
                try
                {
                    kapcsolat.Open();
                    using (SqlCommand Parancs = new SqlCommand())
                    {
                        Parancs.Connection = kapcsolat;
                        Parancs.CommandText = "SELECT * FROM [SzallitoLevel] WHERE [SzallitoId] LIKE @en";
                        Parancs.Parameters.AddWithValue("@en", adat);
                        using (SqlDataReader reader = Parancs.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (lv.Columns.Count == 0) 
                                {
                                    for (int i = 0; i < reader.FieldCount - 1; i++)
                                    {
                                        lv.Columns.Add(reader.GetName(i)); 
                                    }
                                }
                                string[] adatok = new string[reader.FieldCount]; 
                                for (int i = 0; i < reader.FieldCount - 1; i++)
                                {
                                    adatok[i] = reader[i].ToString(); 
                                }
                                lv.Items.Add(new ListViewItem(adatok)); 
                            }
                            reader.Close();
                        }
                    }
                    kapcsolat.Close();
                }
                catch (Exception ex)
                {
                    kapcsolat.Close();
                    throw new Exception(ex.Message + "Sikertelen felolvasás!", ex);
                }
            }

        }
        #endregion
        #region BevetKeresesAzonositovAlapjan
        public static void BevetKeresesAzonositovAlapjan(ListView lv, int adat)
        {

            
                

                lv.Columns.Clear();
                lv.Items.Clear();
                lv.View = View.Details;


                using (SqlConnection kapcsolat = new SqlConnection(cs))
                {
                try
                {
                    kapcsolat.Open();
                    using (SqlCommand Parancs = new SqlCommand())
                    {
                        Parancs.Connection = kapcsolat;
                        Parancs.Parameters.Clear();
                        Parancs.CommandText = "SELECT * FROM [Bevetelezesek] WHERE [BevetId] LIKE @en";
                        Parancs.Parameters.AddWithValue("@en", adat);
                        using (SqlDataReader reader = Parancs.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (lv.Columns.Count == 0) 
                                {
                                    for (int i = 0; i < reader.FieldCount - 1; i++)
                                    {
                                        lv.Columns.Add(reader.GetName(i)); 
                                    }
                                }
                                string[] adatok = new string[reader.FieldCount]; 
                                for (int i = 0; i < reader.FieldCount - 1; i++)
                                {
                                    adatok[i] = reader[i].ToString(); 
                                }
                                lv.Items.Add(new ListViewItem(adatok));
                            }
                            reader.Close();
                        }
                    }
                    kapcsolat.Close();
                }
                catch (Exception ex)
                {
                    kapcsolat.Close();
                    throw new Exception(ex.Message + "Sikertelen felolvasás!", ex);
                }
            }

        }
        #endregion
        #region SzallitolevelKeresesMegrendeloSzamAlapjan
        public static void SzallitolevelKeresesMegrendeloSzamAlapjan(ListView lv, int adat)
        {

           

                lv.Columns.Clear();
                lv.Items.Clear();
                lv.View = View.Details;


                using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                try
                {

                    kapcsolat.Open();
                    using (SqlCommand Parancs = new SqlCommand())
                    {
                        Parancs.Connection = kapcsolat;
                        Parancs.Parameters.Clear();
                        Parancs.CommandText = "SELECT * FROM [SzallitoLevel] WHERE [BizonylatSzam] LIKE @en";
                        Parancs.Parameters.AddWithValue("@en", adat);
                        using (SqlDataReader reader = Parancs.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (lv.Columns.Count == 0) 
                                {
                                    for (int i = 0; i < reader.FieldCount - 1; i++)
                                    {
                                        lv.Columns.Add(reader.GetName(i)); 
                                    }
                                }
                                string[] adatok = new string[reader.FieldCount]; 
                                for (int i = 0; i < reader.FieldCount - 1; i++)
                                {
                                    adatok[i] = reader[i].ToString(); 
                                }
                                lv.Items.Add(new ListViewItem(adatok)); 
                            }
                            reader.Close();
                        }
                    }
                    kapcsolat.Close();
                }
                catch (Exception ex)
                {
                    kapcsolat.Close();
                    throw new Exception(ex.Message + "Sikertelen felolvasás!", ex);
                }
            }

        }
        #endregion
        #region BevetKeresesMegrendeloSzamAlapjan
        public static void BevetKeresesMegrendeloSzamAlapjan(ListView lv, int adat)
        {

           
                

                lv.Columns.Clear();
                lv.Items.Clear();
                lv.View = View.Details;


                using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                try
                {
                    kapcsolat.Open();
                    using (SqlCommand Parancs = new SqlCommand())
                    {
                        Parancs.Connection = kapcsolat;
                        Parancs.Parameters.Clear();
                        Parancs.CommandText = "SELECT * FROM [Bevetelezesek] WHERE [BizonylatSzam2] LIKE @en";
                        Parancs.Parameters.AddWithValue("@en", adat);
                        using (SqlDataReader reader = Parancs.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (lv.Columns.Count == 0) 
                                {
                                    for (int i = 0; i < reader.FieldCount - 1; i++)
                                    {
                                        lv.Columns.Add(reader.GetName(i)); 
                                    }
                                }
                                string[] adatok = new string[reader.FieldCount]; 
                                for (int i = 0; i < reader.FieldCount - 1; i++)
                                {
                                    adatok[i] = reader[i].ToString(); 
                                }
                                lv.Items.Add(new ListViewItem(adatok)); 
                            }
                            reader.Close();
                        }
                    }
                    kapcsolat.Close();
                }
                catch (Exception ex)
                {
                    kapcsolat.Close();
                    throw new Exception(ex.Message + "Sikertelen felolvasás!", ex);
                }
            }

        }
        #endregion
        #region SzallitoLevelDuplikacioBizonylatAlapjan
        public static int SzallitoLevelDuplikacioBizonylatAlapjan(int adat)
        {
            int tmp = -1;
           
                using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                try
                {
                    kapcsolat.Open();
                    using (SqlCommand Parancs = new SqlCommand())
                    {

                        Parancs.Connection = kapcsolat;
                        Parancs.Parameters.Clear();
                        Parancs.CommandText = "SELECT * FROM [Szallitolevel] WHERE [BizonylatSzam] = @bizonylatszam";
                        Parancs.Parameters.AddWithValue("@bizonylatszam", adat);
                        using (SqlDataReader reader = Parancs.ExecuteReader())
                        {
                           
                            while (reader.Read())
                            {

                                tmp = reader.GetInt32(3);

                            }
                            reader.Close();
                        }
                    }
                    kapcsolat.Close();
                }
                catch (Exception ex)
                {
                    kapcsolat.Close();
                    throw new Exception(ex.Message + "A szállítólevél, megrendelőszám alapján már szerepel az adatbázisban!", ex);
                }
            }
           
            return tmp;
        }
        #endregion
        #region AdoszamDuplikacio
        public static string AdoszamDuplikacio(string adat)
        {
            string tmp ="";
           
                using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                try
                {
                    kapcsolat.Open();
                    using (SqlCommand Parancs = new SqlCommand())
                    {

                        Parancs.Connection = kapcsolat;
                        Parancs.Parameters.Clear();
                        Parancs.CommandText = "SELECT * FROM [Partner] WHERE [AdoSzam] = @adoszam ";
                        Parancs.Parameters.AddWithValue("@adoszam", adat);
                        

                        using (SqlDataReader reader = Parancs.ExecuteReader())
                        {
                            
                            if (reader.HasRows) {
                                while (reader.Read()) {

                                    tmp = reader.GetString(1);//ez az aktuális

                                }
                            }
                            reader.Close();
                        }
                    }
                    kapcsolat.Close();
                }
                catch (Exception ex)
                {
                    kapcsolat.Close();
                    throw new Exception($"Az adószám duplikáció lekérdezése az adatbázisból sikertelen!\n{ex.Message}");
                }
            }
            
            return tmp;
        }
        #region AFAModosithato
       /* public static List<Termek> AFAModosithato(int adat)
        {
            

            using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                try
                {
                    kapcsolat.Open();
                    using (SqlCommand Parancs = new SqlCommand())
                    {

                        Parancs.Connection = kapcsolat;
                        Parancs.Parameters.Clear();
                        Parancs.CommandText = "SELECT * FROM [Termekek] WHERE [AfaKulcs]=@afakategoriaSzam  ";
                        Parancs.Parameters.AddWithValue("@afakategoriaSzam", adat);
                       


                        using (SqlDataReader reader = Parancs.ExecuteReader())
                        {

                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {

                                    Termek.AFA tmp = new Termek.AFA();
                                    tmp.SQLID = reader.GetInt32(0);
                                    tmp.Value = Convert.ToDecimal(reader.GetInt32(1));

                                  
                                }
                            }
                            reader.Close();
                        }
                    }
                    kapcsolat.Close();
                }
                catch (Exception ex)
                {
                    kapcsolat.Close();
                    throw new Exception($"Az ÁFA lekérdezése lekérdezése az adatbázisból sikertelen!\n{ex.Message}");
                }
            }

            return adat2;
        }*/
        #endregion
        #endregion
        #region EUAdoszamDuplikacio
        public static string EUAdoszamDuplikacio(string adat)
        {
            string tmp = "";

            using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                try
                {
                    kapcsolat.Open();
                    using (SqlCommand Parancs = new SqlCommand())
                    {

                        Parancs.Connection = kapcsolat;
                        Parancs.Parameters.Clear();
                        Parancs.CommandText = "SELECT * FROM [Partner] WHERE [EUAdoszam] = @euAdoszam ";
                        Parancs.Parameters.AddWithValue("@euAdoszam", adat);


                        using (SqlDataReader reader = Parancs.ExecuteReader())
                        {

                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {

                                    tmp = reader.GetString(1);//ez az aktuális

                                }
                            }
                            reader.Close();
                        }
                    }
                    kapcsolat.Close();
                }
                catch (Exception ex)
                {
                    kapcsolat.Close();
                    throw new Exception($"A Szállítólevél duplikáció lekérdezése az adatbázisból sikertelen!\n{ex.Message}");
                }
            }

            return tmp;
        }
        #endregion
        #region BevetDuplikacioBizonylatAlapjan
        public static int BevetDuplikacioBizonylatAlapjan(int adat)
        {
            int tmp = -1;
           
                using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                try
                {
                    kapcsolat.Open();
                    using (SqlCommand Parancs = new SqlCommand())
                    {

                        Parancs.Connection = kapcsolat;
                        Parancs.Parameters.Clear();
                        Parancs.CommandText = "SELECT * FROM [Bevetelezesek] WHERE [BizonylatSzam2] = @bizonylatszam";
                        Parancs.Parameters.AddWithValue("@bizonylatszam", adat);
                        using (SqlDataReader reader = Parancs.ExecuteReader())
                        {
                            
                            while (reader.Read())
                            {

                                tmp = reader.GetInt32(3);

                            }
                            reader.Close();
                        }
                    }
                    kapcsolat.Close();
                }
                catch (Exception ex)
                {
                    kapcsolat.Close();
                    throw new Exception(ex.Message + "A szállítólevél, megrendelőszám alapján már szerepel az adatbázisban!", ex);
                }
            }
            
            return tmp;
        }
        #endregion
        #region SzallitolevelKeresesDatumAlapjan
        public static void SzallitolevelKeresesDatumAlapjan(ListView lv, string adat)
        {

           

                lv.Columns.Clear();
                lv.Items.Clear();
                lv.View = View.Details;


                using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                try
                {
                    kapcsolat.Open();
                    using (SqlCommand Parancs = new SqlCommand())
                    {
                        Parancs.Connection = kapcsolat;
                        Parancs.Parameters.Clear();
                        Parancs.CommandText = "SELECT * FROM [SzallitoLevel] WHERE CONTAINS ([BizonylatKelte],@en)";
                        Parancs.Parameters.Add(new SqlParameter("@en", adat));
                        using (SqlDataReader reader = Parancs.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (lv.Columns.Count == 0) 
                                {
                                    for (int i = 0; i < reader.FieldCount - 1; i++)
                                    {
                                        lv.Columns.Add(reader.GetName(i)); 
                                    }
                                }
                                string[] adatok = new string[reader.FieldCount]; 
                                for (int i = 0; i < reader.FieldCount - 1; i++)
                                {
                                    adatok[i] = reader[i].ToString(); 
                                }
                                lv.Items.Add(new ListViewItem(adatok)); 
                            }
                            reader.Close();
                        }
                    }
                    kapcsolat.Close();
                }
                catch (Exception ex)
                {
                    kapcsolat.Close();
                    throw new Exception(ex.Message + "Sikertelen felolvasás!", ex);
                }
            }

        }
        #endregion
        #region PartnerLVFeltoltes
        public static void PartnerLVFeltoltes(ListView lv)
        {
          
            
               

                lv.Columns.Clear();
                lv.Items.Clear();
                lv.View = View.Details;
                using (SqlConnection kapcsolat = new SqlConnection(cs))
                {
                try
                {
                    kapcsolat.Open();
                    using (SqlCommand Parancs = new SqlCommand())
                    {
                        Parancs.Connection = kapcsolat;
                        Parancs.Parameters.Clear();
                        Parancs.CommandText = "SELECT [Nev].Titulus,[Nev].VezetekNev, [Nev].KeresztNev,[Nev].CegNev,[Partner].PartnerCsoport,[Partner].AdoSzam,[Partner].TelefonSzam,[Partner].Fax,[Partner].Email," +
                            "[Cim].Orszag,[Cim].Irsz,[Cim].Telepules,[Cim].Kozterulet,[Cim].KozteruletJelleg,[Cim].HazSzam,[Cim].EmeletAjto FROM [Partner] RIGHT JOIN[Cim] ON[Cim].[CimId] =[Partner].[PartnerId] RIGHT JOIN[Nev] " +
                            "ON[Nev].[NevId] =[Partner].[PartnerId] WHERE[PartnerCsoport] <> '1' "; // JAJJ, DEJÓ, MŰKÖDIK, csak rosszul!! WHERE [PartnerId]=@szallitoid
                        

                        using (SqlDataReader reader = Parancs.ExecuteReader())
                        {
                          
                            while (reader.Read())
                            {


                                if (lv.Columns.Count == 0) 
                                {
                                    string[] oszlopnevek =
                                    {
                                        "Titulus","Vezetéknév","Keresztnév","Cégnév","Partnercsoport","Adószám","Telefonszám","Fax","E-mail cím","Ország",
                                        "irányítószám","Település","Közterület","Közterület jellege","Házszám","Emelet/ajtó"
                                    };
                        

                            
                                    for (int i = 0; i < oszlopnevek.Length; i++)
                                    {
                                        lv.Columns.Add(oszlopnevek[i]);
                                        
                                    }

                                }
                                string[] adatok = new string[reader.FieldCount]; 
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    adatok[i] = reader[i].ToString();
                                    if (i==13)
                                    {
                                        int enumindex = Convert.ToInt32(adatok[i]);
                                        adatok[i] = ((KozteruletJelleg)enumindex).ToString();
                                    }
                                }
                                
                                lv.Items.Add(new ListViewItem(adatok)); 
                                

                            }
                            reader.Close();
                        }
                    }
                    kapcsolat.Close();
                }
                catch (Exception ex)
                {
                    kapcsolat.Close();
                    throw new Exception(ex.Message + "Sikertelen felolvasás!", ex);
                }
            }

        }
        #endregion
        #region PartnerLVFeltoltesAdoSzamAlapjan
        public static void PartnerLVFeltoltesAdoSzamAlapjan(ListView lv, int adat2)
        {

            
               

                lv.Columns.Clear();
                lv.Items.Clear();
                lv.View = View.Details;
                using (SqlConnection kapcsolat = new SqlConnection(cs))
                {
                try
                {
                    kapcsolat.Open();
                    using (SqlCommand Parancs = new SqlCommand())
                    {
                        Parancs.Connection = kapcsolat;
                        Parancs.Parameters.Clear();
                        Parancs.CommandText = "SELECT [Nev].Titulus,[Nev].VezetekNev, [Nev].KeresztNev,[Nev].CegNev,[Partner].AdoSzam,[Partner].TelefonSzam,[Partner].Fax,[Partner].Email," +
                            "[Cim].Irsz,[Cim].Telepules,[Cim].Kozterulet,[Cim].KozteruletJelleg,[Cim].HazSzam,[Cim].EmeletAjto FROM [Partner] RIGHT JOIN[Cim] ON[Cim].[CimId] =[Partner].[PartnerId] RIGHT JOIN[Nev] ON[Nev].[NevId] =[Partner].[PartnerId] WHERE[PartnerCsoport] <>@adat AND [AdoSzam]=@adat2 "; // JAJJ, DEJÓ, MŰKÖDIK, csak rosszul!! WHERE [PartnerId]=@szallitoid
                        Parancs.Parameters.AddWithValue("@adat", '1');
                        Parancs.Parameters.AddWithValue("@adat2", adat2);

                        using (SqlDataReader reader = Parancs.ExecuteReader())
                        {
                            while (reader.Read())
                            {


                                if (lv.Columns.Count == 0) 
                                {
                                    string[] oszlopnevek =
                                    {
                                        "Vezetéknév","Keresztnév","Cégnév","Partnercsoport","Adószám","Telefonszám","Fax","E-mail cím","Ország",
                                        "irányítószám","Település","Közterület","Közterület jellege","Házszám","Emelet/ajtó"
                                    };



                                    for (int i = 0; i < oszlopnevek.Length; i++)
                                    {
                                        lv.Columns.Add(oszlopnevek[i]);
                                        
                                    }

                                }
                                string[] adatok = new string[reader.FieldCount]; 
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    adatok[i] = reader[i].ToString(); 
                                }
                                lv.Items.Add(new ListViewItem(adatok)); 

                            }
                            reader.Close();
                        }
                    }
                    kapcsolat.Close();
                }
                catch (Exception ex)
                {
                    kapcsolat.Close();
                    throw new Exception(ex.Message + "Sikertelen felolvasás!", ex);
                }
            }

        }
        #endregion
        #region PartnerListaFeltoltes
        public static Partner PartnerListaFeltoltes(int adat)
        {
            Partner tmp=null;
           
                using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                try
                {
                    kapcsolat.Open();
                    using (SqlCommand Parancs = new SqlCommand())
                    {
                       
                        Parancs.Connection = kapcsolat;
                        Parancs.Parameters.Clear();
                        Parancs.CommandText = "SELECT * FROM[Nev] LEFT JOIN[Cim] ON[Cim].[CimId] =[Nev].[NevId] LEFT JOIN[Partner] ON[Partner].[PartnerId] =[Nev].[PartnerId] WHERE[NevId] = @partnerID";
                        Parancs.Parameters.AddWithValue("@partnerID", adat);
                        using (SqlDataReader reader = Parancs.ExecuteReader())
                        {
                          
                            while (reader.Read())
                            {
                                tmp = new Partner(reader.GetInt32(6), reader.GetString(23), reader.GetString(24), reader.GetString(25),
                                      reader.GetString(26), reader.GetString(27), reader.GetString(28), reader.GetString(29), reader.GetString(30), reader.GetString(31),
                                      Convert.ToDecimal(reader.GetInt32(32)), reader.GetString(33), reader.GetString(34), reader.GetString(1), reader.GetString(2), reader.GetString(3),
                                      reader.GetString(4), reader.GetString(5), reader.GetString(8), reader.GetInt32(9).ToString(), reader.GetString(10), reader.GetString(11),
                                     (KozteruletJelleg)int.Parse(reader.GetString(12)), reader.GetInt32(13).ToString(), reader.GetString(14), reader.GetString(15), reader.GetInt32(16).ToString(), reader.GetString(17), reader.GetString(18),
                                     (KozteruletJelleg)int.Parse(reader.GetString(19)), reader.GetInt32(20).ToString(), reader.GetString(21));
                            }
                            reader.Close();
                        }
                    }
                    kapcsolat.Close();
                }
                catch (Exception ex)
                {
                    kapcsolat.Close();
                    throw new Exception(ex.Message + "Sikertelen felolvasás!", ex);
                }
            }
            
            return tmp;
        }
        #endregion
        #region PartnerListaFeltoltesAdoszamAlapjan
        public static Partner PartnerListaFeltoltesAdoszamAlapjan(int adat)
        {
            Partner tmp = null;
           
                using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                try
                {
                    kapcsolat.Open();
                    using (SqlCommand Parancs = new SqlCommand())
                    {

                        Parancs.Connection = kapcsolat;
                        Parancs.Parameters.Clear();
                        Parancs.CommandText = "SELECT * FROM[Nev] LEFT JOIN[Cim] ON[Cim].[CimId] =[Nev].[NevId] LEFT JOIN[Partner] ON[Partner].[PartnerId] =[Nev].[PartnerId] WHERE[AdoSzam] = @adoSzam";
                        Parancs.Parameters.AddWithValue("@partnerID", adat);
                        using (SqlDataReader reader = Parancs.ExecuteReader())
                        {
                            
                            while (reader.Read())
                            {
                                tmp = new Partner(reader.GetInt32(6), reader.GetString(23), reader.GetString(24), reader.GetString(25),
                                      reader.GetString(26), reader.GetString(27), reader.GetString(28), reader.GetString(29), reader.GetString(30), reader.GetString(31),
                                      Convert.ToDecimal(reader.GetInt32(32)), reader.GetString(33), reader.GetString(34), reader.GetString(1), reader.GetString(2), reader.GetString(3),
                                      reader.GetString(4), reader.GetString(5), reader.GetString(8), reader.GetInt32(9).ToString(), reader.GetString(10), reader.GetString(11),
                                     (KozteruletJelleg)int.Parse(reader.GetString(12)), reader.GetInt32(13).ToString(), reader.GetString(14), reader.GetString(15), reader.GetInt32(16).ToString(), reader.GetString(17), reader.GetString(18),
                                     (KozteruletJelleg)int.Parse(reader.GetString(19)), reader.GetInt32(20).ToString(), reader.GetString(21));
                            }
                            reader.Close();
                        }
                    }
                    kapcsolat.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + "Sikertelen felolvasás!", ex);
                }
            }
           
            return tmp;
        }
        #endregion
        #region KiadottListaFeltoltes
        public static BindingList<KiadottTermek> KiadottListaFeltoltes(int szallitoId)
        {
            KiadottTermek tmp = null;
            BindingList<KiadottTermek> kiadottLista=new BindingList<KiadottTermek>();
            
            using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                kapcsolat.Open();
                using (SqlCommand Parancs = new SqlCommand())
                {

                    Parancs.Connection = kapcsolat;
                    Parancs.Parameters.Clear();
                    Parancs.CommandText = "SELECT * FROM [KiadottTermek] WHERE[SzallitoID] = @szallitoID";
                    Parancs.Parameters.AddWithValue("@szallitoID", szallitoId);
                    using (SqlDataReader reader = Parancs.ExecuteReader())
                    {
                       
                        while (reader.Read())
                        {
                            tmp = new KiadottTermek(reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3), reader.GetDecimal(4));
                            kiadottLista.Add(tmp);
                        }
                        reader.Close();
                    }
                }
                kapcsolat.Close();
            }
           
            return kiadottLista;
        }
        #endregion
        #region SzallitoListaFeltoltes
        public static List<SzallitoLevel> SzallitoListaFeltoltes()
        {
            List<SzallitoLevel> szallito = new List<SzallitoLevel>();
           
            
            using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                kapcsolat.Open();
                using (SqlCommand Parancs = new SqlCommand())
                {

                    Parancs.Connection = kapcsolat;
                    Parancs.Parameters.Clear();
                    Parancs.CommandText = "SELECT * FROM [SzallitoLevel]";
                  
                    using (SqlDataReader reader = Parancs.ExecuteReader())
                    {
                       
                        while (reader.Read())
                        {
                            szallito.Add(new SzallitoLevel(reader.GetInt32(0),reader.GetInt32(1),reader.GetDateTime(2),reader.GetInt32(3),reader.GetInt32(4)));
                            
                        }
                        reader.Close();
                    }
                }
                kapcsolat.Close();
            }
           
            return szallito;
        }
        #endregion
        #region TermekListaFeltoltes
        public static List<Termek> TermekListaFeltoltes()
        {

            List<Termek> termekek = new List<Termek>();

           
            using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                try
                {
                    kapcsolat.Open();
                using (SqlCommand Parancs = new SqlCommand())
                {

                    Parancs.Connection = kapcsolat;
                    Parancs.Parameters.Clear();
                    Parancs.CommandText = "SELECT * FROM [Termekek] WHERE [Statusz]=@statusz";
                        Parancs.Parameters.AddWithValue("@statusz",1);

                    using (SqlDataReader reader = Parancs.ExecuteReader())
                    {
                        
                        while (reader.Read())
                        {
                                termekek.Add(new Termek(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(8), reader.GetInt32(3),
                                 reader.GetString(4), Convert.ToDecimal(reader.GetInt32(5)), Convert.ToDecimal(reader.GetInt32(6)), Convert.ToDecimal(reader.GetInt32(7)), reader.GetString(9),
                                 reader.GetInt32(10)));
                            }
                        reader.Close();
                    }
                }
                kapcsolat.Close();
            }
                catch (Exception ex)
                {
                    kapcsolat.Close();
                    throw new Exception(ex.Message + "Sikertelen felolvasás!", ex);
                }
            }
            
            return termekek;
        }


        #endregion
        #region HianylistaFeltoltes
        public static List<Termek> HianylistaFeltoltes()
        {

            List<Termek> termekek = new List<Termek>();


            using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                try
                {
                    kapcsolat.Open();
                    using (SqlCommand Parancs = new SqlCommand())
                    {

                        Parancs.Connection = kapcsolat;
                        Parancs.Parameters.Clear();
                        Parancs.CommandText = "SELECT * FROM [Termekek] WHERE [Statusz]=@statusz AND [Mennyiseg]<[MinimalisKeszlet]";
                        Parancs.Parameters.AddWithValue("@statusz", 1);

                        using (SqlDataReader reader = Parancs.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                termekek.Add(new Termek(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(8), reader.GetInt32(3),
                                 reader.GetString(4), Convert.ToDecimal(reader.GetInt32(5)), Convert.ToDecimal(reader.GetInt32(6)), Convert.ToDecimal(reader.GetInt32(7)), reader.GetString(9),
                                 reader.GetInt32(10)));
                            }
                            reader.Close();
                        }
                    }
                    kapcsolat.Close();
                }
                catch (Exception ex)
                {
                    kapcsolat.Close();
                    throw new Exception(ex.Message + "Sikertelen felolvasás!", ex);
                }
            }

            return termekek;
        }
        #endregion
        #region BevetBizonylatListaFeltoltes
        public static List<Bevetelezes> BevetBizonylatListaFeltoltes()
        {
            List<Bevetelezes> bevetBizonylat = new List<Bevetelezes>();

           
            using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                kapcsolat.Open();
                using (SqlCommand Parancs = new SqlCommand())
                {

                    Parancs.Connection = kapcsolat;
                    Parancs.Parameters.Clear();
                    Parancs.CommandText = "SELECT * FROM [Bevetelezesek]";
                    Parancs.Parameters.AddWithValue("@szallitoID", szallitoId);
                    using (SqlDataReader reader = Parancs.ExecuteReader())
                    {
                        
                        while (reader.Read())
                        {
                            bevetBizonylat.Add(new Bevetelezes(reader.GetInt32(0), reader.GetInt32(1), reader.GetDateTime(2), reader.GetInt32(3), reader.GetInt32(4)));

                        }
                        reader.Close();
                    }
                }
                kapcsolat.Close();
            }
          
            return bevetBizonylat;
        }
        #endregion
        #region BevettListaFeltoltes
        public static BindingList<BevettTermek> BevettListaFeltoltes(int bevetId)
        {
           
            BindingList<BevettTermek> bevettLista = new BindingList<BevettTermek>();
            
            using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                kapcsolat.Open();
                using (SqlCommand Parancs = new SqlCommand())
                {

                    Parancs.Connection = kapcsolat;
                    Parancs.Parameters.Clear();
                    Parancs.CommandText = "SELECT * FROM [BevettTermek] WHERE [BevetId] = @bevetId";
                    Parancs.Parameters.AddWithValue("@bevetId", bevetId);
                    using (SqlDataReader reader = Parancs.ExecuteReader())
                    {
                       
                        while (reader.Read())
                        {
                            
                            bevettLista.Add(new BevettTermek(reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3), reader.GetDecimal(4)));
                        }
                        reader.Close();
                    }
                }
                kapcsolat.Close();
            }
          
            return bevettLista;
        }
        #endregion
        #region PartnerLVFeltoltesManualisan
        public static void PartnerLVFeltoltesManualisan(ListView lv,Partner partnerek)
        {
            try
            {
                
                lv.View = View.Details;
                lv.Columns.Clear();
                if (lv.Columns.Count == 0) 
                    {
                      
                    lv.Columns.Add("Titulus");
                    lv.Columns.Add("Vezetéknév"); 
                    lv.Columns.Add("Keresztnév");

                    lv.Columns.Add("Cégnév");
                    lv.Columns.Add("Partnercsoport");
                    lv.Columns.Add("Adószám");
                    lv.Columns.Add("Telefonszám");
                    lv.Columns.Add("Fax");

                    lv.Columns.Add("E-mail cím");
                    lv.Columns.Add("Ország");
                    lv.Columns.Add("irányítószám");
                    lv.Columns.Add("Település");
                    lv.Columns.Add("Közterület");
                    lv.Columns.Add("Közterület jellege");

                    lv.Columns.Add("Házszám");
                    lv.Columns.Add("Emelet/ajtó");
                    
                 

                    

                }
                string[] adatok =
                {
                    partnerek.Titulus.ToString(),partnerek.VezetekNev.ToString(),partnerek.KeresztNev.ToString(),partnerek.CegNev.ToString(),
                    partnerek.PartnerCsoport.ToString(),partnerek.AdoSzam.ToString(),partnerek.TelefonSzam.ToString(),partnerek.Fax.ToString(),
                    partnerek.Email.ToString(),partnerek.Orszag.ToString(),partnerek.Irsz.ToString(),partnerek.Telepules.ToString(),
                    partnerek.Kozterulet.ToString(),partnerek.KozteruletJelleg.ToString(),partnerek.HazSzam.ToString(),partnerek.EmeletAjto.ToString()
                    }; 
                    
                    lv.Items.Add(new ListViewItem(adatok)); 
               
            
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Sikertelen felolvasás!", ex);
            }
            lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }
        #endregion
        #region UresPartnerLVFeltöltésManualisan
        public static void UresLVFeltoltesManualisan(ListView lv)
        {
            lv.Clear();
            //lv.Columns.Clear();
            lv.View = View.Details;
            lv.Columns.Add("");

            string[] adatok =
            {
                   "Nincs találat!"
                };

            lv.Items.Add(new ListViewItem(adatok));
            lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }
    
    #endregion
    #region TermekLVFeltoltesManualisan
    public static void TermekLVFeltoltesManualisan(ListView lv, Termek termek)
        {
            if (termek != null) {
                try {
                    lv.View = View.Details;
                    lv.Columns.Clear();
                    if (lv.Columns.Count == 0) //0 volt az UresLvFeltoltes előtt   
                    {
                        lv.Columns.Clear();
                        lv.Columns.Add("Termékazonosító");
                        lv.Columns.Add("Termékkategória");
                        lv.Columns.Add("Megnevezés");
                        lv.Columns.Add("Mennyíség");
                        lv.Columns.Add("Mennyíség egysége");
                        lv.Columns.Add("Brutto ár");
                        lv.Columns.Add("Netto ár");
                        lv.Columns.Add("Áfa kulcs");
                        lv.Columns.Add("NAV sorszám");
                        lv.Columns.Add("Cikkszám");
                        lv.Columns.Add("Minimális készlet");
                    }
                    string[] adatok =
                    {
                   termek.TermekAzonosito.ToString(),termek.TermekKategoria.ToString(),termek.Megnevezes,termek.Mennyiseg.ToString(),termek.MennyisegEgysege,termek.BruttoAr.ToString(),
                   termek.NettoAr.ToString(),termek.AfaKulcs.ToString(),termek.NavSorszam,termek.Cikkszam,termek.MinimalisKeszlet.ToString()
                    };

                    lv.Items.Add(new ListViewItem(adatok));


                } catch (Exception ex) {
                    throw new ArgumentException("Sikertelen felolvasás!", ex);
                }
                lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
        }
        #endregion
        #region SzallitoLVFeltoltesManualisan
        public static void SzallitoLVFeltoltesManualisan(ListView lv, SzallitoLevel szallito,Partner partner)
        {
            //[SzallitoId],[VezetekNev],[KeresztNev],[CegNev],[AdoSzam],[BizonylatKelte],[BizonylatSzam]
            try {
                
                lv.View = View.Details;
                lv.Columns.Clear();



                if (lv.Columns.Count == 0) 
                {

                    lv.Columns.Add("Szállítólevél azonosító");
                    lv.Columns.Add("Vezetéknév");
                    lv.Columns.Add("Keresztnév");
                    lv.Columns.Add("Cégnév");
                    lv.Columns.Add("Adószám");
                    lv.Columns.Add("Bizonylat kelte");
                    lv.Columns.Add("Bizonylatszám");
                    

                }
                string[] adatok ={
                    szallito.SzallitoId.ToString(),partner.VezetekNev,partner.KeresztNev,
                    partner.CegNev,partner.AdoSzam.ToString(),szallito.BizonylatKelte.ToString(),szallito.MegrendelesSzam.ToString()}; 

                lv.Items.Add(new ListViewItem(adatok)); 


            }
            catch (Exception ex)
            {
                throw new ArgumentException("Sikertelen felolvasás!", ex);
            }
        }
        #endregion
        #region BevetLVFeltoltesManualisan
        public static void BevetLVFeltoltesManualisan(ListView lv, Bevetelezes bevet, Partner partner)
        {
            try
            {

                lv.View = View.Details;
                lv.Columns.Clear();
                if (lv.Columns.Count == 0) {
                    lv.Columns.Add("Bevételezési bizonylat azonosító");
                    lv.Columns.Add("Vezetéknév");
                    lv.Columns.Add("Keresztnév");
                    lv.Columns.Add("Cégnév");
                    lv.Columns.Add("Adószám");
                    lv.Columns.Add("Bizonylat  kelte");
                    lv.Columns.Add("Bizonylatszám");
                }
                string[] adatok ={
                    bevet.BevetId.ToString(),partner.VezetekNev.ToString(),partner.KeresztNev.ToString(),
                    partner.CegNev.ToString(),partner.AdoSzam.ToString(),bevet.BizonylatKelte.ToString(),bevet.MegrendelesSzam.ToString()


                    }; 

                lv.Items.Add(new ListViewItem(adatok)); 


            }
            catch (Exception ex)
            {
                throw new ArgumentException("Sikertelen felolvasás!", ex);
            }
            lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }
        #endregion
        #region vanSzallitolevel
        public static bool vanSzallitolevel()
        {
            bool flag=true;
           
                using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                try
                {
                    kapcsolat.Open();
                    using (SqlCommand Parancs = new SqlCommand())
                    {
                        Parancs.Connection = kapcsolat;
                        Parancs.Parameters.Clear();
                        Parancs.CommandText = "SELECT * FROM [SzallitoLevel]"; 
                        using (SqlDataReader reader = Parancs.ExecuteReader())
                        {
                            flag = reader.HasRows;
                            reader.Close();
                        }
                    }
                    kapcsolat.Close();
                }
                catch (Exception ex)
                {
                    kapcsolat.Close();
                    throw new Exception(ex.Message + "Sikertelen felolvasás!", ex);
                }
            }
           
            return flag;
        }
        #endregion
        #region vanBevetBizonylat
        public static bool vanBevetBizonylat()
        {
            bool flag = true;
           
                using (SqlConnection kapcsolat = new SqlConnection(cs))
            {
                try
                {
                    kapcsolat.Open();
                    using (SqlCommand Parancs = new SqlCommand())
                    {
                        Parancs.Connection = kapcsolat;
                        Parancs.Parameters.Clear();
                        Parancs.CommandText = "SELECT * FROM [Bevetelezesek]"; 
                        using (SqlDataReader reader = Parancs.ExecuteReader())
                        {
                            flag = reader.HasRows;
                            reader.Close();
                        }
                    }
                    kapcsolat.Close();
                }
                catch (Exception ex)
                {
                    kapcsolat.Close();
                    throw new Exception(ex.Message + "Sikertelen felolvasás!", ex);
                }
            }
           
            return flag;
        }
        #endregion
        #region SzallitoLVFeltoltes
        public static void SzallitoLVFeltoltes(ListView lv)
        {
            if (vanSzallitolevel())
            {
               
                 

                    lv.Columns.Clear();
                    lv.Items.Clear();
                    lv.View = View.Details;
                if (lv.Columns.Count == 0) {
                    lv.Columns.Add("Szállítólevél azonosító");
                    lv.Columns.Add("Vezetéknév");
                    lv.Columns.Add("Keresztnév");
                    lv.Columns.Add("Cégnév");
                    lv.Columns.Add("Adószám");
                    lv.Columns.Add("Bizonylat kelte");
                    lv.Columns.Add("Bizonylatszám");
                }
                using (SqlConnection kapcsolat = new SqlConnection(cs))
                {
                    try
                    {
                        kapcsolat.Open();
                        using (SqlCommand Parancs = new SqlCommand())
                        {
                            Parancs.Connection = kapcsolat;
                            Parancs.Parameters.Clear();
                            Parancs.CommandText = "SELECT [SzallitoId],[VezetekNev],[KeresztNev],[CegNev],[AdoSzam],[BizonylatKelte],[BizonylatSzam] FROM [Nev] LEFT JOIN [Partner]" +
                                " ON [Partner].PartnerId=[Nev].PartnerId  LEFT JOIN [SzallitoLevel] ON [SzallitoLevel].PartnerId2=[Nev].PartnerId WHERE [Nev].PartnerId=[SzallitoLevel].PartnerId2"; // JAJJ, DEJÓ, MŰKÖDIK, csak rosszul!!
                            using (SqlDataReader reader = Parancs.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    
                                    string[] adatok = new string[reader.FieldCount]; 
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        adatok[i] = reader[i].ToString(); 
                                    }
                                    lv.Items.Add(new ListViewItem(adatok)); 
                                }
                                reader.Close();
                            }
                        }
                        kapcsolat.Close();
                    }
                    catch (Exception ex)
                    {
                        kapcsolat.Close();
                        throw new Exception(ex.Message + "Sikertelen felolvasás!", ex);
                    }
                }

            } else {
                lv.Columns.Clear();
                lv.Items.Clear();
            }
            lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }
        #endregion
        #region SzallitoLVFeltoltesAzonositoAlapjan
        public static void SzallitoLVFeltoltesAzonositoAlapjan(ListView lv,int id )
        {
            if (vanSzallitolevel())
            {
                
                    

                    lv.Columns.Clear();
                    lv.Items.Clear();
                    lv.View = View.Details;
                    using (SqlConnection kapcsolat = new SqlConnection(cs))
                    {
                    try
                    {
                        kapcsolat.Open();
                        using (SqlCommand Parancs = new SqlCommand())
                        {
                            Parancs.Connection = kapcsolat;
                            Parancs.Parameters.Clear();
                            Parancs.CommandText = "SELECT [SzallitoId],[VezetekNev],[KeresztNev],[CegNev],[AdoSzam],[BizonylatKelte],[BizonylatSzam] FROM [Nev] LEFT JOIN [Partner]" +
                                " ON [Partner].PartnerId=[Nev].PartnerId  LEFT JOIN [SzallitoLevel] ON [SzallitoLevel].PartnerId2=[Nev].PartnerId WHERE [SzallitoLevel].[SzallitoId]=@id"; // JAJJ, DEJÓ, MŰKÖDIK, csak rosszul!!
                            Parancs.Parameters.AddWithValue("@id", id);
                            using (SqlDataReader reader = Parancs.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    if (lv.Columns.Count == 0) 
                                    {
                                        for (int i = 0; i < reader.FieldCount; i++)
                                        {
                                            lv.Columns.Add(reader.GetName(i)); 
                                        }
                                    }
                                    string[] adatok = new string[reader.FieldCount]; 
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        adatok[i] = reader[i].ToString(); 
                                    }
                                    lv.Items.Add(new ListViewItem(adatok)); 
                                }
                                reader.Close();
                            }
                        }
                        kapcsolat.Close();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message + "Sikertelen felolvasás!", ex);
                    }
                }
                
            }
        }
        #endregion
        #region SzallitoLVFeltoltesPartnerIdAlapjan
        public static void SzallitoLVFeltoltesPartnerIdAlapjan(ListView lv, int id)
        {
            if (vanSzallitolevel())
            {
               
                    

                    lv.Columns.Clear();
                 
                    lv.View = View.Details;
                    using (SqlConnection kapcsolat = new SqlConnection(cs))
                {
                    try
                    {
                        kapcsolat.Open();
                        using (SqlCommand Parancs = new SqlCommand())
                        {
                            Parancs.Connection = kapcsolat;
                            Parancs.Parameters.Clear();
                            Parancs.CommandText = "SELECT [SzallitoId],[VezetekNev],[KeresztNev],[CegNev],[AdoSzam],[BizonylatKelte],[BizonylatSzam] FROM [Nev] LEFT JOIN [Partner]" +
                                " ON [Partner].PartnerId=[Nev].PartnerId  LEFT JOIN [SzallitoLevel] ON [SzallitoLevel].PartnerId2=[Nev].PartnerId WHERE [SzallitoLevel].[PartnerId2]=@id"; // JAJJ, DEJÓ, MŰKÖDIK, csak rosszul!!
                            Parancs.Parameters.AddWithValue("@id", id);
                            using (SqlDataReader reader = Parancs.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    if (lv.Columns.Count == 0) 
                                    {
                                        for (int i = 0; i < reader.FieldCount; i++)
                                        {
                                            lv.Columns.Add(reader.GetName(i)); 
                                        }
                                    }
                                    string[] adatok = new string[reader.FieldCount]; 
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        adatok[i] = reader[i].ToString(); 
                                    }
                                    lv.Items.Add(new ListViewItem(adatok)); 
                                }
                                reader.Close();
                            }
                        }
                        kapcsolat.Close();
                    }
                    catch (Exception ex)
                    {
                        kapcsolat.Close();
                        throw new Exception(ex.Message + "Sikertelen felolvasás!", ex);
                    }
                }
                
            }
        }
        #endregion
        #region BevetLVFeltoltesAzonositoAlapjan
        public static void BevetLVFeltoltesAzonositoAlapjan(ListView lv, int id)
        {
            if (vanSzallitolevel())
            {
               
                   

                    lv.Columns.Clear();
                    lv.Items.Clear();
                    lv.View = View.Details;
                    using (SqlConnection kapcsolat = new SqlConnection(cs))
                {
                    try
                    {
                        kapcsolat.Open();
                        using (SqlCommand Parancs = new SqlCommand())
                        {
                            Parancs.Connection = kapcsolat;
                            Parancs.Parameters.Clear();
                            Parancs.CommandText = "SELECT [BevetId],[VezetekNev],[KeresztNev],[CegNev],[AdoSzam],[BizonylatKelte],[BizonylatSzam] FROM [Nev] LEFT JOIN [Partner]" +
                                " ON [Partner].PartnerId=[Nev].PartnerId  LEFT JOIN [Bevetelezesek] ON [Bevetelezesek].PartnerId3=[Nev].PartnerId WHERE [Bevetelezesek].[BevetId]=@id"; // JAJJ, DEJÓ, MŰKÖDIK, csak rosszul!!
                            Parancs.Parameters.AddWithValue("@id", id);
                            using (SqlDataReader reader = Parancs.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    if (lv.Columns.Count == 0) 
                                    {
                                        for (int i = 0; i < reader.FieldCount; i++)
                                        {
                                            lv.Columns.Add(reader.GetName(i)); 
                                        }
                                    }
                                    string[] adatok = new string[reader.FieldCount]; 
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        adatok[i] = reader[i].ToString(); 
                                    }
                                    lv.Items.Add(new ListViewItem(adatok)); 
                                }
                                reader.Close();
                            }
                        }
                        kapcsolat.Close();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message + "Sikertelen felolvasás!", ex);
                    }
                }
                lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
        }
        #endregion
        #region BevetLVFeltoltesPartnerIdAlapjan
        public static void BevetLVFeltoltesPartnerIdAlapjan(ListView lv, int id)
        {
            if (vanSzallitolevel())
            {
                
                 

                    lv.Columns.Clear();
                    
                    lv.View = View.Details;
                    
                    using (SqlConnection kapcsolat = new SqlConnection(cs))
                    {
                    try
                    {
                        kapcsolat.Open();
                        using (SqlCommand Parancs = new SqlCommand())
                        {
                            Parancs.Connection = kapcsolat;
                            Parancs.Parameters.Clear();
                            Parancs.CommandText = "SELECT [BevetId],[VezetekNev],[KeresztNev],[CegNev],[AdoSzam],[BizonylatKelte],[BizonylatSzam] FROM [Nev] LEFT JOIN [Partner]" +
                                " ON [Partner].PartnerId=[Nev].PartnerId  LEFT JOIN [Bevetelezesek] ON [Bevetelezesek].PartnerId3=[Nev].PartnerId WHERE [Bevetelezesek].[PartnerId3]=@id"; // JAJJ, DEJÓ, MŰKÖDIK, csak rosszul!!
                            Parancs.Parameters.AddWithValue("@id", id);
                            using (SqlDataReader reader = Parancs.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    if (lv.Columns.Count == 0) 
                                    {
                                        for (int i = 0; i < reader.FieldCount; i++)
                                        {
                                            lv.Columns.Add(reader.GetName(i)); 
                                        }
                                    }
                                    string[] adatok = new string[reader.FieldCount]; 
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        adatok[i] = reader[i].ToString(); 
                                    }
                                    lv.Items.Add(new ListViewItem(adatok)); 
                                }
                                reader.Close();
                            }
                        }
                        kapcsolat.Close();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message + "Sikertelen felolvasás!", ex);
                    }
                }
               
            }
        }
        #endregion
        #region BevetLVFeltoltes
        public static void BevetLVFeltoltes(ListView lv)
        {
            if (vanBevetBizonylat())
            {
                lv.Columns.Clear();
                lv.Items.Clear();
                lv.View = View.Details;
                lv.Columns.Add("Bevételezési bizonylat azonosító");
                lv.Columns.Add("Vezetéknév");
                lv.Columns.Add("Keresztnév");
                lv.Columns.Add("Cégnév");
                lv.Columns.Add("Adószám");
                lv.Columns.Add("Bizonylat  kelte");
                lv.Columns.Add("Bizonylatszám");
                using (SqlConnection kapcsolat = new SqlConnection(cs))
                    {
                    try
                    {
                        kapcsolat.Open();
                        using (SqlCommand Parancs = new SqlCommand())
                        {
                            Parancs.Connection = kapcsolat;
                            Parancs.Parameters.Clear();
                            Parancs.CommandText = "SELECT [BevetId],[VezetekNev],[KeresztNev],[CegNev],[AdoSzam],[BizonylatKelte2],[BizonylatSzam2] FROM [Nev] LEFT JOIN [Partner]" +
                                " ON [Partner].PartnerId=[Nev].PartnerId  LEFT JOIN [Bevetelezesek] ON [Bevetelezesek].PartnerId3=[Nev].PartnerId WHERE [Nev].PartnerId=[Bevetelezesek].PartnerId3"; // JAJJ, DEJÓ, MŰKÖDIK, csak rosszul!!
                            using (SqlDataReader reader = Parancs.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    /*
                                    if (lv.Columns.Count == 0) 
                                    {
                                        for (int i = 0; i < reader.FieldCount; i++)
                                        {
                                            lv.Columns.Add(reader.GetName(i)); 
                                        }
                                    }
                                    */
                                    string[] adatok = new string[reader.FieldCount]; 
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        adatok[i] = reader[i].ToString(); 
                                    }
                                    lv.Items.Add(new ListViewItem(adatok)); 
                                }
                                reader.Close();
                            }
                        }
                        kapcsolat.Close();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message + "Sikertelen felolvasás!", ex);
                    }
                }

            } else {
                lv.Columns.Clear();
                lv.Items.Clear();
            }
            lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }
        #endregion
        #region ListViewToCSV
        public static void ListViewToCSV(ListView listView, string filePath, bool includeHidden)
            {
                //make header string
                StringBuilder result = new StringBuilder();
                WriteCSVRow(result, listView.Columns.Count, i => includeHidden || listView.Columns[i].Width > 0, i => listView.Columns[i].Text);

                //export data rows
                foreach (ListViewItem listItem in listView.Items)
                    WriteCSVRow(result, listView.Columns.Count, i => includeHidden || listView.Columns[i].Width > 0, i => listItem.SubItems[i].Text);
             
                File.WriteAllText(filePath, result.ToString());
        }
        #endregion
        #region WriteCSVRow
        private static void WriteCSVRow(StringBuilder result, int itemsCount, Func<int, bool> isColumnNeeded, Func<int, string> columnValue)
            {
                bool isFirstTime = true;
                for (int i = 0; i < itemsCount; i++)
                {
                    if (!isColumnNeeded(i))
                        continue;

                    if (!isFirstTime)
                        result.Append(",");
                    isFirstTime = false;

                    result.Append(String.Format("\"{0}\"", columnValue(i)));
                }
                result.AppendLine();
        }
        #endregion
        #region TermekFelvitelMySQL
      
        public static void TermekFelvitelMySQL(Termek adat) //kell termék id a táblába, ami lehet cikkszám is.
        {
            using (MySqlConnection kapcsolat = new MySqlConnection(cs2))
            {
                kapcsolat.Open();
                using (MySqlCommand Parancs = kapcsolat.CreateCommand())
                {
                    Parancs.Connection = kapcsolat;
                    Parancs.Parameters.Clear();

                    Parancs.CommandText = $" INSERT INTO Termekek  (TermekKategoria,Megnevezes,CikkSzam,Mennyiseg,MennyisegEgysege,NettoAr,BruttoAr,AfaKulcs,NavSorszam,MinimalisKeszlet,Statusz) " +
                           $"VALUES (@termekKategoria,@megnevezes,@cikkszam,@mennyiseg,@mennyisegEgysege,@nettoAr,@bruttoAr,@afaKulcs," +
                          "@NavSorszam,@minimalisKeszlet,@statusz)";

                    Parancs.Parameters.AddWithValue("@termekKategoria", adat.TermekKategoria);
                    Parancs.Parameters.AddWithValue("@megnevezes", adat.Megnevezes);
                    Parancs.Parameters.AddWithValue("@cikkSzam", adat.Cikkszam);
                    Parancs.Parameters.AddWithValue("@mennyiseg", adat.Mennyiseg);
                    Parancs.Parameters.AddWithValue("@mennyisegEgysege", adat.MennyisegEgysege);
                    Parancs.Parameters.AddWithValue("@nettoAr", adat.NettoAr);
                    Parancs.Parameters.AddWithValue("@bruttoAr", adat.BruttoAr);
                    Parancs.Parameters.AddWithValue("@afaKulcs", adat.AfaKulcs);
                    Parancs.Parameters.AddWithValue("@NavSorszam", adat.NavSorszam);
                    Parancs.Parameters.AddWithValue("@minimalisKeszlet", adat.MinimalisKeszlet);
                    Parancs.Parameters.AddWithValue("@statusz", 1);
                    Parancs.ExecuteNonQuery();

                    //adat.TermekAzonosito = (int)Parancs.ExecuteScalar();  //OUTPUT INSERTED.TermekAzonosito 
                }
                kapcsolat.Close();
            }

        }
        #endregion
        #region TermekModositasMySQL
        public static void TermekModositasMySQL(Termek adat)
        {

            using (MySqlConnection kapcsolat = new MySqlConnection(cs2))
            {
                kapcsolat.Open();
                using (MySqlCommand Parancs = new MySqlCommand())
                {
                    Parancs.Connection = kapcsolat;
                    Parancs.Parameters.Clear();
                    Parancs.CommandText = "UPDATE `Termekek` SET " +
                 "`TermekKategoria`=@termekKategoria,`Megnevezes`=@megnevezes,`Cikkszam`=@cikkSzam,`Mennyiseg`=@mennyiseg,`MennyisegEgysege`=@mennyisegEgysege,`NettoAr`=@nettoAr,`BruttoAr`=@bruttoAr,`AfaKulcs`=@afaKulcs,`NAVsorszam`=@navSorszam,`MinimalisKeszlet`=@minimalisKeszlet " +
                         "WHERE `TermekAzonosito`=@termekAzonosito";
                    Parancs.Parameters.AddWithValue("@termekAzonosito", adat.TermekAzonosito);
                    Parancs.Parameters.AddWithValue("@termekKategoria", adat.TermekKategoria);
                    Parancs.Parameters.AddWithValue("@megnevezes", adat.Megnevezes);
                    Parancs.Parameters.AddWithValue("@cikkSzam", adat.Cikkszam);
                    Parancs.Parameters.AddWithValue("@mennyiseg", adat.Mennyiseg);
                    Parancs.Parameters.AddWithValue("@mennyisegEgysege", adat.MennyisegEgysege);
                    Parancs.Parameters.AddWithValue("@nettoAr", adat.NettoAr);
                    Parancs.Parameters.AddWithValue("@bruttoAr", adat.BruttoAr);
                    Parancs.Parameters.AddWithValue("@afaKulcs", adat.AfaKulcs);
                    Parancs.Parameters.AddWithValue("@nAVsorszam", adat.NavSorszam);
                    Parancs.Parameters.AddWithValue("@minimalisKeszlet", adat.MinimalisKeszlet);
                    Parancs.ExecuteNonQuery();
                }
                kapcsolat.Close();
            }
        }

        #endregion
        #region TermekLomtarbaMySQL

        public static void TermekLomtarbaMySQL(int adat)
        {
            using (MySqlConnection kapcsolat = new MySqlConnection(cs2))
            {
                kapcsolat.Open();
                using (MySqlCommand Parancs = kapcsolat.CreateCommand())
                {
                    using (MySqlTransaction Tranzakcio = kapcsolat.BeginTransaction())
                    { //tranzakció megkezdése
                        Parancs.Connection = kapcsolat;
                        Parancs.Transaction = Tranzakcio;
                        try
                        {

                            Parancs.Parameters.Clear();
                            Parancs.CommandText = "UPDATE `Termekek` SET Statusz=@statusz WHERE `TermekAzonosito`=@termekId";
                            Parancs.Parameters.AddWithValue("@termekId", adat);
                            Parancs.Parameters.AddWithValue("@statusz", 0);

                            Parancs.ExecuteNonQuery();


                            Tranzakcio.Commit();
                        }
                        catch (Exception ex1)
                        {
                            MessageBox.Show(ex1.Message);
                            try
                            {
                                Tranzakcio.Rollback();
                            }
                            catch (Exception ex2)
                            {
                                MessageBox.Show(ex2.Message);
                            }
                        }
                    }
                }
                kapcsolat.Close();
            }
        }


        #endregion
        #region TermekLVFeltoltesMySQL

        public static void TermekLVFeltoltesMySQL(ListView lv)
        {



            lv.Columns.Clear();
            lv.Items.Clear();
            lv.View = View.Details;


            using (MySqlConnection kapcsolat = new MySqlConnection(cs2))
            {
                try
                {
                    kapcsolat.Open();
                    using (MySqlCommand Parancs = new MySqlCommand())
                    {
                        Parancs.Connection = kapcsolat;
                        Parancs.Parameters.Clear();
                        Parancs.CommandText = "SELECT * FROM Termekek WHERE Statusz=@statusz";
                        Parancs.Parameters.AddWithValue("@statusz", 1);
                        using (MySqlDataReader reader = Parancs.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (lv.Columns.Count == 0)
                                {
                                    string[] oszlopnevek =
                                     {
                                        "Termékazonosító","Termékkategória","Megnevezés","Mennyíség","Mennyíség egysége","Netto ár","Brutto ár","ÁFA kulcs","Cikkszám","Vtsz/Szj/Teszor",
                                        "Minimális készlet"
                                    };
                                    for (int i = 0; i < reader.FieldCount - 1; i++)
                                    {
                                        lv.Columns.Add(oszlopnevek[i]);
                                    }
                                }
                                string[] adatok = new string[reader.FieldCount];
                                for (int i = 0; i < reader.FieldCount - 1; i++)
                                {
                                    adatok[i] = reader[i].ToString();
                                }
                                lv.Items.Add(new ListViewItem(adatok));
                            }
                            reader.Close();
                        }
                    }
                    kapcsolat.Close();
                    TermekekFrm.HianyListaJeloles(lv, null, termekKereso());
                }

                catch (Exception ex)
                {
                    kapcsolat.Close();
                    throw new Exception(ex.Message + "Sikertelen felolvasás!", ex);
                }
            }

        }

        #endregion
        #region PartnerFelvitelMySQL

        public static void PartnerFelvitelMySQL(Partner adat)
        {
            using (MySqlConnection kapcsolat = new MySqlConnection(cs2))
            {
                kapcsolat.Open();
                using (MySqlCommand Parancs = kapcsolat.CreateCommand())
                {
                    using (MySqlTransaction Tranzakcio = kapcsolat.BeginTransaction())
                    {
                        Parancs.Connection = kapcsolat;
                        Parancs.Transaction = Tranzakcio;
                        try
                        {

                            Parancs.Parameters.Clear();
                            Parancs.CommandText = " INSERT INTO Partner  (AdoSzam,EuadoSzam,TelefonSzam,Fax,Email,WebCim,BankSzamlaSzam,PartnerCsoport,FizetesiMod,Kedvezmeny,SzamlaMegjegyzes,PartnerMegjegyzes) " +
                                " VALUES(@adoSzam,@EuadoSzam,@telefonSzam,@fax,@email,@webCim,@bankszamlaSzam,@partnerCsoport," +
                                "@fizetesiMod,@kedvezmeny,@szamlaMegjegyzes,@partnerMegjegyzes)";

                            Parancs.Parameters.AddWithValue("@adoSzam", adat.AdoSzam);
                            Parancs.Parameters.AddWithValue("@EuadoSzam", adat.EUAdoszam1);
                            Parancs.Parameters.AddWithValue("@telefonSzam", adat.TelefonSzam);
                            Parancs.Parameters.AddWithValue("@fax", adat.Fax);
                            Parancs.Parameters.AddWithValue("@email", adat.Email);
                            Parancs.Parameters.AddWithValue("@webCim", adat.WebCim);
                            Parancs.Parameters.AddWithValue("@bankSzamlaSzam", adat.BankSzamlaSzam);
                            Parancs.Parameters.AddWithValue("@partnerCsoport", adat.PartnerCsoport);
                            Parancs.Parameters.AddWithValue("@fizetesiMod", adat.FizetesiMod);
                            Parancs.Parameters.AddWithValue("@kedvezmeny", adat.Kedvezmeny);
                            Parancs.Parameters.AddWithValue("@szamlaMegjegyzes", adat.SzamlaMegjegyzes);
                            Parancs.Parameters.AddWithValue("@partnerMegjegyzes", adat.PartnerMegjegyzes);
                            Parancs.ExecuteNonQuery();

                            Parancs.Parameters.Clear();

                            Parancs.CommandText = "DECLARE @id INT DEFAULT 0";
                            Parancs.Parameters.Clear();

                            Parancs.CommandText = "SET @id = LAST_INSERT_ID()";

                            Parancs.Parameters.Clear();

                            Parancs.CommandText = " INSERT INTO Cim  (  CimId,Orszag,Irsz,Telepules,Kozterulet,KozteruletJelleg,HazSzam,EmeletAjto,PostaOrszag, PostaIrsz,PostaTelepules,PostaKozterulet,PostaKozteruletJelleg,PostaHazSzam,PostaEmeletAjto)" +
                                " VALUES(@id,@orszag,@irsz,@telepules,@kozterulet,@kozterjelleg,@hazszam,@emeletajto,@postaorszag,@postairsz,@postatelepules,@postakozterulet,@postakozterjelleg,@postahazszam,@postaemeletajto)";


                            Parancs.Parameters.AddWithValue("@orszag", adat.Orszag);
                            Parancs.Parameters.AddWithValue("@irsz", adat.Irsz);
                            Parancs.Parameters.AddWithValue("@telepules", adat.Telepules);
                            Parancs.Parameters.AddWithValue("@kozterulet", adat.Kozterulet);
                            Parancs.Parameters.AddWithValue("@kozterjelleg", adat.KozteruletJelleg);
                            Parancs.Parameters.AddWithValue("@hazszam", adat.HazSzam);
                            Parancs.Parameters.AddWithValue("@emeletajto", adat.EmeletAjto);
                            Parancs.Parameters.AddWithValue("@postaorszag", adat.PostaOrszag);
                            Parancs.Parameters.AddWithValue("@postairsz", adat.PostaIrsz);
                            Parancs.Parameters.AddWithValue("@postatelepules", adat.PostaTelepules);
                            Parancs.Parameters.AddWithValue("@postakozterulet", adat.PostaKozterulet);
                            Parancs.Parameters.AddWithValue("@postakozterjelleg", adat.PostaKozteruletJelleg);
                            Parancs.Parameters.AddWithValue("@postahazszam", adat.PostaHazSzam);
                            Parancs.Parameters.AddWithValue("@postaemeletajto", adat.PostaEmeletAjto);
                            Parancs.ExecuteNonQuery();




                            Parancs.Parameters.Clear();
                            Parancs.CommandText = " INSERT INTO Nev  (NevId,Titulus,VezetekNev,KeresztNev,UtoNev,CegNev,PartnerId)" +
                                " VALUES(@id,@titulus,@vezetekNev,@keresztNev,@utoNev,@cegNev,@partnerID)";
                            Parancs.Parameters.AddWithValue("@titulus", adat.Titulus);
                            Parancs.Parameters.AddWithValue("@vezetekNev", adat.VezetekNev);
                            Parancs.Parameters.AddWithValue("@keresztNev", adat.KeresztNev);
                            Parancs.Parameters.AddWithValue("@utoNev", adat.UtoNev);
                            Parancs.Parameters.AddWithValue("@cegNev", adat.CegNev);
                            Parancs.Parameters.AddWithValue("@partnerID", adat.PartnerAzonosito);
                            Parancs.ExecuteNonQuery();



                            Tranzakcio.Commit();
                        }
                        catch (Exception ex1)
                        {
                            MessageBox.Show(ex1.Message);
                            try
                            {
                                Tranzakcio.Rollback();
                            }
                            catch (Exception ex2)
                            {
                                MessageBox.Show(ex2.Message);
                            }
                        }
                    }
                }
                kapcsolat.Close();
            }
        }

        #endregion



    }
}

#endregion




























