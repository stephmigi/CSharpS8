using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business.Tests
{
    [TestFixture]
    class EqualityTests
    {
        private class SimpleReferenceType
        {
            //nothing here
        }

        private class ReferenceTypeWithEqualityValue
        {
            int _age;
            string _name;

            public ReferenceTypeWithEqualityValue(int age, string name)
            {
                _age = age;
                _name = name;
            }

            public override bool Equals(object obj)
            {
                ReferenceTypeWithEqualityValue other = obj as ReferenceTypeWithEqualityValue;
                if (other == null) return false;
                return _age == other._age && _name == other._name;
            }

            public override int GetHashCode()
            {
                // Invalid in our case because this does not
                // the way our Equal works.
                // Object.GetHashCode() uses the memory adress
                // of the object to compute a hash
                // It is valid in the object.Equals implementation
                // But we have overriden .Equals so we MUST override GetHashCode
                // return base.GetHashCode();

                // valid but stupid ...
                // return 1;

                // Not Ideal...
                // return _age.GetHashCode()

                // In practice, use xor operator
                return _age ^ _name.GetHashCode();
            }
        }

        // ReferenceTypeWithEqualityValueWithEquals
        public class Person
        {
            int _age;
            string _name;

            public Person(int age, string name)
            {
                _age = age;
                _name = name;
            }

            public override bool Equals(object obj)
            {
                Person other = obj as Person;
                if (ReferenceEquals(other, null)) return false;
                return _age == other._age && _name == other._name;
            }

            public static bool operator == (Person obj1, Person obj2)
            {
                return ReferenceEquals(obj1, null) ? ReferenceEquals(obj2, null) : obj1.Equals(obj2);
            }

            public static bool operator != (Person obj1, Person obj2)
            {
                return !(obj1 == obj2);
            }
        }

        public class IdentityCard
        {
            public readonly Person Person;

            public IdentityCard(Person p)
            {
                Person = p;
            }
        }

        [Test]
        public void SimpleClass()
        {
            var c1 = new SimpleReferenceType();
            var c2 = new SimpleReferenceType();
            var c3 = c1;

            // references equality
            Assert.That(c3 == c1);
            Assert.That(c1 != c2);
            Assert.That(c3 != c2);

            // what is actually called : 
            // ReferenceEquals, static method of object
            Assert.That(ReferenceEquals(c3, c1));
            Assert.That(Object.ReferenceEquals(c1, c2), Is.False);
            Assert.That(Object.ReferenceEquals(c3, c2), Is.False);
        }

        [Test]
        public void SimpleString()
        {
            // compiler merges the two strings
            // the same object is used in memory, s1, s2 and s3
            // all point to the same string
            var s1 = "Albert";
            var s2 = "Albert";
            var s3 = s1;

            // in java, this would be called :
            // it would work because references are the same
            Assert.That(ReferenceEquals(s3, s1));
            Assert.That(Object.ReferenceEquals(s1, s2), Is.True);
            Assert.That(Object.ReferenceEquals(s3, s2), Is.True);

            // to have different strings with the same contents :

            // nope, compiler will resolve the concat
            //string x1 = "Alb + ert";

            // TODO : why bugged?
            //Assert.That(ReferenceEquals(x1, s1));
            //Assert.That(x1.Equals(s1), Is.True, "Value equality !");
            //Assert.That(x1 == s1, Is.True, "== is defined as .Equals in string class");

            // this will work, new string at runtime
            string x2 = "Alb" + 'e' + "rt";
            Assert.That(ReferenceEquals(x2, s1), Is.False, "Two different objects");
        }

        [Test]
        public void Test_EqualityValueOrValueSemantics()
        {
            var person = new ReferenceTypeWithEqualityValue(12, "toto");
            var person2 = new ReferenceTypeWithEqualityValue(12, "toto");

            // not the same references... 
            Assert.That(ReferenceEquals(person, person2), Is.False);

            // ... but objects are value equal, as we have overrided .Equals
            Assert.That(person.Equals(person2));

            // we have not overloaded == in our class
            // -> Reference.Equals is called
            // -> False
            Assert.That(person == person2, Is.False);

            var otherperson1 = new Person(24, "titi");
            var otherperson2 = new Person(24, "titi");

            // not the same references... 
            Assert.That(ReferenceEquals(otherperson1, otherperson2), Is.False);

            // ... but objects are value equal, as we have overrided .Equals
            Assert.That(otherperson1.Equals(otherperson2));

            // we have overloaded == in our class
            // -> Reference.Equals is called
            // -> False
            Assert.That(otherperson1 == otherperson2, Is.True);
        }
        [Test]
        public void FunWithDictionnary()
        {
            {
                Dictionary<string, IdentityCard> db = new Dictionary<string, IdentityCard>();

                var john = "john";
                var paul = "paul";

                var johnCard = new IdentityCard(null);
                var paulCard = new IdentityCard(null);

                db.Add(john, johnCard);
                db.Add(paul, paulCard);

                Assert.That(db[john] == johnCard);
                Assert.That(db[paul] == paulCard);

                var searched = "john";

                Assert.That(searched == john);
                Assert.That(db[searched] == johnCard);
            }
            {
                Dictionary<Person, IdentityCard> db = new Dictionary<Person, IdentityCard>();

                var john = new Person(12, "john");
                var paul = new Person(25, "paul");

                var johnCard = new IdentityCard(john);
                var paulCard = new IdentityCard(paul);

                db.Add(john, johnCard);
                db.Add(paul, paulCard);

                Assert.That(db[john] == johnCard);
                Assert.That(db[paul] == paulCard);

                var searched = new Person(12, "john");

                Assert.That(searched == john);
                Assert.That(db[searched] == johnCard);
            }
        }
    }
}
