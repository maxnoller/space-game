using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
public class NetworkManagerGui : MonoBehaviour
{
    [SerializeField]
    private GameObject buttonCanvas;
    [SerializeField]
    private GameObject inputCanvas;
    [SerializeField]
    private Text ip;
    [SerializeField]
    private Text port;

    public void join(){
        if(ip.text == ""){
            ip.text = "127.0.0.1";
        }
        if(port.text == ""){
            port.text = "7777";
        }
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(
            ip.text,
            ushort.Parse(port.text)
        );
        NetworkManager.Singleton.StartClient();
        NetworkManager.Singleton.SceneManager.LoadScene("space_battle", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    public void setupJoin(){
        buttonCanvas.SetActive(false);
        inputCanvas.SetActive(true);
    }

    public void host(){
        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene("space_battle", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
