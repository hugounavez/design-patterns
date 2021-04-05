using System;
using System.Collections.Generic;
using System.Linq;

namespace DependencyInversionPrinciple
{
    
    // It is actually really well explained in the following video:
    // https://www.youtube.com/watch?v=VBdGd1VR8i4
    
    internal class Program
    {
        public enum RelationShip
        {
            Parent,
            Child,
            Sibling
        }

        public class Person
        {
            public string Name;
        }

        
        public interface IRelationshipBrowser
        {
            IEnumerable<Person> FindAllChildrenOf (string name);
        }
        
        // Low-level
        public class Relationships : IRelationshipBrowser
        {
            List<(Person, RelationShip, Person)> relations = new List<(Person, RelationShip, Person)> ();

            public void AddParentAndChild (Person parent, Person child)
            {
                relations.Add ((parent, RelationShip.Parent, child));
                relations.Add ((child, RelationShip.Child, parent));
            }

            public List<(Person, RelationShip, Person)> Relations => relations;
            
            public IEnumerable<Person> FindAllChildrenOf (string name)
            {
                foreach (var r in relations.Where (
                    x => x.Item1.Name == name &&
                         x.Item2 == RelationShip.Parent
                )) {
                    yield return r.Item3;
                }
            }
        }

        public class Research
        {
            public Research (IRelationshipBrowser browser)
            {
                foreach (var p in  browser.FindAllChildrenOf ("John")) {
                    
                }
            }
            public Research (Relationships relationships)
            {
                var relations = relationships.Relations;
                foreach (var r in relations.Where (
                    x => x.Item1.Name == "John" &&
                         x.Item2 == RelationShip.Parent
                ))
                {
                    Console.WriteLine($"John has a child called {r.Item3.Name}");
                }
            
            }
        }
        
        
        public static void Main (string[] args)
        {
            var parent = new Person () { Name = "John"};
            var child1 = new Person () { Name = "Chris" };
            var child2 = new Person () { Name = "Mary" };

            var relationships = new Relationships ();
            relationships.AddParentAndChild (parent, child1);
            relationships.AddParentAndChild (parent, child2);

            new Research (relationships);
        }
    }
}