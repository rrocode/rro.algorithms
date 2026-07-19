using Algorithms.SlidingWindow.SlidingWindowMaximum;

public class NaiveSlidingWindowMaximum : ISlidingWindowMaximum
{
    public int[] MaxSlidingWindow(int[] nums, int k)
    {
        if (k < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(k), "sliding window size must be greater than 0.");
        }

        if (nums.Length < k || k == 1)
        {
            return nums;
        }

        var maxIterations = nums.Length - k + 1;
        var maxByIteration = new int[maxIterations];
        for (int i = 0; i < maxIterations; i++)
        {
            for (int y = 0; y < k; y++)
            {
                if (y == 0)
                {
                    maxByIteration[i] = nums[i + y];
                    continue;
                }
                if (maxByIteration[i] < nums[i + y])
                {
                    maxByIteration[i] = nums[i + y];
                }
            }
        }

        return maxByIteration.ToArray();
    }
}