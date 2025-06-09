using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialBoxCollider : MonoBehaviour
{
    [SerializeField] private MaterialScriptable materialToGive;
    private float timeToInteract = 0.5f;
    private float lastInteraction;
    [SerializeField] private AudioClip pickupSFX;
    [SerializeField] private GameObject aid;

    private void Start()
    {
        lastInteraction= Time.time;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && Player.Instance.holdingMaterial ==null && Player.Instance.holdingRecipe ==null)
        {
            aid.SetActive(true);
            Debug.Log("LISTO PARA AGARRAR");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {        
        if(Input.GetKey(KeyCode.J)&&(Time.time -lastInteraction)>timeToInteract)
        {
            FindObjectOfType<MaterialMixer>().AidHand();
             Player.Instance.GrabMaterial(materialToGive);
             AudioEvents.Instance.PlaySound(pickupSFX);
            aid.SetActive(false);
            lastInteraction = Time.time;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            aid.SetActive(false);
        }
    }
}
