using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCutterMovementController : MonoBehaviour
{
    [SerializeField]
    private float cutterMovementSpeed;

    [SerializeField]
    private float layerThickness;

    [SerializeField]
    private float movementBorder = -1.1f;

    private Transform _transform;

    private Vector3 startPos;

    private bool isPlaying;

    private float layerDistance, cutPerct;

    private int layerCounter = 1;

    void Start()
    {
        _transform = this.transform;
        isPlaying = true;
        startPos = _transform.position;
        layerDistance = (movementBorder - startPos.z);
        cutPerct = Mathf.Abs((movementBorder - _transform.position.z)/ layerDistance);
    }

    void Update()
    {
        MoveCutter();
    }

    private void MoveCutter()
    {
        if (!isPlaying) return;

        if (Input.GetMouseButton(0))
        {
            _transform.position += Vector3.back * cutterMovementSpeed * Time.deltaTime;
            cutPerct = Mathf.Abs((movementBorder - _transform.position.z) / layerDistance);

            if (_transform.position.z <= movementBorder)
            {
                _transform.position = startPos + Vector3.down * layerThickness;
                startPos = _transform.position;
                layerCounter++;

                if (layerCounter > 3)
                {
                    Debug.Log("Game is finished!");
                    // todo level finish event
                }
            }
        }
    }
}
