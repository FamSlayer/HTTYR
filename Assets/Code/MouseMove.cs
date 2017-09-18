using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMove : MonoBehaviour {

    public float acceleration;
    public float max_speed;
    public float rot_speed;

    public float time_to_turn = 0.4f;

    // testing this out
    public AnimationCurve turn_movespeed_curve;
    
    Vector2 direction = Vector2.left;
    Rigidbody2D rb;
    Dictionary<string, int> trigger_map = new Dictionary<string, int>();

    enum MouseState { MoveForward, Turn};
    MouseState m_state;


    float turn_time;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        m_state = MouseState.MoveForward;
    }

    
    // Update is called once per frame
    void Update()
    {
        // draw a tail
        Debug.DrawLine(transform.position, transform.position + transform.right * 35f, Color.magenta);


        if (m_state == MouseState.MoveForward)
        {
            if (rb.velocity.magnitude < max_speed)
            {
                rb.AddForce(direction * acceleration, ForceMode2D.Force);
            }
        }
        else if (m_state == MouseState.Turn)
        {
            Debug.DrawLine(transform.position, transform.position + (Vector3)direction * 15f, Color.cyan);
            float rot_speed = 90 / time_to_turn;

            bool is_left_turn = Vector2.Dot(transform.up, direction) > 0;
            if (is_left_turn)
                transform.Rotate(Vector3.forward * -rot_speed * Time.deltaTime);
            else
                transform.Rotate(Vector3.forward * rot_speed * Time.deltaTime);

            // move forward a lil bit
            // trying to have movespeed of mouse slow as it moves around corner
            float move_speed;
            move_speed = turn_movespeed_curve.Evaluate(turn_time / time_to_turn);
            print(move_speed);
            move_speed *= max_speed;
            //move_speed = turn_movespeed_curve.Evaluate(turn_time / time_to_turn) * max_speed;

            transform.position = transform.position + transform.right * -1.0f * move_speed * Time.deltaTime;
            turn_time += Time.deltaTime;
            
            //if((Vector2) transform.right * -1.0f == direction)
            if(Vector2.Dot((Vector2)transform.right * -1.0f, direction) > 0.9995f)
            {
                transform.right = direction * -1.0f;
                m_state = MouseState.MoveForward;
            }
            
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // (meme) solution to mouse hitting trigger twice while turning
        if (m_state == MouseState.Turn)
            return;

        // not sure if we want this:
        // Increment this direction's box to dissuade backtracking the maze
        if (!trigger_map.ContainsKey(collision.name))
        {
            trigger_map.Add(collision.name, 1);
        }
        else
        {
            trigger_map[collision.name]++;
        }

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

            // update the direction that we are going in
            direction = (next_target.transform.position - collision.transform.position);
            direction.Normalize();

            print("Time to rotate!");
            m_state = MouseState.Turn;
            turn_time = 0;
            /*
            // bad solution, placeholder for mouse actually turning
            // rotate the mouse 90 degrees
            bool is_left_turn = Vector2.Dot(transform.up, next_direction) > 0;
            if (is_left_turn)
                transform.Rotate(Vector3.forward * -90);
            else
                transform.Rotate(Vector3.forward * 90);

            // place the mouse right where the trigger is
            transform.position = collision.transform.position;
            */
        }

    }

    public void ElectricShockTherapy()
    {

        print("shocked the mouse");
        // todo: eventually figure out a better solution to this, maybe delayed event or bool flag?
        if (m_state == MouseState.Turn)
            return;

        // increment our current target trigger
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right * -1.0f);
        if(!trigger_map.ContainsKey(hit.collider.name))
        {
            trigger_map.Add(hit.collider.name, 1);
        }
        else
        {
            trigger_map[hit.collider.name]++;
        }

        // turn this car around mister
        rb.velocity = new Vector2(0, 0);
        transform.Rotate(Vector3.forward * 180);
        direction *= -1.0f;
        
    }
}
