///////////////////////////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright(c) 2018 Henry de Jongh
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
////////////////////// https://github.com/Henry00IS/CSharp ////////// http://00laboratories.com/ //

using System;
using System.Collections.Generic;

namespace OOLaboratories.Proprietary.UnrealGold
{
    /// <summary>
    /// Represents an Unreal Editor 1 brush.
    /// </summary>
    public class T3dBrush
    {
        /// <summary>
        /// Gets the name of the brush.
        /// </summary>
        /// <value>The name of the brush.</value>
        public string Name /*{ get; }*/;

        /// <summary>
        /// Gets or sets the brush polygons.
        /// </summary>
        /// <value>The brush polygons.</value>
        public List<T3dPolygon> Polygons /*{ get; }*/ = new List<T3dPolygon>();

        /// <summary>
        /// Initializes a new instance of the <see cref="T3dBrush"/> class.
        /// </summary>
        /// <param name="name">The name of the brush.</param>
        public T3dBrush(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return "Unreal Engine 1 Brush Model \"" + Name + "\" (" + Polygons.Count + " Polygons)";
        }
    }
}