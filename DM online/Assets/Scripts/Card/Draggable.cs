using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    private Transform parent;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private int index;

    private void Awake() {
        canvas = GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        canvas.sortingOrder += 1;
        canvasGroup.blocksRaycasts = false;
        index = transform.GetSiblingIndex();
        parent = transform.parent;
        transform.SetParent(parent.parent);
    }

    public void OnDrag(PointerEventData eventData) {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData) {
        canvas.sortingOrder -= 1;
        canvasGroup.blocksRaycasts = true;
        transform.SetParent(parent);
        transform.SetSiblingIndex(index);
    }

}
