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

using System;
using System.Collections;
using System.Collections.Generic;

namespace OOLaboratories.Collections
{
    /// <summary>
    /// A circular buffer that uses a single, fixed-size buffer as if it were connected end-to-end.
    /// This implementation allows you to add elements without memory reallocations or expensive
    /// loops (doesn't apply to all operations- all slower <see cref="IList{T}"/> methods were
    /// marked in the summary) in LIFO. It is useful to store a fixed amount of historical data,
    /// where [0] always points to the latest entry.
    /// </summary>
    /// <typeparam name="T">The generic type stored in this circular buffer.</typeparam>
    public class CircularBuffer<T> : IList<T>
    {
        /// <summary>The internal fixed buffer that contains all data.</summary>
        private T[] buffer;
        /// <summary>The amount of items in the circular buffer.</summary>
        private int count;
        /// <summary>The current index into the circular buffer, pointing to the current item.</summary>
        private int index;

        /// <summary>Creates a new circular buffer of the specified fixed capacity.</summary>
        /// <param name="capacity">The fixed capacity of the circular buffer.</param>
        public CircularBuffer(int capacity)
        {
            if (capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity), "The circular buffer capacity must be greater than zero.");
            buffer = new T[capacity];
        }

        /// <summary>(Fast) Gets the number of elements contained in the <see cref="CircularBuffer{T}"/>.</summary>
        public int Count => count;

        /// <summary>Gets a value indicating whether the <see cref="CircularBuffer{T}"/> is read-only.</summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// (Fast) Adds an item to the <see cref="CircularBuffer{T}"/>, overwriting old data when it
        /// wraps around.
        /// </summary>
        /// <param name="item">The item to add to the <see cref="CircularBuffer{T}"/>.</param>
        public void Add(T item)
        {
            // increase count up to the circular buffer capacity.
            if (count < buffer.Length) count++;
            // overwrite the current item pointed to by the index.
            buffer[index] = item;
            // increment index.
            index = (index + 1) % buffer.Length;
        }

        /// <summary>(Fast) Removes all items from the <see cref="CircularBuffer{T}"/>.</summary>
        public void Clear()
        {
            count = 0;
            index = 0;
        }

        /// <summary>
        /// (Partial Loop) Determines whether the <see cref="CircularBuffer{T}"/> contains a
        /// specific item.
        /// </summary>
        /// <param name="item">The item to locate in the <see cref="CircularBuffer{T}"/>.</param>
        /// <returns>
        /// True if item is found in the <see cref="CircularBuffer{T}"/>; otherwise, false.
        /// </returns>
        public bool Contains(T item)
        {
            for (int i = count; i-- > 0;)
                if (buffer[(index + i) % count].Equals(item)) return true;
            return false;
        }

        /// <summary>
        /// (Loop) Copies the elements of the <see cref="CircularBuffer{T}"/> to an <see
        /// cref="System.Array"/>, starting at a particular <see cref="System.Array"/> index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional <see cref="System.Array"/> that is the destination of the elements
        /// copied from <see cref="CircularBuffer{T}"/>. The <see cref="System.Array"/> must have
        /// zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0) throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            if (arrayIndex + count > array.Length) throw new ArgumentException(nameof(array), "The number of elements in the source collection is greater than the available space from arrayIndex to the end of the destination array.");

            for (int i = count; i-- > 0;)
                array[arrayIndex++] = buffer[(index + i) % count];
        }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = count; i-- > 0;)
                yield return buffer[(index + i) % count];
        }

        /// <summary>
        /// (Loop, Memory Allocation, Slow) Removes the first occurrence of a specific object from
        /// the <see cref="CircularBuffer{T}"/>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="CircularBuffer{T}"/>.</param>
        /// <returns>
        /// True if item was successfully removed from the <see cref="CircularBuffer{T}"/>;
        /// otherwise, false. This method also returns false if item is not found in the original
        /// <see cref="CircularBuffer{T}"/>.
        /// </returns>
        public bool Remove(T item)
        {
            // try finding the item in the buffer.
            int itemIndex = IndexOf(item);
            if (itemIndex == -1) return false;

            // copy all items into a new buffer excluding the item.
            T[] temp = new T[buffer.Length];
            var j = 0;
            for (int i = count; i-- > 0;)
            {
                if (i == itemIndex) continue;
                temp[j] = this[i];
                j++;
            }

            // reset this buffer to the new buffer.
            buffer = temp;
            index = count - 1;
            count -= 1;

            return true;
        }

        /// <summary>
        /// (Loop, Memory Allocation, Slow) Removes the <see cref="CircularBuffer{T}"/> item at the
        /// specified index.
        /// </summary>
        /// <param name="itemIndex">The zero-based index of the item to remove.</param>
        public void RemoveAt(int itemIndex)
        {
            if (itemIndex < 0) throw new ArgumentOutOfRangeException(nameof(itemIndex));
            if (count == 0) throw new IndexOutOfRangeException("The collection is empty.");
            itemIndex %= count;

            // copy all items into a new buffer excluding the item.
            T[] temp = new T[buffer.Length];
            var j = 0;
            for (int i = count; i-- > 0;)
            {
                if (i == itemIndex) continue;
                temp[j] = this[i];
                j++;
            }

            // reset this buffer to the new buffer.
            buffer = temp;
            index = count - 1;
            count -= 1;
        }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>(Partial Loop) Determines the index of a specific item in the <see cref="CircularBuffer{T}"/>.</summary>
        /// <param name="item">The object to locate in the <see cref="CircularBuffer{T}"/>.</param>
        /// <returns>The index of item if found in the list; otherwise, -1.</returns>
        public int IndexOf(T item)
        {
            for (int i = count; i-- > 0;)
                if (buffer[(index + i) % count].Equals(item)) return count - i - 1;
            return -1;
        }

        /// <summary>
        /// (Partial Loop, Slow) Inserts an item to the <see cref="CircularBuffer{T}"/> at the
        /// specified index.
        /// </summary>
        /// <param name="itemIndex">The zero-based index at which item should be inserted.</param>
        /// <param name="item">The object to insert into the <see cref="CircularBuffer{T}"/>.</param>
        public void Insert(int itemIndex, T item)
        {
            if (itemIndex < 0) throw new ArgumentOutOfRangeException(nameof(itemIndex));
            if (itemIndex >= count) throw new ArgumentOutOfRangeException(nameof(itemIndex));
            // same as add.
            if (itemIndex == 0 || count == 0) { Add(item); return; }

            // shift all items along.
            for (int i = count - 1; i-- > itemIndex;)
                this[i + 1] = this[i];

            // set inserted value.
            this[itemIndex] = item;
        }

        /// <summary>
        /// (Fast) Gets or sets the item at the specified index (the collection cannot be empty!).
        /// </summary>
        /// <param name="index">The index of the item to get or set.</param>
        /// <exception cref="IndexOutOfRangeException">The collection cannot be empty!</exception>
        public T this[int i]
        {
            get
            {
                if (count == 0) throw new IndexOutOfRangeException("The collection is empty.");
                return buffer[(index + (count - (i % count) - 1)) % count];
            }
            set
            {
                if (count == 0) throw new IndexOutOfRangeException("The collection is empty.");
                buffer[(index + (count - (i % count) - 1)) % count] = value;
            }
        }
    }
}