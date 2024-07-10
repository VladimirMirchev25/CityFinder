using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp7
{
    public class WordDistanceComparer : IComparer<string>
    {
        private string targetWord;

        public WordDistanceComparer(string target)
        {
            targetWord = target;
        }

        public int Compare(string x, string y)
        {
            int distX = LevenshteinDistance(x, targetWord);
            int distY = LevenshteinDistance(y, targetWord);

            return distX.CompareTo(distY);
        }

        public int LevenshteinDistance(string s, string t)
        {
            int[,] d = new int[s.Length + 1, t.Length + 1];

            for (int i = 0; i <= s.Length; i++)
                d[i, 0] = i;

            for (int j = 0; j <= t.Length; j++)
                d[0, j] = j;

            for (int j = 1; j <= t.Length; j++)
            {
                for (int i = 1; i <= s.Length; i++)
                {
                    int cost = (s[i - 1] == t[j - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
                }
            }

            return d[s.Length, t.Length];
        }
    }

    public class ProximitySortExample
    {
        public static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
            Console.WriteLine("Въведете градовете (за край натиснете Enter без въвеждане):");

            List<string> cities = new List<string>();
            string input;

            while (true)
            {
                input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                    break;

                cities.Add(input);
            }

            Console.WriteLine("Въведете търсената дума:");
            string searchTerm = Console.ReadLine();

            Console.WriteLine("Въведете максималното допустимо разстояние:");
            int maxDistance = int.Parse(Console.ReadLine());

            var comparer = new WordDistanceComparer(searchTerm);

            List<string> matchingCities = new List<string>();
            List<string> closeCities = new List<string>();

            foreach (var city in cities)
            {
                string normalizedCity = city.ToLower();
                string normalizedSearchTerm = searchTerm.ToLower();

                if (normalizedCity.Contains(normalizedSearchTerm))
                {
                    matchingCities.Add(city);
                }
                else if (comparer.LevenshteinDistance(normalizedCity, normalizedSearchTerm) <= maxDistance)
                {
                    closeCities.Add(city);
                }
            }

            closeCities.Sort(comparer);

            Console.WriteLine("Резултати по близост до \"{0}\":", searchTerm);

            foreach (var city in matchingCities)
            {
                Console.WriteLine(city);
            }

            foreach (var city in closeCities)
            {
                Console.WriteLine(city);
            }

            Console.ReadKey();
        }
    }

}