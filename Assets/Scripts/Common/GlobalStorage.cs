using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GlobalStorage
{
    public static int CurrentCategory = 0;

    public static int CurrentStage = 0;

    // Set when returning from the stage-select page so SelectCategoryScene plays
    // the title slide-in animation on entry.
    public static bool AnimateCategoryTitle = false;

    // Set when navigating between categories with the left/right arrows so the
    // reloaded SelectCategoryScene slides its title in from the matching side.
    //  1 = moved to the next category (clicked right), slide in from the right
    // -1 = moved to the previous category (clicked left), slide in from the left
    //  0 = no horizontal slide
    public static int CategorySlideDirection = 0;

    private static Dictionary<string, Sprite> spritesDictionary = null;

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
            Debug.Log(string.Format("No saved record for stage {0} yet (expected for unplayed stages).", stageId));
            return null;
        }

        BinaryFormatter bf = new BinaryFormatter();
        StageRecord record = (StageRecord)bf.Deserialize(file);
        file.Close();


        Debug.Log("Loaded Record: " + record.StageId);
        Debug.Log("Loaded Record: " + record.HighestScore);

        return record;
    }

    public static void LoadSpriteDictionary()
    {
        if (spritesDictionary != null)
        {
            return;
        }

        spritesDictionary = new Dictionary<string, Sprite>();
        Sprite[] spriteSheet1 = Resources.LoadAll<Sprite>("characters/fzlb/fzlb_01");
        Sprite[] spriteSheet2 = Resources.LoadAll<Sprite>("characters/fzlb/fzlb_02");
        Sprite[] spriteSheet3 = Resources.LoadAll<Sprite>("characters/fzlb/fzlb_03");
        Sprite[] spriteSheet4 = Resources.LoadAll<Sprite>("characters/fzlb/fzlb_04");
        List<Sprite> spriteAll = new List<Sprite>();
        spriteAll.AddRange(spriteSheet1);
        spriteAll.AddRange(spriteSheet2);
        spriteAll.AddRange(spriteSheet3);
        spriteAll.AddRange(spriteSheet4);

        foreach (Sprite sprite in spriteAll)
        {
            if (!spritesDictionary.ContainsKey(sprite.name))
            {
                spritesDictionary[sprite.name] = sprite;
            }
        }
    }
    
    public static Sprite GetSpriteFromDictionary(string characterId)
    {
        if (spritesDictionary == null)
        {
            return null;
        }

        string name = "c_" + characterId;
        if (spritesDictionary.ContainsKey(name))
        {
            return spritesDictionary[name];
        }
        else
        {
            return null;
        }
    }
}
