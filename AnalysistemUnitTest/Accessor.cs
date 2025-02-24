﻿using System;
using System.Reflection;
using Analysistem;

namespace AnalysistemUnitTest
{
    // anything defined in the Types enum *MUST* be added to the switch statement in GetType() below
    enum Type // Types (typically, structs & classes) which contain children to be tested
    {
        Synchronizer,
        SynchronizeRecording,
        SynchronizeCsv,
    }

    enum Method // methods to be tested
    {
        ToUnits,
        CombineCsv,
    }

    delegate T _Method<out T>(params object[] arguments);
    delegate object _Method(params object[] arguments);
    struct Method<T>
    {
        public _Method<T> Test;

        public Method(_Method method)
        {
            Test = (parameters) => (T)method(parameters);
        }

        public static implicit operator Method<T>(_Method method) => new Method<T>(method);
    }

    // this class will allows us to access private types and methods without having to change their
    //  implementation, while abstracting away most of the complexity involved in this operation.
    static class Accessor
    {
        // the existence of BindingFlags.Public is not an invitation to use Accessor to access public methods
        //  exposed by public classes. It exists solely for instances of accessing a public method within a
        //  private class
        private const BindingFlags DefaultFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

        public static System.Type GetType(Type typeName, BindingFlags bindingFlags = DefaultFlags)
        {
            switch (typeName)
            {
                case Type.Synchronizer: // top-level class
                    return typeof(Synchronizer);
                // how you would handle a class that is a child class, is public (parent is also public), and has private children:
                // case "NameOfTheClass": return new typeof(ParentName.NameOfTheClass);
                case Type.SynchronizeRecording: // Synchronizer child classes 
                case Type.SynchronizeCsv:
                    return typeof(Synchronizer).GetNestedType(typeName.ToString(), bindingFlags);
                default: return null; // class not found
            }
        }

        public static _Method GetMethod(Type parentClassName, Method methodName, BindingFlags bindingFlags = DefaultFlags)
        {
            System.Type parent = GetType(parentClassName);
            MethodInfo method = parent.GetMethod(methodName.ToString(), bindingFlags);

            return (parameters) => method.Invoke(null, parameters);
        }
    }
}
