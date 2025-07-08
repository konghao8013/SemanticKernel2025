using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK35SemanticSearchBasic.Models
{
    public class MovieResult2
    {
        public List<Movie2> Movies { get; set; }
    }

    public class Movie2
    {
        public string Name { get; set; }
        public string Director { get; set; }
        public string Actor { get; set; }
        public string[] Types { get; set; }
        public string ReleaseDate { get; set; }
        public string Description { get; set; }
    }
}
