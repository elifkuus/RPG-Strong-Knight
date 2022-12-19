using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Move Settings")]
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float turnSpeed = 15f;

    private Animator anim;
    private CharacterController controller;

    private CollisionFlags collisionFlags = CollisionFlags.None; //collision'a nereden temas oldugu

    private Vector3 playerMove = Vector3.zero;
    private Vector3 targetMovePoint = Vector3.zero;

    private float currentSpeed; //suanki hizimiz
    private float playerToPointDistance;
    private float gravity = 9.8f;
    private float height;

    private bool canMove;
    private bool finishedMovement = true;
    private Vector3 newMovePoint;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        currentSpeed = maxSpeed;
    }


    void Update()
    {
        CalculateHeight();
        CheckIfFinishedMovement();
    }

    private bool IsGrounded()
    {
        return collisionFlags == CollisionFlags.CollidedBelow ? true : false;
    }
    private void CalculateHeight()
    {
        if (IsGrounded())
        {
            height = 0f;
        }
        else
        {
            height -= gravity * Time.deltaTime;
        }
    }
    private void CheckIfFinishedMovement()
    {
        if (!finishedMovement)
        {
            if (!anim.IsInTransition(0) && !anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
            //Base layerda gecis durumunda degilse ve ismi idle degilse ve anim oynatma suresi 0.8i gecince
            {
                finishedMovement = true;
            }
        }
        else
        {
            MovePlayer();
            playerMove.y = height * Time.deltaTime;
            collisionFlags = controller.Move(playerMove);
        }
    }
    private void MovePlayer()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                playerToPointDistance = Vector3.Distance(transform.position, hit.point);
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    if (playerToPointDistance >= 1.0f)
                    {
                        canMove = true;
                        targetMovePoint = hit.point;
                    }
                }
            }

        }
        if (canMove)
        {
            anim.SetFloat("Speed", 1.0f); //Run

            newMovePoint = new Vector3(targetMovePoint.x, transform.position.y, targetMovePoint.z);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(newMovePoint - transform.position), turnSpeed * Time.deltaTime);

            playerMove = transform.forward * currentSpeed * Time.deltaTime;

            if (Vector3.Distance(transform.position, newMovePoint) <= 0.3f)
            {
                canMove = false;
            }
        }
        else
        {
            playerMove.Set(0f, 0f, 0f); //Vector3.zero ile ayni
            anim.SetFloat("Speed", 0f);
        }
    }
}
