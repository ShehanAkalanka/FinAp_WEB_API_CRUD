using System;
using System.Collections.Generic;

namespace FinAp_WEB_API_CRUD.Data.DTOs
{
    public class StudentDetailsDTO
    {
        public int StudentId { get; set; }
        public string StudentFirstName { get; set; }
        public string StudentLastName { get; set; }
        public string StudentEmail { get; set; }
        public string ContactedPerson { get; set; }
        public string ContactNo { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public int ClassroomId { get; set; }
        public string ClassroomName { get; set; }
        public List<TeacherDTO> Teachers { get; set; }
    }
}
