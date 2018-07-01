using Server;

namespace ServerConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var Server = new ServerSocket();

            Server.Bind(11000);
            Server.Listen(5);
            Server.Accept();
        }
    }
}
