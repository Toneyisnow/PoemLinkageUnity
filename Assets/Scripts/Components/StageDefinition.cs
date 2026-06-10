using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;
using MongoDB.Bson.Serialization.Attributes;

public class StageDefinition
{
    public StageDefinition()
    {
        // Default scoring used when the stage JSON omits these fields. Newtonsoft
        // keeps these values if the corresponding property is absent.
        this.FullScore = 300;
        this.ThreeStarScore = 200;
        this.TwoStarScore = 100;
    }

    public int StageId
    {
        get; set;
    }

    // The score the player starts the stage with (and the maximum). Drives the
    // length of the progress bar (currentScore / FullScore).
    [BsonElement("full_score")]
    [JsonProperty(PropertyName = "full_score")]
    public int FullScore
    {
        get; set;
    }

    // Final-score threshold (inclusive) to earn 3 stars.
    [BsonElement("three_star_score")]
    [JsonProperty(PropertyName = "three_star_score")]
    public int ThreeStarScore
    {
        get; set;
    }

    // Final-score threshold (inclusive) to earn 2 stars; below it earns 1 star.
    [BsonElement("two_star_score")]
    [JsonProperty(PropertyName = "two_star_score")]
    public int TwoStarScore
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
