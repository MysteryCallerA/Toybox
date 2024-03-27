using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.utils.data {

	public class List2d<T> {

		private List<List<T>> Content = new();
		public int Columns { get { return Content.Count; } }
		public int Rows { get {	if (Content.Count == 0) return 0; else return Content.First().Count; } }
		public T DefaultValue = default;

		public List2d(T defaultValue = default) {
			DefaultValue = defaultValue;

			Content.Add(new List<T>());
			Content[0].Add(defaultValue);
		}

		public List2d(T defaultValue, int cols, int rows) {
			DefaultValue = defaultValue;

			for (int x = 0; x < cols; x++) {
				var column = new List<T>();
				for (int y = 0; y < rows; y++) {
					column.Add(DefaultValue);
				}
				Content.Add(column);
			}
		}

		public T this[int col, int row] {
			get { return Content[col][row]; }
			set { Content[col][row] = value; }
		}

		public bool InBounds(int col, int row) {
			if (col < 0 || row < 0 || col >= Columns || row >= Rows) {
				return false;
			}
			return true;
		}

		public void Set(T value, int col, int row) {
			if (col < 0) {
				ExpandLeft(-col);
				col = 0;
			} else if (col >= Columns) {
				ExpandRight(col - Columns);
			}
			if (row < 0) {
				ExpandUp(-row);
				row = 0;
			} else if (row >= Rows) {
				ExpandDown(row - Rows);
			}

			Content[col][row] = value;
		}

		public void ExpandRight(int num) {
			for (int i = 0; i < num; i++) {
				Content.Add(GetDefaultColumn());
			}
		}

		public void ExpandLeft(int num) {
			for (int i = 0; i < num; i++) {
				Content.Insert(0, GetDefaultColumn());
			}
		}

		public void ExpandDown(int num) {
			for (int x = 0; x < Content.Count; x++) {
				for (int i = 0; i < num; i++) {
					Content[x].Add(DefaultValue);
				}
			}
		}

		public void ExpandUp(int num) {
			for (int x = 0; x < Content.Count; x++) {
				for (int i = 0; i < num; i++) {
					Content[x].Insert(0, DefaultValue);
				}
			}
		}

		public void DropColumns(int index, int num) {
			Content.RemoveRange(index, num);
		}

		public void DropRows(int index, int num) {
			for (int i = 0; i < Content.Count; i++) {
				Content.RemoveRange(index, num);
			}
		}

		private List<T> GetDefaultColumn() {
			var output = new List<T>();
			for (int i = 0; i < Rows; i++) {
				output.Add(DefaultValue);
			}
			return output;
		}

		public void Crop(int left, int top, int width, int height) {
			if (left > 0) DropColumns(0, left);
			if (width < Columns) DropColumns(width, Columns - width);
			if (top < 0) DropRows(0, top);
			if (height < Rows) DropRows(height, Rows - height);
		}

	}
}
