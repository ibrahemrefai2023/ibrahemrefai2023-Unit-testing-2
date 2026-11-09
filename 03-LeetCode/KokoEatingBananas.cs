public class Solution
{
    public int MinEatingSpeed(int[] piles, int h)
    {
        var low = 1;
        var high = 0;

        foreach (var pile in piles)
        {
            if (pile > high)
                high = pile;
        }

        while (low < high)
        {
            var speed = low + (high - low) / 2;
            long neededHours = 0;

            foreach (var pile in piles)
                neededHours += ((long)pile + speed - 1) / speed;

            if (neededHours <= h)
                high = speed;
            else
                low = speed + 1;
        }

        return low;
    }
}
