using System;
using System.Linq;

namespace Greed;

public class PocitadloSkore
{
    private static readonly int[] TrojiceBody = { 0, 1000, 200, 300, 400, 500, 600 };
    private static readonly int[] Nasobitel = { 0, 0, 0, 1, 2, 4, 8 };
    private static readonly int[] Postupka = { 1, 2, 3, 4, 5, 6 };

    public static int SpocitejSkore(int[] kostky, bool extraPravidla = true)
    {
        if (kostky == null || kostky.Length == 0)
        {
            return 0;
        }

        var pocty = new int[7];
        foreach (var k in kostky)
        {
            if (k >= 1 && k <= 6)
            {
                pocty[k]++;
            }
        }

        int special = 0;
        if (extraPravidla && kostky.Length == 6)
        {
            if (JePostupka(pocty))
            {
                special = 1200;
            }
            else if (JsouTriPary(pocty))
            {
                special = 800;
            }
        }

        int obecne = SpocitejTrojceAZbytek(pocty, extraPravidla);
        return Math.Max(special, obecne);
    }
    private static bool JePostupka(int[] kostky) =>
        kostky.Distinct().OrderBy(x => x).SequenceEqual(Postupka);

    private static bool JsouTriPary(int[] pocty) =>
        pocty.Count(x => x == 2) == 3;
    private static int SpocitejTrojceAZbytek(int[] pocty, bool extraPravidla)
    {
        int skore = 0;
        for (int hodnota = 1; hodnota <= 6; hodnota++)
        {
            int pocet = pocty[hodnota];

            if (pocet >= 3)
            {
                if (extraPravidla)
                {
                    skore += TrojiceBody[hodnota] * Nasobitel[pocet];
                }
                else
                {
                    skore += TrojiceBody[hodnota];
                    pocet -= 3;
                }
            }
            if (hodnota == 1)
            {
                skore += pocet * 100;
            }
            if (hodnota == 5)
            {
                skore += pocet * 50;
            }
        }
        return skore;
    }
}





