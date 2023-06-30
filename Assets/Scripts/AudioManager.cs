using UnityEngine.Audio;
using System;
using UnityEngine;
using Unity.VisualScripting;
using System.Collections;

public class AudioManager : MonoBehaviour
{

    public float fadeTime = 1.0f;
    private bool fading = false;
    public static AudioManager instance;

	public AudioMixerGroup mixerGroup;

	public Sound[] sounds;

	void Awake()
	{
        

		if (instance == null)
		{
            instance = this;
            
		}
		else
		{
            Destroy(gameObject);
            return;
		}
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
            s.source.volume = s.volume;
            s.source.clip = s.clip;
			s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;
            
			s.source.outputAudioMixerGroup = s.mixerGroup;
		}

        Play("Intro");
        Play("Wind");
    }

	public void Play(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		//s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		//s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Play();
	}

	public void Fade(string sound1, string sound2)
	{

        Sound running = Array.Find(sounds, item => item.name == sound1);
        Sound target = Array.Find(sounds, item => item.name == sound2);

        if (!fading)
        {
            StartCoroutine(FadeTrack(running,target));
        }
    }



    private IEnumerator FadeTrack(Sound running, Sound target)
    {
        fading = true;
        float t = 0.0f;
        if(running != target) { 
        while (running.source.volume > 0)
        {
            
            t += Time.deltaTime;
            float volume1 = Mathf.Lerp(1.0f, 0.0f, t / fadeTime);
          
            running.source.volume = volume1;
          
            yield return null;
        }

        running.source.Stop();
        }
        target.source.Play();

        while (target.source.volume < 1)
        {

            t += Time.deltaTime;
            
            float volume2 = Mathf.Lerp(0.0f, 1.0f, t / fadeTime);

            target.source.volume = volume2;
            yield return null;
        }
        fading = false;
    }

    public void Stop(String song)
    {
        Sound songToStop = Array.Find(sounds, item => item.name == song);
        foreach (Sound sound in sounds)
        {
            if (sound.source.isPlaying)
            {

                songToStop.source.Stop();
            }

        }
    }

        public void StopRunning()
	{
        foreach (Sound sound in sounds)
        {
            if (sound.source.isPlaying)
            {


                sound.source.Stop();
            }

        }
        
	}


}
