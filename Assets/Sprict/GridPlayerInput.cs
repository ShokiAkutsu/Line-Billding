using UnityEngine;

public class GridPlayerInput : MonoBehaviour
{
	GridMotor _motor;
	GridTerritoryHandler _handler;
	GridManager _gridManager;

	void Awake()
	{
		_motor = GetComponent<GridMotor>();
		_handler = GetComponent<GridTerritoryHandler>();
		_gridManager = FindFirstObjectByType<GridManager>();

		transform.position = new Vector3(
			Mathf.Floor(transform.position.x) + 0.5f,
			0.5f,
			Mathf.Floor(transform.position.z) + 0.5f
		);
	}

	void OnEnable()
	{
		// Motor‚ÌˆÚ“®‚ªI‚í‚Á‚½‚çAHandler‚Ì”»’è‚ğ“®‚©‚·‚æ‚¤‚É•R•t‚¯
		_motor.onMoveComplete += _handler.OnPlayerMoveComplete;
	}

	void Update()
	{
		if (_motor.IsMoving) return;

		// ˆÚ“®“ü—Í
		float h = Input.GetAxisRaw("Horizontal");
		float v = Input.GetAxisRaw("Vertical");

		if (h != 0) _motor.RequestMove(new Vector3(h, 0, 0));
		else if (v != 0) _motor.RequestMove(new Vector3(0, 0, v));

		// “o’¸“ü—Í
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (_handler.CheckIfOnHome(transform.position))
			{
				transform.position += Vector3.up;
				_gridManager.LevelUp(transform.position);
			}
		}
	}
}