using UnityEngine;

public class ArmaAudio : MonoBehaviour
{
    public AudioClip somRecarregar;

    // o mp3 tem várias recargas; a primeira vai de 0s até ~0.78s
    public float inicioRecarga = 0f;
    public float duracaoRecarga = 0.85f;

    AudioSource source;

    void Awake()
    {
        // AudioSource preso na arma, assim o som acompanha o player
        source = GetComponent<AudioSource>();
        if (source == null) source = gameObject.AddComponent<AudioSource>();

        source.playOnAwake = false;
        source.loop = false;
    }

    // chamada pelo Animation Event do clipe Shotgun|ShotgunAction (nome precisa ser igual ao do evento)
    public void TocarRecarrega()
    {
        source.Stop();
        source.clip = somRecarregar;
        source.time = inicioRecarga;
        source.Play();
        source.SetScheduledEndTime(AudioSettings.dspTime + duracaoRecarga);   // corta antes da segunda recarga
    }
}
