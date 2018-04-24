using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOLaboratories.Collections
{
    /// <summary>
    /// Ein <see cref="IList{T}"/> mit swache Verweise.
    /// <para>Diese Einträge verweisen auf ein Objekt, ohne jedoch dessen Freigabe durch die Garbage Collection zu verhindern. Bei der Garbage Collection werden automatisch Einträge aus diesem List gelöscht.</para>
    /// <para>Es ist wichtig zu beachten, dass die Einträge nur entfernt werden sobalt auf die Liste zugegriffen wird (die Werte können bereits Garbage Collected sein). Im Fall, dass eine sofortige löschung aller Einträge mit einem bereits vom Garbage Collector freigegebener Wert gelöscht werden sollen, müssen Sie manuell CleanAbandonedItems() aufrufen oder auf die Liste zugreifen (Eintrag aufräumung findet automatisch statt bei meineListe[index], .Count(), .Clear(), .Contains(), .IndexOf() und .Remove(). Nicht bei Enumeratoren!).</para>
    /// <para>Ansprechpartner: Henry de Jongh.</para>
    /// </summary>
    public class WeakRefList<T> : IList<T>
    {
        /// <summary>Interne liste mit <see cref="WeakReference"/> Verweise.</summary>
        private readonly List<WeakReference> m_List = new List<WeakReference>();

        /// <summary>
        /// Ruft das Element am angegebenen Index ab oder legt dieses fest.
        /// </summary>
        /// <param name="index">Der nullbasierte Index des Elements, das abgerufen oder festgelegt werden soll.</param>
        /// <returns>Den Wert, der dem angegebenen index zugeordnet ist.</returns>
        /// <exception cref="IndexOutOfRangeException">Wird ausgelöst wenn der für den Zugriff auf ein Element in der Auflistung angegebenen index außerhalb der Begrenzungen des Arrays befindet.
        /// Weil die liste swache Verweise beinhaltet, kann der index bereits entfernt oder verändert worden sein durch die entfernung des entsprechenden Objektes vom Garbage Collector.</exception>
        public T this[int index]
        {
            get
            {
                T result;

                if (TryGetValue(index, out result))
                    return result;

                throw new IndexOutOfRangeException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Ruft die Anzahl der Elemente im <see cref="WeakRefList{T}"/> ab.
        /// <para>Weil die Elemente in die Liste swache verweise sind, kann die Anzahl der items in die liste unterscheiden von das Ergebnis einer enumeration. Verwende Count nur als schätzung.</para>
        /// </summary>
        public int Count
        {
            get
            {
                try
                {
                    CleanAbandonedItems();
                    return m_List.Count;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Ruft einen Wert ab, ob das <see cref="WeakRefList{T}"/>-Objekt schreibgeschützt ist.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Fügt am Ende der <see cref="WeakRefList{T}"/> ein Objekt hinzu.
        /// </summary>
        /// <param name="item">Das Objekt, das am Ende der <see cref="WeakRefList{T}"/> hinzugefügt werden soll.</param>
        public void Add(T item)
        {
            m_List.Add(new WeakReference(item));
        }

        /// <summary>
        /// Entfernt alle Elemente aus der <see cref="WeakRefList{T}"/>.
        /// </summary>
        public void Clear()
        {
            m_List.Clear();
        }

        /// <summary>
        /// Ermittelt, ob die <see cref="WeakRefList{T}"/> einen bestimmten Wert enthält.
        /// </summary>
        /// <param name="item">Das im <see cref="WeakRefList{T}"/> zu suchende Objekt.</param>
        /// <returns>Ob der angegebene Wert in die Liste vorhanden ist.</returns>
        public bool Contains(T item)
        {
            CleanAbandonedItems();

            foreach (WeakReference wr in m_List)
            {
                if (wr.Target == (object)item)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Kopiert alle Elemente von <see cref="WeakRefList{T}"/> in ein <see cref="Array"/>, beginnend bei einem bestimmten <see cref="Array"/>-Index.
        /// </summary>
        /// <param name="array">Das eindimensionale Array, das das Ziel der aus ICollection<T> kopierten Elemente ist. Für das Array muss eine nullbasierte Indizierung verwendet werden.</param>
        /// <param name="arrayIndex">Der nullbasierte Index in das array, an dem das Kopieren beginnt.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            int i = 0;
            foreach (WeakReference wr in m_List)
            {
                if (wr.Target != null)
                {
                    array[arrayIndex + i] = (T)wr.Target;
                    i++;
                }
            }
        }

        /// <summary>
        /// Gibt einen Enumerator zurück, der das <see cref="WeakRefList{T}"/> durchläuft.
        /// </summary>
        /// <returns>Einen Enumerator, der das <see cref="WeakRefList{T}"/> durchläuft.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            foreach (WeakReference wr in m_List)
            {
                object innerValue = wr.Target;

                if (innerValue != null)
                    yield return (T)innerValue;
            }
        }

        /// <summary>
        /// Bestimmt den Index eines bestimmten Elements in der <see cref="WeakRefList{T}"/>.
        /// </summary>
        /// <param name="item">Das im <see cref="WeakRefList{T}"/> zu suchende Objekt.</param>
        /// <returns>Den Index eines bestimmten Elements oder -1 falls nicht vorhanden.</returns>
        public int IndexOf(T item)
        {
            CleanAbandonedItems();

            for (int i = 0; i < m_List.Count; i++)
            {
                WeakReference wr = m_List[i];
                if (wr.Target == (object)item)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Fügt am angegebenen Index ein Elements in die <see cref="WeakRefList{T}"/> ein.
        /// </summary>
        /// <param name="index">Der nullbasierte Index, an dem item eingefügt werden soll.</param>
        /// <param name="item">Das in die <see cref="WeakRefList{T}"/> einzufügende Objekt.</param>
        public void Insert(int index, T item)
        {
            try
            {
                m_List.Insert(index, new WeakReference(item));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Entfernt das erste Vorkommen eines angegebenen Objekts aus der <see cref="WeakRefList{T}"/>.
        /// </summary>
        /// <param name="item">Das aus dem <see cref="WeakRefList{T}"/> zu entfernende Objekt.</param>
        /// <returns>True wenn das angegebene Objekt aus dem <see cref="WeakRefList{T}"/> gelöscht wurde sonst false.</returns>
        public bool Remove(T item)
        {
            try
            {
                bool deleted = false;

                m_List.RemoveAll((v) =>
                {
                    // remove all garbage collected entries.
                    if (v.Target == null) return true;

                    // only remove a single entry of 'item'.
                    if (deleted) return false;

                    if (v.Target == (object)item)
                    {
                        deleted = true;
                        return true;
                    }

                    return false;
                });

                return deleted;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Entfernt das <see cref="WeakRefList{T}"/>-Element am angegebenen Index.
        /// </summary>
        /// <param name="index">Der nullbasierte Index des zu entfernenden Elements.</param>
        public void RemoveAt(int index)
        {
            try
            {
                m_List.RemoveAt(index);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Ruft den dem angegebenen Index zugeordneten Wert ab.
        /// </summary>
        /// <param name="index">Der Index des abzurufenden Werts.</param>
        /// <param name="value">Enthält nach dem Beenden dieser Methode den Wert, der dem angegebenen Index zugeordnet ist, wenn der Index gefunden wurde, oder andernfalls den Standardwert für den Typ des value-Parameters. Dieser Parameter wird nicht initialisiert übergeben.</param>
        /// <returns>True, wenn das Element gefunden wurde, andernfalls false.</returns>
        public bool TryGetValue(int index, out T value)
        {
            value = default(T);

            // the index must at least exist.
            if (index > m_List.Count - 1 || index < 0)
                return false;

            // get the value.
            object result = m_List[index].Target;

            if (result == null)
            {
                m_List.RemoveAt(index);
                return false;
            }

            value = (T)result;
            return true;
        }

        /// <summary>
        /// Verweise die bereits vom Garbage Collector gelöscht wurden aus die Liste löschen.
        /// <para>Nur dann wichtig im Fall, dass eine sofortige löschung aller Einträge mit einem bereits vom Garbage Collector freigegebener Wert gelöscht werden sollen.</para>
        /// <para>Diese Funktion wird bereits intern aufgerufen! Bitte beachten Sie die Klassen Dokumentation von <see cref="WeakRefList{T}"/> für weitere Anweisungen und den spezifischen Anwendungsfall.</para>
        /// </summary>
        public void CleanAbandonedItems()
        {
            m_List.RemoveAll((v) => { return v.Target == null; });
        }
    }
}
