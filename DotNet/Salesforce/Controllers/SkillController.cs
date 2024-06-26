﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Reflection;

namespace Salesforce.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SkillController : ControllerBase
    {
        [HttpGet]
        [HttpGet("/")]
        public string  GetSkillConfig()
        {
            return GetEmbbededResoure("ai-plugin.json");
        }

        [HttpGet("apidefs")]
        [HttpGet("/apidefs")]
        public string GetSkillApiDefinitions()
        {
            return GetEmbbededResoure("openapi.yaml");
        }

        string GetEmbbededResoure(string name)
        {
            try
            {
                Assembly assem = typeof(SkillController).Assembly;
                var stream = assem.GetManifestResourceStream($"Salesforce.Templates.{name}");
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
