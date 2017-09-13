using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMove : MonoBehaviour {

    public float acceleration;
    public float max_speed;
    public float rot_speed;
    
    Vector2 direction = Vector2.left;
    Rigidbody2D rb;
    Dictionary<string, int> trigger_map = new Dictionary<string, int>();

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    
    // Update is called once per frame
    void Update()
    {
        // draw a tail
        Debug.DrawLine(transform.position, transform.position + transform.right * 35f, Color.magenta);
        if (rb.velocity.magnitude < max_speed)
        {
            rb.AddForce(direction * acceleration, ForceMode2D.Force);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        DialogueTrigger dt = collision.gameObject.GetComponent<DialogueTrigger>();
        if(dt != null)
        {
            if( !dt.activated )
                dt.Activate();
        }

        // make the mouse choose a new path semi-randomly
        // raycast forward, left, and right to find the 3 directions he can go
        List<RaycastHit2D> collider_hits = new List<RaycastHit2D>();
        
        // check in front of mouse, and to left and right of mouse
        Vector2[] raycast_directions = { transform.up , -1.0f * transform.up , -1.0f * transform.right };
        RaycastHit2D hit;
        float trigger_size = collision.GetComponent<BoxCollider2D>().size.y;
        float rand_total = 0f;
        foreach (Vector2 raycast_dir in raycast_directions)
        {
            // raycast from side of trigger box outwards
            hit = Physics2D.Raycast(collision.transform.position + (Vector3)raycast_dir * trigger_size, raycast_dir);
            if (hit.collider.tag == "MazeTrigger")
            {
                collider_hits.Add(hit);
                if (!trigger_map.ContainsKey(hit.collider.name))
                {
                    rand_total += 1.0f;
                    trigger_map.Add(hit.collider.name, 1);
                }
                else
                {
                    rand_total += 1.0f / trigger_map[hit.collider.name];
                }
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


        print(rand_total);
        Random rnd = new Random();

        float place = Random.Range(0.0f, rand_total);
        float cur_place = 0.0f;
        RaycastHit2D next_target = collider_hits[0];

        for(int i = 0; i < collider_hits.Count; ++i)
        {
            cur_place += 1.0f / trigger_map[collider_hits[i].collider.name];
            if (place < cur_place)
            {
                next_target = collider_hits[i];
                break;
            }
        }

        Vector2 next_direction = (next_target.transform.position - collision.transform.position).normalized;
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

            direction = (next_target.transform.position - collision.transform.position);
            direction.Normalize();

            // this is bad
            transform.position = collision.transform.position;
        }

    }

    public void ElectricShockTherapy()
    {
        // increment our current target trigger
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right * -1.0f);
        trigger_map[hit.collider.name]++;

        // turn this car around mister
        rb.velocity = new Vector2(0, 0);
        transform.Rotate(Vector3.forward * 180);
        direction *= -1.0f;
        
    }
}
