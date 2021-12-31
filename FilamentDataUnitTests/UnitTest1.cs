using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DataDefinitions;

namespace FilamentDataUnitTests
{
    public static class ToleranceExtensions
    {
        public static bool WithinTolerance(this double target, double expected, double tolerance) => Math.Abs(target - expected) < tolerance;
    }
    [TestClass]
    public class ParseingTests
    {
        const double matchTolerance = 1e-4;
        const string unitInch = "in";
        const string unitMM = "mm";
        const string unitCM = "cm";
        const string unitInch2 = "\"";
        const string unitKg = "kg";
        const string unitG = "g";
        const string unitOz = "oz";

        [TestMethod]
        [DataRow("3_5/8", 3.625),
            DataRow("2_1/4", 2.25),
            DataRow("1_5/64", 1.078125),
            DataRow("7/64", 0.109375)]
        public void TestFractionParse(string test, double expected)
        {
            if (CompoundFractionWithUnits.TryParse(test, out CompoundFractionWithUnits value))
            {
                Assert.AreEqual(expected, value.Value);
            }
            else
                Assert.Fail();
        }
        [TestMethod]
        [DataRow("3_5/8", 92.075, matchTolerance,true),
            DataRow("2_1/4", 57.15, matchTolerance,true),
            DataRow("1_18/64", 32.54375, matchTolerance,true),
            DataRow("1_27/64", 36.115625, matchTolerance,true),
            DataRow("_50/64", 19.84375, matchTolerance,true),
            DataRow("3/16", 4.7625, matchTolerance,true),
            DataRow("1_15/32", 37.30625, matchTolerance,true),
            DataRow("1_5/0",double.NaN,matchTolerance,false)]
        public void TestConvertCompoundLength(string test, double expected, double tolerance,bool outcome)
        {
            if (CompoundFractionWithUnits.TryParse(test, out CompoundFractionWithUnits value))
            {
                //int roundDigits = 4;

                var converted = FilamentMath.ConvertLength(value.Value, FilamentMath.SupportedLengthAlias(value.Units), ConvertToLength.Millimeter);
                //var rounded = Math.Round(converted, roundDigits);
                //var meetsTolerance = Math.Abs(converted - expected) < tolerance;
                Assert.AreEqual(outcome, converted.WithinTolerance(expected, tolerance));
            }
            else
                Assert.Fail();
        }
        [TestMethod, DataRow("1.25in", 1.25, unitInch),
            DataRow("54 mm", 54, unitMM),
            DataRow("3.875 in", 3.875, unitInch),
            DataRow("300g", 300, unitG),
            DataRow("1kg", 1, unitKg)]
        public void TestNumberParse(string test, double expected, string expectedUnits)
        {
            if (ValueWithUnits.TryParse(test, out ValueWithUnits valueWithUnits))
            {
                Assert.AreEqual(expected, valueWithUnits.Value);
                Assert.AreEqual(expectedUnits, valueWithUnits.Units);
            }
        }
        [TestMethod, DataRow("1.25in", 31.75, unitInch),
            DataRow("54.587 mm", 54.587, unitMM),
            DataRow("3.875 in", 98.425, unitInch),
            DataRow("3.87cm", 38.7, unitCM),
            DataRow("1.25\"", 31.75, unitInch2)]
        public void TestLengthConvert(string test, double expected, string expectedUnits)
        {
            if (ValueWithUnits.TryParse(test, out ValueWithUnits valueWithUnits))
            {
                var converted = FilamentMath.ConvertLength(valueWithUnits.Value, FilamentMath.SupportedLengthAlias(valueWithUnits.Units), ConvertToLength.Millimeter);
                Assert.AreEqual(true, converted.WithinTolerance(expected, matchTolerance));
                Assert.AreEqual(expectedUnits, valueWithUnits.Units);
            }
            else
                Assert.Fail();
        }
        // TODD: Weight Conversion/Parsing Tests
        [TestMethod, DataRow("300 g", 0.3, unitG),
            DataRow("22 oz", 0.62368951, unitOz),
            DataRow("22 Oz", 0.62368951, unitOz)]
        public void TestWeightConvert(string test, double expected, string expectedUnits)
        {
            if (ValueWithUnits.TryParse(test, out ValueWithUnits valueWithUnits))
            {
                var converted = FilamentMath.ConvertWeight(valueWithUnits.Value, FilamentMath.SupportedWeightAlias(valueWithUnits.Units.ToLower()), ConvertToWeight.Kilograms);
                Assert.AreEqual(true, converted.WithinTolerance(expected, matchTolerance));
                Assert.AreEqual(expectedUnits.ToLower(), valueWithUnits.Units.ToLower());
            }
        }
    }
}
