using Newtonsoft.Json;
using MongoDB.Bson.Serialization.Attributes;

// All score-related configuration for a stage. Every field is optional in the
// JSON; the constructor seeds the defaults that Newtonsoft keeps when a property
// is absent.
public class ScoringDefinition
{
    public ScoringDefinition()
    {
        this.FullScore = 90;
        this.PenaltyPerSecond = 1;
        this.PenaltyPerReveal = 10;
        this.PenaltyPerReshuffle = 12;
        this.BonusPerCorrectChar = 5;
    }

    // The score the player starts the stage with (and the maximum). Drives the
    // length of the progress bar (currentScore / FullScore). Defaults to 300.
    [BsonElement("full_score")]
    [JsonProperty(PropertyName = "full_score")]
    public int FullScore
    {
        get; set;
    }

    // Backing fields for the star thresholds. When the JSON omits a threshold the
    // backing stays null and the getter falls back to a fraction of FullScore, so
    // the default scales with whatever FullScore the stage ends up with.
    private int? threeStarScore;
    private int? twoStarScore;

    // Final-score threshold (inclusive) to earn 3 stars. Defaults to 2/3 of FullScore.
    [BsonElement("three_star_score")]
    [JsonProperty(PropertyName = "three_star_score")]
    public int ThreeStarScore
    {
        get { return threeStarScore ?? (FullScore - 5); }
        set { threeStarScore = value; }
    }

    // Final-score threshold (inclusive) to earn 2 stars; below it earns 1 star.
    // Defaults to 1/3 of FullScore.
    [BsonElement("two_star_score")]
    [JsonProperty(PropertyName = "two_star_score")]
    public int TwoStarScore
    {
        get { return twoStarScore ?? (FullScore / 2); }
        set { twoStarScore = value; }
    }

    // Score deducted each second while the stage is being played. Defaults to 1.
    [BsonElement("penalty_per_second")]
    [JsonProperty(PropertyName = "penalty_per_second")]
    public int PenaltyPerSecond
    {
        get; set;
    }

    // Score deducted each time the player uses a reveal hint. Defaults to 10.
    [BsonElement("penalty_per_reveal")]
    [JsonProperty(PropertyName = "penalty_per_reveal")]
    public int PenaltyPerReveal
    {
        get; set;
    }

    // Score deducted each time the player reshuffles the board. Defaults to 15.
    [BsonElement("penalty_per_reshuffle")]
    [JsonProperty(PropertyName = "penalty_per_reshuffle")]
    public int PenaltyPerReshuffle
    {
        get; set;
    }

    // Score awarded each time the player forms a correct character. Defaults to 10.
    [BsonElement("bonus_per_correct_char")]
    [JsonProperty(PropertyName = "bonus_per_correct_char")]
    public int BonusPerCorrectChar
    {
        get; set;
    }
}
