using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShSoft.Framework2015.EntityFrameworkTests.Entities;

namespace ShSoft.Framework2015.EntityFrameworkTests
{
    [TestClass]
    public class TPC_Tests
    {

        [TestInitialize]
        public void Init()
        {
            //DbSession dbSession = new DbSession("DefaultConnection");
            //dbSession.Database.Delete();
        }

        [TestMethod]
        public void CreateDataBase()
        {
            DbSession dbSession = new DbSession("DefaultConnection");
        }
    }
}
