﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class Section
    {
        public Section(string SectionTitle)
        {
            this.Title = SectionTitle;
        }

        private List<Link> _links = new List<Link>();

        public string Title { get; set; }

        public void AddLink(string LinkTitle, LinkType LinkType, string LinkURL)
        {
            _links.Add(new Link(LinkTitle, LinkType, LinkURL) { });
        }

        public Link[] Links => this._links.ToArray();
    }
}
