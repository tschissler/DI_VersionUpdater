using SharedContracts;

namespace CounterIncreaserV1
{
    public class CounterIncreaserV1 : ICounterIncreaser
    {
        public int IncreaseCounter(int currentValue)
        {
            return currentValue + 1;
        }
    }
}