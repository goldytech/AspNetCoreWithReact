using AuthBackEnd.Models;

namespace AuthBackEnd.Data;

public interface IUserRepository
{
    UserEntity? GetByUsernameAndPassword(string username, string password);
}
public class UserRepository : IUserRepository
{
    private readonly List<UserEntity> _users = new()
    {
        new UserEntity(3522, "afzal", "P@ssw0rd", "blue", "Admin"),
        new UserEntity(3523, "john", "P@ssw0rd", "red", "User")
    };

    public UserEntity? GetByUsernameAndPassword(string username, string password)
    {
        var user = _users.SingleOrDefault(u => u.Name == username && u.Password == password);
        return user;
    }
}