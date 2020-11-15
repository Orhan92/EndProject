using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FirstApp.Tests
{
    [TestClass()]
    public class MainWindowTests
    {
        // Note: Testar är inte dynamiska dem är statiska för att se hur koden funkar
        [TestMethod()]
        public void CheckingSavedCart()
        {
            //Kollar om det finns någon produkt i den sparade varukorgen
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
            //Testar Product kontruktor
            var product = new Product("Mineral vatten", "Gott med äkta mineral vatten", 6, "Bonaqua.png");

            Assert.AreEqual("Mineral vatten", product.Title);
            Assert.AreEqual("Gott med äkta mineral vatten", product.Description);
            Assert.AreEqual(6, product.Price);
            Assert.AreEqual("Bonaqua.png", product.Image);
        }

        [TestMethod()]
        public void CreateDiscount()
        {
            //Testar discount konstruktor
            var discount = new Discount("Mamma", 25);

            Assert.AreEqual("Mamma", discount.Code);
            Assert.AreEqual(25, discount.DiscountPercentage);
        }

        [TestMethod()]
        public void ReadProductCount()
        {
            //Kollar hur många rader Produkter som finns i CSV.
            var productTestResult = MainWindow.ReadProductListWithoutGUI();
            Assert.AreEqual(10, productTestResult.Count);
        }

        [TestMethod()]
        public void ReadDiscountCount()
        {
            //Kolla hur många rader rabattkoder som finns i CSV.
            var discountTestResult = MainWindow.ReadDiscountTest();

            Assert.AreEqual(4, discountTestResult.Count);
        }
    }
}