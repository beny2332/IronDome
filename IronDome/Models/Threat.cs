﻿using IronDome.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IronDome.Models
{
    public class Threat
    {
        public Threat() 
        {
            status = ThreatStatus.Inactive;
        }

        [Key]
        public int Id { get; set; }

        public Missle type { get; set; }

        public TerrorOrg Org  { get; set; }
        
        public ThreatStatus status { get; set; } //Active, Inactive, Failed, Succeeded

        public DateTime fire_at { get; set; }

        [NotMapped]
        public int response_time 
        {
            get  
            {
                return(int)( ((double)Org.distance / (double)type.travel_speed) * 3600); 
            }  
        }
    }
}
