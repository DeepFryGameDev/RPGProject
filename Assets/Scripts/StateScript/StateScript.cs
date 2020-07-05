using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum battleStates //phases of battle
{
    WAIT,
    TAKEACTION,
    PERFORMACTION,
    CHECKALIVE,
    WIN,
    LOSE
}

public enum HeroGUI //phases of a hero selecting input
{
    ACTIVATE,
    WAITING,
    DONE
}

public enum GameStates
{
    HOSTILE_STATE,
    PEACEFUL_STATE,
    BATTLE_STATE,
    IDLE
}

public enum BehaviorStates
{
    IDLE,
    CHOOSEACTION,
    BEFOREMOVE,
    MOVE,
    AFTERMOVE,
    ACTION
}

public enum Types
{
    HERO,
    ENEMY
}

public enum ActionType
{
    ATTACK,
    MAGIC,
    ITEM
}

public enum FaceState
{
    UP,
    DOWN,
    LEFT,
    RIGHT,
    DEFAULT
}
