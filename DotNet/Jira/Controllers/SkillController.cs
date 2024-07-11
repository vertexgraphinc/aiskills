using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Reflection;

namespace Jira.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SkillController : ControllerBase
    {
        [HttpGet(""),HttpGet("~/")]
        public string GetSkillConfig()
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Jira][Skill]Get config skill");

            return GetEmbeddedResource("ai-plugin.json");
        }

        [HttpGet("apidefs"),HttpGet("~/apidefs")]
        public string GetSkillApiDefinitions()
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Jira][Skill]Get api definitions");

            return GetEmbeddedResource("openapi.yaml");
        }

        string GetEmbeddedResource(string name)
        {
            try
            {
                Assembly assem = typeof(SkillController).Assembly;
                var stream = assem.GetManifestResourceStream($"Jira.Templates.{name}");
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
