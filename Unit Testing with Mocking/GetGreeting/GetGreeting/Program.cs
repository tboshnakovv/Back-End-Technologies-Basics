namespace GetGreeting
{
    class Program
    {
        static void Main(string[] args)
        {
            GreetingProvider greetingProvider = new();
            string greeting = greetingProvider.GetGreeting();
            Console.WriteLine(greeting);
        }
    }

}