using FavoriteColor.Data.Models;

internal class Program
{
    private static ColorWithCount _color { get; set; } = null!;
    private static List<User> _users { get; set; } = null!;

    private static void Main(string[] args)
    {
        var users = new List<User>();
        var colorPreferences = new List<ColorPreference>();

        // Update To Be More Relative Path
        PopulateUsersFromFile(users, Path.Combine(
                "C:", "Users", "Hendr", "Source", "Repos", "Wesdrie-Projects",
                "FavoriteColor", "FavoriteColor", "Data", "Source", "users.txt"));

        PopulateColorPreferencesFromFile(colorPreferences, Path.Combine(
                "C:", "Users", "Hendr", "Source", "Repos", "Wesdrie-Projects",
                "FavoriteColor", "FavoriteColor", "Data", "Source", "favourites.txt"));

        DetermineMostPopularColor(users, colorPreferences);

        Console.WriteLine($"Most Popular Color: {_color.Color} With {_color.Count} Votes");
        Console.WriteLine("Users Who Voted For This Color:");
        foreach (var user in _users)
        {
            Console.WriteLine($"{user.LastName}, {user.FirstName}");
        }
    }

    private static void DetermineMostPopularColor(List<User> users,
        List<ColorPreference> colorPreferences)
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

        var preferredColor = colors.OrderByDescending(c => c.Count).FirstOrDefault()
            ?? throw new Exception("Unable To Determine Most Popular Color.");
        _color = preferredColor;

        var popularColorPreferences = colorPreferences
            .Where(cp => cp.Color == _color.Color)
            .Select(cp => cp.UserId)
            .Distinct()
            .ToList();

        _users = users
            .Where(u => popularColorPreferences.Contains(u.Id))
            .OrderBy(u => u.LastName)
            .ToList();
    }

    private static List<User> PopulateUsersFromFile(List<User> users,
        string filePath)
    {
        foreach (var line in File.ReadLines(filePath))
        {
            // Ensure Line Is Not Empty.
            if (string.IsNullOrWhiteSpace(line))
                continue;

            // Remove Non-Valid Spaces.
            var trimmedLine = line.Trim();

            // Split Into UserId And UserName.
            // Build Case For Double Names (?).
            // Add Error Handling.
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

    private static List<ColorPreference> PopulateColorPreferencesFromFile(List<ColorPreference> colorPreferences,
        string filePath)
    {
        foreach (var line in File.ReadLines(filePath))
        {
            // Ensure Line Is Not Empty.
            if (string.IsNullOrWhiteSpace(line))
                continue;

            // Remove Non-Valid Spaces.
            var trimmedLine = line.Trim();

            // Split Into UserId And UserName.
            // Build Case For Double Names (?).
            // Add Error Handling.
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

    private record ColorWithCount()
    {
        public required string Color { get; init; }
        public required int Count { get; set; }
    }
}