using System.ComponentModel.DataAnnotations;

namespace IronDome.Models
{
    public class Missle
    {
        [Key]
        public int id { get; set; }
        public string ? name {  get; set; }
        public int travel_speed { get; set; }
    }
}
