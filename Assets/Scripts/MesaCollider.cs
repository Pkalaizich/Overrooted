using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MesaCollider : MonoBehaviour
{
    Player player;
    private float timeToInteract = 0.5f;
    private float lastInteraction;
    private MaterialMixer mixer;
    [SerializeField] private GameObject mixerContent;

    private void Start()
    {
        player = Player.Instance;
        lastInteraction = Time.time;
        mixer = FindObjectOfType<MaterialMixer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" )
        {
            player.canDrop = false;
            mixer.AnimSlot();
        }
        if(Player.Instance.holdingMaterial !=null)
        {
            mixer.Aid1(true);            
        }
        if(mixer.materialsInMixer.Count!=0)
        {
            mixer.Aid2(true);
            //mixerContent.SetActive(true);
        }
        if(mixer.materialsInMixer.Count == mixer.spritesInMixer.Count)
        {
            mixer.Aid3(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(Time.time - lastInteraction > timeToInteract)
        {
            if (Input.GetKey(KeyCode.J) && player.holdingMaterial != null)
            {
                mixer.AddMaterialToMixer(player.holdingMaterial.materialIndex, player.holdingMaterial.materialIcon);
                player.DropCurrent();
                lastInteraction= Time.time;
                return;
            }            
        }
        if (Input.GetKey(KeyCode.L)&& mixer.materialsInMixer.Count!=0)
        {
            mixer.Mix();
        }
        if(Input.GetKey(KeyCode.I))
        {
            mixer.ClearMixer();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        player.canDrop = true;
        //mixerContent.SetActive(false);
        mixer.AnimSlot();
        mixer.Aid1(false);
        mixer.Aid2(false);
        mixer.Aid3(false);
    }
}
