using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMove : MonoBehaviour {

    public float start_delay = 3.0f;

    public float acceleration;
    public float max_speed;

    public float rot_speed;
    public float turn_delay;
    public float turn_ms_scale;

    float snap_float;


    //debug:
    float total_delta_abs = 0.0f;
    int num_turns = 0;

    Vector2 direction = Vector2.left;
    Rigidbody2D rb;
    Dictionary<string, int> trigger_map = new Dictionary<string, int>();

    enum MouseState { MoveForward, Turn, Wait};
    MouseState m_state;

    float num_deg_this_turn;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        m_state = MouseState.Wait;

        Invoke("start_moving", start_delay);
    }

    void start_moving()
    {
        // set the state to moving
        m_state = MouseState.MoveForward;

        // mess with the animatior
    }

    
    // Update is called once per frame
    void FixedUpdate()
    {
        // draw a tail
        Debug.DrawLine(transform.position, transform.position + transform.up * -1.0f * 15f, Color.magenta);

        if (m_state == MouseState.MoveForward)
        {
            //print("movespeed: " + rb.velocity.magnitude);
            // make mouse max movespeed relative to scale
            if (rb.velocity.magnitude < max_speed)
            {
                rb.AddForce(direction * acceleration, ForceMode2D.Force);
            }
        }
        else if (m_state == MouseState.Turn)
        {
            // draw an line for the direction that we are turning towards
            Debug.DrawLine(transform.position, transform.position + (Vector3)direction * 15f, Color.cyan);

            // rotate a small amount
            bool is_left_turn = Vector2.Dot(transform.right, direction) > 0;
            if (is_left_turn)
                transform.Rotate(Vector3.forward * -rot_speed * Time.deltaTime);
            else
                transform.Rotate(Vector3.forward * rot_speed * Time.deltaTime);
            num_deg_this_turn += rot_speed * Time.deltaTime;

            // move forward a lil bit
            float turn_move_speed =  (-1.0f * (1.0f - turn_ms_scale) * num_deg_this_turn / 90.0f + 1) * max_speed;
            //turn_move_speed = 0.0f;
            //print(turn_move_speed);
            transform.position = transform.position + transform.up * turn_move_speed * Time.deltaTime;

            //transform.position = transform.position + transform.up * turn_ms_scale * max_speed * Time.deltaTime;


            //if (Vector2.Dot((Vector2)transform.up * -1.0f * -1.0f, direction) > 0.9995f)
            if (Vector2.Dot((Vector2)transform.up * -1.0f * -1.0f, direction) > 0.995f)
            {
                // rotate the mouse to be exactly along the desired dimension
                transform.up = direction;
                /*
                if (is_left_turn)
                    transform.Rotate(Vector3.forward * -1f * (num_deg_this_turn - 90f));
                else
                    transform.Rotate(Vector3.forward * (num_deg_this_turn - 90f));
                */
                m_state = MouseState.MoveForward;


                // set initial velocity so turns are slightly smoother
                rb.velocity = direction * max_speed;
                print("max_speed: " + max_speed);

                // snap align to the middle of the "lane"
                if(Vector2.Dot(direction, Vector2.up) == 0)
                {
                    //print("snap y???");
                    Vector3 m_pos = transform.position;

                    float delta = m_pos.y - snap_float;
                    total_delta_abs += Mathf.Abs(delta);
                    num_turns++;

                    m_pos.y = snap_float;
                    transform.position = m_pos;
                }
                else
                {
                    //print("snap x???");
                    Vector3 m_pos = transform.position;

                    float delta = m_pos.x - snap_float;
                    total_delta_abs += Mathf.Abs(delta);
                    num_turns++;

                    m_pos.x = snap_float;
                    transform.position = m_pos;
                }

                //print("avg delta: " + (total_delta_abs / num_turns));
                //print("num deg: " + num_deg_this_turn);
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

        ChangeLevelTrigger changer = collision.gameObject.GetComponent<ChangeLevelTrigger>();
        if(changer != null)
        {
            changer.loadNextLevel();
            return;
        }

        // make the mouse choose a new path semi-randomly
        // raycast forward, left, and right to find the 3 directions he can go
        List<RaycastHit2D> collider_hits = new List<RaycastHit2D>();
        
        // check in front of mouse, and to left and right of mouse
        Vector2[] raycast_directions = { transform.up , -1.0f * transform.right , transform.right };
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

        // FOR DEBUGGING PLEASE REMEMBER TO REMOVE
        //place = 0;

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

            // update the direction that we are going in
            direction = (next_target.transform.position - collision.transform.position);
            direction.Normalize();

            if (Vector2.Dot(direction, Vector2.up) == 0)
            {
                snap_float = next_target.transform.position.y;
            }
            else
            {
                snap_float = next_target.transform.position.x;
            }
            
            
            Invoke("swap_to_turn", turn_delay);
        }

    }

    void swap_to_turn()
    {
        m_state = MouseState.Turn;
        rb.velocity = Vector2.zero;
        num_deg_this_turn = 0;
    }

    public void ElectricShockTherapy()
    {

        //print("shocked the mouse");
        // todo: eventually figure out a better solution to this, maybe delayed event or bool flag?
        if (m_state == MouseState.Turn)
            return;

        // increment our current target trigger
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up);
        if(!trigger_map.ContainsKey(hit.collider.name))
        {
            trigger_map.Add(hit.collider.name, 1);
        }
        else
        {
            trigger_map[hit.collider.name]++;
        }

        // turn this car around mister
        rb.velocity = Vector2.zero;
        transform.Rotate(Vector3.forward * 180);
        direction *= -1.0f;
        
    }
}
