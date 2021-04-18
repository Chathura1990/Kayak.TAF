using Core.Attributes;
using Core.UI;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Core
{
    public class Block : Element
    {
        public Block(string locator) : base(locator)
        {
        }

        protected void InitializeFields()
        {
            Type findByType = typeof(FindByAttribute);
            Type elementType = typeof(Element);
            Type collectionType = typeof(ElementCollection);
            Type genericCollectionType = typeof(ElementCollection<>);

            FieldInfo[] fields = GetType()
                .GetFields(BindingFlags.Public | BindingFlags.Instance);
            IEnumerable<FieldInfo> fieldsToInitialize = fields
                .Where(prop =>
                    (
                        elementType.IsAssignableFrom(prop.FieldType)
                        || collectionType.IsAssignableFrom(prop.FieldType)
                        || (prop.FieldType.IsGenericType && prop.FieldType.GetGenericTypeDefinition() == genericCollectionType)
                    )
                    && prop.GetCustomAttributes(findByType).Any());

            foreach (FieldInfo field in fieldsToInitialize)
            {
                FindByAttribute findByAttribute = (FindByAttribute)field.GetCustomAttributes(findByType).First();
                string locator = findByAttribute.Locator;

                Type actualFieldType = field.FieldType;

                if (elementType.IsAssignableFrom(actualFieldType))
                {
                    field.SetValue(this, Activator.CreateInstance(actualFieldType, locator, this));
                    continue;
                }

                if (collectionType.IsAssignableFrom(actualFieldType))
                {
                    field.SetValue(this, Activator.CreateInstance(actualFieldType, locator, this));
                    continue;
                }

                if (actualFieldType.IsGenericType && actualFieldType.GetGenericTypeDefinition() == genericCollectionType)
                {
                    field.SetValue(this, Activator.CreateInstance(actualFieldType, locator, this));
                }
            }
        }

        public void WaitLoadIndicatorsFields()
        {
            Type loadIndicatorType = typeof(LoadIndicatorAttribute);
            Type elementType = typeof(Element);
            Type collectionType = typeof(ElementCollection);
            Type genericCollectionType = typeof(ElementCollection<>);

            IEnumerable<FieldInfo> fieldsToWait =
                GetType()
                .GetFields(BindingFlags.Public | BindingFlags.Instance)
                .Where(prop => (
                    elementType.IsAssignableFrom(prop.FieldType)
                            || collectionType.IsAssignableFrom(prop.FieldType)
                            || (prop.FieldType.IsGenericType && prop.FieldType.GetGenericTypeDefinition() == genericCollectionType)
                    )
                    && prop.GetCustomAttributes(loadIndicatorType).Any());

            foreach (FieldInfo field in fieldsToWait)
            {
                Type actualFieldType = field.FieldType;
                LoadIndicatorAttribute loadIndicatorAttribute = (LoadIndicatorAttribute)field.GetCustomAttributes(loadIndicatorType).First();

                if (elementType.IsAssignableFrom(actualFieldType))
                {
                    Element actualField = (Element)field.GetValue(this);
                    if (loadIndicatorAttribute.WaitVisible)
                    {
                        actualField.WaitFor(Condition.Displayed);
                    }
                    else
                    {
                        actualField.WaitFor(el => el.IsPresent());
                    }
                    continue;
                }

                if (collectionType.IsAssignableFrom(actualFieldType))
                {
                    IEnumerable<Element> actualField = (IEnumerable<Element>)field.GetValue(this);
                    if (loadIndicatorAttribute.WaitVisible)
                    {
                        actualField.WaitForVisible();
                    }
                    else
                    {
                        actualField.WaitForPresent();
                    }
                    continue;
                }

                if (collectionType.IsAssignableFrom(actualFieldType)
                    || (actualFieldType.IsGenericType && actualFieldType.GetGenericTypeDefinition() == genericCollectionType))
                {
                    IEnumerable<Element> actualField = (IEnumerable<Element>)field.GetValue(this);
                    if (loadIndicatorAttribute.WaitVisible)
                    {
                        actualField.WaitForVisible();
                    }
                    else
                    {
                        actualField.WaitForPresent();
                    }
                }
            }
        }
    }
}