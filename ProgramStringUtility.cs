using System;
using System.Collections.Generic;
using System.Numerics;
using System.Net.Http;

namespace StringUtility
{
    class StringManager
    {
        static void Main(string[] args)
        {   
            // TODO: Change the string manipulation GetSynonyms method.

            Console.Write("Enter a number : ");
            var number = Convert.ToInt32(Console.ReadLine());

            // Calculate factorial for given integer
            var factorial = GetFactorial(number);
            Console.WriteLine("Factorial of {0} is : {1}", number, factorial);

            // Random alphabets
            var randomAlphabets = GetRandomAlphabets(number);
            Console.WriteLine(randomAlphabets);

            //Remove duplicates
            var removeDuplicates = RemoveDuplicates(randomAlphabets);
            Console.WriteLine(removeDuplicates);
            
            // Permutations
            var Permutations = StringPermutations(removeDuplicates);
            foreach (var item in Permutations)
            {
                Console.WriteLine(item);
            }

            // Synonyms
            foreach (var item in Permutations)
            {
                try
                {
                    foreach (var word in GetSynonyms(item))
                    {
                        Console.WriteLine(word);
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("No synonyms found!");
                }               
            }     
        }

        // Factorial of the given integer
        public static BigInteger GetFactorial(BigInteger number)
        {
            if (number <= 1)
            {
                return number;
            }
            else
            {
                return number * GetFactorial(number - 1);
            }
        }

        // Get random alphabets
        public static string GetRandomAlphabets(int number)
        {
            if (number < 1)
            {
                return "";
            }
            else
            {
                var randomAlphabets = "";
                var random = new Random();
                var i = 0;
                while (i < number)
                {
                    randomAlphabets += (char)random.Next(97,122);
                    i++;
                }                
                return randomAlphabets;
            }
        }

        // Remove Duplicate letters from a String
        public static string RemoveDuplicates(string word)
        {
            var newWord = new List<string>();
            var finalWord = "";
            for (var i=0; i < word.Length; i++)
            {
                if (!(newWord.Contains(word[i].ToString())))
                {
                    newWord.Add(word[i].ToString());
                }
            }

            foreach (var item in newWord)
            {
                finalWord += item;
            }
            return finalWord;
        }
        
        // Get string permutations
        public static List<string> StringPermutations(string word)
        {
            if (word.Length <= 1) 
            {
                var permutationWords = new List<string>();
                permutationWords.Add(word);       
                return permutationWords;
            }

            else
            {
                var counter = word.Length;
                var permWords = new List<string>();
                for (var i=0; i < word.Length; i++)
                {
                    var firstLetter = word[i].ToString();
                    var remainingLetters = word.Substring(0,i) + word.Substring((i+1),(counter-1));
                    counter--;

                    foreach(var letter in StringPermutations(remainingLetters)) 
                    {
                        var finalWord = firstLetter + letter;
                        permWords.Add(finalWord);
                    }
                }
                return permWords;
            }
        }

        // Get synonyms for given string
        // TODO: Remove the string manipulation part and include HTML parser
        public static List<string> GetSynonyms(string word)
        {
            var uri = $"https://www.synonym.com/synonyms/{word}";
            var httpClient = new HttpClient();
            var html = httpClient.GetStringAsync(uri);
            var result = html.Result;

            var start_index = result.IndexOf("synonyms:");
            var end_index = result.IndexOf("|");
            var synonym = result.Substring(start_index,end_index);
            var indexOfAntonyms = synonym.IndexOf("antonyms");
            var _synonyms = synonym.Substring(0,indexOfAntonyms).Split(",");

            var synonyms = new List<string>();
            for (var i=0; i < _synonyms.Length; i++)
            {
                if (_synonyms[i].Contains(":"))
                    synonyms.Add(_synonyms[i].Split(":")[1].Trim());
                else if (_synonyms[i].Contains("|"))
                    synonyms.Add(_synonyms[i].Replace("|","").Trim());
                else
                    synonyms.Add(_synonyms[i].Trim());
            }
            return synonyms;
        }
    }
}
