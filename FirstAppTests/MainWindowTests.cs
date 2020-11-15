using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

namespace FirstApp.Tests
{
    [TestClass()]
    public class MainWindowTests
    {
        // Note: Testar är inte dynamiska dem är statiska för att se hur koden funkar
        [TestMethod()]
        public void CheckingSavedCart()
        {
            //Kollar om det finns någon produkt i varukorgen
            //act
            var result = MainWindow.SaveToCsvCart("Mineral vatten, Gott med äkta mineral vatten, 6, Bonaqua.png");

            //Om den är tom så får vi false
            var result2 = MainWindow.SaveToCsvCart(string.Empty);

            Assert.AreEqual(true, result);
            Assert.AreEqual(false, result2);
        }

        [TestMethod()]
        public void CreateProduct()
        {
            // Testar kontruktor
            var product = new Product("Mineral vatten", "Gott med äkta mineral vatten", 6, "Bonaqua.png");

            Assert.AreEqual("Mineral vatten", product.Title);
            Assert.AreEqual("Gott med äkta mineral vatten", product.Description);
            Assert.AreEqual(6, product.Price);
            Assert.AreEqual("Bonaqua.png", product.Image);

        }

        [TestMethod()]
        public void ReadProductListCount()
        {
            var productTestResult = MainWindow.ReadProductListUtanGUI();
            Assert.AreEqual(10, productTestResult.Count);
        }

        //Kolla hur många rader rabattkoder som finns i CSV.
        [TestMethod()]
        public void ReadDiscountCount()
        {
            var discountTestResult = MainWindow.ReadDiscountTest();

            Assert.AreEqual(4, discountTestResult.Count);
        }
    }
}