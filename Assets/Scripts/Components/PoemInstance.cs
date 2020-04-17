using System;
using System.Collections.Generic;
using UnityEngine;

public class PoemInstance
{
    // private PoemDefinition definition = null;

    public int Width
    {
        get; private set;
    }

    public int Height
    {
        get; private set;
    }

    private string[,] characterIds = null;
    private HashSet<int> uncoveredCharIndexes = null;

    public PoemInstance(PoemDefinition def, List<int> selectedLines, HashSet<int> uncoveredIndexes = null)
    {
        //this.definition = def;
        if (def == null || selectedLines == null)
        {
            throw new ArgumentNullException();
        }

        this.Width = def.SentenceLength;
        this.Height = selectedLines.Count;
        this.characterIds = new string[Width, Height];
        for(int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                this.characterIds[i, j] = def.Content[selectedLines[j]][i];
            }
        }

        this.uncoveredCharIndexes = (uncoveredIndexes == null) ? new HashSet<int>() : uncoveredIndexes;
    }

    public bool IsAllCharactersUncovered()
    {
        return (uncoveredCharIndexes.Count == 0);
    }

    public HashSet<int> GetUncoveredChars()
    {
        return uncoveredCharIndexes;
    }

    public List<string> GetUncoveredCharIds()
    {
        List<string> result = new List<string>();
        foreach(int charIndex in uncoveredCharIndexes)
        {
            int x = charIndex % this.Width;
            int y = charIndex / this.Width;
            result.Add(this.characterIds[x, y]);
        }

        return result;
    }
    public HashSet<int> GetCoveredChars()
    {
        HashSet<int> result = new HashSet<int>();
        for(int x = 0; x < this.Width; x++)
        {
            for(int y = 0; y < this.Height; y++)
            {
                if (this.characterIds[x, y] != null)
                {
                    int index = y * this.Width + x;
                    if (!uncoveredCharIndexes.Contains(index))
                    {
                        result.Add(index);
                    }
                }
            }
        }

        Debug.Log("GetCoveredChars: " + result);
        return result;
    }

    public List<string> GetCoveredCharIds()
    {
        List<string> result = new List<string>();
        var covered = GetCoveredChars();

        foreach (int charIndex in covered)
        {
            int x = charIndex % this.Width;
            int y = charIndex / this.Width;
            result.Add(this.characterIds[x, y]);
        }

        return result;
    }

    public void setUncoveredAt(int index)
    {
        uncoveredCharIndexes.Add(index);
    }

    public bool IsUncoveredAt(int index)
    {
        return uncoveredCharIndexes.Contains(index);
    }

    public string GetCharacterIdAt(int w, int h)
    {
        return this.characterIds[w, h];
    }

}


