using FinAp_WEB_API_CRUD.Data;
using FinAp_WEB_API_CRUD.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using FinAp_WEB_API_CRUD.Data.DTOs;

namespace FinAp_WEB_API_CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly FinApDbContext _Context;

        public TeacherController(FinApDbContext DbContext)
        {
            _Context = DbContext;
        }

        // GET: api/Teachers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Teacher>>> GetTeachers()
        {
            try
            {
                var teachers = await _Context.Teachers.ToListAsync();
                return Ok(teachers);
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

        // GET: api/Teachers/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Teacher>> GetTeacher(int id)
        {
            try
            {
                var teacher = await _Context.Teachers
                    .Where(teacher => teacher.Id == id)
                    .Select(teacher => new TeacherDTO
                    {
                        TeacherId = teacher.Id,
                        FirstName = teacher.FirstName,
                        LastName = teacher.LastName,
                        Email = teacher.Email,
                        ContactNumber = teacher.ContactNo,

                        Subjects = teacher.TeacherSubjects
                            .Select(ts => new SubjectDTO
                            {
                                SubjectId = ts.Subject.Id,
                                SubjectName = ts.Subject.Name
                            })
                            .ToList(),
                        Classrooms = teacher.TeacherClassrooms
                            .Select(tc => new ClassroomDTO
                            {
                                ClassroomId = tc.Classroom.Id,
                                ClassroomName = tc.Classroom.Name
                            })
                            .ToList()
                    })
                    .ToListAsync();

                if (teacher == null)
                {
                    return NotFound();
                }

                return Ok(teacher);
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

        // POST: api/Teachers
        [HttpPost]
        public async Task<ActionResult<Teacher>> CreateTeachers(Teacher teacher)
        {
            if (!ModelState.IsValid)//checks if the model passed into the action has passed model validation
            {
                return BadRequest(ModelState);
            }

            try
            {
                _Context.Teachers.Add(teacher);
                await _Context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetTeacher), new { id = teacher.Id }, teacher);
            }
            catch (TimeoutException)
            {
                return StatusCode(StatusCodes.Status408RequestTimeout, "The database query timed out.");
            }
            catch (DbUpdateConcurrencyException)
            {
                // -------------Handle concurrency conflicts
                return Conflict("Another user has updated the same record concurrently.");
            }
            catch (Exception ex)
            {
                // Handle all other exceptions (including database-related ones)
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }

        // PUT: api/Teacher/
        [HttpPut]
        public async Task<ActionResult<IEnumerable<Teacher>>> UpdateTeacher(Teacher teacher)
        {

            if (!ModelState.IsValid)//checks if the model passed into the action has passed model validation
            {
                return BadRequest(ModelState);
            }

            _Context.Entry(teacher).State = EntityState.Modified;//Entry() is a method provided by EF that allows you to access an object's tracking information within the context///EntityState.Modified is one of the possible states that EF tracks for objects. It indicates that the object represents a record in the database that has been modified

            try
            {
                await _Context.SaveChangesAsync();
            }
            catch (TimeoutException)
            {
                // Handle query timeout errors
                return StatusCode(StatusCodes.Status408RequestTimeout, "The database query timed out.");
            }
            catch (DbUpdateConcurrencyException)
            {
                var existingClass = await _Context.Teachers.FindAsync(teacher.Id);
                if (existingClass != null)
                {
                    return NotFound();
                }
                else
                {
                    return Conflict("Concurrency conflict occurred. Another user may have updated the Teacher.");
                }
            }
            catch (Exception ex)
            {
                // Handle all other exceptions (including database-related ones)
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }

            var updatedTeacher = await _Context.Teachers.FindAsync(teacher.Id);

            if (updatedTeacher == null)
            {
                return NotFound();
            }
            return Ok(updatedTeacher);
        }

        // DELETE: api/Teacher/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            var teacher = await _Context.Teachers.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }

            _Context.Teachers.Remove(teacher);

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
