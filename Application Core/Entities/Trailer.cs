using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Core.Entities
{
    [Table("Trailer")]
    public class Trailer
    {
        public int Id { get; set; }
        public int MovieId { get; set; }

        [MaxLength(2084)]
        public string TrailerUrl { get; set; }

        [MaxLength(2084)]
        public string Name { get; set; }

        // Navigation property
        public Movie Movie { get; set; }
    }
}
