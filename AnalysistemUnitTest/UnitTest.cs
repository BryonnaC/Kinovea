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
    public class TimeUnitTest
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
    }

    //[TestClass]
    //public class SynchronizeCsvUnitTest 
    //{
    //    [TestMethod]
    //    // name might need some work
    //    public void CombineCsv_CreamyCsvs_CompoundCsvCreated()
    //    {
    //        Method<CsvFile> CombineCsv = Accessor.GetMethod(Type.SynchronizeCsv, Method.CombineCsv);

    //        CombineCsv.Test("", "", "");
    //    }
    //}

    [TestClass]
    public class FakeUserUnitTest
    {
        /**
         * DetectTarget() relies on CalibrateTemplate being implemented
         * (which it isn't)
         */ 
        [TestMethod]
        public void DetectTarget_TargetOnScreen_TargetDetected()
        {
            /** Calling DetectTarget, when the target is on the screen, should show the target as being detected 
             * Method: DetectTarget()
             * 
             * This test relies on a particular host setup (The test target being on screen),
             * and thus should not be automated
             * 
             * Detect the target => Target foo = DetectTarget(Template.[some_templ]);
             * Verify foo.detected = true (secondary predicate: CalibrateTemplate works properly)
             */
        }

        [TestMethod]
        public void DetectTarget_TargetNotOnScreen_TargetNotDetected()
        {
            /** Calling DetectTarget, when the target is *not* on the screen, should show the target as *not* being detected
             * Method: DetectTarget()
             * 
             * This test relies on a particular host setup (The test target *not* being on screen),
             * and thus should not be automated
             */
        }

        [TestMethod]
        // can PressKey() be used to type out words
        public void PressKey_TypingWord_WordTyped()
        {

        }
    }

    [TestClass]
    public class CsvFileUnitTest
    {
        [TestMethod]
        public void Load_UsingPath_PopulateProperties()
        {
            /** Load, when provided a path to a valid .csv file, should populate the struct properties appropriately
             * Method: Load()
             * 
             * Start with a test string representing a valid .csv file
             *      e.g., bar = "test1,test2,test3\n1,2,3\n11,22,33\n111,222,333"
             * Create an empty CsvFile => CsvFile foo = CsvFile.Empty;
             * Have the CsvFile call Load() on the string => foo.Load(bar);
             * Verify the resultant properties are as expected
             */
        }

        [TestMethod]
        public void Merge_UsingPath_MergeFileIntoProperties()
        {
            /** Merge, when provided a path to a valid .csv file, should load the file and merge its properties with the calling CsvFile
             * Method: Merge()
             * 
             * Start with two test strings representing a valid .csv file
             *      e.g., bar1 = "test1,test2,test3\n1,2,3\n11,22,33\n111,222,333"
             *            bar2 = "test4,test5,test6\n4,5,6\n44,55,66\n444,555,666"
             * Load the first string into a CsvFile =>
             *      CsvFile foo = new CsvFile(bar1);
             * Have the first CsvFile call Merge() on the second string => foo.Merge(bar2);
             * Verify the resultant properties are as expected
             */
        }

        [TestMethod]
        public void Merge_UsingCsvFile_MergeFileIntoProperties()
        {
            /** Merge, when provided a CsvFile, should merge the struct's properties with the calling CsvFile
             * Method: Merge()
             * 
             * Start with two test strings representing a valid .csv file
             *      e.g., bar1 = "test1,test2,test3\n1,2,3\n11,22,33\n111,222,333"
             *            bar2 = "test4,test5,test6\n4,5,6\n44,55,66\n444,555,666"
             * Load the strings into two separate CsvFiles =>
             *      CsvFile foo1 = new CsvFile(bar1);
             *      CsvFile foo2 = new CsvFile(bar2);
             * Have one CsvFile call Merge() on the other => foo1.Merge(foo2);
             * Verify the resultant properties are as expected
             */
        }

        [TestMethod]
        public void Constructor_TwoCsvsProvided_MergeFilesIntoProperties()
        {
            /** Constructor, when provided two .csv paths or files, will merge the files into the resultant properties
             * Method: Constructor
             * 
             * Start with two test strings representing a valid .csv file
             *      e.g., bar1 = "test1,test2,test3\n1,2,3\n11,22,33\n111,222,333"
             *            bar2 = "test4,test5,test6\n4,5,6\n44,55,66\n444,555,666"
             * Pass the two strings into a new CsvFile => CsvFile foo = new CsvFile(bar1, bar2);
             * Verify the resultant properties are as expected
             */
        }

        [TestMethod]
        public void Serialize_Called_ReturnsValidCsv()
        {
            /** Serialize, when called, should return a valid .csv file
             * Method: Serialize()
             * 
             * Start with a test string representing a valid .csv file
             *      e.g., bar = "test1,test2,test3\n1,2,3\n11,22,33\n111,222,333"
             * Load into CsvFile => CsvFile foo = new CsvFile(bar);
             * Serialize it => string fooSerialized = foo.Serialize();
             * Attempt to reload the serialized string into a new CsvFile
             *      => CsvFile fooDeserialized = new CsvFile(fooSerialized);
             * (predicate: foo (as string) == fooDeserialized (as string))
             */
        }
    }

    [TestClass]
    public class FileHandlerUnitTest
    {
        [TestMethod]
        public void GetPaths_FilesExist_CorrectPathsFound()
        {
            // not 100% on how this would get tested
        }
    }
}
