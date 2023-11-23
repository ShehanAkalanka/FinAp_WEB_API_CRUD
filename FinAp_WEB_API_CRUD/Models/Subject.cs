using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace FinAp_WEB_API_CRUD.Models
{
    public class Subject
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Classroom Name is required")]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }
        public ICollection<TeacherSubject> TeacherSubjects { get; set; }
    }
}
