using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is responsible for manipulating the GameLifecycle
public class GameController : MonoBehaviour
{
    public List<Player> players;
    public List<GameObject> playerSpawnPoints;

    void Start(){
        //parse the code
        if(players.Count != playerSpawnPoints.Count){
            Debug.Log("Inconsistent input");
            return;
        }
        foreach(var player in players){
            if(player.type == Player.P_AI){
                Helper.Randomize(player);
                Debug.Log("Player vs AI");
            }
        }
    }

    public void OnPlayerFall(Player p1){

    }

    public void OnBothFall(){

    }
}
