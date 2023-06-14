using Microsoft.Xna.Framework;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Toybox.tools;
using Toybox.tools.info;
using Utils.text;

namespace Toybox.debug {
	public class DebugInfoPanel {

		public bool Active = false;
		private IDebugInfo Target;
		private Stack<IDebugInfo> History = new Stack<IDebugInfo>();
		public Text TextRenderer;
		private TextMeasurer TextM = new TextMeasurer();
		private DebugEditField EditField;

		private Rectangle HoverRect;
		private int HoverLine = -1;

		public DebugInfoPanel(Font f) {
			TextRenderer = new Text(f) { BackColor = Color.Black * 0.5f, 
				Color = Color.White, Position = new Point(1, 1), Scale = 1 };
			EditField = new DebugEditField(f);
		}

		public void SetTarget(object t, bool newstack) {
			if (t == null) return;
			if (newstack) History.Clear();
			var prev = Target;

			if (t is float) {
				EditField.SetContent((float)t);
			} else if (t is int) {
				EditField.SetContent((int)t);
			} else if (t is IDictionary) {
				Target = new DebugDictionaryInfo(t);
			}else if (t is IEnumerable) {
				Target = new DebugEnumerableInfo(t);
			} else if (t is object) {
				Target = new DebugObjectInfo(t);
			}

			if (!newstack && Target != prev) History.Push(prev);
		}

		public void PrevTarget() {
			if (History.Count > 0) Target = History.Pop();
		}

		public void Update() {
			if (Target == null) return;

			TextRenderer.Content = Target.GetText();
			TextM.Update(TextRenderer);

			if (EditField.Active) {
				var line = TextM.GetLine(HoverLine + 1);
				EditField.Position = new Point(line.Right + 2, line.Y + TextRenderer.LineSpace * TextRenderer.Scale);
				EditField.Update();
				if (!EditField.Active) Target.SetField(HoverLine, EditField.GetContent());
				return;
			}
			
			UpdateHoveredField();

			if (Resources.MouseInput.LeftPress) {
				if (HoverLine == -1) PrevTarget();
				else SetTarget(Target.TargetLine(HoverLine), false);
			}
		}

		private void UpdateHoveredField() {
			HoverRect = Rectangle.Empty;
			HoverLine = -2;
			var pick = TextM.PickLine(Resources.MouseInput.Position, out int linenum);
			if (pick.HasValue) {
				HoverRect = pick.Value;
				HoverLine = linenum - 1;
			}
		}

		public void Draw(Renderer r, Camera c) {
			if (TextRenderer.Content == "" || Target == null) return;

			if (HoverRect != Rectangle.Empty) r.DrawRectDirect(HoverRect, Color.Black);
			TextRenderer.Draw(r.Batch);
			if (EditField.Active) EditField.Draw(r, c);
		}

	}
}
