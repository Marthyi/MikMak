﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MikMakCommons
{
    /// <summary>
    /// This is a class that represent or an Existant Pawn, or a click where you want a pawn to be set.
    /// </summary>
    public class Pawn
    {
        /// <summary>
        /// Could be 'Q' for Queen, as it could be 'B' for black...        
        /// </summary>
        public char Name { get; set; }
        public Point Coord { get; set; }

        public Pawn(char name, int x, int y) : this(){
            this.Name = name;
            Coord.x = x;
            Coord.y = y;
        }

        public Pawn() {
            Coord = new Point();
        }
    }
}