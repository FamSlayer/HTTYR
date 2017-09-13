using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMove : MonoBehaviour {

    public float acceleration;
    public float max_speed;



    Vector2 direction = Vector2.left;
    Rigidbody2D rb;
    LayerMask mask;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mask = ~LayerMask.NameToLayer("UI");
    }

    // Update is called once per frame
    void Update ()
    {
        if(rb.velocity.magnitude < max_speed)
        {
            rb.AddForce(direction * acceleration, ForceMode2D.Force);
        }

        if(PathInDir(transform.up))
        {
            //print("boom chika bow wow");
        }
	}


    bool PathInDir(Vector3 dir)
    {
        float distance = 20f;
        Debug.DrawLine(transform.position, transform.position + dir * distance, Color.magenta);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, distance);//, mask);
        if( hit.collider != null )
        {
            //print("We hit something! " + hit.collider.gameObject.name);
            return false;
        }
        else
        {
            //print("We didn't hit anything!");
            return true;
        }
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("We hit a trigger!");
        DialogueTrigger dt = collision.gameObject.GetComponent<DialogueTrigger>();
        if(dt != null)
        {
            if( !dt.activated )
                dt.Activate();
        }
        else
        {
            print("It was not a dialogue trigger :(");
        }

    }

    public void ElectricShockTherapy()
    {
        print("Oof! Ouch! My bones!");
    }
}
