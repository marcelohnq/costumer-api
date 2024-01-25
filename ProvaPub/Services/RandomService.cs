namespace ProvaPub.Services
{
    public class RandomService
    {
        private const int MaxValue = 100;

        private readonly Random _random;
        private int _prev;

        public RandomService()
        {
            int seed = Guid.NewGuid().GetHashCode();
            _random = new Random(seed);
            _prev = -1;
        }

        public int GetRandom()
        {
            int result = _random.Next(MaxValue);

            // Evita que 'Random' gere dois valores iguais seguidos
            while (result == _prev)
            {
                result = _random.Next(MaxValue);
            }

            _prev = result;

            return result;
        }

    }
}
