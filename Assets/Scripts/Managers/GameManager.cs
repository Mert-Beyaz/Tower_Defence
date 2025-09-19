using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int HpAmount = 5;
    public SkillType Skill = SkillType.YellowProjectile;
    [SerializeField] private GameObject levelHolder;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Time.timeScale = 0f;
        UIManager.Instance.Init();
    }

    public void StartLevel()
    {
        Time.timeScale = 1f;
        levelHolder.SetActive(true);
    }    
    
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        HpAmount = 5;
        levelHolder.SetActive(true);
        UIManager.Instance.Reset();
    }

    public void Win()
    {
        Time.timeScale = 0f;
        levelHolder.SetActive(false);
        UIManager.Instance.Win();
    }

    public void Lose()
    {
        Time.timeScale = 0f;
        levelHolder.SetActive(false);
        SoundManager.Instance.StopLoop("Walk");
        UIManager.Instance.Lose();
    }
}
