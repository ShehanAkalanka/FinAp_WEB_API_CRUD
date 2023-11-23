using FinAp_WEB_API_CRUD.Data;
using FinAp_WEB_API_CRUD.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using FinAp_WEB_API_CRUD.Data.DTOs;

namespace FinAp_WEB_API_CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherSubjectController : ControllerBase
    {
        private readonly FinApDbContext _Context;

        public TeacherSubjectController(FinApDbContext DbContext)
        {
            _Context = DbContext;
        }

        // GET: api/TeacherSubject
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeacherSubject>>> GetTeacherSubjects()
        {
            try
            {
                var teacherSubject = await _Context.TeacherSubjects.ToListAsync();
                return Ok(teacherSubject);
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

        // GET: api/TeacherSubject/{teacherId}/{subjectId}
        [HttpGet("{teacherId}/{subjectId}")]
        public async Task<ActionResult<TeacherSubject>> GetTeacherSubjectRelation(int teacherId,int subjectId)
        {
            try
            {
                var teacherSubject = await _Context.TeacherSubjects.FindAsync(teacherId, subjectId);
                if (teacherSubject == null)
                {
                    return NotFound();
                }

                return Ok(teacherSubject);
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

        // POST: api/TeacherSubject
        [HttpPost]
        public async Task<ActionResult<TeacherSubject>> CreateTeacherSubjectRelation(TeacherSubject teacherSubject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the relation already exists in the database
            var existingRelation = await _Context.TeacherSubjects
                .FirstOrDefaultAsync(ts => ts.TeacherId == teacherSubject.TeacherId && ts.SubjectId == teacherSubject.SubjectId);

            if (existingRelation != null)
            {
                // Return a conflict response indicating that the relation already exists
                return Conflict("The TeacherClassroom relation already exists.");
            }

            try
            {
                _Context.TeacherSubjects.Add(teacherSubject);
                await _Context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetTeacherSubjectRelation), new { teacherId = teacherSubject.TeacherId, subjectId = teacherSubject.SubjectId }, teacherSubject);
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

        // PUT: api/TeacherSubject/
        [HttpPut]
        public async Task<ActionResult<IEnumerable<TeacherSubject>>> UpdateTeacherSubject(TeacherSubjectUpdateDTO teacherSubjectDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingRelation = await _Context.TeacherSubjects
                .FirstOrDefaultAsync(ts => ts.TeacherId == teacherSubjectDTO.NewTeacherId && ts.SubjectId == teacherSubjectDTO.NewSubjectId);

            if (existingRelation != null)
            {
                // Return a conflict response indicating that the relation already exists
                return Conflict("The TeacherSubject relation already exists.");
            }

            var oldRelation = await _Context.TeacherSubjects
               .FirstOrDefaultAsync(tr => tr.TeacherId == teacherSubjectDTO.TeacherId && tr.SubjectId == teacherSubjectDTO.SubjectId);

            if (oldRelation == null)
            {
                return BadRequest("The Relation Not Found");
            }

            // Delete the old relation
            _Context.TeacherSubjects.Remove(oldRelation);

            // Create a new relation with the updated values
            var newRelation = new TeacherSubject
            {
                TeacherId = teacherSubjectDTO.NewTeacherId,
                SubjectId = teacherSubjectDTO.NewSubjectId
            };

            // Add the new relation to the context
            _Context.TeacherSubjects.Add(newRelation);

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
                var existingTeacherSubject = await _Context.TeacherSubjects
                    .FirstOrDefaultAsync(ts => ts.TeacherId == teacherSubjectDTO.NewTeacherId && ts.SubjectId == teacherSubjectDTO.NewSubjectId);

                if (existingTeacherSubject != null)
                {
                    return NotFound();
                }
                else
                {
                    return Conflict("Concurrency conflict occurred. Another user may have updated the TeacherSubject relation.");
                }
            }
            catch (Exception ex)
            {
                // Handle all other exceptions (including database-related ones)
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }

            return CreatedAtAction(nameof(GetTeacherSubjectRelation), new { teacherId = teacherSubjectDTO.TeacherId, subjectId = teacherSubjectDTO.SubjectId }, teacherSubjectDTO);
        }


        // DELETE: api/TeacherSubject/{teacherId}/{subjectId
        [HttpDelete("{teacherId}/{subjectId}")]
        public async Task<IActionResult> DeleteTeacherSubject(int teacherId,int subjectId)
        {
            var teacherSubject = await _Context.TeacherSubjects.FindAsync(teacherId, subjectId);
            if (teacherSubject == null)
            {
                return NotFound();
            }

            _Context.TeacherSubjects.Remove(teacherSubject);

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
