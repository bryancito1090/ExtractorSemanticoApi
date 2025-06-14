﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ExtractorSemanticoApi.Dominio;

public partial class Review
{
    public int ReviewId { get; set; }

    public int ProductId { get; set; }

    public string UserName { get; set; }

    public string OriginalText { get; set; }

    public string CleanText { get; set; }

    public int? Rating { get; set; }

    public DateTime? ReviewDate { get; set; }

    public string Location { get; set; }

    public virtual ICollection<ExtractedDatum> ExtractedData { get; set; } = new List<ExtractedDatum>();

    public virtual Product Product { get; set; }

    public virtual ICollection<RdfTriple> RdfTriples { get; set; } = new List<RdfTriple>();

    public virtual ICollection<Sentiment> Sentiments { get; set; } = new List<Sentiment>();
}