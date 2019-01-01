using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptPlayer : MonoBehaviour
{
	[SerializeField]
	private float _diagonalDrag, _playerSpeed, _playerRunSpeedBonus/*= 0.30f*/;
    [SerializeField]
    private bool _playerRunning = false;
    
    public bool playerHasBall, playerMovingRight;
    private float _moveHorizontal, _moveVertical;
    public float juggleForce; // = 2.6f
    public float shootForce; // = 15f
    
    private GameObject _objectBall, _objectSpriteBall, _objectTriggerBall;
    private GameObject _objectSpritePlayer;

    ScriptBall scriptBall;
    ScriptSpriteBall scriptSpriteBall;
    ScriptTriggerBall scriptTriggerBall;

	/// <summary>
	/// //////////////////////////////////////////////////////////////////////////////////////////////
	/// </summary>


    // Use this for initialization
    void Start ()
    {
        _objectBall = GameObject.Find ("Ball");
        scriptBall = _objectBall.GetComponent <ScriptBall> ();
        
        _objectSpriteBall = GameObject.Find ("SpriteBall");
		scriptSpriteBall = _objectSpriteBall.GetComponent <ScriptSpriteBall> ();

        _objectTriggerBall = GameObject.Find ("TriggerBall");
        scriptTriggerBall = _objectTriggerBall.GetComponent <ScriptTriggerBall> ();

        _objectSpritePlayer = GameObject.Find ("SpritePlayer");
    }

    // Update is called once per frame
    void Update ()
    {

    }

    // This function is called every fixed framerate frame (physics frame)
    void FixedUpdate ()
    {
        CheckIfPlayerWantsToRun (); // EDIT EDIT EDIT       FIND BETTER SOLUTION

        PlayerInput_Movement (_playerRunning);
        
        PlayerInput_Ball (playerHasBall, scriptBall.playerOnBall);
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

    private bool CheckIfPlayerWantsToRun ()
    {
        if (Input.GetKeyDown (KeyCode.LeftShift) || Input.GetKeyDown (KeyCode.RightShift) )
            _playerRunning = !_playerRunning;

        return _playerRunning;
    }

    private void PlayerInput_Movement (bool running, float moveSpeed = 0f, float runSpeed = 0f, float dDrag = 0f)
    {
        _moveHorizontal = Input.GetAxis ("Horizontal");
        _moveVertical = Input.GetAxis ("Vertical");

        if (running)
            runSpeed = _playerSpeed * _playerRunSpeedBonus; // +30% of _playerSpeed

		if (_moveVertical != 0)
        {
            dDrag = _diagonalDrag;
            if (running)
			    runSpeed = _playerSpeed * (_playerRunSpeedBonus / 2f); // +15% of _playerSpeed
        }
		if ( (_moveHorizontal != 0 && _moveVertical != 0) || (_moveHorizontal != 0 && _moveVertical != 0 && Input.GetButtonDown ("Jump") ) )
        {
			dDrag = _diagonalDrag;
            if (running)
                runSpeed = _playerSpeed * 0.15f; // +15% of _playerSpeed
        }

        if (_moveHorizontal > 0) // MOVE RIGHT
            SetOrientationToRight (true, 0);

        if (_moveHorizontal < 0) // MOVE LEFT
            SetOrientationToRight (false, -180f);


        moveSpeed = (_playerSpeed + runSpeed) - dDrag;

		this.gameObject.transform.Translate (Vector2.right * _moveHorizontal * moveSpeed * Time.fixedDeltaTime);
        this.gameObject.transform.Translate (Vector2.up * _moveVertical * moveSpeed * Time.fixedDeltaTime);
        
        Debug.Log (moveSpeed);
        return;
    }

    private void PlayerInput_Ball (bool hasBall, bool scriptBall_onBall)
    {
        if (hasBall)
        {
            // RELEASE THE BALL
            if (Input.GetKeyDown (KeyCode.E) )
            {
                SetPlayerHasBall (false);
                scriptTriggerBall.SetPlayerOnBall (false);
            }

            // JUGGLE THE BALL
            if (Input.GetKeyDown (KeyCode.F) && _moveHorizontal == 0)
            {
                SetPlayerHasBall (false);
                scriptTriggerBall.SetPlayerOnBall (false);

                scriptSpriteBall.JuggleBall (juggleForce);
            }

            // SHOOT THE BALL
            if (Input.GetKeyDown (KeyCode.Return) )
            {
                if (!scriptBall_onBall)
                {
                    SetPlayerHasBall (false);
                    scriptSpriteBall.playerIsShooting = true;

                    Shoot (shootForce);
                }
            }
        }
        return;
    }

    public Vector2 SetOrientationToRight (bool b, float r)
    {
        playerMovingRight = scriptBall.playerMovingRight = b;
        
        return _objectSpritePlayer.transform.eulerAngles = new Vector2 (0, r);
    }

    public void SetPlayerHasBall (bool b, bool stopBall = true)
    {
        if (stopBall)
		    scriptBall.StopBall ();


        playerHasBall = scriptBall.playerHasBall = scriptSpriteBall.playerHasBall = b;


        if (playerMovingRight)
            SetOrientationToRight (true, 0);
        else
            SetOrientationToRight (false, -180f);


        return;
	}

    public void Shoot (float shootForce, float moveRight = 1f)
    {
        if (!playerMovingRight)
             moveRight = -1f;
        
        scriptBall.ShootBall (shootForce, moveRight);
        return;
    }

    private void OnTriggerEnter2D (Collider2D other)
    {
		
    }

    private void OnTriggerExit2D (Collider2D other)
    {

    }

    private void OnTriggerStay2D (Collider2D other)
    {

    }

	private void OnCollisionEnter2D (Collision2D collision)
    {
		
	}

	private void OnCollisionExit2D (Collision2D collision)
    {

	}
	private void OnCollisionStay2D (Collision2D collision)
    {

	}
}
