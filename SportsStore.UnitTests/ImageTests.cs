﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Web.Mvc;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class ImageTests
    {
        [TestMethod]
        public void Can_Retreieve_Image_Data()
        {
            // arrange
            Product prod = new Product
                {
                    ProductID = 2,
                    Name = "Test",
                    ImageData = new byte[] {},
                    ImageMimeType = "image/png"
                };

            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
                {
                    new Product {ProductID = 1, Name = "P1"},
                    prod,
                    new Product {ProductID = 3, Name = "P3"}
                }.AsQueryable());

            ProductController target = new ProductController(mock.Object);

            // act
            ActionResult result = target.GetImage(2);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(prod.ImageMimeType, ((FileResult)result).ContentType);
        }

        [TestMethod]
        public void Cannot_Retreive_Data_For_Invalid_ID()
        {
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
                {
                    new Product {ProductID = 1, Name = "P1"},
                    new Product {ProductID = 2, Name = "P2"}
                }.AsQueryable());

            ProductController target = new ProductController(mock.Object);

            // act
            ActionResult result = target.GetImage(100);

            // assert
            Assert.IsNull(result);
        }
    }
}
