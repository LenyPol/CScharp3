
namespace HangmanGame;

public class Program
{
    public static void Main()
    {
        //Úvod a konfigurace limitu pro hádání
        Console.WriteLine("=== Hangman ===");

        int max = 6;
        Console.Write($"Max incorrect guesses [{max}]: ");
        if (int.TryParse(Console.ReadLine(), out int parsed) && parsed >= 0)
            max = parsed;

        var wordSource = WordSources.Create("words_en.txt"); // volba tajného slova
        var secret = wordSource.GetRandomWord();

        try
        {
            var game = new Hangman(secret, max);

            Console.Clear();// skrytí tajného slova z obrazovky
            while (game.IsInProgress)
            {
                Console.WriteLine(game.GetHangmanAscii());
                Console.WriteLine($"\nWord   : {game.MaskedWord}");
                Console.WriteLine($"Right  : {string.Join(", ", game.CorrectGuesses)}");
                Console.WriteLine($"Wrong  : {string.Join(", ", game.IncorrectGuesses.Where(char.IsLetter))}");
                Console.WriteLine($"Left   : {game.RemainingGuesses}");

                Console.Write("\nGuess a letter (A-Z) or type the whole word: "); //vstup od hráče  a vyhodnocení
                var line = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(line))
                {
                    Console.WriteLine("Please enter a letter or a word.");
                    continue;
                }

                var trimmed = line.Trim();
                GuessResult result = trimmed.Length == 1
                    ? game.Guess(trimmed[0])
                    : game.GuessWord(trimmed);

                Console.WriteLine(result switch // vypsání výsledku
                {
                    GuessResult.Invalid => "❌ Invalid input.",
                    GuessResult.Duplicate => "⚠️  You already tried that.",
                    GuessResult.Correct => "✅ Correct!",
                    GuessResult.Incorrect => "❌ Wrong.",
                    GuessResult.GameOver => "The game is already over.",
                    _ => result.ToString()
                });
                Console.WriteLine();
            }

            Console.WriteLine(game.GetHangmanAscii()); // závěr hry a vykreslení
            Console.WriteLine(new string('═', 50));
            Console.WriteLine($"GAME OVER! The word was: {game.SecretWord}");
            Console.WriteLine(game.State == GameState.Won ? "🎉 YOU WON!" : "💀 YOU LOST.");
            Console.WriteLine(new string('═', 50));
        }
        catch (ArgumentException ex) // ošetření chyb
        {
            Console.WriteLine($"Input error: {ex.Message}");
        }
    }
}
