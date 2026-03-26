using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;

#if UNITY_EDITOR

// インスペクター表示にエラーが出るので旧方式に変更

using UnityEditor;
[CustomEditor(typeof(AudioController))]
public class AudioControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // UI Toolkit (新方式) を使わず、IMGUI (旧方式) で描画する
        serializedObject.Update();

        // 従来の「標準的な見た目」でリストを描画
        DrawDefaultInspector();

        serializedObject.ApplyModifiedProperties();
    }
}
#endif

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
            if (_bgmDic.ContainsKey(data.type))
            {
                Debug.LogWarning("BGMタイプが重複しているものがあります！確認してください");
                continue;
            }
            _bgmDic.Add(data.type, data.clip);
        }
        _seDic = new();
        foreach (var data in seDataList)
        {
            if (_seDic.ContainsKey(data.type))
            {
                Debug.LogWarning("SEタイプが重複しているものがあります！確認してください");
                continue;
            }
            _seDic.Add(data.type, data.clip);
        }
    }

    private void SetVolume(float volume, float bgmVolume, float seVolume)
    {
        if (bgmAudioSource != null) bgmAudioSource.volume = volume * bgmVolume;
        if (seAudioSource != null) seAudioSource.volume = volume * seVolume;
    }

    void Update(){
        float masterVolume = SaveDataStore.Instance.CurrentData.Setting.MasterVolume;
        float bgmVolume = SaveDataStore.Instance.CurrentData.Setting.BgmVolume;
        float seVolume = SaveDataStore.Instance.CurrentData.Setting.SeVolume;
        SetVolume(masterVolume, bgmVolume, seVolume);
    }

    public void PlayBGM(Audio_Data.BGMType type)
    {
        if (bgmAudioSource == null) return;
        if (!_bgmDic.ContainsKey(type)) return;
        AudioClip clip = _bgmDic[type];
        bgmAudioSource.clip = clip;
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
        if (!_seDic.ContainsKey(type)) return;
        AudioClip clip = _seDic[type];
        seAudioSource.PlayOneShot(clip);
    }
}
