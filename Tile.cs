using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Animation2D;

namespace PadhiarK_MP2
{
    class Tile
    {
        private bool hasMine;
        private bool reveal;
        private bool hitMine;
        private bool hasFlag;
        private int mineNum = 0;

        private int x;
        private int y;
        private int size;

        private Rectangle rect;
        private Texture2D mineImg;

        public Tile (int x, int y, int size, bool hasMine, bool reveal)
        {
            this.x = x;
            this.y = y;
            this.size = size;
            this.hasMine = hasMine;
            this.reveal = reveal;

            rect = new Rectangle(x,y,size,size);
        }

        public bool GetState ()
        {
            return reveal;
        }

        public void SetState (bool state)
        {
            reveal = state;
        }

        public Rectangle GetRect ()
        {
            return rect;
        }
        public int GetSize ()
        {
            return size;
        }

        public int GetX ()
        {
            return x;
        }

        public int GetY ()
        {
            return y;
        }

        public bool HasMine ()
        {
            return hasMine;
        }

        public void SetMine (bool mine)
        {
            hasMine = mine;
        }

        public void HitMine (bool hit)
        {
            hitMine = hit;
        }

        public void SetMineNum (int num)
        {
            mineNum = num;
        }

        public int GetAdjMineNum ()
        {
            return mineNum;
        }

        public bool IfHitMine ()
        {
            return hitMine;
        }

        public void SetMineImg (Texture2D image)
        {
            mineImg = image;
        }

        public Texture2D GetMineImg ()
        {
            return mineImg;
        }

        public bool HasFlag ()
        {
            return hasFlag;
        }

        public bool SetFlag (bool flag)
        {
            return hasFlag = flag;
        }

        public void DrawTile (SpriteBatch spriteBatch, Texture2D img)
        {

            spriteBatch.Draw(img, rect, Color.White);
        }
    }
}
