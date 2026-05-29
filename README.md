# Film Management System 

Console application written in C# for managing films, actors, roles, users, and reviews.  
The project uses a layered architecture with a DAL (Data Access Layer) and Repository pattern.

---

## Features

### Films
- Add / update / delete films
- View all films with full details (roles + reviews)
- Search films by:
  - Title
  - Country
  - Year
  - Actor
- Film statistics:
  - Average rating
  - Top 5 films
  - Best / worst rated film
  - Films by country
  - Films by year
- Random film selection
- Films without reviews
- Count roles per film

---

### Actors
- Add / update / delete actors
- View all actors with roles
- Link actors to roles
- Search actors by film
- Actor statistics:
  - Actor with most roles
  - Random actor
  - Actors in a specific film
- Manage actor roles (add / remove / unlink)

---

### Reviews
- Add reviews (with user login/registration)
- Update reviews (rating, comment, film)
- Delete reviews
- View all reviews for a film

---

### Users
- Simple authentication system (login / register)
- Each review is connected to a user

---

## ⚙ Technologies

- C#
- .NET (Console Application)
- Entity Framework Core
- LINQ
- Repository Pattern

---
