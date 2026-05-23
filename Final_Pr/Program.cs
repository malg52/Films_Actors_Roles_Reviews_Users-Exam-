using Final_Pr.DAL;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Net;
using System.Transactions;

namespace Final_Pr
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(" ");
            Menu menu = new Menu();
            var rep = new Repository();
            var ins = new Insert();
            ins.InsertInto(rep);
            menu.ReadKey();

            while (true)
            {
                int choice;

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("1 - Movies,\n2 - Actors,\n3 - Reviews,\n0 - Exit.");
                    Console.Write("Enter: ");
                    string inp = Console.ReadLine();

                    if (int.TryParse(inp, out choice))
                    {
                        if (choice >= 0 && choice <= 3)
                            break;
                        else
                            Console.WriteLine("Error! Enter a number from 0 to 3.");
                        menu.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("Input error! Please enter a number.");
                        menu.ReadKey();
                    }
                }
                Console.Clear();
                switch (choice)
                {
                    case 0:
                        return;                      
                    case 1:
                        Films(rep, menu);                       
                        break;
                    case 2:
                        Actors(rep, menu);                       
                        break;
                    case 3:
                        Reviews(rep, menu);
                        break;

                }
            }
        }
        static void Films(Repository rep, Menu menu)
        {         
            
            while (true)
            {
                int choice;
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("1 - Show all movies,\n2 - Add movie,\n3 - Remove movie,\n4 - Update movie,\n5 - Search movie (by parameters),\n6 - Movie statistics,\n7 - Other,\n0 - Exit.");
                    choice = menu.EnterInt("Enter: ");

                    if (choice >= 0 && choice <= 7)
                        break;
                    else
                    {
                        Console.WriteLine("Error! Enter a number from 0 to 7.");
                        menu.ReadKey();
                    }
                }

                Console.Clear();
                switch (choice)
                {
                    case 0:
                        return;
                    case 1:
                        menu.Menu_ShowAllFilms(rep);
                        break;
                    case 2:
                        menu.Menu_AddFilm(rep);
                        break;
                    case 3:
                        menu.Menu_DeleteFilm(rep);
                        break;
                    case 4:
                        menu.Menu_UpdateFilm(rep);
                        break;
                    case 5:
                        menu.Menu_SearchFilm(rep);
                        break;
                    case 6:
                        menu.Menu_StatsFilm(rep);
                        break;
                    case 7:
                        menu.Menu_Other(rep);
                        break;
                }
            }
        }
        static void Actors(Repository rep, Menu menu)
        {
            while (true)
            {
                int choice;
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("1 - Show all actors,\n2 - Add actor,\n3 - Remove actor,\n4 - Update actor,\n5 - Actors / Roles,\n6 - Other,\n0 - Exit.");
                    choice = menu.EnterInt("Enter: ");

                    if (choice >= 0 && choice <= 6)
                        break;
                    else
                    {
                        Console.WriteLine("Error! Enter a number from 0 to 6.");
                        menu.ReadKey();
                    }
                }

                Console.Clear();
                switch (choice)
                {
                    case 0:
                        return;
                    case 1:
                        menu.Menu_ShowAllActors(rep);
                        break;
                    case 2:
                        menu.Menu_AddActor(rep);
                        break;
                    case 3:
                        menu.Menu_RemoveActor(rep);
                        break;
                    case 4:
                        menu.Menu_UpdateActor(rep);
                        break;
                    case 5:
                        menu.Menu_ActorRoles(rep);
                        break;
                    case 6:
                        menu.Menu_OtherActors(rep);
                        break;
                }
            }
        }
        static void Reviews(Repository rep, Menu menu)
        {
            while (true)
            {
                int choice;
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("1 - Show reviews for a movie,\n2 - Add review,\n3 - Remove review,\n4 - Update review,\n0 - Exit.");
                    choice = menu.EnterInt("Enter: ");

                    if (choice >= 0 && choice <= 4)
                        break;
                    else
                    {
                        Console.WriteLine("Error! Enter a number from 0 to 4.");
                        menu.ReadKey();
                    }
                }               
      
                Console.Clear();
                switch (choice)
                {
                    case 0:
                        return;
                    case 1:
                        menu.Menu_ShowAllReviewsToFilm(rep);
                        break;
                    case 2:
                        menu.Menu_AddReview(rep);
                        break;
                    case 3:
                        menu.Menu_DeleteReview(rep);
                        break;
                    case 4:
                        menu.Menu_UpdateReview(rep);
                        break;
                }
            }
        }
    }       
}

    
    
