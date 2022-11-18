using System;
using System.Reflection;

namespace AnalysistemUnitTest
{
    enum Classes // classes which contain children to be tested
    {
        Synchronizer,
        SynchronizeRecording,
        SynchronizeCsv,
    }

    enum Methods // methods to be tested
    {
        ToUnits,
        CombineCsv,
    }

    delegate T _Method<out T>(params object[] arguments);
    delegate object _Method(params object[] arguments);
    struct Method<T>
    {
        public _Method<T> Call;

        public Method(_Method method)
        {
            Call = (parameters) => (T)method(parameters);
        }

        public static implicit operator Method<T>(_Method method) => new Method<T>(method);
    }

    // this class will allows us to access private classes and methods without having to change their
    //  implementation, while abstracting away most of the complexity involved in this operation.
    static class Accessor
    {
        // the existence of BindingFlags.Public is not an invitation to use Accessor to access public methods
        //  exposed by public classes. It exists solely for instances of accessing a public method within a
        //  private class
        private const BindingFlags DefaultFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

        // any class defined in the Classes enum *MUST* be added to the switch statement below
        public static Type GetClass(Classes className, BindingFlags bindingFlags = DefaultFlags)
        {
            switch (className)
            {
                case Classes.Synchronizer: // top-level class
                    return typeof(Synchronizer);
                // how you would handle a class that is a child class, is public (parent is also public), and has private children:
                // case "NameOfTheClass": return new typeof(ParentName.NameOfTheClass);
                case Classes.SynchronizeRecording: // Synchronizer child classes 
                case Classes.SynchronizeCsv:
                    return typeof(Synchronizer).GetNestedType(className.ToString(), bindingFlags);
                default: return null; // class not found
            }
        }

        public static _Method GetMethod(Classes parentClassName, Methods methodName, BindingFlags bindingFlags = DefaultFlags)
        {
            Type parent = GetClass(parentClassName);
            MethodInfo method = parent.GetMethod(methodName.ToString(), bindingFlags);

            return (parameters) => method.Invoke(null, parameters);
        }
    }
}
