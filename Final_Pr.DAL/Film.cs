using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Pr.DAL
{
    public class Film
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string Country { get; set; }

        public List<Role> Roles { get; set; } = new List<Role>();
        public List<Review> Reviews { get; set; } = new List<Review>();

    }
}
