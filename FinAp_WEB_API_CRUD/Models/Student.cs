using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace FinAp_WEB_API_CRUD.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "First Name is Required")]
        [Column(TypeName = "nvarchar(100)")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is Required")]
        [Column(TypeName = "nvarchar(100)")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Contacted Person is Required")]
        [Column(TypeName = "nvarchar(100)")]
        public string ContactedPerson { get; set; }

        [Required(ErrorMessage = "Contact Number is Required")]
        [Column(TypeName = "nvarchar(10)")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Contact No must be a 10-digit number")]
        public string ContactNo { get; set; }

        [Required(ErrorMessage = "Email Address is required")]
        [Column(TypeName = "nvarchar(100)")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Date Of Birth is Required")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [NotMapped]
        public int Age
        {
            get
            {
                // Calculate age based on DateOfBirth
                var today = DateTime.Today;
                var age = today.Year - DateOfBirth.Year;
                if (DateOfBirth.Date > today.AddYears(-age))
                    age--;

                return age;
            }
        }

        [Required(ErrorMessage = "Classroom is required")]
        public int ClassroomId { get; set; }

        [ForeignKey("ClassroomId")]
        public Classroom Classroom { get; set; }
    }
}
