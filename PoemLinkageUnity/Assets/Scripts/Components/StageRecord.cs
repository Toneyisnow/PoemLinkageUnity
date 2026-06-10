using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageRecord
{
    public int StageId;

    /// <summary>
    /// 0 indicates not completed, 1-3 indicates star count. This is what the
    /// stage-select screen renders as stars.
    /// </summary>
    public int HighestScore
    {
        get; set;
    }

    /// <summary>
    /// The highest numeric final score the player has achieved on this stage.
    /// Older save files predate this field and deserialize it as 0.
    /// </summary>
    public int HighScoreValue
    {
        get; set;
    }

    public bool HasCompleted
    {
        get
        {
            return HighestScore > 0;
        }
    }

    public bool JustCompleted
    {
        get; set;
    }

    public static StageRecord Create(int stageId)
    {
        StageRecord record = new StageRecord();
        record.StageId = stageId;
        record.HighestScore = 0;
        record.JustCompleted = false;

        return record;
    }
}
