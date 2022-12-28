using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Newtonsoft.Json;
using UnityEngine.Networking;

namespace SpaceGame.GalaxyMap{
public class GalaxyMapApplication : NetworkBehaviour
{
    APIManager apiManager;

    [SerializeField]
    List<SystemObject> systems;
    [SerializeField]
    GameObject systemPrefab;
    void Awake(){
        apiManager = gameObject.AddComponent<APIManager>();
        GenerateGalaxyMap();
    }

    void GenerateGalaxyMap(){
        StartCoroutine(GetGalaxyMap((systems) => this.systems=systems));
    }

    void CreateSystem(SystemObject system){
        GameObject systemObject = Instantiate(systemPrefab, new Vector3((float)system.x, 0, (float)system.y), Quaternion.identity);
        systemObject.name = systemObject.name;
    }

    public IEnumerator GetGalaxyMap(System.Action<List<SystemObject>> callback){
        UnityWebRequest www = UnityWebRequest.Get(apiManager.baseUrl + "/api/galaxy_map/systems/");
        www.SetRequestHeader("Authorization", "Token " + apiManager.auth_token);
        yield return www.SendWebRequest();
        if(www.result != UnityWebRequest.Result.Success){
            Debug.Log(www.error);
        } else {
            callback(JsonConvert.DeserializeObject<List<SystemObject>>(www.downloadHandler.text));
            foreach(SystemObject s in systems){
                CreateSystem(s);
            }
        }
    }
}
}