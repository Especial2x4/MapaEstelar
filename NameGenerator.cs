using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapaEstelar
{
    public class NameGenerator
    {
        private static readonly string[] Prefixes =
        {
        "Alpha", "Beta", "Gamma", "Delta", "Epsilon",
        "Zeta", "Eta", "Theta", "Iota", "Kappa",
        "Lambda", "Mu", "Nu", "Xi", "Omicron",
        "Pi", "Rho", "Sigma", "Tau", "Upsilon",
        "Phi", "Chi", "Psi", "Omega"
    };

        private static readonly string[] Suffixes =
        {
        "Prime", "Minor", "Major", "II", "III",
        "IV", "V", "VI", "VII", "VIII",
        "IX", "X", "A", "B", "C", "D", "E",
        "F", "G", "H"
    };

        public static string GenerateStarSystemName()
        {
            Random rand = new Random();
            string prefix = Prefixes[rand.Next(Prefixes.Length)];
            string suffix = Suffixes[rand.Next(Suffixes.Length)];
            return $"{prefix} {suffix}";
        }
    }

}
