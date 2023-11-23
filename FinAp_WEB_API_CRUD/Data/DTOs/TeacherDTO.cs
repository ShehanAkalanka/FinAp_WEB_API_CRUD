using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace FinAp_WEB_API_CRUD.Data.DTOs
{
    public class TeacherDTO
    {
        public int TeacherId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TeacherName { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public List<SubjectDTO> Subjects { get; set; }
        public List<ClassroomDTO> Classrooms { get; set; }
    }
}
