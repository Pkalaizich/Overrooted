using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryTable : MonoBehaviour
{
    public GameController gc;
    [SerializeField] private AudioClip correctSFX;
    [SerializeField] private List<AudioClip> bottles;
    private void Start()
    {
        gc = GameController.Instance;
    }
    public void Deliver(RecipeScriptable deliveredRecipe)
    {
        if (deliveredRecipe == null)
        {
            //Debug.Log("NO TENES NADA ARMADO");
            return;
        }
            
        for (int i =0; i<gc.activeRecipes.Count; i++)
        {
            if (deliveredRecipe.name == gc.activeRecipes[i].name)
            {
                //Debug.Log("EXITO");
                int aux1 = Random.Range(0, bottles.Count);
                AudioEvents.Instance.PlaySound(bottles[aux1]);
                AudioEvents.Instance.Delayed(correctSFX);
                gc.RemoveOrder(i, true);
                Player.Instance.DropCurrent();
                SendSuccesMessage();
                //UIController.Instance.ChangeCurrentObjectSprite(null);
                return;
            }
        }        
        Debug.Log("ERRASTE");
        int aux2 = Random.Range(0, bottles.Count);
        AudioEvents.Instance.PlaySound(bottles[aux2]);
        Player.Instance.DropCurrent();
        AudioEvents.Instance.Delayed(AudioEvents.Instance.wrongSFX);
        FindObjectOfType<CameraShake>().shakeDuration = 0.2f;        
        gc.ChangeScore(-gc.baseScore/4);
    }
    

    public void SendSuccesMessage()
    {
        gc.ShowDeliveredDialog();
    }


}
