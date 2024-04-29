using UnityEngine;

public class DesktopLinksService : MonoBehaviour, ILinksService
{
    [SerializeField] private DarknessAnimationController _darknessAnimationController;

    DarknessAnimationController ILinksService._darknessAnimationController => _darknessAnimationController;
}
