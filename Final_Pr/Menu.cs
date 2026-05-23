using Final_Pr.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Pr
{
    public class Menu
    {
        public Menu() { }

        public void ReadKey()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        public string EnterString(string text)
        {
            while (true)
            {
                Console.Write(text);
                string input = Console.ReadLine()?.Trim();
                if (!string.IsNullOrWhiteSpace(input))
                    return input;

                Console.WriteLine("Error! Field cannot be empty.");
            }
        }
        public int EnterInt(string text)
        {
            while (true)
            {
                Console.Write(text);
                if (int.TryParse(Console.ReadLine(), out int num))
                    return num;

                Console.WriteLine("Error! Enter a number.");
            }
        }
        private DateTime EnterDate(string text)
        {
            while (true)
            {
                string input = EnterString(text);
                if (DateTime.TryParse(input, out DateTime date))
                    return date;

                Console.WriteLine("Error! Enter a valid date.");
            }
        }
        private string EnterRoleType()
        {
            while (true)
            {
                string type = EnterString("Role type (main/supporting): ").ToLower();
                if (type == "main" || type == "supporting")
                    return type;
                Console.WriteLine("Error! Enter only 'main' or 'supporting'.");
            }
        }

        //1.1 - Show all films
        public void PrintFilms(Film f)
        {
            if (f == null)
            {
                Console.WriteLine("Film not found.");
                return;
            }

            Console.WriteLine($"Id: {f.Id}, Title: {f.Title}, Year: {f.Year}, Country: {f.Country}");
        }
        public void PrintRoles(Film f)
        {
            if (f.Roles != null && f.Roles.Count > 0)
            {
                Console.WriteLine("Roles and actors:");
                foreach (var r in f.Roles)
                {
                    Console.WriteLine($" - Character: {r.Character}, Role type: {r.Type}, Actor: {r.Actor?.FullName}");
                }
            }
            else
            {
                Console.WriteLine("No roles found.");
            }
        }
        public void PrintReviews(Film f)
        {
            if (f.Reviews != null && f.Reviews.Count > 0)
            {
                Console.WriteLine("Reviews:");
                foreach (var r in f.Reviews)
                {
                    Console.WriteLine($" - User: {r.User?.Name}, Rating: {r.Rating}/10, Comment: {r.Comment}, Date: {r.Date.ToShortDateString()} ");
                }
            }
            else
            {
                Console.WriteLine("No reviews found.");
            }
        }
        public void PrintFilmFull(Film f)
        {
            PrintFilms(f);
            PrintRoles(f);
            PrintReviews(f);
        }
        public void Menu_ShowAllFilms(Repository rep)
        {
            Console.Clear();
            var films = rep.GetAllFilms();

            if (films.Count == 0)
                Console.WriteLine("No films found in the database.");
            else
            {
                Console.WriteLine("List of all films:");
                foreach (var f in films)
                {
                    PrintFilmFull(f);
                    Console.WriteLine("");
                }
            }
            ReadKey();
        }

        //1.2 - Add film
        public void Menu_AddFilm(Repository repo)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("   --- Add Film ---");
                string exit = EnterString("Exit? (y/n): ").ToLower();
                if (exit == "y") break;

                var film = AddFilmWithoutRoles(repo);
                if (film == null) continue; 

                string ans = EnterString("Do you want to add roles and actors? (y/n): ").ToLower();
                if (ans == "y")
                {
                    Menu_Add_Role_Actor(repo, ans, film);
                }

                ReadKey();
            }
        }
        private Film AddFilmWithoutRoles(Repository repo)
        {
            string title = EnterString("Enter film title: ");
            int year = EnterInt("Enter release year: ");
            string country = EnterString("Enter country: ");

            if (repo.GetAllFilms().Any(f => f.Title == title && f.Year == year))
            {
                Console.WriteLine("Such a film already exists in the database.");
                return null;
            }

            var film = repo.AddFilm(new Film
            {
                Title = title,
                Year = year,
                Country = country
            });

            Console.WriteLine("Film successfully added!");
            return film;
        }
        public void Menu_Add_Role_Actor(Repository repo, string ans, Film film)
        {
            while (ans == "y")
            {
                AddRoleToFilm(repo, film);
                ans = EnterString("Add another role? (y/n): ").ToLower();
            }
        }
        private void AddRoleToFilm(Repository repo, Film film)
        {
            Console.WriteLine("\n--- Add Role to Film ---");
            string character = EnterString("Character name: ");
            string type = EnterRoleType();

            Actor actor = null;
            string linkActor = EnterString("Do you want to link the role with an actor? (y/n): ").ToLower();
            if (linkActor == "y")
                actor = EnterOrSelectActor(repo);

            repo.AddRole(new Role
            {
                Character = character,
                Type = type,
                FilmId = film.Id,
                ActorId = actor?.Id
            });

            Console.WriteLine("Role successfully added!");
        }
        private Actor EnterOrSelectActor(Repository repo)
        {
            string exists = EnterString("Does the actor already exist? (y/n): ").ToLower();
            Actor actor = null;

            if (exists == "y")
            {
                string fullName = EnterString("Actor's full name: ");
                string country = EnterString("Actor's country: ");
                DateTime birth = EnterDate("Actor's birth date (dd.mm.yyyy): ");

                actor = repo.GetActorByDetails(fullName, country, birth);
                if (actor == null)
                {
                    Console.WriteLine("Actor not found, creating a new one...");
                    actor = repo.AddActor(new Actor
                    {
                        FullName = fullName,
                        Country = country,
                        BirthDate = birth
                    });
                }
            }
            else
            {
                string full = EnterString("Actor's full name: ");
                DateTime birth = EnterDate("Actor's birth date (dd.mm.yyyy): ");
                string country = EnterString("Actor's country: ");

                actor = repo.AddActor(new Actor
                {
                    FullName = full,
                    Country = country,
                    BirthDate = birth
                });
            }

            return actor;
        }
        

        //1.3 - Удалить фильм
        public void Menu_DeleteFilm(Repository repo)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("   --- Delete movie ---");
                string choice = EnterString("Exit? (y/n): ");
                if (choice == "y") break;

                string title = EnterString("Enter movie title: ");
                int year = EnterInt("Enter release year: ");

                var film = repo.GetAllFilms().FirstOrDefault(t => t.Title == title && t.Year == year);

                if (film == null)
                {
                    Console.WriteLine("Film not found.");
                    ReadKey();
                    continue;
                }

                PrintFilms(film);
                string ans = EnterString("Delete? (y/n): ").ToLower();
                if (ans == "y")
                {
                    repo.RemoveFilm(film.Id);
                    Console.WriteLine("Film deleted!");
                }
                else
                {
                    Console.WriteLine("Deletion canceled.");
                }
                ReadKey();
            }
        }

        //1.4 - Изменить фильм
        public void Menu_UpdateFilm(Repository repo)
        {
            Console.Clear();

            Console.WriteLine("   --- Update movie data ---");
            string title = EnterString("Enter movie title: ");
            int year = EnterInt("Enter release year: ");


            var film = repo.GetAllFilms().FirstOrDefault(t => t.Title == title && t.Year == year);

            if (film == null)
            {
                Console.WriteLine("Film not found.");
                ReadKey();
                return;
            }
            Console.WriteLine("Current data:");
            PrintFilmFull(film);
            while (true)
            {
                Console.WriteLine("\nWhat do you want to change?");
                Console.WriteLine("1 - Title\n2 - Release Year\n3 - Country\n4 - Roles\n5 - Actors\n0 - Exit");
                int choice = EnterInt("Enter: ");
                if (choice == 0)
                    break;

                UpdateFilmSwitch(film, choice, repo);
                repo.UpdateFilm(film);
                Console.Clear();
                Console.WriteLine("Update completed!");
                Console.WriteLine(" Updated movie:");
                PrintFilmFull(film);
            }
        }
        private void UpdateFilmSwitch(Film film, int choice, Repository repo)
        {
            switch (choice)
            {
                case 1: film.Title = EnterString("New title: "); break;
                case 2: film.Year = EnterInt("New release year: "); break;
                case 3: film.Country = EnterString("New country: "); break;

                case 4:
                    Console.WriteLine("   --- Editing roles ---");
                    PrintRoles(film);

                    string delAll = EnterString("Do you want to delete all roles? (y/n): ").ToLower();
                    if (delAll == "y")
                    {
                        foreach (var r in film.Roles.ToList())
                        {
                            repo.RemoveRole(r.Id);
                        }
                        film.Roles.Clear();
                        Console.WriteLine("All roles have been deleted!");
                    }
                    else
                    {
                        string delSome = EnterString("Do you want to delete specific roles? (y/n): ").ToLower();
                        if (delSome == "y")
                        {
                            while (true)
                            {
                                int roleIdd = EnterInt("Enter role Id to delete (0 to exit): ");
                                if (roleIdd == 0) break;
                                repo.RemoveRole(roleIdd);

                                var roleToRemove = film.Roles.FirstOrDefault(r => r.Id == roleIdd);
                                if (roleToRemove != null) film.Roles.Remove(roleToRemove);
                            }
                        }
                    }

                    string addRoles = EnterString("Do you want to add new roles? (y/n): ").ToLower();
                    while (addRoles == "y")
                    {
                        AddRoleToFilm(repo, film);
                        addRoles = EnterString("Add another role? (y/n): ").ToLower();
                    }
                    break;

                case 5:
                    Console.WriteLine("   --- Editing actors ---");
                    PrintRoles(film);

                    int roleId = EnterInt("Enter role Id to change actor: ");
                    var role = film.Roles.FirstOrDefault(r => r.Id == roleId);

                    if (role == null)
                    {
                        Console.WriteLine("Role not found!");
                        break;
                    }

                    Actor NewActor = EnterOrSelectActor(repo);
                    repo.UpdateRoleActor(role.Id, NewActor);
                    Console.WriteLine("Role actor updated!");
                    break;

                default:
                    Console.WriteLine("Error!");
                    break;
            }
        }


        //1.5 - Search movie by parameters
        public void Menu_SearchFilm(Repository repo)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Search by:\n1 - Title\n2 - Country\n3 - Year\n4 - Actor\n0 - Exit");
                int choice = EnterInt("Enter: ");
                if (choice == 0) return;
                SearchFilmSwitch(repo.GetAllFilms(), choice);
            }
        }
        private void SearchFilmSwitch(List<Film> AllFilms, int choice)
        {
            List<Film> films = null;
            Console.Clear();
            switch (choice)
            {
                case 1:
                    var title = EnterString("Title: ");
                    films = AllFilms.Where(t => t.Title == title).ToList();                
                    break;
                case 2:
                    var country = EnterString("Country: ");
                    films = AllFilms.Where(t => t.Country == country).ToList();
                    break;
                case 3:
                    var year = EnterInt("Year: ");
                    films = AllFilms.Where(t => t.Year == year).ToList();
                    break;
                case 4:
                    var actor = EnterString("Actor: ");
                    films = AllFilms.Where(f => f.Roles != null && f.Roles.Any(r => r.Actor != null && r.Actor.FullName == actor)).ToList();
                    break;
                default: Console.WriteLine("Error!"); break;
            }
            if (films != null && films.Count > 0)
            {
                foreach (var film in films)
                {
                    PrintFilms(film);
                    if (choice == 4) PrintRoles(film); 
                }
            }
            else
            {
                Console.WriteLine("Films not found.");
            }
            ReadKey();
        }


        //1.6 - Статистика фильмов
        public void Menu_StatsFilm(Repository repo)
        {
            Console.Clear();
            var films = repo.GetAllFilms();

            if (films.Count == 0)
            {
                Console.WriteLine("No films found.");
                ReadKey();
                return;
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine("--- Film Statistics ---");
                Console.WriteLine("1 - Average rating by films,\n2 - Top-5 films by average rating,\n3 - Film with highest rating,\n4 - Film with lowest rating,\n5 - Number of films by countries,\n6 - Number of films by years,\n0 - Exit");
                int choice = EnterInt("Select option: ");
                if (choice == 0)
                    break;
                Menu_StatsFilmSwich(choice, repo, films);
                ReadKey();
            }
        }
        public void Menu_StatsFilmSwich(int choice,Repository repo, List<Film> films)
        {
            Console.Clear();
            switch (choice)
            {
                case 1:
                    Console.WriteLine("Average rating by films:");
                    foreach (var f in films)
                    {
                        if (f.Reviews != null && f.Reviews.Count > 0)
                            Console.WriteLine($"{f.Title} ({f.Year}): {f.Reviews.Average(r => r.Rating)}/10");
                        else
                            Console.WriteLine($"{f.Title} ({f.Year}): No reviews");
                    }
                    break;

                case 2:
                    Console.WriteLine("Top-5 films by average rating:");
                    var top5 = films.Where(f => f.Reviews != null && f.Reviews.Count > 0).OrderByDescending(f => f.Reviews.Average(r => r.Rating)).Take(5);
                    int rank = 1;
                    foreach (var f in top5)
                    {
                        Console.WriteLine($"{rank}. {f.Title} ({f.Year}) - {f.Reviews.Average(r => r.Rating)}/10");
                        rank++;
                    }
                    break;
                case 3:
                    var bestFilm = films.Where(f => f.Reviews != null && f.Reviews.Count > 0).OrderByDescending(f => f.Reviews.Average(r => r.Rating)).FirstOrDefault();
                    if (bestFilm != null)
                        Console.WriteLine($"Film with highest rating: {bestFilm.Title} ({bestFilm.Year}) - {bestFilm.Reviews.Average(r => r.Rating)}/10");
                    else
                        Console.WriteLine("No films with reviews.");
                    break;
                case 4:
                    var worstFilm = films.Where(f => f.Reviews != null && f.Reviews.Count > 0).OrderBy(f => f.Reviews.Average(r => r.Rating)).FirstOrDefault();
                    if (worstFilm != null)
                        Console.WriteLine($"Film with lowest rating: {worstFilm.Title} ({worstFilm.Year}) - {worstFilm.Reviews.Average(r => r.Rating)}/10");
                    else
                        Console.WriteLine("No films with reviews.");
                    break;
                case 5:
                    Console.WriteLine("Number of films by countries:");
                    
                    Dictionary<string, int> countryCount = new Dictionary<string, int>();

                    foreach (var f in films)
                    {
                        if (countryCount.ContainsKey(f.Country))
                            countryCount[f.Country]++;
                        else
                            countryCount[f.Country] = 1;
                    }

                    foreach (var c in countryCount)
                    {
                        Console.WriteLine($"{c.Key}: {c.Value}");
                    }
                    break;

                case 6:
                    Console.WriteLine("Number of films by years:");

                    Dictionary<int, int> yearCount = new Dictionary<int, int>();
                    foreach (var f in films)
                    {
                        if (yearCount.ContainsKey(f.Year))
                            yearCount[f.Year]++;
                        else
                            yearCount[f.Year] = 1;
                    }

                    foreach (var y in yearCount)
                    {
                        Console.WriteLine($"{y.Key}: {y.Value}");
                    }
                    break;

                default:
                    Console.WriteLine("Error! Please select a valid option.");
                    break;
            }
        }


        //1.7 - Другое(Случайный фильм, Фильмы без отзывов, Количество ролей на фильм)
        public void Menu_Other(Repository repo)
        {
            var films = repo.GetAllFilms();

            if (films.Count == 0)
            {
                Console.WriteLine("No films in the database.");
                ReadKey();
                return;
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine("--- Other ---");
                Console.WriteLine("1 - Random film,\n2 - Films without reviews,\n3 - Number of roles per film,\n0 - Exit");
                int choice = EnterInt("Select an option: ");
                if (choice == 0)
                    break;
                Menu_OtherSwitch(choice, films, repo);
            }
        }
        public void Menu_OtherSwitch(int choice, List<Film> films, Repository repo)
         {
            switch (choice)
            {
                case 1:                 
                    Random rnd = new Random();
                    var film = films[rnd.Next(films.Count)];
                    Console.WriteLine("Random film:");
                    PrintFilms(film);
                    break;

                case 2:
                    var noReviews = new List<Film>();

                    foreach (var f in films)
                    {
                        if (f.Reviews == null || f.Reviews.Count == 0)
                            noReviews.Add(f);
                    }

                    if (noReviews.Count == 0)
                    {
                        Console.WriteLine("No films without reviews.");
                        return;
                    }

                    Console.WriteLine("Films without reviews:");
                    foreach (var f in noReviews)
                    {
                        PrintFilms(f);
                    }
                    break;

                case 3:
                    Console.WriteLine("Number of roles per film:");
                    foreach (var f in films)
                    {
                        int count = 0;
                        if (f.Roles != null)
                        {
                            count = f.Roles.Count;
                        }
                        Console.WriteLine($"{f.Title} ({f.Year}): {count} roles");
                    }
                    break;

                default:
                    Console.WriteLine("Error! Please select a valid option.");
                    break;
            }
            ReadKey();
         }


        //2.1 - Показать всех актеров
        public void Menu_ShowAllActors(Repository repo)
        {
            Console.Clear();
            var actors = repo.GetAllActors();
            if (actors.Count == 0)
            {
                Console.WriteLine("No actors in the database.");
            }
            else
            {
                Console.WriteLine("List of all actors:");
                foreach (var actor in actors)
                {
                    PrintActor(actor);
                }
            }
            ReadKey();
        }
        private void PrintActor(Actor actor)
        {
            Console.WriteLine($"Id: {actor.Id}, Full Name: {actor.FullName}, Country: {actor.Country}, Birth Date: {actor.BirthDate.ToShortDateString()}");

            if (actor.Roles != null && actor.Roles.Count > 0)
            {
                Console.WriteLine("Roles in films:");
                foreach (var role in actor.Roles)
                {
                    Console.WriteLine($" - {role.Character} ({role.Type}) in film '{role.Film.Title}'");
                }
            }
            else
            {
                Console.WriteLine("No roles found.");
            }
            Console.WriteLine(""); 
        }


        //2.2 - Adding an actor
        public void Menu_AddActor(Repository repo)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("   --- Adding a new actor ---");
                if (EnterString("Exit? (y/n): ").ToLower() == "y") break;

                string fullName = EnterString("Full Name: ");
                string country = EnterString("Country: ");
                DateTime birth = EnterDate("Birth Date (dd.mm.yyyy): ");

                var Actor = repo.GetActorByDetails(fullName, country, birth);
                Actor actor;
                if (Actor != null)
                {
                    Console.WriteLine("Such actor already exists.");
                    actor = Actor;
                }
                else
                {
                    actor = repo.AddActor(new Actor
                    {
                        FullName = fullName,
                        Country = country,
                        BirthDate = birth
                    });
                    Console.WriteLine("Actor successfully added!");
                }

                string addRoles = EnterString("Do you want to add roles to this actor? (y/n): ").ToLower();
                while (addRoles == "y")
                {
                    AddRoleToActor(repo, actor);
                    addRoles = EnterString("Add another role? (y/n): ").ToLower();
                }

                ReadKey();
            }
        }
        private void AddRoleToActor(Repository repo, Actor actor)
        {
            Console.WriteLine("\n--- Adding a role to the actor ---");
            string character = EnterString("Character Name: ");
            string type = EnterRoleType();

            string filmTitle = EnterString("Film Title: ");
            int filmYear = EnterInt("Film Release Year: ");
            Film film = repo.GetAllFilms().FirstOrDefault(f => f.Title == filmTitle && f.Year == filmYear);

            if (film == null)
            {
                Console.WriteLine("Film not found in the database.");
                string addNew = EnterString("Do you want to add a new film? (y/n): ").ToLower();
                if (addNew == "y")
                {
                    var newFilm = AddFilmWithoutRoles(repo); 
                    if (newFilm != null)
                        film = newFilm;
                    else
                        return;
                }
                else
                {
                    return;
                }
            }

            bool roleExists = false;
            if (film.Roles != null)
            {
                foreach (var r in film.Roles)
                {
                    if (r.Character == character && r.ActorId == actor.Id)
                    {
                        roleExists = true;
                        break;
                    }
                }
            }

            if (roleExists)
            {
                Console.WriteLine("Such role already exists for this actor in the specified film.");
                return;
            }

            Role takenRole = null;
            if (film.Roles != null)
            {
                foreach (var r in film.Roles)
                {
                    if (r.Character == character && r.ActorId != null && r.ActorId != actor.Id)
                    {
                        takenRole = r;
                        break;
                    }
                }
            }
            if (takenRole != null)
            {
                Console.WriteLine("Such role is already taken by another actor!");
                return;
            }

            Role existingRole = null;
            if (film.Roles != null)
            {
                foreach (var r in film.Roles)
                {
                    if (r.Character == character && r.ActorId == null)
                    {
                        existingRole = r;
                        break;
                    }
                }
            }

            if (existingRole != null)
            {
                existingRole.ActorId = actor.Id;
                repo.UpdateRoleActor(existingRole.Id, actor);
                Console.WriteLine("Such role already exists, and the actor has been linked to this role.");
                return;
            }

            repo.AddRole(new Role
            {
                Character = character,
                Type = type,
                FilmId = film.Id,
                ActorId = actor.Id
            });

            Console.WriteLine("Role successfully added!");
        }


        //2.3 - Удаление актера
        public void Menu_RemoveActor(Repository repo)
        {
            Console.Clear();
            Console.WriteLine("   --- Deleting actor ---");

            string fullName = EnterString("Actor Full Name: ");
            string country = EnterString("Actor Country: ");
            DateTime birth = EnterDate("Actor Birth Date (dd.mm.yyyy): ");

            var actor = repo.GetActorByDetails(fullName, country, birth);
            if (actor == null)
            {
                Console.WriteLine("Actor not found!");
                ReadKey();
                return;
            }

            var roles = repo.GetRolesByActor(actor.Id);

            string deleteRoles = EnterString("Delete all roles of the actor? (y/n): ").ToLower();
            if (deleteRoles == "y")
            {
                foreach (var role in roles)
                    repo.RemoveRole(role.Id);

                Console.WriteLine("All roles of the actor have been deleted!");
            }
            else
            {
                foreach (var role in roles)
                    repo.UpdateRoleActor(role.Id, null);

                Console.WriteLine("Actor has been unlinked from all roles.");
            }

            repo.RemoveActor(actor.Id);
            Console.WriteLine("Actor has been completely removed!");

            ReadKey();
        }


        //2.4 - Именения данных актера
        public void Menu_UpdateActor(Repository repo)
        {
            Console.Clear();
            Console.WriteLine("   --- Updating actor data ---");

            string fullName = EnterString("Enter actor's full name: ");
            string country = EnterString("Enter country: ");
            DateTime birth = EnterDate("Enter birth date (dd.mm.yyyy): ");

            var actor = repo.GetActorByDetails(fullName, country, birth);

            if (actor == null)
            {
                Console.WriteLine("Actor not found!");
                ReadKey();
                return;
            }

            Console.WriteLine("Current actor data:");
            PrintActor(actor);

            while (true)
            {
                Console.WriteLine("What do you want to change?");
                Console.WriteLine("1 - Full Name,\n2 - Country,\n3 - Birth Date,\n0 - Exit,");
                int choice = EnterInt("Enter: ");
                if (choice == 0)
                    break;

                UpdateActorSwitch(actor, choice, repo);
                Console.Clear();
                Console.WriteLine("Changes have been made!");
                Console.WriteLine("Updated actor data:");
                PrintActor(actor);
            }
            repo.UpdateActor(actor);
        }
        private void UpdateActorSwitch(Actor actor, int choice, Repository repo)
        {
            switch (choice)
            {
                case 1:
                    actor.FullName = EnterString("New Full Name: ");
                    break;

                case 2:
                    actor.Country = EnterString("New Country: ");
                    break;

                case 3:
                    actor.BirthDate = EnterDate("New Birth Date (dd.mm.yyyy): ");
                    break;

                default:
                    Console.WriteLine("Input error!");
                    break;
            }
        }



        //2.5 - актеры/роли
        public void Menu_ActorRoles(Repository repo)
        {
            Console.Clear();
            Console.WriteLine("  --- Actors / Roles ---");

            string fullName = EnterString("Enter actor's full name: ");
            string country = EnterString("Enter country: ");
            DateTime birth = EnterDate("Enter birth date (dd.mm.yyyy): ");

            var actor = repo.GetActorByDetails(fullName, country, birth);
            if (actor == null)
            {
                Console.WriteLine("Actor not found!");
                ReadKey();
                return;
            }

            while (true)
            {
                var roles = repo.GetRolesByActor(actor.Id);
                PrintActorRoles(roles);

                Console.WriteLine("1 - Remove role / Unlink role,\n2 - Remove ALL roles,\n3 - Add role to actor,\n4 - Show all roles of all actors,\n0 - Exit");

                int choice = EnterInt("Enter: ");
                switch (choice)
                {
                    case 0:
                        return;

                    case 1:
                        Console.Clear();
                        DeleteSingleActorRole(repo, actor, roles);
                        break;

                    case 2:
                        Console.Clear();
                        DeleteAllActorRoles(repo, actor, roles);
                        break;

                    case 3:
                        Console.Clear();
                        AddRoleToActor(repo, actor);
                        ReadKey();

                        break;
                    case 4:
                        Console.Clear();
                        PrintAllActorsRoles(repo);
                        ReadKey();
                        break;

                    default:
                        Console.WriteLine("Input error!");
                        break;
                }
                
            }
        }
        private void DeleteSingleActorRole(Repository repo, Actor actor, List<Role> roles)
        {
            if (roles == null || roles.Count == 0)
            {
                Console.WriteLine("Actor has no roles.");
                return;
            }

            Console.WriteLine("Enter role details to delete:");

            string character = EnterString("Character: ");
            string type = EnterRoleType();
            string filmTitle = EnterString("Film title: ");

            var role = roles.FirstOrDefault(r => r.Character == character && r.Type == type && r.Film != null && r.Film.Title == filmTitle);
            if (role == null)
            {
                Console.WriteLine("Role not found for this actor!");
                return;
            }

            string ans = EnterString("Delete role completely or unlink from actor? (delete/unlink): ").ToLower();

            if (ans == "delete")
            {
                repo.RemoveRole(role.Id);
                Console.WriteLine("Role deleted completely!");
                ReadKey();
            }
            else if (ans == "unlink")
            {
                repo.UpdateRoleActor(role.Id, null);
                Console.WriteLine("Role unlinked from actor!");
                ReadKey();
            }
            else
            {
                Console.WriteLine("Action not recognized.");
                ReadKey();
            }
        }
        private void DeleteAllActorRoles(Repository repo, Actor actor, List<Role> roles)
        {
            if (roles == null || roles.Count == 0)
            {
                Console.WriteLine("Actor has no roles.");
                return;
            }

            string ans = EnterString("Delete all roles completely or unlink from actor? (delete/unlink): ").ToLower();

            foreach (var role in roles)
            {
                if (ans == "delete")
                    repo.RemoveRole(role.Id);
                else if (ans == "unlink")
                    repo.UpdateRoleActor(role.Id, null);
            }

            if (ans == "delete")
                Console.WriteLine("All roles deleted completely!");
            else if (ans == "unlink")
                Console.WriteLine("All roles unlinked from actor!");
            else
                Console.WriteLine("Action not recognized.");
            ReadKey();
        }
        private void PrintActorRoles(List<Role> roles)
        {
            Console.Clear();
            Console.WriteLine("   --- Actor roles ---");

            if (roles == null || roles.Count == 0)
            {
                Console.WriteLine("No roles found.");
                return;
            }

            foreach (var r in roles)
            {
                Console.WriteLine(
                    $"Character: {r.Character} - Type: {r.Type} - Film: {r.Film?.Title}"
                );
            }
            ReadKey();
        }
        public void PrintAllActorsRoles(Repository repo)
        {
            Console.Clear();
            Console.WriteLine("   --- All roles of all actors ---");

            var actors = repo.GetAllActors();

            if (actors == null || actors.Count == 0)
            {
                Console.WriteLine("No actors found.");
                return;
            }

            foreach (var actor in actors)
            {
                Console.WriteLine($"Actor: {actor.FullName}, Country: {actor.Country}, Birth Date: {actor.BirthDate.ToShortDateString()}");

                var roles = actor.Roles;
                if (roles != null && roles.Count > 0)
                {
                    foreach (var r in roles)
                    {
                        Console.WriteLine($"  - Role: {r.Character} - Type: {r.Type} - Film: {r.Film?.Title}");
                    }
                }
                else
                {
                    Console.WriteLine("  No roles found.");
                }

                Console.WriteLine("");
            }
        }


        //2.6 - Other (Top 10 popular actors, Actors with the most roles, Search actors by movie)
        public void Menu_OtherActors(Repository repo)
        {
            var actors = repo.GetAllActors();

            if (actors.Count == 0)
            {
                Console.WriteLine("No actors found.");
                ReadKey();
                return;
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine("   --- Other (Actors) ---");
                Console.WriteLine("1 - Actor with the most roles,\n2 - Actors from a specific movie,\n3 - Random actor,\n0 - Exit");
                int choice = EnterInt("Select an option: ");
                if (choice == 0) break;

                Menu_OtherActorsSwitch(choice, actors, repo);
            }
        }
        public void Menu_OtherActorsSwitch(int choice, List<Actor> actors, Repository repo)
        {
            Console.Clear();

            switch (choice)
            {
                case 1:
                    Actor topActor = null;
                    int maxRoles = -1;

                    foreach (var a in actors)
                    {
                        int count = 0;
                        if (a.Roles != null)
                            count = a.Roles.Count;

                        if (count > maxRoles)
                        {
                            maxRoles = count;
                            topActor = a;
                        }
                    }

                    if (topActor != null)
                    {
                        Console.WriteLine($"Actor with the most roles: {topActor.FullName} ({topActor.Country}) - {maxRoles} roles");
                    }
                    else
                    {
                        Console.WriteLine("No roles found for any actor.");
                    }
                    break;

                case 2:
                    string Title = EnterString("Film title: ");
                    int Year = EnterInt("Film release year: ");

                    var actorsInFilm = new List<Actor>();
                    foreach (var a in actors)
                    {
                        if (a.Roles != null)
                        {
                            foreach (var r in a.Roles)
                            {
                                if (r.Film != null && r.Film.Title == Title && r.Film.Year == Year)
                                {
                                    actorsInFilm.Add(a);
                                    break;
                                }
                            }
                        }
                    }

                    if (actorsInFilm.Count > 0)
                    {
                        Console.WriteLine($"Actors who participated in the film '{Title}' ({Year}):");
                        foreach (var a in actorsInFilm)
                            Console.WriteLine($" - {a.FullName} ({a.Country})");
                    }
                    else
                    {
                        Console.WriteLine("No actors found in this film.");
                    }
                    break;

                case 3:
                    Random rnd = new Random();
                    var actorRandom = actors[rnd.Next(actors.Count)];
                    Console.WriteLine("Random actor:");
                    Console.WriteLine($"{actorRandom.FullName} ({actorRandom.Country}) - Birth date: {actorRandom.BirthDate.ToShortDateString()}");
                    break;

                default:
                    Console.WriteLine("Error! Please select a valid option.");
                    break;
            }
            ReadKey();
        }



        //3.1 - Show reviews for a film
        public void Menu_ShowAllReviewsToFilm(Repository repo)
        {
            Film film = SelectFilm(repo);
            if (film == null) return;

            if (film.Reviews == null || film.Reviews.Count == 0)
            {
                Console.WriteLine("No reviews found.");
                ReadKey();
                return;
            }

            Console.WriteLine($"Reviews for the film '{film.Title}' ({film.Year}):");
            foreach (var r in film.Reviews)
            {
                string userName;
                if (r.User != null)
                {
                    userName = r.User.Name;
                }
                else
                {
                    userName = "Unknown";
                }
                Console.WriteLine($"- User: {userName}, Rating: {r.Rating}/10, Comment: {r.Comment}, Date: {r.Date.ToShortDateString()}");
            }
            ReadKey();
        }
        private Film SelectFilm(Repository repo)
        {
            while (true)
            {
                string title = EnterString("Film title (or '0' to exit): ");
                if (title == "0") return null;

                int year = EnterInt("Film release year: ");
                Film film = repo.GetAllFilms().FirstOrDefault(f => f.Title == title && f.Year == year);

                if (film == null)
                {
                    Console.WriteLine("Film not found.");
                    ReadKey();
                    continue;
                }

                return film;
            }
        }

        //3.2 - Добавить отзыв
        public void Menu_AddReview(Repository repo)
        {
            Film film = SelectFilm(repo);
            if (film == null) return;

            while (true)
            {
                User user = LoginOrRegisterUser(repo);
                if (user == null) return;

                int rating = EnterInt("Rating (1-10): ");
                string comment = EnterString("Comment: ");
                DateTime date = DateTime.Now;

                Review review = new Review
                {
                    FilmId = film.Id,
                    Film = film,
                    UserId = user.Id,
                    User = user,
                    Rating = rating,
                    Comment = comment,
                    Date = date
                };

                repo.AddReview(review);
                Console.WriteLine("Review added successfully!");

                string addMore = EnterString("Do you want to add another review? (y/n): ").ToLower();
                if (addMore != "y")
                    break;
            }
            ReadKey();
        }
        private User LoginOrRegisterUser(Repository repo)
        {
            while (true)
            {
                string userName = EnterString("User name (or '0' to exit): ");
                if (userName == "0") return null;
                User user = repo.GetAllUsers().FirstOrDefault(u => u.Name == userName);

                if (user != null)
                {
                    string password = EnterString("Enter password: ");
                    if (user.Password == password)
                        return user;

                    Console.WriteLine("Incorrect password! Please try again.");
                    ReadKey();
                }
                else
                {
                    Console.WriteLine("User not found. Creating a new one.");
                    string password = EnterString("Set password: ");
                    string email = EnterString("Set email: ");
                    user = repo.AddUser(new User
                    {
                        Name = userName,
                        Password = password,
                        Email = email
                    });
                    Console.WriteLine("User created successfully!");
                    ReadKey();
                    return user;
                }
            }
        }


        //3.3 - Delete review
        public void Menu_DeleteReview(Repository repo)
        {
            Film film = SelectFilm(repo);
            if (film == null) return;

            User User = LoginOrRegisterUser(repo);
            if (User == null) return;

            while (true)
            {
                var userReviews = film.Reviews.Where(r => r.UserId == User.Id).ToList();
                if (userReviews.Count == 0)
                {
                    Console.WriteLine("You have no reviews for this movie.");
                    return;
                }

                Console.WriteLine("Your reviews for this movie:");
                foreach (var r in userReviews)
                {
                    Console.WriteLine($"Id: {r.Id}, Rating: {r.Rating}/10, Comment: {r.Comment}");
                }

                int Id = EnterInt("Enter review Id to delete: ");
                Review review = userReviews.FirstOrDefault(r => r.Id == Id);
                if (review == null)
                {
                    Console.WriteLine("Review not found.");
                    return;
                }

                repo.RemoveReview(review.Id);
                Console.WriteLine("Review deleted successfully.");

                string cont = EnterString("Do you want to delete another review? (y/n): ").ToLower();
                if (cont != "y") break;

                ReadKey();
            }
        }

        //3.4 - Изменить отзыв
        public void Menu_UpdateReview(Repository repo)
        {
            Film film = SelectFilm(repo);
            if (film == null) return;

            User User = LoginOrRegisterUser(repo);
            if (User == null) return;
         

            while (true)
            {
                var userReviews = film.Reviews.Where(r => r.UserId == User.Id).ToList();
                if (userReviews.Count == 0)
                {
                    Console.WriteLine("You have no reviews for this movie.");
                    return;
                }

                Console.WriteLine("Your reviews for this movie:");
                foreach (var r in userReviews)
                {
                    Console.WriteLine($"Id: {r.Id}, Rating: {r.Rating}/10, Comment: {r.Comment}");
                }

                int reviewId = EnterInt("Enter review Id to update: ");
                Review review = userReviews.FirstOrDefault(r => r.Id == reviewId);
                if (review == null)
                {
                    Console.WriteLine("Review not found.");
                    return;
                }

                bool editMore = true;
                while (editMore)
                {
                    Console.Clear();
                    Console.WriteLine("\nWhat do you want to change?");
                    Console.WriteLine("1 - Rating,\n2 - Comment,\n3 - Film\n0 - Exit");
                    int choice = EnterInt("Enter: ");

                    switch (choice)
                    {
                        case 1:
                            review.Rating = EnterInt("New rating (1-10): ");
                            review.Date = DateTime.Now;
                            repo.UpdateReview(review);
                            Console.WriteLine("Rating updated!");
                            break;

                        case 2:
                            review.Comment = EnterString("New comment: ");
                            review.Date = DateTime.Now;
                            repo.UpdateReview(review);
                            Console.WriteLine("Comment updated!");
                            break;

                        case 3:
                            Console.WriteLine("List of available films:");
                            var allFilms = repo.GetAllFilms();
                            foreach (var f in allFilms)
                                PrintFilms(f);

                            int newFilmId = EnterInt("Enter Id of the new film for the review: ");
                            Film newFilm = allFilms.FirstOrDefault(f => f.Id == newFilmId);

                            if (newFilm != null)
                            {
                                review.FilmId = newFilm.Id;
                                review.Film = newFilm;
                                review.Date = DateTime.Now;
                                repo.UpdateReview(review);
                                Console.WriteLine($"Review moved to film: {newFilm.Title} ({newFilm.Year})");
                            }
                            else
                            {
                                Console.WriteLine("Film not found.");
                            }
                            break;

                        case 0:
                            editMore = false;
                            break;

                        default:
                            Console.WriteLine("Invalid choice.");
                            break;
                    }
                    ReadKey();
                }

                string cont = EnterString("Do you want to edit another review? (y/n): ").ToLower();
                if (cont != "y") break;
                ReadKey();
            }
        }

    }
}
