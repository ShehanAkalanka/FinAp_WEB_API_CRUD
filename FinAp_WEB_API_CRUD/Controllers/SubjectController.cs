using FinAp_WEB_API_CRUD.Data;
using FinAp_WEB_API_CRUD.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace FinAp_WEB_API_CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly FinApDbContext _Context;

        public SubjectController(FinApDbContext DbContext)
        {
            _Context = DbContext;
        }

        // GET: api/Subject
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subject>>> GetSubjects()
        {
            try
            {
                var subject = await _Context.Subjects.ToListAsync();
                return Ok(subject); 
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

        // GET: api/Subject/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Subject>> GetSubject(int id)
        {
            try
            {
                var subject = await _Context.Subjects.FindAsync(id);
                if (subject == null)
                {
                    return NotFound();
                }

                return Ok(subject);
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

        // POST: api/Subject
        [HttpPost]
        public async Task<ActionResult<Subject>> CreateSubject(Subject subject)
        {
            if (!ModelState.IsValid)//checks if the model passed into the action has passed model validation
            {
                return BadRequest(ModelState);
            }

            try
            {
                _Context.Subjects.Add(subject);
                await _Context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetSubject), new { id = subject.Id }, subject);
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

        // PUT: api/Subject/
        [HttpPut]
        public async Task<ActionResult<IEnumerable<Subject>>> UpdateSubject(Subject subject)
        {

            if (!ModelState.IsValid)//checks if the model passed into the action has passed model validation
            {
                return BadRequest(ModelState);
            }

            _Context.Entry(subject).State = EntityState.Modified;//indicates that the object represents a record in the database that has been modified

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
                var existingSubject = await _Context.Subjects.FindAsync(subject.Id);
                if (existingSubject != null)
                {
                    return NotFound();
                }
                else
                {
                    return Conflict("Concurrency conflict occurred. Another user may have updated the Subject.");
                }
            }
            catch (Exception ex)
            {
                // Handle all other exceptions (including database-related ones)
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }

            var updatedSubject = await _Context.Subjects.FindAsync(subject.Id);

            if (updatedSubject == null)
            {
                return NotFound();
            }
            return Ok(updatedSubject);
        }

        // DELETE: api/Subject/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            var subject = await _Context.Subjects.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }

            _Context.Subjects.Remove(subject);

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
