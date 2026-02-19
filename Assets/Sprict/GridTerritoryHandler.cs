using UnityEngine;

public class GridTerritoryHandler : MonoBehaviour
{
	[SerializeField] GridManager _gridManager;
	[SerializeField] GameObject _tracePrefab;
	bool _isOutside = false;

	public void OnPlayerMoveComplete(Vector3 currentPos, Vector3 prevPos) // GridMotor‚ÌˆÚ“®Š®—¹ƒCƒxƒ“ƒg‚É‚±‚ê‚ð“o˜^‚·‚é
	{
		bool onHome = CheckIfOnHome(currentPos);
		
		SpawnTrace(prevPos);

		if (!onHome)
			_isOutside = true;
		else if (_isOutside)
		{
			_gridManager.FinalizeTerritory();
			_isOutside = false;
		}
	}

	public bool CheckIfOnHome(Vector3 pos)
	{
		Collider[] hitColliders = Physics.OverlapSphere(pos, 0.3f);
		foreach (var hit in hitColliders)
		{
			if (hit.CompareTag("Home")) 
				return true;
		}
		return false;
	}

	private void SpawnTrace(Vector3 pos)
	{
		if (_tracePrefab)
			Instantiate(_tracePrefab, pos, Quaternion.identity);
	}
}