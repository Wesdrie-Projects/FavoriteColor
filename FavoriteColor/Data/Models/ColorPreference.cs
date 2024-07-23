namespace FavoriteColor.Data.Models;

/// <summary>
/// Represents a ColorPreference object that contains an related UserId and Color.
/// </summary>
internal class ColorPreference
{
    public int UserId { get; init; }
    public string Color { get; init; } = null!;

    public ColorPreference(int userId, string color)
    {
        UserId = userId;
        Color = color;
    }
}
