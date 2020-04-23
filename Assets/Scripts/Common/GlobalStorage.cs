using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GlobalStorage
{
    public static int CurrentCategory = 0;

    public static int CurrentStage = 0;

    public static void SaveRecord(StageRecord record)
    {
        string destination = Application.persistentDataPath + string.Format("/save_{0}.dat", record.StageId);
        FileStream file;

        if (File.Exists(destination))
        {
            file = File.OpenWrite(destination);
        }
        else
        {
            file = File.Create(destination);
        }

        Debug.Log("Saving record to " + destination);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, record);
        file.Close();
    }

    public static StageRecord LoadRecord(int stageId)
    {
        string destination = Application.persistentDataPath + string.Format("/save_{0}.dat", stageId);
        FileStream file;

        if (File.Exists(destination))
        {
            file = File.OpenRead(destination);
        }
        else
        {
            Debug.LogError("File not found");
            return null;
        }

        BinaryFormatter bf = new BinaryFormatter();
        StageRecord record = (StageRecord)bf.Deserialize(file);
        file.Close();


        Debug.Log("Loaded Record: " + record.StageId);
        Debug.Log("Loaded Record: " + record.HighestScore);

        return record;
    }
}
