using System;
using System.Data.SqlClient;

namespace csharp_biblioteca_db
{
    class Program
    {
       static void Main(string[] args)
        {
            Biblioteca b = new Biblioteca("Civica");

            

            //caricare 100000 clienti da file su DB
            /*
            StreamReader streamReader = new StreamReader(@"D:\Download\100000_USA_Email_Address.txt");
            string linea;

            var conn = db.Connect();
            
            string cmd = "BEGIN TRANSACTION\n";
            while ((linea = streamReader.ReadLine()) != null)
            {
                if (conn == null)
                {
                    throw new Exception("Unable to connect to the database");
                }

                cmd = String.Format("INSERT INTO [clienti] (nome_cliente, email) VALUES ('{0}', '{1}')\n", linea.Split('@')[0], linea);
                
                using (SqlCommand insert = new SqlCommand(cmd, conn))
                {
                    try
                    {
                        var numrows = insert.ExecuteNonQuery();
                        Console.WriteLine(numrows);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            cmd = "END TRANSACTION";
            SqlCommand lastInsert = new SqlCommand(cmd, conn);
            conn.Close();*/


            // CARICARE 10000 libri su db da file
            /*
            StreamReader streamReader = new StreamReader(@"D:\Download\elenco.txt");
            string linea;
           List<Tuple<string, string, string>> vett = new List<Tuple<string, string, string>>();
            while ((linea = streamReader.ReadLine()) != null)
            {
                string cognome = "";
                try
                {
                    cognome = linea.Split(':')[0].Substring(linea.Split(':')[0].Split(' ')[0].Length + 1);
                }
                catch(Exception ex)
                {
                    
                }
                vett.Add(new Tuple<string, string, string>(linea.Split(':')[0].Split(' ')[0], cognome, linea.Split(':')[1]));
            }
            foreach (var item in vett)
            {
                Random rand = new Random(); 
                Scaffale scaffale = b.GetScaffaleFromNumber("S00" + rand.Next(1, 6));             
                List<Autore> listaDiAutori = new List<Autore>();
                listaDiAutori.Add(new Autore(item.Item1, item.Item2, item.Item1 + "@" + item.Item2 + ".com"));
                b.aggiungiLibro(item.Item3, String.Format("{0}", rand.Next(1970, 2022)), "Romanzo", rand.Next(100, 2000), scaffale, listaDiAutori);
            }*/
            //la prossima istruzione inserisce nel db il codice di partenza per generare i codici unici dei documenti (PK)
            //db.setCodiceUnivocoDocumento(0);
            //Console.WriteLine(db.getCodiceUnicoDocumento());


            //List<Autore> listaDiAutori = new List<Autore>();
            //listaDiAutori.Add(new Autore("Alessandr", "Manzoni", "alexmanzo@gmail.com"));
            //listaDiAutori.Add(new Autore("Gianni", "Gianelli", "gianni@gina.gia"));

            //b.aggiungiLibro("I promessi sposi", "2001", "Romanzo", 2000, new Scaffale("S004", "Via Piave, 5", "Stanza A"), listaDiAutori);

            Console.WriteLine("LISTA OPERAZIONI\ncosa vuoi fare?");
            Console.WriteLine("\t1 -> Cerca documento per parola chiave");
            Console.WriteLine("\t2 -> Inserisci documento");
            Console.WriteLine("\t3 -> Crea evento");
            string? input = Console.ReadLine();
          
            while (input != null && input != "")
            {
                b.GestisciOperazioniBiblioteca(input);
                input = Console.ReadLine();
            }
        }
    }
}