using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RepController : MonoBehaviour
{
    public Vector3 start_pos;

    void OnEnable()
    {
        start_pos = transform.position;
    }

    public void RepReady()
    {
        GetComponent<NavMeshAgent>().SetDestination(start_pos);
    }
}
