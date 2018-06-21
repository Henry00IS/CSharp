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
using System.Linq;

namespace OOLaboratories.Proprietary.UnrealGold
{
    /// <summary>
    /// Represents an Unreal Editor 1 map.
    /// </summary>
    public class T3dMap
    {
        /// <summary>
        /// Gets the actors in the map.
        /// </summary>
        /// <value>The actors in the map.</value>
        public List<T3dActor> Actors /*{ get; }*/ = new List<T3dActor>();

        /// <summary>
        /// Gets the brush models in the map.
        /// </summary>
        /// <value>The brush models in the map.</value>
        public List<T3dBrush> BrushModels /*{ get; }*/ = new List<T3dBrush>();

        /// <summary>
        /// Gets the brush actors in the level.
        /// </summary>
        /// <value>The brush actors in the level.</value>
        public List<T3dActor> Brushes { get { return Actors.Where(a => a.Class == "Brush" && a.BrushName != "Brush").ToList(); } }

        /// <summary>
        /// Gets the map title.
        /// </summary>
        /// <value>The map title.</value>
        public string Title
        {
            get
            {
                if (LevelInfo == null) return "";
                object value;
                if (LevelInfo.Properties.TryGetValue("Title", out value))
                    return value as string;
                return "";
            }
        }

        /// <summary>
        /// Gets the map author.
        /// </summary>
        /// <value>The map author.</value>
        public string Author
        {
            get
            {
                if (LevelInfo == null) return "";
                object value;
                if (LevelInfo.Properties.TryGetValue("Author", out value))
                    return value as string;
                return "";
            }
        }

        /// <summary>
        /// Gets the level information actor.
        /// </summary>
        /// <value>The level information actor.</value>
        public T3dActor LevelInfo { get { return Actors.FirstOrDefault(a => a.Class == "LevelInfo"); } }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return "Unreal Engine 1 Map \"" + Title + "\"";
        }
    }
}