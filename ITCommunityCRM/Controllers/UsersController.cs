using ITCommunityCRM.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITCommunityCRM.Controllers
{
    public class UsersController : Controller
    {
        private readonly ITCommunityCRMDbContext _context;


        public UsersController(ITCommunityCRMDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var users = _context.EventUsers.Include(x => x.User).ToList();
            return View(users);
        }
    }
}
