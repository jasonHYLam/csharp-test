namespace postgresTest.Model
{
  public class Note
  {
    public int Id { get; set; }
    public string Text { get; set; }

    // Colors are stored as hex
    public string OriginalHexColor { get; set; }
    public string GuessedHexColor { get; set; }

    public int StudyId { get; set; }
    // doubt i need the study, just the studyid
    // public Study Study { get; set; } = null!;
  }
}
