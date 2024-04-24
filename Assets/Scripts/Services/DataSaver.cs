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

    public bool SaveWithView { get; private set; }

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

    public void SaveGameFinishState()
    {
        YandexGame.savesData.gameIsFinished = true;
    }

    public void SaveResults(List<ResultsOfTheMonth> resultsOfTheMonth)
    {
        var resultConfigs = YandexGame.savesData.resultSaveConfigs;
        resultConfigs.Clear();
        foreach (var result in resultsOfTheMonth)
        {
            resultConfigs.Add(new ResultSaveConfig(result.PurchaseCosts, result.EmergencyCosts, result.OrderIncome, result.BankIncome));
        }
        _resultsSaved = true;
    }

    public void StartSavingWithView()
    {
        SaveWithView = true;
        StartSaving();
    }

    public void StartSavingWithoutView()
    {
        SaveWithView = false;
        StartSaving();
    }

    private void StartSaving()
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
        yield return new WaitUntil(() => _entitiesSaveConfig.entitiesSetted >= 4);
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
    private int _loadCount;

    public bool DataLoaded { get; private set; }
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
        DataLoaded = false;
        var savesData = YandexGame.savesData;

        _loadCount = 0;
        Action loadCallback = () =>
        {
            _loadCount++;
            if (_loadCount >= 6) FinishLoading();
        };
        
        _gameController.IsTutorial = YandexGame.savesData.tutorialPartsCompleted < 2;

        LoadEntities(savesData.entitiesSaveConfig, loadCallback);
        _handsCoinsCounter.ChangeCoinsByLoadData(savesData.handsCoins);
        _bankCoinsCounter.ChangeCoinsByLoadData(savesData.bankCoins);

        if (savesData.resultSaveConfigs != null) _resultsOfTheMonthService.SetResultsByLoadData(savesData.resultSaveConfigs, loadCallback);
        else loadCallback.Invoke();

        _timeController.SetParametersByLoadData(savesData.time, savesData.monthCount);
        loadCallback.Invoke();
    }

    private void FinishLoading()
    {
        DataLoaded = true;
        OnDataLoaded?.Invoke();
        YandexGame.GetDataEvent -= LoadData;
    }

    private void LoadEntities(EntitiesSaveConfig entitiesSaveConfig, Action loadCallback)
    {
        _entitiesLoader.LoadEntities(entitiesSaveConfig, loadCallback);
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

    public void LoadEntities(EntitiesSaveConfig entitiesSaveConfig, Action loadCallback)
    {
        LoadOrders(entitiesSaveConfig.orders, loadCallback);
        LoadDeliveryOrders(entitiesSaveConfig.deliveryOrders, loadCallback);

        LoadGoods(entitiesSaveConfig.goods, loadCallback);
        LoadPC(entitiesSaveConfig.pcs, loadCallback);

        //LoadProblem(entitiesSaveConfig.problem);
    }

    private void LoadOrders(OrderSaveConfig[] orders, Action loadCallback)
    {
        if (orders != null && orders.Length > 0)
        {
            foreach (OrderSaveConfig orderSaveConfig in orders)
            {
                _orderGenerator.GenerateOrderByLoadData(orderSaveConfig, loadCallback);
            }
        }
        else loadCallback.Invoke();
    }

    private void LoadDeliveryOrders(DeliveryOrderSaveConfig[] orders, Action loadCallback)
    {
        if (orders != null && orders.Length > 0)
        {
            foreach (DeliveryOrderSaveConfig orderSaveConfig in orders)
            {
                _deliveryGenerator.GenerateOrderByLoadData(orderSaveConfig, loadCallback);
            }
        }
        else loadCallback.Invoke();
    }

    private void LoadGoods(GoodsSaveConfig[] goods, Action loadCallback)
    {
        if (goods != null && goods.Length > 0)
        {
            foreach (var good in goods)
            {
                _goodsGenerator.GenerateGoodsByLoadData(good, loadCallback);
            }
        }
        else loadCallback.Invoke();
    }

    private void LoadPC(PCSaveConfig[] pcs, Action loadCallback)
    {
        if (pcs != null && pcs.Length > 0)
        {
            foreach (var pc in pcs)
            {
                _pcGenerator.GeneratePCByLoadData(pc, loadCallback);
            }
        }
        else loadCallback.Invoke();
    }

    //private void LoadProblem(ProblemSaveConfig problem)
    //{
    //    if (problem != null && problem.id != "")
    //    {
    //        _problemsGenerator.GenerateProblemByLoadData(problem);
    //    }
    //}
}