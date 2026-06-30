// -----------------------------------------------------------------------------------------------
//  JsonFile.cs by Marcus Medina, Copyright (C) 2021-2025, http://MarcusMedina.Pro.
//  Published under MIT License
//  https://opensource.org/licenses/MIT
// -----------------------------------------------------------------------------------------------

namespace MarcusMedina.IO.JsonFile;

using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

/// <summary>
/// Defines the <see cref="JsonFile{T}" />.
/// </summary>
/// <typeparam name="T">Any object that can be instansiated.</typeparam>
public class JsonFile<T> : IDisposable where T : new()
{
    /// <summary>
    /// Initializes a new instance of the <see cref="JsonFile{T}"/> class.
    /// </summary>
    /// <param name="filename">The filename<see cref="string"/>.</param>
    public JsonFile(string filename)
    {
        Filename = filename;
        Format = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles,
        };
        _ = Load();
    }

    /// <summary>
    /// Gets or sets the Data object.
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Gets or sets the Filename, just the name, without suffix. <see cref="Suffix"/> will be added automatically.
    /// </summary>
    public string Filename { get; } = "";

    /// <summary>
    /// Gets or Sets the serialisation format settings.
    /// </summary>
    public JsonSerializerOptions? Format { get; set; }

    /// <summary>
    /// Gets or Sets the suffix (default is .json).
    /// </summary>
    public string Suffix { get; set; } = "json";

    /// <summary>
    /// Implicit conversion operator to convert JsonFile&lt;T&gt; to T.
    /// </summary>
    /// <param name="file">The JsonFile instance to convert.</param>
    /// <returns>The Data property of type T.</returns>
    public static implicit operator T(JsonFile<T> file)
    {
        file.Data ??= new T();
        return file.Data;
    }

    /// <summary>
    /// Loads the file.
    /// </summary>
    public T? Load()
    {
        var data = "[]";
        if (File.Exists($"{Filename}.{Suffix}"))
        {
            try
            {
                data = File.ReadAllText($"{Filename}.{Suffix}");
            }
            catch (IOException ex)
            {
                Debug.WriteLine("Error reading file:");
                Debug.WriteLine(ex.Message);
                data = "[]";
            }
        }

        try
        {
            Data = JsonSerializer.Deserialize<T>(data, Format);
        }
        catch (JsonException ex)
        {
            Debug.WriteLine("Error deserializing file:");
            Debug.WriteLine(ex.Message);
            Data = new T();
        }

        return Data;
    }

    /// <summary>
    /// Saves the file.
    /// </summary>
    public void Save()
    {
        var json = "";
        Data ??= new T();
        try
        {
            json = JsonSerializer.Serialize(Data, Format);
        }
        catch (JsonException ex)
        {
            Debug.WriteLine("Error serializing data:");
            Debug.WriteLine(ex.Message);
        }

        if (File.Exists($"{Filename}.{Suffix}.bak"))
            File.Delete($"{Filename}.{Suffix}.bak");

        if (File.Exists($"{Filename}.{Suffix}"))
            File.Move($"{Filename}.{Suffix}", $"{Filename}.{Suffix}.bak");

        try
        {
            File.WriteAllText($"{Filename}.{Suffix}", json);
        }
        catch (IOException ex)
        {
            Debug.WriteLine("Error saving data:");
            Debug.WriteLine(ex.Message);
        }
    }

    /// <summary>
    /// Disposes the JsonFile instance, saving any pending changes.
    /// </summary>
    public void Dispose()
    {
        Save();
        Data = default;

        //This will prevent derived types that introduce a finalizer from needing to re-implement 'IDisposable' to call it.
        GC.SuppressFinalize(this);
    }
}