using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace FinAp_WEB_API_CRUD.Models
{
    public class Classroom
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Classroom Name is required")]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }
        public ICollection<Student> Students { get; set; }
        public ICollection<TeacherClassroom> TeacherClassrooms { get; set; }
    }
}
