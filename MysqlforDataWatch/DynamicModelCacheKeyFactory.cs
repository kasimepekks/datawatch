using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace MysqlforDataWatch
{
    public class DynamicModelCacheKeyFactory : IModelCacheKeyFactory
    {
        public object Create(DbContext context)
        {
           var result= context is datawatchContext dynamicContext
               ? (context.GetType(), dynamicContext.vehicleid)
               : (object)context.GetType();
            return result;
        }
    }
}
