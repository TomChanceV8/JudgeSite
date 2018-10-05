//------------------------------------------------------------------------------
// <auto-generated>
//   This code was generated by a tool.
//
//    Zbu.ModelsBuilder v2.1.3.52
//
//   Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Zbu.ModelsBuilder;
using Zbu.ModelsBuilder.Umbraco;

namespace V8Media.Framework.Models.PublishedContent
{
	/// <summary>Menu</summary>
	[PublishedContentModel("Menu")]
	public partial class Menu : Base
	{
#pragma warning disable 0109 // new is redundant
		public new const string ModelTypeAlias = "Menu";
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
#pragma warning restore 0109

		public Menu(IPublishedContent content)
			: base(content)
		{ }

#pragma warning disable 0109 // new is redundant
		public new static PublishedContentType GetModelContentType()
		{
			return PublishedContentType.Get(ModelItemType, ModelTypeAlias);
		}
#pragma warning restore 0109

		public static PublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<Menu, TValue>> selector)
		{
			return PublishedContentModelUtility.GetModelPropertyType(GetModelContentType(), selector);
		}

		/// <summary>Hide In Menu</summary>
		[ImplementPropertyType("umbracoNaviHide")]
		public bool HideInMenu
		{
			get { return this.GetPropertyValue<bool>("umbracoNaviHide"); }
		}

		/// <summary>Hide In Sitemap</summary>
		[ImplementPropertyType("hideInSitemap")]
		public bool HideInSitemap
		{
			get { return this.GetPropertyValue<bool>("hideInSitemap"); }
		}

		/// <summary>URL Redirect</summary>
		[ImplementPropertyType("umbracoRedirect")]
		public IPublishedContent UmbracoRedirect
		{
			get { return this.GetPropertyValue<IPublishedContent>("umbracoRedirect"); }
		}

		/// <summary>URL Alias</summary>
		[ImplementPropertyType("umbracoUrlAlias")]
		public string UmbracoUrlAlias
		{
			get { return this.GetPropertyValue<string>("umbracoUrlAlias"); }
		}

		/// <summary>URL Name</summary>
		[ImplementPropertyType("umbracoUrlName")]
		public string UmbracoUrlName
		{
			get { return this.GetPropertyValue<string>("umbracoUrlName"); }
		}
	}
}
