using UnityEngine;
using UnityEngine.AI;

public class ScareCrowIA : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;
    private PumpkinController pumpkinController;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        pumpkinController = GetComponent<PumpkinController>();
    }

    void Update()
    {
        if (target != null)
        {
            if(pumpkinController.hit)
            {
                agent.isStopped = true;
            }
            else
            {
                agent.isStopped = false;
                agent.SetDestination(target.position);
            }
            
        }
    }
}