using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEngine
{
    public Player p1;
    public Player p2;
    private static GameEngine instance = null;
    private GameEngine()
    {
    }

    public static GameEngine GetInstance()
    {
        if(instance == null){
            instance = new GameEngine();
        }
        return instance;
    }
}
