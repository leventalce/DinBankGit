using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DinBank
{
    public class Database
    {
        //Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename="C:\Users\Bruger\OneDrive\Dokumenter\Dinbank\FinansDb (1).mdf";Integrated Security=True
        //Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Bruger\Downloads\FinansDb.mdf;Integrated Security=True;Connect Timeout=30
        //public SqlConnection Con1 = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Bruger\Downloads\FinansDb.mdf;Integrated Security=True;Connect Timeout=30");
        public SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Bruger\OneDrive\Dokumenter\Dinbank\FinansDbFinal.mdf;Integrated Security=True;Connect Timeout=30");
        //public SqlConnection Con2 = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=C:\Users\Bruger\OneDrive\Dokumenter\Dinbank\FinansDbFinal.mdf;Integrated Security = True; Connect Timeout = 30");
        public bool KommerFraRegistrerEllerLogin { get; set; }
        //Hvis jeg skal have ad vide hvem fejlen opstår hos, så kan jeg kontakte dem hvis der skulle være brug for det.
        public string HentBrugerNavn()
        {
            Con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("Select [BrgNavn] from BrugerTbl where BrgNavn = '" + DinBank.Login.User + "'", Con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows[0][0].ToString() == "")
            {
                Con.Close();
                return "Ikke eksisterende bruger endnu.";
            }
            string brugernavn = dt.Rows[0][0].ToString();
            Con.Close();
            return brugernavn;
        }
        public string HentBrugerNavnTest()
        {
            string brugernavn = "";
            try
            {
                Con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("Select [BrgNavn] from BrugerTbl where BrgNavn = '" + DinBank.Login.User + "'", Con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows[0][0].ToString() == "")
                {
                    Con.Close();
                    return "Ikke eksisterende bruger endnu.";
                }
                brugernavn = dt.Rows[0][0].ToString();
                Con.Close();
            }
            catch (Exception Ex)
            {
                FejlHaandteringOgBesked(Ex);
            }
            return brugernavn;
        }
        //Programmet kan tjekke om det brugernavn der prøver at blive oprettet allerede findes.
        public List<string> HentAlleBrugerNavne()
        {
            List<string> brugernavne = new List<string>();
            string navn;
            using (Con)
            {
                Con.Open();
                SqlCommand command = new SqlCommand("select * from BrugerTbl", Con);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    navn = (string)reader["BrgNavn"];
                    brugernavne.Add(navn);
                }
                Con.Close();
            }
            return brugernavne;
        }
        //Bliver brugt til at udregne saldo, samt vise summen på dashboardet
        public string TotalIndkomst()
        {
            string indkomstString = "";
            try
            {
                Con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("select sum(InkBlb) from IndkomstTbl where InkBrg='" + DinBank.Login.User + "'", Con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows[0][0].ToString() == "")
                {
                    return "";
                }
                Indkomst = Convert.ToInt32(dt.Rows[0][0].ToString());
                indkomstString = Indkomst.ToString("N");
            }
            catch (Exception Ex)
            {
                FejlHaandteringOgBesked(Ex);
            }
            finally
            {
                Con.Close();
            }
            return indkomstString;
        }
        //De to nedenstående variabler bliver sat i oven- og nedenstående metoder, og bruges til at udregne saldo.
        public int Indkomst, Udgift;
        public string TotalUdgift()
        {
            string UdgiftString = "";
            try
            {
                Con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("select sum(UdgBlb) from UdgiftTbl where UdgBrg='" + DinBank.Login.User + "'", Con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows[0][0].ToString() == "")
                {
                    return "";
                }
                Udgift = Convert.ToInt32(dt.Rows[0][0].ToString());
                UdgiftString = Udgift.ToString("N");
            }
            catch (Exception Ex)
            {
                FejlHaandteringOgBesked(Ex);
            }
            finally
            {
                Con.Close();
            }
            return UdgiftString;
        }
        //viser hvor mange gange der er indskrevet en indkomst
        public int AntalIndkomster()
        {
            int antalIndkomster = 0;
            try
            {
                Con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("select count(InkBlb) from IndkomstTbl where InkBrg = '" + DinBank.Login.User + "'", Con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                antalIndkomster = Convert.ToInt32(dt.Rows[0][0].ToString());
            }
            catch (Exception Ex)
            {
                FejlHaandteringOgBesked(Ex);
            }
            finally
            {
                Con.Close();
            }
            return antalIndkomster;
        }
        //-||-
        public int AntalUdgifter()
        {
            int antalUdgifter = 0;
            try
            {
                Con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("select count(UdgBlb) from UdgiftTbl where UdgBrg = '" + DinBank.Login.User + "'", Con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                antalUdgifter = Convert.ToInt32(dt.Rows[0][0].ToString());
            }
            catch (Exception Ex)
            {
                FejlHaandteringOgBesked(Ex);
            }
            finally
            {
                Con.Close();
            }
            return antalUdgifter;
        }
        public string FåSaldo()
        {
            string saldo = (Indkomst - Udgift).ToString("N");
            return saldo;
        }
        public DataTable VisIndkomsterSeIndkomster()
        {
            var ds = new DataSet();
            try
            {
                Con.Open();
                string Query = "Select * from IndkomstTbl where InkBrg = '" + DinBank.Login.User + "' order by (InkDato) desc";
                SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
                SqlCommandBuilder builder = new SqlCommandBuilder(sda);
                ds = new DataSet();
                sda.Fill(ds);
                RetKolonneNavneIndkomst(ds);
            }
            catch (Exception Ex)
            {
                FejlHaandteringOgBesked(Ex);
            }
            finally
            {
                Con.Close();
            }
            return ds.Tables[0];
        }
        //De to nedenstående metoder, henter information fra databasen, og gemmer det i datasettet
        //informationerne bliver brugt på se indkomst og udgift
        public DataTable VisUdgifterSeUdgifter()
        {
            var ds = new DataSet();
            try
            {
                Con.Open();
                string Query = "Select * from UdgiftTbl where UdgBrg = '" + DinBank.Login.User + "' order by (UdgDato) desc";
                SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
                SqlCommandBuilder builder = new SqlCommandBuilder(sda);
                ds = new DataSet();
                sda.Fill(ds);
                RetKolonneNavneUdgift(ds);
            }
            catch (Exception Ex)
            {
                FejlHaandteringOgBesked(Ex);
            }
            finally
            {
                Con.Close();
            }
            return ds.Tables[0];
        }
        //Sørger for at hente transaktioner, med den kategori brugeren søger efter.
        public DataTable VisIndkomsterSeIndkomsterKategori(string Kategori)
        {
            var ds = new DataSet();
            try
            {
                Con.Open();
                string Query = "Select * from IndkomstTbl where InkCat='" + Kategori + "' and InkBrg = '" + DinBank.Login.User + "' order by (InkDato) desc";
                SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
                SqlCommandBuilder builder = new SqlCommandBuilder(sda);
                ds = new DataSet();
                sda.Fill(ds);
                RetKolonneNavneIndkomst(ds);
            }
            catch (Exception Ex)
            {
                FejlHaandteringOgBesked(Ex);
            }
            finally
            {
                Con.Close();
            }
            return ds.Tables[0];
        }
        //Henter transaktioner der minder om det navn der er søgt efter
        public DataTable VisUdgifterNavn(string UdgiftNavn)
        {
            var ds = new DataSet();
            try
            {
                Con.Open();
                string Query = "Select * from UdgiftTbl where UdgNavn like '%" + UdgiftNavn + "%' and UdgBrg = '" + DinBank.Login.User + "' order by (UdgDato) desc";
                SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
                SqlCommandBuilder builder = new SqlCommandBuilder(sda);
                ds = new DataSet();
                sda.Fill(ds);
                RetKolonneNavneUdgift(ds);
            }
            catch (Exception Ex)
            {
                FejlHaandteringOgBesked(Ex);
            }
            finally
            {
                Con.Close();
            }
            return ds.Tables[0];
        }
        //-||-
        public DataTable VisIndkomstNavn(string IndkomstNavn)
        {
            var ds = new DataSet();
            try
            {
                Con.Open();
                string Query = "Select * from IndkomstTbl where InkNavn like '%" + IndkomstNavn + "%' and InkBrg = '" + DinBank.Login.User + "' order by (InkDato) desc";
                SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
                SqlCommandBuilder builder = new SqlCommandBuilder(sda);
                ds = new DataSet();
                sda.Fill(ds);
                RetKolonneNavneIndkomst(ds);
            }
            catch (Exception Ex)
            {
                FejlHaandteringOgBesked(Ex);
            }
            finally
            {
                Con.Close();
            }
            return ds.Tables[0];
        }
        //Henter kategori transaktion for udgifterne
        public DataTable VisUdgifterKategori(string Kategori)
        {
            var ds = new DataSet();
            try
            {
                Con.Open();
                string Query = "Select * from UdgiftTbl where UdgKat='" + Kategori + "' and UdgBrg = '" + DinBank.Login.User + "' order by (UdgDato) desc";
                SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
                SqlCommandBuilder builder = new SqlCommandBuilder(sda);
                ds = new DataSet();
                sda.Fill(ds);
                RetKolonneNavneUdgift(ds);
            }
            catch (Exception Ex)
            {
                Con.Close();
                FejlHaandteringOgBesked(Ex);
            }
            finally
            {
                Con.Close();
            }
            return ds.Tables[0];
        }
        //Dashboard bruger de seneste transaktioner, og det sørger koden her for at sende videre.
        public DataTable HentSenesteIndkomst()
        {
            DataTable dt = new DataTable();
            try
            {
                Con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("select [InkDato], [InkNavn], [InkCat], [InkBlb] from IndkomstTbl where InkBrg='" + DinBank.Login.User + "' order by (InkDato) desc", Con);
                sda.Fill(dt);
            }
            catch(Exception Ex)
            {
                FejlHaandteringOgBesked(Ex);
            }
            finally
            {
                Con.Close();
            }
            return dt;
        }
        //-||-
        public DataTable HentSenesteUdgift()
        {
            DataTable dt = new DataTable();
            try
            {
                Con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("select [UdgDato], [UdgNavn], [UdgKat], [UdgBlb] from UdgiftTbl where UdgBrg='" + DinBank.Login.User + "' order by (UdgDato) desc", Con);
                sda.Fill(dt);
            }
            catch (Exception Ex)
            {
                FejlHaandteringOgBesked(Ex);
            }
            finally
            {
                Con.Close();
            }
            return dt;
        }
        //De to nedenstående variabler bliver subtraheret med hinanden længere nede, så man får en saldo for de sidste 30 dage.
        int Indkomst30Dage, Udgift30Dage;
        //Viser summen udgifter de sidste 30 dage
        public string VisUdgiftSidste30Dage()
        {
            string udgiftString = "Ingen Udgifter";
            try
            {
                Con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("select sum(UdgBlb) from UdgiftTbl where UdgBrg='" + DinBank.Login.User + "' and UdgDato > DATEADD(DAY,-30,GETDATE())", Con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows[0][0].ToString() == "")
                {
                    return udgiftString;
                }
                Udgift30Dage = Convert.ToInt32(dt.Rows[0][0].ToString());
                udgiftString = Udgift30Dage.ToString("N");
            }
            catch (Exception Ex)
            {
                FejlHaandteringOgBesked(Ex);
            }
            finally
            {
                Con.Close();
            }
            return udgiftString;
        }
        //Viser summen for indkomster de sidste 30 dage
        public string VisIndkomstSidste30Dage()
        {
            string indkomstString = "Ingen indkomster";
            try
            {
                Con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("select sum(InkBlb) from IndkomstTbl where InkBrg='" + DinBank.Login.User + "' and InkDato > DATEADD(DAY,-30,GETDATE())", Con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows[0][0].ToString() == "")
                {
                    return indkomstString;
                }
                Indkomst30Dage = Convert.ToInt32(dt.Rows[0][0].ToString());
                indkomstString = Indkomst30Dage.ToString("N");
            }
            catch (Exception Ex)
            {
                FejlHaandteringOgBesked(Ex);
            }
            finally
            {
                Con.Close();
            }
            return indkomstString;
        }
        //tager de to summer man får tilbage fra og trækker dem fra hinanden
        public string saldo30Dage()
        {
            string Saldo30Dage = (Indkomst30Dage - Udgift30Dage).ToString("N");
            return Saldo30Dage;
        }
        // tæller hvor mange indkomster der er blevet foretaget de sidste 30 dage
        public string VisAntalIndkomstSidste30Dage()
        {
            string AntalIndkomser30DageString = "0";
            try
            {
                Con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("select count(InkBlb) from IndkomstTbl where InkBrg='" + DinBank.Login.User + "' and InkDato > DATEADD(DAY,-30,GETDATE())", Con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                int AntalIndkomster30Dage = Convert.ToInt32(dt.Rows[0][0].ToString());
                AntalIndkomser30DageString = AntalIndkomster30Dage.ToString();
                Con.Close();
                return AntalIndkomser30DageString;
            }
            catch (Exception Ex)
            {
                FejlHaandteringOgBesked(Ex);
            }
            finally
            {
                Con.Close();
            }
            return AntalIndkomser30DageString;
        }
        // -||- det samme bare med udgifter
        public string VisAntalUdgiftSidste30Dage()
        {
            string AntalUdgifter30DageString = "";
            try
            {
                Con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("select count(UdgBlb) from UdgiftTbl where UdgBrg='" + DinBank.Login.User + "' and UdgDato > DATEADD(DAY,-30,GETDATE())", Con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                int AntalUdgifter30Dage = Convert.ToInt32(dt.Rows[0][0].ToString());
                AntalUdgifter30DageString = AntalUdgifter30Dage.ToString();
            }
            catch (Exception Ex)
            {
                FejlHaandteringOgBesked(Ex);
            }
            finally
            {
                Con.Close();
            }
            return AntalUdgifter30DageString;
        }
        // gemmeer indkomst i databasen
        public void GemIndkomstIDb(string IndkomstNavn, string IndkomstBeloeb, string IndkomstKategori, string IndkomstDato, string IndkomstBeskrivelse, string bruger)
        {
            try
            {
                Con.Open();
                SqlCommand cmd = new SqlCommand("insert into IndkomstTbl(InkNavn,InkBlb,InkCat,InkDato,InkBesk,InkBrg)values(@IN,@IB,@IC,@ID,@IBE,@IU)", Con);
                cmd.Parameters.AddWithValue("@IN", IndkomstNavn);
                cmd.Parameters.AddWithValue("@IB", IndkomstBeloeb);
                cmd.Parameters.AddWithValue("@IC", IndkomstKategori);
                cmd.Parameters.AddWithValue("@ID", IndkomstDato);
                cmd.Parameters.AddWithValue("@IBE", IndkomstBeskrivelse);
                cmd.Parameters.AddWithValue("@IU", bruger);
                cmd.ExecuteNonQuery();
            }
            catch (Exception Ex)
            {
                FejlHaandteringOgBesked(Ex);
            }
            finally
            {
                Con.Close();
            }
        }
        //Jeg har prøvet at sørge for at brugeren ikke får en masse fejl kastet i hovedet, derfor kommer det ind i databasen hvor jeg kan se hvad fejlen er og prøve at løse det derefter
        public void IndsætFejlTilDB(Exception Ex, string bruger)
        {
            DateTime Dato = DateTime.Now;
            Con.Close();
            Con.Open();
            SqlCommand cmd = new SqlCommand("insert into FejlMeddelelseTbl(Dato,Besked,Bruger)values(@FD,@FB,@FU)", Con);
            cmd.Parameters.AddWithValue("@FD", Dato);
            cmd.Parameters.AddWithValue("@FB", Ex.ToString());
            cmd.Parameters.AddWithValue("@FU", bruger);
            cmd.ExecuteNonQuery();
            Con.Close();
        }

        public class UdgifterData
        {
            public string UdgiftNavn { get; set; }
            public string UdgiftBeloeb { get; set; }
        }
        //Gemmer udgift i databasen
        public void GemUdgiftIDb(string UdgiftNavn, string UdgiftBeloeb, string UdgiftKategori, string UdgiftDato, string UdgiftBeskrivelse, string bruger)
        {
            //TODO: Husk at sætte try finally
            try
            {
                Con.Open();
                SqlCommand cmd = new SqlCommand("insert into UdgiftTbl(UdgNavn,UdgBlb,UdgKat,UdgDato,UdgBesk,UdgBrg)values(@UN,@UB,@UK,@UD,@UBE,@UU)", Con);
                cmd.Parameters.AddWithValue("@UN", UdgiftNavn);
                cmd.Parameters.AddWithValue("@UB", UdgiftBeloeb);
                cmd.Parameters.AddWithValue("@UK", UdgiftKategori);
                cmd.Parameters.AddWithValue("@UD", UdgiftDato);
                cmd.Parameters.AddWithValue("@UBE", UdgiftBeskrivelse);
                cmd.Parameters.AddWithValue("@UU", bruger);
                cmd.ExecuteNonQuery();
            }
            catch (Exception Ex)
            {
                FejlHaandteringOgBesked(Ex);
            }
            finally
            {
                Con.Close();
            }
        }
        //opretter bruger, har ikke lige fungeret som det skulle her til sidst, så har lavet samme kode i registrer siden.
        public void OpretRegister(string Navn, string Fødselsdag, string Telefon, string KodeOrd, string Adresse)
        {
            try
            {
                Con.Open();
                SqlCommand cmd = new SqlCommand("insert into BrugerTbl(BrgNavn,BrgFød,BrgTlf,BrgKode,BrgAdd)values(@BN,@BF,@BT,@BK,@BA)", Con);
                cmd.Parameters.AddWithValue("@BN", Navn);
                cmd.Parameters.AddWithValue("@BF", Fødselsdag);
                cmd.Parameters.AddWithValue("@BT", Telefon);
                cmd.Parameters.AddWithValue("@BK", KodeOrd);
                cmd.Parameters.AddWithValue("@BA", Adresse);
                cmd.ExecuteNonQuery();
            }
            catch (Exception Ex)
            {
                FejlHaandteringOgBesked(Ex);
            }
            finally
            {
                Con.Close();
            }
        }

        public bool Login(string Brugernavn, string Kodeord, bool KorrektLogin)
        {
            try
            {
                Con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("select count(*) from BrugerTbl where BrgNavn='" + Brugernavn + "' and BrgKode='" + Kodeord + "'", Con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows[0][0].ToString() == "1")
                {
                    KorrektLogin = true;
                }
                else
                {
                    KorrektLogin = false;
                }
            }
            catch (Exception Ex)
            {
                FejlHaandteringOgBesked(Ex);
            }
            finally
            {
                Con.Close();
            }
            return KorrektLogin;
        }
        // Tager summen for alle indkomster der har været
        public string VisSumIndkomst()
        {
            string SaldoIndkomstString = "";
            try
            {
                Con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("select sum(InkBlb) from IndkomstTbl where InkBrg='" + DinBank.Login.User + "'", Con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows[0][0].ToString() == "")
                {
                    Con.Close();
                    return "";
                }
                int SaldoIndkomst = Convert.ToInt32(dt.Rows[0][0].ToString());
                SaldoIndkomstString = SaldoIndkomst.ToString("N");
            }
            catch (Exception Ex)
            {
                FejlHaandteringOgBesked(Ex);
            }
            finally
            {
                Con.Close();
            }
            return SaldoIndkomstString;
        }
        //Viser alle de udgifter, der passer til det navn man har søgt efter
        public string VisSumIndkomstNavn(string SøgtIndkomstNavn)
        {
            string SaldoIndkomstString = "";
            try
            {
                Con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("select sum(InkBlb) from IndkomstTbl where InkBrg='" + DinBank.Login.User + "' and InkNavn like '%" + SøgtIndkomstNavn + "%'", Con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows[0][0].ToString() == "")
                {
                    Con.Close();
                    return "";
                }
                int SaldoIndkomst = Convert.ToInt32(dt.Rows[0][0].ToString());
                SaldoIndkomstString = SaldoIndkomst.ToString("N");
            }
            catch (Exception Ex)
            {
                FejlHaandteringOgBesked(Ex);
            }
            finally
            {
                Con.Close();
            }
            return SaldoIndkomstString;
        }
        //Alt efter hvilken kategori det er der bliver valgt, så henter jeg summen fra databasen.
        public string VisSaldoIndkomstKategori(string KategoriIndkomst)
        {
            string SaldoIndkomstString = "";
            try
            {
                Con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("select sum(InkBlb) from IndkomstTbl where InkBrg='" + DinBank.Login.User + "' and InkCat='" + KategoriIndkomst + "'", Con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows[0][0].ToString() == "")
                {
                    Con.Close();
                    return "";
                }
                int SaldoIndkomst = Convert.ToInt32(dt.Rows[0][0].ToString());
                SaldoIndkomstString = SaldoIndkomst.ToString("N");
            }
            catch (Exception Ex)
            {
                FejlHaandteringOgBesked(Ex);
            }
            finally
            {
                Con.Close();
            }
            return SaldoIndkomstString;
        }
        //Henter summen fra alle udgifter i databasen.
        public string VisSumUdgift()
        {
            string SaldoIndkomstString = "";
            try
            {
                Con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("select sum(UdgBlb) from UdgiftTbl where UdgBrg='" + DinBank.Login.User + "'", Con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows[0][0].ToString() == "")
                {
                    Con.Close();
                    return "";
                }
                int SaldoIndkomst = Convert.ToInt32(dt.Rows[0][0].ToString());
                SaldoIndkomstString = SaldoIndkomst.ToString("N");
            }
            catch (Exception Ex)
            {
                FejlHaandteringOgBesked(Ex);
            }
            finally
            {
                Con.Close();
            }
            return SaldoIndkomstString;
        }
        //Viser summen for det man har søgt efter på seudgifter siden.
        public string VisSumUdgiftNavn(string SøgtUdgiftNavn)
        {
            string SaldoIndkomstString = "";
            try
            {
                Con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("select sum(UdgBlb) from UdgiftTbl where UdgBrg='" + DinBank.Login.User + "' and UdgNavn like '%" + SøgtUdgiftNavn + "%'", Con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows[0][0].ToString() == "")
                {
                    Con.Close();
                    return "";
                }
                int SaldoIndkomst = Convert.ToInt32(dt.Rows[0][0].ToString());
                SaldoIndkomstString = SaldoIndkomst.ToString("N");
            }
            catch (Exception Ex)
            {
                FejlHaandteringOgBesked(Ex);
            }
            finally
            {
                Con.Close();
            }
            return SaldoIndkomstString;
        }
        //Viser summen når man søger efter kategori i udgifter
        public string VisSumUdgiftKategori(string KategoriUdgift)
        {
            string SaldoIndkomstString = "";
            try
            {
                Con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("select sum(UdgBlb) from UdgiftTbl where UdgBrg='" + DinBank.Login.User + "' and UdgKat='" + KategoriUdgift + "'", Con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows[0][0].ToString() == "")
                {
                    Con.Close();
                    return "";
                }
                int SaldoIndkomst = Convert.ToInt32(dt.Rows[0][0].ToString());
                SaldoIndkomstString = SaldoIndkomst.ToString("N");
            }
            catch (Exception Ex)
            {
                FejlHaandteringOgBesked(Ex);
            }
            finally
            {
                Con.Close();
            }
            return SaldoIndkomstString;
        }
        //Retter kolonnenavnene, så der ikke står hvad de hedder inde i databasen.
        public void RetKolonneNavneIndkomst(DataSet ds)
        {
            ds.Tables[0].Columns["InkNavn"].ColumnName = "Navn";
            ds.Tables[0].Columns["InkBlb"].ColumnName = "Beløb";
            ds.Tables[0].Columns["InkCat"].ColumnName = "Kategori";
            ds.Tables[0].Columns["InkDato"].ColumnName = "Dato";
            ds.Tables[0].Columns["InkBesk"].ColumnName = "Beskrivelse";
            ds.Tables[0].Columns["InkBrg"].ColumnName = "Bruger";
        }

        public void RetKolonneNavneUdgift(DataSet ds)
        {
            ds.Tables[0].Columns["UdgNavn"].ColumnName = "Navn";
            ds.Tables[0].Columns["UdgBlb"].ColumnName = "Beløb";
            ds.Tables[0].Columns["UdgKat"].ColumnName = "Kategori";
            ds.Tables[0].Columns["UdgDato"].ColumnName = "Dato";
            ds.Tables[0].Columns["UdgBesk"].ColumnName = "Beskrivelse";
            ds.Tables[0].Columns["UdgBrg"].ColumnName = "Bruger";
        }
        //Fejlhåndtering.
        public void FejlHaandteringOgBesked(Exception Ex)
        {
            Con.Close();
            if (KommerFraRegistrerEllerLogin)
            {
                IndsætFejlTilDB(Ex, "bruger ikke oprettet endnu");
                KommerFraRegistrerEllerLogin = false;
            }
            else
            {
                IndsætFejlTilDB(Ex, HentBrugerNavn());
            }
            MessageBox.Show("Der er opstået en fejl skriv en besked til supporten på dashboard siden.");
        }
    }
}
