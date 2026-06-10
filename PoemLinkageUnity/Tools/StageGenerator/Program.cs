using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StageGenerator
{
    /// <summary>
    /// This command line tool is to help generate the data file for the poem stage.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Generate(args);
        }

        static void Generate(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: StageGenerator.exe [DefinitionFileFolder]");
                return;
            }

            for(int i = 1; i <= 3; i++)
            {
                for (int j = 1; j <= 9; j++)
                {
                    Generator generator = new Generator();
                    generator.Generate(string.Format(args[0], i * 100 + j));
                }
            }
        }

        static void Check()
        {
            CharacterEnumerator enumerator = new CharacterEnumerator();

            enumerator.EnumerateFolder(
                @"D:\GitRoot\toneyisnow\PoemLinkageUnity\Assets\Resources\stages",
                @"D:\GitRoot\toneyisnow\PoemLinkageUnity\Tools\Characters\fzlb",
                @".\out.txt");
        }
    }
}
