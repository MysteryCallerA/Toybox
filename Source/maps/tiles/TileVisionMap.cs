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

	public class TileVisionMap {

		private int[] Data;
		public const int Blocked = -1;
		private const int Unexplored = 0;
		private int NextGroupId = 1;
		private readonly Queue<int> Queue = new();

		public int Columns { get; private set; }
		public int Rows { get; private set; }

		public TileVisionMap(int columns, int rows) {
			Data = new int[columns * rows];
			Columns = columns;
			Rows = rows;
		}

		public int Get(int col, int row) {
			return Data[col + (row * Columns)];
		}

		public bool IsTouching(int col, int row, List<int> values, bool doDiagonals = true) {
			var i = col + (row * Columns);
			if (values.Contains(Data[i - 1]) || values.Contains(Data[i + 1]) || values.Contains(Data[i - Columns]) || values.Contains(Data[i + Columns])) {
				return true;
			}
			if (doDiagonals) {
				if (values.Contains(Data[i - Columns - 1]) || values.Contains(Data[i - Columns + 1]) || values.Contains(Data[i + Columns - 1]) || values.Contains(Data[i + Columns + 1])) {
					return true;
				}
			}
			return false;
		}

		public void SolveVision(Tilemap collisionMap, Tile[] blockingTiles) {
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
