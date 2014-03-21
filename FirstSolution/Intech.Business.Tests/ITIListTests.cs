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
    }
}
