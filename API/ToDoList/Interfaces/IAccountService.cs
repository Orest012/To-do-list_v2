using ToDoList.Data;
using ToDoList.DTO;

namespace ToDoList.Interfaces
{
    public interface IAccountService
    {
        string Login(UserLoginDto request);
        string Register(UserRegisterDto request);
    }
}
