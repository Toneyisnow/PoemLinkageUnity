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
                Console.WriteLine("Usage: StageGenerator.exe [DefinitionFileFullName]");
                return;
            }

            Generator generator = new Generator();
            generator.Generate(args[0]);
        }

        static void Check()
        {
            CharacterEnumerator enumerator = new CharacterEnumerator();

            enumerator.EnumerateFolder(
                @"D:\GitRoot\toneyisnow\PoemLinkageUnity\Assets\Resources\stages",
                @"D:\GitRoot\toneyisnow\PoemLinkageUnity\Assets\Resources\characters\fzlb",
                @".\out.txt");
        }
    }
}
