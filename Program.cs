using System;
using System.Data.SqlClient;

namespace csharp_biblioteca_db
{
    class Program
    {
       static void Main(string[] args)
        {
            Biblioteca b = new Biblioteca("Civica");

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