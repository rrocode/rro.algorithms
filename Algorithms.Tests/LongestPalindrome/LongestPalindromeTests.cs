using Algorithms.LongestPalindrome;
using Xunit;

namespace Algorithms.Tests.LongestPalindrome;

public class LongestPalindromeTests
{
    public static IEnumerable<object[]> TestCases =>
    [
        ["babad", "bab"],
        ["cbbd", "bb"],
        ["a", "a"],
        ["ac", "a"],
        ["racecar", "racecar"],
        ["forgeeksskeegfor", "geeksskeeg"],
        ["abacdfgdcaba", "aba"],
        ["abacdfgdcabba", "abba"],
        ["abacdedcaba", "abacdedcaba"]
    ];

    [Theory]
    [MemberData(nameof(TestCases))]
    public void Naive_FindLongestPalindrome(string s, string expected)
    {
        var sut = new NaiveLongestPalindrome();
        Assert.Equal(expected, sut.FindLongestPalindrome(s));
    }
}
