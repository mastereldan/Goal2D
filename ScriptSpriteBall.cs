using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptSpriteBall : MonoBehaviour
{
	[SerializeField]
	private float _ballGravity, // = 0.25f
		_ballOffsetBallInAir = 0.3f,
		_ballOffsetY = 0.15f,
		_resetPositionOffsetY = 0.25f,
		_maxBallVerticalVelocity = 5f;

	public bool playerIsShooting = false;
	public bool	playerNearBall = false;
	public bool	playerHasBall = false;
	public bool	playerOnBall = false;
	public bool	ballInAir = false;

	//private GameObject _mainGameManager;
	private GameObject _objectPlayer, _objectSpritePlayer, /*_objectPlayerBody,*/ _objectTriggerPlayerFeet;
	private GameObject _objectBall;
	private Rigidbody2D _rb2d;
	private Collider2D _thisGameObjectCollider2D, _objectSpritePlayerCollider2D; /*_objectPlayerBodyCollider2D;*/

	//ScriptMainGameManager scriptMainGameManager;
	ScriptPlayer scriptPlayer;
	ScriptSpritePlayer scriptSpritePlayer;
	ScriptBall scriptBall;

	/// <summary>
	/// //////////////////////////////////////////////////////////////////////////////////////////////
	/// </summary>


	// Use this for initialization
	void Start ()
	{
		_rb2d = GetComponent <Rigidbody2D> ();

		//_mainGameManager = GameObject.Find ("MainGameManager");
		//scriptMainGameManager = _mainGameManager.GetComponent<ScriptMainGameManager> ();

		_objectBall = GameObject.Find ("Ball");
		scriptBall = _objectBall.GetComponent <ScriptBall> ();
		
		_objectPlayer = GameObject.Find ("Player");
		scriptPlayer = _objectPlayer.GetComponent <ScriptPlayer> ();

		_objectSpritePlayer = GameObject.Find ("SpritePlayer");
		scriptSpritePlayer = _objectSpritePlayer.GetComponent <ScriptSpritePlayer> ();

		//_objectPlayerBody = GameObject.Find ("PlayerBody");

		_objectTriggerPlayerFeet = GameObject.Find ("TriggerPlayerFeet");

		_thisGameObjectCollider2D = this.gameObject.GetComponent <Collider2D> ();
		_objectSpritePlayerCollider2D = _objectSpritePlayer.GetComponent <Collider2D> ();
		//_objectPlayerBodyCollider2D = _objectPlayerBody.GetComponent <Collider2D> ();
	}

	// Update is called once per frame
	void Update ()
	{
		
	}

	// This function is called every fixed framerate frame (physics frame)
	void FixedUpdate ()
	{
		SetMaxBallVelocity_Y (_maxBallVerticalVelocity);

		MoveBall_Y (playerHasBall, playerOnBall, _ballOffsetY);

		CheckIfBallInAir (_objectBall.transform.position, _ballOffsetBallInAir);
		CheckIfPlayerNotNearBall_IgnorePhysicsCollision (playerNearBall);
		CheckIfPlayerHasBall (playerHasBall, playerIsShooting);
		CheckForSpriteBallPositionYandCorrect (_objectBall.transform.position, _resetPositionOffsetY);
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

	public Vector2 JuggleBall (float jF)
	{
		Debug.Log ("JUGGLE BALL! Force: " + jF);

		return _rb2d.velocity = Vector2.up * jF;
	}

	private void MoveBall_Y (bool hasBall, bool onBall, float y) // VERTICAL
	{
		if (hasBall)
			if (!onBall)
				SetBallPositionToPlayer_Y (y);
		
		return;
	}

	private Vector2 SetBallPositionToPlayer_Y (float bY)
	{	
		// ONLY FOR USE WITH DYNAMIC RIGIDBODY2D
		return transform.position = new Vector2 ( transform.position.x, _objectTriggerPlayerFeet.transform.position.y + bY );
	}

	private bool CheckIfBallInAir (Vector2 ballPosition, float bA, bool inAir = false)
	{
		if (this.gameObject.transform.position.y > (ballPosition.y + bA) )
			inAir = true;

		return ballInAir = inAir;
	}

	private bool CheckIfPlayerNotNearBall_IgnorePhysicsCollision (bool nearBall) // IGNORE COLLISION IF PLAYER NOT NEAR BALL
	{
		if (!nearBall)
		{
			//Debug.Log ("IGNORED COLLISION");
			Physics2D.IgnoreCollision (_thisGameObjectCollider2D, _objectSpritePlayerCollider2D, true);
			//Physics2D.IgnoreCollision (_thisGameObjectCollider2D, _objectPlayerBodyCollider2D, true);
			return true;
		}
		else
		{
			Physics2D.IgnoreCollision (_thisGameObjectCollider2D, _objectSpritePlayerCollider2D, false);
			//Physics2D.IgnoreCollision (_thisGameObjectCollider2D, _objectPlayerBodyCollider2D, false);
			return false;
		}
	}

	private bool CheckIfPlayerHasBall (bool hasBall, bool isShooting, bool b = false)
	{
		if (hasBall || isShooting)
			b = true;

		return DisableGravityForBall (b);
	}

	private bool DisableGravityForBall (bool b, float f = 0f)
	{
		if (b)
		{
			_rb2d.gravityScale = f;
			_rb2d.velocity = new Vector2 (_rb2d.velocity.x, f);
			return true;
		}
		else
			_rb2d.gravityScale = _ballGravity;
			return false;
	}

	private Vector2 SetMaxBallVelocity_Y (float mvv)
	{
		return _rb2d.velocity = new Vector2 (_rb2d.velocity.x, Mathf.Clamp (_rb2d.velocity.y, -mvv, mvv) ); // VERTICAL
	}

	private Vector2 CheckForSpriteBallPositionYandCorrect (Vector2 ballPosition, float rpOffsetY)
	{
		// IF SPRITE BALL FALLS TRHOUGH FLOOR (PAST BALL GAME OBJECT -PHYSICS GLITCH)
        if (this.gameObject.transform.position.y < ballPosition.y)
            _rb2d.transform.position = new Vector2 (ballPosition.x, ballPosition.y + rpOffsetY);
		
		// LET SPRITE BALL OBJECT FOLLOW BALL OBJECT
		return this.gameObject.transform.position = new Vector2 (ballPosition.x, this.gameObject.transform.position.y);
    }

	private void OnTriggerEnter2D (Collider2D other)
	{
		// CONTACT WITH PLAYER FEET TRIGGER
		if (other.name == "TriggerPlayerFeet" && playerNearBall)
		{
			Debug.Log ("Ball contacts Player Feet");

			if (!scriptSpritePlayer.playerInAir && !ballInAir && !playerOnBall)
				scriptPlayer.SetPlayerHasBall (true);
		}
		// CONTACT WITH PLAYER BODY TRIGGER
		if (other.name == "PlayerBody" && playerNearBall)
		{
			Debug.Log ("Ball contacts Player Body");

			if (!playerOnBall)
				scriptPlayer.SetPlayerHasBall (true);
		}
		return;
	}

	private void OnTriggerExit2D (Collider2D other)
	{

	}

	private void OnTriggerStay2D (Collider2D other)
	{

	}

	private void OnCollisionEnter2D (Collision2D collision)
	{
		Debug.Log ("Collision: " + collision.gameObject.name);

		if (collision.gameObject.name == "Ball")
		{
			Debug.Log ("Ball on ground.");

			// AFTER JUGGLING SPRITE BALL, REATTACH IT TO PLAYER
			if (scriptBall.playerHasBall)
				scriptPlayer.SetPlayerHasBall (true);
		}

		// FOR CONTACT WITH SPRITE PLAYER OR PLAYER BODY
		if (collision.gameObject.name == "SpritePlayer" && playerNearBall)
		{
			Debug.Log ("Ball contacts Player");

			if (!scriptSpritePlayer.playerInAir && !ballInAir && !playerOnBall)
				scriptPlayer.SetPlayerHasBall (true);
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
