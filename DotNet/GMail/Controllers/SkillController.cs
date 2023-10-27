using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GMail.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SkillController : ControllerBase
    {
        

        [HttpGet]
        public string  GetSkillConfig()
        {
            return GetEmbbededResoure("ai-plugin.json");
        }

        [HttpGet("apidefs")]
        public string GetSkillApiDefinitions()
        {
            return GetEmbbededResoure("openapi.yaml");
        }

    

        string GetEmbbededResoure(string name)
        {
            try
            {
                Assembly assem = typeof(SkillController).Assembly;


                var stream = assem.GetManifestResourceStream($"GMail.Templates.{name}");
                if (stream == null)
                    return null;

                using (stream)
                using (StreamReader sr = new StreamReader(stream))
                {
                    return sr.ReadToEnd();
                }

            }
            catch (Exception)
            {

            }

            return null;
        }
    }
}
