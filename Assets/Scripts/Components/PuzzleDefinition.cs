using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleDefinition
{
    public PuzzleDefinition()
    {

    }

    [JsonProperty(PropertyName = "selected_lines")]
    public List<int> SelectedLines
    {
        get; set;
    }

    [JsonProperty(PropertyName = "uncovered_chars")]
    public List<int> UncoveredCharIndexes
    {
        get; set;
    }

    [JsonProperty(PropertyName = "panel_size")]
    public PuzzleBoardSize BoardSize
    {
        get; set;
    }

    [JsonProperty(PropertyName = "noise_chars")]
    public List<string> NoiseCharIds
    {
        get; set;
    }

    
    public static PuzzleDefinition LoadFromJsonText(string jsonText)
    {
        PuzzleDefinition def = new PuzzleDefinition();
        def = JsonConvert.DeserializeObject<PuzzleDefinition>(jsonText);
        return def;
    }
}
