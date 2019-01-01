using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptSpritePlayer : MonoBehaviour
{
    [SerializeField]
    private float _resetPositionOffsetX = 0.025f, _resetPositionOffsetY = 0.65f, _playerOffsetPlayerInAir = 0.7f, _maxPlayerVerticalVelocity = 4f;
    [SerializeField]
    private float _jumpForce, _doubleJumpForce;
    [SerializeField]
    private bool _doubleJump = false;

	public bool playerInAir, playerOnGround, playerOnBall = false, canJump = true;
    
    private GameObject _objectTriggerBall;
    private GameObject _objectPlayer, _objectTriggerPlayerFeet;
    private Rigidbody2D _rb2d;

    ScriptTriggerBall scriptTriggerBall;

	/// <summary>
	/// //////////////////////////////////////////////////////////////////////////////////////////////
	/// </summary>


    // Use this for initialization
    void Start ()
    {
        _rb2d = GetComponent <Rigidbody2D> ();

        _objectPlayer = GameObject.Find ("Player");

        _objectTriggerPlayerFeet = GameObject.Find ("TriggerPlayerFeet");

        _objectTriggerBall = GameObject.Find ("TriggerBall");
        scriptTriggerBall = _objectTriggerBall.GetComponent <ScriptTriggerBall> ();
    }

    // Update is called once per frame
    void Update ()
    {

    }

    // This function is called every fixed framerate frame (physics frame)
    void FixedUpdate ()
    {
        CheckIfPlayerInAir (playerOnBall, playerInAir, _playerOffsetPlayerInAir);

        //CheckIfPlayerOnGround ();

		SetMaxPlayerVerticalVelocity (_maxPlayerVerticalVelocity);
        
        PlayerInput_Jump (playerOnBall, _jumpForce, _doubleJumpForce);

        CheckForSpritePlayerPositionYandCorrect (_objectTriggerPlayerFeet.transform.position.y, _resetPositionOffsetX, _resetPositionOffsetY); // IF PLAYER FALLS TROUGH FLOOR
    }

    // LateUpdate is called after all Update functions have been called
    void LateUpdate ()
    {

    }

    // OnGUI is called for rendering and handling GUI events
    void OnGUI ()
    {

    }

    // OnDisable is called when the behaviour becomes disabled () or inactive, also called when object is destroyed
    void OnDisable ()
    {

    }

    // OnEnable is called when the object becomes enabled and active
    void OnEnable ()
    {

    }

    private bool CheckIfPlayerInAir (bool onBall, bool inAir, float pA, float ballHeight = 0f)
	{
        if (onBall)
            ballHeight = 0.36f;


		if (this.gameObject.transform.position.y > (_objectPlayer.transform.position.y + pA + ballHeight) )
			inAir = true;
		else
			inAir = false;


        playerOnGround = !inAir;
        canJump = !inAir;

        return playerInAir = inAir;
	}

    /*private bool CheckIfPlayerOnGround ()
    {
        if (playerOnBall && !playerInAir)
            playerOnGround = true;

        return playerOnGround;
    }*/

	private Vector2 SetMaxPlayerVerticalVelocity (float mvv)
    {
		return _rb2d.velocity = new Vector2 (_rb2d.velocity.x, Mathf.Clamp (_rb2d.velocity.y, -mvv, mvv) );
	}

    private Vector2 PlayerInput_Jump (bool onBall, float jF, float djF)
    {
        if (onBall)
        {
            if ( Input.GetKey (KeyCode.Space) )
            {
                scriptTriggerBall.SetPlayerHasBall (false);
                scriptTriggerBall.SetPlayerOnBall (false);
            }
        }

        if (Input.GetKeyDown (KeyCode.Space) )
        {
            if (_doubleJump)
            {
                _doubleJump = false;
                //transform.Translate (Vector2.up * _jumpVertical * _doubleJumpForce * Time.deltaTime);
                return _rb2d.velocity = new Vector2 (0, djF * 100f * Time.fixedDeltaTime);
            }
            if (canJump)
            {
                canJump = false;
                _doubleJump = true;
                //transform.Translate (Vector2.up * jumpVertical * jumpForce * Time.deltaTime);
			    return _rb2d.velocity = new Vector2 (0, jF * 100f * Time.fixedDeltaTime);
            }
        }
        return _rb2d.velocity = new Vector2 (_rb2d.velocity.x, _rb2d.velocity.y);
    }

    private Vector2 CheckForSpritePlayerPositionYandCorrect (float triggerPlayerFeet_posY, float rpOffsetX, float rpOffsetY)
    {
        // IF PLAYER FALLS TRHOUGH FLOOR (PAST PLAYER GAME OBJECT -PHYSICS GLITCH)
        if (triggerPlayerFeet_posY < _objectPlayer.transform.position.y)
            return _rb2d.transform.position = new Vector2 (_objectPlayer.transform.position.x - rpOffsetX, _objectPlayer.transform.position.y + rpOffsetY);
        else
            return _rb2d.transform.position = new Vector2 (this.gameObject.transform.position.x, this.gameObject.transform.position.y);
    }

    private void OnTriggerEnter2D (Collider2D other)
    {
		//Debug.Log ("Collision: " + other.name + ".");
    }

    private void OnTriggerExit2D (Collider2D other)
    {

    }

    private void OnTriggerStay2D (Collider2D other)
    {

    }

	private void OnCollisionEnter2D (Collision2D collision)
    {
		//Debug.Log ("Collision: " + collision.gameObject.name + ".");

		if (collision.gameObject.name == "Player")
        {
			Debug.Log ("Player on ground.");

            playerOnGround = true;
		}
        return;
	}

	private void OnCollisionExit2D (Collision2D collision)
    {

	}
	private void OnCollisionStay2D (Collision2D collision)
    {
		
	}
}
