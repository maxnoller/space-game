using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class HyperspaceLane {
    [SerializeField]
    public GameObject planet_a;
    [SerializeField]
    public GameObject planet_b;
}

public class CreateHyperspaceLanes : MonoBehaviour
{
    [SerializeField]
    GameObject hyperspace_lane;
    [SerializeField]
    HyperspaceLane[] lanes;
    void Start()
    {
        foreach(HyperspaceLane lane in lanes){
            GameObject lane_object = Instantiate(hyperspace_lane, new Vector3(0,0,0), Quaternion.identity);
            Vector3[] positions = {lane.planet_a.transform.position, lane.planet_b.transform.position};
            lane_object.GetComponent<LineRenderer>().SetPositions(positions);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
