using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MedienKultur.CollectionJsonExtended;
using MedienKultur.Gurps.Models;
using Raven.Client;

namespace MedienKultur.Gurps.Controllers
{
    public class JArticleController : ApiController
    {
        readonly IDocumentSession _ravenSession;
        
        public JArticleController(IDocumentSession ravenSession)
        {
            _ravenSession = ravenSession;
        }
        
        // GET api/jarticle
        public CollectionJsonWriter<Article> Get()
        {
            var models = _ravenSession.Query<Article>();
            return new CollectionJsonWriter<Article>(models);
        }

        // GET api/jarticle/5
        public CollectionJsonWriter<Article> Get(int id)
        {
            var model = _ravenSession.Load<Article>(id);
            return new CollectionJsonWriter<Article>(model);  
        }

        // POST api/jarticle
        public CollectionJsonWriter<Article> Post(CollectionJsonReader<Article> articleReader)
        {
            if (articleReader != null)
            {
                _ravenSession.Store(articleReader.Entity);
                _ravenSession.SaveChanges();
                var article = _ravenSession.Query<Article>()
                                           .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite())
                                           .AsEnumerable()
                                           .LastOrDefault(); //this works, but i do not ike it. maybe i want to get the next id in advance....? when i have it i must lock the documents to prevent somebody else gets it?
                return new CollectionJsonWriter<Article>(article); //but you probably do not need the result as it is in the client.. well if you want to update it... you need at leat the link...
            }

            return new CollectionJsonWriter<Article>(new HttpResponseMessage(HttpStatusCode.NotAcceptable));
        }

        //public CollectionJsonWriter<Article> Post(CollectionJsonReader<IEnumerable<Article>> articleReader)
        //{
        //    throw new NotImplementedException();
        //}


        // PUT api/jarticle/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/jarticle/5
        public void Delete(int id)
        {
        }
    }
}

//input collection json

/*
 {"template": {
      "data": [
        {
          "name": "header",
          "value": "Der Article Header"
        },
        {
          "name": "tooltip",
          "value": "Der Tooltip"
        },
        {
          "name": "categories",
          "values": []
        },
        {
          "name": "hashTags",
          "values": []
        },
        {
          "name": "alienDate",
          "object": null,
          ,
          "prompt": "Alien Date",
          "type": "AlienDate"
        },
        {
          "name": "alienDateTo",
          "object": null,
          "data": [],
          "prompt": "Alien Date To",
          "type": "AlienDate"
        },
        {
          "name": "contents",
          "abstracts": [
            {
              "concrete": "Text",
              "data": [
                {
                  "name": "data",
                  "value": "Ich bin Data von Text."
                }
              ]
            },
            {
              "concrete": "Section",
              "data": [
                {
                  "name": "title",
                  "value": "Ich bin title von Text."
                },
                {
                  "name": "data",
                  "value": "Ich bin Data von Section"
                }
              ]
            }
          ]
        },
        {
          "name": "created",
          "value": "0001-01-01T00:00:00"
        },
        {
          "name": "modified",
          "value": "0001-01-01T00:00:00"
        },
        {
          "name": "id",
          "value": 0
        },
        {
          "name": "userId",
          "value": 0
        }
      ]
    }
  }
 */

//ouptup collectionjson

/*
{
  "collection": {
    "version": "1.0",
    "href": "http://www.example.org/article",
    "links": [],
    "items": [],
    "queries": [],
    "template": {
      "data": [
        {
          "name": "contents",
          "abstracts": [],
          "concretes": [
            {
              "concrete": "Quote",
              "data": [
                {
                  "name": "author",
                  "value": "",
                  "prompt": "Author",
                  "type": "String"
                },
                {
                  "name": "icon",
                  "object": null,
                  "data": [
                    {
                      "name": "iconType",
                      "value": 0,
                      "options": [
                        0,
                        1
                      ],
                      "prompt": "Icon Type",
                      "type": "IconType"
                    },
                    {
                      "name": "value",
                      "value": "",
                      "prompt": "Value",
                      "type": "String"
                    }
                  ],
                  "prompt": "Icon",
                  "type": "Icon"
                },
                {
                  "name": "quoteType",
                  "value": 0,
                  "options": [
                    0,
                    1
                  ],
                  "prompt": "Quote Type",
                  "type": "QuoteType"
                },
                {
                  "name": "data",
                  "value": "",
                  "prompt": "Data",
                  "type": "String"
                }
              ]
            },
            {
              "concrete": "Section",
              "data": [
                {
                  "name": "title",
                  "value": "",
                  "prompt": "Title",
                  "type": "String"
                },
                {
                  "name": "data",
                  "value": "",
                  "prompt": "Data",
                  "type": "String"
                }
              ]
            },
            {
              "concrete": "Text",
              "data": [
                {
                  "name": "image",
                  "object": null,
                  "data": [
                    {
                      "name": "width",
                      "value": 0,
                      "prompt": "Width",
                      "type": "Int32"
                    },
                    {
                      "name": "height",
                      "value": 0,
                      "prompt": "Height",
                      "type": "Int32"
                    },
                    {
                      "name": "imageType",
                      "value": 0,
                      "options": [
                        0,
                        1,
                        2
                      ],
                      "prompt": "Image Type",
                      "type": "ImageType"
                    },
                    {
                      "name": "url",
                      "value": "",
                      "prompt": "Url",
                      "type": "String"
                    },
                    {
                      "name": "byteSize",
                      "value": 0.0,
                      "prompt": "Byte Size",
                      "type": "Decimal"
                    }
                  ],
                  "prompt": "Image",
                  "type": "Image"
                },
                {
                  "name": "data",
                  "value": "",
                  "prompt": "Data",
                  "type": "String"
                }
              ]
            }
          ],
          "prompt": "Contents",
          "type": "IList`1[Content]"
        },
        {
          "name": "header",
          "value": "",
          "prompt": "Header",
          "type": "String"
        },
        {
          "name": "tooltip",
          "value": "",
          "prompt": "Tooltip",
          "type": "String"
        },
        {
          "name": "categories",
          "values": [],
          "prompt": "Categories",
          "type": "String[]"
        },
        {
          "name": "hashTags",
          "values": [],
          "prompt": "Hash Tags",
          "type": "String[]"
        },
        {
          "name": "alienDate",
          "object": null,
          "data": [
            {
              "name": "date",
              "value": "",
              "prompt": "Date",
              "type": "String"
            }
          ],
          "prompt": "Alien Date",
          "type": "AlienDate"
        },
        {
          "name": "alienDateTo",
          "object": null,
          "data": [
            {
              "name": "date",
              "value": "",
              "prompt": "Date",
              "type": "String"
            }
          ],
          "prompt": "Alien Date To",
          "type": "AlienDate"
        },
        {
          "name": "created",
          "value": "0001-01-01T00:00:00",
          "prompt": "Created",
          "type": "DateTime"
        },
        {
          "name": "modified",
          "value": "0001-01-01T00:00:00",
          "prompt": "Modified",
          "type": "DateTime"
        },
        {
          "name": "id",
          "value": 0,
          "prompt": "Id",
          "type": "Int32"
        },
        {
          "name": "userId",
          "value": 0,
          "prompt": "User Id",
          "type": "Int32"
        }
      ]
    }
  }
}
 */
