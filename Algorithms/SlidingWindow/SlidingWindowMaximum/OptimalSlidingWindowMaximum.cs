using Algorithms.SlidingWindow.SlidingWindowMaximum;

public class OptimalSlidingWindowMaximum : ISlidingWindowMaximum
{
    public int[] MaxSlidingWindow(int[] nums, int k)
    {
        if (k < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(k), "sliding window size must be greater than 0.");
        }

        var deque = new LinkedList<int>();
        var result = new List<int>();
        for (int i = 0; i < nums.Length; i++)
        {
            // Remove indices that are out of the current window
            if (deque.Count > 0 && deque.First?.Value < i - k + 1)
            {
                deque.RemoveFirst();
            }

            // Remove indices whose corresponding values are less than nums[i] from the back of the deque
            while (deque.Count > 0 && nums[deque.Last?.Value ?? 0] < nums[i])
            {
                deque.RemoveLast();
            }

            // Add the current index to the deque
            deque.AddLast(i);

            // The front of the deque contains the index of the maximum element for the current window,
            // so once window size is reached, we can add it to the result
            if (i >= k - 1)
            {
                result.Add(nums[deque.First?.Value ?? 0]);
            }
        }

        return result.ToArray();
    }
}