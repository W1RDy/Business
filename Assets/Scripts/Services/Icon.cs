using UnityEngine;
using UnityEngine.UI;

public class Icon : MonoBehaviour
{
    [SerializeField] private Image _hair;

    public void SetNewIconImage(Sprite hair)
    {
        _hair.sprite = hair;
    }
}