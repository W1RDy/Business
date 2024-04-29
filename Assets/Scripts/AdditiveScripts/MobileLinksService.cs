using UnityEngine;

public class MobileLinksService : MonoBehaviour, ILinksService
{
    [SerializeField] private DarknessAnimationController _darknessAnimationController;

    DarknessAnimationController ILinksService._darknessAnimationController => _darknessAnimationController;
}
