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
        public string Codice { get; set; }
        public string Titolo { get; set; }
        public int Anno { get; set; }
        public string Settore { get; set; }
        public Stato Stato { get; set; }
        public List<Autore> Autori { get; set; }
        public Scaffale Scaffale { get; set; }

        public Documento(string Codice, string Titolo, int Anno, string Settore, string numeroScaffale, string sede, string stanza)
        {
            this.Codice = Codice;
            this.Titolo = Titolo;
            this.Settore = Settore;
            this.Autori = new List<Autore>();
            this.Scaffale = new Scaffale(numeroScaffale, sede, stanza);
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

        public Libro(string Codice, string Titolo, int Anno, string Settore, int NumeroPagine, string numeroScaffale, string sede, string stanza) : base(Codice, Titolo, Anno, Settore, numeroScaffale, sede, stanza)
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

        public DVD(string Codice, string Titolo, int Anno, string Settore, int Durata, string numeroScaffale, string sede, string stanza) : base(Codice, Titolo, Anno, Settore, numeroScaffale, sede, stanza)
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
