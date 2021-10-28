using System.Collections;

[System.Serializable]
public class Part {
    public string id;
    public PartType partType;
    //value of mass should be between 0-1
    public float mass;
}