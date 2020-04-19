using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;
using MongoDB.Bson.Serialization.Attributes;

public class StageDefinition
{
    public StageDefinition()
    {

    }

    public int StageId
    {
        get; set;
    }

    [BsonElement("poem")]
    [JsonProperty(PropertyName = "poem")]
    public PoemDefinition PoemDefinition
    {
        get; set;
    }

    [BsonElement("puzzle")]
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

    [BsonElement("formula")]
    [JsonProperty(PropertyName = "formula")]
    public List<List<string>> FormulaRaws
    {
        get; set;
    }

    public FormulaDefinition FindFormula(string targetCharId)
    {
        foreach(FormulaDefinition formula in FormulaDefinitions)
        {
            if (formula.Target == targetCharId)
            {
                return formula;
            }
        }

        return null;
    }

    public FormulaDefinition FindFormula(string sourceCharIdA, string sourceCharIdB)
    {
        foreach (FormulaDefinition formula in FormulaDefinitions)
        {
            if (formula.SourceA == sourceCharIdA && formula.SourceB == sourceCharIdB ||
                formula.SourceA == sourceCharIdB && formula.SourceB == sourceCharIdA)
            {
                return formula;
            }
        }

        return null;
    }
}
