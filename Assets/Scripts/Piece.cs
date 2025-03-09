using System.Collections.Generic;
using UnityEngine;


public class Piece : MonoBehaviour
{
    bool held = false;
    bool correct = false;
    Vector3 mousePos;
    [SerializeField] private List<Vector3> correctPositions = new List<Vector3>();
    [SerializeField] private List<Quaternion> correctRotations = new List<Quaternion>();
    [SerializeField] private float snapZone; 
    [SerializeField] private float rotationThreshold = 5f;
    private float HeldYpos = .1f;
    private float releaseYpos = 0f;
    private Renderer objectRenderer;
    [SerializeField] private AudioClip popClip;
    [SerializeField] private AudioClip bumpClip;

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();

    }

    private void Update()
    {
        //rotates 90 degrees when right click and being held
        if (Input.GetMouseButtonDown(1) && held)
        {
            transform.Rotate(0, 45, 0);
            CenterObject();
        }
    }

    private Vector3 GetMousePos()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    //picks up object
    private void OnMouseDown()
    {
        if (!correct)
        {
            held = true;
            mousePos = Input.mousePosition - GetMousePos();
            Cursor.instance.toGrab();
        }
    }

    //when object dropped and places then on the floor
    private void OnMouseUp()
    {
        Cursor.instance.toIdle();
        held = false;
        transform.position = new Vector3(transform.position.x, releaseYpos, transform.position.z);

      
        for (int i = 0; i < correctPositions.Count; i++)
        {
            float positionDistance = Vector3.Distance(transform.position, correctPositions[i]);
            float rotationDifference = Quaternion.Angle(transform.rotation, correctRotations[i]);

            if (IsPositionOccupied(correctPositions[i]))
            {
                transform.position=new Vector3(transform.position.x, 0.1f, transform.position.z);
                break;
            }

            if (!correct && positionDistance <= snapZone && rotationDifference <= 180 && CompareTag("parrall"))
            {
                SnapToCorrectPosition(correctPositions[i], correctRotations[i]);
                break;
            }
            else if (!correct && positionDistance <= snapZone && rotationDifference <= rotationThreshold)
            {
                SnapToCorrectPosition(correctPositions[i], correctRotations[i]);
                break;
            }
        }
        SoundFXManager.instance.playSoundFX(bumpClip, transform, .5f);
    }

    //object follows cursor
    private void OnMouseDrag()
    {
        if (!correct)
        {
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePos);
            newPosition.y = HeldYpos;
            transform.position = newPosition;
            CenterObject();
        }
    }

    //when rotated object stays under cursor
    private void CenterObject()
    {
        if (objectRenderer == null) return;
        Bounds bounds = objectRenderer.bounds;
        Vector3 cursorWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y - HeldYpos));
        cursorWorldPos.y = HeldYpos; 
        Vector3 centerOffset = bounds.center - transform.position;
        transform.position = cursorWorldPos - centerOffset;
    }

    //will snap piece to the correct postion if it is near
    private void SnapToCorrectPosition(Vector3 pos, Quaternion rot)
    {
        
        transform.position = pos; 
        transform.rotation = rot;

        
        Debug.Log($"Snapped to position: {transform.position}");
        Debug.Log($"Snapped to rotation: {transform.rotation.eulerAngles}");

        correct = true; 
        SoundFXManager.instance.playSoundFX(popClip, transform, 1f);

        PuzzleManager.instance.addTocorrect();
        PuzzleManager.instance.checkIfComplet();
    }

    //make sure that peices can't over lap
    private bool IsPositionOccupied(Vector3 position)
    {
        float checkRadius = 0.05f; 
        Collider[] colliders = Physics.OverlapSphere(position, checkRadius);

        foreach (Collider col in colliders)
        {
            if (col.gameObject != gameObject ) 
            {
                float detectedDistance = Vector3.Distance(col.transform.position, position);
                Debug.Log($"Detected {col.gameObject.name} at distance {detectedDistance}");

                if (detectedDistance < 0.05f) 
                {
                    return true; 
                }
            }
        }

        return false; 
    }
}


