using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.graphic;
using static Toybox.maps.tiles.Tilemap;

namespace Toybox.maps.tiles {

	public class TileRoomMap {

		private readonly int[] Data;
		public const int Blocked = -1;
		private const int Unexplored = 0;
		private int NextGroupId = 1;
		private readonly Queue<int> Queue = new();

		public readonly int Columns;
		public readonly int Rows;
		private readonly int[] TouchingOffsets;

		public TileRoomMap(int columns, int rows) {
			Data = new int[columns * rows];
			Columns = columns;
			Rows = rows;
			TouchingOffsets = new int[] { 0, -1, 1, Columns, -Columns, -Columns - 1, -Columns + 1, Columns - 1, Columns + 1 };
		}

		public int Get(int col, int row) {
			var i = col + (row * Columns);
			if (i < 0 || i >= Data.Length) return Blocked;
			return Data[i];
		}

		public void GetTouching(int col, int row, HashSet<int> output, bool doDiagonals) {
			var pos = col + (row * Columns);
			var stop = TouchingOffsets.Length;
			if (!doDiagonals) stop = 4;

			for (int i = 0; i < stop; i++) {
				var select = pos + TouchingOffsets[i];
				if (select < 0 || select >= Data.Length) continue;
				var value = Data[select];
				if (value == Blocked) continue;
				output.Add(value);
			}
		}

		public bool IsTouching(int col, int row, HashSet<int> values, bool doDiagonals) {
			var pos = col + (row * Columns);
			var stop = TouchingOffsets.Length;
			if (!doDiagonals) stop = 4;

			for (int i = 0; i < stop; i++) {
				var select = pos + TouchingOffsets[i];
				if (select < 0 || select >= Data.Length) continue;
				if (values.Contains(Data[select])) return true;
			}
			return false;
		}

		public void SolveRooms(Tilemap collisionMap, Tile[] blockingTiles) {
			NextGroupId = 1;
			int x = 0, y = 0;

			for (int i = 0; i < Data.Length; i++) {
				if (blockingTiles.Contains(collisionMap.Map[x][y])) Data[i] = Blocked;
				else Data[i] = Unexplored;

				x++;
				if (x >= Columns) {
					x = 0;
					y++;
				}
			}

			for (int i = 0; i < Data.Length; i++) {
				if (Data[i] == Unexplored) ExplorePoint(i);
			}
		}

		private void ExplorePoint(int pos) {
			Queue.Enqueue(pos);

			while (Queue.Any()) {
				var i = Queue.Dequeue();
				if (i < 0 || i >= Data.Length) continue;
				if (Data[i] == Blocked || Data[i] == NextGroupId) continue;

				Data[i] = NextGroupId;
				Queue.Enqueue(i - 1);
				Queue.Enqueue(i + 1);
				Queue.Enqueue(i - Columns);
				Queue.Enqueue(i + Columns);
			}
			NextGroupId++;
			Queue.Clear();
		}
	}
}
