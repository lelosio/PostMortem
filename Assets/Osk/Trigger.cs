using UnityEngine;
using System.Collections.Generic;

public class Trigger : MonoBehaviour
{
    public List<AiController> enemiesToFollow;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (AiController enemy in enemiesToFollow)
            {
                enemy.StartChasing(other.transform.position);
            }
        }
    }
}
