﻿extern alias MicrosoftProjectOxfordCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.ContentSearch.Diagnostics;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;

namespace Sitecore.SharedSource.CognitiveServices.Search.ComputedFields
{
    public abstract class BaseComputedField : IComputedIndexField
    {
        public virtual string FieldName { get; set; }
        public virtual string ReturnType { get; set; }

        public List<string> TextualFieldTypes = new List<string>() { "Rich Text", "Single-Line Text", "Multi-Line Text", "html", "text", "memo" };

        public object ComputeFieldValue(IIndexable indexable)
        {
            Assert.ArgumentNotNull(indexable, "indexable");

            Item indexItem = indexable as SitecoreIndexableItem;
            if (indexItem == null)
                return null;

            if (indexItem?.Database?.Name == "core")
                return null;

            if (indexItem.TemplateID == TemplateIDs.MediaFolder || indexItem.ID == ItemIDs.MediaLibraryRoot)
                return null;
            
            try {

                CognitiveIndexableItem cognitiveIndexable;
                try
                {
                    cognitiveIndexable = (CognitiveIndexableItem)indexable;
                }
                catch (Exception ex)
                {
                    CrawlingLog.Log.Warn("Unable to convert indexable to CognitiveIndexableItem. Id: " + indexable.UniqueId + ", Message: " + ex.Message);
                    return null;
                }
                return GetFieldValue(cognitiveIndexable);
            } catch (Exception e) {
                Log.Error($"Error indexing field: {FieldName}, Item Path: {indexItem.Paths.FullPath}", e, GetType());
            }

            return null;
        }

        protected virtual object GetFieldValue(CognitiveIndexableItem cognitiveIndexable)
        {
            return null;
        }

        protected void LogError(Exception ex, Item indexItem)
        {
            MicrosoftProjectOxfordCommon::Microsoft.ProjectOxford.Common.ClientException exception = ex.InnerException as MicrosoftProjectOxfordCommon::Microsoft.ProjectOxford.Common.ClientException;

            if (exception != null)
                Log.Error($"ImageItemAnalysis failed to index {indexItem.Paths.Path}: {exception.Error.Message}", exception, GetType());
            else
                Log.Error(ex.Message, ex, GetType());
        }

        /// <summary>
        /// This will get all text based content fields (as opposed to system fields) from the provided item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public IEnumerable<Field> GetTextualFields(Item item)
        {
            IEnumerable<Field> fields = item.Fields
                    .Where(f => !f.Name.StartsWith("__") && TextualFieldTypes.Contains(f.Type));

            return fields;
        }

        protected string RemoveHtmlMarkup(string value)
        {
            return Regex.Replace(value, "<.*?>", string.Empty);
        }

        protected string GetFormattedString(string value, int sizeLimit)
        {
            string plainString = RemoveHtmlMarkup(value);

            var contentSize = Encoding.Unicode.GetByteCount(plainString);
            if (contentSize < sizeLimit)
                return plainString;

            string[] sentences = plainString.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sb = new StringBuilder();
            foreach (string s in sentences)
            {
                if (Encoding.Unicode.GetByteCount($"{sb} {s}.") > sizeLimit)
                    break;

                sb.Append(sb.Length == 0 ? $"{s}." : $" {s}");
            }

            return sb.ToString();
        }
    }
}