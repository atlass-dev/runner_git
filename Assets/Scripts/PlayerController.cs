using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private CapsuleCollider col;
    private Animator anim;
    private Vector3 dir;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravity;
    [SerializeField] private int coins;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private Text coinsText;
    [SerializeField] private Score scoreScript;

    private int lineToMove = 1;
    public float lineDistance = 4; 
    private float maxSpeed = 110;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        col = GetComponent<CapsuleCollider>();
        Time.timeScale = 1;
        coins = PlayerPrefs.GetInt("coins");
        coinsText.text = coins.ToString();
        StartCoroutine(SpeedIncrease());
    }

    private void Update()
    {
        if (SwipeController.swipeRight)
        {
            if (lineToMove < 2)
                lineToMove++;
        }

        if (SwipeController.swipeLeft)
        {
            if (lineToMove > 0)
                lineToMove--;
        }

        if (SwipeController.swipeUp)
        {
            if (controller.isGrounded)
                Jump();
        }

        if (SwipeController.swipeDown)
        {
            StartCoroutine(Slide());
        }

        if (controller.isGrounded)
            anim.SetTrigger("isRunning");

        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
        if (lineToMove == 0)
            targetPosition += Vector3.left * lineDistance;
        else if (lineToMove == 2)
            targetPosition += Vector3.right * lineDistance;

        if (transform.position == targetPosition)
            return;
        Vector3 diff = targetPosition - transform.position;
        Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;
        if (moveDir.sqrMagnitude < diff.sqrMagnitude)
            controller.Move(moveDir);
        else
            controller.Move(diff);

        

    }

    private void Jump()
    {
        dir.y = jumpForce;
        anim.SetTrigger("isJumping");
    }

    void FixedUpdate()
    {
        dir.z = speed;
        dir.y += gravity * Time.fixedDeltaTime;
        controller.Move(dir * Time.fixedDeltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "obstacle")
        {
            losePanel.SetActive(true);
            int lastRunScore = int.Parse(scoreScript.scoreText.text.ToString());
            PlayerPrefs.SetInt("lastRunScore", lastRunScore);
            Time.timeScale = 0;
        }    
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "Coin")
        {
            coins++;
            PlayerPrefs.SetInt("coins", coins);
            coinsText.text = coins.ToString();
            Destroy(other.gameObject);
        }
    }

    private IEnumerator SpeedIncrease()
    {
        yield return new WaitForSeconds(1);
        if (speed < maxSpeed)
        {
            speed += 1;
            StartCoroutine(SpeedIncrease());
        }
    }

    private IEnumerator Slide()
    {
        col.center = new Vector3(0, -0.4f, 0);
        col.height = 2;

        yield return new WaitForSeconds(1);

        col.center = new Vector3(0, 0.8398392f, 0);
        col.height = 4.420422f;
    }
}
