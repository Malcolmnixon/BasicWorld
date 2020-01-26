using System;

namespace WebServerWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create and start server
            var world = new BasicWorld.WebGame.WebServerWorld();
            world.Start();
            
            Console.WriteLine("Server started. Press ENTER to terminate.");
            Console.ReadLine();
        }
    }
}
