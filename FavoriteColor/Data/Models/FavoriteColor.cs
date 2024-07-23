namespace FavoriteColor.Data.Models;

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
