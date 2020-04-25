using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageRecord
{
    public int StageId;

    /// <summary>
    /// 0 indicates not completed, 1-3 indicates score.
    /// </summary>
    public int HighestScore
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

    public static StageRecord Create(int stageId)
    {
        StageRecord record = new StageRecord();
        record.StageId = stageId;
        record.HighestScore = 0;
        return record;
    }
}
