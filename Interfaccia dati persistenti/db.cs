using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_biblioteca_db
{

    internal class db
    {
        private static string connectionString = "Data Source=DESKTOP-IN7U4B6;Initial Catalog=biblioteca;User ID=sa;Password=sa;Pooling=False";
    
        private static SqlConnection? Connect()
        {
            SqlConnection conn = new SqlConnection(connectionString);            
            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            return conn;        
        }

        internal static int addScaffale(Scaffale nuovoScaffale)
        {
            var conn = Connect();
            if (conn == null)
            {
                throw new Exception("Unable to connect to the database");
            }

            var cmd = String.Format("INSERT INTO [scaffali] (codice, sede, stanza) VALUES ('{0}', '{1}', '{2}')", nuovoScaffale.Numero, nuovoScaffale.Sede, nuovoScaffale.Stanza);

            using (SqlCommand insert = new SqlCommand(cmd,conn))
            {
                try
                {
                    var numrows = insert.ExecuteNonQuery();
                    return numrows;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return 0;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        internal static string getInsertAutoreCommandString(Autore nuovoAutore)
        {
            var conn = Connect();
            if (conn == null)
            {
                throw new Exception("Unable to connect to the database");
            }

            return String.Format("INSERT INTO [autori] (nome, cognome, mail) VALUES ('{0}', '{1}', '{2}')", nuovoAutore.Nome, nuovoAutore.Cognome, nuovoAutore.Mail);
        }
        internal static List<Tuple<string, string, string>> getScaffaliFromDb()
        {
            List<Tuple<string, string, string>> result = new List<Tuple<string, string, string>>();

            var conn = Connect();
            if (conn == null)
            {
                throw new Exception("Unable to connect to the database");
            }

            var cmd = String.Format("SELECT [codice], [sede], [stanza] FROM [scaffali]");

            using (SqlCommand select = new SqlCommand(cmd, conn))
            {
                try
                {
                    var response = select.ExecuteReader();
                    while (response.Read())
                    {
                        result.Add(new Tuple<string, string, string>(response.GetString(0), response.GetString(1), response.GetString(2)));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
                return result;
            }
        }
        internal static int AddLibro(Libro nuovoLibro)
        {
            var conn = Connect();
            if (conn == null)
            {
                throw new Exception("Unable to connect to the database");
            }

            string cmd = "BEGIN TRANSACTION\n";
            
            cmd += String.Format("INSERT INTO [documenti] (codice, titolo, anno, settore, stato, tipo, scaffale) VALUES ('{0}', '{1}', '{2}', '{3}', 'disponibile', 'libro', '{4}')\n", nuovoLibro.Codice, nuovoLibro.Titolo, nuovoLibro.Anno, nuovoLibro.Settore, nuovoLibro.Scaffale.Numero);
            cmd += String.Format("INSERT INTO [libri] (codice, numero_pagine) VALUES ('{0}', '{1}')", nuovoLibro.Codice, nuovoLibro.NumeroPagine);

            foreach(Autore autore in nuovoLibro.Autori)
            {
                cmd += String.Format("BEGIN\nIF NOT EXISTS(SELECT * FROM [autori] WHERE [nome] = '{0}' AND [cognome] = '{1}' AND [mail] = '{2}')\n", autore.Nome, autore.Cognome, autore.Mail);
                cmd += String.Format("BEGIN\nINSERT INTO [autori](nome, cognome, mail) VALUES ('{0}', '{1}', '{2}')\nEND\n", autore.Nome, autore.Cognome, autore.Mail);
                cmd += String.Format("END\n");
            }

            cmd += "COMMIT TRANSACTION;";
            using (SqlCommand insert = new SqlCommand(cmd, conn))
            {
                try
                {
                    var numrows = insert.ExecuteNonQuery();
                    return numrows;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return 0;
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }
}
