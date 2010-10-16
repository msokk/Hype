using System;

namespace Hype
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (HypeGame game = new HypeGame())
            {
                game.Run();
            }
        }
    }
#endif
}

