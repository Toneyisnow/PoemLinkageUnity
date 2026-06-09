using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleDefinition
{
    public PuzzleDefinition()
    {
        // Default number of reveal hints granted for a stage when the JSON omits
        // "reveal_count". Newtonsoft keeps this value if the property is absent.
        this.RevealCount = 3;
    }

    [BsonElement("selected_lines")]
    [JsonProperty(PropertyName = "selected_lines")]
    public List<int> SelectedLines
    {
        get; set;
    }

    [BsonElement("uncovered_chars")]
    [JsonProperty(PropertyName = "uncovered_chars")]
    public HashSet<int> UncoveredCharIndexes
    {
        get; set;
    }

    [BsonElement("panel_size")]
    [JsonProperty(PropertyName = "panel_size")]
    public PuzzleBoardSize BoardSize
    {
        get; set;
    }

    [BsonElement("noise_chars")]
    [JsonProperty(PropertyName = "noise_chars")]
    public List<string> NoiseCharIds
    {
        get; set;
    }

    [BsonElement("is_easy_mode")]
    [JsonProperty(PropertyName = "is_easy_mode")]
    public bool IsEasyMode
    {
        get; set;
    }

    // Number of reveal hints granted for the stage. When 0, the reveal button is
    // hidden in the main game scene. Defaults to 3 (set in the constructor) when
    // the JSON omits "reveal_count".
    [BsonElement("reveal_count")]
    [JsonProperty(PropertyName = "reveal_count")]
    public int RevealCount
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
