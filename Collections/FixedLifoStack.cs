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
    /// A last-in first-out stack that uses a single, fixed-size buffer. This implementation allows
    /// you to add elements without memory reallocations. It is useful to store a fixed amount of
    /// historical data, where [0] always points to the latest entry.
    /// </summary>
    /// <typeparam name="T">The generic type stored in this collection.</typeparam>
    public class FixedLifoStack<T> : IList<T>
    {
        /// <summary>The internal fixed buffer that contains all data.</summary>
        private T[] buffer;
        /// <summary>The amount of items in the circular buffer.</summary>
        private int count;
        /// <summary>The current index into the circular buffer, pointing to the current item.</summary>
        private int index;

        /// <summary>Creates a new collection of the specified fixed capacity.</summary>
        /// <param name="capacity">The fixed capacity of the collection.</param>
        public FixedLifoStack(int capacity)
        {
            if (capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity), "The collection capacity must be greater than zero.");
            buffer = new T[capacity];
        }

        /// <summary>Gets the fixed capacity of the collection.</summary>
        public int Capacity => buffer.Length;

        /// <summary>Gets the number of elements contained in the collection.</summary>
        public int Count => count;

        /// <summary>Gets a value indicating whether the collection is read-only.</summary>
        public bool IsReadOnly => false;

        /// <summary>Adds an item to the collection, overwriting the oldest data.</summary>
        /// <param name="item">The item to add to the collection.</param>
        public void Add(T item)
        {
            // increase count up to the collection capacity.
            if (count < buffer.Length) count++;
            // overwrite the current item pointed to by the index.
            buffer[index] = item;
            // decrement index.
            index = (index - 1) % buffer.Length;
            if (index < 0) index += buffer.Length;
        }

        /// <summary>Resets the item count to zero without clearing the internal buffer.</summary>
        public void Clear()
        {
            count = 0;
        }

        /// <summary>Removes all items from the collection.</summary>
        public void ReferenceClear()
        {
            Array.Clear(buffer, 0, buffer.Length);
            count = 0;
        }

        /// <summary>Determines whether the collection contains a specific item.</summary>
        /// <param name="item">The item to locate in the collection.</param>
        /// <returns>True if item is found in the collection; otherwise, false.</returns>
        public bool Contains(T item)
        {
            for (int i = 0; i < count; i++)
                if (this[i].Equals(item))
                    return true;
            return false;
        }

        /// <summary>
        /// Copies the elements of the collection to an array, starting at a particular array index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional array that is the destination of the elements copied from the
        /// collection. The array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0) throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            if (arrayIndex + count > array.Length) throw new ArgumentException("The number of elements in the source collection is greater than the available space from arrayIndex to the end of the destination array.", nameof(array));

            for (int i = 0; i < count; i++)
                array[arrayIndex++] = this[i];
        }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < count; i++)
                yield return this[i];
        }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>Removes the first occurrence of a specific object from the collection.</summary>
        /// <param name="item">The object to remove from the collection.</param>
        /// <returns>
        /// True if item was successfully removed from the collection; otherwise, false. This method
        /// also returns false if item is not found in the original collection.
        /// </returns>
        public bool Remove(T item)
        {
            // try finding the object in the collection.
            int itemIndex = IndexOf(item);
            if (itemIndex == -1) return false;

            // remove the item by index.
            RemoveAt(itemIndex);

            return true;
        }

        /// <summary>Removes the item at the specified index.</summary>
        /// <param name="itemIndex">The zero-based index of the item to remove.</param>
        public void RemoveAt(int itemIndex)
        {
            if (count == 0) throw new IndexOutOfRangeException("The collection is empty.");
            if (itemIndex < 0 || itemIndex > count) throw new ArgumentOutOfRangeException(nameof(itemIndex));

            // move all elements below item index upwards.
            for (int i = itemIndex; i < count - 1; i++)
                this[i] = this[i + 1];

            // decrement the count.
            count--;
        }

        /// <summary>Determines the index of a specific item in the collection.</summary>
        /// <param name="item">The object to locate in the collection.</param>
        /// <returns>The index of item if found in the collection; otherwise, -1.</returns>
        public int IndexOf(T item)
        {
            for (int i = 0; i < count; i++)
                if (this[i].Equals(item))
                    return i;
            return -1;
        }

        /// <summary>Inserts an item to the collection at the specified index.</summary>
        /// <param name="itemIndex">The zero-based index at which item should be inserted.</param>
        /// <param name="item">The object to insert into the collection.</param>
        public void Insert(int itemIndex, T item)
        {
            if (itemIndex == 0) { Add(item); return; }
            if (itemIndex < 0 || itemIndex >= count) throw new ArgumentOutOfRangeException(nameof(itemIndex));

            // increase count up to the collection capacity.
            if (count < buffer.Length) count++;

            // shift all items along.
            for (int i = count - 1; i-- > itemIndex;)
                this[i + 1] = this[i];

            // set inserted value.
            this[itemIndex] = item;
        }

        /// <summary>Gets or sets the item at the specified index.</summary>
        /// <param name="index">The index of the item to get or set.</param>
        public T this[int i]
        {
            get
            {
                if (i < 0 || i >= count) throw new IndexOutOfRangeException();
                return buffer[(index + 1 + i) % buffer.Length];
            }
            set
            {
                if (i < 0 || i >= count) throw new IndexOutOfRangeException();
                buffer[(index + 1 + i) % buffer.Length] = value;
            }
        }
    }
}