﻿using System;
using Ebuy;

namespace IntegrationTests
{
    public class TestDataGenerator
    {
        private volatile static int _lastRandomNumber;

        public static TestDataGenerator Current
        {
            get { return _current; }
            set { _current = value; }
        }
        private static TestDataGenerator _current = new TestDataGenerator();

        public T GenerateValid<T>()
        {
            var type = typeof(T);

            if (type == typeof(Auction))
                return (T)GenerateValidAuction();

            if (type == typeof(Category))
                return (T)GenerateValidCategory();

            if (type == typeof(Product))
                return (T)GenerateValidProduct();

            if (type == typeof(User))
                return (T)GenerateValidUser();

            throw new NotSupportedException("No test data generator registered for type " + type.Name);
        }

        protected virtual object GenerateValidAuction()
        {
            var id = Random();
            var auction = new Auction()
                       {
                           Title = "Test Auction #" + id,
                           Description = "Description for test auction #" + id,
                           CurrentPrice = "$1",
                           StartTime = Clock.Now,
                           EndTime = Clock.Now.AddDays(7),
                           Owner = GenerateValid<User>(),
                           Product = GenerateValid<Product>(),
                           Categories = new[] { GenerateValid<Category>(), GenerateValid<Category>() },
                       };

            auction.Images.Add(string.Format("http://www.test.com/image_{0}.png", id));

            return auction;
        }

        protected virtual object GenerateValidCategory()
        {
            var id = Random();
            return new Category { Name = "Test Category #" + id };
        }

        protected virtual object GenerateValidProduct()
        {
            var id = Random();
            return new Product()
            {
                Categories = new[] { GenerateValid<Category>(), GenerateValid<Category>() },
                Name = "Test product " + id,
                Description = "Test product " + id,
                Images = new WebsiteImage[] { "http://www.test.com/image.png" },
            };
        }

        protected virtual object GenerateValidUser()
        {
            var id = Random();

            return new User()
            {
                DisplayName = "Test User #" + id,
                EmailAddress = String.Format("user_{0}@email.com", id),
            };
        }

        protected virtual long Random()
        {
            return _lastRandomNumber++;
        }
    }
}
