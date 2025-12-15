namespace LearningSystem.Application.Commands.Courses;

public class UpdateCourseCommand
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string LongDescription { get; set; }
    public int CategoryId { get; set; }
    public string Difficulty { get; set; }
    public string Thumbnail { get; set; }
    public bool IsPublished { get; set; }

    public UpdateCourseCommand(int id, string title, string shortDescription, string longDescription, int categoryId, string difficulty, string thumbnail, bool isPublished)
    {
        Id = id;
        Title = title;
        ShortDescription = shortDescription;
        LongDescription = longDescription;
        CategoryId = categoryId;
        Difficulty = difficulty;
        Thumbnail = thumbnail;
        IsPublished = isPublished;
    }
}
