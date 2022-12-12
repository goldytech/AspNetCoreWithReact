namespace AuthBackEnd.Models;

    public record LoginModel (string Username, string Password, bool RememberMe);
    public record UserEntity(int Id, string Name, string Password, string FavoriteColor, string Role);