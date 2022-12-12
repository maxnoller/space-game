using UnityEngine;


[CreateAssetMenu(fileName = "Planet", menuName = "GameData/PlanetScritableObject", order = 1)]
public class PlanetScriptableObject : ScriptableObject
{
    public string planetName;
    public Vector3 coordinates;
    public Material material;
}
