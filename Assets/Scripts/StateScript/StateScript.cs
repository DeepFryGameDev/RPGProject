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
