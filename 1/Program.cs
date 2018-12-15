using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            // ---- ask for starting frequency
            int startingFrequency = 0;
            Console.WriteLine("Enter starting frequency [default is 0]:");
            while (true)
            {
                string input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    // default value
                    break;
                }
                else if (int.TryParse(input, out startingFrequency))
                {
                    // valid input
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid value");
                }
            }
            Console.WriteLine("Starting frequency set to " + startingFrequency);

            // ---- ask for list of frequency changes
            List<int> changes = new List<int>();
            while (true)
            {
                Console.WriteLine("Enter next frequency change or press enter to continue:");

                string input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    // done entering changes
                    break;
                }

                int value;
                if (int.TryParse(input, out value))
                {
                    changes.Add(value);
                }
                else
                {
                    Console.WriteLine("Invalid value");
                }
            }
            Console.WriteLine("Change list:" + changes
                .Select(x => x.ToString())
                .Aggregate((i, j) => i + ", " + j)
            );
            
            // ---- find frequency repeated twice
            HashSet<int> frequencies = new HashSet<int>();
            int frequency = startingFrequency;
            bool isDone = false;
            while (!isDone)
            {
                foreach (var change in changes)
                {
                    frequency += change;
                    if (frequencies.Contains(frequency))
                    {
                        isDone = true;
                        break;
                    }
                    else
                    {
                        frequencies.Add(frequency);
                    }
                }
            }

            Console.WriteLine("Repeated Frequency = " + frequency);
        }
    }
}
