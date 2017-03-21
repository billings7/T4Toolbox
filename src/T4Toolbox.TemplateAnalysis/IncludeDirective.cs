// <copyright file="IncludeDirective.cs" company="Oleg Sych">
//  Copyright © Oleg Sych. All Rights Reserved.
// </copyright>

namespace T4Toolbox.TemplateAnalysis
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;

    [DisplayName("include"), Description("Includes text from another file in the current template.")]
    internal sealed class IncludeDirective : Directive
    {
        public IncludeDirective(DirectiveBlockStart start, DirectiveName name, IEnumerable<Attribute> attributes, BlockEnd end)
            : base(start, name, attributes, end)
        {
            Debug.Assert(name.Text.Equals("include", StringComparison.OrdinalIgnoreCase), "name");
        }

        [Required(ErrorMessage = "The File attribute is required")]
        [Description("Absolute or relative path to the included file.")]
        public string File
        {
            get { return this.GetAttributeValue(); }
        }

        [KnownValues(typeof(IncludeDirective), nameof(GetKnownOnceValues))]
        [Description("Used to ensure that a template is included only once, even if it’s invoked from more than one other include file.")]
        public string Once
        {
            get { return this.GetAttributeValue(); }
        }

        protected internal override void Accept(SyntaxNodeVisitor visitor)
        {
            visitor.VisitIncludeDirective(this);
        }

        private static IEnumerable<ValueDescriptor> GetKnownOnceValues()
        {
            yield return new ValueDescriptor("false", "The template will be included in each file that invokes it. False is the default.");
            yield return new ValueDescriptor("true", "The template will only be inluced once, even if it’s invoked from more than one other include file.");
        }
    }
}