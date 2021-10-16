using System.Collections.Generic;

namespace Hospital.Models
{
    public class Clinic
    {
        public int Id { get; set; }

        public string Address { get; set; }
        public string Type { get; set; }
        
        public List<Doctor> Doctors { get; set; }
    }
}