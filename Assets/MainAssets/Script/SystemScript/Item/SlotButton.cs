using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotButton : MonoBehaviour,
    IPointerClickHandler,
    IPointerDownHandler,
    IPointerUpHandler
{
    [SerializeField] GameObject itemKanriObject;
    [SerializeField] GameObject itemSellObject;

    ItemKanri itemscript;
    ItemSellExplanation itemSellExplanation;

    void Start()
    {
        itemscript = itemKanriObject.GetComponent<ItemKanri>();
        itemSellExplanation = itemSellObject.GetComponent<ItemSellExplanation>();
    }

    // ����  
    public void OnPointerClick(PointerEventData eventData)
    {
        itemscript.slotkoushin();
        itemSellExplanation.slotkoushin();
    }

    // �����ꂽ�܂�
    public void OnPointerDown(PointerEventData eventData)
    {

    }

    // ������������� 
    public void OnPointerUp(PointerEventData eventData)
    {

    }
}
