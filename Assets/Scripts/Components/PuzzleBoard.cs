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

    public Vector2 Position
    {
        get; set;
    }

    public PuzzleCharacter(int index, string charId)
    {
        this.Index = index;
        this.CharacterId = charId;
        this.Position = new Vector2(-1, -1);
    }

}

public interface PuzzleBoardHandler
{

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

    public Vector2 LastSelectedPosition
    {
        get; set;
    }

    public List<PuzzleCharacter> PuzzleCharacters
    {
        get; private set;
    }

    public PuzzleCharacter[][] CharacterMatrix
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

        GenerateAndEnsureMatrix(this.PuzzleCharacters);
    }

    public void TakeActionAt(Vector2 position)
    {

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

        result.AddRange(this.poemInstance.GetCoveredCharIds());
        result.AddRange(this.puzzleDefinition.NoiseCharIds);

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

    }

    private void PlaceCharacterInMatrix(PuzzleCharacter puzzleChar)
    {

    }

    private bool IsMatrixDeadlock()
    {
        return false;
    }

    /// <summary>
    /// Check whether any chars are missing from the PoemInstance
    /// </summary>
    /// <returns></returns>
    private List<string> CheckBoardMissingSourceChars()
    {
        return null;
    }

    private bool AreCharactersMatching(Vector2 positionA, Vector2 positionB)
    {
        return false;
    }

    private List<Vector2> ConnectCharacters(Vector2 positionA, Vector2 positionB)
    {
        return null;
    }

    private bool AreDirectConnected(Vector2 positionA, Vector2 positionB)
    {
        return false;
    }

    private PuzzleCharacter PushCharacter(string characterId)
    {
        PuzzleCharacter character = new PuzzleCharacter(this.currentCharacterIndex++, characterId);
        this.PuzzleCharacters.Add(character);
        return character;
    }

    private void ClearCharacterAt(Vector2 position)
    {

    }

    /// <summary>
    /// Check whether a given character exsits in the board, if Yes, return the position
    /// </summary>
    /// <param name="charId"></param>
    /// <returns></returns>
    private Vector2 GetCharacter(string charId)
    {
        return Vector2.zero;
    }

    private PuzzleCharacter GetCharacterAt(Vector2 position)
    {
        return null;
    }

    private Vector2 TranslateBoardSize(PuzzleBoardSize boardSize)
    {
        switch (boardSize)
        {
            case PuzzleBoardSize.TINY:
                {
                    return new Vector2(5, 6);
                }
            case PuzzleBoardSize.SMALL:
                {
                    return new Vector2(6, 8);
                }
            case PuzzleBoardSize.MEDIUM:
                {
                    return new Vector2(8, 10);
                }
            case PuzzleBoardSize.LARGE:
                {
                    return new Vector2(9, 12);
                }
            case PuzzleBoardSize.HUGE:
                {
                    return new Vector2(10, 14);
                }
            default:
                {
                    return new Vector2(6, 8);
                }
        }
    }

}
