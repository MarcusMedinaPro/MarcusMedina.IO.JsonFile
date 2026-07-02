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
    public JsonFileTests() => CleanupTestFiles();

    /// <summary>
    /// Cleans up the tests - runs after each test
    /// </summary>
    public void Dispose()
    {
        CleanupTestFiles();

        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Deletes every test artefact for <see cref="filename"/> — the base file, and every
    /// suffix/backup variant JsonFile can produce (".json", ".json.bak", custom suffixes, etc.).
    /// Without this, JsonFile's actual output files (e.g. "Hello.txt.json") never get cleaned
    /// between tests, since JsonFile writes to "{filename}.{suffix}", not "{filename}" itself.
    /// </summary>
    private static void CleanupTestFiles()
    {
        foreach (var path in Directory.GetFiles(".", $"{filename}*"))
            File.Delete(path);
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
    /// Loads previously saved data back correctly (round-trip).
    /// </summary>
    [Fact]
    public void LoadTest()
    {
        // Arrange
        var expected = new List<string>() { message };
        var writer = new JsonFile<List<string>>(filename)
        {
            Data = expected,
        };
        writer.Save();

        // Act
        var reader = new JsonFile<List<string>>(filename);
        var actual = reader.Load();

        // Assert
        Assert.Equal(expected, actual);
    }

    /// <summary>
    /// Loading a file with invalid/corrupt JSON falls back to a fresh instance instead of throwing.
    /// </summary>
    [Fact]
    public void LoadCorruptJsonFallsBackToNewInstanceTest()
    {
        // Arrange
        File.WriteAllText($"{filename}.json", "{ not valid json ][");

        // Act
        var file = new JsonFile<List<string>>(filename);

        // Assert
        Assert.NotNull(file.Data);
        Assert.Empty(file.Data);
    }

    /// <summary>
    /// Dispose() saves any pending changes before releasing the instance.
    /// </summary>
    [Fact]
    public void DisposeSavesPendingChangesTest()
    {
        // Arrange
        var expected = new List<string>() { message };

        // Act
        using (var file = new JsonFile<List<string>>(filename))
        {
            file.Data = expected;
        }

        // Assert
        var reloaded = new JsonFile<List<string>>(filename);
        Assert.Equal(expected, reloaded.Data);
    }

    /// <summary>
    /// A custom Suffix produces a file with that suffix instead of the default ".json".
    /// </summary>
    [Fact]
    public void CustomSuffixWritesExpectedFileTest()
    {
        // Arrange
        const string suffix = "bak2";
        var file = new JsonFile<List<string>>(filename)
        {
            Suffix = suffix,
            Data = new List<string>() { message },
        };

        // Act
        file.Save();

        // Assert
        Assert.True(File.Exists($"{filename}.{suffix}"));
        File.Delete($"{filename}.{suffix}");
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