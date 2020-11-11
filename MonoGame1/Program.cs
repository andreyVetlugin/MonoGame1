using System;

namespace MonoGame1
{
    public static class Program
    {
        [STAThread]
        static void Main(string [] args)
        {
            using (var game = new Game1(args[1],args[2],Convert.ToInt32(args[3])))
                game.Run();
        }
    }
}
