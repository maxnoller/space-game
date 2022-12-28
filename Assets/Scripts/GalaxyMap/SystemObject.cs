using System.Collections;
using System.Collections.Generic;

namespace SpaceGame.GalaxyMap{
[System.Serializable]
public class SystemObject
{
    public int id { get; set; }
    public string name { get; set; }
    public double x { get; set; }
    public double y { get; set; }
    public int? current_owner { get; set; }

    public override string ToString()
    {
        return "System{" +
            "id=" + id +
            ", name='" + name + '\'' +
            ", x=" + x +
            ", y=" + y +
            ", current_owner=" + current_owner +
            '}';
    }
}
}
