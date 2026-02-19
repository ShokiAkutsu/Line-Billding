using UnityEngine;
using System.Collections;

public class GridMotor : MonoBehaviour
{
	[SerializeField] float _moveSpeed = 5f;
	[SerializeField] bool _isMoving;
	public bool IsMoving => _isMoving;
	public System.Action<Vector3, Vector3> onMoveComplete; // 移動が終わった時に判定を呼び出す

	public void RequestMove(Vector3 direction)
	{
		if (!_isMoving)
			StartCoroutine(MoveRoutine(direction));
	}

	private IEnumerator MoveRoutine(Vector3 direction)
	{
		_isMoving = true;
		Vector3 startPos = transform.position;
		Vector3 targetPos = startPos + direction;

		float elapsedTime = 0;
		float timeToMove = 1f / _moveSpeed;

		while (elapsedTime < timeToMove)
		{
			transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / timeToMove);
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		transform.position = targetPos;
		_isMoving = false;

		onMoveComplete?.Invoke(transform.position, startPos);// 移動が終わった（引数：今の位置、1個前の位置）
	}
}