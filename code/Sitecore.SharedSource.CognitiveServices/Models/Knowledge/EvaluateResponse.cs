﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.SharedSource.CognitiveServices.Models.Knowledge {
    public class EvaluateResponse {
        public string Expr { get; set; }
        public List<EvaluateResult> Entities { get; set; } 
    }
}