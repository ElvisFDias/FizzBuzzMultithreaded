using System;
using System.Threading;

namespace FizzBuzzMultithreaded
{
    partial class Program
    {
        public class FizzBuzzOld
        {
            private int n;
            private int currentNumber;
            private SemaphoreSlim semaphoreFizzBuzz;
            private bool isFinalized;

            public FizzBuzzOld(int n) 
            {
                this.n = n;
                this.currentNumber = 1;
                this.semaphoreFizzBuzz = new SemaphoreSlim(1);
                this.isFinalized = this.currentNumber > n;
                
            }

            // only output "fizz"
            public void fizz(Action<string> printFizz) 
            {
                semaphoreFizzBuzz.Wait();
                if(IsFizz() && !IsFinalized())
                {
                    printFizz("fizz");
                    IncrementCurrent();
                }
                semaphoreFizzBuzz.Release();
            }

            // only output "buzz"
            public void buzz(Action<string> printBuzz) {
                semaphoreFizzBuzz.Wait();
                if (IsBuzz() && !IsFinalized())
                {
                    printBuzz("buzz");
                    IncrementCurrent();
                }
                semaphoreFizzBuzz.Release();
            }

            // only output "fizzbuzz"
            public void fizzbuzz(Action<string> printFizzBuzz) 
            {
                semaphoreFizzBuzz.Wait();
                if (IsFizzBuzz() && !IsFinalized())
                {
                    printFizzBuzz("fizzbuzz");
                    IncrementCurrent();
                }
                semaphoreFizzBuzz.Release();
            }

            // only output the numbers
            public void number(Action<string> printNumber)
            {
                semaphoreFizzBuzz.Wait();
                if (IsNumber() && !IsFinalized())
                {
                    printNumber(this.currentNumber.ToString());
                    IncrementCurrent();
                }
                semaphoreFizzBuzz.Release();
            }

            private bool IsFizz()
            {
                return 
                    (this.currentNumber % 3 == 0)
                    && (this.currentNumber % 5 != 0);
            }

            private bool IsBuzz()
            {
                return 
                    (this.currentNumber % 5 == 0)
                    && (this.currentNumber % 3 != 0);
            }

            private bool IsFizzBuzz()
            {

                return
                    (this.currentNumber % 3 == 0)
                    && (this.currentNumber % 5 == 0); 
            }

            private bool IsNumber()
            {
                return !IsBuzz() && !IsFizz() && !IsFizzBuzz();
            }

            private void IncrementCurrent()
            {
                this.isFinalized = Interlocked.Increment(ref this.currentNumber) > this.n;
            }
            
            public bool IsFinalized()
            {
                return this.isFinalized;
            }
        }

   
    }
}
