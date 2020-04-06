using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDefinition
{
    public StageDefinition()
    {

    }

    public int StageId
    {
        get; set;
    }

    public PoemDefinition PoemDefinition
    {
        get; set;
    }

    public PuzzleDefinition PuzzleDefinition
    {
        get; set;
    }

    public List<FormulaDefinition> FormulaDefinitions
    {
        get; set;
    }

}
