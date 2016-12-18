using UnityEngine;
using System.Collections;

public class Dragable : MonoBehaviour {
	TouchPiece dragablePiece;
	GameObject dragableObject;
	GameObject rotateObject;
    public bool isLocked = false;
	int layerMask = (1 << 10);

	void Start() {
		layerMask = ~layerMask;
	}

	// Update is called once per frame
	void Update () {
		#if UNITY_EDITOR

		if (Input.GetMouseButtonDown(0)) {

			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero, Mathf.Infinity, layerMask);

			if(hit) {
				rotateObject = hit.collider.gameObject;
				if(gameObject == rotateObject && rotateObject.name == "center" && !isLocked) {
					dragablePiece = hit.collider.gameObject.transform.parent.GetComponent<TouchPiece>();
					dragableObject = hit.collider.gameObject.transform.parent.gameObject;
					dragablePiece.DragPiece(dragableObject);
				}
			}
		}
		if (Input.GetMouseButton(0)) {
            if (gameObject == rotateObject && dragablePiece && rotateObject && !isLocked)
				dragablePiece.ContinueDragging(dragableObject);
		}
		
		if (Input.GetMouseButtonUp(0)) {

			if(gameObject == rotateObject && dragablePiece && rotateObject && !isLocked) {
				dragablePiece.StopDragging(dragableObject);
				rotateObject = null;
			}
		}

		#endif

		if (Input.touchCount > 0) {
			Touch touch = Input.GetTouch (0);
			switch (touch.phase) {
				
			case TouchPhase.Began:
				RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero, Mathf.Infinity, layerMask);

				if (hit) {
					rotateObject = hit.collider.gameObject;
					if (gameObject == rotateObject && rotateObject.name == "center" && !isLocked) {
						dragablePiece = hit.collider.gameObject.transform.parent.GetComponent<TouchPiece> ();
						dragableObject = dragablePiece.gameObject;
						dragablePiece.AndrDragPiece (dragableObject, touch);
					}
				}
				break;
			case TouchPhase.Moved:
                if (gameObject == rotateObject && dragablePiece && rotateObject && !isLocked)
					dragablePiece.AndrContinueDragging (dragableObject, touch);

				break;
			case TouchPhase.Ended:
				if (gameObject == rotateObject && dragablePiece && rotateObject && !isLocked) {
					dragablePiece.AndrStopDragging (dragableObject, touch);
					rotateObject = null;
				}
				break;
			}
		}
	}
}
