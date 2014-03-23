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
        [Test]
        public void Basics()
        {
            ITIList<int> myList = new ITIList<int>();
            ITIList<string> myList2 = new ITIList<string>();
            var myList3 = new ITIList<ITIList<string>>();
        }

        [Test]
        public void AddingItems()
        {
            // Arrange
            ITIList<int> myList = new ITIList<int>();
            Assert.That( myList.Count, Is.EqualTo( 0 ) );
            // Act
            myList.Add( 25 );
            // Assert
            Assert.That( myList.Count, Is.EqualTo( 1 ) );
            Assert.That( myList[0], Is.InstanceOf( typeof( int ) ) );
            Assert.That( myList[0], Is.EqualTo( 25 ) );
            myList.RemoveAt( 0 );
            Assert.That( myList.Count, Is.EqualTo( 0 ) );
        }

        [Test]
        public void TraverseItems()
        {
            ITIList<int> list = new ITIList<int>();

            // movenext has not been called yet, current is negative => exception
            Assert.That(() => list.GetEnumerator().Current, Throws.Exception.TypeOf<InvalidOperationException>());

            //test empty list
            Assert.That(list.GetEnumerator().MoveNext(), Is.False);

            int sum = 0;
            list.Add(1);
            list.Add(2);

           foreach (var element in list)
               sum += element;

           Assert.That(sum, Is.EqualTo(3));

            // movenext will return false because end of list has been reached
           Assert.That(() => list.GetEnumerator().Current, Throws.Exception.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void TestInsertAt()
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

        [Test]
        public void TestIndexOf()
        {
            ITIList<string> list = new ITIList<string>();

            var index = list.IndexOf("test");

            Assert.That(index, Is.EqualTo(-1));

            list.Add("hello");
            list.Add("world");

            index = list.IndexOf("hello");
            Assert.That(index, Is.EqualTo(0));

            index = list.IndexOf("world");
            Assert.That(index, Is.EqualTo(1));

            index = list.IndexOf("test again");
            Assert.That(index, Is.EqualTo(-1));
        }
    }
}
