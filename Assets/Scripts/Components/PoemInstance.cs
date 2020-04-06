using System.Collections.Generic;

public class PoemInstance
{
    private PoemDefinition definition = null;

    private List<int> UncoveredCharIndexes = null;

    public PoemInstance(PoemDefinition def, List<int> uncoveredIndexes = null)
    {
        this.definition = def;

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

}


