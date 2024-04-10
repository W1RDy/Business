using UnityEngine;

[CreateAssetMenu(fileName = "IconComponents", menuName = "Components/New Icon Components")]
public class IconComponents : ScriptableObject
{
    [SerializeField] private Sprite _body;
    [SerializeField] private Sprite[] _hairs;

    public Sprite Body => _body;
    public Sprite[] Hairs => _hairs;
}
