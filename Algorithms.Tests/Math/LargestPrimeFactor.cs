namespace Algorithms.Tests.Math.LargestPrimeFactor;

using System.Diagnostics;
using Algorithms.Math.LargestPrimeNumbers;

public class LargestPrimeFactorTests
{
    [Fact]
    public void Assert_99vs11()
    {
        var sut = new LargestPrimeNumbers();

        long largestPrimeFactor = sut.CalculateFrom(99);
        Assert.Equal(11, largestPrimeFactor);
    }

    [Fact]
    public void Assert_33vs11()
    {
        var sut = new LargestPrimeNumbers();

        long largestPrimeFactor = sut.CalculateFrom(33);
        Assert.Equal(11, largestPrimeFactor);
    }

    [Fact]
    public void Assert_ProjectEuler3_Sample()
    {
        var sut = new LargestPrimeNumbers();

        long largestPrimeFactor = sut.CalculateFrom(13195);
        Assert.Equal(29, largestPrimeFactor);
    }

    [Fact]
    public void Assert_ProjectEuler3()
    {
        var sut = new LargestPrimeNumbers();
        
        Stopwatch timer = Stopwatch.StartNew();
        long largestPrimeFactor = sut.CalculateFrom(600851475143);
        Console.WriteLine($" Elapsed: {timer.Elapsed} ms");
        Assert.Equal(6857, largestPrimeFactor);
    }

    [Fact]
    public void Assert_ProjectEuler3_6857()
    {
        var sut = new LargestPrimeNumbers();

        long largestPrimeFactor = sut.CalculateFrom(600851475143);
        Assert.Equal(6857, largestPrimeFactor);
    }
}