using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverCollider : MonoBehaviour
{
    private DeliveryTable dt;
    private Player player;
    [SerializeField] private GameObject aid;
    [SerializeField] private GameObject aidHand;
    [SerializeField]private Transform spawnPoint;

    private void Start()
    {
        dt =FindObjectOfType<DeliveryTable>();
        player = FindObjectOfType<Player>();
        Debug.Log("algo");
    }
    private void OnEnable()
    {
        if(player == null)
        {
            player = FindObjectOfType<Player>();
        }        
            if (player.holdingRecipe != null)
            {
                aidHand.SetActive(true);
            }
            else
            {
                aidHand.SetActive(false);
            }
              
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(Player.Instance.holdingRecipe!=null)
        {
            aid.SetActive(true);
        }        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag =="Player")
        {
            if(Input.GetKey(KeyCode.J) && player.holdingRecipe!=null)
            {
                dt.Deliver(player.holdingRecipe);
                player.DropCurrent();
                //aidHand.SetActive(false);
                UIController.Instance.ChangeCurrentObjectSprite(null);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        aid.SetActive(false);
    }

    public void ActiveHand(bool active)
    {
        aidHand.SetActive(active);
    }
}
