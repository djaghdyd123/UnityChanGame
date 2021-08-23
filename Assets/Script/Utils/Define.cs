﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum WorldObject
    {
        Unknown,
        Monster,
        Player,
        Env
    }
    public enum State
    {
        Die,
        Idle,
        Run,
        Run_B,
        Run_R,
        Run_L,
        Run_FL,
        Run_FR,
        Attack,
        Jump
    }
    public enum layer
    {
        Monster = 8,
        Ground = 9,
        Block = 10
    }
    public enum Scene
    {
        Unknown,
        Login,
        Lobby,
        Game,
        Game2,
    }
    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }

    public enum UIEvent
    {
        Clicked,
        Drag,
    }
    public enum MousEvent
    {
        PointerDown,
        PointerUp,
        Clicked,
        Pressed,
        PointerDownRight,
        PointerUpRight,
        PressedRight,
    }

    public enum CameraMode
    {
        QuarterView,
        ShoulderView,
        ShoulderView2,

    }

    public enum GameMode
    {
        QuaterView,
        ShoudlerView,
        
    }
}
