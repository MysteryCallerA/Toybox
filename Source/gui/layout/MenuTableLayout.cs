using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.gui.core;

namespace Toybox.gui.layout {
	public class MenuTableLayout:IMenuLayout {

		public int HSpacing = 0;
		public int VSpacing = 0;
		public int Columns = 1;
		public int Rows { get; private set; }
		private List<int> ColumnSizes = new();
		private List<int> RowSizes = new();
		private HashSet<int> FillOuterRows = new();
		private HashSet<int> FillOuterColumns = new();

		public void UpdateContentSize(List<MenuElement> content, Point contentContainerSize, out Point contentSize) {
			if (content.Count == 0) {
				contentSize = Point.Zero;
				return;
			}

			ColumnSizes.Clear();
			RowSizes.Clear();
			Rows = (int)Math.Ceiling((float)content.Count / Columns);
			for (int i = 0; i < Columns; i++) ColumnSizes.Add(0);
			for (int i = 0; i < Rows; i++) RowSizes.Add(0);

			//Initial minimum sizing
			int row = 0;
			int col = 0;
			foreach (var e in content) {
				if (e != null) {
					e.UpdateSize(contentContainerSize);
					var size = e.OuterSize;
					if (size.X > ColumnSizes[col]) {
						ColumnSizes[col] = size.X;
					}
					if (size.Y > RowSizes[row]) {
						RowSizes[row] = size.Y;
					}
				}

				col++;
				if (col >= Columns) {
					col = 0;
					row++;
				}
			}

			contentSize = new Point(HSpacing * (Columns - 1), VSpacing * (Rows - 1));
			contentSize = new Point(contentSize.X + ColumnSizes.Sum(), contentSize.Y + RowSizes.Sum());
			if (contentContainerSize == Point.Zero) return;

			//Find FillOuter cells
			row = 0;
			col = 0;
			FillOuterColumns.Clear();
			FillOuterRows.Clear();
			foreach (var e in content) {
				if (e != null) {
					if (e.HFit == MenuElement.FitType.FillOuter) FillOuterColumns.Add(col);
					if (e.VFit == MenuElement.FitType.FillOuter) FillOuterRows.Add(row);
				}

				col++;
				if (col >= Columns) {
					col = 0;
					row++;
				}
			}
			if (FillOuterColumns.Count == 0 && FillOuterRows.Count == 0) return;

			//Set cell sizes
			Point remaining = contentContainerSize - contentSize;
			int count = FillOuterColumns.Count;
			foreach (int c in FillOuterColumns) {
				int add = remaining.X / count;
				ColumnSizes[c] += add;
				remaining.X -= add;
				count--;
			}
			count = FillOuterRows.Count;
			foreach (int r in FillOuterRows) {
				int add = remaining.Y / count;
				RowSizes[r] += add;
				remaining.Y -= add;
				count--;
			}

			//Final updates
			row = 0;
			col = 0;
			foreach (var e in content) {
				if (e != null && (e.HFit == MenuElement.FitType.FillOuter || e.VFit == MenuElement.FitType.FillOuter)) {
					e.UpdateSize(new Point(ColumnSizes[col], RowSizes[row]));
				}

				col++;
				if (col >= Columns) {
					col = 0;
					row++;
				}
			}
			contentSize = new Point(HSpacing * (Columns - 1), VSpacing * (Rows - 1));
			contentSize = new Point(contentSize.X + ColumnSizes.Sum(), contentSize.Y + RowSizes.Sum());
		}

		public void UpdateContentPosition(List<MenuElement> content, MenuElement container) {
			if (content.Count == 0) return;

			var bounds = container.ContentBounds;
			int x = bounds.X, y = bounds.Y;
			int col = 0, row = 0;

			foreach (var e in content) {
				var cell = new Rectangle(x, y, ColumnSizes[col], RowSizes[row]);
				if (e != null) {
					if (e.HAlign == MenuElement.HAlignType.Left) {
						e.Position = new Point(cell.X, cell.Y);
					} else if (e.HAlign == MenuElement.HAlignType.Right) {
						e.Position = new Point(cell.Right - e.OuterSize.X, cell.Y);
					} else if (e.HAlign == MenuElement.HAlignType.Center) {
						e.Position = new Point(cell.Center.X - (e.OuterSize.X / 2), cell.Y);
					}
					if (e.VAlign == MenuElement.VAlignType.Bottom) {
						e.Position = new Point(e.Position.X, cell.Bottom - e.OuterSize.Y);
					} else if (e.VAlign == MenuElement.VAlignType.Center) {
						e.Position = new Point(e.Position.X, cell.Center.Y - (e.OuterSize.Y / 2));
					}
					e.UpdateContentPositions();
				}

				x = cell.Right + HSpacing;
				col++;
				if (col >= Columns) {
					col = 0;
					x = bounds.X;
					y = cell.Bottom;
					row++;
				}
			}
		}

		public void SelectDown(List<MenuElement> content, int selection, out int newSelection, out bool dirPossible, out bool wrappedAround) {
			newSelection = selection + Columns;
			if (newSelection >= content.Count) {
				newSelection = selection % Columns;
				wrappedAround = true;
			} else wrappedAround = false;
			dirPossible = true;
		}

		public void SelectLeft(List<MenuElement> content, int selection, out int newSelection, out bool dirPossible, out bool wrappedAround) {
			newSelection = selection - 1;
			int row = selection / Columns;
			if (newSelection < 0 || newSelection / Columns != row) {
				newSelection = (row * Columns) + (Columns - 1);
				if (newSelection >= content.Count) {
					newSelection = content.Count - 1;
				}
				wrappedAround = true;
			} else wrappedAround = false;
			dirPossible = true;
		}

		public void SelectRight(List<MenuElement> content, int selection, out int newSelection, out bool dirPossible, out bool wrappedAround) {
			newSelection = selection + 1;
			int row = selection / Columns;
			if (newSelection >= content.Count || newSelection / Columns != row) {
				newSelection = row * Columns;
				wrappedAround = true;
			} else wrappedAround = false;
			dirPossible = true;
		}

		public void SelectUp(List<MenuElement> content, int selection, out int newSelection, out bool dirPossible, out bool wrappedAround) {
			newSelection = selection - Columns;
			if (newSelection < 0) {
				newSelection = ((Rows - 1) * Columns) + (selection % Columns);
				if (newSelection >= content.Count) {
					newSelection -= Columns;
				}
				wrappedAround = true;
			} else wrappedAround = false;
			dirPossible = true;
		}
	}
}
