using CoinsCounter;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using YG;

public class DataSaver : MonoBehaviour, IService
{
    private EntitiesSaveConfig _entitiesSaveConfig;

    public event Action OnStartSaving;
    public event Action OnEndSaving;

    private bool _resultsSaved;

    public void SaveOrders(Order[] orders)
    {
        _entitiesSaveConfig.SetOrders(orders);
    }

    public void SaveDeliveryOrders(DeliveryOrder[] deliveryOrders)
    {
        _entitiesSaveConfig.SetDeliveryOrders(deliveryOrders);
    }

    //public void SaveProblem(ProblemConfig problemConfig)
    //{
    //    if (_entitiesSaveConfig == null) _entitiesSaveConfig = YandexGame.savesData.entitiesSaveConfig;
    //    _entitiesSaveConfig.SetProblem(problemConfig);
    //}

    public void SaveGoods(Goods[] goods)
    {
        _entitiesSaveConfig.SetGoods(goods);
    }

    public void SavePCS(PC[] pcs)
    {
        _entitiesSaveConfig.SetPC(pcs);
    }

    public void SaveHandCoins(int count)
    {
        YandexGame.savesData.handsCoins = count;
    }

    public void SaveBankCoins(int count)
    {
        YandexGame.savesData.bankCoins = count;
    }

    public void SaveMonthsAndTime(int months, int time)
    {
        YandexGame.savesData.monthCount = months;
        YandexGame.savesData.time = time;
    }

    public void SaveTutorialState()
    {
        if (YandexGame.savesData.tutorialPartsCompleted < 2)
        {
            YandexGame.savesData.tutorialPartsCompleted++;
        }
    }

    public void SaveResults(List<ResultsOfTheMonth> resultsOfTheMonth)
    {
        var resultConfigs = YandexGame.savesData.resultSaveConfigs;
        foreach (var result in resultsOfTheMonth)
        {
            resultConfigs.Add(new ResultSaveConfig(result.PurchaseCosts, result.EmergencyCosts, result.OrderIncome, result.BankIncome));
        }
        _resultsSaved = true;
    }

    public void StartSaving()
    {
        if (_entitiesSaveConfig == null) _entitiesSaveConfig = YandexGame.savesData.entitiesSaveConfig;
        _entitiesSaveConfig.entitiesSetted = 0;
        StartCoroutine(SaveCoroutine());
    }

    private IEnumerator SaveCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        _resultsSaved = false;
        OnStartSaving?.Invoke();
        yield return new WaitUntil(() => _entitiesSaveConfig.entitiesSetted >= 5);
        yield return new WaitUntil(() => _resultsSaved);
        yield return new WaitForSeconds(0.5f);
        Debug.Log("SaveToCloud");
        SaveAllDatasToCloud();
        yield return new WaitForSeconds(0.5f);

        OnEndSaving?.Invoke();
    }

    private void SaveAllDatasToCloud()
    {
        YandexGame.SaveProgress();
        PrintDatas();
    }

    private void PrintDatas()
    {
        var saveData = YandexGame.savesData;
        Debug.Log("Time " + saveData.time);
        Debug.Log("Months " + saveData.monthCount);
        if (saveData.entitiesSaveConfig != null)
        {
            if (saveData.entitiesSaveConfig.orders != null) Debug.Log("Orders " + saveData.entitiesSaveConfig.orders.Length);
            if (saveData.entitiesSaveConfig.deliveryOrders != null) Debug.Log("Delivery orders" + saveData.entitiesSaveConfig.deliveryOrders.Length);
            if (saveData.entitiesSaveConfig.goods != null) Debug.Log("Goods " + saveData.entitiesSaveConfig.goods.Length);
            if (saveData.entitiesSaveConfig.pcs != null) Debug.Log("Pc " + saveData.entitiesSaveConfig.pcs.Length);
        }

        Debug.Log("Hands coins " + saveData.handsCoins);
        Debug.Log("Bank coins " + saveData.bankCoins);
        if (saveData.entitiesSaveConfig.orders != null) Debug.Log(saveData.entitiesSaveConfig.orders[0].remainTime);
        if (saveData.entitiesSaveConfig.orders != null) Debug.Log(saveData.entitiesSaveConfig.orders[0].cost);
        if (saveData.resultSaveConfigs != null) Debug.Log("Results " + saveData.resultSaveConfigs.Count);
    }
}

public class DataLoader : ClassForInitialization, IService
{
    private EntitiesLoader _entitiesLoader;

    private HandsCoinsCounter _handsCoinsCounter;
    private BankCoinsCounter _bankCoinsCounter;

    private TimeController _timeController;
    private GameController _gameController;

    private ResultsOfTheMonthService _resultsOfTheMonthService;

    public event Action OnDataLoaded;

    public DataLoader() : base() { }

    public override void Init()
    {
        _entitiesLoader = new EntitiesLoader();

        _handsCoinsCounter = ServiceLocator.Instance.Get<HandsCoinsCounter>();
        _bankCoinsCounter = ServiceLocator.Instance.Get<BankCoinsCounter>();

        _timeController = ServiceLocator.Instance.Get<TimeController>();
        _gameController = ServiceLocator.Instance.Get<GameController>();

        _resultsOfTheMonthService = ServiceLocator.Instance.Get<ResultsOfTheMonthService>();
        YandexGame.GetDataEvent += LoadData;
    }

    private void PrintDatas()
    {
        var saveData = YandexGame.savesData;
        Debug.Log("Time " + saveData.time);
        Debug.Log("Months " + saveData.monthCount);
        if (saveData.entitiesSaveConfig != null)
        {
            if (saveData.entitiesSaveConfig.orders != null) Debug.Log("Orders " + saveData.entitiesSaveConfig.orders.Length);
            if (saveData.entitiesSaveConfig.deliveryOrders != null) Debug.Log("Delivery orders" + saveData.entitiesSaveConfig.deliveryOrders.Length);
            if (saveData.entitiesSaveConfig.goods != null) Debug.Log("Goods " + saveData.entitiesSaveConfig.goods.Length);
            if (saveData.entitiesSaveConfig.pcs != null) Debug.Log("Pc " + saveData.entitiesSaveConfig.pcs.Length);
        }

        Debug.Log("Hands coins " + saveData.handsCoins);
        Debug.Log("Bank coins " + saveData.bankCoins);
        if (saveData.entitiesSaveConfig.orders != null) Debug.Log(saveData.entitiesSaveConfig.orders[0].remainTime);
        if (saveData.entitiesSaveConfig.orders != null) Debug.Log(saveData.entitiesSaveConfig.orders[0].cost);
        if (saveData.resultSaveConfigs != null) Debug.Log("Results " + saveData.resultSaveConfigs.Count);
    }

    public void LoadData()
    {
        var savesData = YandexGame.savesData;
        PrintDatas();

        _gameController.IsTutorial = YandexGame.savesData.tutorialPartsCompleted < 2;
        LoadEntities(savesData.entitiesSaveConfig);
        _handsCoinsCounter.ChangeCoins(savesData.handsCoins);
        _bankCoinsCounter.ChangeCoins(savesData.bankCoins);

        if (savesData.resultSaveConfigs != null) _resultsOfTheMonthService.SetResultsByLoadData(savesData.resultSaveConfigs);
        _timeController.SetParametersByLoadData(savesData.time, savesData.monthCount);
        OnDataLoaded?.Invoke();
        YandexGame.GetDataEvent -= LoadData;
    }

    private void LoadEntities(EntitiesSaveConfig entitiesSaveConfig)
    {
        _entitiesLoader.LoadEntities(entitiesSaveConfig);
    }
}

public class EntitiesLoader
{
    private OrderGenerator _orderGenerator;
    private DeliveryOrderGenerator _deliveryGenerator;

    private GoodsGenerator _goodsGenerator;
    private PCGenerator _pcGenerator;

    //private ProblemsGenerator _problemsGenerator;

    public EntitiesLoader()
    {
        _orderGenerator = ServiceLocator.Instance.Get<OrderGenerator>();
        _deliveryGenerator = ServiceLocator.Instance.Get<DeliveryOrderGenerator>();

        _goodsGenerator = ServiceLocator.Instance.Get<GoodsGenerator>();
        _pcGenerator = ServiceLocator.Instance.Get<PCGenerator>();

        //_problemsGenerator = ServiceLocator.Instance.Get<ProblemsGenerator>();
    }

    public void LoadEntities(EntitiesSaveConfig entitiesSaveConfig)
    {
        if (entitiesSaveConfig != null)
        {
            LoadOrders(entitiesSaveConfig.orders);
            LoadDeliveryOrders(entitiesSaveConfig.deliveryOrders);

            LoadGoods(entitiesSaveConfig.goods);
            LoadPC(entitiesSaveConfig.pcs);

            //LoadProblem(entitiesSaveConfig.problem);
        }
    }

    private void LoadOrders(OrderSaveConfig[] orders)
    {
        if (orders != null && orders.Length > 0)
        {
            foreach (OrderSaveConfig orderSaveConfig in orders)
            {
                _orderGenerator.GenerateOrderByLoadData(orderSaveConfig);
            }
        }
    }

    private void LoadDeliveryOrders(DeliveryOrderSaveConfig[] orders)
    {
        if (orders != null && orders.Length > 0)
        {
            foreach (DeliveryOrderSaveConfig orderSaveConfig in orders)
            {
                _deliveryGenerator.GenerateOrderByLoadData(orderSaveConfig);
            }
        }
    }

    private void LoadGoods(GoodsSaveConfig[] goods)
    {
        if (goods != null && goods.Length > 0)
        {
            foreach (var good in goods)
            {
                _goodsGenerator.GenerateGoodsByLoadData(good);
            }
        }
    }

    private void LoadPC(PCSaveConfig[] pcs)
    {
        if (pcs != null && pcs.Length > 0)
        {
            foreach (var pc in pcs)
            {
                _pcGenerator.GeneratePCByLoadData(pc);
            }
        }
    }

    //private void LoadProblem(ProblemSaveConfig problem)
    //{
    //    if (problem != null && problem.id != "")
    //    {
    //        _problemsGenerator.GenerateProblemByLoadData(problem);
    //    }
    //}
}