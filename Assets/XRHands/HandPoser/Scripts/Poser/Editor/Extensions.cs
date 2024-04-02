// Author: Cody Tedrick https://github.com/ctedrick
// MIT License - Copyright (c) 2024 Cody Tedrick

using UnityEditor;

namespace InteractionsToolkit.Utility
{
    public static class Extensions
    {
        public static string ConvertToProjectRelativePath(this string path) => FileUtil.GetProjectRelativePath(path);


    }

}