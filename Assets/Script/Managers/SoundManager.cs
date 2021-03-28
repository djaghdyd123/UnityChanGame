using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.MaxCount];

    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    // 끝까지 들고갈 AudioSource를 게임오브젝트에 생성하여 넣고, 위에 배열로 들고 있는 작업. 
    public void Init()
    {
        GameObject root = GameObject.Find("@Sound");
        if (root == null)
            root = new GameObject { name = "@Sound" };
        Object.DontDestroyOnLoad(root);

        string[] Soundnames = System.Enum.GetNames(typeof(Define.Sound));
        for(int i = 0; i < Soundnames.Length -1; i++)
        {
            GameObject go = new GameObject { name = Soundnames[i] };
            _audioSources[i] = go.AddComponent<AudioSource>();
            go.transform.parent = root.transform;
        }

        _audioSources[(int)Define.Sound.Bgm].loop = true;
         
    }

    // Path로 AudiClip을 Load, 성격에 맞는 AudioSource에 Play 

    public void Play(string path, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        AudioClip audioClip = GetOrAddAudioClip(path);
        Play(audioClip, type, pitch);

    }

    public void Play(AudioClip audioClip, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        if (audioClip == null)
            return;
        
        if (type == Define.Sound.Bgm)
        {
           
            AudioSource audioSource = _audioSources[(int)Define.Sound.Bgm];
            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else
        {
            
            AudioSource audioSource = _audioSources[(int)Define.Sound.Effect];
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }
    }

    // AudioClip을 바로 Load 하지 않고 일단 찾는 메소드

    AudioClip GetOrAddAudioClip(string path, Define.Sound type = Define.Sound.Effect)
    {
        if (path.Contains("Sounds/") == false)
            path = $"Sounds/{path}";

        AudioClip audioClip = null;

        if (type == Define.Sound.Bgm)
        {
            audioClip = Managers.Resources.Load<AudioClip>(path);
            if (audioClip == null)
            {
                Debug.Log($"Audio Clip Missing! {path}");
            }
        }
        else
        {
           
            if (_audioClips.TryGetValue(path, out audioClip) == false)
            {
                audioClip = Managers.Resources.Load<AudioClip>(path);
                _audioClips.Add(path, audioClip);
            }

            if (audioClip == null)
            {
                Debug.Log($"Audio Clip Missing! {path}");
            } 
        }
        return audioClip;
    }

    // Dictionary 와 Source Clear
    public void Clear()
    {
        foreach(AudioSource audiosource in _audioSources)
        {
            audiosource.clip = null;
            audiosource.Stop();

        }
        _audioClips.Clear();
    }
}
