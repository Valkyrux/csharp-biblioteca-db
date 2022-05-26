using System;
using System.Data.SqlClient;

namespace csharp_biblioteca_db
{
    class Program
    {
       static void Main(string[] args)
        {
            Biblioteca b = new Biblioteca("Civica");

            List<Autore> listaDiAutori = new List<Autore>();
            listaDiAutori.Add(new Autore("Alessandro", "Manzoni", "alexmanzo@gmail.com"));
            b.aggiungiLibro("9", "I promessi sposi", "2001", "Romanzo", 2000, b.ScaffaliBiblioteca[0], listaDiAutori);

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