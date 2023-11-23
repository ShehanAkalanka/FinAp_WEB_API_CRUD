using FinAp_WEB_API_CRUD.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FinAp_WEB_API_CRUD.Data.DTOs
{
    public class TeacherClassroomUpdateDTO
    {
        [Required(ErrorMessage = "Teacher Id is required")]
        public int TeacherId { get; set; }

        [Required(ErrorMessage = "Classroom Id is required")]
        public int ClassroomId { get; set; }

        [Required(ErrorMessage = "New Teacher Id is required")]
        public int NewTeacherId { get; set; }

        [Required(ErrorMessage = "New Classroom Id is required")]
        public int NewClassroomId { get; set; }
    }
}
