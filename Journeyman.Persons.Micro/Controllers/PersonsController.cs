using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Journeyman.Persons.Micro.Context;
using Journeyman.Persons.Micro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Journeyman.Persons.Micro.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonsController : ControllerBase
    {
        private readonly PersonsDbContext _db;

        public PersonsController(PersonsDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> Get()
        {
            return await _db.Persons.ToListAsync();
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<Person>> Get(long userId)
        {
            var result =  await _db.Persons.FirstOrDefaultAsync(p => long.Equals(p.UserId, userId));
            if (result is null)
                return StatusCode(404);

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Person>> Post([FromBody] Person person)
        {
            if (person is null)
                return BadRequest();

            _db.Persons.Add(person);
            await _db.SaveChangesAsync();
            return Ok(person);
        }

        [HttpPut]
        public async Task<ActionResult<Person>> Put([FromBody] Person person)
        {
            if (person is null)
                return BadRequest();

            if(!_db.Persons.Any(p => p.Id == person.Id))
                return NotFound();

            _db.Update(person);
            await _db.SaveChangesAsync();
            return Ok(person);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Person>> Delete(int id)
        {
            var person = await _db.Persons.FirstOrDefaultAsync(p => p.Id == id);
            if (person is null)
                return NotFound();

            _db.Persons.Remove(person);
            await _db.SaveChangesAsync();
            return Ok(person);
        }
    }
}
