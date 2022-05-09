using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Rigidbody2D))]
[RequireComponent(typeof (Player))]
public class PlayerControler : EntityController
{
    public Player player;

    public Animator playerAnimator;

    float input_x = 0;

    float input_y = 0;

    bool isWalking = false;

    // public bool allowMoviment = true;
    Rigidbody2D rb2D;

    Vector2 moviment = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        isWalking = false;
        rb2D = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!allowMoviment) return;

        input_x = Input.GetAxisRaw("Horizontal");
        input_y = Input.GetAxisRaw("Vertical");
        isWalking = (input_x != 0 || input_y != 0);
        moviment = new Vector2(input_x, input_y);

        if (isWalking)
        {
            playerAnimator.SetFloat("input_x", input_x);
            playerAnimator.SetFloat("input_y", input_y);
        }

        playerAnimator.SetBool("isWalking", isWalking);

        if (Input.GetButtonDown("Fire1")) playerAnimator.SetTrigger("attack");
    }

    private void FixedUpdate()
    {
        rb2D
            .MovePosition(rb2D.position +
            moviment * player.entity.speed * Time.fixedDeltaTime);
    }
}