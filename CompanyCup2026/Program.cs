using System.Text.Json;
using System.Text.Json.Serialization;

namespace CompanyCup2026
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string jsonString = File.ReadAllText("Inputs/1.txt");

            // 2. Deserialize the string into your C# class
            var root = JsonSerializer.Deserialize<Root>(jsonString);
            
            var output = new OutputRoot();

            File.WriteAllText("output.txt", JsonSerializer.Serialize(output));
        }
    }
}
