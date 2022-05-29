using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_biblioteca_db
{
    internal class Biblioteca
    {
        public string Nome { get; set; }
        public List<Scaffale> ScaffaliBiblioteca { get; set; }

        public bool ScaffaliContains(Scaffale scaffale)
        {
            foreach (Scaffale scaffaleInLista in this.ScaffaliBiblioteca)
            {
                if(scaffaleInLista.Numero == scaffale.Numero && scaffaleInLista.Sede == scaffale.Sede && scaffaleInLista.Stanza == scaffale.Stanza)
                {
                    return true;
                }
            }
            return false;
        }

        public Scaffale GetScaffaleFromNumber(string Numero)
        {
            foreach (Scaffale scaffale in this.ScaffaliBiblioteca)
            {
                if(scaffale.Numero == Numero)
                {
                    return scaffale;
                }
            }
            throw new Exception("No such scaffali with this number");
        }

        public Biblioteca(string Nome)
        {
            this.Nome = Nome;
            this.ScaffaliBiblioteca = new List<Scaffale>();
            getScaffali();
        }

        public int GestisciOperazioniBiblioteca(string? CodiceOperazione)
        {
            switch (CodiceOperazione)
            {
                case "1":
                    Console.WriteLine("\t-> Inserisci parola chiave");
                    string? inputAutore = Console.ReadLine();
                    this.SearchByAutore(inputAutore);
                    break;
                case "2":
                    Console.WriteLine("- SELEZIONA LA TIPOLOGIA DI DOCUMENTO DA CARICARE");
                    Console.WriteLine("\t- 1 : libro");
                    Console.WriteLine("\t- 2 : dvd");
                    Console.WriteLine("\t- qualsiasi altro input per annullare l'operazione");
                    
                    string? addChoice = Console.ReadLine();

                    if (addChoice == "1" || addChoice == "2")
                    {
                        string? titolo;
                        List<Autore> Autori = new List<Autore>();
                        int anno;
                        string? settore;
                        string? numeroScaffale;

                        Console.WriteLine("-> Inserisci Titolo");
                        titolo = Console.ReadLine();
                        
                        string? autoreNome;
                        string? autoreCognome;
                        string? autoreMail;

                        bool Continue = true;
                        Console.WriteLine("-> Inserisci Autori");
                        while (Continue)
                        {
                            Console.WriteLine("  -> Inserisci il nome dell'autore");
                            autoreNome = Console.ReadLine();
                            Console.WriteLine("  -> Inserisci il cognome dell'autore");
                            autoreCognome = Console.ReadLine();
                            Console.WriteLine("  -> Inserisci la mail dell'autore");
                            autoreMail = Console.ReadLine();

                            if (autoreNome != "" && autoreCognome != "" && autoreNome != null && autoreCognome != null)
                            {
                                Autori.Add(new Autore(autoreNome, autoreCognome, autoreMail));
                            }
                            else
                            {
                                Console.WriteLine(" autore non valido\n");
                            }

                            Console.WriteLine("Inserire un altro autore? digita 'SI' per continuare, qualsiasi altro input per passare alla voce successiva");
                            string? inputToContinue = Console.ReadLine();
                            if (inputToContinue != "SI")
                            {
                                Continue = false;
                            }
                        }
                        Console.WriteLine("-> Inserisci Anno");                            
                        int.TryParse(Console.ReadLine(), out anno);

                        Console.WriteLine("-> Inserisci Settore");
                        settore = Console.ReadLine();

                        Console.WriteLine("-> Inserisci numero scaffale");
                        numeroScaffale = Console.ReadLine();


                        if (addChoice == "1")
                        {
                            try
                            {
                                int numPagine;
                                Console.WriteLine("-> Inserisci Numero di Pagine");
                                int.TryParse(Console.ReadLine(), out numPagine);
                                this.aggiungiLibro(titolo, anno.ToString(), settore, numPagine, GetScaffaleFromNumber(numeroScaffale), Autori);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            
                        }
                        if(addChoice == "2")
                        {
                            try
                            {
                                TimeSpan durata;
                                Console.WriteLine("-> Inserisci durata (hh:mm)");
                                if (TimeSpan.TryParse(Console.ReadLine(), out durata))
                                {
                                    this.aggiungiDvd(titolo, anno.ToString(), settore, durata, GetScaffaleFromNumber(numeroScaffale), Autori);
                                }
                                else
                                {
                                    Console.WriteLine("Durata non valida");
                                }
                            }
                            catch(Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }

                        }
                    }

                    break;

                case "3":
                    Console.WriteLine("\t-> Inserisci il codice del libro");
                    int codiceLibro;

                    if(int.TryParse(Console.ReadLine(), out codiceLibro))
                    {
                        Console.WriteLine("\t-> Inserisci la data (YYYY-MM-DD, hh:mm)");
                        DateTime dataEvento;
                        if(DateTime.TryParse(Console.ReadLine(), out dataEvento))
                        {
                            string titolo;
                            bool? controllaLibroInDb = db.checkLibroInDB(codiceLibro, out titolo);
                            if (controllaLibroInDb == true) 
                            {
                                List<Tuple<string, string, string>> listaDiAutori = db.getAutoriFromCodiceDocumento(codiceLibro);
                                foreach (Tuple<string, string, string> autore in listaDiAutori)
                                {
                                    Console.WriteLine("A: {0}", autore.Item3);
                                    Console.WriteLine("OGGETTO: Promemoria per {0} {1} sulla presentazione del tuo libro", autore.Item1, autore.Item2);
                                    Console.WriteLine("MESSAGGIO:\nGiorno {0} nella biblioteca '{1}' presenteremo il tuo libro '{2}'\n", dataEvento, this.Nome, titolo);
                                }
                                Random rand = new Random();
                                int numeroPartecipanti = rand.Next(1, 500);
                                db.insertEvento(codiceLibro, dataEvento, numeroPartecipanti);
                                List<int> listaInteressati = db.getRandomInteressati(numeroPartecipanti);

                            }
                            else if(controllaLibroInDb == false)
                            {
                                Console.WriteLine("Libro non presente nel database, inserirlo prima di creare un evento associato");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Data inserita NON valida");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Codice inserito NON valido");
                    }
                    break;
                default: 
                    Console.WriteLine("\tOperazione NON valida");
                    break;
            }
            return 0;
        }

        private void getScaffali()
        {
            db.getScaffaliFromDb().ForEach(element =>
            {
                Scaffale nuovoScaffale = new Scaffale(element.Item1, element.Item2, element.Item3);
                AggiungiScaffale(nuovoScaffale, false);
            });
        } 
        public bool AggiungiScaffale(Scaffale nuovoScaffale, bool addToDb = true)
        {
            if (!string.IsNullOrEmpty(nuovoScaffale.Numero) && !this.ScaffaliBiblioteca.Contains(nuovoScaffale))
            {
                this.ScaffaliBiblioteca.Add(nuovoScaffale);
                if (addToDb)
                {
                    db.addScaffale(nuovoScaffale);
                }
                return true;
            }
            return false;
        }

        public void aggiungiLibro(string Titolo, string Anno, string Settore, int NumeroPagine, Scaffale scaffale, List<Autore> listaAutori)
        {
            getScaffali();
            if (this.ScaffaliContains(scaffale))
            {
                Libro nuovoLibro = new Libro(db.getCodiceUnicoDocumento(), Titolo, Anno, Settore, NumeroPagine, scaffale, listaAutori);
                Console.WriteLine("Righe inserite nel database: {0}", db.AddLibro(nuovoLibro));
            }
            else
            {
                Console.WriteLine("Scaffale non valido");
            }
        }

        public void aggiungiDvd(string Titolo, string Anno, string Settore, TimeSpan Durata, Scaffale scaffale, List<Autore> listaAutori)
        {
            getScaffali();
            if (this.ScaffaliContains(scaffale))
            {
                DVD nuovoDVD = new DVD(db.getCodiceUnicoDocumento(), Titolo, Anno, Settore, Durata, scaffale, listaAutori);
                Console.WriteLine("Righe inserite nel database: {0}", db.AddDVD(nuovoDVD));
            }
            else
            {
                Console.WriteLine("Scaffale non valido");
            }
        }

        public List<Documento>? SearchByCodice(string Codice)
        {
            return null;
        }

        public List<Documento>? SearchByTitolo(string Titolo)
        {
            return null;
        }

        public void SearchByAutore(string? parola)
        {
            List<string> codiciInseriti = new List<string> (); 
            db.getDocumentiFromDBByString(parola).ForEach(element =>
            {
                List<Autore> autoriList = new List<Autore>();
                db.getAutoriFromCodiceDocumento(long.Parse(element[1])).ForEach(element =>
                {
                    Autore nuovoAutore = new Autore(element.Item1, element.Item2, element.Item3);
                    autoriList.Add(nuovoAutore);
                });
                if (element[0] == "libro" && !codiciInseriti.Contains(element[1]))
                {
                    Libro nuovoLibro = new Libro(long.Parse(element[1]), element[2], element[3], element[4], int.Parse(element[7]), this.GetScaffaleFromNumber(element[6]), autoriList);
                    Console.WriteLine("{0}\n", nuovoLibro.ToString());
                    codiciInseriti.Add(element[1]);
                }
                else if(element[0] == "dvd" && !codiciInseriti.Contains(element[1]))
                {
                    DVD nuovoDvd = new DVD(long.Parse(element[1]), element[2], element[3], element[4], TimeSpan.Parse(element[7]), this.GetScaffaleFromNumber(element[6]), autoriList);
                    Console.WriteLine("{0}\n", nuovoDvd.ToString());
                    codiciInseriti.Add(element[1]);
                }
            });
        }

        public List<Prestito>? SearchPrestiti(string Numero)
        {
            return null;
        }

        public List<Prestito>? SearchPrestiti(string Nome, string Cognome)
        {
            return null;
        }
    }

    class Scaffale
    {
        public string Numero { get; set; }
        public string Sede { get; set; }
        public string Stanza { get; set; }

        public Scaffale(string Numero, string Sede, string Stanza)
        {
            this.Numero = Numero;
            this.Sede = Sede;
            this.Stanza = Stanza;
        }
    }
}
