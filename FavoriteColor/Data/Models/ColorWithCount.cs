namespace FavoriteColor.Data.Models;

/// <summary>
/// Represents a User object that contains an Id, First Name and Last Name.
/// </summary>
public class ColorWithCount
{
    public string Color { get; init; } = null!;
    public int Count { get; set; }
}
