using UnityEngine;

public static class GridCalculator
{
	// 二次元配列と、外側かどうかを判定するフラグ配列を受け取って計算
	public static void FloodFill(int x, int z, int[,] grid, bool[,] isOutside)
	{
		int width = grid.GetLength(0);
		int height = grid.GetLength(1);

		if (x < 0 || x >= width || z < 0 || z >= height) return;
		if (isOutside[x, z] || grid[x, z] == 1) return;

		isOutside[x, z] = true;
		FloodFill(x + 1, z, grid, isOutside);
		FloodFill(x - 1, z, grid, isOutside);
		FloodFill(x, z + 1, grid, isOutside);
		FloodFill(x, z - 1, grid, isOutside);
	}
}