﻿using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Droppable : MonoBehaviour, IDropHandler {

    public abstract void OnDrop(PointerEventData eventData);

    public CardNetworkData GetCardNetworkData(PointerEventData eventData) {
        return eventData.pointerDrag.GetComponent<CardNetworkData>();
    }

}
