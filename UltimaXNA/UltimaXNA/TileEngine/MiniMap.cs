﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace UltimaXNA.TileEngine
{
    public class MiniMap
    {
        public Texture2D Texture;
        private Map _map;
        private GraphicsDevice _graphics;
        private int _lastUpdateTicker;

        public MiniMap(GraphicsDevice graphics, Map map)
        {
            _map = map;
            _graphics = graphics;
        }

        public unsafe void Update()
        {
            if ((_map.UpdateTicker != _lastUpdateTicker) || (Texture == null))
            {
                int size = _map.GameSize * 2;
                _lastUpdateTicker = _map.UpdateTicker;
                Texture = new Texture2D(_graphics, size, size, 1, TextureUsage.None, SurfaceFormat.Bgra5551);
                ushort[] data = new ushort[size * size];
                fixed (ushort* pData = data)
                {
                    for (int y = 0; y < _map.GameSize; y++)
                    {
                        ushort* cur = pData + ((size /2 - 1) + (size - 1) * y);
                        for (int x = 0; x < _map.GameSize; x++)
                        {
                            MapCell m = _map.GetMapCell(TileEngine.startX + x, TileEngine.startY + y);
                            int i;
                            for (i = m.Objects.Count - 1; i > 0; i--)
                            {
                                if (m.Objects[i].Type == MapObjectTypes.StaticTile)
                                {
                                    *cur++ = (ushort)(Data.Radarcol.Colors[m.Objects[i].ID] | 0x8000);
                                    *cur = (ushort)(Data.Radarcol.Colors[m.Objects[i].ID] | 0x8000);
                                    cur += size;
                                    break;
                                }
                            }
                            if (i == 0)
                            {
                                *cur++ = (ushort)(Data.Radarcol.Colors[m.GroundTile.ID] | 0x8000);
                                *cur = (ushort)(Data.Radarcol.Colors[m.Objects[i].ID] | 0x8000);
                                cur += size;
                            }
                        }
                    }
                }
                Texture.SetData<ushort>(data);
            }
        }
    }
}
