using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_biblioteca_db
{
    enum Stato { Disponibile, Prestito }
    internal class Documento
    {
        public long Codice { get; set; }
        public string Titolo { get; set; }
        public string Anno { get; set; }
        public string Settore { get; set; }
        public Stato Stato { get; set; }
        public List<Autore> Autori { get; set; }
        public Scaffale Scaffale { get; set; }

        public Documento(long Codice, string Titolo, string Anno, string Settore, Scaffale Scaffale, List<Autore> listaDiAutori)
        {
            this.Codice = Codice;
            this.Titolo = Titolo;
            this.Settore = Settore;
            this.Anno = Anno;
            this.Autori = new List<Autore>();
            this.Autori = listaDiAutori;
            this.Scaffale = Scaffale;
            this.Stato = Stato.Disponibile;
        }

        public override string ToString()
        {
            return string.Format("Codice:{0}\nTitolo:{1}\nSettore:{2}\nStato:{3}\nScaffale numero:{4}",
                this.Codice,
                this.Titolo,
                this.Settore,
                this.Stato,
                this.Scaffale.Numero);
        }

        public void ImpostaInPrestito()
        {
            this.Stato = Stato.Prestito;
        }

        public void ImpostaDisponibile()
        {
            this.Stato = Stato.Disponibile;
        }

    }

    class Libro : Documento
    {
        public int NumeroPagine { get; set; }

        public Libro(long Codice, string Titolo, string Anno, string Settore, int NumeroPagine, Scaffale scaffale, List<Autore> listaAutori) : base(Codice, Titolo, Anno, Settore, scaffale, listaAutori)
        {
            this.NumeroPagine = NumeroPagine;
        }

        public override string ToString()
        {
            return string.Format("{0}\nNumeroPagine:{1}",
                base.ToString(),
                this.NumeroPagine);
        }
    }

    class DVD : Documento
    {

        public int Durata { get; set; }

        public DVD(long Codice, string Titolo, string Anno, string Settore, int Durata, Scaffale scaffale, List<Autore> listAutori) : base(Codice, Titolo, Anno, Settore, scaffale, listAutori)
        {
            this.Durata = Durata;
        }

        public override string ToString()
        {
            return string.Format("{0}\nDurata:{1}",
                base.ToString(),
                this.Durata);
        }
    }
}
