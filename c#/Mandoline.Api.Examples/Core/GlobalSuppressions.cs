// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Documentation not required")]
[assembly: SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1611:Element parameters should be documented", Justification = "Documentation not required")]
[assembly: SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1636:File header copyright text should match", Justification = "Documentation not required")]
[assembly: SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1638:File header copyright text should match", Justification = "Documentation not required")]

[assembly: SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1618:Generic type parameters should be documented", Justification = "Documentation not required")]
[assembly: SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Generic type parameters should be documented", Justification = "Documentation not required")]

// [assembly: SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1204:Static elements should appear before instance elements", Justification = "<Pending>", Scope = "member", Target = "~M:Core.Client.Utilities.MandolineWebRequest.HandleWebException``1(System.Net.WebException)~Core.Client.Models.Assertion{``0}")]
[assembly: SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:Elements should appear in the correct order", Justification = "N/A", Scope = "member", Target = "~M:Core.Client.ServiceModels.SelectionDto.#ctor")]

[assembly: SuppressMessage("Design", "CA1068:CancellationToken parameters must come last", Justification = "N/A", Scope = "member", Target = "~M:Core.ApiClient.PerformPost``1(System.Object,System.String,System.Threading.CancellationToken,System.Boolean)~System.Threading.Tasks.Task{Core.Client.Models.Assertion{``0}}")]

[assembly: SuppressMessage("StyleCop.CSharp.NamingRules", "SA1309:Field names should not begin with underscore", Justification = "N/A", Scope = "member", Target = "~F:Core.ApiClient._clientName")]

[assembly: SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:Elements should be ordered by access", Justification = "Conflicts with static", Scope = "member", Target = "~M:Core.Client.Utilities.MandolineWebRequest.Execute``1(System.Object,System.String,System.Threading.CancellationToken)~System.Threading.Tasks.Task{Core.Client.Models.Assertion{``0}}")]
