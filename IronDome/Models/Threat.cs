using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IronDome.Models
{
    public class Threat
    {
        [Key]
        int Id { get; set; }
        public Missle ?missle_type { get; set; }
        public TerrorOrg ?Org  { get; set; }

        [NotMapped]
        public int response_time {  get; set; }
        public bool isActive {  get; set; }
        public DateTime? fire_at { get; set; }
    }
}
