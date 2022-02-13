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

    [SerializeField]
    private ParticleSystem cutVFX;

    private Transform _transform;

    private Vector3 startPos;

    private bool isPlaying, isMoved;

    private float layerDistance;

    private float cutPerct;
    public float CutPerct { get { return cutPerct; } }

    private int layerCounter = 1;
    
    private void OnEnable()
    {
        EventManager.OnLevelStart.AddListener( () => isPlaying = true );
        EventManager.OnLevelFinish.AddListener( () => isPlaying = false );
    }

    private void OnDisable()
    {
        EventManager.OnLevelStart.RemoveListener( () => isPlaying = true );
        EventManager.OnLevelFinish.RemoveListener( () => isPlaying = false );
    }  

    void Start()
    {
        _transform = this.transform;
        isPlaying = true;
        isMoved = false;
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
        if (!isPlaying || !Input.GetMouseButton(0))
        {
            cutVFX.Stop();
            return;
        }

        if (Input.GetMouseButton(0))
        {
            cutVFX.Play();
            _transform.position += Vector3.back * cutterMovementSpeed * Time.deltaTime;
            cutPerct = Mathf.Abs((movementBorder - _transform.position.z) / layerDistance);

            if (_transform.position.z <= movementBorder)
            {
                // MoveForNewLayer();
                // LeanTween.move(this.gameObject, startPos + Vector3.down * layerThickness, 0.5f);
                _transform.position = startPos + Vector3.down * layerThickness;
                startPos = _transform.position;
                layerCounter++;
                EventManager.OnSoapLayerCompleted?.Invoke();
            }
        }
    }

    private void MoveForNewLayer()
    {
        if (isMoved) return;

        isMoved = true;
        LeanTween.move(this.gameObject, startPos + Vector3.down * layerThickness, 0.25f);
    }
}
