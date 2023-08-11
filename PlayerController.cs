using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speedMultiplier;
    public float normalSpeed = 700f;
    public float RotationSpeed = 10f;
    private Rigidbody rb;
    private Vector2 Direction;
    private float acceleration;
    private bool isMobile;

    [SerializeField] private Animator anim;
    Vector2 initialPos;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        speedMultiplier = normalSpeed;
        isMobile = Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (isMobile)
        {
            HandleMobileInput();
        }
        else
        {
            HandleKeyboardInput();
        }

    }

    private void HandleMobileInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            initialPos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            Accelerate();
            Direction = ((Vector2)(Input.mousePosition) - initialPos);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            initialPos = Vector2.zero;
        }
       
        else if (acceleration > 0)
        {
            Deaccelerate();
        }
    }

    private void HandleKeyboardInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f)
        {
            Direction = new Vector2(horizontalInput, verticalInput);
            Accelerate();
        }
        else if (acceleration > 0)
        {
            Deaccelerate();
        }
    }

    private void Accelerate()
    {
        acceleration += Time.deltaTime * 25f;
    }

    private void Deaccelerate()
    {
        acceleration -= Time.deltaTime * 30f;
        acceleration = Mathf.Max(acceleration, 0f);
    }

    private void FixedUpdate()
    {

        if (Direction.magnitude >= 0.1f)
        {
            Vector3 direction = new Vector3(Direction.x, 0, Direction.y);
            rb.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), RotationSpeed * Time.deltaTime);
            acceleration = Mathf.Clamp01(acceleration);
            Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // Extract the horizontal velocity
            Vector3 newVelocity = horizontalVelocity + transform.forward * speedMultiplier * acceleration * Time.fixedDeltaTime;
            rb.velocity = new Vector3(newVelocity.x, rb.velocity.y, newVelocity.z);
            anim.SetFloat("Speed", acceleration);
        }
    }

   
}
