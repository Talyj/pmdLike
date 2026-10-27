using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MinimapUI : MonoBehaviour
{
    [SerializeField] private Dictionary<string, Sprite> listSprite = new Dictionary<string, Sprite>();
    [SerializeField] private Image imageToChange;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var npc = GetComponentInParent<NpcController>();
        var player = GetComponentInParent<PlayerController>();
        if(npc != null)
        {
            if (npc.isCompanion)
            {
                imageToChange.sprite = listSprite["ally"];
            }
            else
            {
                imageToChange.sprite = listSprite["enemy"];
            }
        }
        else if(player != null)
        {
            imageToChange.sprite = listSprite["player"];
        }
        else
        {
            imageToChange.sprite = listSprite["goal"];
        }
    }
}
