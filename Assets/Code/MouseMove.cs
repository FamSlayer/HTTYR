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
        // draw a head
        Debug.DrawLine(transform.position, transform.position + transform.right * 35f, Color.magenta);
        if (rb.velocity.magnitude < max_speed)
        {
            rb.AddForce(direction * acceleration, ForceMode2D.Force);
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

        // make the mouse choose a new path semi-randomly
        // raycast forward, left, and right to find the 3 directions he can go
        List<RaycastHit2D> collider_hits = new List<RaycastHit2D>();
        
        // check in front of mouse, and to left and right of mouse
        Vector2[] raycast_directions = { transform.up , -1.0f * transform.up , -1.0f * transform.right };
        RaycastHit2D hit;
        float trigger_size = collision.GetComponent<BoxCollider2D>().size.y;
        foreach (Vector2 raycast_dir in raycast_directions)
        {
            // raycast from side of trigger box outwards
            hit = Physics2D.Raycast(collision.transform.position + (Vector3)raycast_dir * trigger_size, raycast_dir);
            if (hit.collider.tag == "MazeTrigger")
            {
                collider_hits.Add(hit);
            }
        }

        if(collider_hits.Count == 0)
        {
            // have to turn around
            rb.velocity = new Vector2(0, 0);
            transform.Rotate(Vector3.forward * 180);
            direction *= -1.0f;
            return;
        }
        
        Random rnd = new Random();
        int index = Random.Range(0, collider_hits.Count);
        

        Vector2 next_direction = (collider_hits[index].transform.position - collision.transform.position).normalized;
        if (direction != next_direction)
        {
            // we are going to need to turn
            rb.velocity = new Vector2(0, 0);
            
            // hmmm
            bool is_left_turn = Vector2.Dot(transform.up, next_direction) > 0;
            if (is_left_turn)
                transform.Rotate(Vector3.forward * -90);
            else
                transform.Rotate(Vector3.forward * 90);
            //transform.Rotate(Vector3.forward * Vector3.Angle(direction, next_direction));

            direction = (collider_hits[index].transform.position - collision.transform.position);
            direction.Normalize();

            // this is bad
            transform.position = collision.transform.position;

        }

    }

    public void ElectricShockTherapy()
    {
        print("Oof! Ouch! My bones!");
    }
}
