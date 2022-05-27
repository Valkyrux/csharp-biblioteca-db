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
            return String.Format("INSERT INTO [autori] (nome, cognome, mail) VALUES ('{0}', '{1}', '{2}')", nuovoAutore.Nome, nuovoAutore.Cognome, nuovoAutore.Mail);
        }

        internal static string getReadAutoreIdCommandString(Autore autoreDaCercare, string costantePerSelect="")
        {
            //costante per select aggiunge un parametro inserito sul risultato della select da scrivere nel formato ", constante per select"
            return String.Format("SELECT [id_autore]{0} FROM [autori] WHERE [nome] = '{1}' AND [cognome] = '{2}' AND [mail] = '{3}'",costantePerSelect, autoreDaCercare.Nome, autoreDaCercare.Cognome, autoreDaCercare.Mail);
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
                cmd += String.Format("BEGIN\nIF NOT EXISTS({0})\n", getReadAutoreIdCommandString(autore));
                cmd += String.Format("BEGIN\n{0}\nEND\n", getInsertAutoreCommandString(autore));
                //cmd += String.Format("ELSE\nINSERT INTO [autore_documento](id_autore, id_documento)\n{0}\n", getReadAutoreIdCommandString(autore, String.Format(", {0}", nuovoLibro.Codice)));
                cmd += String.Format("END\n");
                cmd += String.Format("INSERT INTO [autore_documento](id_autore, id_documento)\n{0}\n", getReadAutoreIdCommandString(autore, String.Format(", {0}", nuovoLibro.Codice)));
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

        internal static int AddDVD(DVD nuovoDVD)
        {
            var conn = Connect();
            if (conn == null)
            {
                throw new Exception("Unable to connect to the database");
            }

            string cmd = "BEGIN TRANSACTION\n";

            cmd += String.Format("INSERT INTO [documenti] (codice, titolo, anno, settore, stato, tipo, scaffale) VALUES ('{0}', '{1}', '{2}', '{3}', 'disponibile', 'dvd', '{4}')\n", nuovoDVD.Codice, nuovoDVD.Titolo, nuovoDVD.Anno, nuovoDVD.Settore, nuovoDVD.Scaffale.Numero);
            cmd += String.Format("INSERT INTO [dvd] (codice, durata) VALUES ('{0}', '{1}')", nuovoDVD.Codice, nuovoDVD.Durata);

            foreach (Autore autore in nuovoDVD.Autori)
            {
                cmd += String.Format("BEGIN\nIF NOT EXISTS({0})\n", getReadAutoreIdCommandString(autore));
                cmd += String.Format("BEGIN\n{0}\nEND\n", getInsertAutoreCommandString(autore));
                //cmd += String.Format("ELSE\nINSERT INTO [autore_documento](id_autore, id_documento)\n{0}\n", getReadAutoreIdCommandString(autore, String.Format(", {0}", nuovoLibro.Codice)));
                cmd += String.Format("END\n");
                cmd += String.Format("INSERT INTO [autore_documento](id_autore, id_documento)\n{0}\n", getReadAutoreIdCommandString(autore, String.Format(", {0}", nuovoDVD.Codice)));
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

        internal static long getCodiceUnicoDocumento()
        {
            var conn = Connect();
            if(conn == null)
            {
                throw new Exception("Unable to connect to the database");
            }
            string cmd = "DECLARE @valore BIGINT UPDATE[codici_univoco] SET valore = valore + 1, @valore = valore + 1 WHERE[campo] = 'documento' SELECT @valore";
            long codice;
            using(SqlCommand select = new SqlCommand(cmd, conn))
            {
                var reader = select.ExecuteReader();
                reader.Read();
                codice = reader.GetInt64(0);
            }
            conn.Close();
            return codice;
        }

        internal static void setCodiceUnivocoDocumento(long valoreDiPartenza)
        {
            //utilizzare questo metodo per settare il valore di partenza nel DB da cui verrano calcolate le chiavi
            var conn = Connect();
            if (conn == null)
            {
                throw new Exception("Unable to connect to the database");
            }
            string cmd = String.Format("INSERT INTO [codici_univoco] (campo, valore) VALUES ('documento', {0})", valoreDiPartenza);
            
            using (SqlCommand select = new SqlCommand(cmd, conn))
            {
                var insert = select.ExecuteNonQuery();

                Console.WriteLine(insert);
                Console.WriteLine("Valore di partenza per la generazione dei codici documenti impostato a {0}", valoreDiPartenza);
            }
            conn.Close();
        }

        internal static List<Tuple<string, string, string>> getAutoriFromCodiceDocumento(long codice)
        {
            List<Tuple<string, string, string>> result = new List<Tuple<string, string, string>>();

            var conn = Connect();
            if (conn == null)
            {
                throw new Exception("Unable to connect to the database");
            }

            var cmd = String.Format("SELECT [nome], [cognome], [mail]\nFROM [autori]\nINNER JOIN [autore_documento] ON [autori].[id_autore] = [autore_documento].[id_autore]\nWHERE [autore_documento].[id_documento] = {0}", codice);

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

        internal static List<List<string>> getDocumentiFromDBByString(string autore)
        {
            List<List<string>> result = new List<List<string>>();

            var conn = Connect();
            if (conn == null)
            {
                throw new Exception("Unable to connect to the database");
            }

            var cmd = String.Format("SELECT [tipo], [documenti].[codice], [titolo], [anno], [settore], [stato], [scaffale], [libri].[numero_pagine], [dvd].[durata]\nFROM [documenti]\nINNER JOIN [autore_documento] ON [documenti].[codice] = [autore_documento].[id_documento]\nINNER JOIN [autori] ON [autori].[id_autore] = [autore_documento].[id_autore]\nFULL OUTER JOIN [libri] ON [documenti].[codice] = [libri].[codice]\nFULL OUTER JOIN [dvd] ON [documenti].[codice] = [dvd].[codice]\nWHERE CONCAT([documenti].[titolo], ' ',[autori].[nome], ' ', [autori].[cognome]) LIKE '%{0}%'\n", autore);
        
            using (SqlCommand select = new SqlCommand(cmd, conn))
            {
                try
                {
                    var response = select.ExecuteReader();
                    while (response.Read())
                    {
                        List<string> nuovoDocInLista = new List<string>();
                        nuovoDocInLista.Add(response.GetString(0));
                        nuovoDocInLista.Add(response.GetInt32(1).ToString());
                        nuovoDocInLista.Add(response.GetString(2));
                        nuovoDocInLista.Add(response.GetString(3));
                        nuovoDocInLista.Add(response.GetString(4));
                        nuovoDocInLista.Add(response.GetString(5));
                        nuovoDocInLista.Add(response.GetString(6));

                        if(response.GetString(0) == "libro")
                        {
                            nuovoDocInLista.Add(response.GetInt32(7).ToString());
                        } 
                        else if(response.GetString(0) == "dvd")
                        {
                            nuovoDocInLista.Add(response.GetTimeSpan(8).ToString());
                        }
                        else
                        {
                            throw new Exception("Invalid field 'type' on DB");
                        }
                        result.Add(nuovoDocInLista);
                    }

                    //Console.WriteLine(select.ExecuteNonQuery());
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
    }
}
