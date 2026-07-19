using Algorithms.SlidingWindow.SlidingWindowMaximum;
using Xunit;

namespace Algorithms.Tests.SlidingWindow;

public class SlidingWindowMaximumTests
{
    public static IEnumerable<object[]> TestCases =>
    [
        [new[] { 1, 3, -1, -3, 5, 3, 6, 7 }, 3, new[] { 3, 3, 5, 5, 6, 7 }],
        [new[] { 1, 2, 3, 4, 5 }, 3, new[] { 3, 4, 5 }],
        [new[] { 5, 4, 3, 2, 1 }, 3, new[] { 5, 4, 3 }],
        [new[] { 2, 2, 2, 2 }, 2, new[] { 2, 2, 2 }],
        [new[] { 1, 3, -1, -3, 5, 3, 6, 7 }, 1, new[] { 1, 3, -1, -3, 5, 3, 6, 7 }],
        [new[] { 1, 3, -1, -3, 5, 3, 6, 7 }, 8, new[] { 7 }],
    ];

    [Theory]
    [MemberData(nameof(TestCases))]
    public void Naive_ReturnsCorrectMaximums(int[] nums, int k, int[] expected)
    {
        var sut = new NaiveSlidingWindowMaximum();
        Assert.Equal(expected, sut.MaxSlidingWindow(nums, k));
    }

    [Fact]
    public void Naive_Throws_WhenSlidingWindowSizeIsLessThanOne()
    {
        var sut = new NaiveSlidingWindowMaximum();

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => sut.MaxSlidingWindow([1, 2, 3], -1));

        Assert.Equal("k", exception.ParamName);
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void Optimal_ReturnsCorrectMaximums(int[] nums, int k, int[] expected)
    {
        var sut = new OptimalSlidingWindowMaximum();
        Assert.Equal(expected, sut.MaxSlidingWindow(nums, k));
    }

    [Fact]
    public void Optimal_Throws_WhenSlidingWindowSizeIsLessThanOne()
    {
        var sut = new OptimalSlidingWindowMaximum();

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => sut.MaxSlidingWindow([1, 2, 3], -1));

        Assert.Equal("k", exception.ParamName);
    }
}
