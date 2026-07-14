using Algorithms.LongestPalindrome;

public class NaiveLongestPalindrome : ILongestPalindrome
{
    public string FindLongestPalindrome(string s) {
        
        if (string.IsNullOrEmpty(s)) {
            return "";
        }

        if (s.Length == 1) {
            return s;
        }

        // this would give a wrong result for "ab"
        // while correct one is "a"
        //if (s.Length == 2) {
        //    return s[0] == s[1] ? s : "";
        //}

        // we assume first is
        var longestPalindromeWord = s[0].ToString();
        for(int start = 0; start < s.Length; start++) {
            for(int end = 0; end < s.Length; end++) {
                int currentLenght = end - start + 1;
                if (currentLenght <= longestPalindromeWord.Length)
                    continue;

                if (IsPalindrome(s, start, end)) {
                    longestPalindromeWord = s.Substring(start, currentLenght);
                }
            }
        }

        return longestPalindromeWord;
    }

    private bool IsPalindrome(string s, int start, int end) {
        while (start < end) {
            // we check if the current complete cut is a palindrome
            if (s[start] != s[end]) {
                return false;
            }

            start++;
            end--;
        }

        return true;
    }
}