using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Menu")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private Button startButton;

    [Header("Ingame")]
    [SerializeField] private GameObject ingamePanel;

    [Header("Wave UIs")]
    [SerializeField] private TextMeshProUGUI waveHolderText;
    [SerializeField] private TextMeshProUGUI waveInfoText;

    [Header("HP UIs")]
    [SerializeField] private TextMeshProUGUI hpAmountText;

    [Header("Player HP UIs")]
    [SerializeField] private Image playerHPFillBar;
    [SerializeField] private Image playerHPAnimBar;    
    
    [Header("Skill UIs")]
    [SerializeField] private Image blueSkill;
    [SerializeField] private Image yellowSkill;

    [Header("WinPanel")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private Button playAgainWinPanelButton;

    [Header("LosePanel")]
    [SerializeField] private GameObject losePanel;
    [SerializeField] private Button playAgainLosePanelButton;

    private Tween _waveTween;
    private Color _color;

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

    public void Init()
    {
        menuPanel.SetActive(true);
        ingamePanel.SetActive(false);
        startButton.onClick.AddListener(OnClickStartButton);
        playAgainWinPanelButton.onClick.AddListener(OnClickPlayAgainButton);
        playAgainLosePanelButton.onClick.AddListener(OnClickPlayAgainButton);
    }

    public void SetWaveText()
    {
        _waveTween?.Kill();
        waveInfoText.SetText("WAVE " + (WaveController.Instance.WaveCounter + 1));
        _waveTween = waveInfoText.DOFade(1, 2f).SetLoops(2, LoopType.Yoyo);
        waveHolderText.SetText("WAVE " + (WaveController.Instance.WaveCounter + 1));
        _color = waveInfoText.color;
        _color.a = 0f;
        waveInfoText.color = _color;
    }

    public void SetHPText()
    {
        if (GameManager.Instance.HpAmount < 0) return;
        GameManager.Instance.HpAmount--;
        hpAmountText.SetText(GameManager.Instance.HpAmount.ToString());
        if (GameManager.Instance.HpAmount.Equals(0))
        {
            GameManager.Instance.Lose();
        }
    }

    public void SetPlayerHPUI(float _maxHP, float _currentlyHP)
    {
        playerHPFillBar.fillAmount = _currentlyHP / _maxHP;
        playerHPAnimBar.DOFillAmount(_currentlyHP / _maxHP, 1f).OnComplete(() =>
        {
            if (_currentlyHP <= 0)
            {
                GameManager.Instance.Lose();
            }
        });
    }

    private void OnClickStartButton()
    {
        menuPanel.SetActive(false);
        ingamePanel.SetActive(true);
        SetSkillUI();
        GameManager.Instance.StartLevel();
    }

    private void OnClickPlayAgainButton()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        GameManager.Instance.RestartLevel();
    }

    public void SetSkillUI()
    {
        switch (GameManager.Instance.Skill)
        {
            case SkillType.BlueProjectile:
                blueSkill.DOFade(1, .1f);
                yellowSkill.DOFade(.1f, .1f);
                break;

            case SkillType.YellowProjectile:
                blueSkill.DOFade(.1f, .1f);
                yellowSkill.DOFade(1, .1f);
                break;
        }
    }

    public void Reset()
    {
        hpAmountText.SetText(GameManager.Instance.HpAmount.ToString());
        waveHolderText.SetText("WAVE " + (WaveController.Instance.WaveCounter));
    }

    public void Win()
    {
        winPanel.SetActive(true);
    }

    public void Lose()
    {
        losePanel.SetActive(true);
    }

    private void OnDestroy()
    {
        startButton.onClick.RemoveListener(OnClickStartButton);
        playAgainWinPanelButton.onClick.RemoveListener(OnClickPlayAgainButton);
        playAgainLosePanelButton.onClick.RemoveListener(OnClickPlayAgainButton);
    }
}
