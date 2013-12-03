using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MedienKultur.CollectionJsonExtended;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using MedienKultur.Gurps.Models.Extensions;



namespace MedienKultur.Gurps.Models
{

    
    
    
    public abstract class ContentBundle
    {
        public abstract class Content
        {
            public string Data { get; set; }

        }

        public class Text : ContentBundle.Content
        {
            public Image Image { get; set; }
        }

        public class Section : Content
        {
            public string Title { get; set; }
        }

        public enum QuoteType
        {
            Written,
            Spoken
        }

        public class Quote : Content
        {
            public string Author { get; set; }
            public Icon Icon { get; set; }
            public QuoteType QuoteType { get; set; }
        }

        
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public abstract int Id { get; set; }
        public abstract IList<Content> Contents { get; protected set; }

    }

 
    public class Article : ContentBundle, IJsonSerializeable
    {
        private IList<Content> _contents = new List<Content>();

        public Article()
        {
            
        }

        public override int Id { get; set; }
        public int UserId { get; set; }

        [CollectionJsonConcrete(typeof(ContentBundle.Text))]
        [CollectionJsonConcrete(typeof(ContentBundle.Section))]
        [CollectionJsonConcrete(typeof(ContentBundle.Quote))]
        public override IList<Content> Contents
        {
            get { return _contents; }
            protected set { _contents = value; }
        }

        public string Header { get; set; }
        public string Tooltip { get; set; }
        public string[] Categories { get; set; }
        public string[] HashTags { get; set; }
        public AlienDate AlienDate { get; set; }
        public AlienDate AlienDateTo { get; set; }


    }


    public enum IconType
        {
            Sprite,
            Image
        }

    public class Icon
    {
        public Icon(IconType iconType, string value)
        {
            IconType = iconType;
            Value = value;
        }
 
        public IconType IconType { get; set; }
        public string Value { get; set; }
    }


    public abstract class Resource
    {
        string _url;
        string _path;

        public enum ImageType
        {
            png,
            jpg,
            gif
        }

        public string Url
        {
            get { return _url ?? HttpContext.Current.Server.MapPath(_path); }
            set
            {
                value = value.ToLowerInvariant();
                if (value.StartsWith("http://") || value.StartsWith("https://"))
                {
                    _url = value;
                }
                else
                {
                    _path = value;
                }

            }
        }

        public Decimal ByteSize { get; set; }
        
    }

    public class Image : Resource
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public ImageType ImageType { get; set; }
    }

    public class Download : Resource
    {
        public string MimeType { get; set; }
    }

    
    public class AlienDate
    {
        public string Date { get; set; }
    }


    public class HashTag
    {
        public string Tag { get; set; }
        public int UserId { get; set; }
    }


}
