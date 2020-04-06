using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleDefinition
{
    public PuzzleDefinition()
    {

    }

    public List<int> SelectedLines
    {
        get; set;
    }

    public List<int> UncoveredCharIndexes
    {
        get; set;
    }

    public List<string> NoiseCharIds
    {
        get; set;
    }

    public PuzzleBoardSize BoardSize
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
