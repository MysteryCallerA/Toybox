using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Toybox.maps;
using Utils.graphic;

namespace Toybox.tiled
{

    public class TiledFile
    {

        public Dictionary<string, TileLayer> TileLayers = new Dictionary<string, TileLayer>();
        public Dictionary<string, ObjectLayer> ObjectLayers = new Dictionary<string, ObjectLayer>();
        public Dictionary<string, Tileset> Tilesets = new Dictionary<string, Tileset>();
        public List<string> TilesetOrder = new List<string>();

        public class TileLayer
        {
            public string Name;
            public int LayerPos;
            public int Width;
            public int Height;
            /// <summary> This collection is in Y,X format!!! </summary>
            public List<List<int>> Data;
        }

        public class ObjectLayer
        {
            public string Name;
            public int LayerPos;
            public List<TiledObject> Objects;//mabye convert this to a dictionary
        }

        public class TiledObject
        {
            public string Name;
            public Point Position;
        }

        public class Tileset
        {
            public string Name;
            public int FirstGid;
        }

        public TiledFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new Exception("File not found. Path:" + path);
            }

            string xml = File.ReadAllText(path);
            if (path.EndsWith(".tmx"))
            {
                ParseXml(xml);
                return;
            }

            throw new Exception("Unsupported file format");
        }

        private void ParseXml(string xml)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                var map = doc.SelectSingleNode("map");
                int layerPos = 0;

                foreach (XmlNode node in map.ChildNodes)
                {
                    if (node.Name == "layer")
                    {
                        ParseTileLayer(node, layerPos);
                        layerPos++;
                    }
                    else if (node.Name == "objectgroup")
                    {
                        ParseObjectLayer(node, layerPos);
                        layerPos++;
                    }
                    else if (node.Name == "tileset")
                    {
                        ParseTileset(node);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error occurred while parsing Tiled file:", e);
            }
        }

        //NEXT add a way of converting these semi-loaded layer objects into useful objects like Tilemap

        private void ParseTileLayer(XmlNode node, int layerPos)
        {
            TileLayer layer = new TileLayer();
            layer.Name = node.Attributes["name"].Value;
            layer.LayerPos = layerPos;
            layer.Width = int.Parse(node.Attributes["width"].Value);
            layer.Height = int.Parse(node.Attributes["height"].Value);

            string[] lines = node.SelectSingleNode("data").InnerText.Split(Environment.NewLine);
            var layerData = new List<List<int>>();
            for (int i = 0; i < lines.Length; i++)
            {
                string[] line = lines[i].Split(',');
                var lineData = new List<int>();
                for (int tile = 0; tile < line.Length; tile++)
                {
                    lineData.Add(int.Parse(line[tile]));
                }
                layerData.Add(lineData);
            }
            layer.Data = layerData;

            TileLayers.Add(layer.Name, layer);
        }

        private void ParseObjectLayer(XmlNode node, int layerPos)
        {
            ObjectLayer layer = new ObjectLayer();
            layer.Name = node.Attributes["name"].Value;
            layer.LayerPos = layerPos;
            foreach (XmlNode data in node.ChildNodes)
            { //TODO not sure how this will work with other kinds of objects, this might only work with points
                TiledObject o = new TiledObject();
                o.Name = data.Attributes["name"].Value;
                o.Position = new Point(int.Parse(data.Attributes["x"].Value), int.Parse(data.Attributes["y"].Value));
                layer.Objects.Add(o);
            }

            ObjectLayers.Add(layer.Name, layer);
        }

        private void ParseTileset(XmlNode node)
        {
            var tileset = new Tileset();
            tileset.Name = node.Attributes["source"].Value;
            tileset.FirstGid = int.Parse(node.Attributes["firstgid"].Value);
            Tilesets.Add(tileset.Name, tileset);
            for (int i = 0; i <= TilesetOrder.Count; i++)
            {
                if (i == TilesetOrder.Count)
                {
                    TilesetOrder.Add(tileset.Name);
                    break;
                }
                if (Tilesets[TilesetOrder[i]].FirstGid > tileset.FirstGid)
                {
                    TilesetOrder.Insert(i, tileset.Name);
                    break;
                }
            }
        }

        public Tilemap GetTilemap(string layerName, string tilesetName, TextureGrid t)
        {
            var data = new List<List<Tilemap.Tile>>();
            int sub = Tilesets[tilesetName].FirstGid;
            TileLayer layer = TileLayers[layerName];

            for (int x = 0; x < layer.Width; x++)
            {
                List<Tilemap.Tile> col = new List<Tilemap.Tile>();
                for (int y = 0; y < layer.Height; y++)
                {
                    if (layer.Data[y][x] == 0)
                    {
                        col.Add(new Tilemap.Tile());
                        continue;
                    }

                }
                data.Add(col);
            }

            return new Tilemap(t, data);
        }

    }
}
