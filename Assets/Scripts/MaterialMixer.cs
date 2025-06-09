using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MaterialMixer : MonoBehaviour
{
    //[SerializeField] private int maxMaterialsInMixer;
    public List<int> materialsInMixer;
    public List<SpriteRenderer> spritesInMixer;
    [SerializeField] private Sprite emptySlot;
    [SerializeField] private Sprite normalTable;
    [SerializeField] private Sprite fireTable;
    private SpriteRenderer sr;

    [SerializeField] private List<AudioClip> machaques;
    [SerializeField] private AudioClip pick;

    [SerializeField] private GameObject aid1;
    [SerializeField] private GameObject aid2;
    [SerializeField] private GameObject aid3;
    [SerializeField] private GameObject aidCircle;
    [SerializeField] private GameObject aidHand;
    private bool materialLoaded =false;

    private Animator anim;

    public RecipeScriptable debugHoldingRecipe;

    //DEBUG LIST
    public List<MaterialScriptable> availableMaterials;

    private void Start()
    {
        sr =GetComponent<SpriteRenderer>();
        anim= GetComponent<Animator>();
    }
    public void Mix()
    {
        bool isSuccesful = false;
        foreach(RecipeScriptable recipe in GameController.Instance.recipes) 
        {
            if(recipe.requiredMaterials.Count==materialsInMixer.Count)
            {                
                isSuccesful = materialsInMixer.OrderBy(e => e).SequenceEqual(recipe.requiredMaterials);
                //isSuccesful = materialsInMixer.SequenceEqual(recipe.requiredMaterials);
                if(isSuccesful)
                {
                    Player.Instance.DropCurrent();
                    Player.Instance.GrabRecipe(recipe);
                    break;
                }                
            }            
        }
        if(isSuccesful )
        {            
            int aux = Random.Range(0,machaques.Count);
            AudioEvents.Instance.PlaySound(machaques[aux]);
            SuccessfulMix();
        }
        else
        {
            FailedMix();
        }
        //StartCoroutine(ChangeSprite());
        anim.SetTrigger("Fire");
        ClearMixer();
    }

    private void SuccessfulMix()
    {
        FindObjectOfType<DeliverCollider>().ActiveHand(true);
        //Debug.Log("Successfull");
    }

    private void FailedMix()
    {
        AudioEvents.Instance.PlaySound(AudioEvents.Instance.wrongSFX);
        FindObjectOfType<CameraShake>().shakeDuration = 0.2f;        
        //Debug.Log("Failed");
    }

    public void ClearMixer()
    {
        Aid3(false);
        AidCircle(false);
        materialsInMixer.Clear();
        Aid2(false);
        foreach(SpriteRenderer sr in spritesInMixer)
        {
            sr.sprite = emptySlot;
        }
    }

    public void DebugDeliver()
    {
        DeliveryTable dt = FindObjectOfType<DeliveryTable>();
        dt.Deliver(debugHoldingRecipe);
        debugHoldingRecipe = null;
        UIController.Instance.ChangeCurrentObjectSprite(null);
    }

    public void AddMaterialToMixer(int materialIndex, Sprite icon)
    {
        if(materialsInMixer.Count < spritesInMixer.Count)
        {
            aidHand.SetActive(false);
            Aid1(false);
            Aid2(true);
            materialsInMixer.Add(materialIndex);
            AudioEvents.Instance.PlaySound(pick);
            spritesInMixer[materialsInMixer.Count - 1].sprite = icon;
            if(materialsInMixer.Count == spritesInMixer.Count)
            {
                Aid3(true);
                AidCircle(true);
            }
        }
        else
        {
            Debug.Log("Mixer Lleno");
        }        
    }

    private IEnumerator ChangeSprite()
    {
        sr.sprite = fireTable;
        yield return new WaitForSeconds(0.5f);
        sr.sprite = normalTable;
    }

    public void Aid1(bool show)
    {
        aid1.gameObject.SetActive(show);
    }

    public void Aid2(bool show)
    {
        aid2.gameObject.SetActive(show);
    }

    public void Aid3(bool show)
    {
        aid3.gameObject.SetActive(show);
    }

    public void AidCircle(bool show)
    {
        aidCircle.gameObject.SetActive(show);
    }

    public void AidHand()
    {
        if(!materialLoaded) 
        {
            aidHand.gameObject.SetActive(true);
            materialLoaded = true;
        }        
    }

    public void AnimSlot()
    {
        anim.SetTrigger("ChangeSlot");
    }
}
