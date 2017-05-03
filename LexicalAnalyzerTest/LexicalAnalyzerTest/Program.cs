using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace LexicalAnalyzerTest
{
    class Pet
    {
        public string family { get; set; }
        public int speed { get; set; }

        public override string ToString()
        {
            return $"Family : {family}, Speed : {speed}";
        }
    }

    class Program
    {
        static readonly List<char> PossibleCharacters = new List<char> { '=', '!', '>', '<'};

        public static List<Pet> Pets = new List<Pet>
        {
            new Pet { family="dog", speed=100},
            new Pet { family="dog", speed=325},
            new Pet { family="cat", speed=50},
            new Pet { family="cat", speed=350}
        };

        static void Main(string[] args)
        {
            var fields = args[0].Split(',');

            IEnumerable<Pet> pets = Pets;
            
            if (fields != null)
            { 
                foreach (var field in fields)
                {
                    var sb = new StringBuilder();
                    int index = 0;
                    while (!PossibleCharacters.Contains(field[index])) sb.Append(field[index++]);

                    var leftside = sb.ToString();
                    var symbol = field[index];
                    var rightside = field.Substring(index + 1);

                    pets = getMatches(pets, leftside, symbol, rightside);
                }
            }

            var petsList = pets.ToList();
            petsList.ForEach(Console.WriteLine);

            Console.Read();
        }

        static IEnumerable<Pet> getMatches(IEnumerable<Pet> pets, string leftside, char symbol, string rightside)
        {
            var prop = typeof(Pet).GetProperty(leftside);

            if (prop.PropertyType == typeof(string))
            {
                switch (symbol)
                {
                    case '=': return pets.Where(x => (string)prop.GetValue(x) == rightside);
                    case '!': return pets.Where(x => (string)prop.GetValue(x) != rightside);
                }
            }
            else if (prop.PropertyType == typeof(int))
            {
                int value = Convert.ToInt32(rightside);
                switch (symbol)
                {
                    case '=': return pets.Where(x => (int)prop.GetValue(x) == value);
                    case '!': return pets.Where(x => (int)prop.GetValue(x) != value);
                    case '>': return pets.Where(x => (int)prop.GetValue(x) > value);
                    case '<': return pets.Where(x => (int)prop.GetValue(x) < value);
                }
            }

            return pets;
        }
    }
}