using System;

namespace MonoGame1
{
    public static class Program
    {
        [STAThread]
        static void Main(string [] args)
        {
            using (var game = new Game1(args[0],args[1],Convert.ToInt32(args[2])))
                game.Run();
        }
    }
}
