using SharedContracts;

namespace CounterIncreaserV2
{
    public class CounterIncreaserV2 : ICounterIncreaser
    {
        public int IncreaseCounter(int currentValue)
        {
            return currentValue + 10;
        }
    }
}