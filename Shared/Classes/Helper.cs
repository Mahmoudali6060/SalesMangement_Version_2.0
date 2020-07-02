using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Classes
{
    public class Helper
    {
        public static string SerializeObject(object obj)
        {
         return   JsonConvert.SerializeObject(obj, Formatting.None,
                      new JsonSerializerSettings()
                      {
                          ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                      });
        }
    }
}
