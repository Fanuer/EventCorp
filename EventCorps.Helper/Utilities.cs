using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using EventCorps.Helper.Filter;
using Swashbuckle.Application;

namespace EventCorps.Helper
{
    public static class Utilities
    {
        #region Fields
        private static bool invalid = false;
        #endregion

        public static string GetHash(string input)
        {
            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();
            var byteValue = Encoding.UTF8.GetBytes(input);
            var byteHash = hashAlgorithm.ComputeHash(byteValue);
            return Convert.ToBase64String(byteHash);
        }

        /// <summary>
        /// Returns an objects displayname or the string-representation
        /// </summary>
        /// <param name="obj">calling object</param>
        /// <returns></returns>
        public static string GetDisplayName(this object obj)
        {
            var fieldInfo = obj.GetType().GetField(obj.ToString());

            var descriptionAttributes = fieldInfo.GetCustomAttributes(
                typeof(DisplayAttribute), false) as DisplayAttribute[];

            if (descriptionAttributes == null) return string.Empty;
            return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].GetName() : obj.ToString();
        }

        /// <summary>
        /// Initializes Swagger Documentation
        /// </summary>
        /// <param name="httpConfig">configuration to initialise Swagger</param>
        /// <param name="apiName">Name of the Api (e.g. EventCorp AuthServer API)</param>
        /// <param name="xmlDocName">Name of the XMLDoc file. Must be located within the project's bin-dir</param>
        public static void InitialiseSwagger(HttpConfiguration httpConfig, string apiName, string xmlDocName)
        {
            try
            {
                httpConfig
                .EnableSwagger(c =>
                {
                    c.OperationFilter<AddParametersFilter>();
                    c.SingleApiVersion("v1", apiName);
                    c.IncludeXmlComments(GetXmlPath(xmlDocName));
                    c.IgnoreObsoleteActions();
                    c.DescribeAllEnumsAsStrings();
                }).EnableSwaggerUi();
            }
            catch (Exception e)
            {
                throw new Exception("Error on creating Swagger: " + e.Message + Environment.NewLine + e.StackTrace);
            }

        }

        /// <summary>
        /// Checks if string is a valid email Address
        /// from: https://msdn.microsoft.com/en-us/library/01escwtf(v=vs.110).aspx
        /// </summary>
        /// <param name="strIn">string to test</param>
        /// <returns></returns>
        public static bool IsValidEmail(string strIn)
        {
            invalid = false;
            if (String.IsNullOrEmpty(strIn))
                return false;

            // Use IdnMapping class to convert Unicode domain names.
            try
            {
                strIn = Regex.Replace(strIn, @"(@)(.+)$", Utilities.DomainMapper, RegexOptions.None, TimeSpan.FromMilliseconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

            if (invalid)
                return false;

            // Return true if strIn is in valid e-mail format.
            try
            {
                return Regex.IsMatch(strIn,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private static string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }

        private static string GetXmlPath(string xmlDocName)
        {
            return Path.Combine(HttpRuntime.AppDomainAppPath, "bin", String.Concat(xmlDocName, ".xml"));
        }
    }
}
