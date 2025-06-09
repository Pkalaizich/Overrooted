using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    private static UIController instance;
    public static UIController Instance { get => instance; }

    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI holdingItemAid;

    [Header("Active Orders Variables")]
    [SerializeField] private List<Image> activeOrders;
    [SerializeField] private List<Image> orderTimers;
    [SerializeField] private List<Animator> ordersAnimators;
    [SerializeField] private Color plentyTimeColor;
    [SerializeField] private Color attentionTimeColor;
    [SerializeField] private Color hurryTimeColor;

    [Header("Current Object Variables")]
    [SerializeField] private Image currentObject;
    [SerializeField] private Sprite Empty;

    [Header("Final Panel Variables")]
    [SerializeField] private GameObject endPanel;
    [SerializeField] private TextMeshProUGUI finalScore;
    [SerializeField] private TMP_InputField nameField;
    [SerializeField] private Button endButton;

    [Header("Dialog Panel")]
    [SerializeField] private GameObject dialogPanel;
    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private TextMeshProUGUI dialogContent;
    [SerializeField] private float readTime;

    private GameController gc;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        endButton.interactable = false;
        endButton.onClick.AddListener(() =>
        {
            HighScoreManager.instance.NewHighScore(GameController.Instance.SCORE,nameField.text);
            GameController.Instance.RestartGame();
        });
        nameField.onValueChanged.AddListener(delegate
        {
            if(nameField.text =="")
            {
                endButton.interactable = false;
            }
            else
            {
                endButton.interactable = true;
            }
        });
    }

    private void Start()
    {
        gc = GameController.Instance;        
        RefreshOrdersUI();
    }

    public void SetTimer(float timeLeft)
    {
        int minutes = Mathf.FloorToInt(timeLeft / 60);
        int seconds = Mathf.FloorToInt(timeLeft-minutes*60);
        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void SetScore(int points)
    {
        score.text = points.ToString("000000");
    }

    public void RefreshOrdersUI()
    {
        for(int i =0; i < activeOrders.Count; i++) 
        {
            if(gc.activeRecipes.Count>i)
            {                
                activeOrders[i].sprite = gc.activeRecipes[i].recipeIcon;
                activeOrders[i].gameObject.SetActive(true);
            }
            else
            {
                ordersAnimators[i].SetTrigger("ToIdle");
                activeOrders[i].transform.localScale= Vector3.one;
                activeOrders[i].transform.localRotation = Quaternion.identity;
                activeOrders[i].gameObject.SetActive(false);
                
            }
        }
    }

    public void RefreshOrderTimer()
    {
        for (int i = 0; i < activeOrders.Count; i++)
        {
            if (gc.activeRecipes.Count > i)
            {                
                orderTimers[i].color = plentyTimeColor;
                float scale = Mathf.Clamp(gc.orderTimer[i] / gc.orderDuration,0,1);
                if(scale <0.5)
                {
                    orderTimers[i].color = attentionTimeColor;
                }
                if(scale <0.3)
                {
                    gc.showHurryMessage(i);
                    orderTimers[i].color = hurryTimeColor;
                    ordersAnimators[i].SetTrigger("ToShaking");
                }
                else
                {
                    ordersAnimators[i].SetTrigger("ToIdle");
                }
                orderTimers[i].transform.localScale = new Vector3(scale, 1, 1);
                activeOrders[i].gameObject.SetActive(true);
            }            
        }
    }

    public void SetFinalPanel(int score)
    {
        endPanel.SetActive(true);
        finalScore.text = score.ToString("000000");
    }

    public void ChangeCurrentObjectSprite(Sprite icon)
    {
        if(icon == null)
        {
            currentObject.sprite = Empty;
            holdingItemAid.gameObject.SetActive(false);

        }
        else
        {
            currentObject.sprite = icon;
            holdingItemAid.gameObject.SetActive(true);
        }        
    }

    public void SetDialogPanel(string character, string dialog)
    {
        if(!dialogPanel.gameObject.activeSelf)
        {
            characterName.text = character;
            dialogContent.text = dialog;
            StartCoroutine(ShowDialog());
        }
    }

    private IEnumerator ShowDialog()
    {
        dialogPanel.gameObject.SetActive(true);
        yield return new WaitForSeconds(readTime);
        dialogPanel.gameObject.SetActive(false);

    }
}
