using System;
using System.Data.SqlClient;

namespace csharp_biblioteca_db
{
    class Program
    {
       static void Main(string[] args)
        {
            Biblioteca b = new Biblioteca("Civica");
            
            //la prossima istruzione inserisce nel db il codice di partenza per generare i codici unici dei documenti (PK)
            //db.setCodiceUnivocoDocumento(0);
            //Console.WriteLine(db.getCodiceUnicoDocumento());

            /*
            List<Autore> listaDiAutori = new List<Autore>();
            listaDiAutori.Add(new Autore("Alessandr", "Manzoni", "alexmanzo@gmail.com"));
            listaDiAutori.Add(new Autore("Benito", "Mussolini", "benito@nonsonofascista.hitler"));

            b.aggiungiLibro("I promessi sposi", "2001", "Romanzo", 2000, new Scaffale("S001", "Via Piave, 5", "Stanza A"), listaDiAutori);*/

            Console.WriteLine("LISTA OPERAZIONI\ncosa vuoi fare?");
            Console.WriteLine("\t1 -> cerca documento per autore");
            string? input = Console.ReadLine();
          
            while (input != null && input != "")
            {
                b.GestisciOperazioniBiblioteca(input);
                foreach (var item in b.ScaffaliBiblioteca)
                {
                    Console.WriteLine(item.Numero + " | " + item.Sede + " | " + item.Sede);
                }
                input = Console.ReadLine();
            }
        }
    }
}