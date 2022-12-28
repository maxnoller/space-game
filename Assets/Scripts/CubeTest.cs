using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceGame.Infrastructure;

public class CubeTest : MonoBehaviour, IDamageable
{
    public void damage(){
        Debug.Log("shit got hit");
    }
}
