using System.Collections.Generic;

[System.Serializable]
public class CameraPoint
{
    public string pointId;       // code key: "kitchen_01"
    public string pointName;     // display label: "Kitchen"
    public List<string> neighbors = new();
}
