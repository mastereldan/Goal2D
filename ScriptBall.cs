using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptBall : MonoBehaviour
{
	[SerializeField]
	private float _ballOffsetX = 0.56f, _ballOffsetXplayerOnBall = 0f;

	public bool playerMovingRight;
	public bool	playerHasBall = false;
	public bool	playerOnBall = false;

	private Rigidbody2D _rb2d;
	private GameObject _objectPlayer, _objectTriggerShootingOrigin;

	/// <summary>
	/// //////////////////////////////////////////////////////////////////////////////////////////////
	/// </summary>


	// Use this for initialization
	void Start ()
	{
		// SET THE PHYSICS ENGINE TO IGNORE CERTAIN COLLISION (PLAYER OBJECT AND BALL OBJECT FOR EXAMPLE)
		Physics2D.IgnoreLayerCollision (9, 9);
		Physics2D.IgnoreLayerCollision (8, 9);
		Physics2D.IgnoreLayerCollision (9, 10);
		Physics2D.IgnoreLayerCollision (8, 11);

		_rb2d = GetComponent <Rigidbody2D> ();

		_objectPlayer = GameObject.Find ("Player");

		_objectTriggerShootingOrigin = GameObject.Find ("TriggerShootingOrigin");
	}

	// Update is called once per frame
	void Update ()
	{
		
	}

	// This function is called every fixed framerate frame (physics frame)
	void FixedUpdate ()
	{
		MoveBall (playerHasBall, playerOnBall, playerMovingRight, _ballOffsetX);
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

	private void MoveBall (bool hasBall, bool onBall, bool mR, float x)
	{
		if (hasBall)
		{
			if (onBall)
				x = _ballOffsetXplayerOnBall;

			//SetBallPositionToPlayer_X (mR, x);
			//SetBallOrientationToPlayer (mR);
			SetBallPositionXandRotationY (mR, x);
		}
		return;
	}

	private Vector2 SetBallPositionToPlayer_X (bool mR, float bX)
	{
		if (!mR) // IF PLAYER IS NOT MOVING RIGHT SET BALL POSITION ACCORDINGLY
			bX = -bX;

		return this.gameObject.transform.position = new Vector2 ( _objectPlayer.transform.position.x + bX, _objectPlayer.transform.position.y);
	}

	private Vector2 SetBallOrientationToPlayer (bool mR, float r = 0)
	{
		if (!mR) // IF PLAYER NOT MOVING RIGHT SET BALL ROTATION (ALONG Y AXIS)
			r = -180f;
		
		return this.gameObject.transform.eulerAngles = new Vector2 (0, r);
	}

	private void SetBallPositionXandRotationY (bool mR, float bX, float r = 0)
	{
		if (!mR) // IF PLAYER NOT MOVING RIGHT
		{
			bX = -bX;
			r = -180f;
		}

		// SET BALL POSITION ACCORDINGLY
		this.gameObject.transform.position = new Vector2 ( _objectPlayer.transform.position.x + bX, _objectPlayer.transform.position.y);
		// SET BALL ROTATION (ALONG Y AXIS)
		this.gameObject.transform.eulerAngles = new Vector2 (0, r);

		return;
	}

	public Vector2 SlowBallDown (float slowingFactor)
	{
		return _rb2d.velocity = _rb2d.velocity * slowingFactor; // .AddForce() HAS NO EFFECT BECAUSE ITS A KINEMATIC RIGIDBODY2D
	}

	public Transform DetachParent (bool b, Transform t = null)
	{
		if (!b)
		{
			_objectTriggerShootingOrigin.transform.position = this.gameObject.transform.position; // SET POSITION TO BALL OBJECT POSITION
			t = this.gameObject.transform; // REATTACH TO PARENT;
		}
		return _objectTriggerShootingOrigin.transform.parent = t;
	}

	public bool StopBall ()
	{
		Debug.Log ("BALL STOPPING");

		_rb2d.velocity = Vector2.zero;
		_rb2d.angularVelocity = 0f;

		return DetachParent (false); // REATTACH OBJECT TRIGGERSHOOTINGPATHMAIN TO OBJECT BALL
	}

	public Vector2 ShootBall (float sF, float mR)
	{
		Debug.Log ("SHOOT BALL!");
		DetachParent (true);

		return _rb2d.velocity = new Vector2 ( sF * 10f * mR * Time.fixedDeltaTime, _rb2d.velocity.y );
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
