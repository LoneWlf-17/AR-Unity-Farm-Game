using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovementController : MonoBehaviour
{
    private Vector3 previousPos;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        previousPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (previousPos.z > transform.position.z)
        {
            animator.SetBool("move", true);
        }
        else
        {
            animator.SetBool("move", false);
        }

        previousPos = transform.position;
    }
}
