using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace cygfixpath
{
    class Program
    {
        static void Main(string[] args)
        {
            var arg = string.Join(" ", args).Trim();
            if (string.IsNullOrWhiteSpace(arg))
            {
                Console.WriteLine("Usage: fixpath (-s) PATH");
                Console.WriteLine("       fixpath [-E|-E NAME|-H]");
                Console.WriteLine("");
                Console.WriteLine("Path conversion options:");
                Console.WriteLine("");
                Console.WriteLine("  -s, --slash        Replace Forward Slashes with Back Slashes");
                Console.WriteLine("");
                Console.WriteLine("System information:");
                Console.WriteLine("");
                Console.WriteLine("  -E, --environment  output all environmental variables and exit");
                Console.WriteLine("  -U, --userprofile  output the `USERPROFILE` directory and exit");
                Console.WriteLine("  -H, --home         output the cygwin home directory and exit");
                Console.WriteLine("");
            }

            var key = arg.Substring(2, arg.Length - 2).Trim();

            if (arg.StartsWith("-s") || arg.StartsWith("--slash"))
            {
                Console.Write(key.Replace("/", "\\"));
                return;
            }

            

            var environmentDict = GetEnvironmentVariables();
            if (environmentDict == null) return;

            if (arg.StartsWith("-E") || arg.StartsWith("--environment"))
            {
                if (arg.Trim().Equals("-E"))
                {
                    foreach (KeyValuePair<string, string> entry in environmentDict)
                        Console.WriteLine(entry.Key + "|" + entry.Value);
                    return;
                }
                else
                {
                    if (environmentDict.ContainsKey(key))
                        Console.WriteLine(environmentDict[key]);
                    return;
                }
            }
            if (arg.StartsWith("-H") || arg.StartsWith("--home"))
            {
                if (environmentDict.ContainsKey("USERPROFILE"))
                    Console.Write(environmentDict["USERPROFILE"].Replace("Users", @"cygdrive\" + environmentDict["USERPROFILE"].Substring(0,1).ToLower() + @"\Users"));
                return;
            }

#if DEBUG
            Console.ReadLine();
#endif
        }

        /// <summary>
        /// method for getting all available environment variables
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetEnvironmentVariables()
        {
            try
            {
                //dictionary object to hold the key/value pairs
                Dictionary<string, string> variables = new Dictionary<string, string>();

                //loop through the list and add to our dictionary list
                Parallel.ForEach<DictionaryEntry>(Environment.GetEnvironmentVariables().OfType<DictionaryEntry>(), entry =>
                {
                    variables.Add(entry.Key.ToString(), entry.Value.ToString());
                });

                return variables;
            }
            catch (SecurityException ex)
            {
                Console.WriteLine("Error retrieving environment variables: {0}", ex.Message);
                return null;
            }
        }




    }
}
