using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using postgresTest.Model;

namespace postgresTest.Controllers;

[ApiController]
// may need to change the name of this
[Route("api/[controller]")]
public class StudyController : ControllerBase
{
  private readonly UserManager<User> _userManager;
  private readonly ApplicationDbContext _context;

  public StudyController(ApplicationDbContext context, UserManager<User> userManager)
  {
    _context = context;
    _userManager = userManager;
  }

  // Create study; requires data
  [HttpPost]
  public async Task<ActionResult<Study>> CreateStudy([FromForm] CreateStudyInput input)
  {
    // okay THIS WORKS
    // Console.WriteLine(input.Title);
    // Console.WriteLine(input.OriginalLink);

    // ClaimsPrincipal currentUser = this.User;
    // var currentUserId = currentUser.FindFirstValue(ClaimsTypes.NameIdentifier);

    // null
    // Console.WriteLine("Checking another method");
    // Console.WriteLine(" ");
    // var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
    // Console.WriteLine(user);
    // Console.WriteLine("checking type of user");

    // var firstClaimValue = HttpContext.User.Claims.FirstOrDefault()?.Value;
    // Console.WriteLine(firstClaimValue);

    Console.WriteLine("\n Checking users...? \n");
    Console.WriteLine("below");
    Console.WriteLine(_userManager.Users);
    Console.WriteLine("above");
    // Corresponds to Claims Principal
    Console.WriteLine(HttpContext.User);
    Console.WriteLine(HttpContext.User.Identity.Name);
    Console.WriteLine(" ");
    var currentUser = HttpContext.User.Identity.Name;
    // var user = _context.Users.Where(x => x.UserName == currentUser).FirstOrDefault();
    // var user = _context.Users.FirstOrDefault();

    var user = await _userManager.FindByNameAsync(currentUser);

    // var user = await _userManager.GetUserAsync(HttpContext.User);
    Console.WriteLine(" ");
    Console.WriteLine("below is user");
    Console.WriteLine(user);
    Console.WriteLine("is above null? above must be null");
    Console.WriteLine(" ");

    Study newStudy = new Study
    {
      Title = input.Title,
      OriginalLink = input.OriginalLink,
      ImageLink = "mockLink",
      ThumbnailLink = "mockThumbnailLink",
      UserId = user.Id,
      DateCreated = new DateTime(),
    };


    _context.Studies.Add(newStudy);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetStudy), new { id = newStudy.Id }, newStudy);
  }

  // Load studies, based on user id
  [HttpGet("all")]
  public async Task<ActionResult<IEnumerable<Study>>> GetAllStudies()
  {
    return await _context.Studies.ToListAsync();
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<Study>> GetStudy(long id)
  {
    var study = await _context.Studies.FindAsync(id);

    if (study == null)
    {
      return NotFound();
    }
    return study;
  }
}
