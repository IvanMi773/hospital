using System.Collections.Generic;

namespace Hospital.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Surname { get; set; }
        
        public string CityOfWork { get; set; }
        public string PlaceOfWork { get; set; }
        public string Avatar { get; set; }
        public string TypeOfDoctor { get; set; }

        public int CostOfAdmission { get; set; }
        public int Experience { get; set; }

        public List<Clinic> Clinics { get; set; }
        public List<Booking> Bookings { get; set; }
    }
}