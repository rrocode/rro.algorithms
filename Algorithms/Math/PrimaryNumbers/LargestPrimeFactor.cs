namespace Algorithms.Math.LargestPrimeNumbers;

public class LargestPrimeNumbers
{
    int counter = 0;
    public int CalculateFrom(long primeNumber, int nextLargestPrimenumberFactor = 3)
    {
        if (primeNumber <= 2)
        {
            throw new InvalidOperationException("Invalid input");
        }

        if (primeNumber == nextLargestPrimenumberFactor)
        {
            return nextLargestPrimenumberFactor;
        }

        if (counter > 1000)
        {
            throw new InvalidOperationException("Too many attempts");
        }

        Console.WriteLine($"Iteration: {++counter}");
        Console.WriteLine($"Prime Number: {primeNumber}; LargestPrimeNumberFactor: {nextLargestPrimenumberFactor}");
        
        int largestPrimeNumberFactor = nextLargestPrimenumberFactor; // base
        long primeNumberBackup = primeNumber;
        while ( primeNumber % largestPrimeNumberFactor == 0 )
        {
            primeNumber /= largestPrimeNumberFactor;
            Console.WriteLine($"New PrimeNumber: {primeNumber}, LargestPrimeNumber: {largestPrimeNumberFactor}");
        }

        if (primeNumber > largestPrimeNumberFactor)
        {
            largestPrimeNumberFactor = largestPrimeNumberFactor + 2;
            largestPrimeNumberFactor = CalculateFrom(primeNumberBackup, largestPrimeNumberFactor);
        }

        return largestPrimeNumberFactor;
    }
}