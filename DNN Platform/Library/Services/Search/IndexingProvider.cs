#region Copyright
// 
// DotNetNukeŽ - http://www.dotnetnuke.com
// Copyright (c) 2002-2014
// by DotNetNuke Corporation
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
#endregion
#region Usings

using System;
using System.Collections.Generic;

using DotNetNuke.ComponentModel;
using DotNetNuke.Services.Search.Entities;

#endregion

namespace DotNetNuke.Services.Search
{
    public abstract class IndexingProvider
    {
        public static IndexingProvider Instance()
        {
            return ComponentFactory.GetComponent<IndexingProvider>();
        }

        /// <summary>
        /// This method must save search docuents in batches to minimize memory usage instead of returning all documents ollection at once.
        /// </summary>
        /// <param name="portalId">Portal ID to index</param>
        /// <param name="startDateLocal">Minimum modification date of items that need to be indexed</param>
        /// <param name="indexer">A delegate function to send the collection of documents to for saving/indexing</param>
        /// <returns></returns>
        public virtual int IndexSearchDocuments(int portalId, DateTime startDateLocal, Action<IEnumerable<SearchDocument>> indexer)
        {
            throw new NotImplementedException();
        }

        [Obsolete("Depricated in DNN 7.4.2 Use 'IndexSearchDocuments' instead for lower memory footprint during search.")]
        public virtual IEnumerable<SearchDocument> GetSearchDocuments(int portalId, DateTime startDateLocal)
        {
            return new List<SearchDocument>();
        }

        [Obsolete("Legacy Search (ISearchable) -- Depricated in DNN 7.1. Use 'GetSearchDocuments' instead.")]
        public abstract SearchItemInfoCollection GetSearchIndexItems(int portalId);
    }
}