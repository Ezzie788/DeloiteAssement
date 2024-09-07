using System;
using System.ComponentModel.DataAnnotations;

namespace DeloiteAssement.Models
{
    public class AppointmentFormModel
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "The Name field should contain only alphabetic characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Surname is required.")]
        [StringLength(50, ErrorMessage = "Surname cannot be longer than 50 characters.")]
        public string Surname { get; set; }

        public string MobileNumber { get; set; }

        public string Email { get; set; }

        [Required(ErrorMessage = "Date and time of appointment are required.")]
        public DateTime DateTime { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Product selection is required.")]
        public string Product { get; set; }

        public string Comments { get; set; }

        public bool IsValid()
        {
            // Check if either Email or MobileNumber is provided
            return !string.IsNullOrEmpty(Email) || !string.IsNullOrEmpty(MobileNumber);
        }
    }
}
