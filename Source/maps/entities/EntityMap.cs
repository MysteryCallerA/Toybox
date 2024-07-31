using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Toybox.utils.data;

namespace Toybox.maps.entities {

	public class EntityMap:EntityMap<Entity> {
		public EntityMap(int cellwidth, int cellheight):base(cellwidth, cellheight) {
		}
	}

	public class EntityMap<T>:IEntityCollection<T> where T : Entity {

		private Dictionary<Point, LinkedList<int>> Map = new Dictionary<Point, LinkedList<int>>();
		private StableList<T> Entities = new StableList<T>();
		private EntityLockoutBuffer<T> Buffer = new EntityLockoutBuffer<T>();

		private int CellWidth;
		private int CellHeight;

		private List<T> VisibleEntities = new List<T>();

		public EntityMap(int cellwidth, int cellheight) {
			CellWidth = cellwidth;
			CellHeight = cellheight;
		}

		public void Draw(Renderer r, Camera c) {
			VisibleEntities = GetEntitiesNearBounds(c.Bounds);//Possible bug: this might need to be c.GetGameBounds?
			VisibleEntities.Sort();

			foreach (var e in VisibleEntities) {
				e.Draw(r, c);
			}
		}

		public void Add(T e) {
			if (Resources.Game.InUpdateStep) { Buffer.QueueAdd(e); return; }

			int id = Entities.Add(e);
			e.Id = id;
			AddToMap(e);
		}

		public void Add(ICollection<T> ents) {
			foreach (var e in ents) {
				Add(e);
			}
		}

		public Entity Find(string name) {
			foreach (Entity e in Entities) {
				if (e == null) continue;
				if (e.Name == name) return e;
			}
			return null;
		}

		public T Find(Point pos) {
			return GetEntity(pos);
		}

		public void Remove(T e) {
			if (Resources.Game.InUpdateStep) { Buffer.QueueAdd(e); return; }

			Entities.RemoveAt(e.Id);
			RemoveFromMap(e);
			e.Id = -1;
		}

		public void Update() {
			foreach (T e in Entities) {
				if (e == null) continue;
				e.Update();
				UpdateMapPosition(e);
			}
		}

		public void PostUpdate() {
			Buffer.Apply(this);
		}

		private void AddToMap(T e) {
			var cell = GetCell(e.X, e.Y);
			if (!Map.ContainsKey(cell)) {
				Map.Add(cell, new LinkedList<int>());
			}
			Map[cell].AddLast(e.Id);
			e.MapCell = cell;
		}

		private void RemoveFromMap(T e) {
			if (Map[e.MapCell].Count == 1) {
				Map.Remove(e.MapCell);
				return;
			}
			Map[e.MapCell].Remove(e.Id);
		}

		private void UpdateMapPosition(T e) {
			var cell = GetCell(e.X, e.Y);
			if (cell == e.MapCell) return;

			if (Map[e.MapCell].Count == 1) {
				var c = Map[e.MapCell];
				Map.Remove(e.MapCell);
				if (Map.ContainsKey(cell)) {
					Map[cell].AddLast(e.Id);
				} else {
					Map.Add(cell, c);
				}
				e.MapCell = cell;
				return;
			}

			Map[e.MapCell].Remove(e.Id);
			if (!Map.ContainsKey(cell)) {
				Map.Add(cell, new LinkedList<int>());
			}
			Map[cell].AddLast(e.Id);
			e.MapCell = cell;
		}

		private Point GetCell(int x, int y) {
			return new Point((int)Math.Floor((float)x / CellWidth), (int)Math.Floor((float)y / CellHeight));
		}

		public T GetEntity(Point gamepos) {
			var possible = new List<T>();
			var cell = GetCell(gamepos.X, gamepos.Y);
			AddEntitiesFromCell(possible, cell);
			cell.X -= 1;
			AddEntitiesFromCell(possible, cell);
			cell.Y -= 1;
			AddEntitiesFromCell(possible, cell);
			cell.X += 1;
			AddEntitiesFromCell(possible, cell);

			possible.Sort();
			foreach (var e in possible) {
				if (e.Hitbox.Bounds.Contains(gamepos)) return e;
			}
			return null;
		}

		public T GetEntity(int id) {
			return Entities[id];
		}

		public bool TryGetEntity(int id, out T e) {
			if (!Entities.IsIdValid(id)) {
				e = null;
				return false;
			}
			e = GetEntity(id);
			return e != null;
		}

		private void AddEntitiesFromCell(List<T> output, Point cell) {
			if (!Map.ContainsKey(cell)) return;
			foreach (var id in Map[cell]) {
				output.Add(Entities[id]);
			}
		}

		/// <summary> Checks if the size of cells is large enough for the contained entities. Cells should be at least as large as the largest entity. </summary>
		public bool ValidateCellSize() {
			foreach (T e in Entities) {
				if (e == null) continue;
				var box = e.Hitbox.Bounds;
				if (box.Width > CellWidth || box.Height > CellHeight) return false;
			}
			return true;
		}

		private List<Point> GetCellsNearBounds(Rectangle bounds) {
			var topleft = GetCell(bounds.X, bounds.Y);
			topleft.X -= 1;
			topleft.Y -= 1;
			var botright = GetCell(bounds.Right, bounds.Bottom);

			var output = new List<Point>();
			for (int x = topleft.X; x <= botright.X; x++) {
				for (int y = topleft.Y; y <= botright.Y; y++) {
					var p = new Point(x, y);
					if (Map.ContainsKey(p)) {
						output.Add(p);
					}
				}
			}
			return output;
		}

		public List<T> GetEntitiesNearBounds(Rectangle bounds) {
			var cells = GetCellsNearBounds(bounds);
			var output = new List<T>();
			foreach (var cell in cells) {
				foreach (var id in Map[cell]) {
					output.Add(Entities[id]);
				}
			}
			return output;
		}

		public IEnumerable<T> GetCollisions(Rectangle collider) {
			var cells = GetCellsNearBounds(collider);
			foreach (var cell in cells) {
				foreach (var id in Map[cell]) {
					var e = Entities[id];
					if (e.Hitbox.Bounds.Intersects(collider)) {
						yield return e;
					}
				}
			}
		}
	}
}
