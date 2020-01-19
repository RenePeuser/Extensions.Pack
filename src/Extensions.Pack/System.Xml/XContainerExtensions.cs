using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Extensions.Pack
{
    /// <summary>Represents the extensions fro the <see cref="XContainer" /> class.</summary>
    public static class XContainerExtensions
    {
        /// <summary>Gets a specific enumeration of <see cref="XElement" /> from a specific <see cref="XElement" /> by its local name of type <see cref="TcLocalName" />.</summary>
        /// <param name="xContainer">The <see cref="XContainer" /> which contains the expected value.</param>
        /// <param name="localName">The local name of the expected element.</param>
        /// <returns>A specific elements which expect the expected local name<see cref="XElement" />.</returns>
        public static XElement ElementBy(this XContainer xContainer, string localName)
        {
            Throw.IfNull(() => xContainer);
            Throw.IfNull(() => localName);

            var element = xContainer.ElementsBy(localName).FirstOrDefault();

            return element;
        }

        /// <summary>Gets a specific enumeration of <see cref="XElement" /> from a specific <see cref="XElement" /> by its local name of type <see cref="TcLocalName" />.</summary>
        /// <param name="xContainer">The <see cref="XContainer" /> which contains the expected value.</param>
        /// <param name="localName">The local name of the expected element.</param>
        /// <returns>A specific enumeration of elements which expect the expected local name<see cref="XElement" />.</returns>
        public static IEnumerable<XElement> ElementsBy(this XContainer xContainer, string localName)
        {
            Throw.IfNull(() => xContainer);
            Throw.IfNull(() => localName);

            var elements = xContainer.Descendants().Where(item => item.Name.LocalName.Equals(localName));

            return elements;
        }

        /// <summary>Gets a specific of <see cref="XElement" /> from a specific <see cref="XElement" /> by its local name of type <see cref="TcLocalName" />.</summary>
        /// <param name="xContainer">The <see cref="XContainer" /> which contains the expected value.</param>
        /// <param name="attributeName">The attribute name of the attribute element.</param>
        /// <returns>A specific element which expect the expected local name<see cref="XElement" />.</returns>
        public static XElement ElementByAttribute(this XContainer xContainer, string attributeName)
        {
            Throw.IfNull(() => xContainer);
            Throw.IfNull(() => attributeName);

            var element = xContainer.ElementsBy(attributeName).FirstOrDefault();

            return element;
        }

        /// <summary>Gets a specific enumeration of <see cref="XElement" /> from a specific <see cref="XElement" /> by its local name of type <see cref="TcLocalName" />.</summary>
        /// <param name="xContainer">The <see cref="XContainer" /> which contains the expected value.</param>
        /// <param name="attributeName">The attribute name of the attribute element.</param>
        /// <returns>A specific enumeration of elements which expect the expected attribute name<see cref="XElement" />.</returns>
        public static IEnumerable<XElement> ElementsByAttribute(this XContainer xContainer, string attributeName)
        {
            Throw.IfNull(() => xContainer);
            Throw.IfNull(() => attributeName);

            var elements = xContainer.Descendants().Where(item => item.Attributes().Any(a => a.Name.LocalName.Equals(attributeName)));

            return elements;
        }

        /// <summary>Gets a specific enumeration of <see cref="XElement" /> from a specific <see cref="XElement" /> by its local name of type <see cref="TcLocalName" />.</summary>
        /// <param name="xContainer">The <see cref="XContainer" /> which contains the expected value.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="attributeValue">The attribute value.</param>
        /// <returns>A specific enumeration of elements which expect the expected attribute name<see cref="XElement" />.</returns>
        public static XElement ElementByAttribute(this XContainer xContainer, string attributeName, string attributeValue)
        {
            Throw.IfNull(() => xContainer);
            Throw.IfNull(() => attributeName);

            var element = xContainer.Descendants()
                .FirstOrDefault(
                    item => item.Attributes()
                        .Any(
                            a => a.Name.LocalName.EqualsTo(attributeName) && a.Value.EqualsTo(attributeValue)));

            return element;
        }
    }
}
