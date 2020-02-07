using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WallpaperManager.Tagging
{
    public class TaggingInfo
    {
        private List<CategoryData> CategoryInfo;

        public TaggingInfo(List<CategoryData> categoryInfo = null)
        {
            if (categoryInfo != null)
            {
                CategoryInfo = new List<CategoryData>(categoryInfo);
            }
            else
            {
                CategoryInfo = new List<CategoryData>();
            }
        }

        // Categories
        public void AddCategory(CategoryData category)
        {
            if (!ContainsCategory(category))
            {
                CategoryInfo.Add(category);
            }
        }

        public void RemoveCategory(CategoryData category)
        {
            if (ContainsCategory(category))
            {
                // First, unlink every tag inside the category from the images they're linked to
                //? No need to call RemoveTag since we aren't concerned with the hashset
                foreach (TagData tag in category.Tags)
                {
                    tag.ClearTag();
                }

                // Now remove the actual category
                CategoryInfo.Remove(category);
            }
        }

        public CategoryData GetCategory(int categoryIndex)
        {
            return CategoryInfo[categoryIndex];
        }

        public CategoryData GetCategory(string categoryName)
        {
            foreach (CategoryData category in CategoryInfo)
            {
                if (category.Name == categoryName)
                {
                    return category;
                }
            }

            return null;
        }

        public CategoryData[] GetAllCategories()
        {
            return CategoryInfo.ToArray();
        }

        public int GetCategoryIndex(CategoryData category)
        {
            return CategoryInfo.IndexOf(category);
        }

        public void SetCategory(int index, CategoryData category)
        {
            CategoryInfo[index] = category;
        }

        public bool ContainsCategory(CategoryData category)
        {
            return ContainsCategory(category.Name);
        }

        public bool ContainsCategory(string categoryName)
        {
            foreach (CategoryData curCategory in CategoryInfo)
            {
                if (curCategory.Name == categoryName) // this category already exists
                {
                    return true;
                }
            }

            return false;
        }

        public int CategoryCount()
        {
            return CategoryInfo.Count;
        }

        // Tags
        public void AddTag(TagData tag)
        {
            GetTagParentCategory(tag).Tags.Add(tag);
        }

        public void RemoveTag(TagData tag)
        {
            tag.ClearTag();
            GetTagParentCategory(tag).Tags.Remove(tag);
        }

        public TagData GetTag(string categoryName, string tagName)
        {
            foreach (CategoryData category in CategoryInfo)
            {
                if (category.Name == categoryName)
                {
                    return GetTag(category, tagName);
                }
            }

            return null;
        }

        public TagData GetTag(CategoryData category, string tag)
        {
            foreach (TagData curTag in CategoryInfo[CategoryInfo.IndexOf(category)].Tags)
            {
                if (curTag.Name == tag)
                {
                    return curTag;
                }
            }

            return null;
        }

        public CategoryData GetTagParentCategory(TagData tag)
        {
            return GetCategory(tag.ParentCategoryName);
        }

        public bool ContainsTag(TagData tag)
        {
            int categoryIndex = CategoryInfo.IndexOf(GetTagParentCategory(tag));

            if (categoryIndex != -1)
            {
                foreach (TagData curTag in CategoryInfo[categoryIndex].Tags)
                {
                    if (curTag.Name == tag.Name)
                    {
                        return true;
                    }
                }

                return false;
            }

            return false;
        }

        public int TagCount()
        {
            int tagCount = 0;

            foreach (CategoryData categiory in CategoryInfo)
            {
                tagCount += categiory.Tags.Count;
            }

            return tagCount;
        }
    }
}
