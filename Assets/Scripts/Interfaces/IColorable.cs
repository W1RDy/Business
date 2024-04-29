using UnityEngine;

public interface IColorable
{
    public MonoBehaviour ColorableObj {  get; }
    public Color Color { get; set; }
}
