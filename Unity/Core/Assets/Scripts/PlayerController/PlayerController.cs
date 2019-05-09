﻿using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    float moveSpeed = 4f;

    Vector3 forward, right;
    Vector3 lookPos;

    Animator anim;

    public Collider[] attackHitBoxes;

    bool runningBool;

    // Use this for initialization
    void Start () {
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            launchAttack(attackHitBoxes[0]);
        }
        if (Input.GetMouseButtonDown(1))
        {
            launchAttack(attackHitBoxes[1]);
        }
        if (Input.GetKeyUp(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            runningBool = false;
            movementAnimation();
        }
        else if (!Input.GetKey(KeyCode.W) && Input.GetKeyUp(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            runningBool = false;
            movementAnimation();
        }
        else if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && Input.GetKeyUp(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            runningBool = false;
            movementAnimation();
        }
        else if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && Input.GetKeyUp(KeyCode.D))
        {
            runningBool = false;
            movementAnimation();
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            Move();
            if (runningBool == false)
            {
                runningBool = true;
                movementAnimation();
            }
        }
    }
    void Move()
    {
        Vector3 direction = new Vector3(Input.GetAxis("HorizontalKey"), 0, Input.GetAxis("VerticalKey"));
        Vector3 rightMovement = right * moveSpeed * Time.deltaTime * Input.GetAxis("HorizontalKey");
        Vector3 upMovement = forward * moveSpeed * Time.deltaTime * Input.GetAxis("VerticalKey");

        Vector3 heading = Vector3.Normalize(rightMovement + upMovement);

        transform.forward = heading;
        transform.position += rightMovement;
        transform.position += upMovement;
    }

    private void launchAttack(Collider col)
    {
        anim.SetTrigger("Attack");
        var cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("HitBox"));
        foreach(Collider c in cols)
        {
            if (c.transform.parent.parent == transform)
            {
                continue;
            }
            int damage = 0;
            switch (col.name)
            {
                case "MeleeCollider":
                    damage = 2;
                    break;
                case "LongCollider":
                    damage = 3;
                    break;
                default:
                    Debug.Log("Collider Unknown");
                    break;
            }
            c.SendMessageUpwards("takeDamage",damage);
        }
    }

    private void movementAnimation()
    {
        if (runningBool == true)
        {
            anim.SetTrigger("Run");
        } else
        {
            anim.SetTrigger("Idle");
        }
    }

}