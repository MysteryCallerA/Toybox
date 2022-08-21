using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Utils.data;
using Utils.save;

namespace Toybox.maps {
	public class EntityMap:IXmlSaveable {

		private Dictionary<Point, LinkedList<int>> Map = new Dictionary<Point, LinkedList<int>>();
		private StableList<Entity> Entities = new StableList<Entity>();

		private bool Updating = false;
		private HashSet<Entity> WillAdd = new HashSet<Entity>();
		private HashSet<Entity> WillRemove = new HashSet<Entity>();

		private int CellWidth;
		private int CellHeight;

		private List<Entity> VisibleEntities = new List<Entity>();

		public EntityMap(int cellwidth, int cellheight) {
			CellWidth = cellwidth;
			CellHeight = cellheight;
		}

		public void Draw(SpriteBatch s, Camera c) {
			var cells = GetVisibleCells(c.GetWorldBounds());

			VisibleEntities.Clear();
			foreach (var cell in cells) {
				foreach (var id in Map[cell]) {
					VisibleEntities.Add(Entities[id]);
				}
			}
			VisibleEntities.Sort();

			foreach (var e in VisibleEntities) {
				e.Draw(s, c);
			}
		}

		private List<Point> GetVisibleCells(Rectangle bounds) {
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

		public void Add(Entity e) {
			if (Updating) {
				WillAdd.Add(e);
				return;
			}

			int id = Entities.Add(e);
			e.Id = id;
			AddToMap(e);
		}

		public void Add(ICollection<Entity> ents) {
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

		public void Remove(Entity e) {
			if (Updating) {
				WillRemove.Add(e);
				return;
			}

			Entities.RemoveAt(e.Id);
			RemoveFromMap(e);
			e.Id = -1;
		}

		public void Update() {
			Updating = true;
			foreach (Entity e in Entities) {
				if (e == null) continue;
				e.Update();
				UpdateMapPosition(e);
			}
			Updating = false;

			foreach (Entity e in WillAdd) {
				Add(e);
			}
			WillAdd.Clear();

			foreach (Entity e in WillRemove) {
				Remove(e);
			}
			WillRemove.Clear();
		}

		private void AddToMap(Entity e) {
			var cell = GetCell(e.X, e.Y);
			if (!Map.ContainsKey(cell)) {
				Map.Add(cell, new LinkedList<int>());
			}
			Map[cell].AddLast(e.Id);
			e.MapCell = cell;
		}

		private void RemoveFromMap(Entity e) {
			if (Map[e.MapCell].Count == 1) {
				Map.Remove(e.MapCell);
				return;
			}
			Map[e.MapCell].Remove(e.Id);
		}

		private void UpdateMapPosition(Entity e) {
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

		public Entity GetEntity(Point gamepos) {
			var possible = new List<Entity>();
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
				if (e.GetHitbox().Contains(gamepos)) return e;
			}
			return null;
		}

		public Entity GetEntity(int id) {
			return Entities[id];
		}

		public bool TryGetEntity(int id, out Entity e) {
			if (!Entities.IsIdValid(id)) {
				e = null;
				return false;
			}
			e = GetEntity(id);
			return e != null;
		}

		private void AddEntitiesFromCell(List<Entity> output, Point cell) {
			if (!Map.ContainsKey(cell)) return;
			foreach (var id in Map[cell]) {
				output.Add(Entities[id]);
			}
		}

		/// <summary> Checks if the size of cells is large enough for the contained entities. Cells should be at least as large as the largest entity. </summary>
		public bool ValidateCellSize() {
			foreach (Entity e in Entities) {
				if (e == null) continue;
				var box = e.GetHitbox();
				if (box.Width > CellWidth || box.Height > CellHeight) return false;
			}
			return true;
		}

		public void Save(XmlWriter writer) {
		}
	}
}
