using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptTriggerShootingPath : MonoBehaviour
{
	private bool _triggerShootingSlowing, _triggerShootingBallFinal, _triggerShootingBallStop;

	private GameObject _objectBall, _objectSpriteBall;
	
	ScriptBall scriptBall;
	ScriptSpriteBall scriptSpriteBall;
	
	/// <summary>
	/// //////////////////////////////////////////////////////////////////////////////////////////////
	/// </summary>


	// Use this for initialization
	void Start ()
	{
		 if (this.gameObject.name == "TriggerShootingPathSlowing")
			_triggerShootingSlowing = true;

		else if (this.gameObject.name == "TriggerShootingPathFinal")
			_triggerShootingBallFinal = true;
			
		else if (this.gameObject.name == "TriggerShootingPathStop")
			_triggerShootingBallStop = true;


		_objectBall = GameObject.Find ("Ball");
		scriptBall = _objectBall.GetComponent <ScriptBall> ();

		_objectSpriteBall = GameObject.Find ("SpriteBall");
		scriptSpriteBall = _objectSpriteBall.GetComponent <ScriptSpriteBall> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	// This function is called every fixed framerate frame (physics frame)
	void FixedUpdate ()
	{
		
	}

	private void OnTriggerEnter2D (Collider2D other)
	{
		if (_triggerShootingSlowing)
		{
			if (other.name == "TriggerBall")
			{
				Debug.Log ("SHOOTING BALL SLOWING");

				scriptBall.SlowBallDown (0.6f);
				scriptSpriteBall.playerIsShooting = false;
			}
		}

		if (_triggerShootingBallFinal)
		{
			if (other.name == "TriggerBall")
			{
				Debug.Log ("SHOOTING BALL FINAL");

				scriptBall.SlowBallDown (0.33f);
			}
		}
		
		if (_triggerShootingBallStop)
		{
			if (other.name == "TriggerBall")
				scriptBall.StopBall ();
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
		
	}

	private void OnCollisionExit2D (Collision2D collision)
	{

	}
	private void OnCollisionStay2D (Collision2D collision)
	{

	}
}
