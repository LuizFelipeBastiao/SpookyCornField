using System.Collections;
using UnityEngine;

// toca o som de passos enquanto o objeto se move (serve pro player e pros inimigos)
// - modo loop: toca o clipe em loop enquanto anda (bom pro player, que não tem pés visíveis)
// - modo eventos: cada Animation Event "Passo" nos clipes de andar/correr toca um passo (sincronizado com os pés)
public class Passos : MonoBehaviour
{
    public AudioClip somPassos;
    [Range(0f, 1f)] public float volume = 0.5f;
    [Range(0.1f, 3f)] public float pitch = 1f;            // < 1 deixa mais grave
    [Range(0f, 1f)] public float spatialBlend = 0f;       // 0 = 2D (player), 1 = 3D (inimigo)
    public float distanciaMaxima = 30f;                   // só vale no 3D: a partir daqui não se ouve mais

    [Header("Velocidade (modo loop)")]
    public float velocidadeMinima = 0.5f;                 // abaixo disso considera parado
    public float velocidadeReferencia = 5f;               // velocidade em que toca com o pitch normal
    public float multiplicadorMaximo = 1.3f;              // correndo, os passos aceleram até esse limite

    [Header("Animation Events (modo eventos)")]
    public bool usarEventosDeAnimacao = false;
    public AudioClip somPassoUnico;                       // opcional: um passo isolado; se vazio, usa um trecho do somPassos
    public float duracaoPasso = 0.3f;                     // tamanho do trecho do somPassos usado como um passo
    public float fadePasso = 0.08f;                       // fade no fim do trecho, pra não dar estalo
    // onde cada pisada começa no PASSOS.mp3 (o resto do arquivo é só chiado entre os passos)
    public float[] iniciosPassos = { 0.05f, 0.59f, 1.37f, 1.98f, 2.59f, 3.23f, 3.74f };
    public float intervaloMinimo = 0.15f;                 // ignora eventos duplicados (ex: blend entre walk e run)

    AudioSource source;
    Vector3 ultimaPos;
    bool pausado = false;
    Coroutine fadeAtual;
    float ultimoPasso = -1f;
    int ultimoIndice = -1;

    void Awake()
    {
        // AudioSource próprio, pra não conflitar com outros sons do objeto
        source = gameObject.AddComponent<AudioSource>();
        source.clip = somPassos;
        source.loop = !usarEventosDeAnimacao;
        source.playOnAwake = false;
        source.volume = volume;
        source.spatialBlend = spatialBlend;
        source.rolloffMode = AudioRolloffMode.Linear;
        source.minDistance = 2f;
        source.maxDistance = distanciaMaxima;

        ultimaPos = transform.position;
    }

    void Update()
    {
        if (somPassos == null || usarEventosDeAnimacao) return;

        // jogo pausado (Time.timeScale = 0)
        if (Time.deltaTime <= 0f)
        {
            Pausar();
            return;
        }

        Vector3 delta = transform.position - ultimaPos;
        delta.y = 0f;   // ignora gravidade/queda
        ultimaPos = transform.position;

        float velocidade = delta.magnitude / Time.deltaTime;

        if (velocidade > velocidadeMinima)
        {
            source.pitch = pitch * Mathf.Clamp(velocidade / velocidadeReferencia, 1f, multiplicadorMaximo);
            Tocar();
        }
        else
        {
            Pausar();
        }
    }

    // chamada pelo Animation Event "Passo" nos clipes walk/run
    public void Passo()
    {
        if (!usarEventosDeAnimacao) return;

        // durante a transição walk <-> run os dois clipes disparam evento quase juntos
        if (Time.time - ultimoPasso < intervaloMinimo) return;
        ultimoPasso = Time.time;

        float pitchPasso = pitch * Random.Range(0.93f, 1.07f);   // variação pra não soar repetido

        if (somPassoUnico != null)
        {
            source.pitch = pitchPasso;
            source.PlayOneShot(somPassoUnico, volume);
            return;
        }

        if (somPassos == null) return;

        // recorta uma das pisadas do loop de passos
        if (fadeAtual != null) StopCoroutine(fadeAtual);
        source.Stop();
        source.pitch = pitchPasso;
        source.volume = volume;
        source.time = SortearInicioPasso();
        source.Play();
        fadeAtual = StartCoroutine(FadeOutPasso());
    }

    private float SortearInicioPasso()
    {
        if (iniciosPassos == null || iniciosPassos.Length == 0)
            return Random.Range(0f, Mathf.Max(0f, somPassos.length - duracaoPasso * 2f));

        // não repete a mesma pisada duas vezes seguidas
        int indice = Random.Range(0, iniciosPassos.Length);
        if (iniciosPassos.Length > 1 && indice == ultimoIndice)
            indice = (indice + 1) % iniciosPassos.Length;
        ultimoIndice = indice;

        return Mathf.Clamp(iniciosPassos[indice], 0f, somPassos.length - 0.01f);
    }

    private IEnumerator FadeOutPasso()
    {
        // tempo real do trecho (pitch menor = toca mais devagar)
        float duracaoReal = duracaoPasso / source.pitch;
        // tempo real: o corte acontece mesmo com Time.timeScale = 0 (tela de morte)
        yield return new WaitForSecondsRealtime(Mathf.Max(0f, duracaoReal - fadePasso));

        float t = 0f;
        while (t < fadePasso)
        {
            t += Time.unscaledDeltaTime;
            source.volume = Mathf.Lerp(volume, 0f, t / fadePasso);
            yield return null;
        }

        source.Stop();
        source.volume = volume;
        fadeAtual = null;
    }

    private void Tocar()
    {
        if (pausado)
        {
            source.UnPause();   // continua de onde parou, pra não repetir sempre o mesmo começo
            pausado = false;
        }
        else if (!source.isPlaying)
        {
            source.time = Random.Range(0f, somPassos.length);
            source.Play();
        }
    }

    private void Pausar()
    {
        if (source.isPlaying)
        {
            source.Pause();
            pausado = true;
        }
    }
}
