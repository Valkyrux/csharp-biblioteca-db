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
                    List<Documento>? result = SearchByAutore(inputAutore);
                    if (result != null)
                    {
                        return 1;
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

        public List<Documento>? SearchByCodice(string Codice)
        {
            return null;
        }

        public List<Documento>? SearchByTitolo(string Titolo)
        {
            return null;
        }

        public List<Documento>? SearchByAutore(string? autore)
        {
            return null;
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
