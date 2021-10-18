using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Models
{
    public class Booking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
        
        [DataType(DataType.DateTime)]
        public DateTime DateFrom { get; set; }
        
        [DataType(DataType.DateTime)]
        public DateTime DateTo { get; set; }

        public string Description { get; set; }
    }
}