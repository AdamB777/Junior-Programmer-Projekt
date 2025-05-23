using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public MainManager Manager;

  private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Ball"))
    {
        Manager.BallLost();
    }
}

}
