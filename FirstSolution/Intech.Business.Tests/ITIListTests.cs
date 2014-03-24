using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Intech.Business.Tests
{
    [TestFixture]
    public class ITIListTests
    {
        #region General

        [Test]
        public void ITIList_CountEmptyList()
        {
            ITIList<int> myList = new ITIList<int>();

            Assert.That(myList.Count, Is.EqualTo(0));
        }

        #endregion

        #region Adding
        [Test]
        public void ITIList_AddAnItem()
        {
            ITIList<int> myList = new ITIList<int>();

            myList.Add(25);

            Assert.That(myList.Count, Is.EqualTo(1));
            Assert.That(myList[0], Is.InstanceOf(typeof(int)));
            Assert.That(myList[0], Is.EqualTo(25));
        }

        [Test]
        public void ITIList_InsertAt()
        {
            ITIList<string> list = new ITIList<string>();

            //insert at in empty list
            list.InsertAt(0, "LEONIDAS SAYS : ");

            Assert.That(list[0], Is.EqualTo("LEONIDAS SAYS : "));
            Assert.That(list.Count, Is.EqualTo(1));

            // add some more elements to the list
            list.Add("MADNESS");
            list.Add("THIS");

            //insert at middle of list
            list.InsertAt(1, "?");

            Assert.That(list[1], Is.EqualTo("?"));
            Assert.That(list.Count, Is.EqualTo(4));

            // insert at end of list
            list.InsertAt(list.Count, "IS");

            Assert.That(list[4], Is.EqualTo("IS"));
            Assert.That(list.Count, Is.EqualTo(5));

            // insert more elements to test array growth
            list.InsertAt(list.Count, "SPARTAA");
            list.InsertAt(list.Count, "AAAAAAAAA");
            list.InsertAt(list.Count, "AAAAAAAAAAA");
            list.InsertAt(list.Count, "AAAAAAAAAAAA");
            list.InsertAt(list.Count, "!!!!!!!!!!!!!!");

            Assert.That(list.Count, Is.EqualTo(10));
            Assert.That(list[9], Is.EqualTo("!!!!!!!!!!!!!!"));

            // index can't be negative
            Assert.That(() => list.InsertAt(-1, "test"), Throws.Exception.TypeOf<IndexOutOfRangeException>());

            // index can't be higher that number of list elements
            Assert.That(() => list.InsertAt(55, "test"), Throws.Exception.TypeOf<IndexOutOfRangeException>());
        }

        #endregion

        #region Removing

        [Test]
        public void ITIList_RemoveAt_RemoveAnItem()
        {
            ITIList<int> myList = new ITIList<int>();

            myList.Add(25);
            myList.RemoveAt(0);

            Assert.That(myList.Count, Is.EqualTo(0));
        }

        #endregion

        #region Traverse Items

        [Test]
        public void ITIList_TraverseItems_ExceptionOnGetCurrentOnEmptyList()
        {
            ITIList<int> list = new ITIList<int>();

            // movenext has not been called yet, current is negative => exception
            Assert.That(() => list.GetEnumerator().Current, Throws.Exception.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void ITIList_TraverseItems_ExceptionOnNoNextElementAtEndOfList()
        {
            ITIList<int> list = new ITIList<int>();

            foreach (var element in list)
                // do nothing

            // end of list, movenext will throw an exception because there is not next element
            Assert.That(() => list.GetEnumerator().Current, Throws.Exception.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void ITIList_TraverseItems_EmptyList()
        {
            ITIList<int> list = new ITIList<int>();

            // movenext returns false because there is no next element
            Assert.That(list.GetEnumerator().MoveNext(), Is.False);
        }

        [Test]
        public void ITIList_TraverseItems()
        {
            ITIList<int> list = new ITIList<int>();

            int sum = 0;
            list.Add(1);
            list.Add(2);

           foreach (var element in list)
               sum += element;

           Assert.That(sum, Is.EqualTo(3));
        }

        #endregion

        #region IndexOf

        [Test]
        public void ITIList_IndexOfOnEmptyList()
        {
            ITIList<string> list = new ITIList<string>();

            var index = list.IndexOf("test");

            Assert.That(index, Is.EqualTo(-1));
        }

        [Test]
        public void ITIList_IndexOfOnList()
        {
            ITIList<string> list = new ITIList<string>();

            list.Add("hello");
            list.Add("world");

            int index = list.IndexOf("hello");
            Assert.That(index, Is.EqualTo(0));
        }

        [Test]
        public void ITIList_IndexOfInvalidElement()
        {
            ITIList<string> list = new ITIList<string>();

            list.Add("hello");
            list.Add("world");

            var index = list.IndexOf("test");
            Assert.That(index, Is.EqualTo(-1));
        }

        #endregion
    }
}
