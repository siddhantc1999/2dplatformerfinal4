using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class playermove : MonoBehaviour
{
    public Charactermovement mycharactermovment;
    public float movement;
     public Rigidbody2D myrigibody;
    public float movespeed;
    Animator myanimator;
    bool here;
    public float upspeed;
    public bool iscollided;
    public float ymovement=1f;
    bool ishurted;
    AnimatorClipInfo[] myanimatorclipinfo;
    public string currentanimation;
    bossfire mybossfire;
    [SerializeField] GameObject enemyfire;
    public int lives=3;
    [SerializeField] GameObject mylives;
    TextMeshProUGUI livestext;
    scenemanager myscenemanager;
    private void Awake()
    {
       
    }
    // Start is called before the first frame update
    void Start()
    {
    

        mycharactermovment = new Charactermovement();
        mycharactermovment.playermovment.Enable();
        myanimator = GetComponent<Animator>();
     /*   myanimatorclipinfo = myanimator.GetCurrentAnimatorClipInfo(0);*/
        mycharactermovment.playermovment.jump.performed += playerjump;
        //mycharactermovment.playermovment.jump.canceled += playerjumpended;
        myrigibody = GetComponent<Rigidbody2D>();
        mybossfire = FindObjectOfType<bossfire>();
        lives = 3;
        mylives = GameObject.Find("lives");
        livestext = mylives.GetComponent<TextMeshProUGUI>();
        myscenemanager = FindObjectOfType<scenemanager>();
        

    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        FindObjectOfType<scenemanager>().currentlives = lives;
        if (myscenemanager.isstart)
        {
            movement = mycharactermovment.playermovment.movement.ReadValue<Vector2>().x;
            myanimatorclipinfo = myanimator.GetCurrentAnimatorClipInfo(0);
            currentanimation = myanimatorclipinfo[0].clip.name;
            /*  Debug.Log("the current animation "+ myanimatorclipinfo[0].clip.name);*/

            //if (movement != 0 && iscollided && currentanimation!= "hurt")
            if (movement != 0 && currentanimation != "hurt")
            {
                myanimator.SetBool("run", true);
                if (movement > 0f)
                {
                    gameObject.transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);
                }
                else
                if (movement < 0f)
                {
                    gameObject.transform.localScale = new Vector3(-1f, transform.localScale.y, transform.localScale.z);
                }


                myrigibody.velocity = new Vector2(movement * 4f, myrigibody.velocity.y);



                //transform.position =new Vector2(transform.position.x, transform.position.y) + new Vector2(movement * movespeed * Time.deltaTime,0F); new Vector2(movement * movespeed * Time.deltaTime,0F);



            }
            else
            {
                myanimator.SetBool("run", false);
            }




        }
     
    }
    public void playerjump(InputAction.CallbackContext move)
    {
        if (iscollided && currentanimation != "hurt" && myscenemanager.isstart)
        {
            
            myanimator.SetTrigger("jump");
        

            myrigibody.velocity = new Vector2(myrigibody.velocity.x, ymovement*10f);
           

        }
    }
  
    private void OnCollisionStay2D(Collision2D collision)
    {

        
            if (collision.gameObject.layer == LayerMask.NameToLayer("landarea"))
            {

                iscollided = true;
            }
        if (currentanimation == "fjump" && collision.gameObject.layer == LayerMask.NameToLayer("landarea"))
        {
            if (myrigibody.velocity.y == 0)
            {
                myanimator.SetTrigger("jumpexit");
            }
        }
    }
        
        
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        iscollided = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "hammer")
        {

            myanimator.SetTrigger("hurt");
            lives -= 1;
            livesprint();
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(-60f, 30f, 0f));
        }
        if (collision.gameObject.tag == "eggparent")
        {
            mybossfire.keepfire = false;


            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(-400f, -400f, 0f));
        }
        
        if (currentanimation == "fjump" && collision.gameObject.layer == LayerMask.NameToLayer("landarea"))
            {
      
                myanimator.SetTrigger("jumpexit");
            
        }
        if(collision.gameObject.tag=="enemies")
        {
            if(currentanimation == "fjump")
            {
                GameObject newenemyfire = Instantiate(enemyfire, collision.gameObject.transform.position, Quaternion.identity);
                Destroy(collision.gameObject);
            }
            else
            {
                if(collision.gameObject.name== "rhino")
                {

                    myanimator.SetTrigger("hurt");
                    lives -= 1;
                    livesprint();
                    if (transform.position.x > collision.gameObject.transform.position.x)
                    {
                        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(60f, 30f, 0f));
                    }
                    else
                    {
                        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(-60f, 30f, 0f));
                    }

                }
               

            }

          

        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "hitobjects")
        {

            
            myanimator.SetTrigger("hurt");
            lives -= 1;
            livesprint();
        }
        if (collision.gameObject.name == "bees")
        {
            if (currentanimation != "fjump")
            {
                myanimator.SetTrigger("hurt");
                lives -= 1;
                livesprint();
                if (transform.position.x > collision.gameObject.transform.position.x)
                {
                    gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(50f, 30f, 0f));
                }
                else
                {
                    gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(-50f, 30f, 0f));
                }
            }
            else if(currentanimation == "fjump")
            {
                GameObject newenemyfire = Instantiate(enemyfire, collision.gameObject.transform.position, Quaternion.identity);
                Destroy(collision.gameObject);
               
            }


        }

    }
    public void livesprint()
    {
        
        livestext.text = "LIVES:" + lives.ToString();
    }



}
