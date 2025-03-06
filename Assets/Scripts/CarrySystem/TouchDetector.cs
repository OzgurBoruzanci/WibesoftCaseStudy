using UnityEngine;

public class TouchDetector : MonoBehaviour
{
    private CarryController selectedCarry;
    private float holdTime = 0f;
    private float holdThreshold = 2f;
    private bool isHolding = false;
    private bool isTouching = false;
    public bool IsCreateField { get; private set; }
    public static TouchDetector Instance;
    public RaycastHit hit;
    public Camera camera;

    void Start()
    {
        Instance = this;
    }

    void Update()
    {
        if (Input.touchCount > 0 || Input.GetMouseButton(0))
        {
            holdTime += Time.deltaTime;
        }
        else
        {
            holdTime = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            IsCreateField = true;
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            IsCreateField = false;
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            HandleTouchInput(touch);
        }
        else if (Input.GetMouseButton(0))
        {
            HandleMouseInput();
        }

        if (isHolding && selectedCarry != null)
        {
            selectedCarry.MoveWithTouch();
            selectedCarry.MoveWithMouse();
        }

        if ((Input.GetMouseButtonUp(0) || isTouching) && isHolding)
        {
            isHolding = false;
            selectedCarry = null;
            isTouching = false;
        }

        PerformRaycast();
    }

    private void HandleMouseInput()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent<CarryController>(out var carry))
            {
                if (holdTime > holdThreshold)
                {
                    isHolding = true;
                    selectedCarry = carry;
                    selectedCarry.StartCarrying();
                }
            }
        }
        else
        {
            selectedCarry = null;
            isHolding = false;
            holdTime = 0f;
        }
    }

    private void HandleTouchInput(Touch touch)
    {
        if (touch.phase == TouchPhase.Began)
        {
            isTouching = true;
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent<CarryController>(out var carry))
                {
                    if (holdTime > holdThreshold)
                    {
                        isHolding = true;
                        selectedCarry = carry;
                        selectedCarry.StartCarrying();
                    }
                }
            }
            else
            {
                selectedCarry = null;
                holdTime = 0f;
            }
        }
        else if (touch.phase == TouchPhase.Ended)
        {
            isTouching = false;
        }
    }

    private void PerformRaycast()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.TryGetComponent<IHandler>(out var handler))
            {
                handler.HandleRayInteraction(hit);
            }
        }
    }
}
