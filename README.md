V8Media Framework is a base framework for rapid development it provides the most common aspects of an Umbraco website suchas:
 
Base document types for common properties; Website, Base, Menu.
News functionality
Simple Contact form
RSS feeds generator
Sitemap
Navigation
Commonn Extionsions
 
V8Media framework is built using strongly-typed published content models, DocTypes are represented with C# class and enables the default IPublishedContentModelFactory so the content cache returns these strongly typed models instead of just IPublishedContent
For any new doctypes which are a child of the base Menu DocType should have a C# PublishedContentModel created for it as lot of the base uses the built doctype filters e.g. Children<Menu> where it returns 

When setting a site live any changes required to files which can't be changed in dev e.g. web.config, these should be copied into the live folder and then changed.