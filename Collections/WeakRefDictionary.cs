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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOLaboratories.Collections
{
    /// <summary>
    /// Ein <see cref="Dictionary{TKey, TValue}"/> mit swache Verweise.
    /// <para>Diese Einträge verweisen auf ein Objekt, ohne jedoch dessen Freigabe durch die Garbage Collection zu verhindern. Bei der Garbage Collection werden automatisch Einträge aus diesem Dictionary gelöscht. Null werte werden unterstützt.</para>
    /// <para>Es ist wichtig zu beachten, dass die Schlüssel nur entfernt werden sobalt auf das Wörterbuch zugegriffen wird (die Werte können bereits Garbage Collected sein). Im Fall, dass Objektinstanzen als Schlüssel verwendet werden und eine sofortige löschung aller Schlüssel mit einem bereits vom Garbage Collector freigegebener Wert gelöscht werden sollen, müssen Sie manuell CleanAbandonedItems() aufrufen oder auf das dictionary zugreifen (Schlüssel aufräumung findet automatisch statt bei meinDictionary[schluessel], .Count(), .Clear(), .Contains() und .ContainsKey(). Nicht bei Enumeratoren!).</para>
    /// <para>Ansprechpartner: Henry de Jongh.</para>
    /// </summary>
    public class WeakRefDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        /// <summary>Interne dictionary mit <see cref="WeakReference"/> Verweise.</summary>
        private readonly Dictionary<TKey, WeakReference> m_Dictionary = new Dictionary<TKey, WeakReference>();

        /// <summary>
        /// Ruft den Wert ab, der dem angegebenen Schlüssel zugeordnet ist, oder legt diesem fest.
        /// </summary>
        /// <param name="key">Der Schlüssel des abzurufenden oder festzulegenden Werts.</param>
        /// <returns>Den Wert, der dem angegebenen Schlüssel zugeordnet ist.</returns>
        /// <exception cref="KeyNotFoundException">Wird ausgelöst wenn der für den Zugriff auf ein Element in der Auflistung angegebenen Schlüssel keinem Schlüssel in der Auflistung entspricht.
        /// Weil der dictionary swache Verweise beinhaltet, kann der Schlüssel bereits entfernt worden sein durch die entfernung des entsprechenden Objektes vom Garbage Collector.</exception>
        public TValue this[TKey key]
        {
            get
            {
                TValue result;

                if (TryGetValue(key, out result))
                    return result;

                throw new KeyNotFoundException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Ruft die Anzahl der Schlüssel-Wert-Paare im <see cref="WeakRefDictionary{TKey, TValue}"/> ab.
        /// <para>Weil die <see cref="TValue"/>-Items im dictionary swache referenzen sind, kann die Anzahl der items im dictionary unterscheiden von das Ergebnis einer enumeration. Verwende Count nur als schätzung.</para>
        /// </summary>
        public int Count
        {
            get
            {
                CleanAbandonedItems();
                return m_Dictionary.Count;
            }
        }

        /// <summary>
        /// Ruft einen Wert ab, ob das <see cref="WeakRefDictionary{TKey, TValue}"/>-Objekt schreibgeschützt ist.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Fügt der <see cref="WeakRefDictionary{TKey, TValue}"/> ein Element hinzu.
        /// </summary>
        /// <param name="item">Das Objekt, das <see cref="WeakRefDictionary{TKey, TValue}"/> hinzugefügt werden soll.</param>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        /// <summary>
        /// Fügt der <see cref="WeakRefDictionary{TKey, TValue}"/> ein Element mit dem angegebenen Schlüssel und Wert hinzu.
        /// </summary>
        /// <param name="key">Das Objekt, das als Schlüssel für das hinzuzufügende Element verwendet werden soll.</param>
        /// <param name="value">Das Objekt, das als Wert für das hinzuzufügende Element verwendet werden soll.</param>
        /// <exception cref="Exception">Schlüssel ist bereits im Dictionary vorhanden.</exception>
        public void Add(TKey key, TValue value)
        {
            TValue dummy;
            if (TryGetValue(key, out dummy))
                throw new Exception("Schlüssel ist bereits im Dictionary vorhanden.");

            m_Dictionary.Add(key, new WeakReference(EncodeNullObject(value)));
        }

        /// <summary>
        /// Entfernt sämtliche Schlüssel und Werte aus dem <see cref="WeakRefDictionary{TKey, TValue}"/>.
        /// </summary>
        public void Clear()
        {
            m_Dictionary.Clear();
        }

        /// <summary>
        /// Bestimmt, ob das <see cref="WeakRefDictionary{TKey, TValue}"/> den angegebenen Wert enthält.
        /// </summary>
        /// <param name="item">Der im <see cref="WeakRefDictionary{TKey, TValue}"/> zu suchenden Wert.</param>
        /// <returns>Ob der angegebene Wert im dictionary vorhanden ist.</returns>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            TValue entry;
            if (TryGetValue(item.Key, out entry))
                return item.Value.Equals(entry);
            return false;
        }

        /// <summary>
        /// Bestimmt, ob das <see cref="WeakRefDictionary{TKey, TValue}"/> den angegebenen Schlüssel enthält.
        /// </summary>
        /// <param name="key">Der im <see cref="WeakRefDictionary{TKey, TValue}"/> zu suchenden Schlüssel.</param>
        /// <returns>Ob der angegebene Schlüssel im dictionary vorhanden ist.</returns>
        public bool ContainsKey(TKey key)
        {
            TValue dummy;
            return TryGetValue(key, out dummy);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gibt einen Enumerator zurück, der das <see cref="WeakRefDictionary{TKey, TValue}"/> durchläuft.
        /// </summary>
        /// <returns>Einen Enumerator, der das <see cref="WeakRefDictionary{TKey, TValue}"/> durchläuft.</returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach (KeyValuePair<TKey, WeakReference> kvp in m_Dictionary)
            {
                object innerValue = kvp.Value.Target;

                if (innerValue != null)
                    yield return new KeyValuePair<TKey, TValue>(kvp.Key, DecodeNullObject<TValue>(innerValue));
            }
        }

        /// <summary>
        /// Entfernt den Wert aus dem <see cref="WeakRefDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="item">Der zu entfernenden Wert.</param>
        /// <returns>True, wenn das Element gefunden und entfernt wurde, andernfalls false.</returns>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (Contains(item)) // wert muss übereinstimmen.
                return Remove(item.Key);
            return false;
        }

        /// <summary>
        /// Entfernt den Wert mit dem angegebenen Schlüssel aus dem <see cref="WeakRefDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="key">Der Schlüssel des zu entfernenden Elements.</param>
        /// <returns>True, wenn das Element gefunden und entfernt wurde, andernfalls false.</returns>
        public bool Remove(TKey key)
        {
            return m_Dictionary.Remove(key);
        }

        /// <summary>
        /// Ruft den dem angegebenen Schlüssel zugeordneten Wert ab.
        /// </summary>
        /// <param name="key">Der Schlüssel des abzurufenden Werts.</param>
        /// <param name="value">Enthält nach dem Beenden dieser Methode den Wert, der dem angegebenen Schlüssel zugeordnet ist, wenn der Schlüssel gefunden wurde, oder andernfalls den Standardwert für den Typ des value-Parameters. Dieser Parameter wird nicht initialisiert übergeben.</param>
        /// <returns>True, wenn das Element gefunden wurde, andernfalls false.</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            value = default(TValue);
            WeakReference wr;

            if (!m_Dictionary.TryGetValue(key, out wr))
                return false;

            object result = wr.Target;

            if (result == null)
            {
                m_Dictionary.Remove(key);
                return false;
            }

            value = DecodeNullObject<TValue>(result);
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Verweise die bereits vom Garbage Collector gelöscht wurden aus dem Dictionary löschen.
        /// <para>Nur dann wichtig im Fall, dass Objektinstanzen als Schlüssel verwendet werden und eine sofortige löschung aller Schlüssel mit einem bereits vom Garbage Collector freigegebener Wert gelöscht werden sollen.</para>
        /// <para>Diese Funktion wird bereits intern aufgerufen! Bitte beachten Sie die Klassen Dokumentation von <see cref="WeakRefDictionary{TKey, TValue}"/> für weitere Anweisungen und den spezifischen Anwendungsfall.</para>
        /// </summary>
        public void CleanAbandonedItems()
        {
            List<TKey> deadKeys = new List<TKey>();

            foreach (KeyValuePair<TKey, WeakReference> kvp in m_Dictionary)
                if (kvp.Value.Target == null)
                    deadKeys.Add(kvp.Key);

            foreach (TKey key in deadKeys)
                m_Dictionary.Remove(key);
        }

        /// <summary>
        /// Ein NullObject zurück verwandeln.
        /// </summary>
        /// <typeparam name="TObject">Den typ für einen standardwert.</typeparam>
        /// <param name="innerValue">Der wert.</param>
        /// <returns>Ein standardobjekt oder der wert.</returns>
        protected static TObject DecodeNullObject<TObject>(object innerValue)
        {
            if (innerValue is NullObject)
                return default(TObject);
            else
                return (TObject)innerValue;
        }

        /// <summary>
        /// Erstellt eine NullObject für 'null' oder gibt den gleichen Wert zurück.
        /// </summary>
        /// <param name="value">Der wert.</param>
        /// <returns>Ein NullObject für 'null' oder den value parameter.</returns>
        protected static object EncodeNullObject(object value)
        {
            if (value == null)
                return typeof(NullObject);
            else
                return value;
        }

        /// <summary>
        /// Eine alternative Form für 'null' der in ein Dictionary gespeichert werden kann.
        /// </summary>
        protected class NullObject { }
    }
}
