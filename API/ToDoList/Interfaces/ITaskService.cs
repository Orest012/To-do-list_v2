using ToDoList.DTO;

namespace ToDoList.Interfaces
{
    public interface ITaskService
    {
        List<AssignmentReadDTO> GetAllTasks(string id);
        Task<AssignmentReadDTO> GetTaskById(int taskId);
        Task<AssignmentReadDTO> CreateTask(AssignmentCreateDTO newTask, int userId);
        Task<AssignmentReadDTO> UpdateTask(AssignmentUpdateDTO newTask);
        Task<List<AssignmentSummaryDTO>> GetShortInformation(string id);
        Task DeleteTask(int taskId);
    }

}
