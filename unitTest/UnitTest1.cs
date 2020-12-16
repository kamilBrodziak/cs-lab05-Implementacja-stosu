using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stos;
using System;

namespace UnitTestProjectStos
{
    [TestClass]
    public class UnitTestStosChar
    {
        private IStos<char> stos;
        private Random rnd = new Random();
        //zwraca znak ASCII o kodzie z zakresu 33..126
        private char RandomElement => (char)rnd.Next(33, 126);

        // s.create ==> s.IsEmpty==true
        [TestMethod]
        public void IsEmpty_PoUtworzeniuStosJestPusty()
        {
            stos = new StosWTablicy<char>();
            Assert.IsTrue(stos.IsEmpty);
        }

        // s.create.Push(e) ==> s.IsEmpty==false
        [TestMethod]
        public void IsEmpty_PoUtworzeniuIDodaniuElementuStosNieJestPusty()
        {
            stos = new StosWTablicy<char>();
            stos.Push(RandomElement);
            Assert.IsFalse(stos.IsEmpty);
        }

        // s.Pop( s.Push(e) ) == s
        [TestMethod]
        public void PushPop_StosSieNieZmienia()
        {
            stos = new StosWTablicy<char>();
            stos.Push(RandomElement);
            stos.Push(RandomElement);

            char[] tabPrzed = stos.ToArray();
            char e = RandomElement;
            stos.Push(e);
            stos.Pop();
            char[] tabPo = stos.ToArray();

            CollectionAssert.AreEqual(tabPrzed, tabPo);
        }

        // s.Peek( s.Push(e) ) == e
        [TestMethod]
        public void Peek_ZwracaOstatnioWstawionyElement()
        {
            stos = new StosWTablicy<char>();
            char e = RandomElement;

            stos.Push(e);

            Assert.AreEqual(stos.Peek, e);
        }

        // s.create.Peek ==> throw StosEmptyException
        [TestMethod]
        [ExpectedException(typeof(StosEmptyException))]
        public void PeekDlaPustegoStosu_ZwracaWyjatek_StosEmptyException()
        {
            stos = new StosWTablicy<char>();
            Assert.IsTrue(stos.IsEmpty);

            char c = stos.Peek;
        }

        // s.create.Pop() ==> throw StosEmptyException
        [TestMethod]
        [ExpectedException(typeof(StosEmptyException))]
        public void PopDlaPustegoStosu_ZwracaWyjatek_StosEmptyException()
        {
            stos = new StosWTablicy<char>();
            Assert.IsTrue(stos.IsEmpty);

            char c = stos.Peek;
        }

        // TrimExcess() ==> throw StosEmptyException
        [TestMethod]
        [ExpectedException(typeof(StosEmptyException))]
        public void TrimExcessDlaPustegoStosu_StosEmptyException() {
            var s = new StosWTablicy<int>();
            s.TrimExcess();
        }

        // TrimExcess() valid
        [TestMethod]
        [DataRow(5)]
        [DataRow(11)]
        [DataRow(28)]
        [DataRow(3)]
        [DataRow(155)]
        [DataRow(85)]
        public void TrimExcessDlaPustegoStosu_Valid(int size) {
            var s = new StosWTablicy<int>(size);
            for(int i = 0; i < size; ++i) {
                s.Push(i);
            }
            s.TrimExcess();
            Assert.AreEqual(size, (int)Math.Floor(s.GetTabLength() * 0.9));
        }

        // Indexer ==> throw ArgumentOutOfRangeException
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [DataRow(-5)]
        [DataRow(1)]
        public void Indexer_ArgumentOutOfRangeException(int ind) {
            var s = new StosWTablicy<int>();
            int i = s[ind];            
        }

        // Indexer ==> valid
        [TestMethod]
        [DataRow(new int[] { 0, 5, 55, 7, 1 }, 4, 1)]
        [DataRow(new int[] { 2521, 67, 1, 245, 26 }, 1, 67)]
        [DataRow(new int[] { 2, 55, 12, 5, 2 }, 0, 2)]
        [DataRow(new int[] { 2, 55, 12, 5, 2 }, 2, 12)]
        public void Indexer_Valid(int[] array, int ind, int value) {
            var s = new StosWTablicy<int>();
            foreach(int i in array) {
                s.Push(i);
            }
            Assert.AreEqual(value, s[ind]);
        }
    }

}
