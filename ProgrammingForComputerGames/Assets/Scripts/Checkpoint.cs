using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Car"))
        {
            GetComponentInParent<RoadMaker>().checkpoints++;
            GetComponentInParent<RoadMaker>().SpawnCheckPoint();
            Destroy(gameObject);
        }
    }
}
