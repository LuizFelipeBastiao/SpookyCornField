using UnityEngine;
using UnityEngine.AI;

public class ScareCrowIA : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;
    private PumpkinController pumpkinController;

    [Header("Corrida")]
    public float velocidadeCorrida = 12f;
    public float aceleracaoCorrida = 40f;          // alta pra arrancada ser repentina
    public Vector2 intervaloEntreCorridas = new Vector2(4f, 10f);   // min/max em segundos andando
    public Vector2 duracaoCorrida = new Vector2(1f, 3f);            // min/max em segundos correndo

    private float velocidadeAndar;
    private float aceleracaoAndar;
    private bool correndo = false;
    private float timerCorrida;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        pumpkinController = GetComponent<PumpkinController>();

        // valores configurados no NavMeshAgent viram a velocidade de andar
        velocidadeAndar = agent.speed;
        aceleracaoAndar = agent.acceleration;

        timerCorrida = Random.Range(intervaloEntreCorridas.x, intervaloEntreCorridas.y);
    }

    void Update()
    {
        if (target != null)
        {
            if(pumpkinController.hit)
            {
                agent.isStopped = true;
                if (correndo) PararCorrida();   // tomou tiro ou está atacando: cancela a corrida
            }
            else
            {
                agent.isStopped = false;
                agent.SetDestination(target.position);
                AtualizarCorrida();
            }

        }
    }

    private void AtualizarCorrida()
    {
        timerCorrida -= Time.deltaTime;
        if (timerCorrida > 0f) return;

        if (correndo) PararCorrida();
        else ComecarCorrida();
    }

    private void ComecarCorrida()
    {
        correndo = true;
        agent.speed = velocidadeCorrida;
        agent.acceleration = aceleracaoCorrida;
        pumpkinController.animator.SetBool("isRunning", true);   // Andar -> Correr (clipe run)
        timerCorrida = Random.Range(duracaoCorrida.x, duracaoCorrida.y);
    }

    private void PararCorrida()
    {
        correndo = false;
        agent.speed = velocidadeAndar;
        agent.acceleration = aceleracaoAndar;
        pumpkinController.animator.SetBool("isRunning", false);
        timerCorrida = Random.Range(intervaloEntreCorridas.x, intervaloEntreCorridas.y);
    }
}
