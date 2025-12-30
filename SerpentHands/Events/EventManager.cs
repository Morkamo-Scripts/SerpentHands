namespace SerpentHands.Events
{
    public static class EventManager
    {
        public static PlayerEvents PlayerEvents { get; private set; } = new();
        public static RoundEvents RoundEvents { get; private set; } = new();
    }
}