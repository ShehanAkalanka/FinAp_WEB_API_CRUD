using FinAp_WEB_API_CRUD.Data;
using FinAp_WEB_API_CRUD.Data.DTOs;
using FinAp_WEB_API_CRUD.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FinAp_WEB_API_CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly FinApDbContext _Context;

        public StudentController(FinApDbContext DbContext)
        {
            _Context = DbContext;
        }

        // GET: api/student
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            try
            {
                var students = await _Context.Students.ToListAsync();
                return Ok(students); 
            }
            catch (TimeoutException)
            {
                return StatusCode(StatusCodes.Status408RequestTimeout, "The database query timed out.");
            }
            catch (Exception ex)
            {
                // Handle all other exceptions (including database-related ones)
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }

        // GET: api/student/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            try
            {
                var studentDetails = await _Context.Students
                    .Where(s => s.Id == id)
                    .Select(s => new StudentDetailsDTO
                    {
                        StudentId = s.Id,
                        StudentFirstName = s.FirstName,
                        StudentLastName=s.LastName,
                        StudentEmail = s.Email,
                        ContactedPerson = s.ContactedPerson,
                        ContactNo = s.ContactNo,
                        Age=s.Age,
                        DateOfBirth =s.DateOfBirth,
                        ClassroomId = s.ClassroomId,
                        ClassroomName = s.Classroom.Name,
                        Teachers = _Context.Teachers
                                .Where(t => t.TeacherClassrooms.Any(c => c.Id == s.Classroom.Id))
                                .Select(t => new TeacherDTO
                                {
                                    TeacherId = t.Id,
                                    TeacherName = $"{t.FirstName} {t.LastName}",
                                    Subjects = _Context.TeacherSubjects
                                        .Where(ts => ts.TeacherId == t.Id)
                                        .Select(sub => new SubjectDTO
                                        {
                                            SubjectId = sub.Subject.Id,
                                            SubjectName = sub.Subject.Name
                                        })
                                        .ToList()
                                })
                                .ToList()
                    })
                    .FirstOrDefaultAsync();

                if (studentDetails == null)
                {
                    return NotFound();
                }

                return Ok(studentDetails);
            }
            catch (TimeoutException)
            {
                return StatusCode(StatusCodes.Status408RequestTimeout, "The database query timed out.");
            }
            catch (Exception ex)
            {
                // Handle all other exceptions (including database-related ones)
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }

        // POST: api/student
        [HttpPost]
        public async Task<ActionResult<Student>> CreateStudent(Student student)
        {
            if (!ModelState.IsValid)//checks if the model passed into the action has passed model validation
            {
                return BadRequest(ModelState);
            }

            try
            {
                _Context.Students.Add(student);
                await _Context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);
            }
            catch (TimeoutException)
            {
                return StatusCode(StatusCodes.Status408RequestTimeout, "The database query timed out.");
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency conflicts
                return Conflict("Another user has updated the same record concurrently.");
            }
            catch (Exception ex)
            {
                // Handle all other exceptions (including database-related ones)
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }

        }

        // PUT: api/student/
        [HttpPut]
        public async Task<ActionResult<IEnumerable<Student>>> UpdateStudent(Student student)
        {

            if (!ModelState.IsValid)//checks if the model passed into the action has passed model validation
            {
                return BadRequest(ModelState);
            }

            _Context.Entry(student).State = EntityState.Modified;// indicates that the object represents a record in the database that has been modified

            try
            {
                await _Context.SaveChangesAsync();
            }
            catch (TimeoutException)
            {
                return StatusCode(StatusCodes.Status408RequestTimeout, "The database query timed out.");
            }
            catch (DbUpdateConcurrencyException)
            {
                var existingStudent = await _Context.Students.FindAsync(student.Id);
                if (existingStudent != null)
                {
                    return NotFound();
                }
                else
                {
                    return Conflict("Concurrency conflict occurred. Another user may have updated the Student.");
                }
            }
            catch (Exception ex)
            {
                // Handle all other exceptions (including database-related ones)
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }

            var updatedStudent = await _Context.Students.FindAsync(student.Id);

            if (updatedStudent == null)
            {
                return NotFound();
            }
            return Ok(updatedStudent);
        }

        // DELETE: api/student/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _Context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _Context.Students.Remove(student);

            try
            {
                await _Context.SaveChangesAsync();
            }
            catch (TimeoutException)
            {
                return StatusCode(StatusCodes.Status408RequestTimeout, "The database query timed out.");
            }
            catch (Exception ex)
            {
                // Handle all other exceptions (including database-related ones)
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }

            return NoContent();
        }
    }
}
