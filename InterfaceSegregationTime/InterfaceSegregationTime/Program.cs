namespace InterfaceSegregationTime
{
    internal class Program
    {
        /*
         * The idea is to have small interfaces instead of having a single interface with
         * lot of responsibilities.
         */
        
        public class Document 
        {
            
        }
        
        public interface IMachine
        {
            void Print (Document d);
            void Scan (Document d);
            void Fax (Document d);
        }
        
        // Instead create smaller interfaces
        public interface IPrinter
        {
            void Print (Document d);
        }
        
        public interface IScanner
        {
            void Scan (Document d);
        }
        
        
        public class Photocopier: IPrinter, IScanner
        {
            public void Print (Document d)
            {
                throw new System.NotImplementedException ();
            }

            public void Scan (Document d)
            {
                throw new System.NotImplementedException ();
            }
        }
        
        public interface IMultiFunctionDevice : IScanner, IPrinter
        {
            
        }
        
        public class MultiFuntionMachine : IMultiFunctionDevice
        {
            IPrinter printer;
            IScanner scanner;

            public MultiFuntionMachine (IPrinter printer, IScanner scanner)
            {
                this.printer = printer;
                this.scanner = scanner;
            }

            public void Print (Document d)
            {
                printer.Print (d);
            }

            public void Scan (Document d)
            {
                scanner.Scan (d);
                // decorator pattern
            }
        }

        public static void Main (string[] args)
        {
        }
    }
}