using System.Text.Json;

namespace CompanyCup2026
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string jsonString = File.ReadAllText("Inputs/1.txt");

            // 2. Deserialize the string into your C# class
            var root = JsonSerializer.Deserialize<Root>(jsonString);
            
        }
    }
}
