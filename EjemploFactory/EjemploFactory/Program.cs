using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjemploFactory
{
    class Program
    {
        static void Main(string[] args)
        {
            Action<string> WriteLine = Console.WriteLine;
            Func<string> ReadLine = Console.ReadLine;
            var selection = new Selection(WriteLine, ReadLine);
            var log = new Log(WriteLine, ReadLine);

            var pay = new MainPayment(selection.Select());

            if (pay.IsLogged(log.Logger())) pay.Pay(10);
            ReadLine();
        }
    }

    public enum TypePayment { CARD, PAYPAL, TRANSFER };

    public struct Login
    {
        public string user;
        public string pass;
    }

    public interface Payment
    {
        bool IsValidLogin(Login login);
        void DoPayment(int shoppingList);

    }

    public class CardPayment : Payment
    {
        public bool IsValidLogin(Login login)
        {
            return true;
        }
        public void DoPayment(int shoppingList)
        {
            Console.WriteLine("Pago realizado con TARJETA.");
        }
    }

    public class PayPalPayment : Payment
    {
        public bool IsValidLogin(Login login)
        {
            return true;
        }
        public void DoPayment(int shoppingList)
        {
            Console.WriteLine("Pago realizado con PayPal.");
        }
    }

    public class WireTransferPayment : Payment
    {
        public bool IsValidLogin(Login login)
        {
            return true;
        }
        public void DoPayment(int shoppingList)
        {
            Console.WriteLine("Pago realizado con TRANSFERENCIA BANCARIA.");
        }
    }

    public class FactoryPayment
    {
        public static Payment GetPayment(TypePayment typePayment)
        {
            switch (typePayment)
            {
                case TypePayment.CARD:
                    return new CardPayment();

                case TypePayment.PAYPAL:
                    return new PayPalPayment();

                case TypePayment.TRANSFER:
                    return new WireTransferPayment();

                default:
                    return new CardPayment();
            }
        }
    }

    public class MainPayment
    {
        private Payment payment;

        public MainPayment(TypePayment typePayment)
        {
            payment = FactoryPayment.GetPayment(typePayment);
        }

        public bool IsLogged(Login login)
        {
            return payment.IsValidLogin(login);
        }

        public void Pay(int ShoppingList)
        {
            payment.DoPayment(ShoppingList);
        }
    }

    public class Selection
    {
        public Action<string> _writeLine;
        public Func<string> _readLine;
        private string opt;

        public Selection(Action<string> writeLine, Func<string> readLine)
        {
            _writeLine = writeLine;
            _readLine = readLine;
        }

        public TypePayment Select()
        {
            _writeLine("Elija método de pago:");
            _writeLine("1. Tarjeta de Credito");
            _writeLine("2. Paypal");
            _writeLine("3. Transferencia");
            do
            {
                opt = _readLine();
            }
            while (!(String.Equals("1", opt) || String.Equals("2", opt) || String.Equals("3", opt)));

            switch (opt.ToString())
            {
                case "1":
                    return TypePayment.CARD;
                case "2":
                    return TypePayment.PAYPAL;
                case "3":
                    return TypePayment.TRANSFER;
                default:
                    return TypePayment.CARD;
            }
        }
    }

    public class Log
    {
        private Action<string> _writeLine;
        private Func<string> _readLine;
        private Login login;

        public Log(Action<string> writeLine, Func<string> readLine)
        {
            _writeLine = writeLine;
            _readLine = readLine;
        }
        
        public Login Logger()
        {
            _writeLine("Usuario: ");
            login.user =  _readLine();
            _writeLine("Contraseña: ");
            login.pass = _readLine();

            return login;
        }
    }
}
