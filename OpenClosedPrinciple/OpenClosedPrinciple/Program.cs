﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace OpenClosedPrinciple
{
    internal class Program
    {
        public enum Color
        {
            Red, Green, Blue
        }

        public enum Size
        {
            Small, Medium, Large, Huge
        }

        public class Product
        {
            public string Name;
            public Color Color;
            public Size Size;

            public Product (string name, Color color, Size size)
            {
                if (name == null) {
                    throw new ArgumentNullException (paramName: nameof (name));
                }

                Name = name;
                Color = color;
                Size = size;
                
            }
        }

        public class ProductFilter
        {
            public IEnumerable<Product> FilterBySize (IEnumerable<Product> products, Size size)
            {
                foreach (var p in products) {
                    if (p.Size == size)
                        yield return p;
                }
            }
            
            public IEnumerable<Product> FilterByColor (IEnumerable<Product> products, Color color)
            {
                foreach (var p in products) {
                    if (p.Color == color)
                        yield return p;
                }
            }
            
            public IEnumerable<Product> FilterBySizeAndColor (IEnumerable<Product> products, Size size, Color color)
            {
                foreach (var p in products) {
                    if (p.Color == color && p.Size == size)
                        yield return p;
                }
            }
        }
        
        // Specification pattern
        public interface ISpecification<in T>
        {
            bool IsSatisfied (T t);
        }
        
        public interface IFilter<T>
        {
            IEnumerable<T> Filter (IEnumerable<T> items, ISpecification<T> spec);
            
        }
        // Let's suppose we want to filter the products by color, then we create a color specification
        public class ColorSpecification : ISpecification<Product>
        {
            Color color;

            public ColorSpecification (Color color)
            {
                this.color = color;
            }
            
            public bool IsSatisfied (Product t)
            {
                return t.Color == color;
            }
        }

        public class SizeSpecification : ISpecification<Product>
        {
            Size size;

            public SizeSpecification (Size size)
            {
                this.size = size;
            }
            
            public bool IsSatisfied (Product t)
            {
                return t.Size == size;
            }
        }

        // Combinator
        public class AndSpecification<T> : ISpecification<T>
        {
            ISpecification<T> first, second;

            public AndSpecification (ISpecification<T> first, ISpecification<T> second)
            {
                this.first = first ?? throw new ArgumentNullException(paramName: nameof(first));
                this.second = second ?? throw new ArgumentNullException(nameof(second));
            }
            
            public bool IsSatisfied (T t)
            {
                return first.IsSatisfied (t) && second.IsSatisfied (t);
            }
        }

        public class BetterFilter : IFilter<Product>
        {
            public IEnumerable<Product> Filter (IEnumerable<Product> items, ISpecification<Product> spec)
            {
                foreach (var i in items) {
                    if (spec.IsSatisfied (i))
                        yield return i;
                }
            }
        }

        public static void Main (string[] args)
        {
            var apple = new Product ("Apple", Color.Green, Size.Small);
            var tree = new Product ("Tree", Color.Green, Size.Large);
            var house = new Product ("House", Color.Blue, Size.Large);

            Product[] products = { apple, tree, house };

            var pf = new ProductFilter ();
            Console.WriteLine ("Green products (old):");
            foreach (var p in pf.FilterByColor (products, Color.Blue)) {
                Console.WriteLine($"{p.Name} is {p.Color}");
            }
            
            var bf = new BetterFilter ();
            Console.WriteLine ("Green products (new):");
            foreach (var p in bf.Filter (products, new ColorSpecification (Color.Blue))) {
                Console.WriteLine($"{p.Name} is {p.Color}");
            }

            foreach (var p in bf.Filter (products, new AndSpecification<Product> (new ColorSpecification (Color.Blue), new SizeSpecification (Size.Large)))) {
                Console.WriteLine($"{p.Name} is {p.Color}");
            }
        }
    }
}