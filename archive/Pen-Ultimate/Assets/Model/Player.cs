using System.Collections;

[System.Serializable]
public class Player {
    public string id;
    public PlayerType type;
    //parts owned by the player
    public Part parts;
    //configs owned by the player
    public Config configs;
}