using System;

namespace LiskovSubstitutionPrinciple
{
    /*  In this principle I should build classes in a way that I can create
        classes that I can assign a child object to a parent object type, for example:
        
        class Burger;
        
        class CheeseBurger : Burger;
        
        I should be able to do with no change in logic whatsoever:
        Burger burger = new CheeseBurger(); 
     
    */
    
    public class Rectangle
    {
        public virtual int Width { get; set; }
        public virtual int Height { get; set; }

        public Rectangle ()
        {
                
        }

        public Rectangle (int width, int height)
        {
            Width = width;
            Height = height;
        }

        public override string ToString ()
        {
            return $"{nameof (Width)}: {Width}, {nameof (Height)}: {Height}";
        }
    }

    public class Square : Rectangle
    {
        public override int Height {
            set { base.Width = base.Height = value; }
        }

        public override int Width {
            set { base.Width = base.Height = value; }
        }
    }
    
    internal class Program
    {
        static public int Area (Rectangle r) => r.Width * r.Height;
        public static void Main (string[] args)
        {
            Rectangle rc = new Rectangle (2,4);
            Console.WriteLine ($"{rc} has area {Area (rc)}");

            Rectangle sq = new Square ();
            sq.Height = 4;
            Console.WriteLine ($"{sq} has area {Area (sq)}");

        }
    }
}