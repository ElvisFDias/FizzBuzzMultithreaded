using System;
using System.Threading;

namespace FizzBuzzMultithreaded
{
    public class FizzBuzz
    {
        private int n;
        private int currentNumber;
        private bool isFinalized;
        private readonly SemaphoreSlim semaphoreFizz;
        private readonly SemaphoreSlim semaphoreBuzz;
        private readonly SemaphoreSlim semaphoreFizzBuzz;
        private readonly SemaphoreSlim semaphoreNumber;
        private readonly CancellationTokenSource ts;

        public FizzBuzz(int n) 
        {
            this.n = n;
            this.currentNumber = 0;
            this.semaphoreFizz = new SemaphoreSlim(0, 1);
            this.semaphoreBuzz = new SemaphoreSlim(0, 1);
            this.semaphoreFizzBuzz = new SemaphoreSlim(0, 1);
            this.semaphoreNumber = new SemaphoreSlim(0, 1);
            this.ts = new CancellationTokenSource();
            this.MoveToNextNumber();
        }

        // only output "fizz"
        public void Fizz(Action printFizz) 
        {
            while (!ts.IsCancellationRequested)
            {
                try
                {
                    semaphoreFizz.Wait(ts.Token);
                }
                catch (OperationCanceledException) { return; }
                
                printFizz();

                MoveToNextNumber();
            }

        }

        // only output "buzz"
        public void Buzz(Action printBuzz) {
            while (!ts.IsCancellationRequested)
            {
                try
                {
                    semaphoreBuzz.Wait(ts.Token);
                }
                catch (OperationCanceledException) { return; }             

                printBuzz();
                
                MoveToNextNumber();
            }

        }

        // only output "fizzbuzz"
        public void Fizzbuzz(Action printFizzBuzz) 
        {
            while (!ts.IsCancellationRequested)
            {
                try
                {
                    semaphoreFizzBuzz.Wait(ts.Token);
                }
                catch (OperationCanceledException) { return; }
                
                printFizzBuzz();
                
                MoveToNextNumber();
            }

        }

        // only output the numbers
        public void Number(Action<int> printNumber)
        {
            while (!ts.IsCancellationRequested)
            {
                try
                {
                    semaphoreNumber.Wait(ts.Token);
                }
                catch (OperationCanceledException) { return; }

                printNumber(this.currentNumber);
                
                MoveToNextNumber();
            }

        }

        public bool IsFinalized() => this.isFinalized;

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

        private void MoveToNextNumber()
        {
            this.isFinalized = Interlocked.Increment(ref this.currentNumber) > this.n;

            if (this.isFinalized)
                ts.Cancel();

            else if (IsFizzBuzz())
                semaphoreFizzBuzz.Release();

            else if (IsFizz())
                semaphoreFizz.Release();

            else if (IsBuzz())
                semaphoreBuzz.Release();

            else
                semaphoreNumber.Release();
        }

            
    }
}
