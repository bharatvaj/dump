[System.Serializable]
public class Config {
    public string id;
    //name of the configuration
    public string name;
    //description of the configuration
    public string desc;
    /*
        [0] SNIB
        [1] BARREL
        [2] CAP
     */
    public Part[] parts;
}