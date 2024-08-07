using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Reflection;

namespace Zoho.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SkillController : ControllerBase
    {
        [HttpGet("")]
        [HttpGet("~/")]
        public string  GetSkillConfig()
        {
            return GetEmbeddedResource("ai-plugin.json");
        }

        [HttpGet("apidefs")]
        [HttpGet("~/apidefs")]
        public string GetSkillApiDefinitions()
        {
            return GetEmbeddedResource("openapi.yaml");
        }

        string GetEmbeddedResource(string name)
        {
            try
            {
                Assembly assem = typeof(SkillController).Assembly;
                var stream = assem.GetManifestResourceStream($"Zoho.Templates.{name}");
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
                System.Diagnostics.Debug.WriteLine("[vertex][Zoho][Skill]Embedded Resource Ex");
            }

            return null;
        }
    }
}