using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// 効果音のID定義
/// </summary>
public enum SEID
{
    SE_ButtonDecision = 0,
    SE_Decision,
    SE_VoxelDestroy,
    SE_Minigun,
    SE_Shotgun,
    SE_Launcher,
    SE_Reload,
    SE_WeaponChange,
    SE_CoasterLaunch,
    SE_CoasterRunning,
    SE_OpenMenu,
    SE_ReticuleDecision,
    SE_ResultScoreDisplay,
}

/// <summary>
/// BGMのID定義
/// </summary>
public enum BGMID
{
    BGM_Title = 0,
    BGM_StageSelect,
    BGM_Stage,
    BGM_Stage1,
    BGM_Stage2,
    BGM_Stage3,
    BGM_Stage4,
    BGM_Clear,
}

/// <summary>
/// サウンド管理クラス
/// </summary>
public class AudioManager : MonoBehaviour
{
    // シングルトンインスタンス
    public static AudioManager Instance { get; private set; }

    // SEのリスト
    private List<AudioClip> seClips = new List<AudioClip>();

    // BGMのリスト
    private List<AudioClip> bgmClips = new List<AudioClip>();

    // SE用のAudioSource
    private List<AudioSource> audioSourceList = new List<AudioSource>();

    // BGM用のAudioSource(常に一つだけ)
    private AudioSource bgmSource;

    private void Awake()
    {
        // シングルトン初期化処理
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーンをまたいでも保持
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // SEリストをenum順で追加
        foreach (SEID id in Enum.GetValues(typeof(SEID)))
        {
            seClips.Add((AudioClip)Resources.Load("SE/" + id.ToString()));
        }

        // BGMリストをenum順で追加
        foreach (BGMID id in Enum.GetValues(typeof(BGMID)))
        {
            bgmClips.Add((AudioClip)Resources.Load("BGM/" + id.ToString()));
        }
    }

    /// <summary>
    /// 効果音再生
    /// </summary>
    public void PlaySE(SEID se)
    {
        int index = (int)se;
        if (index >= 0 && index < seClips.Count && seClips[index] != null)
        {
            AudioSource audioSource = GetAudioSource();
            audioSource.PlayOneShot(seClips[index]);
        }
        else
        {
            Debug.LogWarning("AudioClipが見つかりません: " + se);
        }
    }

    /// <summary>
    /// SE停止
    /// </summary>
    public void StopSE(SEID se)
    {
        int index = (int)se;
        if (index < 0 || index >= seClips.Count || seClips[index] == null) return;

        foreach (AudioSource audio in audioSourceList)
        {
            if (audio.isPlaying && audio.clip == seClips[index])
            {
                audio.Stop();
            }
        }
    }

    /// <summary>
    /// BGM再生
    /// </summary>
    public void PlayBGM(BGMID bgm)
    {
        int index = (int)bgm;
        if (index >= 0 && index < bgmClips.Count && bgmClips[index] != null)
        {
            if (bgmSource == null)
            {
                bgmSource = gameObject.AddComponent<AudioSource>();
                bgmSource.loop = true;
                bgmSource.volume = 0.5f;
            }

            bgmSource.Stop();
            bgmSource.clip = bgmClips[index];
            bgmSource.Play();

            Debug.Log("BGM再生: " + bgm);
        }
        else
        {
            Debug.LogWarning("AudioClipが見つかりません: " + bgm);
        }
    }

    public void StopAllSE()
    {
        foreach (AudioSource audio in audioSourceList)
        {
            if (audio.isPlaying)
            {
                audio.Stop();
            }
        }
    }

    public void StopBGM()
    {
        if (bgmSource != null && bgmSource.isPlaying)
        {
            bgmSource.Stop();
            bgmSource.clip = null;
        }
    }

    /// <summary>
    /// AudioSourceを取得
    /// </summary>
    private AudioSource GetAudioSource()
    {
        foreach (AudioSource audio in audioSourceList)
        {
            if (!audio.isPlaying)
            {
                return audio;
            }
        }
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        audioSourceList.Add(newSource);
        return newSource;
    }
}