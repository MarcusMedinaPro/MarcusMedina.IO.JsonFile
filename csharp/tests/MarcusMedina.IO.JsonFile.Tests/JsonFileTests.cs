// -----------------------------------------------------------------------------------------------
//  JsonFileTests.cs by Marcus Medina, Copyright (C) 2021-2025, http://MarcusMedina.Pro.
//  Published under MIT License
//  https://opensource.org/licenses/MIT
// -----------------------------------------------------------------------------------------------

namespace MarcusMedina.IO.JsonFile.Tests;

using MarcusMedina.IO.JsonFile;
using Xunit;

/// <summary>
/// The json file tests.
/// </summary>
public class JsonFileTests : IDisposable
{
    /// <summary>
    /// The filename.
    /// </summary>
    private const string filename = "Hello.txt";

    /// <summary>
    /// The test message
    /// </summary>
    private const string message = "Hello Crazy World";

    /// <summary>
    /// Initialize the tests - runs before each test
    /// </summary>
    public JsonFileTests()
    {
        if (File.Exists(filename))
            File.Delete(filename);
    }

    /// <summary>
    /// Cleans up the tests - runs after each test
    /// </summary>
    public void Dispose()
    {
        if (File.Exists(filename))
            File.Delete(filename);

        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Test to see if implicit conversion works.
    /// </summary>
    [Fact]
    public void ImplicitGetDataTest()
    {
        // Arrange
        var file = new JsonFile<List<string>>(filename)
        {
            Data = new List<string> { "One", "Two", "Three" }
        };
        var expected = file.Data.Count;

        // Act
        List<string> actual = file;

        // Assert
        Assert.Equal(expected, actual.Count);
    }

    /// <summary>
    /// Jsons the file test.
    /// </summary>
    [Fact]
    public void JsonFileTest()
    {
        var file = new JsonFile<List<string>>("test")
        {
            Data = new List<string>() { message },
            Format = null
        };
        Assert.NotNull(file);
    }

    /// <summary>
    /// Loads a file that does not exist test.
    /// </summary>
    [Fact]
    public void LoadEmptyFileTest()
    {
        // Arrange
        var file = new JsonFile<List<string>>("FileDoesNotExist");

        // Act
        _ = file.Load();

        // Assert
        Assert.NotNull(file.Data);
        Assert.Empty(file.Data);
    }

    /// <summary>
    /// Loads a test.
    /// </summary>
    [Fact]
    public void LoadTest()
    {
        // Arrange
        var expected = new List<string>() { message };
        File.WriteAllText(filename, message);
        var file = new JsonFile<List<string>>(filename);

        // Act
        _ = file.Load();

        // Assert
        Assert.Equal(message, string.Join(',', expected));
    }

    /// <summary>
    /// Loads a file that does not exist test.
    /// </summary>
    [Fact]
    public void SavesEvenIfDataIsNull()
    {
        // Arrange
        var file = new JsonFile<List<string>>("DataNull");

        // Act
        file.Save();

        // Assert
        Assert.NotNull(file.Data);
        Assert.Empty(file.Data);
    }

    /// <summary>
    /// Saves a test.
    /// </summary>
    [Fact]
    public void SaveTest()
    {
        // Arrange
        var file = new JsonFile<List<string>>(filename)
        {
            Data = new List<string>() { "Hello Crazy World" },
            Format = null,
        };

        // Act
        file.Save();

        // Assert
        Assert.NotNull(file);
    }

    /// <summary>
    /// Saves a test with backup file creation.
    /// </summary>
    [Fact]
    public void SaveTestBackup()
    {
        // Arrange
        var file = new JsonFile<List<string>>(filename)
        {
            Data = new List<string>() { "Hello Crazy World" },
            Format = null,
        };

        if (File.Exists(filename + ".json.bak"))
            File.Delete(filename + ".json.bak");

        // Act
        file.Save();
        file.Save();

        // Assert
        Assert.True(File.Exists(filename + ".json.bak"));
    }
}