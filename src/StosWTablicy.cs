using System;
using System.Collections;
using System.Collections.Generic;

namespace Stos {
    public class StosWTablicy<T> : IStos<T>, IEnumerable<T> {
        private T[] tab;
        private int szczyt = -1;

        public StosWTablicy(int size = 10) {
            tab = new T[size];
            szczyt = -1;
        }

        public int GetTabLength() => tab.Length;
        public T this[int index] { get {
                if(index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException();
                return tab[index];
            } }

        public T Peek => IsEmpty ? throw new StosEmptyException() : tab[szczyt];

        public int Count => szczyt + 1;

        public bool IsEmpty => szczyt == -1;

        public void Clear() => szczyt = -1;

        public T Pop() {
            if(IsEmpty)
                throw new StosEmptyException();

            szczyt--;
            return tab[szczyt + 1];
        }

        public void TrimExcess() {
            if(IsEmpty)
                throw new StosEmptyException();
            int newSize = (int)Math.Ceiling(10.0 / 9.0 * Count);
            Array.Resize(ref tab, newSize);
        }

        public void Push(T value) {
            if(szczyt == tab.Length - 1) {
                Array.Resize(ref tab, tab.Length * 2);
            }

            szczyt++;
            tab[szczyt] = value;
        }

        public T[] ToArray() {
            //return tab;  //bardzo źle - reguły hermetyzacji

            //poprawnie:
            T[] temp = new T[szczyt + 1];
            for(int i = 0; i < temp.Length; i++)
                temp[i] = tab[i];
            return temp;
        }

        public IEnumerator<T> GetEnumerator() {
            var enumerator = new Enumerator(this);
            while(enumerator.MoveNext()) {
                yield return enumerator.Current;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerable<T> GetEnumerableTopToBottom() {
            var enumerator = new Enumerator(this, true);
            while(enumerator.MoveNext()) {
                yield return enumerator.Current;
            }
        }
        private class Enumerator : IEnumerator<T> {
            private readonly StosWTablicy<T> stos;
            private int index = -1;
            private bool toBottom = false;
            public Enumerator(StosWTablicy<T> s, bool toBottom = false) {
                this.stos = s;
                this.toBottom = toBottom;
            }
            public T Current => stos[index];

            object IEnumerator.Current => Current;

            public void Dispose() { }

            public bool MoveNext() {
                if(toBottom && index > 0) {
                    index--;
                    return true;
                } else if(index + 1 < stos.Count) {
                    index++;
                    return true;
                }
                return false;
            }

            public void Reset() => index = -1;
        }

        public System.Collections.ObjectModel.ReadOnlyCollection<T> ToArrayReadOnly() {
            return Array.AsReadOnly(tab);
        }
    }
}
