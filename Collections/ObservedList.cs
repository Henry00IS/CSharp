using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOLaboratories.Collections
{
    /// <summary>
    /// A <see cref="IList{T}"/> with CollectionChanged event.
    /// </summary>
    /// <typeparam name="T">The generic type stored in this list.</typeparam>
    /// <seealso cref="System.Collections.Generic.IList{T}"/>
    public class ObservedList<T> : IList<T>
    {
        /// <summary>
        /// The internal list used to store all items.
        /// </summary>
        private readonly List<T> m_List;

        /// <summary>
        /// Occurs when the collection has changed in any way.
        /// </summary>
        public event EventHandler CollectionChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservedList{T}"/> class.
        /// </summary>
        public ObservedList()
        {
            m_List = new List<T>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservedList{T}"/> class.
        /// </summary>
        /// <param name="capacity">The number of elements that the new list can initially store.</param>
        public ObservedList(int capacity)
        {
            m_List = new List<T>(capacity);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservedList{T}"/> class.
        /// <para>This constructor will immediately cause a <see cref="CollectionChanged"/> event.</para>
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new list.</param>
        public ObservedList(IEnumerable<T> collection)
        {
            m_List = new List<T>(collection);
            Changed();
        }

        /// <summary>
        /// Gets or sets the <see cref="T"/> at the specified index.
        /// <para>This operation will cause a <see cref="CollectionChanged"/> event.</para>
        /// </summary>
        /// <value>The <see cref="T"/>.</value>
        /// <param name="index">The index.</param>
        /// <returns>The <see cref="T"/>.</returns>
        public T this[int index] { get => m_List[index]; set { m_List[index] = value; Changed(); } }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        public int Count => m_List.Count;

        /// <summary>
        /// Gets a value indicating whether the <see
        /// cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// <para>This operation will cause a <see cref="CollectionChanged"/> event.</para>
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        public void Add(T item)
        {
            m_List.Add(item);
            Changed();
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// <para>This operation will cause a <see cref="CollectionChanged"/> event.</para>
        /// </summary>
        public void Clear()
        {
            m_List.Clear();
            Changed();
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains
        /// a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see
        /// cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
        /// </returns>
        public bool Contains(T item)
        {
            return m_List.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an
        /// <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements
        /// copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see
        /// cref="T:System.Array"/> must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">
        /// The zero-based index in <paramref name="array"/> at which copying begins.
        /// </param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            m_List.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return m_List.GetEnumerator();
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
        /// <returns>The index of <paramref name="item"/> if found in the list; otherwise, -1.</returns>
        public int IndexOf(T item)
        {
            return m_List.IndexOf(item);
        }

        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"/> at the
        /// specified index.
        /// <para>This operation will cause a <see cref="CollectionChanged"/> event.</para>
        /// </summary>
        /// <param name="index">
        /// The zero-based index at which <paramref name="item"/> should be inserted.
        /// </param>
        /// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
        public void Insert(int index, T item)
        {
            m_List.Insert(index, item);
            Changed();
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// <para>This operation will cause a <see cref="CollectionChanged"/> event.</para>
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see
        /// cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also
        /// returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        public bool Remove(T item)
        {
            bool result = m_List.Remove(item);
            Changed();
            return result;
        }

        /// <summary>
        /// Removes the <see cref="T:System.Collections.Generic.IList`1"/> item at the specified index.
        /// <para>This operation will cause a <see cref="CollectionChanged"/> event.</para>
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        public void RemoveAt(int index)
        {
            m_List.RemoveAt(index);
            Changed();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate
        /// through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_List.GetEnumerator();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return "Count = " + m_List.Count;
        }

        /// <summary>
        /// Executes the <see cref="CollectionChanged"/> event.
        /// </summary>
        private void Changed()
        {
            CollectionChanged?.Invoke(this, null);
        }
    }
}