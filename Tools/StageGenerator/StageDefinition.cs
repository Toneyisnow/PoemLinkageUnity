using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StageGenerator
{
    public class StageDefinition
    {

        public StageDefinition()
        {

        }

        [JsonProperty(PropertyName = "poem")]
        public PoemDefinition Poem
        {
            get; set;
        }

        [JsonProperty(PropertyName = "puzzle")]
        public PuzzleDefinition Puzzle
        {
            get; set;
        }

        [JsonProperty(PropertyName = "formula")]
        public List<List<string>> Formulas
        {
            get; set;
        }
    }

    public class PoemDefinition
    {
        [JsonProperty(PropertyName = "id")]
        public string Identifier
        {
            get; set;
        }

        [JsonProperty(PropertyName = "title")]
        public List<string> Title
        {
            get; set;
        }

        [JsonProperty(PropertyName = "author")]
        public List<string> Author
        {
            get; set;
        }

        [JsonProperty(PropertyName = "line_count")]
        public int LineCount
        {
            get; set;
        }

        [JsonProperty(PropertyName = "column_count")]
        public int ColumnCount
        {
            get; set;
        }

        [JsonProperty(PropertyName = "content")]
        public List<List<string> > Content
        {
            get; set;
        }
    }

    public class PuzzleDefinition
    {
        [JsonProperty(PropertyName = "selected_lines")]
        public List<int> SelectedLines
        {
            get; set;
        }

        [JsonProperty(PropertyName = "uncovered_chars")]
        public HashSet<int> UncoveredCharacters
        {
            get; set;
        }

        [JsonProperty(PropertyName = "panel_size")]
        public int PanelSize
        {
            get; set;
        }

        [JsonProperty(PropertyName = "noise_chars")]
        public List<string> NoiseCharacters
        {
            get; set;
        }
    }

    public class FormulaDefinition
    {
        public List<string> characters
        {
            get; set;
        }
    }

}
