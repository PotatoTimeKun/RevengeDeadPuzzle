using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class BGMData
{
    public Audio_Data.BGMType type;
    public AudioClip clip;
}

[Serializable]
public class SEData
{
    public Audio_Data.SEType type;
    public AudioClip clip;
}

public class AudioController : MonoBehaviour
{
    public static AudioController Instance { get; private set; }
    public float BGMVolume = 1f;
    public float SEVolume = 1f;
    public AudioSource bgmAudioSource;
    public AudioSource seAudioSource;
    public List<BGMData> bgmDataList;
    public List<SEData> seDataList;
    
    private Dictionary<Audio_Data.BGMType, AudioClip> _bgmDic;
    private Dictionary<Audio_Data.SEType, AudioClip> _seDic;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        Initialize();
    }

    private void Initialize()
    {
        _bgmDic = new();
        foreach (var data in bgmDataList)
        {
            if (!_bgmDic.ContainsKey(data.type))
            {
                Debug.LogWarning("BGMタイプが重複しているものがあります！確認してください");
                continue;
            }
            _bgmDic.Add(data.type, data.clip);
        }
        _seDic = new();
        foreach (var data in seDataList)
        {
            if (!_seDic.ContainsKey(data.type))
            {
                Debug.LogWarning("SEタイプが重複しているものがあります！確認してください");
                continue;
            }
            _seDic.Add(data.type, data.clip);
        }
    }

    public void PlayBGM(Audio_Data.BGMType type)
    {
        if (bgmAudioSource == null) return;
        if (_bgmDic.ContainsKey(type)) return;
        AudioClip clip = _bgmDic[type];
        bgmAudioSource.clip = clip;
        bgmAudioSource.volume = BGMVolume;
        bgmAudioSource.Play();
    }

    public void StopBGM()
    {
        if (bgmAudioSource == null) return;
        bgmAudioSource.Stop();
    }

    public void PlaySE(Audio_Data.SEType type)
    {
        if (seAudioSource == null) return;
        if (_seDic.ContainsKey(type)) return;
        AudioClip clip = _seDic[type];
        seAudioSource.PlayOneShot(clip, SEVolume);
    }
}
