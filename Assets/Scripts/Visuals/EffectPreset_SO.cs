using UnityEngine;
[CreateAssetMenu(fileName = "EffectPreset", menuName = "Effect/EffectPreset")]

public class EffectPreset_SO : ScriptableObject
{
    public EffectType type;
    public GameObject effectPrefab;
    public float duration = 2f;
    public Vector3 offset;
    public Vector3 scale = Vector3.one;
}
