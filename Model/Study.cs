using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace postgresTest.Model
{
  public class Study
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string OriginalLink { get; set; }
    public DateTime DateCreated { get; set; }
    public string ImageLink { get; set; }
    public string ThumbnailLink { get; set; }


    public required string UserId { get; set; }
    public User? User { get; set; }

    public ICollection<Note> Notes { get; } = new List<Note>();
  }
}