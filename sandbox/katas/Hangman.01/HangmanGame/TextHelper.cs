using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace HangmanGame;

// Pomocná třída pro zachování textu
public static class TextHelper
{
    public static string RemoveDiacritics(string text) // odstranění diakritiky ikdyž nyní máme nastaveno pouze hádání anglických slov
    {
        if (string.IsNullOrWhiteSpace(text))
            return text;

        string normalized = text.Normalize(NormalizationForm.FormD); // rozdělí znaky na základní písmeno + kombinační značky
        var sb = new StringBuilder();

        foreach (char c in normalized)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }

        return sb.ToString().Normalize(NormalizationForm.FormC);
    }

    // Ověřuje pouze anglická písmena (A–Z / a–z) mezery, nebo pomlčky
    public static bool IsValidSecretWord(string word)
    {
        if (string.IsNullOrWhiteSpace(word))
            return false;

        string cleaned = RemoveDiacritics(word);
        return cleaned.All(c =>
            (c >= 'A' && c <= 'Z') ||
            (c >= 'a' && c <= 'z') ||
            c == ' ' || c == '-');
    }
    //Převedeme znak na string, odstraníme diakritiku (pro případ, že by šlo o „é“).
    public static bool IsValidGuessLetter(char letter)
    {
        string cleaned = RemoveDiacritics(letter.ToString());

        if (string.IsNullOrEmpty(cleaned))
            return false;

        char normalized = char.ToUpperInvariant(cleaned[0]);
        return normalized >= 'A' && normalized <= 'Z';
    }
}

