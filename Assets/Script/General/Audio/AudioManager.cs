// reference: https://www.youtube.com/watch?v=6OT43pvUyfY

using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : SingletonWMonoBehaviour<AudioManager>
{
	public static AudioManager Instance { get; private set; }

	public AudioMixerGroup mixerGroup;

	public Sound[] sounds;

	public float totalVolume = 1;

	void Awake()
	{
		Instance = CreateInstance(Instance, true);

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;

			s.source.outputAudioMixerGroup = mixerGroup;
		}
	}

	public void Play(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		//Debug.Log("Playing " + sound + ".");

		s.source.volume = totalVolume * s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Play();
	}

}
