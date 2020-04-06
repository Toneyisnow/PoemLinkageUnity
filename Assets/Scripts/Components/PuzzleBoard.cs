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


public class PuzzleBoard
{
    private StageDefinition stageDefinition = null;

    private PoemDefinition poemDefinition = null;

    private PuzzleDefinition puzzleDefinition = null;


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

    
    public PuzzleBoard(StageDefinition stageDefinition)
    {

    }

    public void TakeActionAt(Vector2 position)
    {

    }

    public void GenerateAndEnsureMatrix(List<PuzzleCharacter> puzzleChars)
    {

    }

    public void CheckShuffle()
    {

    }


    private List<string> ComposeTargetCharIds()
    {
        return null;
    }

    private List<string> GenerateAppearingCharIds(List<string> targetCharIds)
    {
        return null;
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
    private bool CheckBoardMissingChar()
    {
        return false;
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
