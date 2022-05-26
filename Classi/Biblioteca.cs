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
                if(scaffaleInLista.Numero == scaffale.Numero)
                {
                    return true;
                }
            }
            return false;
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
                    List<Documento> result = SearchByAutore(inputAutore);
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

        public List<Documento> SearchByAutore(string? autore)
        {
            List<Documento> result = new List<Documento>();

            db.getDocumentiFromDBByAutore(autore);

            return result;
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
