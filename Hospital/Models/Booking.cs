﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Hospital.Models
{
    public class Booking
    {
        public int Id { get; set; }

        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime DateFrom { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime DateTo { get; set; }

        public string Description { get; set; }
    }
}