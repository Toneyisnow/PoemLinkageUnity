using System;
using System.Collections.Generic;

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

    private string[,] CharacterIds = null;
    private List<int> UncoveredCharIndexes = null;

    public PoemInstance(PoemDefinition def, List<int> selectedLines, List<int> uncoveredIndexes = null)
    {
        //this.definition = def;
        if (def == null || selectedLines == null)
        {
            throw new ArgumentNullException();
        }

        this.Width = def.SentenceLength;
        this.Height = selectedLines.Count;
        this.CharacterIds = new string[Width, Height];
        for(int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                this.CharacterIds[i, j] = def.Content[selectedLines[j]][i];
            }
        }

        this.UncoveredCharIndexes = (uncoveredIndexes == null) ? new List<int>() : uncoveredIndexes;
    }

    public bool IsAllCharactersUncovered()
    {
        return false;
    }

    public List<int> GetUncoveredChars()
    {
        return null;
    }

    public List<int> GetCoveredChars()
    {
        return null;
    }

    public void setUncoveredAt(int index)
    {

    }

    public bool IsUncoveredAt(int index)
    {
        return false;
    }

    public string GetCharacterIdAt(int w, int h)
    {
        return this.CharacterIds[w, h];
    }

}


