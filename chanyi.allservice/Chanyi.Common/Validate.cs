using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Chanyi.Common.Domain;

namespace Chanyi.Common
{
    public static class Validate
    {
        public static void IsTrue(bool expression, string message)
        {
            Validate.IsTrue(expression, message, null);
        }

        public static void IsTrue(bool expression, string message, params object[] args)
        {
            if (!expression)
            {
                throw new DomainException(message, args);
            }
        }


        public static void NoNullElements<T>(IEnumerable<T> collection, string message)
        {
            Validate.NoNullElements(collection, message, null);
        }

        public static void NoNullElements<T>(IEnumerable<T> collection, string message, params object[] args)
        {
            foreach (T t in collection)
            {
                if (t == null)
                {
                    throw new DomainException(message, args);
                }
            }
        }


        public static void NotEmpty<T>(ICollection<T> collection, string message)
        {
            Validate.NotEmpty(collection, message, null);
        }

        public static void NotEmpty<T>(ICollection<T> collection, string message, params object[] args)
        {
            if (collection.Count == 0)
            {
                throw new DomainException(message, args);
            }
        }


        public static void NotEmptyOrWhiteSpace(string str, string message)
        {
            Validate.NotEmptyOrWhiteSpace(str, message, null);
        }

        public static void NotEmptyOrWhiteSpace(string str, string message, params object[] args)
        {
            if (String.IsNullOrWhiteSpace(str))
            {
                throw new DomainException(message, args);
            }
        }


        public static void NotNull(object obj, string message)
        {
            Validate.NotNull(obj, message, null);
        }

        public static void NotNull(object obj, string message, params object[] args)
        {
            if (obj == null)
            {
                throw new DomainException(message, args);
            }
        }
    }
}
