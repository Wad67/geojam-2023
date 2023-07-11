using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
  public IEnumerator Interact(Transform initiator)
    {
        Debug.Log("Pickup is working");
        yield break;
    }
}
