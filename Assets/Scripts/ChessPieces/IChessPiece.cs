using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Implemented by Chess Pieces so that GameManager can interact
 * */
public interface IChessPiece {
    /*
     * What side this piece belongs to
     */ 
    Affiliation Team { get; }

    /*
     * The GameObject this piece is attached to
     */ 
    GameObject gameObject { get; }

    /* 
     * Returns all valid squares where this chess piece can move
     */ 
    List<int> AvailableMoves(List<GameObject> board, int currentPos);

    /* 
     * Notifies this chess piece that it has moved to a new position
     */ 
    List<ChessCommand> Moved(List<GameObject> board, int from, int to);

    /* 
     * Notifies this chess piece that it has been deleted
     */ 
    void OnDeath();


}

public enum Affiliation 
{
    White,
    Black
}
