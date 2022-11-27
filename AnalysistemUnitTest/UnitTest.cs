using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Analysistem.Utils;
using Analysistem;

/* https://learn.microsoft.com/en-us/visualstudio/test/unit-test-basics?view=vs-2022#write-your-tests
 * 
 * To open the Test Explorer: Test > Test Explorer
 * 
 * ===================================================================================================
 * ================ READ THE FOLLOWING IN ITS ENTIRETY BEFORE CONTRIBUTING UNIT TESTS ================
 * ===================================================================================================
 * 
 *  Until a reason presents itself to change this decision, any class existing within Analysistem should have
 *      a corresponding unit test class in this file. This is mostly just to encapsulate tests/scope method names.
 *      
 *      
 *  The naming of methods should adhere to the following pattern (when possible):
 *      [Name/Action]_[WhatIsBeingTested]_[ExpectedResult]
 *      
 *      e.g., Imagine we're testing a banking system and wanted to test withdrawing. In this scenario,
 *          we would want to test withdrawing various amounts relative to the current account balance, such as
 *          greater than. 
 *          Thus a potential test could be:
 *              - Name/Action:       Withdraw
 *              - WhatIsBeingTested: AmountMoreThanBalance
 *              - ExpectedResult:    Throw (throws exception)
 *          So, the full method declaration for the above test would be:
 *              - `public void Withdraw_AmountMoreThanBalance_Throw() { }` 
 *          If we want the result to be a specific exception, such as ArgumentOutOfRange, then a better method
 *              declaration might be:
 *              - `public void Withdraw_AmountMoreThanBalance_ThrowArgumentOutOfRange() { }`
 *          
 *          **> DON'T BE AFRAID OF ABSURDLY LONG NAMES; THE MORE DESCRIPTIVE THE BETTER <**
 *              
 *      If the syntax still doesn't seem very intuitive, a good way to think of it is to read it like this:
 *          - Calling [Name/Action], when [WhatIsBeingTested], should [ExpectedResult]
 *      With the above example:
 *          - Calling Withdraw, when amount more than balance, should throw ArgumentOutOfRange.
 *              
 *      In the circumstance where tests are being written *after* the relevant code, use the name of the method
 *          being tested for 'Name/Action.'
 *      
 *      
 *  We will rely on the AAA (Arrange, Act, Assert) pattern as our methodology for writing unit tests.
 *  This should be strictly adhered to as much as possible (although, exceptions may exist).
 *
 *  - The Arrange section of a unit test method initializes objects and sets the value of 
 *      the data that is passed to the method under test.
 *      Try to divide your Arrange section into two parts: 
 *          - Variables that should/can be set manually by a developer (comment: dev-mut)
 *          - Variables that should not be set manually by a delevoper (comment: dev-const)
 *      The idea behind this is that, if we find we want to adjust certain tests, we should only need to adjust
 *          'dev-mut' variables and doing so should NOT change the actual functionality of the test.
 *          Conversely, 'dev-const' variables should *only* be changed if we find the functionality of the test
 *          needs to be tweaked.
 *      If possible, try placing Assert statements between your 'dev-mut' and 'dev-const' sections as a way
 *          of validating whatever any future developer attempts to hardcode for the test and explicitly stating
 *          the expected parameter domains.
 *      If the method to be tested is private, *DO NOT* change its accessibility. Instead, use the Accessor class
 *          in Accessor.cs. *DO NOT* use the Accessor class if a method can be accessed normally. In general,
 *          methods should be declared in 'dev-const.'
 *          To declare a method using Accessor:
 *              - Method<[ReturnType]> [MethodName] = Accessor.GetMethod(Types.[ParentTypeName], Methods.[MethodName]);
 *              - Put `object` for the ReturnType if void/unknown/irrelevant
 *          To call the method using Accessor:
 *              - [MethodName].Call([params...]);
 *              - The params will not be type-checked at all. Not even the number of params. Be mindful.
 *          
 *      A common pattern is to have a variable `expected` in 'dev-mut' and a variable `actual` in 'dev-const.'
 *          - `expected` will be hardcoded by the developer
 *          - `actual` will be populated in the Act section
 *          - These two variables will then be compared in the Assert section to validate the test
 *     
 *  - The Act section invokes the method under test with the arranged parameters.
 *      Where you place the Act and Assert sections (i.e., whatever degree of nesting) does not matter so long
 *          as Act is *always* entered before Assert.
 *  
 *  - The Assert section verifies that the action of the method under test behaves as expected.
 *      For .NET, methods in the Assert class are often used for verification.
 *      If you can reasonably fit both the Act and the Assert sections in a one-liner, then feel free to combine
 *          the two into a single section.
 *      Try to place all print commands between the Act and Assert sections. This should help to prevent 
 *          clutter and will ensure the prints all fire before an assertion potentially throws an error.           
 */

namespace AnalysistemUnitTest
{
    [TestClass]
    // miscellaneous test class for functionality that won't/shouldn't have a corresponding test class
    public class UnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            /* Arrange */
            /* Act */
            /* Assert */
        }
    }

    [TestClass]
    public class SynchronizeRecordingUnitTest
    {
        [TestMethod]
        // quite possibly the single lowest priority method to test, but also the most straightforward
        // so now it exists as an example/reference
        public void ToUnits_ValidUnits_ReturnConvertedTime()
        {
            #region TEST
            /* Arrange */
            // dev-mut      (comment is here for example purposes)
            TimeSpan baseTicks = new TimeSpan(10_000);
            Unit[] unitsToTest = { Unit.Milliseconds, Unit.Microseconds, Unit.Nanoseconds };
            double[] expected = new double[] { 1.0, 1_000.0, 1_000_000.0 };

            // dev-assert   (comment is here for example purposes)
            Assert.AreEqual(unitsToTest.Length, expected.Length);

            // dev-const    (comment is here for example purposes)
            double[] actual = new double[unitsToTest.Length];

            /* Act */
            for (int i = 0; i < unitsToTest.Length; i++)
                actual[i] = baseTicks.ToUnits(unitsToTest[i]);

            // print        (comment is here for example purposes)
            Console.WriteLine(string.Join(", ", actual));
            
            /* Assert */
            CollectionAssert.AreEqual(expected, actual);
            #endregion
        }

        [TestMethod]
        public void ToUnits_InvalidArgs_Throw()
        {
            /* COMPILER WILL NOT ALLOW ARGS THAT WOULD BREAK THIS METHOD */

            #region RIP
            // Got confused because MethodInfo.Invoke() allows you to pass variables that otherwise would
            //  have been flagged by the compiler. The following commented code is entirely unnecessary,
            //  but will remain here for now as an example for patterns.

            ///* Arrange */
            //// dev-mut (comment is here for example purposes)
            //TimeSpan validTicks = new TimeSpan(10_000);
            //Units validUnit = Units.Milliseconds;
            //int invalidTicks = 10_000;
            //double invalidUnit = 0.0;
            //// expected = throws error

            //// dev-const (comment is here for example purposes)
            //MethodInfo ToUnits = typeof(Synchronizer).GetMethod("ToUnits", BindingFlags.NonPublic | BindingFlags.Static);
            //object[][] parameterValues = new object[][] 
            //{
            //    new object[] { invalidTicks, validUnit },
            //    new object[] { validTicks, invalidUnit },
            //    new object[] { null, validUnit },
            //    new object[] { validTicks, null },
            //};
            //double actual;

            //List<int> successfulIterations = new List<int>();
            //for (int i = 0; i < parameterValues.Length; i++)
            //{
            //    try
            //    {
            //        /* Act */
            //        actual = (double)ToUnits.Invoke(null, parameterValues[i]);

            //        successfulIterations.Add(i); // no exception was thrown
            //    }
            //    /* Assert */
            //    catch (ArgumentException e)
            //    {
            //        string systemArgumentExceptionMessage = $"Object of type '{parameterValues[i][i].GetType()}' cannot be converted to type '{(i == 0 ? "System.TimeSpan" : "Units")}'.";
            //        StringAssert.Contains(e.Message, systemArgumentExceptionMessage);
            //    }
            //    catch (TargetInvocationException e)
            //    {
            //        Exception innerE = e.InnerException;
            //        Assert.IsNotNull(innerE);
            //        Assert.IsInstanceOfType(innerE, typeof(NullReferenceException));

            //        // assert the null is occuring at the intended argument position
            //        Assert.IsNull(parameterValues[i][i == 2 ? 0 : 1]);
            //        Assert.IsNotNull(parameterValues[i][i == 2 ? 1 : 0]);

            //        string toUnitsNullExceptionMessage = i == 2 ? Synchronizer.ToUnitsInvalidTimeSpanMessage : Synchronizer.ToUnitsInvalidUnitsMessage;
            //        StringAssert.Contains(innerE.Message, toUnitsNullExceptionMessage);
            //    }
            //}

            //if (successfulIterations.Count > 0)
            //    Assert.Fail($"Too few exceptions thrown: iteration(s) {string.Join(", ", successfulIterations)} failed");
            #endregion
        }
    }

    [TestClass]
    public class SynchronizeCsvUnitTest 
    {
        [TestMethod]
        // name might need some work
        public void CombineCsv_CreamyCsvs_CompoundCsvCreated()
        {
            Method<CsvFile> CombineCsv = Accessor.GetMethod(Type.SynchronizeCsv, Method.CombineCsv);

            CombineCsv.Test("", "", "");
        }
    }

    [TestClass]
    public class FakeUserUnitTest
    {
        [TestMethod]
        public void DetectTarget_TargetOnScreen_TargetDetected()
        {

        }

        [TestMethod]
        public void DetectTarget_TargetNotOnScreen_TargetNotDetected()
        {

        }

        [TestMethod]
        // can PressKey() be used to type out words
        public void PressKey_TypingWord_WordTyped()
        {

        }
    }

    [TestClass]
    public class FileHandlerUnitTest
    {
        [TestMethod]
        public void ExportSparkvue_SparkvueOnScreen_CsvFileCreated()
        {

        }

        [TestMethod]
        // mouse should not move/click, keyboard should not type, no file should be created
        public void ExportSparkvue_SparkvueNotOnScreen_NoActionTaken()
        {
            
        }
    }
}
