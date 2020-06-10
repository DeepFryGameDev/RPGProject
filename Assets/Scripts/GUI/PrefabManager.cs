using UnityEngine;

public class PrefabManager : MonoBehaviour //fixes a prefab destroyed bug (when clearing lists)
{
    // Assign the prefab in the inspector
    public GameObject itemPrefab; //for the items in item list in battle, menu, and shop
    public GameObject keyItemPrefab;
    public GameObject magicPrefab; //for magic listed in menu
    public GameObject equipPrefab;
    public GameObject damagePrefab;
    public GameObject itemVictoryPrefab;

    public GameObject shopBuyItemPrefab;
    public GameObject shopSellItemPrefab;
    public GameObject shopBuyEquipPrefab;
    public GameObject shopSellEquipPrefab;

    public GameObject inactiveHeroButton;

    public GameObject activeQuestListButton;

    public GameObject bestiaryEntryButton;

    public GameObject battleActionButton;
    public GameObject battleMagicButton;
    public GameObject battleItemButton;

    //Singleton
    private static PrefabManager m_Instance = null;
    public static PrefabManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = (PrefabManager)FindObjectOfType(typeof(PrefabManager));
            }
            return m_Instance;
        }
    }
}