using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Toybox.utils.text;

namespace Toybox.components.pathing {
	public class GridPathfinder {

		/// <summary> (Point from, Point to)-> Return true if movement is not possible. 
		/// <br></br> Points are in grid-space, where each unit is one full step in the grid. </summary>
		public Func<Point, Point, bool> IsSolidFunc = IsSolid;

		/// <summary> (GridPathNode node, Point endPoint) Set node.Traveled, node.Remaining, and node.Total 
		/// <br></br> EndPoint is in grid-space, where each unit is one full step in the grid. </summary>
		public Action<GridPathNode, Point> SolveNodeFunc = SolveNode;

		public bool DiagonalAllowed = true;
		public int MaxPathLength = 1000;

		private List<GridPathNode> OpenNodes = new();
		private HashSet<Point> ClosedNodes = new();
		private Point End;

		/// <summary> Points are in grid-space, where each unit is one full step in the grid. </summary>
		public List<Point> FindPath(Point start, Point end) {
			End = end;
			OpenNodes.Clear();
			ClosedNodes.Clear();

			OpenNodes.Add(new GridPathNode(null, start));
			int pathLength = 1;

			while (OpenNodes.Count > 0) {
				var nextnode = GetNextNode(out int index);
				OpenNodes.RemoveAt(index);
				ClosedNodes.Add(nextnode.Position);

				if (nextnode.Position == end) {
					return GetPath(nextnode);
				}

				ExploreNode(nextnode);

				pathLength++;
				if (pathLength > MaxPathLength) break;
			}
			return null;
		}

		private GridPathNode GetNextNode(out int index) {
			var nextnode = OpenNodes.Last();
			index = OpenNodes.Count - 1;
			for (int i = index; i >= 0; i--) {
				if (OpenNodes[i].Total < nextnode.Total) {
					nextnode = OpenNodes[i];
					index = i;
				}
			}
			return nextnode;
		}

		private List<Point> GetPath(GridPathNode end) {
			var path = new List<Point>();
			while (end != null) {
				path.Add(end.Position);
				end = end.Parent;
			}
			path.Reverse();
			return path;
		}

		private void ExploreNode(GridPathNode node) {
			if (node.Parent == null) { ExploreAllDirections(node); return; }
			var dir = node.Position - node.Parent.Position;
			if (dir == Point.Zero) { ExploreAllDirections(node); return; }

			var main = node.Position + dir;
			TryCreateNode(node, main);

			if (dir.Y == 0) {
				TryCreateNode(node, new Point(main.X, main.Y - 1));
				TryCreateNode(node, new Point(main.X, main.Y + 1));
				if (DiagonalAllowed) {
					TryCreateNode(node, new Point(node.Position.X, main.Y - 1));
					TryCreateNode(node, new Point(node.Position.X, main.Y + 1));
				}
			} else if (dir.X == 0) {
				TryCreateNode(node, new Point(main.X - 1, main.Y));
				TryCreateNode(node, new Point(main.X + 1, main.Y));
				if (DiagonalAllowed) {
					TryCreateNode(node, new Point(main.X - 1, node.Position.Y));
					TryCreateNode(node, new Point(main.X + 1, node.Position.Y));
				}
			} else {
				TryCreateNode(node, new Point(main.X, node.Position.Y));
				TryCreateNode(node, new Point(node.Position.X, main.Y));
				TryCreateNode(node, new Point(node.Position.X - dir.X, main.Y));
				TryCreateNode(node, new Point(main.X, node.Position.Y - dir.Y));
			}
		}

		private void ExploreAllDirections(GridPathNode node) {
			TryCreateNode(node, new Point(node.Position.X + 1, node.Position.Y));
			TryCreateNode(node, new Point(node.Position.X, node.Position.Y + 1));
			TryCreateNode(node, new Point(node.Position.X - 1, node.Position.Y));
			TryCreateNode(node, new Point(node.Position.X, node.Position.Y - 1));
			if (!DiagonalAllowed) return;
			TryCreateNode(node, new Point(node.Position.X + 1, node.Position.Y + 1));
			TryCreateNode(node, new Point(node.Position.X - 1, node.Position.Y + 1));
			TryCreateNode(node, new Point(node.Position.X - 1, node.Position.Y - 1));
			TryCreateNode(node, new Point(node.Position.X + 1, node.Position.Y - 1));
		}

		private void TryCreateNode(GridPathNode parent, Point pos) {
			if (ClosedNodes.Contains(pos)) return;
			if (IsSolidFunc.Invoke(parent.Position, pos)) return;

			var node = new GridPathNode(parent, pos);
			SolveNodeFunc.Invoke(node, End);

			for (int i = OpenNodes.Count - 1; i >= 0; i--) {
				var openNode = OpenNodes[i];
				if (openNode.Position == node.Position) {
					if (node.Traveled > openNode.Traveled) {
						return;
					}
					OpenNodes.RemoveAt(i);
					break;
				}
			}

			OpenNodes.Add(node);
		}

		private static bool IsSolid(Point from, Point to) {
			return false;
		}

		public static void SolveNode(GridPathNode node, Point end) {
			if (node.Parent.Position.X == node.Position.X || node.Parent.Position.Y == node.Position.Y) {
				node.Traveled = node.Parent.Traveled + 2;
			} else {
				node.Traveled = node.Parent.Traveled + 3;
			}
			var dif = new Point(Math.Abs(end.X - node.Position.X), Math.Abs(end.Y - node.Position.Y));
			var diagonal = Math.Min(dif.X, dif.Y);
			node.Remaining = ((dif.X - diagonal) * 2) + ((dif.Y - diagonal) * 2) + (diagonal * 3);
			node.Remaining *= 3;

			node.Remaining = Math.Abs(end.X - node.Position.X) * 2 + Math.Abs(end.Y - node.Position.Y) * 2;
			node.Total = node.Traveled + node.Remaining;
		}

		public class GridPathNode {
			public int Total;
			public int Traveled;
			public int Remaining;
			public Point Position;
			public GridPathNode Parent;

			public GridPathNode(GridPathNode parent, Point pos) {
				Parent = parent;
				Position = pos;
			}
		}

		public static Color ColorOpen = Color.DarkGreen * 0.8f;
		public static Color ColorClosed = Color.DarkRed * 0.5f;

		public void Draw(Renderer r, Camera c, Point scale) {
			foreach (var p in ClosedNodes) {
				r.DrawRect(new Rectangle(p * scale, scale), ColorClosed, c, Camera.Space.Pixel);
			}
			foreach (var node in OpenNodes) {
				var rect = r.DrawRect(new Rectangle(node.Position * scale, scale), ColorOpen, c, Camera.Space.Pixel);
				Resources.TextRenderer.Draw(r.Batch, Color.White, rect.Location, $"{node.Total}");
			}
		}

		public void DebugStart(Point start, Point end) {
			End = end;
			OpenNodes.Clear();
			ClosedNodes.Clear();

			OpenNodes.Add(new GridPathNode(null, start));
		}

		public List<Point> DebugUpdate() {
			int pathLength = OpenNodes.Count + ClosedNodes.Count;
			if (pathLength >= MaxPathLength) return new List<Point>();

			if (OpenNodes.Count > 0) {
				var nextnode = GetNextNode(out int index);
				OpenNodes.RemoveAt(index);
				ClosedNodes.Add(nextnode.Position);

				if (nextnode.Position == End) {
					return GetPath(nextnode);
				}

				ExploreNode(nextnode);
			}
			return null;
		}

	}
}
