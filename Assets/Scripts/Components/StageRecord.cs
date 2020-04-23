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

}
