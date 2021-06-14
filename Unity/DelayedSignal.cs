///////////////////////////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright(c) 2021 Henry de Jongh
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute,
// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
// NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
////////////////////// https://github.com/Henry00IS/CSharp ////////// http://00laboratories.com/ //

using UnityEngine;

namespace OOLaboratories.Unity
{
    /// <summary>
    /// Represents a delayed signal that when set, returns true on the next frame. Then it's
    /// automatically reset (unless set again), acts just like <seealso cref="Input.GetKeyDown"/>.
    /// </summary>
    [System.Serializable]
    public struct DelayedSignal
    {
        private int? frame1;
        private int? frame2;

        /// <summary>Sets the signal for the next frame.</summary>
        public void Set()
        {
            // prepare to set the state for the next frame.
            int currentFrame = Time.frameCount;
            int nextFrame = currentFrame + 1;

            // if the next frame is already set, do nothing.
            if (frame1 == nextFrame || frame2 == nextFrame)
                return;

            // find a free slot that isn't the current frame.
            if (frame1 != currentFrame) { frame1 = nextFrame; return; }
            if (frame2 != currentFrame) { frame2 = nextFrame; return; }
        }

        /// <summary>Returns true if the signal was set on the previous frame.</summary>
        public bool IsSet()
        {
            // get the state for the current frame.
            int currentFrame = Time.frameCount;

            // find a slot for the current frame.
            return (frame1 == currentFrame || frame2 == currentFrame);
        }

        /// <summary>Returns true if the signal was set on the previous frame.</summary>
        public static implicit operator bool(DelayedSignal signal)
        {
            return signal.IsSet();
        }
    }
}