﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ExtractorSemanticoApi.Dominio;

public partial class RdfTriple
{
    public int TripleId { get; set; }

    public string Subject { get; set; }

    public string Predicate { get; set; }

    public string Object { get; set; }

    public int? SourceReviewId { get; set; }

    public virtual Review SourceReview { get; set; }
}