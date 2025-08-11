using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance { get; private set; }

    [SerializeField]
    private List<EffectPreset_SO> presets;

    private Dictionary<EffectType, EffectPreset_SO> presetDict;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        presetDict = new Dictionary<EffectType, EffectPreset_SO>();
        foreach(var p in presets)
        {
            if(!presetDict.ContainsKey(p.type))
                presetDict.Add(p.type, p);
        }
    }

    public void PlayEffect(EffectType type, Vector3 position)
    {
        if (!presetDict.TryGetValue(type, out var preset))
            return;

        GameObject obj = Instantiate(preset.effectPrefab, position + preset.offset, Quaternion.identity);
        obj.transform.localScale = preset.scale;
        Destroy(obj, preset.duration);
    }
}
