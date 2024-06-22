// actually not sure if i need this
namespace postgresTest.Model;

public class CreateStudyInput
{
  public string Title { get; set; }
  public string OriginalLink { get; set; }

  // trying to figure out file upload
  public IFormFile file { get; set; }

}