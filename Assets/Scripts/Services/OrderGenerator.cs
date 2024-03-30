using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class OrderGenerator : MonoBehaviour
{
    [SerializeField] private OrderConfig[] orders;

    private OrderPool _pool;

    private void Start()
    {
        _pool = ServiceLocator.Instance.Get<OrderPool>();
        StartCoroutine(GenerateCoroutine());
    }

    private IEnumerator GenerateCoroutine()
    {
        yield return new WaitForSeconds(3f);
        GenerateOrder();
    }

    private OrderConfig GetOrderConfig()
    {
        var randomIndex = Random.Range(0, orders.Length);
        return orders[randomIndex];
    }

    public Order GenerateOrder()
    {
        var order = _pool.Get();
        order.Init(GetOrderConfig());
        return order;
    }
}
