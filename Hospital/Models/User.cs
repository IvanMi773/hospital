using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Hospital.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Surname { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfBorn { get; set; }

        public string PlaceOfBorn { get; set; }

        public UserRole UserRole { get; set; }
    }
}