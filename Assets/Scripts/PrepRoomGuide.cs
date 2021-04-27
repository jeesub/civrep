using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class PrepRoomDestination
{
    public string name;
    public Transform transform;
}

public class PrepRoomGuide : MonoBehaviour
{
    public List<GameObject> reps;
    public List<PrepRoomDestination> destinationLists;

    private Dictionary<string, Transform> destinations = new Dictionary<string, Transform>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (PrepRoomDestination x in destinationLists)
        {
            destinations.Add(x.name, x.transform);
        }
    }

    public void GuidePrep(int repIdx, string dest)
    {
        if (dest.Equals("rest"))
        {
            if (!reps[repIdx].activeSelf)
            {
                reps[repIdx].SetActive(true);
            }
            Debug.Log("rep is ready!");
            reps[repIdx].GetComponent<RepController>().RepReady();
        }
        else
        {
            Transform destination = destinations[dest];
            if (destination != null)
            {
                if (!reps[repIdx].activeSelf)
                {
                    reps[repIdx].SetActive(true);
                }
                Debug.Log("destination is: " + dest);
                reps[repIdx].GetComponent<RepController>().GoToDestination(destination);
            }
            else
            {
                Debug.Log("Destination is null: " + dest);
            }
        }        
    }
}
