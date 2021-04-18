using Core.Selenium.WebDriver;
using OpenQA.Selenium;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Core.UI
{
    public class ElementCollection<T> : IEnumerable<T> where T : Element
    {
        public ElementCollection(string locator, Element parent = null)
        {
            Parent = parent;
            Locator = locator;
            Runner = TestRunner.Current;
        }

        protected internal TestRunner Runner;
        protected LocatorHandler Lh = new LocatorHandler();

        protected Element Parent { get; set; }
        public string Locator { get; set; }

        private ISearchContext GetSearchContext()
        {
            if (Parent != null)
            {
                return Parent.WrappedElement;
            }

            return Runner.Driver;
        }

        protected IEnumerable<T> _collection;

        protected IEnumerable<T> Collection
        {
            get
            {
                if (_collection == null)
                {
                    _collection = GetSearchContext().FindElements(Lh.GetSeleniumBy(Locator)).Select(el =>
                    {
                        T item = ((T)Activator.CreateInstance(typeof(T), null, Parent));
                        item.WrappedElement = el;
                        return item;
                    });
                }

                return _collection;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class ElementCollection : ElementCollection<Element>
    {
        public ElementCollection(string locator, Element parent = null) : base(locator, parent)
        {
        }
    }
}