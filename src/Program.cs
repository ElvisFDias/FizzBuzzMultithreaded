using Microsoft.VisualBasic.CompilerServices;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace FizzBuzzMultithreaded
{
    partial class Program
    {
        static Action<string> print = (value) => { Console.Write($"{(Console.CursorLeft == 0 ? string.Empty : ", ")}{value}"); };
        static Action printFizz = () => print("fizz");
        static Action printBuzz = () => print("buzz");
        static Action printFizzBuzz = () => print("fizzbuzz");
        static Action<int> printNumber = (number) => print(number.ToString());

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("Type n: ");

                var input = Console.ReadLine();

                if (int.TryParse(input, out int n))
                {
                    var fizzBuzz = new FizzBuzz(n);

                    var fizzTask = Task.Run(() => fizzBuzz.Fizz(printFizz) );
                    var buzzTask = Task.Run(() => fizzBuzz.Buzz(printBuzz) );
                    var fizzbuzzTask = Task.Run(() => fizzBuzz.Fizzbuzz(printFizzBuzz));
                    var numberTask = Task.Run(() => fizzBuzz.Number(printNumber) );
                    
                    Task.WaitAll(fizzTask, buzzTask, fizzbuzzTask, numberTask);

                    Console.WriteLine($"{Environment.NewLine}########## End ########## {Environment.NewLine}");
                }
                else
                    break;

            }

        }

  

    
    }
}
