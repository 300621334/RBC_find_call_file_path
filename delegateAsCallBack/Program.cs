using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace delegateAsCallBack
{
    class Program
    {
        static void Main(string[] args)
        {
            containsDelegate obj = new containsDelegate();

            obj.aMethod(methodToBeCalledback);//a method passed as arg in place of a delegate param
        }

        private static void methodToBeCalledback(int i)
        {
            Console.WriteLine(i);
        }
    }//class Program ends




    class containsDelegate
    {
        public delegate void Callback(int i);//will delegate(substitute/proxy for) any method that takes int as arg & has void as return type

        public void aMethod(Callback passedMethod)//a method that accepts param of type delegate, so it will accept any method whose signature(returnType & param) is same as that of delegate Callback
        {
            for(int i=0;i<10000;i++)
            {
                passedMethod(i);//the method that was passed on instead of a delegate Callback, will be invoked now & will be sent some info "i".
            }
        }
    }
}
