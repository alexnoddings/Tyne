namespace Tyne.Aerospace.Client.Infrastructure.Users;

public class UserService : ITyneUserService
{
    // Defaults to the super user from DataSeeder
    public Guid UserId { get; set; } = Guid.Parse("7bd43de0-30e4-452b-85ec-54b3a20c520e");

    public Guid? TryGetUserId() => UserId;
}
