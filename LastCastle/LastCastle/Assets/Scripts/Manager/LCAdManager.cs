using UnityEngine;
using UnityEngine.Advertisements;
using System;

public class LCAdManager : MonoBehaviour {

	#region Singleton

	private static object m_SingletonLock = new object();
	private static LCAdManager m_Instance = null;

	public static LCAdManager Instance
	{
		get
		{
			lock (m_SingletonLock)
			{
				if (m_Instance == null)
				{
					var gameObject = GameObject.Instantiate(Resources.Load("Prefabs/AdManager")) as GameObject;
					m_Instance = gameObject.GetComponent<LCAdManager>();
				}
				return m_Instance;
			}
		}
	}

	public static LCAdManager GetInstance()
	{
		return Instance;
	}

	#endregion

	#region Properties

	#endregion

	#region Implementation Monobehaviour

	void Awake()
	{
		m_Instance = this;
		DontDestroyOnLoad(this);
	}

	#endregion

	public void ShowRewardedAd(Action complete, Action error)
	{
		if (Advertisement.IsReady("rewardedVideo"))
		{
			var option = new ShowOptions { resultCallback = (result) => 
					{
						HandleShowResult(result, complete, error); 
					}
			};
			Advertisement.Show("rewardedVideo", option);
		}
	}

	private void HandleShowResult(ShowResult result, Action complete, Action error)
	{
		switch (result)
		{
			case ShowResult.Finished:
#if UNITY_EDITOR
				Debug.Log("The ad was successfully shown.");
#endif
				if (complete != null)
				{
					complete();
				}
				break;
			case ShowResult.Skipped:
#if UNITY_EDITOR
				Debug.Log("The ad was skipped before reaching the end.");
#endif
				if (complete != null)
				{
					complete();
				}
				break;
			case ShowResult.Failed:
#if UNITY_EDITOR
				Debug.LogError("The ad failed to be shown.");
#endif
				if (error != null)
				{
					error();
				}
				break;
		}
	}

}
