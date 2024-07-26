using FavoriteColor.Features.MostPopularColorFunctions;

namespace Tests;

public class MostPopularColorFunctionsTests
{
    private readonly MostPopularColorFunctions _func;

    public MostPopularColorFunctionsTests()
    {
        _func = new MostPopularColorFunctions();
    }

    [Fact]
    public void PopulatesUsersFromFile_ReturnsListOfUsers()
    {
        // Arrange
        var filePath = "Tests\\Files\\test_users.txt";

        // Act
        var users = _func.PopulateUsersFromFile(filePath);

        // Assert
        Assert.NotEmpty(users);
        Assert.Equal(1, users[0].Id);
        Assert.Equal("User", users[0].FirstName);
        Assert.Equal("One", users[0].LastName);
    }

    [Fact]
    public void PopulateColorPreferencesFromFile_ReturnsListOfColorPrefernces()
    {
        // Arrange
        var filePath = "Tests\\Files\\test_colorPreferences.txt";

        // Act
        var colors = _func.PopulateColorPreferencesFromFile(filePath);

        // Assert
        Assert.NotEmpty(colors);
        Assert.Equal(1, colors[0].UserId);
        Assert.Equal("Red", colors[0].Color);
    }

    [Fact]
    public void DetermineMostPopularColorWithUsers_ReturnsColorWithCount()
    {
        // Arrange
        var filePath = "Tests\\Files\\test_colorPreferences.txt";
        var colors = _func.PopulateColorPreferencesFromFile(filePath);

        // Act
        var color = _func.DetermineMostPopularColorWithUsers(colors);

        // Assert
        Assert.NotNull(color);
        Assert.Equal(1, color.Count);
        Assert.Equal("Red", color.Color);
    }

    [Fact]
    public void DetermineUsersWhoVotedForColor_ReturnsListOfUsers()
    {
        // Arrange
        var colorsFilePath = "Tests\\Files\\test_colorPreferences.txt";
        var testColors = _func.PopulateColorPreferencesFromFile(colorsFilePath);

        // Arrange
        var usersFilePath = "Tests\\Files\\test_users.txt";
        var testUsers = _func.PopulateUsersFromFile(usersFilePath);

        var color = _func.DetermineMostPopularColorWithUsers(testColors);

        // Act
        var users = _func.DetermineUserWhoeVotedForMostPopularColor(testUsers, testColors, color.Color);

        // Assert
        Assert.NotEmpty(users);
        Assert.Equal(1, users[0].Id);
        Assert.Equal("User", users[0].FirstName);
        Assert.Equal("One", users[0].LastName);
    }
}