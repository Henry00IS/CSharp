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
    /// Represents Unreal Editor 1's Polygon Flags.
    /// </summary>
    [Flags]
    public enum T3dPolygonFlags
    {
        Invisible = 1,
        Masked = 2,
        Translucent = 4,
        Environment = 16,
        Modulated = 64,
        FakeBackdrop = 128,
        TwoSided = 256,
        UPan = 512,
        VPan = 1024,
        NoSmooth = 2048,
        SpecialPoly = 4096,
        SmallWavy = 8192,
        ForceViewZone = 16384,
        LowShadowDetail = 32768,
        AlphaBlend = 131072,
        DirtyShadows = 262144,
        BrightCorners = 524288,
        SpecialLit = 1048576,
        NoBoundsReject = 2097152,
        Unlit = 4194304,
        HighShadowDetail = 8388608,
        Portal = 67108864,
        Mirror = 134217728
    }
}