using FavoriteColor.Data.Models;

namespace FavoriteColor.Features.MostPopularColorFunctions;

public class MostPopularColorFunctions
{
    public MostPopularColorFunctions() { }

    public ColorWithCount DetermineMostPopularColorWithUsers(List<ColorPreference> colorPreferences)
    {
        var colors = GetColorsWithCountOfVotes(colorPreferences);

        var preferredColor = colors.OrderByDescending(c => c.Count).FirstOrDefault()
            ?? throw new Exception("Unable To Determine Most Popular Color.");

        return preferredColor;
    }

    /// <summary>
    /// Determines the most popular colors with a list of Users who voted for that color. 
    /// </summary>
    /// <param name="users"></param>
    /// <param name="colorPreferences"></param>
    /// <exception cref="Exception"></exception>
    public List<User> DetermineUserWhoeVotedForMostPopularColor(List<User> users,
        List<ColorPreference> colorPreferences,
        string color)
    {
        var userIds = colorPreferences
            .Where(cp => cp.Color == color)
            .Select(cp => cp.UserId)
            .Distinct()
            .ToList();

        return users
            .Where(u => userIds.Contains(u.Id))
            .OrderBy(u => u.LastName)
            .ToList();
    }

    /// <summary>
    /// Populates a list of User objects from a data source with the provided file path.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public List<User> PopulateUsersFromFile(string filePath)
    {
        var users = new List<User>();

        foreach (var line in File.ReadLines(filePath))
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var trimmedLine = line.Trim();
            var information = trimmedLine.Split('\t');

            var id = int.Parse(information[0]);
            if (id != 0)
            {
                var names = information[1].Split(' ');
                if (names != null)
                {
                    var user = new User(id, names[0], names[1]);
                    users.Add(user);
                }
            }
        }

        return users;
    }

    /// <summary>
    /// Populates a list of ColorPreference objects from a data source with the provided path.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public List<ColorPreference> PopulateColorPreferencesFromFile(string filePath)
    {
        var colorPreferences = new List<ColorPreference>();

        foreach (var line in File.ReadLines(filePath))
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var trimmedLine = line.Trim();
            var information = trimmedLine.Split(' ');

            var userId = int.Parse(information[0]);
            if (userId != 0)
            {
                if (information[1] != null)
                {
                    var colorPreference = new ColorPreference(userId, information[1]);
                    colorPreferences.Add(colorPreference);
                }
            }
        }

        return colorPreferences;
    }

    /// <summary>
    /// Gets a list of ColorsWithCount that represent the amount of times a color has been voted for by users.
    /// </summary>
    /// <param name="colorPreferences"></param>
    /// <returns></returns>
    private static List<ColorWithCount> GetColorsWithCountOfVotes(List<ColorPreference> colorPreferences)
    {
        var colors = new List<ColorWithCount>();

        foreach (var color in colorPreferences)
        {
            var existingColor = colors.SingleOrDefault(c => c.Color == color.Color);

            if (existingColor != null)
                existingColor.Count++;
            else
            {
                var colorToAdd = new ColorWithCount()
                {
                    Color = color.Color,
                    Count = 1,
                };

                colors.Add(colorToAdd);
            }
        }

        return colors;
    }
}