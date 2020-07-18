using UnityEngine;

public class AttackPrefabManager : MonoBehaviour
{
    //Houses prefabs that need to be instantiated in UI

    // Assign the prefab in the inspector
    public GameObject magicCast;
    public GameObject fire;
    public GameObject slash;

    //Singleton
    private static AttackPrefabManager m_Instance = null;
    public static AttackPrefabManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = (AttackPrefabManager)FindObjectOfType(typeof(AttackPrefabManager));
            }
            return m_Instance;
        }
    }
}