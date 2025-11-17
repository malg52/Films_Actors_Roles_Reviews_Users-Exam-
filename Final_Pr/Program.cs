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
                    Console.WriteLine("1 - Фильмы,\n2 - Актеры,\n3 - Отзывы,\n0 - выход.");
                    Console.Write("Введите: ");
                    string inp = Console.ReadLine();

                    if (int.TryParse(inp, out choice))
                    {
                        if (choice >= 0 && choice <= 3)
                            break;
                        else
                            Console.WriteLine("Ошибка! Введите число от 0 до 3.");
                        menu.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("Ошибка ввода! Введите число.");
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
                    Console.WriteLine("1 - Показать все фильмы,\n2 - Добавить фильм,\n3 - Удалить фильм,\n4 - Изменить фильм,\n5 - Поиск фильма(по параметрам),\n6 - Статистика фильмов,\n7 - Другое,\n0 - выход.");
                    choice = menu.EnterInt("Введите: ");

                    if (choice >= 0 && choice <= 7)
                        break;
                    else
                    {
                        Console.WriteLine("Ошибка! Введите число от 0 до 7.");
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
                    Console.WriteLine("1 - Показать всех актеров,\n2 - Добавить актера,\n3 - Удалить актера,\n4 - Изменить данные актера,\n5 - Актеры / Роли,\n6 - Другое,\n0 - выход.");  
                    choice = menu.EnterInt("Введите: ");

                    if (choice >= 0 && choice <= 6)
                        break;
                    else
                    {
                        Console.WriteLine("Ошибка! Введите число от 0 до 6.");
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
                    Console.WriteLine("1 - Показать отзывы к фильму,\n2 - Добавить отзыв,\n3 - Удалить отзыв,\n4 - Изменить отзыв,\n0 - выход.");
                    choice = menu.EnterInt("Введите: ");

                    if (choice >= 0 && choice <= 4)
                        break;
                    else
                    {
                        Console.WriteLine("Ошибка! Введите число от 0 до 4.");
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

    
    
