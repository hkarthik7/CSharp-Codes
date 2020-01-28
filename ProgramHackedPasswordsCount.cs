using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace HackedPasswords
{
    class HackedPasswordsCount
    {
        static void Main(string[] args)
        {
            // variables declaration
            var passWord = new List<string> { };
            Dictionary<string, int> dict = new Dictionary<string, int>();

            // Get input and run the program
            Console.Write("Enter Passwords to check: ");
            var readInput = Console.ReadLine().Split(",");

            foreach (var input in readInput)
            {
                passWord.Add(input.Trim());
            }

            foreach (var word in passWord)
            {
                var checkHash = GetHashedPassword(word);
                var passTrim = TrimHashedPassword(checkHash);
                var response = GetResponse(passTrim[0]);
                var hackCounts = GetHackedCounts(response, passTrim[1]);

                dict.Add(word, hackCounts);
            }
            foreach (KeyValuePair<string, int> kvp in dict)
            {
                Console.WriteLine($"( SuppliedPassword = {kvp.Key} , HackedCounts = {kvp.Value} )");
            }
        }
        public static string GetResponse(string hashvalue)
        {
            var url = "https://api.pwnedpasswords.com/range/" + hashvalue;
            var passRequest = new HttpClient();
            var responseText = passRequest.GetStringAsync(url);
            return responseText.Result;
        }

        public static string GetHashedPassword(string password)
        {
            SHA1Managed sha1 = new SHA1Managed();
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(password));
            var hashedpassword = new StringBuilder(hash.Length * 2);

            foreach (byte b in hash)
            {
                hashedpassword.Append(b.ToString("X2"));
            }
            return hashedpassword.ToString();
        }

        public static string[] TrimHashedPassword(string hashedpassword)
        {
            var start = hashedpassword.Substring(0,5);
            var tail = hashedpassword.Substring(5);
            var stringTail = new string[2] { start, tail };
            return stringTail;           
        }

        public static int GetHackedCounts(string passwordresponse, string hashedtail)
        {
            var hackedCounts = new List<string>();
            var splitResponse = passwordresponse.Split();
            
            foreach (var item in splitResponse)
            {
                if (!(string.IsNullOrWhiteSpace(item))) 
                {
                    hackedCounts.Add(item);
                }
            }

            foreach (var value in hackedCounts)
            {
                if (value.Split(":")[0] == hashedtail)
                {
                    return Convert.ToInt32(value.Split(":")[1]);
                }
            }
            return 0;
        }
    }
}
