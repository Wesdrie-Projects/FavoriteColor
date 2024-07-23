using FavoriteColor.Data.Models;

internal class Program
{
    private static ColorWithCount _mostPopularColor { get; set; } = null!;
    private static List<User> _usersWhoVotedForPopularColor { get; set; } = null!;

    private static void Main(string[] args)
    {
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        var users = PopulateUsersFromFile(Path.Combine(
            baseDirectory, "Data", "Source", "users.txt"));
        var colorPreferences = PopulateColorPreferencesFromFile(Path.Combine(
            baseDirectory,  "Data", "Source", "favourites.txt"));

        DetermineMostPopularColorWithUsers(users, colorPreferences);

        Console.WriteLine($"Most Popular Color: {_mostPopularColor.Color} With {_mostPopularColor.Count} Votes");
        Console.WriteLine("Users Who Voted For This Color:");
        foreach (var user in _usersWhoVotedForPopularColor)
        {
            Console.WriteLine($"{user.LastName}, {user.FirstName}");
        }
    }

    /// <summary>
    /// Determines the most popular colors with a list of Users who voted for that color. 
    /// </summary>
    /// <param name="users"></param>
    /// <param name="colorPreferences"></param>
    /// <exception cref="Exception"></exception>
    private static void DetermineMostPopularColorWithUsers(List<User> users,
        List<ColorPreference> colorPreferences)
    {
        var colors = GetColorsWithCountOfVotes(colorPreferences);

        var preferredColor = colors.OrderByDescending(c => c.Count).FirstOrDefault()
            ?? throw new Exception("Unable To Determine Most Popular Color.");
        _mostPopularColor = preferredColor;

        var userIds = colorPreferences
            .Where(cp => cp.Color == _mostPopularColor.Color)
            .Select(cp => cp.UserId)
            .Distinct()
            .ToList();

        _usersWhoVotedForPopularColor = users
            .Where(u => userIds.Contains(u.Id))
            .OrderBy(u => u.LastName)
            .ToList();
    }

    /// <summary>
    /// Populates a list of User objects from a data source with the provided file path.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    private static List<User> PopulateUsersFromFile(string filePath)
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
    private static List<ColorPreference> PopulateColorPreferencesFromFile(string filePath)
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

    private record ColorWithCount()
    {
        public required string Color { get; init; }
        public required int Count { get; set; }
    }
}