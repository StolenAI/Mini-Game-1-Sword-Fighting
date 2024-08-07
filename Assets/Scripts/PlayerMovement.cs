using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] GameObject Enemy;
    [SerializeField] Text VictoryText;
    [SerializeField] Text DefeatText;
    private const float walkingSpeed = 5.0f;
    private Animator characterAnimator;
    private Rigidbody rb;
    bool Dead = false;  
    bool Victory = false;
    private void Start()
    {
        VictoryText.enabled = false;
        DefeatText.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        characterAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Update()
    {
        if (!Victory && !Dead)
        {
            transform.LookAt(Enemy.transform.position);
            MovePlayer();
            AnimatePlayer();
            JumpPlayer();
        }

    }

    private void MovePlayer()
    {
        if (Input.GetKey(KeyCode.W))
            transform.Translate(Vector3.forward * walkingSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.S))
            transform.Translate(Vector3.back * walkingSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.A))
            transform.Translate(Vector3.left * walkingSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.D))
            transform.Translate(Vector3.right * walkingSpeed * Time.deltaTime);
    }

    private void AnimatePlayer()
    {
        bool Walking = Input.GetKey(KeyCode.W);
        characterAnimator.SetBool("Walking", Walking);

        bool WalkingBack = Input.GetKey(KeyCode.S);
        characterAnimator.SetBool("WalkingBack", WalkingBack);

        bool WalkingRight = Input.GetKey(KeyCode.D);
        characterAnimator.SetBool("WalkingRight", WalkingRight);

        bool WalkingLeft = Input.GetKey(KeyCode.A);
        characterAnimator.SetBool("WalkingLeft", WalkingLeft);

        bool Jumping = Input.GetKey(KeyCode.Space);
        characterAnimator.SetBool("Jumping", Jumping);
    }

    private void JumpPlayer()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            StartCoroutine(JumpDelay());
        }
    }

    private bool IsGrounded()
    {
        float rayDistance = 1.1f;
        return Physics.Raycast(transform.position, Vector3.down, rayDistance);
    }

    private IEnumerator JumpDelay()
    {
        yield return new WaitForSeconds(0.3f);
        rb.AddForce(new Vector3(0, 5, 0), ForceMode.Impulse);
    }

    private void EnemyIsDead()
    {
        Victory = true;
        characterAnimator.SetBool("Victory", true);
        VictoryText.enabled = true;

    }

    private void PlayerDied()
    {
        Dead = true;
        characterAnimator.SetBool("Dead", true);
        DefeatText.enabled = true;
        Enemy.SendMessage("PlayerIsDead");
    }
}
