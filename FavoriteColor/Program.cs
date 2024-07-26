using FavoriteColor.Data.Models;
using FavoriteColor.Features.MostPopularColorFunctions;

internal class Program
{
    private static ColorWithCount _mostPopularColor { get; set; } = null!;
    private static List<User> _usersWhoVotedForPopularColor { get; set; } = null!;

    private static void Main(string[] args)
    {
        var func = new MostPopularColorFunctions();
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        var users = func.PopulateUsersFromFile(Path.Combine(
            baseDirectory, "Data", "Source", "users.txt"));
        var colorPreferences = func.PopulateColorPreferencesFromFile(Path.Combine(
            baseDirectory,  "Data", "Source", "favourites.txt"));

        _mostPopularColor = func.DetermineMostPopularColorWithUsers(colorPreferences);
        _usersWhoVotedForPopularColor = func.DetermineUserWhoeVotedForMostPopularColor(users, colorPreferences, _mostPopularColor.Color);

        Console.WriteLine($"Most Popular Color: {_mostPopularColor.Color} With {_mostPopularColor.Count} Votes");
        Console.WriteLine("Users Who Voted For This Color:");
        foreach (var user in _usersWhoVotedForPopularColor)
        {
            Console.WriteLine($"{user.LastName}, {user.FirstName}");
        }
    }
}