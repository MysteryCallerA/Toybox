using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.load {
	public interface IAssetLoader<T> {

		/// <summary> Filepath must include Content.RootDirectory and .xnb file extension. </summary>
		public T Load(string assetName);

		/// <summary> Filepath must include Content.RootDirectory and .xnb file extension. </summary>
		public bool IsLoadable(string assetName);

	}
}
