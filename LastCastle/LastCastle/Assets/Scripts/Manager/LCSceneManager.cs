using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class LCSceneManager : CMonoBehaviour {

	#region Singleton

	private static object m_SingletonLock = new object();
	private static LCSceneManager m_Instance = null;

	public static LCSceneManager Instance
	{
		get
		{
			lock (m_SingletonLock)
			{
				if (m_Instance == null)
				{
					var gameObject = GameObject.Instantiate(Resources.Load("Prefabs/SceneManager")) as GameObject;
					m_Instance = gameObject.GetComponent<LCSceneManager>();
				}
				return m_Instance;
			}
		}
	}

	public static LCSceneManager GetInstance()
	{
		return Instance;
	}

	#endregion

	#region Properties

	private static Dictionary<LCEnum.EScene, string> m_Scenes = new Dictionary<LCEnum.EScene, string>();
	private static LCEnum.EScene m_CurrentScene = LCEnum.EScene.StartScene;

	#endregion

	#region Implementation Monobehaviour

	void Awake()
	{
		m_Instance = this;

		m_Scenes.Clear();
		m_Scenes.Add(LCEnum.EScene.StartScene, "StartScene");
		m_Scenes.Add(LCEnum.EScene.BattleScene, "BattleScene");
	}

	void Start()
	{
		UpdateScene();
	}

	void OnLevelWasLoaded(int level) 
	{
		UpdateScene();
	}

	void OnApplicationQuit()
	{
		LCPlayerManager.SaveStatus();
	}

	#endregion

	#region Main methods

	private void UpdateScene()
	{
		LCDataReader.GetInstance();
		LCAdManager.GetInstance();
		var entityManager = LCEntityManager.GetInstance();
		if (LCPlayerManager.CurrentPlayer == null)
		{
			LCPlayerManager.CurrentPlayer = entityManager.CreateEntity(LCEnum.EEntityType.Lord, Vector3.zero, Quaternion.identity);
		}
		LCUIManager.GetInstance();
		if (CheckScene(LCEnum.EScene.StartScene))
		{
			
		}
		else if (CheckScene(LCEnum.EScene.BattleScene))
		{
			LCGameManager.GetInstance();
		}
	}

	public bool CheckScene(LCEnum.EScene scene)
	{
		return m_CurrentScene == scene;
	}

	public void LoadScene(LCEnum.EScene scene)
	{
		m_CurrentScene = scene;
		SceneManager.LoadScene(m_Scenes[scene]);
	}

	public void LoadSceneAsyn(LCEnum.EScene scene)
	{
		m_CurrentScene = scene;
		SceneManager.LoadSceneAsync(m_Scenes[scene]);
	}

	public void LoadSceneAsyn(LCEnum.EScene scene, Action complete)
	{
		StartCoroutine(HandleLoadSceneAsyn(scene, complete));
	}

	private IEnumerator HandleLoadSceneAsyn(LCEnum.EScene scene, Action complete)
	{
		var loading = SceneManager.LoadSceneAsync(m_Scenes[scene]);
		yield return loading.isDone;
		m_CurrentScene = scene;
		if (complete != null)
		{
			complete();
		}
	}

	#endregion

}
