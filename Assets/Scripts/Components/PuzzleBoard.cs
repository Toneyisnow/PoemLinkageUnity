using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PuzzleBoardStatus
{
    IDLE = 0,
    ONE_SELECTED = 1
}

public class PuzzleCharacter
{
    public string CharacterId
    {
        get; set;
    }

    public int Index
    {
        get; set;
    }

    public Vector2Int Position
    {
        get; set;
    }

    public PuzzleCharacter(int index, string charId)
    {
        this.Index = index;
        this.CharacterId = charId;
        this.Position = new Vector2Int(-1, -1);
    }

}

public interface PuzzleBoardHandler
{
    void OnChooseCharacter(PuzzleCharacter character);
    void OnUnchooseCharacter(PuzzleCharacter character, PuzzleCharacter firstCharacter);
    void OnMatchNotConnected(PuzzleCharacter character, PuzzleCharacter firstCharacter);
    void OnConnected(PuzzleCharacter character, PuzzleCharacter firstCharacter, List<Vector2Int> connectionPoints, string targetCharId);


}

public class PuzzleBoard
{
    private StageDefinition stageDefinition = null;

    private PoemDefinition poemDefinition = null;

    private PuzzleDefinition puzzleDefinition = null;

    private PoemInstance poemInstance = null;

    private PuzzleBoardHandler handler = null;

    private int currentCharacterIndex = 0;

    public int Width
    {
        get; private set;
    }

    public int Height
    {
        get; private set;
    }

    public int AutoIncreaseCharIndex
    {
        get; set;
    }

    public PuzzleBoardStatus Status
    {
        get; set;
    }

    public PuzzleCharacter LastSelectedCharacter
    {
        get; set;
    }

    public List<PuzzleCharacter> PuzzleCharacters
    {
        get; private set;
    }

    public PuzzleCharacter[,] CharacterMatrix
    {
        get; set;
    }

    
    public PuzzleBoard(PuzzleBoardHandler handler, StageDefinition stageDefinition, PoemInstance poemInstance)
    {
        this.handler = handler;
        this.stageDefinition = stageDefinition;
        this.poemDefinition = stageDefinition.PoemDefinition;
        this.puzzleDefinition = stageDefinition.PuzzleDefinition;
        this.poemInstance = poemInstance;

        Vector2 size = TranslateBoardSize(this.puzzleDefinition.BoardSize);
        this.Width = (int)size.x;
        this.Height = (int)size.y;

        List<string> targetCharacterIds = ComposeTargetCharIds();
        List<string> appearingCharacterIds = GenerateAppearingCharIds(targetCharacterIds);

        this.PuzzleCharacters = new List<PuzzleCharacter>();
        currentCharacterIndex = 0;
        foreach (string appearingCharId in appearingCharacterIds)
        {
            this.PushCharacter(appearingCharId);
        }

        Debug.Log("Pushed all appearing characters.");

        try
        {
            GenerateAndEnsureMatrix(this.PuzzleCharacters);
        }
        catch(Exception ex)
        {
            throw ex;
        }
    }

    public void TakeAction(PuzzleCharacter character)
    {
        if(this.Status == PuzzleBoardStatus.IDLE)
        {
            this.LastSelectedCharacter = character;
            this.Status = PuzzleBoardStatus.ONE_SELECTED;
            this.handler.OnChooseCharacter(character);

            return;
        }

        var matchingFormula = this.AreCharactersMatching(character, this.LastSelectedCharacter);

        if(character.Index == this.LastSelectedCharacter.Index || matchingFormula == null)
        {
            // Cancel selection
            this.Status = PuzzleBoardStatus.IDLE;
            this.handler.OnUnchooseCharacter(character, this.LastSelectedCharacter);
            return;
        }

        var connetionPoints = this.ConnectCharacters(character, this.LastSelectedCharacter);
        if (connetionPoints != null && connetionPoints.Count > 0)
        {
            // Erase the two characters
            this.ClearCharacter(this.LastSelectedCharacter);
            this.ClearCharacter(character);

            this.Status = PuzzleBoardStatus.IDLE;
            this.handler.OnConnected(character, this.LastSelectedCharacter, connetionPoints, matchingFormula.Target);
        }
        else
        {
            // Matching but cannot connect
            this.Status = PuzzleBoardStatus.IDLE;
            this.handler.OnMatchNotConnected(character, this.LastSelectedCharacter);
        }
    }

    public void GenerateAndEnsureMatrix(List<PuzzleCharacter> puzzleChars)
    {
        do
        {
            GenerateMatrix(puzzleChars);
        } while (this.IsMatrixDeadlock());
    }

    public bool CheckShuffle()
    {
        bool hasShuffled = false;

        var missingSourceChars = this.CheckBoardMissingSourceChars();
        if (missingSourceChars != null && missingSourceChars.Count > 0)
        {
            Debug.Log("CheckShuffle: Missing chars found, adding them.");

            foreach(string missingSourceChar in missingSourceChars)
            {
                var puzzleChar = this.PushCharacter(missingSourceChar);
                this.PlaceCharacterInMatrix(puzzleChar);
            }
            hasShuffled = true;
        }

        if (this.IsMatrixDeadlock())
        {
            this.GenerateAndEnsureMatrix(this.PuzzleCharacters);
            hasShuffled = true;
        }

        return hasShuffled;
    }

    private List<string> ComposeTargetCharIds()
    {
        List<string> result = new List<string>();

        var coveredChars = this.poemInstance.GetCoveredCharIds();
        if(coveredChars != null)
        {
            result.AddRange(coveredChars);
        }

        var noiseChars = this.puzzleDefinition.NoiseCharIds;
        if (noiseChars != null)
        {
            result.AddRange(noiseChars);
        }

        return result;
    }

    private List<string> GenerateAppearingCharIds(List<string> targetCharIds)
    {
        List<string> result = new List<string>();

        foreach(string targetCharId in targetCharIds)
        {
            var formula = this.stageDefinition.FindFormula(targetCharId);
            if(formula == null)
            {
                Debug.LogWarning("Formula not found for character " + targetCharId);
                continue;
            }

            result.Add(formula.SourceA);
            result.Add(formula.SourceB);
        }

        return result;
    }

    private void GenerateMatrix(List<PuzzleCharacter> puzzleChars)
    {
        this.CharacterMatrix = new PuzzleCharacter[this.Width + 2, this.Height + 2];

        foreach(PuzzleCharacter character in this.PuzzleCharacters)
        {
            this.PlaceCharacterInMatrix(character);
        }
    }

    private void PlaceCharacterInMatrix(PuzzleCharacter puzzleChar)
    {
        int posX = Utils.RandomInteger(1, this.Width);
        int posY = Utils.RandomInteger(1, this.Height);

        while(this.CharacterMatrix[posX, posY] != null)
        {
            posX = Utils.RandomInteger(1, this.Width);
            posY = Utils.RandomInteger(1, this.Height);
        }

        this.CharacterMatrix[posX, posY] = puzzleChar;
        puzzleChar.Position = new Vector2Int(posX, posY);
    }

    private bool IsMatrixDeadlock()
    {
        if (this.PuzzleCharacters == null || this.PuzzleCharacters.Count == 0)
        {
            // Empty
            return false;
        }

        foreach (PuzzleCharacter characterA in this.PuzzleCharacters)
        {
            foreach (PuzzleCharacter characterB in this.PuzzleCharacters)
            {
                if (characterA == characterB)
                {
                    continue;
                }

                if (this.AreCharactersMatching(characterA, characterB) != null)
                {
                    List<Vector2Int> connect = this.ConnectCharacters(characterA, characterB);
                    if (connect != null && connect.Count > 0)
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    /// <summary>
    /// Check whether any chars are missing from the PoemInstance
    /// </summary>
    /// <returns></returns>
    private List<string> CheckBoardMissingSourceChars()
    {
        List<string> result = new List<string>();

        foreach(string charId in this.poemInstance.GetUncoveredCharIds())
        {
            var formula = this.stageDefinition.FindFormula(charId);
            if (formula != null)
            {
                if (this.GetCharacter(formula.SourceA) == null)
                {
                    result.Add(formula.SourceA);
                }

                if (this.GetCharacter(formula.SourceB) == null)
                {
                    result.Add(formula.SourceB);
                }
            }
        }

        return result;
    }

    private bool AreCharactersMatchingWithPosisiton(Vector2 positionA, Vector2 positionB)
    {
        return false;
    }

    private FormulaDefinition AreCharactersMatching(PuzzleCharacter charA, PuzzleCharacter charB)
    {
        if (charA == null || charB == null)
        {
            return null;
        }

        var formula = this.stageDefinition.FindFormula(charA.CharacterId, charB.CharacterId);
        return formula;
    }

    private List<Vector2> ConnectCharactersWithPosisiton(Vector2 positionA, Vector2 positionB)
    {
        return null;
    }

    private List<Vector2Int> ConnectCharacters(PuzzleCharacter charA, PuzzleCharacter charB)
    {
        Debug.Log("Start ConnectCharacters.");

        var posA = charA.Position;
        var posB = charB.Position;

        List<Vector2Int> result = new List<Vector2Int>();
        result.Add(posA);
        
        // Check One Connection
        if (this.AreDirectConnectedPos(posA, posB))
        {
            result.Add(posB);
            return result;
        }
        Debug.Log("Check one connection failed.");

        // Check Two Connections
        var candidate = new Vector2Int(posA.x, posB.y);
        if (this.GetCharacterAt(candidate) == null && 
            this.AreDirectConnectedPos(posA, candidate) && this.AreDirectConnectedPos(candidate, posB))
        {
            result.Add(candidate);
            result.Add(posB);
            return result;
        }
        candidate = new Vector2Int(posB.x, posA.y);
        if (this.GetCharacterAt(candidate) == null &&
            this.AreDirectConnectedPos(posA, candidate) && this.AreDirectConnectedPos(candidate, posB))
        {
            result.Add(candidate);
            result.Add(posB);
            return result;
        }
        Debug.Log("Check two connections failed.");

        // Check Three Inner Connections
        if (posA.x != posB.x)
        {
            var deltaX = posA.x < posB.x ? 1 : -1;
            var iter = posA.x + deltaX;
            while(iter != posB.x)
            {
                var candidate1 = new Vector2Int(iter, posA.y);
                var candidate2 = new Vector2Int(iter, posB.y);
                if(this.GetCharacterAt(candidate1) == null && this.GetCharacterAt(candidate2) == null
                    && this.AreDirectConnectedPos(posA, candidate1)
                    && this.AreDirectConnectedPos(candidate1, candidate2)
                    && this.AreDirectConnectedPos(candidate2, posB)
                    )
                {
                    result.Add(candidate1);
                    result.Add(candidate2);
                    result.Add(posB);
                    return result;
                }

                iter += deltaX;
            }
        }
        if (posA.y != posB.y)
        {
            var deltaY = posA.y < posB.y ? 1 : -1;
            var iter = posA.y + deltaY;
            while (iter != posB.y)
            {
                var candidate1 = new Vector2Int(posA.x, iter);
                var candidate2 = new Vector2Int(posB.x, iter);
                if (this.GetCharacterAt(candidate1) == null && this.GetCharacterAt(candidate2) == null
                    && this.AreDirectConnectedPos(posA, candidate1)
                    && this.AreDirectConnectedPos(candidate1, candidate2)
                    && this.AreDirectConnectedPos(candidate2, posB)
                    )
                {
                    result.Add(candidate1);
                    result.Add(candidate2);
                    result.Add(posB);
                    return result;
                }

                iter += deltaY;
            }
        }
        Debug.Log("Check three inner connections failed.");

        // Check Three Outter Connection
        var minX = Math.Min(posA.x, posB.x);
        var maxX = Math.Max(posA.x, posB.x);
        var minY = Math.Min(posA.y, posB.y);
        var maxY = Math.Max(posA.y, posB.y);

        for(int i = maxX + 1; i <= this.Width + 1; i++)
        {
            var candidate1 = new Vector2Int(i, posA.y);
            var candidate2 = new Vector2Int(i, posB.y);
            if(this.GetCharacterAt(candidate1) == null && this.GetCharacterAt(candidate2) == null
                && this.AreDirectConnectedPos(posA, candidate1)
                && this.AreDirectConnectedPos(candidate1, candidate2)
                && this.AreDirectConnectedPos(candidate2, posB)
                )
            {
                result.Add(candidate1);
                result.Add(candidate2);
                result.Add(posB);
                return result;
            }
        }
        for (int i = minX - 1; i >= 0; i--)
        {
            var candidate1 = new Vector2Int(i, posA.y);
            var candidate2 = new Vector2Int(i, posB.y);
            if (this.GetCharacterAt(candidate1) == null && this.GetCharacterAt(candidate2) == null
                && this.AreDirectConnectedPos(posA, candidate1)
                && this.AreDirectConnectedPos(candidate1, candidate2)
                && this.AreDirectConnectedPos(candidate2, posB)
                )
            {
                result.Add(candidate1);
                result.Add(candidate2);
                result.Add(posB);
                return result;
            }
        }
        for (int j = maxY + 1; j <= this.Height + 1; j++)
        {
            var candidate1 = new Vector2Int(posA.x, j);
            var candidate2 = new Vector2Int(posB.x, j);
            if (this.GetCharacterAt(candidate1) == null && this.GetCharacterAt(candidate2) == null
                && this.AreDirectConnectedPos(posA, candidate1)
                && this.AreDirectConnectedPos(candidate1, candidate2)
                && this.AreDirectConnectedPos(candidate2, posB)
                )
            {
                result.Add(candidate1);
                result.Add(candidate2);
                result.Add(posB);
                return result;
            }
        }
        for (int j = minY - 1; j >= 0; j--)
        {
            var candidate1 = new Vector2Int(posA.x, j);
            var candidate2 = new Vector2Int(posB.x, j);
            if (this.GetCharacterAt(candidate1) == null && this.GetCharacterAt(candidate2) == null
                && this.AreDirectConnectedPos(posA, candidate1)
                && this.AreDirectConnectedPos(candidate1, candidate2)
                && this.AreDirectConnectedPos(candidate2, posB)
                )
            {
                result.Add(candidate1);
                result.Add(candidate2);
                result.Add(posB);
                return result;
            }
        }
        Debug.Log("Check three outer connections failed.");

        return null;
    }

    private bool AreDirectConnectedPos(Vector2Int positionA, Vector2Int positionB)
    {
        if (positionA.x == positionB.x)
        {
            var begin = Math.Min(positionA.y, positionB.y);
            var end = Math.Max(positionA.y, positionB.y);
            for(int i = begin + 1; i < end; i++)
            {
                if (this.GetCharacterAt(new Vector2Int(positionA.x, i)) != null)
                {
                    return false;
                }
            }
            return true;
        }
        else if (positionA.y == positionB.y)
        {
            var begin = Math.Min(positionA.x, positionB.x);
            var end = Math.Max(positionA.x, positionB.x);
            for (int i = begin + 1; i < end; i++)
            {
                if (this.GetCharacterAt(new Vector2Int(i, positionA.y)) != null)
                {
                    return false;
                }
            }
            return true;
        }

        return false;
    }

    private PuzzleCharacter PushCharacter(string characterId)
    {
        PuzzleCharacter character = new PuzzleCharacter(this.currentCharacterIndex++, characterId);
        this.PuzzleCharacters.Add(character);
        return character;
    }

    private void ClearCharacter(PuzzleCharacter character)
    {
        if (character != null)
        {
            this.PuzzleCharacters.Remove(character);
            Vector2Int position = character.Position;
            this.CharacterMatrix[position.x, position.y] = null;
        }
        else
        {
            Debug.LogWarning("ClearCharacter: character is null.");
        }
    }

    /// <summary>
    /// Check whether a given character exsits in the board, if Yes, return the position
    /// </summary>
    /// <param name="charId"></param>
    /// <returns></returns>
    private PuzzleCharacter GetCharacter(string charId)
    {
        foreach(var character in this.PuzzleCharacters)
        {
            if (character.CharacterId == charId)
            {
                return character;
            }
        }

        return null;
    }

    private PuzzleCharacter GetCharacterAt(Vector2Int position)
    {
        try
        {
            var character = this.CharacterMatrix[position.x, position.y];
            return character;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private Vector2Int TranslateBoardSize(PuzzleBoardSize boardSize)
    {
        switch (boardSize)
        {
            case PuzzleBoardSize.TINY:
                {
                    return new Vector2Int(5, 6);
                }
            case PuzzleBoardSize.SMALL:
                {
                    return new Vector2Int(6, 8);
                }
            case PuzzleBoardSize.MEDIUM:
                {
                    return new Vector2Int(7, 10);
                }
            case PuzzleBoardSize.LARGE:
                {
                    return new Vector2Int(8, 12);
                }
            case PuzzleBoardSize.HUGE:
                {
                    return new Vector2Int(9, 14);
                }
            default:
                {
                    return new Vector2Int(6, 8);
                }
        }
    }

}
