using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FamilyTreeProject.Web.Components;
using System.Web.Mvc;

namespace FamilyTreeProject.Tests.Utilities
{
    public static class AssertEx
    {
        public static void Throws<TException>(Action act) where TException : Exception
        {
            Throws<TException>(act, ex => true);
        }

        public static void Throws<TException>(Action act, Predicate<TException> checker) where TException: Exception
        {
            bool thrown = false;
            try
            {
                act();
            }
            catch (TException ex)
            {
                thrown = checker(ex);
            }
            Assert.IsTrue(thrown, String.Format("Expected exception: {0} was not thrown", typeof(TException).FullName));
        }

        public static void IsErrorResult(ActionResult result, int statusCode)
        {
            HttpStatusCodeResult statusCodeResult = (HttpStatusCodeResult)result;
            Assert.AreEqual(statusCode, statusCodeResult.StatusCode);
            Assert.AreEqual("Error", ((ViewResult)statusCodeResult.NextResult).ViewName);
        }
    }
}
