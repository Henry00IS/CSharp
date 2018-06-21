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

namespace OOLaboratories.Proprietary.UnrealGold
{
    /// <summary>
    /// Represents Unreal Editor 1's Brush Flags.
    /// </summary>
    [Flags]
    public enum T3dBrushFlags
    {
        /// <summary>
        /// Whether the brush is invisible.
        /// </summary>
        Invisible = 1,

        /// <summary>
        /// Whether the brush is using masked textures.
        /// </summary>
        Masked = 2,

        /// <summary>
        /// Whether the brush is using transparent rendering.
        /// </summary>
        Transparent = 4,

        /// <summary>
        /// Whether the brush doesn't have collision.
        /// </summary>
        NonSolid = 8,

        /// <summary>
        /// Whether the brush is essentially Sabre's NoCSG.
        /// </summary>
        SemiSolid = 32,

        /// <summary>
        /// Whether the brush is used to split off sections of the world.
        /// </summary>
        ZonePortal = 67108864,

        /// <summary>
        /// Whether the brush is using two-sided rendering.
        /// </summary>
        TwoSided = 256
    }
}