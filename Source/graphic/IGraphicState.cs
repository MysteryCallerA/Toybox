using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.graphic {
	public interface IGraphicState {

		public void GetDrawData(Point destPos, out Texture2D graphic, out Rectangle source, out Rectangle dest); 

	}
}
