using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundMap", menuName = "SoundMap", order = 1)]
public class SoundMap : ScriptableObject
{
    [SerializeField] private SoundData[] _sounds;

    public SoundData[] Sounds => _sounds;
}

public enum SoundId
{
    FriendlyHit,
    EnemyHit,
}

public class SoundData
{
    [SerializeField] private SoundId _soundId;
    [SerializeField] private AudioClip _audioClip;

    public SoundId SoundId => _soundId;
    public AudioClip AudioClip => _audioClip;
}
