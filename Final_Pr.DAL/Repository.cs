using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Final_Pr.DAL
{
    public class Repository
    {
        private readonly AppDbContext dbContext;

        public Repository()
        {
            dbContext = new AppDbContext();
        }
        public List<Film> GetAllFilms()
        {
            return dbContext.Films.Include(f => f.Roles).ThenInclude(r => r.Actor).Include(f => f.Reviews).ThenInclude(rv => rv.User).ToList();
        }

        public Film AddFilm(Film f)
        {
            dbContext.Films.Add(f);
            dbContext.SaveChanges();
            return f;
        }

        public void RemoveFilm(int filmId)
        {
            var film = dbContext.Films.FirstOrDefault(f => f.Id == filmId);
            if (film == null) return;

            dbContext.Films.Remove(film);
            dbContext.SaveChanges();
        }

        public void UpdateFilm(Film Upd_Film)
        {
            var film = dbContext.Films.First(f => f.Id == Upd_Film.Id);

            film.Title = Upd_Film.Title;
            film.Year = Upd_Film.Year;
            film.Country = Upd_Film.Country;

            dbContext.SaveChanges();
        }

        public int RemoveUnusedRoles()
        {
            var unRoles = dbContext.Roles.Where(r => r.FilmId == null || !dbContext.Films.Any(f => f.Id == r.FilmId)).ToList();

            int count = unRoles.Count;

            if (count > 0)
            {
                dbContext.Roles.RemoveRange(unRoles);
                dbContext.SaveChanges();
            }

            return count;
        }

        public void RemoveRole(int roleId)
        {
            var role = dbContext.Roles.Find(roleId);
            if (role != null)
            {
                dbContext.Roles.Remove(role);
                dbContext.SaveChanges();
            }
        }

        public void UpdateRoleActor(int roleId, Actor actor)
        {
            var role = dbContext.Roles.Find(roleId);
            if (role != null)
            {
                if (actor == null)
                    role.ActorId = null;
                else
                    role.ActorId = actor.Id;

                dbContext.SaveChanges();
            }
        }

        public Film GetFilmById(int id)
        {
            return dbContext.Films.FirstOrDefault(f => f.Id == id);
        }

        public Actor AddActor(Actor actor)
        {
            dbContext.Actors.Add(actor);
            dbContext.SaveChanges();
            return actor;
        }

        public Actor GetActorById(int id)
        {
            return dbContext.Actors.FirstOrDefault(a => a.Id == id);
        }

        public void AddRole(Role role)
        {
            dbContext.Roles.Add(role);
            dbContext.SaveChanges();
        }

        public Actor GetActorByDetails(string fullName, string country, DateTime birth)
        {
            return dbContext.Actors.FirstOrDefault(a => a.FullName == fullName && a.Country == country && a.BirthDate.Year == birth.Year && a.BirthDate.Month == birth.Month && a.BirthDate.Day == birth.Day);
        }
         
        public List<Actor> GetAllActors()
        {
            return dbContext.Actors.Include(a => a.Roles).ThenInclude(r => r.Film).ToList();
        }

        public List<Role> GetRolesByActor(int actorId)
        {
            return dbContext.Roles.Where(r => r.ActorId == actorId).ToList();
        }
        public void RemoveActor(int actorId)
        {
            var actor = dbContext.Actors.Find(actorId);
            if (actor != null)
            {
                dbContext.Actors.Remove(actor);
                dbContext.SaveChanges();
            }
        }

        public void UpdateActor(Actor updatedActor)
        {
            var actor = dbContext.Actors.FirstOrDefault(a => a.Id == updatedActor.Id);

            if (actor != null)
            {
                actor.FullName = updatedActor.FullName;
                actor.Country = updatedActor.Country;
                actor.BirthDate = updatedActor.BirthDate;

                dbContext.SaveChanges();
            }
        }

        public List<User> GetAllUsers()
        {
            return dbContext.Users.ToList();
        }

        public User AddUser(User user)
        {
            dbContext.Users.Add(user);
            dbContext.SaveChanges();
            return user;
        }

        public void AddReview(Review review)
        {
            dbContext.Reviews.Add(review);
            dbContext.SaveChanges();
        }

        public void RemoveReview(int id)
        {
            var review = dbContext.Reviews.FirstOrDefault(r => r.Id == id);
            if (review != null)
            {
                dbContext.Reviews.Remove(review);
                dbContext.SaveChanges();
            }
        }

        public void UpdateReview(Review review)
        {
            var rev = dbContext.Reviews.FirstOrDefault(r => r.Id == review.Id);
            if (rev != null)
            {
                rev.Rating = review.Rating;
                rev.Comment = review.Comment;
                rev.Date = review.Date;
                dbContext.SaveChanges();
            }
        }

    }
}
