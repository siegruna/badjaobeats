using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Sprite defaultSprite;
    [SerializeField] Sprite hoverSprite;
    [SerializeField] Image background;
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        background.sprite = hoverSprite;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        background.sprite = defaultSprite;
    }
}
