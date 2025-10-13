using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Linq;

namespace HangmanGame;

//abstrakce pro získání náhodného slova
public interface IWordSource
{
    string GetRandomWord();
}
public sealed class FileWordSource : IWordSource
{
    private readonly List<string> _words;
    //načte slova ze souboru, vyčistí a odfiltruje neplatná/duplicitní
    public FileWordSource(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException("Word list file not found.", path);

        _words = File.ReadAllLines(path)
            .Select(line => line.Trim())
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Where(TextHelper.IsValidSecretWord)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (_words.Count == 0)
            throw new InvalidOperationException("Word list file has no valid entries.");
    }

    public string GetRandomWord() // náhodný výběr
    {
        int idx = Random.Shared.Next(_words.Count);
        return _words[idx];
    }
}

// vrátí náhodné slovo z vestavěného pole, lze rozšířit
public sealed class BuiltInWordSource : IWordSource
{
    private static readonly string[] Words =
    {
        "APPLE", "ORANGE", "BANANA", "PINEAPPLE", "STRAWBERRY",
        "COMPUTER", "KEYBOARD", "MONITOR", "MOUSE", "PRINTER",
        "DEVELOPER", "SOFTWARE", "HARDWARE", "NETWORK", "SECURITY",
        "PYTHON", "CLOUD", "SERVER", "CLIENT", "DATABASE",
        "DEVIL", "ANGEL", "MOUNTAIN", "RIVER", "FOREST",
        "CITY", "VILLAGE", "COUNTRY", "OCEAN", "ISLAND"
    };
    public string GetRandomWord()
    {
        return Words[Random.Shared.Next(Words.Length)];
    }
}
// Factory preferuje zdroj založený na souborech (pokud je k dispozici a platný), jinak se vrátí k vestavěnému seznamu
public static class WordSources
{
    public static IWordSource Create(string? filePath = null)
    {
        if (!string.IsNullOrWhiteSpace(filePath) && File.Exists(filePath))
        {
            try
            {
                return new FileWordSource(filePath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Falling back to built-in words: {ex.Message}");
            }
        }

        return new BuiltInWordSource();
    }
}
