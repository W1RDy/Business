using UnityEngine;

public class DesktopLinksEntitiesService : MonoBehaviour, IEntitiesLinksService
{
    [SerializeField] private CompositeOrder _compositeDeliveryOrder;

    [Header("Pools")]

    #region PoolsTransforms

    [SerializeField] private RectTransform _orderPoolContainer;
    [SerializeField] private Transform _orderParent;

    [SerializeField] private RectTransform _goalPoolContainer;
    [SerializeField] private Transform _goalParent;

    [SerializeField] private RectTransform _deliveryOrderPoolContainer;
    [SerializeField] private Transform _deliveryOrderParent;

    [SerializeField] private RectTransform _goodsPoolContainer;
    [SerializeField] private Transform _goodsParent;

    CompositeOrder IEntitiesLinksService._compositeDeliveryOrder => _compositeDeliveryOrder;

    RectTransform IEntitiesLinksService._orderPoolContainer => _orderPoolContainer;

    Transform IEntitiesLinksService._orderParent => _orderParent;

    RectTransform IEntitiesLinksService._goalPoolContainer => _goalPoolContainer;

    Transform IEntitiesLinksService._goalParent => _goalParent;

    RectTransform IEntitiesLinksService._deliveryOrderPoolContainer => _deliveryOrderPoolContainer;

    Transform IEntitiesLinksService._deliveryOrderParent => _deliveryOrderParent;

    RectTransform IEntitiesLinksService._goodsPoolContainer => _goodsPoolContainer;

    Transform IEntitiesLinksService._goodsParent => _goodsParent;

    #endregion
}
