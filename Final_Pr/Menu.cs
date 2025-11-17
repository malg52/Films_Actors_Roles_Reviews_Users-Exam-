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
            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
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

                Console.WriteLine("Ошибка! Поле не может быть пустым.");
            }
        }
        public int EnterInt(string text)
        {
            while (true)
            {
                Console.Write(text);
                if (int.TryParse(Console.ReadLine(), out int num))
                    return num;

                Console.WriteLine("Ошибка! Введите число.");
            }
        }
        private DateTime EnterDate(string text)
        {
            while (true)
            {
                string input = EnterString(text);
                if (DateTime.TryParse(input, out DateTime date))
                    return date;

                Console.WriteLine("Ошибка! Введите корректную дату.");
            }
        }
        private string EnterRoleType()
        {
            while (true)
            {
                string type = EnterString("Тип роли (main/supporting): ").ToLower();
                if (type == "main" || type == "supporting")
                    return type;
                Console.WriteLine("Ошибка! Введите только 'main' или 'supporting'.");
            }
        }

        //1.1 - Показать все фильмы
        public void PrintFilms(Film f)
        {
            if (f == null)
            {
                Console.WriteLine("Фильм не найден.");
                return;
            }

            Console.WriteLine($"Id: {f.Id}, Название: {f.Title}, Год выпуска: {f.Year}, Страна: {f.Country}");           
        }
        public void PrintRoles(Film f)
        {
            if (f.Roles != null && f.Roles.Count > 0)
            {
                Console.WriteLine("Роли и актёры:");
                foreach (var r in f.Roles)
                {
                    Console.WriteLine($" - Персонаж: {r.Character}, Тип роли: {r.Type}, Актёр: {r.Actor?.FullName}");
                }
            }
            else
            {
                Console.WriteLine("Ролей не найдено.");
            }
        }
        public void PrintReviews(Film f)
        {
            if (f.Reviews != null && f.Reviews.Count > 0)
            {
                Console.WriteLine("Отзывы:");
                foreach (var r in f.Reviews)
                {
                    Console.WriteLine($" - Пользователь: {r.User?.Name}, Оценка: {r.Rating}/10, Комментарий: {r.Comment}, Дата: {r.Date.ToShortDateString()} ");
                }
            }
            else
            {
                Console.WriteLine("Отзывов пока нет.");
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
                Console.WriteLine("Фильмов нет в базе.");
            else
            {
                Console.WriteLine("Список всех фильмов:");
                foreach (var f in films)
                {
                    PrintFilmFull(f);
                    Console.WriteLine("");
                }
            }
            ReadKey();
        }

        //1.2 - Добавить фильм
        public void Menu_AddFilm(Repository repo)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("   --- Добавление фильма ---");
                string exit = EnterString("Выйти? (y/n): ").ToLower();
                if (exit == "y") break;

                var film = AddFilmWithoutRoles(repo);
                if (film == null) continue; 

                string ans = EnterString("Хотите добавить роли и актёров? (y/n): ").ToLower();
                if (ans == "y")
                {
                    Menu_Add_Role_Actor(repo, ans, film);
                }

                ReadKey();
            }
        }
        private Film AddFilmWithoutRoles(Repository repo)
        {
            string title = EnterString("Введите название фильма: ");
            int year = EnterInt("Введите год выпуска: ");
            string country = EnterString("Введите страну: ");

            if (repo.GetAllFilms().Any(f => f.Title == title && f.Year == year))
            {
                Console.WriteLine("Такой фильм уже есть в базе.");
                return null;
            }

            var film = repo.AddFilm(new Film
            {
                Title = title,
                Year = year,
                Country = country
            });

            Console.WriteLine("Фильм успешно добавлен!");
            return film;
        }
        public void Menu_Add_Role_Actor(Repository repo, string ans, Film film)
        {
            while (ans == "y")
            {
                AddRoleToFilm(repo, film);
                ans = EnterString("Добавить ещё одну роль? (y/n): ").ToLower();
            }
        }
        private void AddRoleToFilm(Repository repo, Film film)
        {
            Console.WriteLine("\n--- Добавление роли к фильму ---");
            string character = EnterString("Имя персонажа: ");
            string type = EnterRoleType();

            Actor actor = null;
            string linkActor = EnterString("Хотите связать роль с актёром? (y/n): ").ToLower();
            if (linkActor == "y")
                actor = EnterOrSelectActor(repo);

            repo.AddRole(new Role
            {
                Character = character,
                Type = type,
                FilmId = film.Id,
                ActorId = actor?.Id
            });

            Console.WriteLine("Роль успешно добавлена!");
        }
        private Actor EnterOrSelectActor(Repository repo)
        {
            string exists = EnterString("Актёр уже существует? (y/n): ").ToLower();
            Actor actor = null;

            if (exists == "y")
            {
                string fullName = EnterString("ФИО актёра: ");
                string country = EnterString("Страна актёра: ");
                DateTime birth = EnterDate("Дата рождения (дд.мм.гггг): ");

                actor = repo.GetActorByDetails(fullName, country, birth);
                if (actor == null)
                {
                    Console.WriteLine("Актёр не найден, создаём нового...");
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
                string full = EnterString("ФИО актёра: ");
                DateTime birth = EnterDate("Дата рождения (дд.мм.гггг): ");
                string country = EnterString("Страна актёра: ");

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
                Console.WriteLine("   --- Удаление фильма ---");
                string choice = EnterString("Выйти? (y/n): ");
                if (choice == "y") break;

                string title = EnterString("Введите название фильма: ");              
                int year = EnterInt("Введите год выпуска: ");

                var film = repo.GetAllFilms().FirstOrDefault(t => t.Title == title && t.Year == year);

                if (film == null)
                {
                    Console.WriteLine("Фильм не найден.");
                    ReadKey();
                    continue;
                }

                PrintFilms(film);
                string ans = EnterString("Удалить? (y/n): ").ToLower();
                if (ans == "y")
                {
                    repo.RemoveFilm(film.Id);
                    Console.WriteLine("Фильм удалён!");
                }
                else
                {
                    Console.WriteLine("Удаление отменено.");
                }
                ReadKey();
            }
        }

        //1.4 - Изменить фильм
        public void Menu_UpdateFilm(Repository repo)
        {
            Console.Clear();

            Console.WriteLine("   --- Изменение данных фильма ---");
            string title = EnterString("Введите название фильма: ");
            int year = EnterInt("Введите год выпуска: ");


            var film = repo.GetAllFilms().FirstOrDefault(t => t.Title == title && t.Year == year);

            if (film == null)
            {
                Console.WriteLine("Фильм не найден.");
                ReadKey();
                return;
            }
            Console.WriteLine("Текущие данные:");
            PrintFilmFull(film);
            while (true)
            {             
                Console.WriteLine("\nЧто хотите изменить?");
                Console.WriteLine("1 - Название\n2 - Год выпуска\n3 - Страну\n4 - Роли\n5 - Актеров\n0 - Выход");
                int choice = EnterInt("Введите: ");
                if (choice == 0)
                    break;

                UpdateFilmSwitch(film, choice, repo);
                repo.UpdateFilm(film);
                Console.Clear();
                Console.WriteLine("Изменение выполнено!");
                Console.WriteLine(" Обновленный фильм:");
                PrintFilmFull(film);
            }
        }
        private void UpdateFilmSwitch(Film film, int choice, Repository repo)
        {
            switch (choice)
            {
                case 1: film.Title = EnterString("Новое название: "); break;
                case 2: film.Year = EnterInt("Новый год выпуска: "); break;
                case 3: film.Country = EnterString("Новая страна: "); ; break;
                case 4:
                    Console.WriteLine("   --- Изменение ролей ---");
                    PrintRoles(film);

                    string delAll = EnterString("Хотите удалить все роли? (y/n): ").ToLower();
                    if (delAll == "y")
                    {
                        foreach (var r in film.Roles.ToList())
                        {
                            repo.RemoveRole(r.Id);
                        }
                        film.Roles.Clear();
                        Console.WriteLine("Все роли удалены!");
                    }
                    else
                    {
                        string delSome = EnterString("Хотите удалить конкретные роли? (y/n): ").ToLower();
                        if (delSome == "y")
                        {
                            while (true)
                            {
                                int roleIdd = EnterInt("Введите Id роли для удаления (0 для выхода): ");
                                if (roleIdd == 0) break;
                                repo.RemoveRole(roleIdd);
                                var roleToRemove = film.Roles.FirstOrDefault(r => r.Id == roleIdd);
                                if (roleToRemove != null) film.Roles.Remove(roleToRemove);
                            }
                        }
                    }

                    string addRoles = EnterString("Хотите добавить новые роли? (y/n): ").ToLower();
                    while (addRoles == "y")
                    {
                        AddRoleToFilm(repo, film);
                        addRoles = EnterString("Добавить ещё одну роль? (y/n): ").ToLower();
                    }
                    break;
                case 5:
                    Console.WriteLine("   --- Изменение актеров ---");
                    PrintRoles(film);
                    int roleId = EnterInt("Введите Id роли, для которой хотите изменить актёра: ");
                    var role = film.Roles.FirstOrDefault(r => r.Id == roleId);

                    if (role == null)
                    {
                        Console.WriteLine("Роль не найдена!");
                        break;
                    }

                    Actor NewActor = EnterOrSelectActor(repo);
                    repo.UpdateRoleActor(role.Id, NewActor);
                    Console.WriteLine("Актёр роли обновлён!");
                    break;
                default: Console.WriteLine("Ошибка!"); break;
            }
        }


        //1.5 - Поиск фильма по параметрам
        public void Menu_SearchFilm(Repository repo)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Поиск по:\n1 - Названию\n2 - Стране\n3 - Году\n4 - Актеру\n0 - Выход");
                int choice = EnterInt("Введите: ");
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
                    var title = EnterString("Название: ");
                    films = AllFilms.Where(t => t.Title == title).ToList();                
                    break;
                case 2:
                    var country = EnterString("Страна: ");
                    films = AllFilms.Where(t => t.Country == country).ToList();
                    break;
                case 3:
                    var year = EnterInt("Год: ");
                    films = AllFilms.Where(t => t.Year == year).ToList();
                    break;
                case 4:
                    var actor = EnterString("Актер: ");
                    films = AllFilms.Where(f => f.Roles != null && f.Roles.Any(r => r.Actor != null && r.Actor.FullName == actor)).ToList();
                    break;
                default: Console.WriteLine("Ошибка!"); break;
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
                Console.WriteLine("Фильмы не найдены.");
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
                Console.WriteLine("Фильмов нет в базе.");
                ReadKey();
                return;
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine("--- Статистика фильмов ---");
                Console.WriteLine("1 - Средний рейтинг по фильмам,\n2 - Топ-5 фильмов по средней оценке,\n3 - Фильм с найбольшим рейтингом,\n4 - Фильм с найменшим рейтингом,\n5 - Количество фильмов по странам,\n6 - Количество фильмов по годам,\n0 - Выход");
                int choice = EnterInt("Выберите опцию: ");
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
                    Console.WriteLine("Средний рейтинг по фильмам:");
                    foreach (var f in films)
                    {
                        if (f.Reviews != null && f.Reviews.Count > 0)
                            Console.WriteLine($"{f.Title} ({f.Year}): {f.Reviews.Average(r => r.Rating)}/10");
                        else
                            Console.WriteLine($"{f.Title} ({f.Year}): Нет отзывов");
                    }
                    break;

                case 2:
                    Console.WriteLine("Топ-5 фильмов по средней оценке:");
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
                        Console.WriteLine($"Фильм с наибольшим рейтингом: {bestFilm.Title} ({bestFilm.Year}) - {bestFilm.Reviews.Average(r => r.Rating)}/10");
                    else
                        Console.WriteLine("Нет фильмов с отзывами.");
                    break;
                case 4:
                    var worstFilm = films.Where(f => f.Reviews != null && f.Reviews.Count > 0).OrderBy(f => f.Reviews.Average(r => r.Rating)).FirstOrDefault();
                    if (worstFilm != null)
                        Console.WriteLine($"Фильм с наименьшим рейтингом: {worstFilm.Title} ({worstFilm.Year}) - {worstFilm.Reviews.Average(r => r.Rating)}/10");
                    else
                        Console.WriteLine("Нет фильмов с отзывами.");
                    break;
                case 5:
                    Console.WriteLine("Количество фильмов по странам:");
                    
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
                    Console.WriteLine("Количество фильмов по годам:");

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
                    Console.WriteLine("Ошибка! Выберите корректный пункт.");
                    break;
            }
        }


        //1.7 - Другое(Случайный фильм, Фильмы без отзывов, Количество ролей на фильм)
        public void Menu_Other(Repository repo)
        {
            var films = repo.GetAllFilms();

            if (films.Count == 0)
            {
                Console.WriteLine("Фильмов нет в базе.");
                ReadKey();
                return;
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine("--- Другое ---");
                Console.WriteLine("1 - Случайный фильм,\n2 - Фильмы без отзывов,\n3 - Количество ролей на фильм,\n0 - Выход");
                int choice = EnterInt("Выберите опцию: ");
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
                    Console.WriteLine("Случайный фильм:");
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
                        Console.WriteLine("Все фильмы имеют отзывы.");
                        return;
                    }

                    Console.WriteLine("Фильмы без отзывов:");
                    foreach (var f in noReviews)
                    {
                        PrintFilms(f);
                    }
                    break;

                case 3:
                    Console.WriteLine("Количество ролей на фильм:");
                    foreach (var f in films)
                    {
                        int count = 0;
                        if (f.Roles != null)
                        {
                            count = f.Roles.Count;
                        }                            
                        Console.WriteLine($"{f.Title} ({f.Year}): {count} ролей");
                    }
                    break;

                default:
                    Console.WriteLine("Ошибка! Выберите корректный пункт.");
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
                Console.WriteLine("Актёров нет в базе.");
            }
            else
            {
                Console.WriteLine("Список всех актёров:");
                foreach (var actor in actors)
                {
                    PrintActor(actor);
                }
            }
            ReadKey();
        }
        private void PrintActor(Actor actor)
        {
            Console.WriteLine($"Id: {actor.Id}, ФИО: {actor.FullName}, Страна: {actor.Country}, Дата рождения: {actor.BirthDate.ToShortDateString()}");

            if (actor.Roles != null && actor.Roles.Count > 0)
            {
                Console.WriteLine("Роли в фильмах:");
                foreach (var role in actor.Roles)
                {
                    Console.WriteLine($" - {role.Character} ({role.Type}) в фильме '{role.Film.Title}'");
                }
            }
            else
            {
                Console.WriteLine("Роли не найдены.");
            }
            Console.WriteLine(""); 
        }
        

        //2.2 - Добавление актера
        public void Menu_AddActor(Repository repo)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("   --- Добавление нового актёра ---");
                if (EnterString("Выйти? (y/n): ").ToLower() == "y") break;

                string fullName = EnterString("ФИО актёра: ");
                string country = EnterString("Страна: ");
                DateTime birth = EnterDate("Дата рождения (дд.мм.гггг): ");

                var Actor = repo.GetActorByDetails(fullName, country, birth);
                Actor actor;
                if (Actor != null)
                {
                    Console.WriteLine("Такой актёр уже существует.");
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
                    Console.WriteLine("Актёр успешно добавлен!");
                }

                string addRoles = EnterString("Хотите добавить роли этому актёру? (y/n): ").ToLower();
                while (addRoles == "y")
                {
                    AddRoleToActor(repo, actor);
                    addRoles = EnterString("Добавить ещё одну роль? (y/n): ").ToLower();
                }

                ReadKey();
            }
        }
        private void AddRoleToActor(Repository repo, Actor actor)
        {
            Console.WriteLine("\n--- Добавление роли актёру ---");
            string character = EnterString("Имя персонажа: ");
            string type = EnterRoleType();

            string filmTitle = EnterString("Название фильма: ");
            int filmYear = EnterInt("Год выпуска фильма: ");
            Film film = repo.GetAllFilms().FirstOrDefault(f => f.Title == filmTitle && f.Year == filmYear);

            if (film == null)
            {
                Console.WriteLine("Фильма нет в базе.");
                string addNew = EnterString("Хотите добавить новый фильм? (y/n): ").ToLower();
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
                Console.WriteLine("Такая роль уже существует для этого актёра в указанном фильме.");
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
                Console.WriteLine("Данная роль уже занята другим актёром!");
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
                Console.WriteLine("Роль уже существует, актёр был связан с этой ролью.");
                return;
            }

            repo.AddRole(new Role
            {
                Character = character,
                Type = type,
                FilmId = film.Id,
                ActorId = actor.Id
            });

            Console.WriteLine("Роль успешно добавлена!");
        }


        //2.3 - Удаление актера
        public void Menu_RemoveActor(Repository repo)
        {
            Console.Clear();
            Console.WriteLine("   --- Удаление актёра ---");

            string fullName = EnterString("ФИО актёра: ");
            string country = EnterString("Страна актёра: ");
            DateTime birth = EnterDate("Дата рождения (дд.мм.гггг): ");

            var actor = repo.GetActorByDetails(fullName, country, birth);
            if (actor == null)
            {
                Console.WriteLine("Актёр не найден!");
                ReadKey();
                return;
            }

            var roles = repo.GetRolesByActor(actor.Id);

            string deleteRoles = EnterString("Удалить ВСЕ роли актёра? (y/n): ").ToLower();
            if (deleteRoles == "y")
            {
                foreach (var role in roles)
                    repo.RemoveRole(role.Id);

                Console.WriteLine("Все роли актёра удалены!");
            }
            else
            {
                foreach (var role in roles)
                    repo.UpdateRoleActor(role.Id, null);

                Console.WriteLine("Актёр был отвязан от всех ролей.");
            }

            repo.RemoveActor(actor.Id);
            Console.WriteLine("Актёр полностью удалён!");

            ReadKey();
        }


        //2.4 - Именения данных актера
        public void Menu_UpdateActor(Repository repo)
        {
            Console.Clear();
            Console.WriteLine("   --- Изменение данных актёра ---");

            string fullName = EnterString("Введите ФИО актёра: ");
            string country = EnterString("Введите страну: ");
            DateTime birth = EnterDate("Введите дату рождения (дд.мм.гггг): ");

            var actor = repo.GetActorByDetails(fullName, country, birth);

            if (actor == null)
            {
                Console.WriteLine("Актёр не найден!");
                ReadKey();
                return;
            }

            Console.WriteLine("Текущие данные актёра:");
            PrintActor(actor);

            while (true)
            {
                Console.WriteLine("Что хотите изменить?");
                Console.WriteLine("1 - ФИО,\n2 - Страну,\n3 - Дату рождения,\n0 - Выход,");
                int choice = EnterInt("Введите: ");
                if (choice == 0)
                    break;

                UpdateActorSwitch(actor, choice, repo);
                Console.Clear();
                Console.WriteLine("Изменение выполнено!");
                Console.WriteLine("Обновлённые данные актёра:");
                PrintActor(actor);
            }
            repo.UpdateActor(actor);
        }
        private void UpdateActorSwitch(Actor actor, int choice, Repository repo)
        {
            switch (choice)
            {
                case 1:
                    actor.FullName = EnterString("Новое ФИО: ");
                    break;

                case 2:
                    actor.Country = EnterString("Новая страна: ");
                    break;

                case 3:
                    actor.BirthDate = EnterDate("Новая дата рождения (дд.мм.гггг): ");
                    break;

                default:
                    Console.WriteLine("Ошибка ввода!");
                    break;
            }
        }



        //2.5 - актеры/роли
        public void Menu_ActorRoles(Repository repo)
        {
            Console.Clear();
            Console.WriteLine("  --- Актеры / Роли ---");

            string fullName = EnterString("Введите ФИО актёра: ");
            string country = EnterString("Введите страну актёра: ");
            DateTime birth = EnterDate("Введите дату рождения (дд.мм.гггг): ");

            var actor = repo.GetActorByDetails(fullName, country, birth);
            if (actor == null)
            {
                Console.WriteLine("Актёр не найден!");
                ReadKey();
                return;
            }

            while (true)
            {
                var roles = repo.GetRolesByActor(actor.Id);
                PrintActorRoles(roles);

                Console.WriteLine("1 - Удалить роль / отвязать роль,\n2 - Удалить ВСЕ роли,\n3 - Добавить роль актеру,\n4 - Вывести все роли всех актеров,\n0 - Выход");

                int choice = EnterInt("Введите: ");
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
                        Console.WriteLine("Ошибка выбора!");
                        break;
                }
                
            }
        }
        private void DeleteSingleActorRole(Repository repo, Actor actor, List<Role> roles)
        {
            if (roles == null || roles.Count == 0)
            {
                Console.WriteLine("У актёра нет ролей.");
                return;
            }

            Console.WriteLine("Введите данные роли для удаления:");

            string character = EnterString("Персонаж: ");
            string type = EnterRoleType();
            string filmTitle = EnterString("Название фильма: ");

            var role = roles.FirstOrDefault(r => r.Character == character && r.Type == type && r.Film != null && r.Film.Title == filmTitle);
            if (role == null)
            {
                Console.WriteLine("Роль не найдена у данного актёра!");
                return;
            }

            string ans = EnterString("Удалить роль полностью или отвязать от актёра? (delete/unlink): ").ToLower();

            if (ans == "delete")
            {
                repo.RemoveRole(role.Id);
                Console.WriteLine("Роль удалена полностью!");
                ReadKey();
            }
            else if (ans == "unlink")
            {
                repo.UpdateRoleActor(role.Id, null);
                Console.WriteLine("Роль отвязана от актёра!");
                ReadKey();
            }
            else
            {
                Console.WriteLine("Действие не распознано.");
                ReadKey();
            }
        }
        private void DeleteAllActorRoles(Repository repo, Actor actor, List<Role> roles)
        {
            if (roles == null || roles.Count == 0)
            {
                Console.WriteLine("У актёра нет ролей.");
                return;
            }

            string ans = EnterString("Удалить все роли полностью или отвязать их от актёра? (delete/unlink): ").ToLower();
            
            foreach (var role in roles)
            {
                if (ans == "delete")
                    repo.RemoveRole(role.Id);
                else if (ans == "unlink")
                    repo.UpdateRoleActor(role.Id, null);
            }

            if (ans == "delete")
                Console.WriteLine("Все роли удалены полностью!");
            else if (ans == "unlink")
                Console.WriteLine("Все роли отвязаны от актёра!");
            else
                Console.WriteLine("Действие не распознано.");
            ReadKey();
        }
        private void PrintActorRoles(List<Role> roles)
        {
            Console.Clear();
            Console.WriteLine("   --- Роли актёра ---");

            if (roles == null || roles.Count == 0)
            {
                Console.WriteLine("Ролей нет.");
                return;
            }

            foreach (var r in roles)
            {
                Console.WriteLine(
                    $"Персонаж: {r.Character} - Тип: {r.Type} - Фильм: {r.Film?.Title}"
                );
            }
            ReadKey();
        }
        public void PrintAllActorsRoles(Repository repo)
        {
            Console.Clear();
            Console.WriteLine("   --- Все роли всех актёров ---");

            var actors = repo.GetAllActors();

            if (actors == null || actors.Count == 0)
            {
                Console.WriteLine("Актёров нет в базе.");
                return;
            }

            foreach (var actor in actors)
            {
                Console.WriteLine($"Актёр: {actor.FullName}, Страна: {actor.Country}, Дата рождения: {actor.BirthDate.ToShortDateString()}");

                var roles = actor.Roles;
                if (roles != null && roles.Count > 0)
                {
                    foreach (var r in roles)
                    {
                        Console.WriteLine($"  - Роль: {r.Character} - Тип: {r.Type} - Фильм: {r.Film?.Title}");
                    }
                }
                else
                {
                    Console.WriteLine("  Ролей нет.");
                }

                Console.WriteLine("");
            }
        }


        //2.6 - Другое(Топ-10 популярных актёров, Актеры с наибольшим числом ролей, Поиск актеров по фильму)
        public void Menu_OtherActors(Repository repo)
        {
            var actors = repo.GetAllActors();

            if (actors.Count == 0)
            {
                Console.WriteLine("Актёров нет в базе.");
                ReadKey();
                return;
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine("   --- Другое (Актёры) ---");
                Console.WriteLine("1 - Актёр с наибольшим числом ролей,\n2 - Актёры из конкретного фильма,\n3 - Случайный актёр,\n0 - Выход");
                int choice = EnterInt("Выберите опцию: ");
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
                        Console.WriteLine($"Актёр с наибольшим числом ролей: {topActor.FullName} ({topActor.Country}) - {maxRoles} ролей");
                    }
                    else
                    {
                        Console.WriteLine("Ролей ни у одного актёра нет.");
                    }
                    break;

                case 2:
                    string Title = EnterString("Название фильма: ");
                    int Year = EnterInt("Год выпуска фильма: ");

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
                        Console.WriteLine($"Актёры, участвовавшие в фильме '{Title}' ({Year}):");
                        foreach (var a in actorsInFilm)
                            Console.WriteLine($" - {a.FullName} ({a.Country})");
                    }
                    else
                    {
                        Console.WriteLine("Актёров в этом фильме не найдено.");
                    }
                    break;

                case 3:
                    Random rnd = new Random();
                    var actorRandom = actors[rnd.Next(actors.Count)];
                    Console.WriteLine("Случайный актёр:");
                    Console.WriteLine($"{actorRandom.FullName} ({actorRandom.Country}) - Дата рождения: {actorRandom.BirthDate.ToShortDateString()}");
                    break;

                default:
                    Console.WriteLine("Ошибка! Выберите корректный пункт.");
                    break;
            }
            ReadKey();
        }



        //3.1 - Показать отзывы к фильму
        public void Menu_ShowAllReviewsToFilm(Repository repo)
        {
            Film film = SelectFilm(repo);
            if (film == null) return;

            if (film.Reviews == null || film.Reviews.Count == 0)
            {
                Console.WriteLine("Отзывы отсутствуют.");
                ReadKey();
                return;
            }

            Console.WriteLine($"Отзывы к фильму '{film.Title}' ({film.Year}):");
            foreach (var r in film.Reviews)
            {
                string userName;
                if (r.User != null)
                {
                    userName = r.User.Name;
                }
                else
                {
                    userName = "Неизвестный";
                }
                Console.WriteLine($"- Пользователь: {userName}, Оценка: {r.Rating}/10, Комментарий: {r.Comment}, Дата: {r.Date.ToShortDateString()}");
            }
            ReadKey();
        }
        private Film SelectFilm(Repository repo)
        {
            while (true)
            {
                string title = EnterString("Название фильма (или '0' для выхода): ");
                if (title == "0") return null;

                int year = EnterInt("Год выпуска фильма: ");
                Film film = repo.GetAllFilms().FirstOrDefault(f => f.Title == title && f.Year == year);

                if (film == null)
                {
                    Console.WriteLine("Фильм не найден.");
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

                int rating = EnterInt("Оценка (1-10): ");
                string comment = EnterString("Комментарий: ");
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
                Console.WriteLine("Отзыв успешно добавлен!");

                string addMore = EnterString("Хотите добавить ещё один отзыв? (y/n): ").ToLower();
                if (addMore != "y")
                    break;
            }
            ReadKey();
        }
        private User LoginOrRegisterUser(Repository repo)
        {
            while (true)
            {
                string userName = EnterString("Имя пользователя (или '0' для выхода): ");
                if (userName == "0") return null;
                User user = repo.GetAllUsers().FirstOrDefault(u => u.Name == userName);

                if (user != null)
                {
                    string password = EnterString("Введите пароль: ");
                    if (user.Password == password)
                        return user;

                    Console.WriteLine("Неверный пароль! Попробуйте снова.");
                    ReadKey();
                }
                else
                {
                    Console.WriteLine("Пользователь не найден. Создаём нового.");
                    string password = EnterString("Задайте пароль: ");
                    string email = EnterString("Задайте email: ");
                    user = repo.AddUser(new User
                    {
                        Name = userName,
                        Password = password,
                        Email = email
                    });
                    Console.WriteLine("Пользователь создан!");
                    ReadKey();
                    return user;
                }
            }
        }
        

        //3.3 - Удалить отзыв
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
                    Console.WriteLine("У вас нет отзывов для этого фильма.");
                    return;
                }

                Console.WriteLine("Ваши отзывы к этому фильму:");
                foreach (var r in userReviews)
                {
                    Console.WriteLine($"Id: {r.Id}, Оценка: {r.Rating}/10, Комментарий: {r.Comment}");
                }

                int Id = EnterInt("Введите Id отзыва для удаления: ");
                Review review = userReviews.FirstOrDefault(r => r.Id == Id);
                if (review == null)
                {
                    Console.WriteLine("Отзыв не найден.");
                    return;
                }

                repo.RemoveReview(review.Id);
                Console.WriteLine("Отзыв удалён.");

                string cont = EnterString("Хотите удалить ещё один отзыв? (y/n): ").ToLower();
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
                    Console.WriteLine("У вас нет отзывов для этого фильма.");
                    return;
                }

                Console.WriteLine("Ваши отзывы к этому фильму:");
                foreach (var r in userReviews)
                {
                    Console.WriteLine($"Id: {r.Id}, Оценка: {r.Rating}/10, Комментарий: {r.Comment}");
                }

                int reviewId = EnterInt("Введите Id отзыва для изменения: ");
                Review review = userReviews.FirstOrDefault(r => r.Id == reviewId);
                if (review == null)
                {
                    Console.WriteLine("Отзыв не найден.");
                    return;
                }

                bool editMore = true;
                while (editMore)
                {
                    Console.Clear();
                    Console.WriteLine("\nЧто хотите изменить?");
                    Console.WriteLine("1 - Оценку,\n2 - Комментарий,\n3 - Фильм\n0 - Выход");
                    int choice = EnterInt("Введите: ");

                    switch (choice)
                    {
                        case 1:
                            review.Rating = EnterInt("Новая оценка (1-10): ");
                            review.Date = DateTime.Now;
                            repo.UpdateReview(review);
                            Console.WriteLine("Оценка обновлена!");
                            break;

                        case 2:
                            review.Comment = EnterString("Новый комментарий: ");
                            review.Date = DateTime.Now;
                            repo.UpdateReview(review);
                            Console.WriteLine("Комментарий обновлён!");
                            break;

                        case 3:
                            Console.WriteLine("Список доступных фильмов:");
                            var allFilms = repo.GetAllFilms();
                            foreach (var f in allFilms)
                                PrintFilms(f);

                            int newFilmId = EnterInt("Введите Id нового фильма для отзыва: ");
                            Film newFilm = allFilms.FirstOrDefault(f => f.Id == newFilmId);

                            if (newFilm != null)
                            {
                                review.FilmId = newFilm.Id;
                                review.Film = newFilm;
                                review.Date = DateTime.Now;
                                repo.UpdateReview(review);
                                Console.WriteLine($"Отзыв перенесён на фильм: {newFilm.Title} ({newFilm.Year})");
                            }
                            else
                            {
                                Console.WriteLine("Фильм не найден.");
                            }
                            break;

                        case 0:
                            editMore = false;
                            break;

                        default:
                            Console.WriteLine("Неверный выбор.");
                            break;
                    }
                    ReadKey();
                }

                string cont = EnterString("Хотите редактировать ещё один отзыв? (y/n): ").ToLower();
                if (cont != "y") break;
                ReadKey();
            }
        }

    }
}
