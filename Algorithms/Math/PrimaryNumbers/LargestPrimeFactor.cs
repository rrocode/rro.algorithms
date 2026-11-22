using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Microsoft.VisualBasic;

namespace Algorithms.Math.LargestPrimeNumbers;

public class LargestPrimeNumbers
{
    int counter = 0;
    public int CalculateFrom(long primeNumber, int nextLargestPrimenumberFactor = 3, bool print = false, int maxAttempts = 10000)
    {
        if (primeNumber <= 2)
        {
            throw new InvalidOperationException("Invalid input");
        }

        if (primeNumber == nextLargestPrimenumberFactor)
        {
            return nextLargestPrimenumberFactor;
        }

        if (counter > maxAttempts)
        {
            throw new InvalidOperationException("Too many attempts");
        }

        if (print)
        {
            Console.WriteLine($"Iteration: {++counter}");
            Console.WriteLine($"Prime Number: {primeNumber}; LargestPrimeNumberFactor: {nextLargestPrimenumberFactor}");   
        }
        
        int largestPrimeNumberFactor = nextLargestPrimenumberFactor; // base
        while ( primeNumber % largestPrimeNumberFactor == 0 )
        {
            primeNumber /= largestPrimeNumberFactor;
            if (print)
            {
                Console.WriteLine($"New PrimeNumber: {primeNumber}, LargestPrimeNumber: {largestPrimeNumberFactor}");   
            }            
        }

        if (primeNumber > largestPrimeNumberFactor)
        {
            largestPrimeNumberFactor = largestPrimeNumberFactor + 2;
            largestPrimeNumberFactor = CalculateFrom(primeNumber, largestPrimeNumberFactor, print, maxAttempts);
        }

        return largestPrimeNumberFactor;
    }
}