using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

namespace SpaceGame.GalaxyMap{
public class APIManager : MonoBehaviour
{
    [SerializeField]
    public string baseUrl = "http://localhost:8000";

    public string auth_token = "0c481fe891e4fcc6c098beeeba120850c582ce9f";
    


}
}