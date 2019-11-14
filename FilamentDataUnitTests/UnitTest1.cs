using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Filament_Data;
namespace FilamentDataUnitTests
{
    [TestClass]
    public class FilamentTests
    {
        //DefinedFilaments filamentsDictionary = new DefinedFilaments(true);
        //DefinedSpools spoolsDictionary = new DefinedSpools(true);
        [TestMethod]
        [DataRow(5f,10f,.785f)][DataRow(10f,20f,6.283f)][DataRow(3f,200f,5.655f)][DataRow(0.875f,100f,0.241f)][DataRow(0.875f,1000f,2.405f)]
        public void TestFilamentVolume(float radius,float length,float expected)
        {
            var result =(float) Math.Round(Filament_Data.LibraryMath.FilamentVolumeInCubicCentimeters(radius, length),3);
            Assert.AreEqual<float>(expected, result);
        }
        [TestMethod][DataRow(100f,.298f,1.24f),DataRow(1000f,2.98f,1.24f),DataRow(500f,1.491f,1.24f)]
        [DataRow(100f,.250f,1.04f),DataRow(1000f,2.50f,1.04f),DataRow(500f,1.25f,1.04f)]
        public void TestFilamentDensity(float length,float weight,float expectedDensity)
        {
            var densityCalc = new MeasuredDensity(weight, FilamentDefn.StandardFilamentDiameter, length);
            var result = (float)Math.Round(densityCalc.DensityInGmPerCC,2);
            Assert.AreEqual<float>(expectedDensity, result);
        }
        //[TestMethod][DataRow("3D Solutech",200),DataRow("Hatchbox",200)]
        //public void TestSpoolDictionary(string lookup,int diameter)
        //{
        //    //var dictionary = new Filament_Data.PredefinedSpools(true);
        //    var result = spoolsDictionary[lookup];
        //    Assert.AreEqual<int>(diameter, (int)result.SpoolDiameter);
        //}
        //[TestMethod][DataRow("Generic PLA",1.24),DataRow("Generic ABS",1.04)]
        //public void TestFilamentDictionary(string lookup,double density)
        //{
        //    //var dictionary = new Filament_Data.PredefinedFilaments(true);
        //    var result = filamentsDictionary[lookup];
        //    Assert.AreEqual<decimal>((decimal)density,result.Density);
        //}
    }
}
