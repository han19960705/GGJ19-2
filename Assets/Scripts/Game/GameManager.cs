﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EGameState
{
    LOBBY,
    GAMING,
    GAMEOVER,
    WINNER
}

public class GameManager : MonoBehaviour
{
    public static GameManager Ins { get; private set; }

    EGameState _state = EGameState.GAMING;
    public EGameState state
    {
        get
        {
            return _state;
        }
        set
        {
            this._state = value;
            //TODO:
            switch (this._state)
            {
                case EGameState.LOBBY:
                    break;
                case EGameState.GAMING:
                    break;
                case EGameState.GAMEOVER:
                    CanvasManager.Ins.ShowGameOver();
                    break;
                case EGameState.WINNER:
                    break;
            }
        }
    }

    private void Awake()
    {
        if (Ins != null)
        {
            Destroy(this);
        }

        Ins = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool CheckIsGameOver()
    {

        return false;
    }
}