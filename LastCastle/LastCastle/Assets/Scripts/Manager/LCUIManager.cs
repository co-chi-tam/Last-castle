using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

public class LCUIManager : CMonoBehaviour {

	#region Singleton

	private static object m_SingletonLock = new object();
	private static LCUIManager m_Instance = null;

	public static LCUIManager Instance
	{
		get
		{
			lock (m_SingletonLock)
			{
				if (m_Instance == null)
				{
					var gameObject = GameObject.Instantiate(Resources.Load("Prefabs/UIManager")) as GameObject;
					m_Instance = gameObject.GetComponent<LCUIManager>();
				}
				return m_Instance;
			}
		}
	}

	public static LCUIManager GetInstance()
	{
		return Instance;
	}

	#endregion

	#region Properties

	[Header("Start Scene")]
	[SerializeField]
	private GameObject m_StartGamePanel;
	[SerializeField]
	private Button[] m_MapPointButtons;
	[SerializeField]
	private CUICastleShopItem[] m_ShopCastleItems;
	[SerializeField]
	private Text m_LordGoldText;
	[SerializeField]
	private GameObject m_ShopPanel;
	[SerializeField]
	private Button m_BackShopButton;

	[Header("Main Scene")]
	[SerializeField]
	private GameObject m_MainGamePanel;
	[SerializeField]
	private Button m_Troop_1_Button;
	[SerializeField]
	private Text m_Troop_1_Text;
	[SerializeField]
	private Button m_Troop_2_Button;
	[SerializeField]
	private Text m_Troop_2_Text;
	[SerializeField]
	private Button m_Troop_3_Button;
	[SerializeField]
	private Text m_Troop_3_Text;
	[SerializeField]
	private Button m_Troop_4_Button;
	[SerializeField]
	private Text m_Troop_4_Text;
	[SerializeField]
	private Button m_Troop_5_Button;
	[SerializeField]
	private Text m_Troop_5_Text;
	[SerializeField]
	private Text m_CerealAmountText;
	[SerializeField]
	private Text m_FishAmountText;
	[SerializeField]
	private Text m_MeatAmountText;
	[SerializeField]
	private Text m_MetalAmountText;
	[SerializeField]
	private GameObject m_EndGamePanel;
	[SerializeField]
	private GameObject m_LoadingPanel;
	[SerializeField]
	private Text m_TipText;
	[SerializeField]
	private GameObject m_WinPanel;
	[SerializeField]
	private GameObject m_LosePanel;
	[SerializeField]
	private Text m_BonusGoldText;
	[SerializeField]
	private Button m_BackWorldMapButton;

	[Header("Advertisements UI")]
	[SerializeField]
	private Button[] m_TreasureButtons;
	[SerializeField]
	private GameObject m_SelectRewardPanel;
	[SerializeField]
	private Button m_NormalRewardButton;
	[SerializeField]
	private Button m_VideoRewardButton;
	[SerializeField]
	private GameObject m_ClaimRewardPanel;
	[SerializeField]
	private Button m_ClaimRewardButton;

	private LCDataReader m_DataReader;
	private LCSceneManager m_SceneManager;

	private static float m_LoadAdCoundown = 0f;
	private float m_LoadAdInterval = 300f;
	private bool m_SelectReward = false;

	#endregion

	#region Implementation Monobehaviour

	void Awake()
	{
		m_Instance = this;
		DontDestroyOnLoad(this);
	}

	void Start()
	{
		SetUpUI();
	}

	void OnLevelWasLoaded(int index)
	{
		SetUpUI();
	}

	void Update()
	{
		if ((Time.time - m_LoadAdCoundown) > m_LoadAdInterval)
		{
			var random = Random.Range(0, 999) % m_TreasureButtons.Length;
			m_TreasureButtons[random].gameObject.SetActive(true);
			m_LoadAdCoundown = Time.time;
		}
	}

	#endregion

	#region Main methods

	private void SetUpUI()
	{
		m_DataReader = LCDataReader.GetInstance();
		m_SceneManager = LCSceneManager.GetInstance();
		if (m_SceneManager.CheckScene(LCEnum.EScene.StartScene))
		{
			m_MainGamePanel.SetActive(false);
			m_StartGamePanel.SetActive(true);
			ShowShopPanel(false);
			ShowClaimRewardPanel (false);
			LoadUILord();
			LoadUIWorlMap();
			LoadUIShop();
			LoadUITreasure();
			LoadSelectReward();
		}
		else if (m_SceneManager.CheckScene(LCEnum.EScene.BattleScene))
		{
			m_MainGamePanel.SetActive(true);
			m_StartGamePanel.SetActive(false);
			m_EndGamePanel.SetActive(false);
			m_WinPanel.SetActive(false);
			m_LosePanel.SetActive(false);
			m_LoadingPanel.SetActive(false);
		}
		if (m_LoadAdCoundown == 0f)
		{
			m_LoadAdCoundown = Time.time;
		}
	}

	private void LoadUIShop()
	{
		var shopCastles = m_DataReader.GetCastleData();
		var playerCastle = LCPlayerManager.CurrentPlayer.GetCastleUnlock();
		m_BackShopButton.onClick.RemoveAllListeners();
		m_BackShopButton.onClick.AddListener(() =>
			{
				ShowShopPanel(!m_ShopPanel.activeInHierarchy);
			});
		for (int i = 0; i < m_ShopCastleItems.Length; i++)
		{
			if (i < shopCastles.Count)
			{
				var item = m_ShopCastleItems[i];
				var castleType = shopCastles[i].EntityType;
				var unlock = playerCastle.Contains(castleType);
				var price = shopCastles[i].CostGold;
				item.CastleNameText.text = shopCastles[i].Name;
				item.CastlePriceText.text = "Price " + shopCastles[i].CostGold;
				item.SelectCastleButton.gameObject.SetActive(unlock);
				item.BuyCastleButton.gameObject.SetActive(!unlock);
				item.SelectCastleButton.onClick.RemoveAllListeners();
				item.SelectCastleButton.onClick.AddListener(() =>
					{
						if (unlock)
						{
							LCPlayerManager.CurrentCastle = castleType;
							m_SceneManager.LoadSceneAsyn(LCEnum.EScene.BattleScene);
							LCPlayerManager.SaveStatus();
						}
					});

				item.BuyCastleButton.onClick.RemoveAllListeners();
				item.BuyCastleButton.onClick.AddListener(() =>
					{
						if (!unlock)
						{
							var gold = LCPlayerManager.CurrentPlayer.GetGold();
							if (gold > price)
							{
								gold -= price;
								LCPlayerManager.CurrentPlayer.SetGold(gold);
								LCPlayerManager.CurrentPlayer.SetCastleUnlock(castleType);
								LoadUIShop();
								LCPlayerManager.SaveStatus();
							}
						}
					});
			}
			else
			{
				m_ShopCastleItems[i].gameObject.SetActive(false);
			}
		}
	}

	private void LoadUILord()
	{
		var player = LCPlayerManager.CurrentPlayer;
		player.OnGoldChange -= SetGoldText;
		player.OnGoldChange += SetGoldText;
		player.InitUI();
	}

	private void LoadUIWorlMap()
	{
		var worldMapObjects = m_DataReader.GetWorldMapDatas();
		var mapUnlock = LCPlayerManager.CurrentPlayer.GetMapUnlock();
		for (int i = 0; i < m_MapPointButtons.Length; i++)
		{
			var button = m_MapPointButtons[i];
			if (i < worldMapObjects.Count)
			{
				var obj = worldMapObjects[i];
				if (mapUnlock.Contains(obj.MapObject))
				{
					button.transform.localPosition = obj.Position;
					button.gameObject.SetActive(true);
					button.onClick.RemoveAllListeners();
					button.onClick.AddListener(() =>
						{
							LCPlayerManager.CurrentMap = obj.MapObject;
							ShowShopPanel(true);
						});
				}
				else
				{
					button.gameObject.SetActive(false);
					button.onClick.RemoveAllListeners();
				}
			}
			else
			{
				button.gameObject.SetActive(false);
				button.onClick.RemoveAllListeners();
			}
		}
	}

	private void LoadUITreasure()
	{
		m_ClaimRewardButton.onClick.RemoveAllListeners();
		m_ClaimRewardButton.onClick.AddListener(() =>
			{
				ShowClaimRewardPanel(!m_ClaimRewardPanel.activeInHierarchy);
			});
		for (int i = 0; i < m_TreasureButtons.Length; i++)
		{
			var button = m_TreasureButtons[i];
			//button.gameObject.SetActive(false);
			button.onClick.RemoveAllListeners();
			button.onClick.AddListener(() =>
				{
					button.gameObject.SetActive(false);
					m_SelectRewardPanel.SetActive(true);
					m_SelectReward = false;
				});
		}
	}

	public void InitBattleSceneUI(LCEntity entity)
	{
		var remainEntity = entity;
		m_MainGamePanel.SetActive(true);
		m_StartGamePanel.SetActive(false);
		m_Troop_1_Button.onClick.RemoveAllListeners();
		m_Troop_1_Button.onClick.AddListener(() =>
			{
				SetTroopSpawn(0, remainEntity);
			});
		m_Troop_2_Button.onClick.RemoveAllListeners();
		m_Troop_2_Button.onClick.AddListener(() =>
			{
				SetTroopSpawn(1, remainEntity);
			});
		m_Troop_3_Button.onClick.RemoveAllListeners();
		m_Troop_3_Button.onClick.AddListener(() =>
			{
				SetTroopSpawn(2, remainEntity);
			});
		m_Troop_4_Button.onClick.RemoveAllListeners();
		m_Troop_4_Button.onClick.AddListener(() =>
			{
				SetTroopSpawn(3, remainEntity);
			});
		m_Troop_5_Button.onClick.RemoveAllListeners();
		m_Troop_5_Button.onClick.AddListener(() =>
			{
				SetTroopSpawn(4, remainEntity);
			});
		
		remainEntity.OnCerealChange -= SetCerealText;
		remainEntity.OnFishChange -= SetFishText;
		remainEntity.OnMeatChange -= SetMeatText;
		remainEntity.OnMetalChange -= SetMetalText;

		remainEntity.OnCerealChange += SetCerealText;
		remainEntity.OnFishChange += SetFishText;
		remainEntity.OnMeatChange += SetMeatText;
		remainEntity.OnMetalChange += SetMetalText;

		m_Troop_1_Text.text = GetSodierDetail(0, remainEntity).Name;
		m_Troop_2_Text.text = GetSodierDetail(1, remainEntity).Name;
		m_Troop_3_Text.text = GetSodierDetail(2, remainEntity).Name;
		m_Troop_4_Text.text = GetSodierDetail(3, remainEntity).Name;
		m_Troop_5_Text.text = GetSodierDetail(4, remainEntity).Name;

		m_BackWorldMapButton.onClick.RemoveAllListeners();
		m_BackWorldMapButton.onClick.AddListener(() => {
			LCSceneManager.Instance.LoadSceneAsyn(LCEnum.EScene.StartScene);
		});
	}

	public void ShowLoadingPanel(bool show)
	{
		m_LoadingPanel.SetActive(show);
	}

	private void ShowShopPanel(bool show)
	{
		m_ShopPanel.SetActive(show);
	}

	private void ShowClaimRewardPanel(bool show)
	{
		m_ClaimRewardPanel.SetActive(show);
	}

	private void LoadSelectReward()
	{
		m_NormalRewardButton.onClick.RemoveAllListeners();
		m_NormalRewardButton.onClick.AddListener(() =>
			{
				if (m_SelectReward == false)
				{
					var gold = LCPlayerManager.CurrentPlayer.GetGold();
					LCPlayerManager.CurrentPlayer.SetGold(gold + 500);
					m_SelectReward = true;
					m_SelectRewardPanel.SetActive(false);
				}
			});

		m_VideoRewardButton.onClick.RemoveAllListeners();
		m_VideoRewardButton.onClick.AddListener(() =>
			{
				if (m_SelectReward == false)
				{
					LCAdManager.Instance.ShowRewardedAd(() => {
						var gold = LCPlayerManager.CurrentPlayer.GetGold();
						LCPlayerManager.CurrentPlayer.SetGold(gold + 1000);
						ShowClaimRewardPanel (true);
					}, () => {
#if UNITY_EDITOR
						Debug.Log("Error Add");
#endif
					});
					m_SelectReward = true;
					m_SelectRewardPanel.SetActive(false);
				}
			});
	}

	#endregion

	#region Getter && Setter

	private LCSodierData GetSodierDetail(int index, LCEntity entity)
	{
		var types = entity.GetTroopTypes();
		if (index >= types.Length)
			return null;
		var type = types[index];
		var data = LCGameManager.Instance.GetSodierData(type);
		return data;
	}

	private void SetTroopSpawn(int index, LCEntity entity)
	{
		entity.SetSingleTroop(index);
	}

	private void SetGoldText(int source, int value, int max)
	{
		if (m_LordGoldText != null)
		{
			m_LordGoldText.text = "Gold " + value;
		}
	}

	private void SetCerealText(int source, int value, int max)
	{
		m_CerealAmountText.text = "Cereal " + source;
	}

	private void SetFishText(int source, int value, int max)
	{
		m_FishAmountText.text = "Fish " + source;
	}

	private void SetMeatText(int source, int value, int max)
	{
		m_MeatAmountText.text = "Meat " + source;
	}

	private void SetMetalText(int source, int value, int max)
	{
		m_MetalAmountText.text = "Metal " + source;
	}

	public void SetGameEnd(bool isWin, string text)
	{
		m_EndGamePanel.SetActive(true);
		m_WinPanel.SetActive(isWin);
		m_LosePanel.SetActive(!isWin);
		m_BonusGoldText.gameObject.SetActive(isWin);
		m_BonusGoldText.text = "Bonus : " + text;
	}

	#endregion

}
