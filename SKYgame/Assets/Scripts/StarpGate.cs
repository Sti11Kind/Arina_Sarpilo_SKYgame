using UnityEngine;
using static GameManager;

public class StarpGate : MonoBehaviour
{
    public static TimerEvent StartRace;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            StartRace.Invoke();
        }
    }
}
