using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HangmanGame;

public enum GameState { InProgress, Won, Lost } // stav hry

public enum GuessResult //výsledek
{
    Correct,
    Incorrect,
    Invalid,
    Duplicate,
    GameOver
}

public class Hangman
{
    private readonly string _secret; //ALLCAPS, jen A–Z
    private readonly int _maxIncorrect; // limit chyb pro hru
    private readonly HashSet<char> _correct = new();
    private readonly HashSet<char> _usedLetters = new();
    private int _wrongGuesses = 0;

    public Hangman(string secretWord, int maxIncorrectGuesses) // Zajišťuje “EN-only” a vše převádí na velká písmena
    {
        if (string.IsNullOrWhiteSpace(secretWord))
            throw new ArgumentException("Secret word must not be empty.", nameof(secretWord));

        if (maxIncorrectGuesses < 0)
            throw new ArgumentOutOfRangeException(nameof(maxIncorrectGuesses),
                "Max incorrect guesses must be >= 0.");

        if (!TextHelper.IsValidSecretWord(secretWord))
            throw new ArgumentException("Use only English letters (A-Z), spaces or hyphens.", nameof(secretWord));

        _secret = secretWord.Trim().ToUpperInvariant();
        _maxIncorrect = maxIncorrectGuesses;
        State = GameState.InProgress;
    }

    public GameState State { get; private set; }
    public bool IsInProgress => State == GameState.InProgress;

    //Maskování slova
    public string MaskedWord
    {
        get
        {
            var sb = new StringBuilder(_secret.Length);
            foreach (char ch in _secret)
            {
                if (IsLetter(ch))
                    sb.Append(_correct.Contains(ch) ? ch : '_');
                else
                    sb.Append(ch);
            }
            return sb.ToString();
        }
    }

    //vytvoření nového listu při každém čtení
    public IReadOnlyCollection<char> CorrectGuesses =>
        _correct.OrderBy(c => c).ToList().AsReadOnly();
    public IReadOnlyCollection<char> UsedLetters =>
    _usedLetters.OrderBy(c => c).ToList().AsReadOnly();

    public int RemainingGuesses => Math.Max(0, _maxIncorrect - _wrongGuesses);
    public string SecretWord => _secret;

    //Zpracuje pokus o hádání jednoho písmene
    public GuessResult Guess(char letter)
    {
        if (!IsInProgress)
            return GuessResult.GameOver;

        char normalized = char.ToUpperInvariant(letter);
        if (!IsLetter(normalized))
            return GuessResult.Invalid;

        if (_correct.Contains(normalized) || _usedLetters.Contains(normalized))
            return GuessResult.Duplicate;

        if (_secret.Contains(normalized))
        {
            _correct.Add(normalized);
            RecalculateState();
            return GuessResult.Correct;
        }

        _wrongGuesses++; // zvýšení počtu chybných pokusů
        RecalculateState();
        return GuessResult.Incorrect;
    }

    //zpracuje tip písmena (validace, duplicita, zásah/vedle) a přepočítá stav
    public GuessResult GuessWord(string word)
    {
        if (!IsInProgress)
            return GuessResult.GameOver;
        if (string.IsNullOrWhiteSpace(word))
            return GuessResult.Invalid;
        if (!TextHelper.IsValidSecretWord(word))
            return GuessResult.Invalid;

        string normalized = word.Trim().ToUpperInvariant();

        if (normalized == _secret)
        {
            foreach (char ch in _secret.Where(IsLetter))
                _correct.Add(ch);

            State = GameState.Won;
            return GuessResult.Correct;
        }

        _wrongGuesses++;
        RecalculateState();
        return GuessResult.Incorrect;
    }

    //určí Won/Lost/InProgress podle správných/špatných pokusů.
    private void RecalculateState()
    {
        if (_wrongGuesses >= _maxIncorrect)
        {
            State = GameState.Lost;
            return;
        }

        var needed = _secret.Where(IsLetter).Distinct();
        if (needed.All(c => _correct.Contains(c)))
        {
            State = GameState.Won;
            return;
        }

        State = GameState.InProgress;
    }
    private static bool IsLetter(char c) => c is >= 'A' and <= 'Z'; //vrátí true, pokud je znak A–Z

    //ASCII “šibenice” podle počtu chyb.
    public string GetHangmanAscii()
    {
        int wrong = _wrongGuesses;
        return wrong switch
        {
            0 => @"
            +---+
            |   |
                |
                |
                |
                |
            =========",
            1 => @"
            +---+
            |   |
            O   |
                |
                |
                |
            =========",
            2 => @"
            +---+
            |   |
            O   |
            |   |
                |
                |
            =========",
            3 => @"
            +---+
            |   |
            O   |
            /|  |
                |
                |
            =========",
            4 => @"
            +---+
            |   |
            O   |
            /|\ |
                |
                |
            =========",
            5 => @"
            +---+
            |   |
            O   |
            /|\ |
            /   |
                |
            =========",
            _ => @"
            +---+
            |   |
            O   |
            /|\ |
            / \ |
                |
            ========="
        };
    }
}
