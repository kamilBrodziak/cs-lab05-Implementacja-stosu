using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Stos {
    public class StosWLiscie<T> : IStos<T> {
        private int szczyt = -1;
        private TabElement Current = new TabElement();
        private TabElement Last = null;
        private TabElement First = null;
        private int length;

        private class TabElement {
            public T Data { get; set; }
            public TabElement PrevEl { get; set; }
            public TabElement NextEl { get; set; }

            public bool HasNext() {
                return NextEl != null;
            }

            public bool HasPrev() {
                return PrevEl != null;
            }

            public bool HasData() {
                return Data != null;
            }
        }

        public StosWLiscie(int size = 10) {
            length = size;
            szczyt = -1;
            if(size > 0) {
                First = new TabElement();
                First.PrevEl = Current;
                Current.NextEl = First;
                Last = First;
                for(int i = 1; i < size; ++i) {
                    var newEl = new TabElement();
                    Last.NextEl = newEl;
                    newEl.PrevEl = Last;
                    Last = newEl;
                }
            }
        }

        public int GetTabLength() => length;
        public T this[int index] {
            get {
                if(index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException();
                var el = First;
                for(int i = 0; i < index; ++i) {
                    el = el.NextEl;
                }
                return el.Data;
            }
        }

        public T Peek => IsEmpty ? throw new StosEmptyException() : this[szczyt];

        public int Count => szczyt + 1;

        public bool IsEmpty => szczyt == -1;

        public void Clear() => szczyt = -1;

        public T Pop() {
            if(IsEmpty)
                throw new StosEmptyException();

            szczyt--;
            var current = Current;
            Current = Current.PrevEl;
            return current.Data;
        }

        public void TrimExcess() {
            if(IsEmpty)
                throw new StosEmptyException();
            int newSize = (int)Math.Ceiling(10.0 / 9.0 * Count);
            for(int i = length; i < newSize; ++i ) {
                var newEl = new TabElement();
                newEl.PrevEl = Last;
                Last.NextEl = newEl;
                Last = newEl;
            }
            length = newSize;
        }

        public void Push(T value) {
            if(szczyt == length) {
                for(int i = 0; i < length; ++i) {
                    var newEl = new TabElement();
                    newEl.PrevEl = Last;
                    Last.NextEl = newEl;
                    Last = newEl;
                    length++;
                }
            }
            szczyt++;
            Current = Current.NextEl;
            Current.Data = value;
        }

        public T[] ToArray() {
            T[] temp = new T[szczyt + 1];
            for(int i = 0; i < Count; i++) {
                temp[i] = this[i];
            }
            return temp;
        }

    }
}
