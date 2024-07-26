namespace FavoriteColor.Data.Models;

/// <summary>
/// Represents a User object that contains an Id, First Name and Last Name.
/// </summary>
public class User
{
    public int Id { get; init; }
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;

    public User(int id, string firstName, string lastName)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
    }
}
