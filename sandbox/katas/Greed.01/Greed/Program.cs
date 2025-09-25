using System;
using Greed;

var rnd = new Random();

// === ČÁST 1: Hráč hází vždy 5 kostkami ===
int[] kostky5 = new int[5];
for (int i = 0; i < kostky5.Length; i++)
{
    kostky5[i] = rnd.Next(1, 7);
}
Console.WriteLine("=== Hod 5 kostkami ===");
Console.WriteLine($"Hod: [{string.Join(",", kostky5)}]");

int zaklad5 = PocitadloSkore.SpocitejSkore(kostky5, extraPravidla: false);
int extra5 = PocitadloSkore.SpocitejSkore(kostky5, extraPravidla: true);

Console.WriteLine($"Skóre (základní): {zaklad5}");
Console.WriteLine($"Skóre (extra):   {extra5}");


// === ČÁST 2: Hráč může hodit 1–6 kostkami ===
int pocetKostek = rnd.Next(1, 7); // náhodně 1–6
int[] kostkyRandom = new int[pocetKostek];
for (int i = 0; i < kostkyRandom.Length; i++)
{
    kostkyRandom[i] = rnd.Next(1, 7);
}

Console.WriteLine("\n=== Hod náhodným počtem kostek (1-6) ===");
Console.WriteLine($"Počet kostek: {pocetKostek}");
Console.WriteLine($"Hod: [{string.Join(",", kostkyRandom)}]");

int zakladR = PocitadloSkore.SpocitejSkore(kostkyRandom, extraPravidla: false);
int extraR = PocitadloSkore.SpocitejSkore(kostkyRandom, extraPravidla: true);

Console.WriteLine($"Skóre (základní): {zakladR}");
Console.WriteLine($"Skóre (extra): {extraR}");
