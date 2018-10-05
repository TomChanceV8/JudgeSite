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
	/// <summary>Home</summary>
	[PublishedContentModel("Home")]
	public partial class Home : Menu
	{
#pragma warning disable 0109 // new is redundant
		public new const string ModelTypeAlias = "Home";
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
#pragma warning restore 0109

		public Home(IPublishedContent content)
			: base(content)
		{ }

#pragma warning disable 0109 // new is redundant
		public new static PublishedContentType GetModelContentType()
		{
			return PublishedContentType.Get(ModelItemType, ModelTypeAlias);
		}
#pragma warning restore 0109

		public static PublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<Home, TValue>> selector)
		{
			return PublishedContentModelUtility.GetModelPropertyType(GetModelContentType(), selector);
		}

		/// <summary>Text</summary>
		[ImplementPropertyType("bodyText")]
		public IHtmlString BodyText
		{
			get { return this.GetPropertyValue<IHtmlString>("bodyText"); }
		}

		/// <summary>Header</summary>
		[ImplementPropertyType("headerText")]
		public string Header
		{
			get { return this.GetPropertyValue<string>("headerText"); }
		}

		/// <summary>Image Slider</summary>
		[ImplementPropertyType("imageSlider")]
		public IEnumerable<IPublishedContent> ImageSlider
		{
			get { return this.GetPropertyValue<IEnumerable<IPublishedContent>>("imageSlider"); }
		}
	}
}
