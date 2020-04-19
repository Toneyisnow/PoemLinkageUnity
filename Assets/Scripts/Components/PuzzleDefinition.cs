using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleDefinition
{
    public PuzzleDefinition()
    {

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

    
    public static PuzzleDefinition LoadFromJsonText(string jsonText)
    {
        PuzzleDefinition def = new PuzzleDefinition();
        def = JsonConvert.DeserializeObject<PuzzleDefinition>(jsonText);
        return def;
    }
}
