using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FinAp_WEB_API_CRUD.Models
{
    public class Teacher
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [Column(TypeName = "nvarchar(100)")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [Column(TypeName = "nvarchar(100)")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Contact Number is required")]
        [Column(TypeName = "nvarchar(10)")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Contact No must be a 10-digit number")]
        public string ContactNo { get; set; }

        [Required(ErrorMessage = "Email Address is required")]
        [Column(TypeName = "nvarchar(100)")]
        [EmailAddress]
        public string Email { get; set; }

        public ICollection<TeacherClassroom> TeacherClassrooms { get; set; }
        public ICollection<TeacherSubject> TeacherSubjects { get; set; }
    }
}
