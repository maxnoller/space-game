using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatePlanets : MonoBehaviour
{
    [SerializeField]
    private PlanetScriptableObject[] planetObjects;
    [SerializeField]
    private GameObject planetPrefab;
    [SerializeField]
    private GameObject planetUI;
    [SerializeField]
    Camera cam;
    [SerializeField]
    private GameObject canvas;

    private void Start(){
        create_planets();
    }

    private void create_planets(){
        foreach(PlanetScriptableObject planet_object in planetObjects){
            GameObject planet = Instantiate(planetPrefab, planet_object.coordinates, Quaternion.identity);
            planet.name = planet_object.planetName;
            Vector3 screen_pos = cam.WorldToScreenPoint(planet.transform.position);
            GameObject planet_ui_go = Instantiate(planetUI, screen_pos, Quaternion.identity, canvas.transform);
            planet_ui_go.transform.GetChild(0).GetComponent<Text>().text = planet_object.name;


        }
    }
}
