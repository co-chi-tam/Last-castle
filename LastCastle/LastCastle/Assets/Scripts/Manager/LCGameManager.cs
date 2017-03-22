using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ObjectPool;

public class LCGameManager : CMonoBehaviour
{

    #region Singleton

    private static object m_SingletonLock = new object();
    private static LCGameManager m_Instance = null;

    public static LCGameManager Instance
    {
        get
        {
            lock (m_SingletonLock)
            {
                if (m_Instance == null)
                {
					var gameObject = GameObject.Instantiate(Resources.Load("Prefabs/GameManager")) as GameObject;
					m_Instance = gameObject.GetComponent<LCGameManager>();
                }
                return m_Instance;
            }
        }
    }

    public static LCGameManager GetInstance()
    {
        return Instance;
    }

    #endregion

	#region Properties

	private LCEntityManager m_EntityManager;
	private LCUIManager m_UIManager;
	private LCDataReader m_DataReader;
	private Dictionary<LCEnum.EEntityType, ObjectPool<LCEntity>> m_ObjectPool;
	private Dictionary<LCEnum.EEntityType, LCBaseData> m_DefaultValue;
	private int m_CurrentPoolAmount;
	private LCMapObject m_Map;

	public bool OnLoadComplete
	{
		get;
		private set;
	}
	public bool OnPlaying
	{
		get;
		private set;
	}

	#endregion

    #region Implementation Monobehaviour

    void Awake()
    {
        m_Instance = this;

		m_EntityManager = LCEntityManager.GetInstance();
		m_DataReader = LCDataReader.GetInstance();
		m_UIManager = LCUIManager.GetInstance();
		m_ObjectPool = new Dictionary<LCEnum.EEntityType, ObjectPool<LCEntity>>();
		m_DefaultValue = new Dictionary<LCEnum.EEntityType, LCBaseData>();

		Application.targetFrameRate = 60;
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		OnPlaying = true;
    }

	void Start()
	{
		m_DataReader.GetDefaultValue(ref m_DefaultValue);
		m_UIManager.ShowLoadingPanel(true);
		StartCoroutine(LoadObjectPool(() => { 
			LoadFirstEntity(); 
			OnLoadComplete = true;
		}));
	}

    #endregion

    #region Main method

	private void LoadFirstEntity() 
	{
		var position = new Vector3(-20f, 0f, 0f);
		LCEntity stoneCastle = null;
		if (GetObjectPool(LCPlayerManager.CurrentCastle, ref stoneCastle))
		{
			stoneCastle.SetOwner(LCPlayerManager.CurrentPlayer);
			stoneCastle.SetTransformPosition(position);
			stoneCastle.SetTeam(LCEnum.EClass.Human);
			stoneCastle.SetCommand(LCEnum.ECommand.Manual);     // Command mode AI/Manual
			LCUIManager.Instance.InitBattleSceneUI(stoneCastle);
			stoneCastle.InitUI();
			stoneCastle.SetActive(true);
		}

		m_Map = m_DataReader.GetMapData(LCPlayerManager.CurrentMap);
		LoadMap(m_Map, stoneCastle);

		OnPlaying = true;

		m_UIManager.ShowLoadingPanel(false);
	}

	private void LoadMap(LCMapObject mapObject, LCEntity owner)
	{
		for (int i = 0; i < mapObject.Childs.Length; i++)
		{
			var obj = mapObject.Childs[i];
			LCEntity entity = null;
			if (GetObjectPool(obj.MapObject, ref entity))
			{
				entity.SetTransformPosition(obj.Position);
				entity.SetTeam(obj.Team);
				entity.SetCommand(LCEnum.ECommand.AI);
				if (obj.ObjectType == LCEnum.EObjectType.Partner)
				{
					entity.SetOwner(owner);
				}
				else if (obj.ObjectType == LCEnum.EObjectType.Vilain)
				{
					entity.SetEnemyEntity(owner);
					owner.SetEnemyEntity(entity);
				}
				entity.SetActive(true);
			}
			LoadMap(obj, owner);
		}
	}

	private IEnumerator LoadObjectPool(Action complete = null) {
		if (m_ObjectPool.Count > 0)
			yield break; // Once times
		m_CurrentPoolAmount = 0;
		var objPool = m_DataReader.GetObjectPoolData();
		for (int i = 0; i < objPool.Count; i++)
		{
			var poolData = objPool[i];
			m_ObjectPool.Add(poolData.GameType, new ObjectPool<LCEntity>());
			StartCoroutine(LoadAmountObject(poolData, complete));
		}
		yield return null;
	}

	private IEnumerator LoadAmountObject(ObjectPoolData poolData, Action complete = null) {
		for (int x = 0; x < poolData.Amount; x++) {
			var obj = m_EntityManager.CreateEntity(poolData.GameType, Vector3.zero, Quaternion.identity, this.gameObject);
			obj.SetActive(false);
			m_ObjectPool[poolData.GameType].Create(obj);
			yield return obj != null;
			m_CurrentPoolAmount++;
			if (complete != null && m_CurrentPoolAmount == m_DataReader.MaxPoolAmount)
			{
				complete();
			}
		}
	}

	public void WasCastleFail(LCEntity castle)
	{
		OnPlaying = false;
		var enemy = castle.GetEnemyEntity();
		if (enemy != null)
		{
			var lord = enemy.GetOwner();
			if (lord != null)
			{
				var gold = lord.GetGold() + castle.GetGoldWinBonus();
				lord.SetGold(gold);
				if (m_Map.UnlockMap != LCEnum.EEntityType.None)
				{
					lord.SetMapUnlock(m_Map.UnlockMap);
				}
				m_UIManager.SetGameEnd(true, castle.GetGoldWinBonus().ToString());
			}
			else
			{
				m_UIManager.SetGameEnd(false, string.Empty);
			}
		}
		LCPlayerManager.SaveStatus();
	}

    #endregion

	#region Getter && Setter

	public LCSodierData GetSodierData(LCEnum.EEntityType type)
	{
		return m_DefaultValue[type] as LCSodierData;
	}

	public LCEntity GetEntityByName(string name) {
		return m_EntityManager.GetEntityByName(name);
	}

	public LCEntity GetObjectPool(LCEnum.EEntityType type) {
		LCEntity result = null;
		if (m_ObjectPool[type].Get(ref result))
		{
			return result;
		}
		else
		{
			Debug.LogError("[GameManager] Set more pool " + type);
		}
		return result;
	}

	public bool GetObjectPool(LCEnum.EEntityType type, ref LCEntity obj) {
		if (m_ObjectPool[type].Get(ref obj))
		{
			return true;
		}
		else
		{
			Debug.LogError("[GameManager] Set more pool " + type);
		}
		return false;
	}

	public void SetObjectPool(LCEntity obj) {
		var gameType = obj.GetEntityType();
		if (m_ObjectPool.ContainsKey(gameType) == false)
		{
			m_ObjectPool[gameType] = new ObjectPool<LCEntity>();
			Debug.LogError("[GameManager] Pool not instance " + gameType);
		}
		m_ObjectPool[gameType].Set(obj);
		obj.GetController().transform.SetParent(this.transform);
	}

	#endregion

	#region Ultilities

	public bool CheckSodierPrice(LCEnum.EEntityType type, int cereal, int fish, int meat, int gold, int population)
	{
		var price = m_DefaultValue[type] as LCSodierData;
		var result = cereal >= price.CostResources[(int)LCEnum.EResourcesType.Cereal] 
			&& fish >= price.CostResources[(int)LCEnum.EResourcesType.Fish] 
			&& meat >= price.CostResources[(int)LCEnum.EResourcesType.Meat] 
			&& gold >= price.CostResources[(int)LCEnum.EResourcesType.Metal]
			&& population >= price.Population;
		return result;
	}

	public bool CheckSodierPrice(LCEnum.EEntityType type, int[] resource, int population)
	{
		var price = m_DefaultValue[type] as LCSodierData;
		var result = true;
		for (int i = 0; i < price.CostResources.Length; i++)
		{
			result &= resource[i] >= price.CostResources[i];
		}
		result &= population >= price.Population;
		return result;
	}

	#endregion

}
