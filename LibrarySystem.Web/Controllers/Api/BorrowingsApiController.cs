using LibrarySystem.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Web.Controllers.Api
{
    [Route("api/borrowings")]
    [ApiController]
    public class BorrowingsApiController : ControllerBase
    {
        private readonly IBorrowingService _borrowingService;

        public BorrowingsApiController(IBorrowingService borrowingService)
        {
            _borrowingService = borrowingService;
        }

        // GET: api/borrowings
        [HttpGet]
        public IActionResult GetAllBorrowings()
        {
            var borrowings = _borrowingService.GetAllBorrowings();
            return Ok(borrowings);
        }

        // GET: api/borrowings/{id}
        [HttpGet("{id}")]
        public IActionResult GetBorrowing(Guid id)
        {
            var borrowing = _borrowingService.GetBorrowingById(id);

            if (borrowing == null)
            {
                return NotFound(new { message = $"Borrowing with ID {id} not found." });
            }

            return Ok(borrowing);
        }

        // GET: api/borrowings/active
        [HttpGet("active")]
        public IActionResult GetActiveBorrowings()
        {
            var borrowings = _borrowingService.GetActiveBorrowings();
            return Ok(borrowings);
        }

        // GET: api/borrowings/overdue
        [HttpGet("overdue")]
        public IActionResult GetOverdueBorrowings()
        {
            var borrowings = _borrowingService.GetOverdueBorrowings();
            return Ok(borrowings);
        }

        // GET: api/borrowings/member/{memberId}
        [HttpGet("member/{memberId}")]
        public IActionResult GetBorrowingsByMember(Guid memberId)
        {
            var borrowings = _borrowingService.GetBorrowingsByMember(memberId);
            return Ok(borrowings);
        }

        // POST: api/borrowings/borrow
        [HttpPost("borrow")]
        public IActionResult BorrowBook([FromBody] BorrowBookRequest request)
        {
            try
            {
                var borrowing = _borrowingService.BorrowBook(request.BookId, request.MemberId);
                return CreatedAtAction(nameof(GetBorrowing), new { id = borrowing.Id }, borrowing);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/borrowings/return/{id}
        [HttpPost("return/{id}")]
        public IActionResult ReturnBook(Guid id)
        {
            try
            {
                var borrowing = _borrowingService.ReturnBook(id);
                return Ok(borrowing);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/borrowings/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteBorrowing(Guid id)
        {
            var borrowing = _borrowingService.GetBorrowingById(id);
            if (borrowing == null)
            {
                return NotFound(new { message = $"Borrowing with ID {id} not found." });
            }

            _borrowingService.DeleteBorrowing(id);
            return NoContent();
        }
    }

    public class BorrowBookRequest
    {
        public Guid BookId { get; set; }
        public Guid MemberId { get; set; }
    }
}