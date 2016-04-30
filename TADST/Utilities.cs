using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace TADST
{
    public static class Utilities
    {
        /// <summary>
        /// Create a deep clone of any serializable object.
        /// Used for copying profile instead of using copy constructor.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="original"></param>
        /// <returns></returns>
        public static T DeepClone<T>(T original)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "original");
            }

            if (ReferenceEquals(original, null))
            {
                return default(T);
            }

            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter
                {
                    Context = new StreamingContext(StreamingContextStates.Clone)
                };

                formatter.Serialize(stream, original);
                stream.Position = 0;

                return (T)formatter.Deserialize(stream);
            }
        }

        /// <summary>
        /// Convert a string to mixed case (first letter in every word is uppercase)
        /// </summary>
        /// <param name="str">String to change to title case</param>
        /// <returns>Title case formatted string</returns>
        public static string ToMixedCase(string str)
        {
            System.Globalization.CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Globalization.TextInfo textInfo = cultureInfo.TextInfo;

            return textInfo.ToTitleCase(str.ToLower());    
        }

        /// <summary>
        /// Swap places of items in a list
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="list">The list calling the function</param>
        /// <param name="indexA">Index of first item</param>
        /// <param name="indexB">Index of second item</param>
        /// <returns>List with new order of items</returns>
        public static IList<T> Swap<T>(this IList<T> list, int indexA, int indexB)
        {
            if (indexB > -1 && indexB < list.Count)
            {
                T tmp = list[indexA];
                list[indexA] = list[indexB];
                list[indexB] = tmp;
            }
            return list;
        }

        /// <summary>
        /// Swap places of two items
        /// </summary>
        /// <param name="lstBox">The calling CheckedListBox</param>
        /// <param name="indexA">Index of forst item</param>
        /// <param name="indexB">Index of second item</param>
        /// <returns>CheckedListBox with new item order</returns>
        public static System.Windows.Forms.CheckedListBox SwapItems(this System.Windows.Forms.CheckedListBox lstBox, int indexA, int indexB)
        {
            if (indexB > -1 && indexB < lstBox.Items.Count)
            {
                object tmpItem = lstBox.Items[indexA];
                lstBox.Items[indexA] = lstBox.Items[indexB];
                lstBox.Items[indexB] = tmpItem;

                var stateA = lstBox.GetItemCheckState(indexA);
                var stateB = lstBox.GetItemCheckState(indexB);
                lstBox.SetItemCheckState(indexB, stateA);
                lstBox.SetItemCheckState(indexA, stateB);
            }
            return lstBox;
        }

        /// <summary>
        /// Creates random password
        /// </summary>
        /// <param name="passwordLength">Length of generated password</param>
        /// <returns>Generated password of selected length</returns>
        public static string CreateRandomPassword(int passwordLength)
        {
            const string consonants = "bdfghjklmnprstvyw";
            const string wovels = "aeiou";

            var password = "";
            var randomNum = new Random();

            while (password.Length < passwordLength)
            {
                password += consonants[randomNum.Next(consonants.Length)];
                if (password.Length < passwordLength)
                    password += wovels[randomNum.Next(wovels.Length)];
            }

            return password;
        }
    }
}
