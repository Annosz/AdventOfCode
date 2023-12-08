namespace Shared;
public static class MathHelpers
{
    public static long LeastCommonMultiple(int[] elements)
    {
        long leastCommonMultiple = 1;
        int divisor = 2;

        while (true)
        {

            int counter = 0;
            bool divisible = false;
            for (int i = 0; i < elements.Length; i++)
            {

                // LeastCommonMultiple (n1, n2, ... 0) = 0.
                // For negative number we convert into
                // positive and calculate lcm_of_array_elements.
                if (elements[i] == 0)
                {
                    return 0;
                }
                else if (elements[i] < 0)
                {
                    elements[i] *= -1;
                }
                if (elements[i] == 1)
                {
                    counter++;
                }

                // Divide elements by devisor if complete
                // division i.e. without remainder then replace
                // number with quotient; used for find next factor
                if (elements[i] % divisor == 0)
                {
                    divisible = true;
                    elements[i] /= divisor;
                }
            }

            // If divisor able to completely divide any number
            // from array multiply with LeastCommonMultiple
            // and store into LeastCommonMultiple and continue
            // to same divisor for next factor finding.
            // else increment divisor
            if (divisible)
            {
                leastCommonMultiple *= divisor;
            }
            else
            {
                divisor++;
            }

            // Check if all element_array is 1 indicate 
            // we found all factors and terminate while loop.
            if (counter == elements.Length)
            {
                return leastCommonMultiple;
            }
        }
    }
}
