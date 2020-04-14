using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;

public class StageDefinition
{
    public StageDefinition()
    {

    }

    public int StageId
    {
        get; set;
    }

    [JsonProperty(PropertyName = "poem")]
    public PoemDefinition PoemDefinition
    {
        get; set;
    }

    [JsonProperty(PropertyName = "puzzle")]
    public PuzzleDefinition PuzzleDefinition
    {
        get; set;
    }

    private List<FormulaDefinition> formulaDefinitions = null;
    public List<FormulaDefinition> FormulaDefinitions
    {
        get
        {
            if (formulaDefinitions == null)
            {
                formulaDefinitions = new List<FormulaDefinition>();
                if (FormulaRaws != null)
                {
                    foreach(List<string> stringList in FormulaRaws)
                    {
                        var formula = FormulaDefinition.LoadFromArray(stringList.ToArray());
                        formulaDefinitions.Add(formula);
                    }
                }
            }

            return formulaDefinitions;
        }
    }

    [JsonProperty(PropertyName = "formula")]
    public List<List<string>> FormulaRaws
    {
        get; set;
    }
}
