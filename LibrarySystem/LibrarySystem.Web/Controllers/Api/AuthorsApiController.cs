using LibrarySystem.Domain.Models;
using LibrarySystem.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Web.Controllers.Api
{
    [Route("api/authors")]
    [ApiController]
    public class AuthorsApiController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorsApiController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        // GET: api/authors
        [HttpGet]
        public ActionResult<IEnumerable<Author>> GetAllAuthors()
        {
            var authors = _authorService.GetAllAuthors();
            return Ok(authors);
        }

        // GET: api/authors/{id}
        [HttpGet("{id}")]
        public ActionResult<Author> GetAuthor(Guid id)
        {
            var author = _authorService.GetAuthorById(id);

            if (author == null)
            {
                return NotFound(new { message = $"Author with ID {id} not found." });
            }

            return Ok(author);
        }

        // POST: api/authors
        [HttpPost]
        public ActionResult<Author> CreateAuthor([FromBody] Author author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _authorService.CreateAuthor(author);
            return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
        }

        // PUT: api/authors/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateAuthor(Guid id, [FromBody] Author author)
        {
            if (id != author.Id)
            {
                return BadRequest(new { message = "ID mismatch." });
            }

            var existingAuthor = _authorService.GetAuthorById(id);
            if (existingAuthor == null)
            {
                return NotFound(new { message = $"Author with ID {id} not found." });
            }

            _authorService.UpdateAuthor(author);
            return NoContent();
        }

        // DELETE: api/authors/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteAuthor(Guid id)
        {
            var author = _authorService.GetAuthorById(id);
            if (author == null)
            {
                return NotFound(new { message = $"Author with ID {id} not found." });
            }

            _authorService.DeleteAuthor(id);
            return NoContent();
        }
    }
}