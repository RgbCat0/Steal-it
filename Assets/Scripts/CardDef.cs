using UnityEngine;

[CreateAssetMenu]
public class CardDef : ScriptableObject
{
    /// <summary>
    /// First check
    /// </summary>
    [field: SerializeField]
    public CardType Type { get; private set; }

    /// <summary>
    /// Only used if Type is Special
    /// </summary>
    [field: SerializeField]
    public SpecialType SpecialType { get; private set; }

    /// <summary>
    /// Only used for normal or minus cards
    /// </summary>
    [field: SerializeField]
    public int CardWorth { get; private set; }
}
