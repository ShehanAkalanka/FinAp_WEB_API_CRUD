using FinAp_WEB_API_CRUD.Data;
using FinAp_WEB_API_CRUD.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinAp_WEB_API_CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassroomController : ControllerBase
    {
        private readonly FinApDbContext _Context;

        public ClassroomController(FinApDbContext DbContext)
        {
            _Context = DbContext;
        }

        // GET: api/classroom
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Classroom>>> GetClassrooms()
        {
            try
            {
                var classrooms = await _Context.Classrooms.ToListAsync();
                return Ok(classrooms); 
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

        // GET: api/classroom/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Classroom>> GetClassroom(int id)
        {
            try
            {
                var clasroom = await _Context.Classrooms.FindAsync(id);
                if (clasroom == null)
                {
                    return NotFound();
                }

                return Ok(clasroom);
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

        // POST: api/classroom
        [HttpPost]
        public async Task<ActionResult<Classroom>> CreateClassroom(Classroom classroom)
        {
            if (!ModelState.IsValid) //checks if the model passed into the action has passed model validation
            {
                return BadRequest(ModelState);
            }

            try
            {
                _Context.Classrooms.Add(classroom);
                await _Context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetClassroom), new { id = classroom.Id }, classroom);
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

        // PUT: api/classroom/
        [HttpPut]
        public async Task<ActionResult<IEnumerable<Classroom>>> UpdateClassroom(Classroom classroom)
        {

            if (!ModelState.IsValid)//checks if the model passed into the action has passed model validation
            {
                return BadRequest(ModelState);
            }

            _Context.Entry(classroom).State = EntityState.Modified;//indicates that the object represents a record in the database that has been modified

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
                var existingClass = await _Context.Classrooms.FindAsync(classroom.Id);
                if (existingClass != null)
                {
                    return NotFound();
                }
                else
                {
                    return Conflict("Concurrency conflict occurred. Another user may have updated the Classroom.");
                }
            }
            catch (Exception ex)
            {
                // Handle all other exceptions (including database-related ones)
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }

            var updatedClassroom = await _Context.Classrooms.FindAsync(classroom.Id);

            if (updatedClassroom == null)
            {
                return NotFound();
            }
            return Ok(updatedClassroom);
        }

        // DELETE: api/classroom/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClassroom(int id)
        {
            var classroom = await _Context.Classrooms.FindAsync(id);
            if (classroom == null)
            {
                return NotFound();
            }

            _Context.Classrooms.Remove(classroom);

            try
            {
                await _Context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlException && sqlException.Number == 547)
                {
                    // Handle foreign key constraint violation
                    return BadRequest("Cannot delete the classroom because it has associated students.");
                }
                else
                {
                    // Handle other database-related exceptions
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
                }
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
