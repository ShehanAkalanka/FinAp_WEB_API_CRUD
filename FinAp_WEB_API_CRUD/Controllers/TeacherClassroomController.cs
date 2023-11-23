using FinAp_WEB_API_CRUD.Data;
using FinAp_WEB_API_CRUD.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using FinAp_WEB_API_CRUD.Data.DTOs;
using FinAp_WEB_API_CRUD.Migrations;

namespace FinAp_WEB_API_CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherClassroomController : ControllerBase
    {
        private readonly FinApDbContext _Context;

        public TeacherClassroomController(FinApDbContext DbContext)
        {
            _Context = DbContext;
        }

        // GET: api/TeacherClassroom
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeacherClassroom>>> GetTeacherClassroomRelations()
        {
            try
            {
                var teacherClassroom = await _Context.TeacherClassrooms.ToListAsync();
                return Ok(teacherClassroom); 
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

        // GET: api/TeacherClassroom/{teacherId}/{classroomId}
        [HttpGet("{teacherId}/{classroomId}")]
        public async Task<ActionResult<TeacherClassroom>> GetTeacherClassroomRelation(int teacherId,int classroomId)
        {
            try
            {
                var teacherClassroom = await _Context.TeacherClassrooms.FindAsync(teacherId, classroomId);
                if (teacherClassroom == null)
                {
                    return NotFound();
                }

                return Ok(teacherClassroom);
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

        // POST: api/TeacherClassroom
        [HttpPost]
        public async Task<ActionResult<TeacherClassroom>> CreateTeacherClassroomRelation(TeacherClassroom teacherClassroom)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the relation already exists in the database
            var existingRelation = await _Context.TeacherClassrooms
                .FirstOrDefaultAsync(tr => tr.TeacherId == teacherClassroom.TeacherId && tr.ClassroomId == teacherClassroom.ClassroomId);

            if (existingRelation != null)
            {
                // Return a conflict response indicating that the relation already exists
                return Conflict("The TeacherClassroom relation already exists.");
            }

            try
            {
                _Context.TeacherClassrooms.Add(teacherClassroom);
                await _Context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetTeacherClassroomRelation), new { teacherId = teacherClassroom.TeacherId,classroomId = teacherClassroom.ClassroomId}, teacherClassroom);
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

        // PUT: api/TeacherClassroom
        [HttpPut]
        public async Task<ActionResult<IEnumerable<TeacherClassroom>>> UpdateTeacherClassroomRelation(TeacherClassroomUpdateDTO teacherClassroomDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingRelation = await _Context.TeacherClassrooms
               .FirstOrDefaultAsync(tr => tr.TeacherId == teacherClassroomDto.NewTeacherId && tr.ClassroomId == teacherClassroomDto.NewClassroomId);

            if (existingRelation != null)
            {
                // Return a conflict response indicating that the relation already exists
                return Conflict("The TeacherClassroom relation already exists.");
            }

            var oldRelation = await _Context.TeacherClassrooms
               .FirstOrDefaultAsync(tr => tr.TeacherId == teacherClassroomDto.TeacherId && tr.ClassroomId == teacherClassroomDto.ClassroomId);

            if (oldRelation == null)
            {
                return BadRequest("No User Found");
            }

            // Delete the old relation
            _Context.TeacherClassrooms.Remove(oldRelation);

            // Create a new relation with the updated values
            var newRelation = new TeacherClassroom
            {
                TeacherId = teacherClassroomDto.NewTeacherId,
                ClassroomId = teacherClassroomDto.NewClassroomId
            };

            // Add the new relation to the context
            _Context.TeacherClassrooms.Add(newRelation);

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
                // Handle concurrency conflicts
                return Conflict("Concurrency conflict occurred. Another user may have updated the TeacherClassroom relation.");
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }

            return CreatedAtAction(nameof(GetTeacherClassroomRelation), new { teacherId = teacherClassroomDto.TeacherId, classroomId = teacherClassroomDto.ClassroomId }, teacherClassroomDto);
        }

        // DELETE: api/TeacherClassroom/{teacherId}/{classroomId}
        [HttpDelete("{teacherId}/{classroomId}")]
        public async Task<IActionResult> DeleteTeacherClassroomRelation(int teacherId, int classroomId)
        {
            var teacherClassroom = await _Context.TeacherClassrooms.FindAsync(teacherId, classroomId);
            if (teacherClassroom == null)
            {
                return NotFound();
            }

            _Context.TeacherClassrooms.Remove(teacherClassroom);

            try
            {
                await _Context.SaveChangesAsync();
            }
            catch (TimeoutException)
            {
                // Handle query timeout errors
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
