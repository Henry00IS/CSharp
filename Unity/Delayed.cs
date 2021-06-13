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
    /// Represents a delayed type that allows you to assign a value for the next frame. Until then
    /// it will return the value that was set on previous frames.
    /// </summary>
    /// <typeparam name="T">The generic type stored in this struct.</typeparam>
	[System.Serializable]
    public struct Delayed<T>
    {
        private int lastFrame;
        private T nextValue;
        private T currentValue;

        /// <summary>Refreshes the current value by checking the frame count.</summary>
        private void Refresh()
        {
            int frameCount = Time.frameCount;
            if (lastFrame != frameCount)
            {
                currentValue = nextValue;
                lastFrame = frameCount;
            }
        }

        /// <summary>
        /// Gets the value that was set on previous frames or sets the value for the next frame.
        /// </summary>
        public T value
        {
            get { Refresh(); return currentValue; }
            set { Refresh(); nextValue = value; }
        }

        /// <summary>Calls ToString() on the current value that was set on previous frames.</summary>
        public override string ToString() => value.ToString();
    }
}