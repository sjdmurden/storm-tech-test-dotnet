using Storm.TechTask.Core.ProjectAggregate;

namespace Storm.TechTask.Api.Endpoints.Project
{
    public record ProjectDto(int Id, string Name, IEnumerable<ToDoItemDto> Items);
    public record ProjectDetailsDto(int Id, string Name, ProjectCategory Category, ProjectStatus Status);

    public record ToDoItemDto(int Id, string Title, string Description, bool IsDone)
    {
        public ToDoItemDto(ToDoItem item) : this(item.Id, item.Title, item.Description, item.IsDone) { }
    }

}
