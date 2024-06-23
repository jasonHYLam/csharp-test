using System.Security.Claims;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using postgresTest.Model;

namespace postgresTest.Controllers;

[ApiController]
[Route("[controller]")]
public class StudyController : ControllerBase
{
  private readonly Cloudinary _cloudinary;
  private readonly UserManager<User> _userManager;
  private readonly ApplicationDbContext _context;

  public StudyController(
    ApplicationDbContext context,
    UserManager<User> userManager,
    Cloudinary cloudinary
    )
  {
    _context = context;
    _userManager = userManager;
    _cloudinary = cloudinary;
  }

  [HttpPost]
  public async Task<ActionResult<StudyDTO>> CreateStudy([FromForm] CreateStudyInput input)
  {

    var uploadParams = new ImageUploadParams()
    {
      File = new FileDescription(input.ImageFile.FileName, input.ImageFile.OpenReadStream()),
      EagerTransforms = new List<Transformation>()
      {
       new EagerTransformation().AspectRatio("1.0").Width(300).Chain().FetchFormat("webp")
      }
    };
    var uploadResult = _cloudinary.Upload(uploadParams);

    var user = await _userManager.GetUserAsync(HttpContext.User);
    Console.WriteLine("checking user");
    Console.WriteLine(user.ToJson());

    Study newStudy = new Study
    {
      Title = input.Title,
      OriginalLink = input.OriginalLink,
      ImageLink = uploadResult.SecureUrl.ToString(),
      ThumbnailLink = uploadResult.Eager[0].SecureUrl.ToString(),
      UserId = user.Id,
      DateCreated = new DateTime(),
    };

    _context.Studies.Add(newStudy);
    await _context.SaveChangesAsync();

    var CreatedStudyDTO = new StudyDTO
    {
      Id = newStudy.Id,
      Title = newStudy.Title,
      OriginalLink = newStudy.OriginalLink,
      DateCreated = newStudy.DateCreated,
    };

    return CreatedAtAction(nameof(GetStudy), new { id = newStudy.Id }, CreatedStudyDTO);
  }

  [HttpGet("allStudies")]
  public async Task<ActionResult<IEnumerable<StudyPreviewDTO>>> GetAllStudies()
  {

    Console.WriteLine("checking HttpContext.user");
    // Console.WriteLine(HttpContext.User.ToJson());
    Console.WriteLine(HttpContext.User);

    var user = await _userManager.GetUserAsync(HttpContext.User);
    Console.WriteLine("checking user");
    Console.WriteLine(user.ToJson());

    IQueryable<Study> studiesQuery =
    from study in _context.Studies
    where study.UserId == user.Id
    select study;

    var allStudies = await studiesQuery.ToListAsync();

    Console.WriteLine("checking allStudies");
    Console.WriteLine(allStudies.ToJson());

    var allStudyDTOs = allStudies.Select(
      s => new StudyPreviewDTO
      {
        Id = s.Id,
        Title = s.Title,
        DateCreated = s.DateCreated,
        ThumbnailLink = s.ThumbnailLink
      }).ToList();

    return allStudyDTOs;
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<StudyDTO>> GetStudy(long id)
  {
    var study = await _context.Studies.FindAsync(id);

    if (study == null)
    {
      return NotFound();
    }
    else
    {

      var studyDTO = new StudyDTO
      {
        Id = study.Id,
        Title = study.Title,
        OriginalLink = study.OriginalLink,
        DateCreated = study.DateCreated
      };
      return studyDTO;
    }
  }
}
