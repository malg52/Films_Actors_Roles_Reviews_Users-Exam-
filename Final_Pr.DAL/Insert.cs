using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Pr.DAL
{
    public class Insert
    {
        public Insert() { }

        public void InsertInto(Repository repo)
        {
            var films = new List<Film>
            {
                new Film { Title = "Harry Potter and the Philosopher’s Stone", Year = 2001, Country = "UK" },
                new Film { Title = "Harry Potter and the Chamber of Secrets", Year = 2002, Country = "UK" },
                new Film { Title = "The Lord of the Rings: The Fellowship of the Ring", Year = 2001, Country = "New Zealand" },
                new Film { Title = "The Lord of the Rings: The Two Towers", Year = 2002, Country = "New Zealand" },
                new Film { Title = "Avatar", Year = 2009, Country = "USA" },
                new Film { Title = "Avatar 2", Year = 2022, Country = "USA" },
                new Film { Title = "The Matrix", Year = 1999, Country = "USA" },
                new Film { Title = "The Matrix Reloaded", Year = 2003, Country = "USA" },
                new Film { Title = "Spider-Man", Year = 2002, Country = "USA" },
                new Film { Title = "Spider-Man 2", Year = 2004, Country = "USA" },
                new Film { Title = "Iron Man", Year = 2008, Country = "USA" },
                new Film { Title = "Iron Man 2", Year = 2010, Country = "USA" },
                new Film { Title = "Joker", Year = 2019, Country = "USA" },
                new Film { Title = "Forrest Gump", Year = 1994, Country = "USA" },
                new Film { Title = "The Shawshank Redemption", Year = 1994, Country = "USA" },
                new Film { Title = "Dune", Year = 2021, Country = "USA" },
                new Film { Title = "Terminator 2: Judgment Day", Year = 1991, Country = "USA" },
                new Film { Title = "Interstellar", Year = 2014, Country = "USA" },
                new Film { Title = "Inception", Year = 2010, Country = "USA/UK" },
                new Film { Title = "Titanic", Year = 1997, Country = "USA" },
                new Film { Title = "Gladiator", Year = 2000, Country = "UK" },
                new Film { Title = "Pirates of the Caribbean: The Curse of the Black Pearl", Year = 2003, Country = "USA" },
                new Film { Title = "The Dark Knight", Year = 2008, Country = "UK" },
                new Film { Title = "Shrek", Year = 2001, Country = "USA" },
                new Film { Title = "Alien", Year = 1979, Country = "UK" },
                new Film { Title = "Venom", Year = 2018, Country = "USA" },
                new Film { Title = "Guardians of the Galaxy", Year = 2014, Country = "USA" },
                new Film { Title = "Avengers: Endgame", Year = 2019, Country = "USA" },
                new Film { Title = "The Lion King", Year = 1994, Country = "USA" },
                new Film { Title = "Wonder Woman", Year = 2017, Country = "USA" }
            };
            var actors = new List<Actor>
            {
                new Actor { FullName = "Daniel Radcliffe", BirthDate = new DateTime(1989,7,23), Country = "UK" },
                new Actor { FullName = "Rupert Grint", BirthDate = new DateTime(1988,8,24), Country = "UK" },
                new Actor { FullName = "Emma Watson", BirthDate = new DateTime(1990,4,15), Country = "UK" },
                new Actor { FullName = "Elijah Wood", BirthDate = new DateTime(1981,1,28), Country = "USA" },
                new Actor { FullName = "Ian McKellen", BirthDate = new DateTime(1939,5,25), Country = "UK" },
                new Actor { FullName = "Sam Worthington", BirthDate = new DateTime(1976,8,2), Country = "Australia" },
                new Actor { FullName = "Keanu Reeves", BirthDate = new DateTime(1964,9,2), Country = "Canada" },
                new Actor { FullName = "Tobey Maguire", BirthDate = new DateTime(1975,6,27), Country = "USA" },
                new Actor { FullName = "Robert Downey Jr.", BirthDate = new DateTime(1965,4,4), Country = "USA" },
                new Actor { FullName = "Joaquin Phoenix", BirthDate = new DateTime(1974,10,28), Country = "USA" },
                new Actor { FullName = "Tom Hanks", BirthDate = new DateTime(1956,7,9), Country = "USA" },
                new Actor { FullName = "Morgan Freeman", BirthDate = new DateTime(1937,6,1), Country = "USA" },
                new Actor { FullName = "Timothée Chalamet", BirthDate = new DateTime(1995,12,27), Country = "USA" },
                new Actor { FullName = "Arnold Schwarzenegger", BirthDate = new DateTime(1947,7,30), Country = "Austria" },
                new Actor { FullName = "Matthew McConaughey", BirthDate = new DateTime(1969,11,4), Country = "USA" },
                new Actor { FullName = "Leonardo DiCaprio", BirthDate = new DateTime(1974,11,11), Country = "USA" },
                new Actor { FullName = "Russell Crowe", BirthDate = new DateTime(1964,4,7), Country = "Australia" },
                new Actor { FullName = "Johnny Depp", BirthDate = new DateTime(1963,6,9), Country = "USA" },
                new Actor { FullName = "Christian Bale", BirthDate = new DateTime(1974,1,30), Country = "UK" },
                new Actor { FullName = "Mike Myers", BirthDate = new DateTime(1963,5,25), Country = "Canada" },
                new Actor { FullName = "Sigourney Weaver", BirthDate = new DateTime(1949,10,8), Country = "USA" },
                new Actor { FullName = "Tom Hardy", BirthDate = new DateTime(1977,9,15), Country = "UK" },
                new Actor { FullName = "Chris Pratt", BirthDate = new DateTime(1979,6,21), Country = "USA" },
                new Actor { FullName = "Zoe Saldana", BirthDate = new DateTime(1978,6,19), Country = "USA" },
                new Actor { FullName = "Chadwick Boseman", BirthDate = new DateTime(1976,11,29), Country = "USA" },
                new Actor { FullName = "Gal Gadot", BirthDate = new DateTime(1985,4,30), Country = "Israel" },
                new Actor { FullName = "Donald Glover", BirthDate = new DateTime(1983,9,25), Country = "USA" },
                new Actor { FullName = "Brad Pitt", BirthDate = new DateTime(1963,12,18), Country = "USA" },
                new Actor { FullName = "Margot Robbie", BirthDate = new DateTime(1990,7,2), Country = "Australia" },
                new Actor { FullName = "Hugh Jackman", BirthDate = new DateTime(1968,10,12), Country = "Australia" }

            };
            var roles = new List<Role>
            {
                new Role { Character = "Harry Potter", Type = "Main", FilmId = 1, ActorId = 1 },
                new Role { Character = "Harry Potter", Type = "Main", FilmId = 2, ActorId = 1 },
                new Role { Character = "Ron Weasley", Type = "Main", FilmId = 1, ActorId = 2 },
                new Role { Character = "Ron Weasley", Type = "Main", FilmId = 2, ActorId = 2 },
                new Role { Character = "Hermione Granger", Type = "Main", FilmId = 1, ActorId = null },
                new Role { Character = "Hermione Granger", Type = "Main", FilmId = 2, ActorId = 3 },
                new Role { Character = "Frodo Baggins", Type = "Main", FilmId = 3, ActorId = 4 },
                new Role { Character = "Frodo Baggins", Type = "Main", FilmId = 4, ActorId = 4 },
                new Role { Character = "Gandalf", Type = "Main", FilmId = 3, ActorId = 5 },
                new Role { Character = "Gandalf", Type = "Main", FilmId = 4, ActorId = null },
                new Role { Character = "Jake Sully", Type = "Main", FilmId = 5, ActorId = 6 },
                new Role { Character = "Jake Sully", Type = "Main", FilmId = 6, ActorId = null },
                new Role { Character = "Neo", Type = "Main", FilmId = 7, ActorId = 7 },
                new Role { Character = "Neo", Type = "Main", FilmId = 8, ActorId = 7 },
                new Role { Character = "Peter Parker", Type = "Main", FilmId = 9, ActorId = 8 },
                new Role { Character = "Peter Parker", Type = "Main", FilmId = 10, ActorId = 8 },
                new Role { Character = "Tony Stark", Type = "Main", FilmId = 11, ActorId = 9 },
                new Role { Character = "Tony Stark", Type = "Main", FilmId = 12, ActorId = 9 },
                new Role { Character = "Arthur Fleck", Type = "Main", FilmId = 13, ActorId = 10 },
                new Role { Character = "Arthur Fleck", Type = "Main", FilmId = 14, ActorId = 10 },
                new Role { Character = "Forrest Gump", Type = "Main", FilmId = 15, ActorId = null },
                new Role { Character = "Cooper", Type = "Main", FilmId = 16, ActorId = 11 },
                new Role { Character = "Jack Dawson", Type = "Main", FilmId = 17, ActorId = 11 },
                new Role { Character = "Lieutenant Dan", Type = "Supporting", FilmId = 15, ActorId = 12 },
                new Role { Character = "G-Man", Type = "Supporting", FilmId = 16, ActorId = null },
                new Role { Character = "Narrator", Type = "Supporting", FilmId = 17, ActorId = 12 },
                new Role { Character = "Paul Atreides", Type = "Main", FilmId = 18, ActorId = 13 },
                new Role { Character = "Tarrk", Type = "Supporting", FilmId = 19, ActorId = 13 },
                new Role { Character = "Leto Atreides", Type = "Supporting", FilmId = 20, ActorId = null },
                new Role { Character = "Terminator", Type = "Main", FilmId = 21, ActorId = 14 },
                new Role { Character = "T-800", Type = "Supporting", FilmId = 22, ActorId = 14 },
                new Role { Character = "Guardian", Type = "Supporting", FilmId = 23, ActorId = 14 },
                new Role { Character = "Cooper Nolan", Type = "Main", FilmId = 24, ActorId = 15 },
                new Role { Character = "Astronaut", Type = "Supporting", FilmId = 25, ActorId = 15 },
                new Role { Character = "Explorer", Type = "Supporting", FilmId = 26, ActorId = null },
                new Role { Character = "Maximus", Type = "Main", FilmId = 27, ActorId = 16 },
                new Role { Character = "Jack Sparrow", Type = "Main", FilmId = 28, ActorId = 17 },
                new Role { Character = "Bruce Wayne / Batman", Type = "Main", FilmId = 29, ActorId = 18 },
                new Role { Character = "Shrek", Type = "Main", FilmId = 30, ActorId = 19 },
                new Role { Character = "Ripley", Type = "Main", FilmId = 20, ActorId = 20 },
                new Role { Character = "Eddie Brock / Venom", Type = "Main", FilmId = 21, ActorId = 21 },
                new Role { Character = "Star-Lord", Type = "Main", FilmId = 22, ActorId = null },
                new Role { Character = "Gamora", Type = "Main", FilmId = 23, ActorId = 23 },
                new Role { Character = "Black Panther", Type = "Main", FilmId = 24, ActorId = 24 },
                new Role { Character = "Wonder Woman", Type = "Main", FilmId = 25, ActorId = 25 },
            };
            var users = new List<User>
            {
                new User { Name = "Alice", Password = "alice123", Email = "alice@mail.com" },
                new User { Name = "Bob", Password = "bob456", Email = "bob@mail.com" },
                new User { Name = "Charlie", Password = "charlie789", Email = "charlie@mail.com" },
                new User { Name = "Diana", Password = "diana111", Email = "diana@mail.com" },
                new User { Name = "Ethan", Password = "ethan222", Email = "ethan@mail.com" },
                new User { Name = "Fiona", Password = "fiona333", Email = "fiona@mail.com" },
                new User { Name = "George", Password = "george444", Email = "george@mail.com" },
                new User { Name = "Hannah", Password = "hannah555", Email = "hannah@mail.com" },
                new User { Name = "Ivan", Password = "ivan666", Email = "ivan@mail.com" },
                new User { Name = "Julia", Password = "julia777", Email = "julia@mail.com" }
            };
            var reviews = new List<Review>
            {
                new Review { FilmId = 1, UserId = 1, Rating = 9, Comment = "Amazing movie!", Date = new DateTime(2021, 6, 15) },
                new Review { FilmId = 1, UserId = 2, Rating = 8, Comment = "Really enjoyed it.", Date = new DateTime(2021, 6, 18) },
                new Review { FilmId = 2, UserId = 3, Rating = 10, Comment = "Epic adventure!", Date = new DateTime(2021, 7, 5) },
                new Review { FilmId = 3, UserId = 4, Rating = 9, Comment = "Visual masterpiece.", Date = new DateTime(2020, 12, 22) },
                new Review { FilmId = 4, UserId = 5, Rating = 10, Comment = "Mind-blowing!", Date = new DateTime(2020, 12, 25) },
                new Review { FilmId = 5, UserId = 6, Rating = 8, Comment = "Great action.", Date = new DateTime(2019, 5, 10) },
                new Review { FilmId = 6, UserId = 7, Rating = 9, Comment = "Iron Man rocks!", Date = new DateTime(2019, 5, 15) },
                new Review { FilmId = 7, UserId = 8, Rating = 8, Comment = "Dark and intense.", Date = new DateTime(2018, 9, 30) },
                new Review { FilmId = 8, UserId = 9, Rating = 10, Comment = "Heartwarming.", Date = new DateTime(2018, 10, 3) },
                new Review { FilmId = 9, UserId = 10, Rating = 9, Comment = "Classic!", Date = new DateTime(2017, 2, 14) },
                new Review { FilmId = 10, UserId = 1, Rating = 9, Comment = "Sci-Fi brilliance.", Date = new DateTime(2017, 3, 1) },
                new Review { FilmId = 11, UserId = 2, Rating = 8, Comment = "Action packed!", Date = new DateTime(2016, 8, 20) },
                new Review { FilmId = 12, UserId = 3, Rating = 9, Comment = "Amazing visuals.", Date = new DateTime(2016, 9, 2) },
                new Review { FilmId = 13, UserId = 4, Rating = 10, Comment = "Complex but brilliant.", Date = new DateTime(2015, 11, 11) },
                new Review { FilmId = 14, UserId = 5, Rating = 9, Comment = "Romantic and tragic.", Date = new DateTime(2015, 12, 24) },
                new Review { FilmId = 15, UserId = 6, Rating = 9, Comment = "Epic battle scenes.", Date = new DateTime(2014, 7, 4) },
                new Review { FilmId = 16, UserId = 7, Rating = 8, Comment = "Fun pirate adventure.", Date = new DateTime(2014, 7, 10) },
                new Review { FilmId = 17, UserId = 8, Rating = 10, Comment = "Best Batman ever!", Date = new DateTime(2013, 10, 31) },
                new Review { FilmId = 18, UserId = 9, Rating = 8, Comment = "Hilarious!", Date = new DateTime(2013, 11, 2) },
                new Review { FilmId = 19, UserId = 10, Rating = 9, Comment = "Sci-Fi horror classic.", Date = new DateTime(2012, 4, 18) },
                new Review { FilmId = 20, UserId = 1, Rating = 7, Comment = "Good antihero story.", Date = new DateTime(2012, 5, 5) },
                new Review { FilmId = 21, UserId = 2, Rating = 8, Comment = "Fun superhero team.", Date = new DateTime(2011, 6, 12) },
                new Review { FilmId = 22, UserId = 3, Rating = 10, Comment = "Epic finale!", Date = new DateTime(2011, 7, 19) },
                new Review { FilmId = 23, UserId = 4, Rating = 9, Comment = "Great animation.", Date = new DateTime(2010, 8, 23) },
                new Review { FilmId = 24, UserId = 5, Rating = 9, Comment = "Awesome movie!", Date = new DateTime(2010, 9, 7) },
                new Review { FilmId = 25, UserId = 6, Rating = 8, Comment = "Strong female lead.", Date = new DateTime(2009, 12, 15) }
            };


            foreach (var actor in actors)
            {
                if (repo.GetActorByDetails(actor.FullName, actor.Country, actor.BirthDate) == null)
                {
                    repo.AddActor(actor);
                }
            }



            foreach (var film in films)
            {
                if (!repo.GetAllFilms().Any(f => f.Title == film.Title && f.Year == film.Year))
                {
                    repo.AddFilm(film);
                }
            }


            foreach (var role in roles)
            {
                var film = repo.GetFilmById(role.FilmId);
                Actor actor = null;

                if (role.ActorId.HasValue)
                {
                    actor = repo.GetActorById(role.ActorId.Value);
                }
                else if (role.Actor != null)
                {
                    actor = repo.GetAllActors().FirstOrDefault(a => a.FullName == role.Actor.FullName && a.BirthDate == role.Actor.BirthDate && a.Country == role.Actor.Country);
                }

                if (film != null)
                {
                    bool exists = film.Roles.Any(r => r.Character == role.Character && r.Type == role.Type && r.ActorId == actor?.Id);

                    if (!exists)
                    {
                        repo.AddRole(new Role
                        {
                            Character = role.Character,
                            Type = role.Type,
                            FilmId = film.Id,
                            ActorId = actor?.Id
                        });
                    }
                }
            }



            foreach (var user in users)
            {
                if (!repo.GetAllUsers().Any(u => u.Name == user.Name))
                {
                    repo.AddUser(user);
                }
            }


            foreach (var review in reviews)
            {
                var film = repo.GetFilmById(review.FilmId);
                var user = repo.GetAllUsers().FirstOrDefault(u => u.Id == review.UserId);

                if (film != null && user != null)
                {
                    bool exists = film.Reviews.Any(r => r.UserId == user.Id && r.Comment == review.Comment && r.Rating == review.Rating && r.Date == review.Date);

                    if (!exists)
                    {
                        repo.AddReview(new Review
                        {
                            FilmId = film.Id,
                            UserId = user.Id,
                            Rating = review.Rating,
                            Comment = review.Comment,
                            Date = review.Date
                        });
                    }
                }
            }

            Console.WriteLine("Все данные успешно записаны в базу!");

        }
    }
}
