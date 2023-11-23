using FinAp_WEB_API_CRUD.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinAp_WEB_API_CRUD.Data.DTOs
{
    public class TeacherSubjectUpdateDTO
    {
        [Required(ErrorMessage = "Teacher Id is required")]
        public int TeacherId { get; set; }

        [Required(ErrorMessage = "Subject Id is required")]
        public int SubjectId { get; set; }

        [Required(ErrorMessage = "New Teacher Id is required")]
        public int NewTeacherId { get; set; }

        [Required(ErrorMessage = "New Subject Id is required")]
        public int NewSubjectId { get; set; }
    }
}
