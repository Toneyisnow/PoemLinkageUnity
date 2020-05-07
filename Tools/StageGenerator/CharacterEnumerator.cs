using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StageGenerator
{
    public class CharacterEnumerator
    {
        public void EnumerateFolder(string folderPath, string resourceCharsFolder, string outputFile)
        {
            Dictionary<string, string> missingChars = new Dictionary<string, string>();

            foreach(string inputFileName in Directory.EnumerateFiles(folderPath, "*.def"))
            {
                string content = string.Empty;
                using (StreamReader reader = new StreamReader(inputFileName))
                {
                    content = reader.ReadToEnd();
                }

                StageDefinition stage = JsonConvert.DeserializeObject<StageDefinition>(content);
                foreach(int line in stage.Puzzle.SelectedLines)
                {
                    foreach(string character in stage.Poem.Content[line])
                    {
                        CheckCharacter(character, resourceCharsFolder, missingChars);
                    }
                }

                foreach (string character in stage.Puzzle.NoiseCharacters)
                {
                    CheckCharacter(character, resourceCharsFolder, missingChars);
                }

                foreach(var formula in stage.Formulas)
                {
                    foreach(string formulaCh in formula)
                    {
                        CheckCharacter(formulaCh, resourceCharsFolder, missingChars);
                    }
                }
            }

            // Write to output file
            using(StreamWriter sw = new StreamWriter(outputFile))
            {
                foreach(string unicode in missingChars.Keys)
                {
                    sw.WriteLine(string.Format(@"{0}    {1}", unicode, missingChars[unicode]));
                }
            }
        }

        private void CheckCharacter(string character, string resourceCharsFolder, Dictionary<string, string> missingChars)
        {
            if (character.StartsWith("~") || character.StartsWith("*"))
            {
                return;
            }

            // read the string as UTF-8 bytes.
            byte[] encodedBytes = Encoding.UTF8.GetBytes(character);

            // convert them into unicode bytes.
            byte[] unicodeBytes = Encoding.Convert(Encoding.UTF8, Encoding.Unicode, encodedBytes);
            try
            {

                string unicodeString = UnicodeBytesToString(unicodeBytes);

                string resourceFileFullPath = Path.Combine(resourceCharsFolder, string.Format(@"c_{0}.png", unicodeString));
                if (!File.Exists(resourceFileFullPath) && !missingChars.ContainsKey(unicodeBytes.ToString()))
                {
                    missingChars[unicodeString] = character;
                }
            }
            catch (Exception ex)
            {
            }
        }

        static private string UnicodeBytesToString(byte[] value)
        {
            StringBuilder hex = new StringBuilder(4);
            hex.AppendFormat("{0:x2}", value[1]);
            hex.AppendFormat("{0:x2}", value[0]);
            return hex.ToString();
        }
    }
}
