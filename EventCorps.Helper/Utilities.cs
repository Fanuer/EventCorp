using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using EventCorps.Helper.Filter;
using Swashbuckle.Application;

namespace EventCorps.Helper
{
    public static class Utilities
    {
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

        private static string GetXmlPath(string xmlDocName)
        {
            return Path.Combine(HttpRuntime.AppDomainAppPath, "bin", String.Concat(xmlDocName, ".xml"));
        }
    }
}
