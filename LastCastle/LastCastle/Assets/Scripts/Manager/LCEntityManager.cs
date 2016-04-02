using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LCEntityManager : CMonoBehaviour 
{
	#region Singleton

	private static object m_SingletonLock = new object();
	private static LCEntityManager m_Instance = null;

	public static LCEntityManager Instance
	{
		get
		{
			lock (m_SingletonLock)
			{
				if (m_Instance == null)
				{
					var gameObject = GameObject.Instantiate(Resources.Load("Prefabs/EntityManager")) as GameObject;
					m_Instance = gameObject.GetComponent<LCEntityManager>();
				}
				return m_Instance;
			}
		}
	}

	public static LCEntityManager GetInstance()
	{
		return Instance;
	}

	#endregion

	#region Properties

	private static Dictionary<string, LCEntity> m_ListEntities = new Dictionary<string, LCEntity>();
	private LCDataReader m_DataReader;

	#endregion

	#region Implementation Monobehaviour


	void Awake()
	{
		m_Instance = this;
		DontDestroyOnLoad(this);
		m_DataReader = LCDataReader.Instance;
	}

	void Start()
	{
		
	}

	#endregion

	#region Main methods

	public LCEntity CreateEntity(LCEnum.EEntityType type,
									Vector3 position,
									Quaternion rotation,
									GameObject parent = null)
	{
		GameObject gObject = null;
		LCBaseData data = null;
		LCBaseController controller = null;
		LCEntity entity = null;
		string name = type.ToString() + (m_ListEntities.Count + 1);
		var index = m_ListEntities.Count;

		if (type == LCEnum.EEntityType.Lord)
		{
			// Lord
			data = m_DataReader.GetLordData();
			entity = new LCLord(controller, data);
		}
		else if ((int)type >= (int)LCEnum.EEntityType.Human && (int)type < (int)LCEnum.EEntityType.Castle)
		{
			// Human && Demon
			data = m_DataReader.GetSodierData(type);
			gObject = GameObject.Instantiate(Resources.Load(data.ModelPaths[index % data.ModelPaths.Length]), position, rotation) as GameObject;
			controller = gObject.AddComponent<LCSodierController>();
			entity = new LCSodier(controller, data);
		}
		else if ((int)type >= (int)LCEnum.EEntityType.Castle && (int)type < (int)LCEnum.EEntityType.CastleResources)
		{
			// Castle
			data = m_DataReader.GetCastleData(type);
			gObject = GameObject.Instantiate(Resources.Load(data.ModelPaths[index % data.ModelPaths.Length]), position, rotation) as GameObject;
			controller = gObject.AddComponent<LCCastleController>();
			entity = new LCCastle(controller, data);
		}
		else if ((int)type >= (int)LCEnum.EEntityType.CastleResources && (int)type < (int)LCEnum.EEntityType.Cave)
		{
			// Castle resource
			data = m_DataReader.GetCastleResourceData(type);
			gObject = GameObject.Instantiate(Resources.Load(data.ModelPaths[index % data.ModelPaths.Length]), position, rotation) as GameObject;
			controller = gObject.AddComponent<LCCastleResourceController>();
			entity = new LCCastleResource(controller, data);
		}
		else if ((int)type >= (int)LCEnum.EEntityType.Cave && (int)type < (int)LCEnum.EEntityType.Animal)
		{
			// Cave
			data = m_DataReader.GetCaveData(type);
			gObject = GameObject.Instantiate(Resources.Load(data.ModelPaths[index % data.ModelPaths.Length]), position, rotation) as GameObject;
			controller = gObject.AddComponent<LCCaveController>();
			entity = new LCCave(controller, data);
		}
		else
		{
			// Animal
			data = m_DataReader.GetAnimalData(type);
			gObject = GameObject.Instantiate(Resources.Load(data.ModelPaths[index % data.ModelPaths.Length]), position, rotation) as GameObject;
			controller = gObject.AddComponent<LCAnimalController>();
			entity = new LCAnimal(controller, data);
		}
		entity.SetActive(false);
		if (controller != null)
		{
			controller.SetEntity(entity);
			controller.Init();
			gObject.name = name;
			if (parent != null)
			{
				gObject.transform.SetParent(parent.transform);
			}
		}
		m_ListEntities.Add(name, entity);	
		return entity;
	}

	#endregion

	#region Getter && Setter

	public LCEntity GetEntityByName(string name) {
		if (!m_ListEntities.ContainsKey(name))
		{
			Debug.LogError("The Entity not exist " + name);
			return null;
		}
		return m_ListEntities[name];
	}

	#endregion

}
