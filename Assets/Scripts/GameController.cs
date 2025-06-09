using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private static GameController instance;
    public static GameController Instance { get => instance; }

    [SerializeField] public List<RecipeScriptable> recipes;
    [SerializeField] public List<RecipeScriptable> activeRecipes;
    [SerializeField] private List<GameObject> deliverPoints;
    [SerializeField] public List<float> orderTimer;
    [SerializeField] private float timeBetweenRecipes;
    private float requestTimer;
    [SerializeField] private float gameDuration;
    [SerializeField] public float orderDuration;
    private float timer;
    

    [SerializeField] public int baseScore;


    [SerializeField] private int score;
    public int SCORE => score;
    [SerializeField] private GameObject floatingPoints;
    [SerializeField] private int maxSimultaneousOrders =5;

    [Header("Dialogs")]
    [SerializeField] private List<string> charactersNames;
    [SerializeField] private List<string> currentCharacters;
    [SerializeField] private List<string> hurryPhrases;
    [SerializeField] private List<string> deliveredPhrases;
    private float lastHurryMessageTime;

    public bool Ongoing { get; private set; }

    private UIController ui;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        timer = gameDuration;
        lastHurryMessageTime = Time.time;        
    }

    private void Start()
    {
        requestTimer = 2f;
        //RequestRecipe();
        ui = UIController.Instance;
        Ongoing= true;
        int i = Random.Range(0, deliverPoints.Count);
        deliverPoints[i].SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.F12))
        {
            RestartGame();
        }
        if (Ongoing)
        {
            timer = Mathf.Clamp(timer - Time.deltaTime, 0, gameDuration);
            requestTimer = Mathf.Clamp(requestTimer - Time.deltaTime, 0, timeBetweenRecipes);
            ui.SetTimer(timer);
            if(requestTimer<=0)
            {
                RequestRecipe();
            }
            if(activeRecipes.Count==0 && requestTimer>1)
            {
                requestTimer = 1;
            }
            if (timer <= 0)
            {
                Ongoing= false;
                ui.SetFinalPanel(score);                
            }
            for (int i = 0; i < orderTimer.Count; i++)
            {
                orderTimer[i] = Mathf.Clamp(orderTimer[i] - Time.deltaTime, 0, orderDuration);
                if (orderTimer[i] <= 0)
                {
                    RemoveOrder(i, false);
                }
            }
            ui.RefreshOrderTimer();            
        }               
    }

    public void RequestRecipe()
    {
        if(activeRecipes.Count < maxSimultaneousOrders)
        {
            int index = Random.Range(0, recipes.Count);
            RecipeScriptable rs = recipes[index];
            activeRecipes.Add(rs);
            orderTimer.Add(orderDuration);
            UIController.Instance.RefreshOrdersUI();
            ShowStartingDialog(rs);
        }
        else
        {
            Debug.Log("Ta lleno pa");
        }
        requestTimer = timeBetweenRecipes;
    }

    public void RemoveOrder(int i, bool delivered)
    {
        if (delivered)
        {
            int points = Mathf.RoundToInt(baseScore + (orderTimer[i] / orderDuration) * baseScore);
            ChangeScore(points);            
            //score += Mathf.RoundToInt(baseScore + (orderTimer[i] / orderDuration) * baseScore);
            //ui.SetScore(score);
        }
        else
        {
            AudioEvents.Instance.PlaySound(AudioEvents.Instance.wrongSFX);
            FindObjectOfType<CameraShake>().shakeDuration = 0.2f;
            ChangeScore(-baseScore/2);
            //score = Mathf.Clamp(score - baseScore, 0,score);
            //ui.SetScore(score);
        }
        int index = Random.Range(0, deliverPoints.Count);
        for (int j=0; j < deliverPoints.Count; j++)
        {
            if (j == index)
            {
                deliverPoints[j].SetActive(true);
            }
            else
            {
                deliverPoints[j].SetActive(false);
            }
        }
        activeRecipes.RemoveAt(i);
        orderTimer.RemoveAt(i);
        currentCharacters.RemoveAt(i);
        UIController.Instance.RefreshOrdersUI();        
    }

    public void ChangeScore(int points)
    {
        score = Mathf.Clamp(score + points, 0, 100000000);
        GameObject go = Instantiate(floatingPoints, Player.Instance.pointsSpawn.position,Quaternion.identity);
        if(points>0)
        {
            go.transform.GetChild(0).GetComponent<TextMesh>().text = "+" + Mathf.Abs(points).ToString();
            go.transform.GetChild(0).GetComponent<TextMesh>().color = Color.green;
        }
        else
        {
            go.transform.GetChild(0).GetComponent<TextMesh>().text = "-" + Mathf.Abs(points).ToString();
            go.transform.GetChild(0).GetComponent<TextMesh>().color = Color.red;
        }
        ui.SetScore(score);
    }

    public void ShowStartingDialog(RecipeScriptable recipe) 
    {
        int name = Random.Range(0,charactersNames.Count);
        int aux = Random.Range(0,recipe.startDialogs.Count);
        currentCharacters.Add(charactersNames[name]);
        ui.SetDialogPanel(charactersNames[name], recipe.startDialogs[aux]);
    }

    public void ShowDeliveredDialog()
    {
        int aux = Random.Range(0, deliveredPhrases.Count);
        ui.SetDialogPanel("Enepece", deliveredPhrases[aux]);
    }

    public void showHurryMessage(int i)
    {
        if(Time.time - lastHurryMessageTime > 6f)
        {
            int j = Random.Range(0, hurryPhrases.Count);
            ui.SetDialogPanel(currentCharacters[i], hurryPhrases[j]);
            lastHurryMessageTime= Time.time;
        }        
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
