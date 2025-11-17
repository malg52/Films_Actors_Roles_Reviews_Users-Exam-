using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Pr.DAL
{
    public class Role
    {
        public int Id { get; set; }
        public string Character { get; set; }
        public string Type { get; set; }

        public int FilmId { get; set; }
        public Film Film { get; set; }

        public int? ActorId { get; set; }
        public Actor Actor { get; set; }
    }
}
