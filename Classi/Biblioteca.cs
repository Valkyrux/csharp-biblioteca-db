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

        protected Scaffale GetScaffaleFromNumber(string Numero)
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
                    Console.WriteLine("\t-> Inserisci autore");
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
                        int numPagine;
                        TimeSpan duranta;

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
                            Console.WriteLine("-> Inserisci Numero di Pagine");
                            int.TryParse(Console.ReadLine(), out numPagine);
                            this.aggiungiLibro(titolo, anno.ToString(), settore, numPagine, GetScaffaleFromNumber(numeroScaffale), Autori);
                        }
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

        public List<Documento>? SearchByCodice(string Codice)
        {
            return null;
        }

        public List<Documento>? SearchByTitolo(string Titolo)
        {
            return null;
        }

        public void SearchByAutore(string? autore)
        {
            db.getDocumentiFromDBByAutore(autore).ForEach(element =>
            {
                List<Autore> autoriList = new List<Autore>();
                db.getAutoriFromCodiceDocumento(long.Parse(element[1])).ForEach(element =>
                {
                    Autore nuovoAutore = new Autore(element.Item1, element.Item2, element.Item3);
                    autoriList.Add(nuovoAutore);
                });
                if (element[0] == "libro")
                {
                    Libro nuovoLibro = new Libro(long.Parse(element[1]), element[2], element[3], element[4], int.Parse(element[7]), this.GetScaffaleFromNumber(element[6]), autoriList);
                    Console.WriteLine("{0}\n", nuovoLibro.ToString());
                }
                else if(element[0] == "dvd")
                {
                    DVD nuovoDvd = new DVD(long.Parse(element[1]), element[2], element[3], element[4], TimeSpan.Parse(element[7]), this.GetScaffaleFromNumber(element[6]), autoriList);
                    Console.WriteLine("{0}\n", nuovoDvd.ToString());
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
