using System.Security;
using UnityEngine;

public class GridManager : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] GameObject _tracePrefab;
	[SerializeField] Color _homeColor = Color.blue;
	[Header("Grid Range")]
	[SerializeField] int _minX = -10; [SerializeField] int _maxX = 10;
	[SerializeField] int _minZ = -10; [SerializeField] int _maxZ = 10;
	[SerializeField] int _currentLevel = 0;
	public float CurrentHeight => _currentLevel + 0.5f;

	public void LevelUp(Vector3 playerPos)
	{
		_currentLevel++;
		CreateHomeTile(Mathf.FloorToInt(playerPos.x), Mathf.FloorToInt(playerPos.z));
	}

	public void FinalizeTerritory()
	{
		// 1. TraceをHomeに昇格
		foreach (GameObject t in GameObject.FindGameObjectsWithTag("Trace"))
		{
			SetToHome(t);
		}
		// 2. 塗りつぶし
		FillEnclosedArea();
	}

	private void FillEnclosedArea()
	{
		int width = _maxX - _minX + 1;
		int height = _maxZ - _minZ + 1;
		int[,] grid = new int[width, height];
		bool[,] isOutside = new bool[width, height];

		// スキャン（目）
		for (int x = 0; x < width; x++)
			for (int z = 0; z < height; z++)
				if (Physics.CheckSphere(new Vector3(x + _minX + 0.5f, CurrentHeight, z + _minZ + 0.5f), 0.3f))
					grid[x, z] = 1;

		// 計算（脳）：GridCalculatorを呼び出す
		for (int x = 0; x < width; x++)
		{
			GridCalculator.FloodFill(x, 0, grid, isOutside);
			GridCalculator.FloodFill(x, height - 1, grid, isOutside);
		}
		for (int z = 0; z < height; z++)
		{
			GridCalculator.FloodFill(0, z, grid, isOutside);
			GridCalculator.FloodFill(width - 1, z, grid, isOutside);
		}

		// 生成（手足）
		for (int x = 0; x < width; x++)
			for (int z = 0; z < height; z++)
				if (grid[x, z] == 0 && !isOutside[x, z])
					CreateHomeTile(x + _minX, z + _minZ);
	}

	public void SetToHome(GameObject obj)
	{
		obj.tag = "Home";
		if (obj.TryGetComponent<Renderer>(out var ren)) ren.material.color = _homeColor;
	}

	private void CreateHomeTile(int x, int z)
	{
		Vector3 pos = new Vector3(x + 0.5f, CurrentHeight, z + 0.5f);
		GameObject newTile = Instantiate(_tracePrefab, pos, Quaternion.identity);
		SetToHome(newTile);
	}
}