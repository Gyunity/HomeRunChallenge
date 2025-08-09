using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/AudioLibrary")]
public class AudioLibrary : ScriptableObject
{
    [Serializable]
    public struct Entry
    {
        public string key;
        //랜덤 선택용 복수 클립
        public AudioClip[] clips;
    }

    [SerializeField]
    private Entry[] entries;
    private Dictionary<string, AudioClip[]> _dict;

    private void OnEnable()
    {
        _dict = new Dictionary<string, AudioClip[]>(StringComparer.OrdinalIgnoreCase);
        foreach ( var e in entries)
        {
            if(string.IsNullOrEmpty(e.key)) continue;
            _dict[e.key] = e.clips;
        }
    }

    public AudioClip Get(string key)
    {
        if (_dict == null)
            OnEnable();
        if (_dict.TryGetValue(key, out var arr) && arr != null && arr.Length > 0)
            return arr[UnityEngine.Random.Range(0, arr.Length)];
        Debug.LogWarning($"AudioLibrary : key '{key}'가 없습니다.");
        return null;
    }
}
