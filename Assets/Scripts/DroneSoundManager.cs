using System.Collections;
using UnityEngine;

public class DroneSoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource flightSound;

    [Header("PWM порог")]
    [SerializeField] private float flightThreshold = 1350f;

    [Header("Настройки звука")]
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float maxVolume = 1f;


    private bool isFlying = false;
    private float[] pwms;
    private Coroutine currentFadeCoroutine;

    private void Update()
    {
        if (pwms == null || pwms.Length == 0) return;

        float avgPWM = 0f;
        foreach (float pwm in pwms)
            avgPWM += pwm;
        avgPWM /= pwms.Length;

        if (!isFlying && avgPWM > flightThreshold)
        {
            StartFlightSound();
            isFlying = true;
        }
        else if (isFlying && avgPWM <= flightThreshold)
        {
            StopFlightSound();
            isFlying = false;
        }

        pwms = null;
    }

    public void SetPWM(float[] pwms)
    {
        this.pwms = pwms;
    }

    private void StartFlightSound()
    {
        if (flightSound == null) return;

        flightSound.loop = true;
        flightSound.time = 0f;

        if (!flightSound.isPlaying)
            flightSound.Play();

        StartFade(flightSound, maxVolume);
    }

    private void StopFlightSound()
    {
        if (flightSound == null) return;

        StartFade(flightSound, 0f, stopAfter: true);
    }

    private void StartFade(AudioSource source, float targetVolume, bool stopAfter = false)
    {
        if (currentFadeCoroutine != null)
            StopCoroutine(currentFadeCoroutine);

        currentFadeCoroutine = StartCoroutine(FadeAudio(source, targetVolume, stopAfter));
    }

    private IEnumerator FadeAudio(AudioSource source, float targetVolume, bool stopAfter)
    {
        float startVolume = source.volume;
        float timeElapsed = 0f;

        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, targetVolume, timeElapsed / fadeDuration);
            yield return null;
        }

        source.volume = targetVolume;

        if (stopAfter && targetVolume == 0f)
            source.Stop();
    }
}