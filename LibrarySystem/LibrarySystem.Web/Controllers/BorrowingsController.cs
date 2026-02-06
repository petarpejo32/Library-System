using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using LibrarySystem.Domain.Models;
using LibrarySystem.Service.Interface;
using LibrarySystem.Web.ViewModels;

namespace LibrarySystem.Web.Controllers
{
    public class BorrowingsController : Controller
    {
        private readonly IBorrowingService _borrowingService;
        private readonly IBookService _bookService;
        private readonly IMemberService _memberService;

        public BorrowingsController(
            IBorrowingService borrowingService,
            IBookService bookService,
            IMemberService memberService)
        {
            _borrowingService = borrowingService;
            _bookService = bookService;
            _memberService = memberService;
        }

        // GET: Borrowings
        public IActionResult Index()
        {
            var borrowings = _borrowingService.GetAllBorrowings();
            return View(borrowings);
        }

        // GET: Borrowings/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowing = _borrowingService.GetBorrowingById(id.Value);
            if (borrowing == null)
            {
                return NotFound();
            }

            return View(borrowing);
        }

        // GET: Borrowings/Create
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_bookService.GetAvailableBooks(), "Id", "Title");
            ViewData["MemberId"] = new SelectList(_memberService.GetAllMembers(), "Id", "Name");
            return View();
        }

        // POST: Borrowings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("BorrowDate,DueDate,BookId,MemberId")] Borrowing borrowing)
        {
            if (ModelState.IsValid)
            {
                borrowing.Id = Guid.NewGuid();
                borrowing.IsReturned = false;
                borrowing.Fine = 0;
                _borrowingService.CreateBorrowing(borrowing);
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_bookService.GetAvailableBooks(), "Id", "Title", borrowing.BookId);
            ViewData["MemberId"] = new SelectList(_memberService.GetAllMembers(), "Id", "Name", borrowing.MemberId);
            return View(borrowing);
        }

        // GET: Borrowings/BorrowBook
        public IActionResult BorrowBook()
        {
            ViewData["BookId"] = new SelectList(_bookService.GetAvailableBooks(), "Id", "Title");
            ViewData["MemberId"] = new SelectList(_memberService.GetAllMembers(), "Id", "Name");
            return View();
        }

        // POST: Borrowings/BorrowBook
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BorrowBook(Guid bookId, Guid memberId)
        {
            try
            {
                var borrowing = _borrowingService.BorrowBook(bookId, memberId);
                TempData["SuccessMessage"] = "Book borrowed successfully!";
                return RedirectToAction(nameof(Details), new { id = borrowing.Id });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                ViewData["BookId"] = new SelectList(_bookService.GetAvailableBooks(), "Id", "Title", bookId);
                ViewData["MemberId"] = new SelectList(_memberService.GetAllMembers(), "Id", "Name", memberId);
                return View();
            }
        }

        // GET: Borrowings/ReturnBook/5
        public IActionResult ReturnBook(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowing = _borrowingService.GetBorrowingById(id.Value);

            if (borrowing == null)
            {
                return NotFound();
            }

            if (borrowing.IsReturned)
            {
                TempData["ErrorMessage"] = "This book has already been returned!";
                return RedirectToAction(nameof(Details), new { id = id });
            }

            var viewModel = new ReturnBookViewModel
            {
                Borrowing = borrowing,
                DaysUntilDue = (borrowing.DueDate - DateTime.Now).Days,
                IsOverdue = DateTime.Now > borrowing.DueDate,
                DaysOverdue = DateTime.Now > borrowing.DueDate ? (DateTime.Now - borrowing.DueDate).Days : 0,
                EstimatedFine = DateTime.Now > borrowing.DueDate ? (DateTime.Now - borrowing.DueDate).Days * 1.0m : 0m,
                BookTitle = borrowing.Book?.Title ?? "Unknown",
                AuthorName = borrowing.Book?.Author?.Name ?? "Unknown",
                MemberName = borrowing.Member?.Name ?? "Unknown"
            };

            return View(viewModel);
        }

        // POST: Borrowings/ReturnBook/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ReturnBookConfirmed(Guid id)
        {
            try
            {
                var borrowing = _borrowingService.ReturnBook(id);

                if (borrowing.Fine > 0)
                {
                    TempData["WarningMessage"] = $"Book returned with a fine of ${borrowing.Fine:F2}";
                }
                else
                {
                    TempData["SuccessMessage"] = "Book returned successfully!";
                }

                return RedirectToAction(nameof(Details), new { id = borrowing.Id });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(ReturnBook), new { id = id });
            }
        }

        // GET: Borrowings/ActiveBorrowings
        public IActionResult ActiveBorrowings()
        {
            var borrowings = _borrowingService.GetActiveBorrowings();
            return View("Index", borrowings);
        }

        // GET: Borrowings/OverdueBorrowings
        public IActionResult OverdueBorrowings()
        {
            var borrowings = _borrowingService.GetOverdueBorrowings();
            return View("Index", borrowings);
        }

        // GET: Borrowings/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowing = _borrowingService.GetBorrowingById(id.Value);
            if (borrowing == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_bookService.GetAllBooks(), "Id", "Title", borrowing.BookId);
            ViewData["MemberId"] = new SelectList(_memberService.GetAllMembers(), "Id", "Name", borrowing.MemberId);
            return View(borrowing);
        }

        // POST: Borrowings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,BorrowDate,DueDate,ReturnDate,IsReturned,Fine,BookId,MemberId")] Borrowing borrowing)
        {
            if (id != borrowing.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _borrowingService.UpdateBorrowing(borrowing);
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_bookService.GetAllBooks(), "Id", "Title", borrowing.BookId);
            ViewData["MemberId"] = new SelectList(_memberService.GetAllMembers(), "Id", "Name", borrowing.MemberId);
            return View(borrowing);
        }

        // GET: Borrowings/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowing = _borrowingService.GetBorrowingById(id.Value);
            if (borrowing == null)
            {
                return NotFound();
            }

            return View(borrowing);
        }

        // POST: Borrowings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _borrowingService.DeleteBorrowing(id);
            return RedirectToAction(nameof(Index));
        }
    }
}